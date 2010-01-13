// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1.  Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.
// 2.  Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
// 3.  Neither the name of CJSC NII STT ("NII STT") nor the names of
//     its contributors may be used to endorse or promote products derived
//     from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY NII STT AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL NII STT OR ITS CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using KaorCore.RPU;
using KaorCore.Trace;
using RPURPV3;

namespace RPURPV3.PowerMeter
{
	public class RPV3PowerMeter : IPowerMeter
	{
		#region ================ Поля ================

		/// <summary>
		/// Связанное РПУ
		/// </summary>
		CRPURPV3 rpu;

		/// <summary>
		/// Текущая полоса фильтра измерителя мощности
		/// </summary>
		Byte filterBand = 0;
		Int32 avgTime = 1000;
		Byte activeBand = 255;
		Int32 activeAvgTime = -1;

		private Double lastPwrValue;

		/// <summary>
		/// Список поддерживаемых полос фильтра
		/// </summary>
		List<int> supportedFilterBands;

		private bool isRunning = false;
		private bool NeedStart = true;

		/// <summary>
		/// Событие окончания измерения мощности
		/// </summary>
		//AutoResetEvent evtPowerMeasured;

		object measurePowerLock;

		#endregion

		#region ================ Конструктор ================
		public RPV3PowerMeter(CRPURPV3 pRPU)
		{
			rpu = pRPU;

			/// Инициализация списка поддерживаемых фильтров
			supportedFilterBands = new List<int> {1000, 3000, 7500, 10000, 30000, 100000, 280000, 120000};

			/// Инициализация события окончания измерения мощности
			//evtPowerMeasured = new AutoResetEvent(false);

			measurePowerLock = new object();
		}

		#endregion

		#region ================ IPowerMeter Members ================

		public int FilterBand
		{
			get
			{
				return supportedFilterBands[filterBand];
			}
			set
			{
				if (supportedFilterBands.Contains(value))
				{
					unchecked { filterBand = (Byte)supportedFilterBands.IndexOf(value); }
					//if (isRunning)
					{
						Stop();
						Start();
					}

					rpu.CallOnRPUParamsChanged();
				}
				else
					throw new ArgumentException("Invalid filter band!");
			}
		}

		public int AverageTime
		{
			get
			{
				return avgTime;
			}
			set
			{
				avgTime = value;
				//if (isRunning)
				{
					Stop();
					Start();

					rpu.CallOnRPUParamsChanged();
				}
			}
		}

		public bool ProcessPkt(Byte[] Pkt)
		{
			Int32 v;

			if (Pkt[0] == 0x90)
			{
				lock (measurePowerLock)
				{
					v = Pkt[1] + 256 * Pkt[2];
					unchecked
					{
						if ((v & 0x8000) != 0)
							v |= ((Int32)(0xFFFF0000));
					}
					lastPwrValue = (Double)v / 10;

					/// Сигнал
					Monitor.Pulse(measurePowerLock);
				}

				//if (isRunning)
				{
					if (OnNewPowerMeasure != null)
						OnNewPowerMeasure(rpu, 0, (float)lastPwrValue);
				}
#if DEBUG
				Console.WriteLine(String.Format("Power: {0}", lastPwrValue.ToString("F1")));

				/// Установка события окончания измерения мощности
				///
				Console.WriteLine("Set PW Event");
#endif
				//evtPowerMeasured.Set();

				return true;
			}
			else
				return false;
		}

		public double MeasurePower()
		{

			if (rpu.Mode == RPUMode.Off)
				return -140.0;

			Byte[] cmdPwr = { 0x21, 0, 5, 0, 3, 0, 0, 0 };

			/// Сброс события измерения мощности
#if DEBUG
			Console.WriteLine("Reset PWR Event");
#endif
			//evtPowerMeasured.Reset();



			cmdPwr[2] = (Byte)(filterBand + 1);
			unchecked
			{
				cmdPwr[3] = (Byte)(avgTime & 0xFF);
				cmdPwr[4] = (Byte)((avgTime >> 8) & 0xFF);
			}
			
			if (NeedStart || (activeBand != filterBand) || (activeAvgTime != avgTime))
			{
#if DEBUG
				Console.WriteLine(String.Format("Set: Band = {0} AverageTime = {1}", supportedFilterBands[filterBand], avgTime));
#endif
				rpu.SendCommand(cmdPwr);
				activeAvgTime = avgTime;
				activeBand = filterBand;
				NeedStart = false;
			}

			lock (measurePowerLock)
			{
				lastPwrValue = 50000;
				while (lastPwrValue == 50000)
					if (!Monitor.Wait(measurePowerLock, 2000))
						throw new InvalidOperationException("Timeout!");
			}
			/*
			if (!evtPowerMeasured.WaitOne(2000, true))
				throw new InvalidOperationException("Таймаут измерения мощности!");
		*/
			return lastPwrValue;
		}

		public double MeasurePower(long pBaseFreq)
		{
			double _power;

			if (rpu.Mode == RPUMode.Off)
				return -140.0;


			//lock (measurePowerLock)
			{
#if DEBUG
				Console.WriteLine(String.Format("Measure at {0}", pBaseFreq));
#endif
				rpu.SetFrequency(pBaseFreq);
				//rpu.BaseFreq = pBaseFreq;
				_power = MeasurePower();
			}

			return _power;
		}

		public bool IsRunning
		{
			get { return isRunning; }
		}

		public void Start()
		{
#if DEBUG
			Console.WriteLine("Measure PWR Start");
#endif

			if (rpu.Mode == RPUMode.Off)
				return;

			isRunning = true;
			MeasurePower();
		}

		public void Stop()
		{
			Byte[] cmdModeNone = { 0x20 };

#if DEBUG
			Console.WriteLine("Measure PWR Stop");
#endif

			if (rpu.Mode == RPUMode.Off)
				return;

			rpu.SendCommand(cmdModeNone);
			isRunning = false;
			NeedStart = true;
			//System.Threading.Thread.Sleep(avgTime + 100);

			//if(OnNewPowerMeasure != null)
			//	OnNewPowerMeasure(this.rpu, 0, -120);
		}

#if OLD_TRACES
		/// <summary>
		/// Диалоговое окно создания новой трассы на базе 
		/// измерителя мощности приемника РПВ-3
		/// </summary>
		/// <returns></returns>
		public BaseTrace UserCreateNewTrace(long pFstart, long pFstop)
		{
			RPV3Trace _trace = new RPV3Trace();
			_trace.Fstart = pFstart;
			_trace.Fstop = pFstop;

#if false
			NewTraceDialog _dlg = new NewTraceDialog();
			NewTraceSelectTypeDialog _typeDlg = new NewTraceSelectTypeDialog();

			if (_typeDlg.ShowDialog() != DialogResult.OK)
				return null;

			_trace = (RPV3Trace)Activator.CreateInstance(_typeDlg.SelectedType, rpu);

			if (!_trace.UserFillParamsFromDialog())
				_trace = null;
#endif
			if (!_trace.UserFillParamsFromDialog())
				_trace = null;

			return _trace;
		}
#endif
		public event NewPowerMeasure OnNewPowerMeasure;

		#endregion
	}
}
