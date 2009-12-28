using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

using KarlsTools;
using SignalAnalyzer.Core;

namespace SignalAnalyzer.Demodulators
{
	enum EGSMDemodulatorState
	{
		FB_SYNC,
		SB_SYNC,
		NORMAL,
		ERROR
	}

	public class GSMDemodulator : IDemodulator
	{
		IDemodulatorInput demInput;
		long frequency;
		GSMDemodulatorControl gsmControl;
		EGSMDemodulatorState demodulatorState;
		Queue rawDataQueue;
		AutoResetEvent evtNewRawData;
		Thread thrRawData;
		bool demodulatorRunning;


		#region Параметры GSM-демодулятора
		double freqShift;
		const int SPS = 1;
		const int NBursts = 2;
		Complex[] rawBurstData;
		int burstCenterIdx;
		int burstLength;


		/// <summary>
		/// Счетчик переходов для поиска FB
		/// </summary>
		int fbSyncCount;

		#endregion

		public GSMDemodulator(IDemodulatorInput pDemodInput)
		{
			demInput = pDemodInput;
			demInput.DataChunkSize = 196 * SPS;
			demInput.OnDataChunkReceived += new DataChunkReceivedHandler(demInput_OnDataChunkReceived);
			demInput.OnDemodulatorError += new DemodulatorErrorHandler(demInput_OnDemodulatorError);

			rawBurstData = new Complex[196 * 2 * SPS];

			gsmControl = new GSMDemodulatorControl(this);

			demodulatorRunning = true;

			rawDataQueue = new Queue();
			evtNewRawData = new AutoResetEvent(false);

			ResetDemodulator();
			
			//thrRawData.Start();
		}

		~GSMDemodulator()
		{
			Stop();
		}

		void ResetDemodulator()
		{
			freqShift = 0.0;
			demodulatorState = EGSMDemodulatorState.FB_SYNC;
			fbSyncCount = 0;

			lock (rawDataQueue)
			{
				rawDataQueue.Clear();
				evtNewRawData.Reset();
			}
		}

		#region IDemodulator Members

		public System.Windows.Forms.UserControl DemodulatorControl
		{
			get 
			{
				return gsmControl;
			}
		}

		public IDemodulatorInput DemodulatorInput
		{
			get
			{
				return demInput;
			}
			set
			{
				try
				{

				}
				catch
				{
					demInput = null;
				}
			}
		}

		void demInput_OnDemodulatorError(int pErrorCode)
		{
			
		}

		/// <summary>
		/// Обработчик прихода нового блока сырых данных
		/// Помещает сформированный массив комплексных отсчетов в очередь обработки
		/// </summary>
		/// <param name="pI"></param>
		/// <param name="pQ"></param>
		/// <param name="pOverflow"></param>
		void demInput_OnDataChunkReceived(double[] pI, double[] pQ, bool pOverflow)
		{
			Complex[] _chunk = new Complex[pI.Length];

			for (int _i = 0; _i < pI.Length; _i++)
			{
				_chunk[_i] = new Complex(pI[_i], pQ[_i]);
			}

			while (rawDataQueue.Count > 10)
				Thread.Sleep(100);

			lock (rawDataQueue)
			{
				rawDataQueue.Enqueue(_chunk);
				evtNewRawData.Set();
			}
		}

		/// <summary>
		/// Тело треда обработки сырых отсчетов данных
		/// </summary>
		void ThreadRawData()
		{
			
			int ii = 0;
			double d_clock_counter = 0.0;
			uint d_samples_counter = 0;
			double GSM_SYMBOL_PERIOD = 1.0 / (1625000.0 / 6.0);
			Complex d_last_sample = new Complex(0.0, 0.0);
			double d_sample_interval = 1.0 / (1625000.0 / 6.0);
			burstCenterIdx = rawBurstData.Length / 2;
			burstLength = rawBurstData.Length;

			FileStream _dumpFile = new FileStream("dump.txt", FileMode.Create);
			TextWriter _textW = new StreamWriter(_dumpFile, Encoding.ASCII);
			Complex[] _equalizedData = new Complex[rawBurstData.Length];

			try
			{
				while (demodulatorRunning)
				{
					bool _newData = false;

					do
					{
						lock (rawDataQueue)
						{
							_newData = rawDataQueue.Count > 0;
						}

						if (_newData == false)
							evtNewRawData.WaitOne();

					} while (_newData == false);

					Complex[] _dataChunk = null;

					lock (rawDataQueue)
					{
						_dataChunk = (Complex[])rawDataQueue.Dequeue();
					}

					if (_dataChunk == null)
						continue;

					/// Перенос данных буфера
					/// 
					Array.Copy(rawBurstData, rawBurstData.Length / (NBursts),
						rawBurstData, 0, rawBurstData.Length - rawBurstData.Length / (NBursts));
					Array.Copy(_dataChunk, 0, rawBurstData, rawBurstData.Length - rawBurstData.Length / NBursts,
						_dataChunk.Length);

					int ni = burstCenterIdx + burstLength / 2;
					if (ni >= rawBurstData.Length - MMSEInterpolator.NTAPS)
						ni -= MMSEInterpolator.NTAPS;

					for (ii = burstCenterIdx - burstLength / 2; ii < ni; ii++)
					{
						//clock symbols 
						//TODO: this is very basic and can be improved.  Need tracking...
						//TODO: use burst_start offsets as timing feedback
						//TODO: save complex samples for Viterbi EQ
						if (d_clock_counter >= GSM_SYMBOL_PERIOD)
						{
							d_clock_counter -= GSM_SYMBOL_PERIOD; //reset clock for next sample, keep the remainder

							double mu = 1.0 - d_clock_counter / GSM_SYMBOL_PERIOD;
							Complex sample = MMSEInterpolator.interpolate(rawBurstData, ii, mu);	//FIXME: this seems noisy, make sure it is being used correctly

							_equalizedData[ii] = sample;

							Complex conjprod = sample * Complex.Conj(d_last_sample);
							double diff_angle = Math.Atan2(conjprod.Imag(), conjprod.Real());
							d_last_sample = sample;
							/*
							if (ii >= 196 && ii < 196 * 2)
							{
								_textW.WriteLine(sample.Real() + " " + sample.Imag());
							}
							 */
						}

						d_clock_counter += d_sample_interval;

						if (ii >= 196 && ii < 196 * 2)
						{
							unchecked
							{
								d_samples_counter++;
							}
						}
					}

					if (d_samples_counter > 19180 - 196 && d_samples_counter < 19370 - 196)
					{
						gsmControl.DrawIQ(rawBurstData, _equalizedData);
						//gsmControl.DrawIQ(_equalizedData, true);
					}


					/// После копирования буфер выглядит следующим образом:
					/// | B0 B0 B0 B0 | B1 B1 B1 B1 | ... | BN BN BN BN |
					/// Самые новые данные находятся в хвосте буфера
					/// 

					switch (demodulatorState)
					{
						case EGSMDemodulatorState.FB_SYNC:
							if (ProcessFBSearch(_dataChunk))
							{
								demodulatorState = EGSMDemodulatorState.SB_SYNC;
							}
							else
							{

							}
							break;

						case EGSMDemodulatorState.SB_SYNC:
							break;

						case EGSMDemodulatorState.NORMAL:
							break;

						case EGSMDemodulatorState.ERROR:
							break;
					}
				}
			}

			catch
			{
			}

			finally
			{
				_dumpFile.Close();
			}
		}


		bool ProcessFBSearch(Complex[] pDataChunk)
		{
			int _i;

			for (_i = 1; _i < rawBurstData.Length; _i++)
			{
				Complex _c = rawBurstData[_i];
				Complex _cp = rawBurstData[_i - 1];
			}
			return false;
		}
		#endregion

		#region IDemodulator Members


		public void Start()
		{
			if (thrRawData != null && thrRawData.IsAlive == true)
				throw new Exception("Thread already running!");

			thrRawData = new Thread(ThreadRawData);
			thrRawData.Start();
			demInput.StartReceiveData();
		}

		public void Stop()
		{
			if (thrRawData != null && thrRawData.IsAlive == true)
			{
				demInput.StopReceiveData();
				demodulatorRunning = false;
				thrRawData.Join();

			}
		}

		#endregion
	}
}
