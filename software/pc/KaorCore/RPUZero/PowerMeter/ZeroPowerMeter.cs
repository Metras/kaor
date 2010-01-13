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

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.Trace;

namespace KaorCore.RPUZero.PowerMeter
{
	public class ZeroPowerMeter : IPowerMeter
	{
		#region ================ Поля ================
		
		List<int> supportedFilterBands;
		int filterBand;
		int averageTime;

		Random powerRandom;
		CRPUZero rpu;

		bool isRunning;
		Thread powerThread;
		bool powerThreadRunning;

		object lockObject = new object();

		#endregion

		#region ================ Конструктор ================
		public ZeroPowerMeter(CRPUZero pRPU)
		{
			rpu = pRPU;
			supportedFilterBands = new List<int> { 1000, 2200, 7500, 10000, 235000 };
			averageTime = 10;
			powerRandom = new Random();
			isRunning = false;
			powerThreadRunning = false;
		}
		#endregion

		#region IPowerMeter Members

		public int FilterBand
		{
			get
			{
				return filterBand;
			}
			set
			{
				if (supportedFilterBands.Contains(value))
				{
					filterBand = value;
					Thread.Sleep(200);
				}
				
			}
		}

		public int AverageTime
		{
			get
			{
				return averageTime;
			}
			set
			{
				Thread.Sleep(100);
				averageTime = value;
			}
		}

		public double MeasurePower()
		{
			//Thread.Sleep(2);
			return powerRandom.NextDouble() * 20.0 - 100.0;
		}

		public double MeasurePower(long pBaseFreq)
		{
			rpu.BaseFreq = pBaseFreq;
			return MeasurePower();
		}

		public bool IsRunning
		{
			get { return isRunning; }
		}

		/// <summary>
		/// Запуск треда моделирования измерения мощности
		/// </summary>
		public void Start()
		{
			powerThread = new Thread(PowerThreadMain);
			
			lock (lockObject)
			{
				powerThreadRunning = true;
			}

			powerThread.Start();
		}

		public void Stop()
		{
			lock (lockObject)
				powerThreadRunning = true;

			if(powerThread != null)
				powerThread.Join();
		}
#if false
		public BaseTrace UserCreateNewTrace(long pFstart, long pFstop)
		{
			BaseTrace _trace = null;

			NewTraceDialog _dlg = new NewTraceDialog();
			_dlg.Antennas = rpu.Antennas;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				/// Шаг перестройки РАВЕН полосе фильтра измерителя мощности
				/// 
				_trace = new ZeroTrace(_dlg.FStart, _dlg.FStop, 
					_dlg.FilterBand, _dlg.FilterBand, _dlg.AverageTime, 
					rpu, _dlg.SelectedAntenna);
				_trace.Name = _dlg.txtTraceName.Text;
				_trace.Description = _dlg.txtTraceDescr.Text;
				_trace.LineItem.Color = _dlg.cmbTraceColor.Value;
			}

			return _trace;
		}

		public BaseTrace UserQuickCreateNewTrace(long pFstart, long pFstop)
		{
			throw new NotImplementedException();
		}
#endif
		public event NewPowerMeasure OnNewPowerMeasure;

		#endregion

		#region ================ Тред моделирования мощности ================
		
		void PowerThreadMain(object pParam)
		{
			bool _running;
			int _tmp;
			double _power;

			do
			{
				lock (lockObject)
				{
					_tmp = averageTime;
				}
				
				Thread.Sleep(_tmp);

				lock (powerRandom)
					_power = powerRandom.NextDouble();

				if (OnNewPowerMeasure != null)
					OnNewPowerMeasure(rpu, rpu.BaseFreq, (float)_power);

				lock (lockObject)
					_running = powerThreadRunning;
			} while (_running);
		}

		#endregion
	}
}
