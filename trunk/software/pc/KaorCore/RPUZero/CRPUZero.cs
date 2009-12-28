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
// THIS SOFTWARE IS PROVIDED BY APPLE AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL APPLE OR ITS CONTRIBUTORS BE LIABLE FOR
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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.AntennaManager;
using KaorCore.RPU;
using KaorCore.Task;
using KaorCore.Trace;

using KaorCore.RPUZero.PowerMeter;

namespace KaorCore.RPUZero
{
	public class CRPUZero : BaseRPU
	{
		#region ============== Поля ==============
		
		IAntennaManager managerRF1;
		IAntennaManager managerRF2;

		List<IAntenna> antennas;
		Int64 baseFreq;

		ZeroPowerMeter powerMeter;

		RPUZeroControl rpuControl;

		Guid rpuType = new Guid("{80DC1EA5-4ADA-4fef-8C37-53AE8577AB04}");
		#endregion

		#region ============== Конструктор ==============
		public CRPUZero() : 
			base()
		{
			isDisabled = false;

			name = "Эмулятор приемника";
			powerMeter = new ZeroPowerMeter(this);
			hasDemodulator = true;
			hasPowerMeter = true;
			hasSpectrograph = false;

			antennas = new List<IAntenna>();
			managerRF1 = new AntennaManagerCrab8x1();
			((AntennaManagerCrab8x1)managerRF1).AntennasArray[0] = new BaseAntenna();

			antennas.Add(new BaseAntenna());
		}
		#endregion

		public override List<IAntenna> Antennas
		{
			get
			{
				if (antennas == null)
				{
					antennas = new List<IAntenna>();

					if (managerRF1 != null)
						antennas.AddRange(managerRF1.Antennas);

					if (managerRF2 != null)
						antennas.AddRange(managerRF2.Antennas);
				}

				//return antennas;
				return managerRF1.Antennas;
			}
		}

		#region IRPU Members

		public override System.Windows.Forms.Form SettingsForm
		{
			get { throw new NotImplementedException(); }
		}

		public override System.Windows.Forms.UserControl RPUControl
		{
			get 
			{
				if (rpuControl == null)
				{
					rpuControl = new RPUZeroControl(this);
				}

				return rpuControl;
			}
		}

		public override void SwitchOn()
		{
			//Thread.Sleep(1000);
			Mode = RPUMode.Free;
			try
			{
				managerRF1.SwitchOn();
			}

			catch
			{
				foreach (IAntenna _a in managerRF1.Antennas)
					_a.State = EAntennaState.FAULT;
			}
		}

		public override void SwitchOff()
		{
			Thread.Sleep(1000);
			Mode = RPUMode.Off;
		}

#if false
		public IRPUAssignable AssignetTo
		{
			get { throw new NotImplementedException(); }
		}

		public void Assign(IRPUAssignable pAssigned)
		{
			throw new NotImplementedException();
		}

		public void Release(IRPUAssignable pAssigned)
		{
			throw new NotImplementedException();
		}
#endif

		public override IAudioDemodulator Demodulator
		{
			get 
			{
				/*if (!hasDemodulator)
					return null;
				*/
				return null;
			}
		}

		public override IPowerMeter PowerMeter
		{
			get 
			{
				/*if (!hasPowerMeter)
					return null;
				*/
				return powerMeter;
			}
		}

		public override ISpectrograph Spectrograph
		{
			get 
			{
				/*if (!hasSpectrograph)
					return null;
				*/
				return null;
			}
		}

		public override System.Windows.Forms.UserControl statusControl
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Установка базовой частоты
		/// </summary>
		public override long BaseFreq
		{
			get
			{
				return baseFreq;
			}
			set
			{
				if (value >= freqMin && value <= freqMax)
				{
					Thread.Sleep(20);
					baseFreq = value;

					if (OnBaseFrequencyChanged != null)
						OnBaseFrequencyChanged(baseFreq);
				}
			}
		}

		public override BaseRPUParams Parameters
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void LoadFromXmlNode(System.Xml.XmlNode pNode)
		{
			if (pNode == null || pNode.Name != "RPU" ||
				pNode.Attributes["type"].Value != this.GetType().AssemblyQualifiedName)
				throw new ArgumentException("Некорректное значение типа нода или типа РПУ");

			id = new Guid(pNode.Attributes["id"].Value);

			if (pNode.Attributes["freqMin"] != null)
				if (!Int64.TryParse(pNode.Attributes["freqMin"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out freqMin))
					throw new InvalidOperationException("Error parsing freqmin!");
			
			if (pNode.Attributes["freqMax"] != null)
				if (!Int64.TryParse(pNode.Attributes["freqMax"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out freqMax))
					throw new InvalidOperationException("Error parsing freqmax!");

			if (pNode.Attributes["hasDemodulator"] != null)
				if (!bool.TryParse(pNode.Attributes["hasDemodulator"].Value, out hasDemodulator))
					throw new InvalidOperationException("Error parsing hasDemodulator!");

			if (pNode.Attributes["hasSpectrograph"] != null)
				if (!bool.TryParse(pNode.Attributes["hasSpectrograph"].Value, out hasSpectrograph))
					throw new InvalidOperationException("Error parsing hasSpectrograph!");

			if (pNode.Attributes["hasPowerMeter"] != null)
				if (!bool.TryParse(pNode.Attributes["hasPowerMeter"].Value, out hasPowerMeter))
					throw new InvalidOperationException("Error parsing hasPowerMeter!");

			/// Антенный вход №1 РПУ
			/// 
			try
			{
				managerRF1 = LoadAntennaManager(pNode, "RFInput1");
			}
			catch
			{
				/// RFInput1 не заполнен
			}

			/// Антенный вход №2 РПУ
			/// 
			try
			{
				managerRF2 = LoadAntennaManager(pNode, "RFInput2");
			}
			catch
			{
				/// RFInput2 не заполнен
			}

		}

		IAntennaManager LoadAntennaManager(XmlNode pNode, string pAntennaManagerName)
		{
			XmlNode _node;

			_node = pNode.SelectSingleNode(pAntennaManagerName);

			if (_node == null)
				throw new Exception();

			if (_node.Attributes["disabled"] != null)
				throw new Exception();

			XmlNode _managerNode = _node.SelectSingleNode("AntennaManager");

			if (_managerNode == null)
				throw new Exception();

			if (_managerNode.Attributes["assembly"] == null || _managerNode.Attributes["type"] == null)
				throw new Exception();

			string _assemblyName = _managerNode.Attributes["assembly"].Value;
			Assembly _assembly = Assembly.Load(_assemblyName);
			string _typeName = _managerNode.Attributes["type"].Value;

			Type _t = _assembly.GetType(_typeName);

			object _o = Activator.CreateInstance(_t);

			IAntennaManager _antennaManager = _o as IAntennaManager;

			if (_antennaManager == null)
				throw new Exception();

			_antennaManager.LoadFromXmlNode(_managerNode);

			return _antennaManager;
		}

		#endregion

		/// <summary>
		/// Переключение на указанную антенну
		/// </summary>
		/// <param name="pAntenna"></param>
		/// <returns></returns>
		public override bool SwitchAntenna(IAntenna pAntenna)
		{
			if (pAntenna == null)
				return false;

			if (managerRF1 != null)
			{
				if (managerRF1.Antennas.Contains(pAntenna))
				{
					/// Выбор первого антенного входа РПУ
					SelectRFInput(1);

					managerRF1.SelectAntenna(pAntenna);
					return true;
				}
			}

			if (managerRF2 != null)
			{
				if (managerRF2.Antennas.Contains(pAntenna))
				{
					/// Выбор второго антенного входа РПУ
					SelectRFInput(2);

					managerRF2.SelectAntenna(pAntenna);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Выбор антенного входа РПУ
		/// </summary>
		/// <param name="pInput"></param>
		/// <returns></returns>
		private bool SelectRFInput(int pInput)
		{
			return false;
		}
#if false
		#region ================ Процессор запросов ================

		Thread taskProcessorThread;
		TraceTaskProvider traceTaskProvider;
		
		ManualResetEvent evtTaskThreadStop;
		ManualResetEvent evtTaskThreadStopped;

		/// <summary>
		/// Запуск процессора запросов
		/// </summary>
		/// <returns></returns>
		public bool StartTaskProcessor(ITaskProvider pTaskProvider)
		{

			taskProcessorThread = new Thread(TaskProcessorMain);

			taskProcessorThread.Start(pTaskProvider);

			if(evtTaskThreadStop == null)
				evtTaskThreadStop = new ManualResetEvent(false);
			if(evtTaskThreadStopped == null)
				evtTaskThreadStopped = new ManualResetEvent(false);

			evtTaskThreadStop.Reset();
			evtTaskThreadStopped.Reset();

			return true;
		}


		public bool StopTaskProcessor(ITaskProvider pProvider)
		{
			if(taskProcessorThread != null && taskProcessorThread.IsAlive)
			{
				evtTaskThreadStop.Set();

				while (taskProcessorThread.IsAlive)
				{
					if (evtTaskThreadStopped.WaitOne(50, true))
						break;

					Application.DoEvents();
				}
			}

			return true;
		}

		public bool InterruptTaskProcessor(ITaskProvider pProvider)
		{
			StopTaskProcessor(pProvider);
			return true;
		}

		public bool IsTaskProcessorRunning
		{
			get { return taskProcessorThread.IsAlive; }
		}

		void TaskProcessorMain(object pParam)
		{
			ITaskProvider _provider = pParam as ITaskProvider;

			if (_provider == null)
			{
				evtTaskThreadStopped.Set();
				return;
			}

			while (!evtTaskThreadStop.WaitOne(0, true))
			{
				BaseTask _task = _provider.NextTask();

				if (_task == null)
					break;

				if ((_task as TaskMeasurePower) != null)
				{
					TaskMeasurePower _measureTask = _task as TaskMeasurePower;

					double _power = powerMeter.MeasurePower(_measureTask.Frequency);

					_provider.TaskComplete(_task, new TaskMeasureResponse(_measureTask, _power));
				}

			}

			evtTaskThreadStopped.Set();
		}

	/*	public ITaskProvider ScanTraceTaskProvider(BaseTrace pTrace)
		{
			if (pTrace as ZeroTrace == null)
				throw new ArgumentException("Неправильная трасса!");

			traceTaskProvider = new TraceTaskProvider(pTrace as ZeroTrace);

			return traceTaskProvider;
		}
		*/
		#endregion
#endif

		public override string ToString()
		{
			return name;
		}
		public override event BaseFrequencyChanged OnBaseFrequencyChanged;

		#region IRPU Members


		public override bool IsAvailable
		{
			get
			{
				return true;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion


		#region ================ Процессор запросов ================
		int init = 0;
		protected override double MeasurePowerTask(TaskMeasurePower pTask)
		{
			double _power;
			/// Установка параметров измерителя мощности из параметров задачи на измерение
			/// 
			if (init == 0)
			{
				powerMeter.MeasurePower();

				init++;
			}

			_power = powerMeter.MeasurePower(pTask.Frequency);
			return _power;
		}

		/*	public ITaskProvider ScanTraceTaskProvider(BaseTrace pTrace)
			{
				if (pTrace as ZeroTrace == null)
					throw new ArgumentException("Неправильная трасса!");

				traceTaskProvider = new TraceTaskProvider(pTrace as ZeroTrace);

				return traceTaskProvider;
			}
			*/


		#endregion

		#region IRPU Members
#if BASE

		public override bool PauseTaskProcessors()
		{
			foreach (TaskProcessorItem _item in taskProcessors)
				_item.EvtTaskThreadPause.Set();

			return true;
		}

		public override bool ResumeTaskProcessors()
		{
			foreach (TaskProcessorItem _item in taskProcessors)
				_item.EvtTaskThreadPause.Reset();

			return true;
		}
#endif
		#endregion

		#region IRPU Members


		public override object ShowRecordParamsDialog(object pRecordParams)
		{
			MessageBox.Show("Установка параметров записи сигнала",
				"Информация",
				MessageBoxButtons.OK, MessageBoxIcon.Information);

			return null;
		}


		public override object DefaultSignalRecordParams
		{
			get 
			{
				return new object();
			}
		}


		public override void SetParamsFromSignal(KaorCore.Signal.BaseSignal signalChart)
		{
		}

		public override void StartRecordSignal(KaorCore.Signal.BaseSignal pSignal, object pRecordParams, NewRecordInfoDelegate pNewRecordInfo, BaseTrace pScanTrace)
		{
			
		}

		public override string StopRecordSignal(object pRecordParams)
		{
			return "";
		}

		#endregion

		#region IRPU Members


		public override void ShowStartSplash()
		{
			
		}

		#endregion

		#region IDisposable Members

		public override void Dispose()
		{
		}

		#endregion

		public override bool SetupScanParams(BaseTrace pTrace)
		{
			MessageBox.Show("Установка параметров сканирования");

			TraceScanParams _scanParams = new TraceScanParams(this);
			
			_scanParams.Antenna = antennas[0];
			pTrace.ScanParams = _scanParams;
			pTrace.ScanParams.FilterBand = 100000;
			pTrace.ScanParams.AverageTime = 20;

			return true;
		}

		public override void SaveToXmlWriter(XmlWriter pWriter)
		{
			
		}

		public override Guid RPUType
		{
			get 
			{
				return rpuType;
			}
		}

		public override void CheckConfiguration()
		{
			
		}
	}
}
