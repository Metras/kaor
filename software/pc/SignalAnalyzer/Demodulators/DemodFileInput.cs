using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using SignalAnalyzer.Core;

namespace SignalAnalyzer.Demodulators
{
	public class DemodFileInput : IDemodulatorInput
	{
		FileStream file;
		int chunkSize;
		int sampleRate;
		bool autoRewind;
		bool readThreadRunning;
		Thread thrReadFile;
		string fileName;

		public DemodFileInput(bool pAutoRewind)
		{
			
			sampleRate = 0;
			autoRewind = pAutoRewind;

			
		}

		public string FileName
		{
			get { return fileName; }
			set
			{
				fileName = value;
			}
		}
		#region IDemodulatorInput Members

		public int DataChunkSize
		{
			get
			{
				return chunkSize;
			}
			set
			{
				chunkSize = value;
			}
		}

		public event DataChunkReceivedHandler OnDataChunkReceived;

		public event DemodulatorErrorHandler OnDemodulatorError;

		public long BaseFreq
		{
			get
			{
				return 0;
			}
			set
			{
				
			}
		}

		public long FreqMin
		{
			get 
			{
				return 0;
			}
		}

		public long FreqMax
		{
			get 
			{
				return long.MaxValue;
			}
		}

		public int SampleRate
		{
			get
			{
				return sampleRate;
			}
			set
			{
				sampleRate = value;
			}
		}

		public void StartReceiveData()
		{
			thrReadFile = new Thread(ReadThreadMain);
			readThreadRunning = true;
			thrReadFile.Start();
		}

		public void StopReceiveData()
		{
			readThreadRunning = false;
			thrReadFile.Join();
		}

		void ReadThreadMain()
		{
			byte[] _dataIQ = new byte[chunkSize * 2 * 2];
			short[] _IQ = new short[chunkSize * 2];
			double[] _I = new double[chunkSize];
			double[] _Q = new double[chunkSize];
			long _pos = 0;
			int _slotCounter = 0;

			IntPtr _copyBuf = Marshal.AllocHGlobal(_dataIQ.Length);
			file = new FileStream(fileName, FileMode.Open);
			try
			{

				while (readThreadRunning)
				{

					if (file.Length - file.Position < chunkSize * 2 * 2)
						file.Seek(0, SeekOrigin.Begin);

					file.Read(_dataIQ, 0, _dataIQ.Length);
					Marshal.Copy(_dataIQ, 0, _copyBuf, _dataIQ.Length);
					Marshal.Copy(_copyBuf, _IQ, 0, _IQ.Length);

					for (int _i = 0; _i < _I.Length; _i++)
					{
						_I[_i] = (double)_IQ[_i * 2] / (double)short.MaxValue;
						_Q[_i] = (double)_IQ[_i * 2 + 1] / (double)short.MaxValue;
					}

					if (OnDataChunkReceived != null)
						OnDataChunkReceived(_I, _Q, false);
				}
			}

			catch
			{
			}

			finally
			{
				file.Close();
				Marshal.FreeHGlobal(_copyBuf);
			}
		}

		public bool IsRunning
		{
			get 
			{ 
				return (thrReadFile != null && thrReadFile.IsAlive);
			}
		}
		#endregion
	}
}
