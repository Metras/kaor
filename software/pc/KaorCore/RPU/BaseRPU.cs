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
using System.Xml;

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.Trace;
using KaorCore.Task;

namespace KaorCore.RPU
{
	public enum RPUMode
	{
		Scan,
		ZeroSpan,
		AudioRecord,
		Free,
		Off
	}

	public abstract class BaseRPU : IRPU
	{
		

		public RPUMode Mode
		{
			get { return mode; }
			set
			{
				if (value != mode)
				{
					mode = value;
					CallOnModeChanged(mode);
				}
			}
		}

		public delegate void ModeChangedDelegate(RPUMode pMode);

		public event ModeChangedDelegate OnModeChanged;

		public void CallOnModeChanged(RPUMode pMode)
		{
			if (OnModeChanged != null)
				OnModeChanged(pMode);
		}

		protected bool isBusy;
		/// <summary>
		/// Доступность приемника
		/// </summary>
		protected bool isAvailable;

		protected object cmdLock;

		protected AutoResetEvent evtStopRecordSignal;
		private RPUMode mode;

		protected Guid id;
		protected string name;
		protected string description;
		protected string serial;
		protected long freqMin;
		protected long freqMax;
		protected bool hasPowerMeter;
		protected bool hasDemodulator;
		protected bool hasSpectrograph;

		protected bool isDisabled;

		public BaseRPU()
		{
			id = Guid.NewGuid();

			isBusy = false;

			/// Изначально считаем приемник доступным
			isAvailable = true;

			cmdLock = new object();
			taskProcessors = new List<TaskProcessorItem>();
			evtStopRecordSignal = new AutoResetEvent(false);
			mode = RPUMode.Off;
			freqMin = 20000000;
			freqMax = 3000000000;

			hasPowerMeter = false;
			hasDemodulator = false;
			hasSpectrograph = false;

			isDisabled = true;
		}

		#region IRPU Members

		public string Name
		{
			get { return name; }
		}

		public string Serial
		{
			get { return serial; } 
		}

		public string Description
		{
			get { return description; }
		}

		public Guid Id
		{
			get { return id; }
		}

		public abstract System.Windows.Forms.Form SettingsForm
		{
			get;
		}

		public abstract System.Windows.Forms.UserControl RPUControl
		{
			get;
		}

		public abstract void SwitchOn();

		public abstract void SwitchOff();


		public virtual bool IsBusy
		{
			get
			{
				return isBusy;
			}

			set
			{
				if (value == true && isBusy == true)
					throw new InvalidOperationException("Receiver already in use!");

				isBusy = value;
			}
		}

		public long FreqMin
		{
			get { return freqMin; }
		}

		public long FreqMax
		{
			get { return freqMax; }
		}

		public bool HasDemodulator
		{
			get { return hasDemodulator; }
		}

		public bool HasSpectrograph
		{
			get { return hasSpectrograph; }
		}

		public bool HasPowerMeter
		{
			get { return hasPowerMeter; }
		}

		public abstract IAudioDemodulator Demodulator
		{
			get;
		}

		public abstract IPowerMeter PowerMeter
		{
			get;
		}

		public abstract ISpectrograph Spectrograph
		{
			get;
		}

		public abstract bool SwitchAntenna(IAntenna pAntenna);

		public abstract System.Windows.Forms.UserControl statusControl
		{
			get;
		}

		public abstract long BaseFreq
		{
			get;
			
			set;
		}

		public abstract BaseRPUParams Parameters
		{
			get;
			
			set;
			
		}

		public abstract List<KaorCore.Antenna.IAntenna> Antennas
		{
			get;
		}

		public abstract void LoadFromXmlNode(System.Xml.XmlNode pNode);
		

		#region ================ Процессор запросов ================

		class TaskProcessorItem
		{
			public Thread taskProcessorThread;
			public ManualResetEvent evtTaskThreadStop;
			public ManualResetEvent evtTaskThreadStopped;
			public ManualResetEvent evtTaskThreadPause;

			public ITaskProvider taskProvider;

			public TaskProcessorItem(ParameterizedThreadStart pStartProc, ITaskProvider pTaskProvider)
			{
				evtTaskThreadStop = new ManualResetEvent(false);
				evtTaskThreadStopped = new ManualResetEvent(false);
				evtTaskThreadPause = new ManualResetEvent(false);
				taskProcessorThread = new Thread(pStartProc);
				taskProvider = pTaskProvider;
			}

			public Thread TaskProcessorThread
			{
				get { return taskProcessorThread; }
			}

			public ManualResetEvent EvtTaskThreadStop
			{
				get { return evtTaskThreadStop; }
			}

			public ManualResetEvent EvtTaskThreadStopped
			{
				get { return evtTaskThreadStopped; }
			}

			public ManualResetEvent EvtTaskThreadPause
			{
				get { return evtTaskThreadPause; }
			}

			public ITaskProvider TaskProvider
			{
				get { return taskProvider; }
			}
		}

		//TraceTaskProvider traceTaskProvider;

		List<TaskProcessorItem> taskProcessors;
		object rpuLock = new object();
		int init;


		/// <summary>
		/// Запуск процессора запросов
		/// </summary>
		/// <returns></returns>
		public bool StartTaskProcessor(ITaskProvider pTaskProvider)
		{
			if (pTaskProvider == null)
				return false;

			if (Mode != RPUMode.Free && Mode != RPUMode.Scan)
				return false;

			/// Установка режима сканирования
			Mode = RPUMode.Scan;

			TaskProcessorItem _taskItem = new TaskProcessorItem(TaskProcessorMain, pTaskProvider);

			if (taskProcessors.Count == 0)
				init = 0;

			/// Добавление треда в список процессоров
			taskProcessors.Add(_taskItem);

			/// Запуск треда процессора запросов
			_taskItem.taskProcessorThread.Start(_taskItem);

			return true;
		}


		public bool StopTaskProcessor(ITaskProvider pTaskProvider)
		{

			if (Mode == RPUMode.Off)
				return false;

			List<TaskProcessorItem> _removeItems = new List<TaskProcessorItem>();

			evtStopRecordSignal.Set();

			var q = from _taskItem in taskProcessors
					where _taskItem.TaskProvider == pTaskProvider
					select _taskItem;

			foreach (TaskProcessorItem _item in q)
			{
				if (_item.TaskProcessorThread != null && _item.TaskProcessorThread.IsAlive)
				{
					_item.evtTaskThreadStop.Set();

					while (_item.taskProcessorThread.IsAlive)
					{
						if (_item.evtTaskThreadStopped.WaitOne(50, true))
							break;

						Application.DoEvents();
					}
				}

				/// Формирование списка процессоров на удаление из общего списка процессоров
				_removeItems.Add(_item);
			}

			/// Удаление ненужных процессоров запросов из общего списка процессоров
			foreach (TaskProcessorItem _item in _removeItems)
			{
				taskProcessors.Remove(_item);
			}

			if (taskProcessors.Count == 0)
			{
				//powerMeter.Stop();
				init = 0;

				/// Освобождение приемника
				Mode = RPUMode.Free;
			}

			return true;
		}

		public bool InterruptTaskProcessor(ITaskProvider pTaskProvider)
		{
			StopTaskProcessor(pTaskProvider);
			return true;
		}

		/// <summary>
		/// Пауза процессоров запросов
		/// </summary>
		/// <returns></returns>
		public bool PauseTaskProcessors()
		{
			foreach (TaskProcessorItem _item in taskProcessors)
			{
				_item.evtTaskThreadPause.Set();
			}

			/// Освобождение приемника
			Mode = RPUMode.Free;
			return true;
		}

		/// <summary>
		/// Продолжение процессоров запросов
		/// </summary>
		/// <returns></returns>
		public bool ResumeTaskProcessors()
		{
			/// Установка режима сканирования
			Mode = RPUMode.Scan;

			foreach (TaskProcessorItem _item in taskProcessors)
			{
				_item.evtTaskThreadPause.Reset();
			}

			return true;
		}

		public bool IsTaskProcessorRunning
		{
			get 
			{
				var q = from _tp in taskProcessors
						where _tp.TaskProcessorThread.IsAlive
						select _tp;

				return (q.Count() > 0);
			}
		}

		protected abstract double MeasurePowerTask(TaskMeasurePower pTask);

		/// <summary>
		/// Основной цикл процессора запросов от провайдеров запросов
		/// </summary>
		/// <param name="pParam"></param>
		void TaskProcessorMain(object pParam)
		{
			TaskProcessorItem _taskItem = pParam as TaskProcessorItem;

			ITaskProvider _provider = _taskItem.taskProvider;

			if (_provider == null)
			{
				_taskItem.evtTaskThreadStopped.Set();
				return;
			}

			while (!_taskItem.evtTaskThreadStop.WaitOne(0, true))
			{
				lock (rpuLock)
				{

					BaseTask _task = _provider.NextTask();

					if (_task == null)
						break;

					if ((_task as TaskMeasurePower) != null)
					{


						TaskMeasurePower _measureTask = _task as TaskMeasurePower;
						double _power;

						_power = MeasurePowerTask(_measureTask);

#if false
					lock (rpuLock)
					{
						TaskProcessorMainBody(_measureTask);


						/// Переключение антенны
						if (currentAntenna != _measureTask.ScanParams.Antenna)
							SwitchAntenna(_measureTask.ScanParams.Antenna);

						/// Установка параметров измерителя мощности из параметров задачи на измерение
						/// 
						if (init == 0 || powerMeter.FilterBand != _measureTask.ScanParams.FilterBand ||
							powerMeter.AverageTime != _measureTask.ScanParams.AverageTime)
						{
							powerMeter.FilterBand = _measureTask.ScanParams.FilterBand;
							powerMeter.AverageTime = _measureTask.ScanParams.AverageTime;
							powerMeter.MeasurePower();

							init++;
						}

						_power = powerMeter.MeasurePower(_measureTask.Frequency);

					}
#endif
						_provider.TaskComplete(_task, new TaskMeasureResponse(_measureTask, _power));
					}
				}

				/// Ждем здесь окончания паузы
				if (_taskItem.evtTaskThreadPause.WaitOne(0, true))
				{
					while (_taskItem.evtTaskThreadPause.WaitOne(100, true) &&
						!_taskItem.evtTaskThreadStop.WaitOne(0, true))
						;
				}
			}

			//powerMeter.Stop();
			_taskItem.evtTaskThreadStopped.Set();
		}

		#endregion

		public virtual bool IsAvailable
		{
			get
			{
				return isAvailable;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public virtual bool IsDisabled
		{
			get
			{
				return isDisabled;
			}

			set
			{
				isDisabled = value;
			}
		}
		public abstract object ShowRecordParamsDialog(object pRecordParams);
		public abstract void StartRecordSignal(KaorCore.Signal.BaseSignal pSignal, object pRecordParams, NewRecordInfoDelegate pNewRecordInfo, KaorCore.Trace.BaseTrace pScanTrace);

		public abstract string StopRecordSignal(object pRecordParams);

		public abstract object DefaultSignalRecordParams
		{
			get;
		}

		public abstract bool SetupScanParams(BaseTrace pTrace);
		
		public abstract void SetParamsFromSignal(KaorCore.Signal.BaseSignal signalChart);
		public abstract void ShowStartSplash();


		public virtual event BaseFrequencyChanged OnBaseFrequencyChanged;
		#endregion

		#region IDisposable Members

		public virtual void Dispose()
		{
		}

		#endregion

		public override string ToString()
		{
			return name;
		}

		public abstract void SaveToXmlWriter(XmlWriter pWriter);

		#region IRPU Members


		public event RPUParamsChangedDelegate OnRPUParamsChanged;

		protected void CallOnRPUParamsChanged()
		{
			if (OnRPUParamsChanged != null)
				OnRPUParamsChanged(this);
		}

		#endregion

		#region IRPU Members


		public abstract Guid RPUType { get; }

		#endregion

		#region IRPU Members


		public abstract void CheckConfiguration();

		#endregion
	}
}
