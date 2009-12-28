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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


using ControlUtils.Splash;
using KaorCore.I18N;
using KaorCore.RPU;
using KaorCore.RPUNull;
using KaorCore.Interfaces;

namespace KaorCore.RPUManager
{
	/// <summary>
	/// Делегат обработки ответа задания
	/// </summary>
	/// <param name="pResponse"></param>
	public delegate void TaskProcessCallback(RPUTaskResponse pResponse);

	public class TaskRequestItem
	{
		public RPUTaskRequest request;
		public TaskProcessCallback callBack;

		public TaskRequestItem(RPUTaskRequest pRequest, TaskProcessCallback pCallback)
		{
			request = pRequest;
			callBack = pCallback;
		}
	}

    /// <summary>
    /// Менеджер радиоприемных устройств
    /// Отвечает за назначение элементов задания РПУ, сопоставление РПУ пользователю, и т.п.
    /// 
    /// Работа с РПУ ведется через интерфейс IRPU
    /// В перспективе возможно подключение удаленных РПУ
    /// </summary>
    public class BaseRPUManager : IRPUManager
    {
        #region Private fields
        /// <summary>
        /// Список доступных РПУ
        /// </summary>
        ObservableCollection<IRPU> rpuDevices;
#if false
		/// <summary>
		/// Очередь запросов на обработку
		/// </summary>
		Queue<TaskRequestItem> taskRequestsQueue;

		/// <summary>
		/// Тред обработчика заданий
		/// </summary>
		Thread taskProcessorThread;

		/// <summary>
		/// Признак запущенного процессора
		/// </summary>
		bool isTaskProcessorRunning;

		AutoResetEvent evtTaskProcessed;
		AutoResetEvent evtTaskNew;
		AutoResetEvent evtFreeRPU;
#endif
		/// <summary>
		/// РПУ ручного управления
		/// </summary>
		IRPU manualRPU;

		/// <summary>
		/// Объекто блокировки смены ручного РПУ
		/// </summary>
		object manualRPUChangeLock;

		/// <summary>
		/// Нулевой РПУ
		/// </summary>
		CRPUNull rpuNull;
        #endregion

		#region ================ Constructor ================
		/// <summary>
        /// Конструктор менежера РПУ
        /// Создается пустой список РПУ
        /// </summary>
        public BaseRPUManager()
        {
            rpuDevices = new ObservableCollection<IRPU>();
			rpuDevices.CollectionChanged += new NotifyCollectionChangedEventHandler(rpuDevices_CollectionChanged);
			manualRPUChangeLock = new object();

			rpuNull = new CRPUNull();

#if DEBUG
			IRPU _rpuZero = new RPUZero.CRPUZero();
			RegisterRPU(_rpuZero);
#endif
			//rpuDevices.Add(rpuNull);
			manualRPU = rpuNull;
        }

		/// <summary>
		/// Обработчик события изменения списка устройств
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void rpuDevices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (OnRPUAdded != null)
				{
					foreach (IRPU _rpu in e.NewItems)
					{
						OnRPUAdded(_rpu);
					}
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				if (OnRPUDeleted != null)
				{
					foreach (IRPU _rpu in e.OldItems)
					{
						OnRPUDeleted(_rpu);
					}
				}
			}
			else
			{
			}
		}

		
        /// <summary>
        /// Конструктор менеджера РПУ из файла параметров (XML)
        /// </summary>
        /// <param name="pXMLParamsFile"></param>
        public void LoadFromXmlNode(XmlNode pNode)
        {

			XmlNode _node;

			_node = pNode.SelectSingleNode("RPUDevices");

			if (_node != null)
			{
				/// Создание списка подключенных РПУ из XML
				foreach (XmlNode _rpuNode in _node.SelectNodes("RPU"))
				{
					string _rpuAssembly = _rpuNode.Attributes["assembly"].Value;
					string _rpuType = _rpuNode.Attributes["type"].Value;
					bool _isManual = false;
					IRPU _rpu;

					/// Если РПУ отключено, то идем дальше
					//if (_rpuNode.Attributes["disabled"] != null)
					//	continue;

					if (_rpuNode.Attributes["manual"] != null)
						if (!bool.TryParse(_rpuNode.Attributes["manual"].Value, out _isManual))
							_isManual = false;
					try
					{
						//Assembly _ass = Assembly.Load(_rpuAssembly);
						//Type _t = _ass.GetType(_rpuType);
						//if (_rpuAssembly == "" && _rpuType == "")
						//	continue;

						Type _t = Type.GetType(_rpuType);

						object _o = Activator.CreateInstance(_t);
						 _rpu = (IRPU)_o;

						if (_rpu == null)
							continue;
					}
					catch
					{
						continue;
					}

					_rpu.LoadFromXmlNode(_rpuNode);

					Splash.Status = String.Format(Locale.status_loading_rpu_params, _rpu.Name);

					/// Регистрация РПУ в системе
					RegisterRPU(_rpu);

					/// Присвоение объекта РПУ ручного управления
					if (_isManual)
						manualRPU = _rpu;
				}

				/// РПУ ручного управления не выбрано
				if (manualRPU == null)
				{

				}
			}
        }

        #endregion

		#region ================ Properties ================
		public ObservableCollection<IRPU> RPUDevices
		{
			get
			{
				return rpuDevices;
			}
		}

		public List<IRPU> AvailableRPUDevices
		{
			get
			{
				var q = from rpu in rpuDevices
						where rpu.IsDisabled == false && rpu.IsAvailable == true
						select rpu;

				List<IRPU> _rpus = new List<IRPU>();
				_rpus.Add(rpuNull);
				_rpus.AddRange(q);

				return _rpus;
			}
		}

		/// <summary>
		/// РПУ для ручного управления
		/// </summary>
		/// 
		
		public IRPU ManualRPU
		{
			get
			{
				lock (manualRPUChangeLock)
				{
					if (manualRPU.IsAvailable)
						return manualRPU;
					else
						return rpuNull;
				}
			}

			set
			{
				IRPU _setRPU = value;
				IRPU _oldRPU = manualRPU;

				/// Блокировка для защиты от передачи параметров не тому ручному РПУ
				lock (manualRPUChangeLock)
				{
					if (_setRPU == null)
						_setRPU = rpuNull;

					/// Проверка наличия РПУ в списке доступных
					if (!AvailableRPUDevices.Contains(_setRPU))
						RegisterRPU(_setRPU);

					manualRPU = _setRPU;

					CallOnManualRPUChanged(_oldRPU, _setRPU);
				}
			}
		}

		void CallOnManualRPUChanged(IRPU pOldRPU, IRPU pNewRPU)
		{
			if (OnManualRPUChanged != null)
				OnManualRPUChanged(pOldRPU, pNewRPU);
		}

		public event ManualRPUChangedDelegate OnManualRPUChanged;

        #endregion

		#region ================ Public methods ================
		
		public IRPU UserSelectAvailableRPU()
		{
			if (AvailableRPUDevices.Count == 0)
				return null;

			if (AvailableRPUDevices.Count == 1)
				return AvailableRPUDevices[0];

			if (AvailableRPUDevices.Count == 2 && AvailableRPUDevices[0] == rpuNull)
				return AvailableRPUDevices[1];
			/// К менеджеру подключено более 1 устройства, значит надо спросить пользователя
			/// чем он хочет поуправлять
			/// 

			SelectRPUDialog _dlg = new SelectRPUDialog();
			IRPU _rpu = null;

			_dlg.RPUManager = this;
			_dlg.SelectedRPU = manualRPU;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				_rpu = _dlg.SelectedRPU;
			}

			return _rpu;
		}

#if false		
		/// <summary>
		/// Помещение задачи в менеджер РПУ
		/// </summary>
		/// <param name="pTask"></param>
		/// <returns></returns>
		public bool TaskRequest(RPUTaskRequest pTask, TaskProcessCallback pCallback)
		{
			bool _res = false;

			if (taskProcessorThread == null ||
				taskProcessorThread.ThreadState != ThreadState.Running)
				return false;

			try
			{
				/// Ожидание очереди
				while (taskRequestsQueue.Count > 0)
					if (!evtTaskProcessed.WaitOne(1000, true))
						throw new TimeoutException("Таймаут ожидания очереди!");

				/// Добавление задания в очередь
				taskRequestsQueue.Enqueue(new TaskRequestItem(pTask, pCallback));

				/// Установка флага нового задания
				evtTaskNew.Set();

				_res = true;
			}

			catch
			{
				_res = false;
			}

			return _res;
		}

		/// <summary>
		/// Запуск обработчика запросов
		/// </summary>
		public void StartTaskProcessor()
		{
			taskProcessorThread = new Thread(TaskProcessorMain);
			taskProcessorThread.Start();
		}

		public void StopTaskProcessor()
		{
			taskProcessorThread.Abort();
		}
#endif
		/// <summary>
		/// Включение всех РПУ
		/// </summary>
		public void SwitchOn()
		{
			foreach (IRPU _rpu in AvailableRPUDevices)
			{
				Splash.Status = String.Format(Locale.status_turning_on_rpu, _rpu.Name);

				try
				{
					_rpu.SwitchOn();
				}

				catch
				{
					//MessageBox.Show(String.Format(Locale.err_turning_on_rpu, _rpu),
					//	Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);

					/// Ошибка включения РПУ
					/// Исключаем ошибочный РПУ из списка доступных
					/// 
					//AvailableRPUDevices.Remove(_rpu);
					//rpuDevices.Remove(_rpu);
					//_rpu.Dispose();
				}
			}
		}

		/// <summary>
		/// Выключение всех РПУ
		/// </summary>
		public void SwitchOff()
		{
			foreach (IRPU _rpu in AvailableRPUDevices)
			{
				try
				{
					if (_rpu.IsAvailable)
					{
						/// Отключение РПУ
						_rpu.SwitchOff();
					}
					_rpu.Dispose();
				}
				catch
				{
					/// Ошибка оключения РПУ
					/// 
					
					//MessageBox.Show("Ошибка выключения РПУ " + _rpu.Name + "!", "Ошибка", 
					//	MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			rpuDevices.Clear();
			AvailableRPUDevices.Clear();
		}

		/// <summary>
		/// Получение РПУ по идентификатору
		/// </summary>
		/// <param name="pId"></param>
		/// <returns></returns>
		public IRPU GetRPUById(Guid pId)
		{
			var q = from rpu in AvailableRPUDevices
					where rpu.Id == pId
					select rpu;
			
			IRPU _rpu = null;

			int _rpuCount = q.Count();
			
			if (_rpuCount == 0)
				_rpu = null;
			else if (_rpuCount == 1)
				_rpu = q.ToArray()[0];
			else
				throw new InvalidOperationException("Different RPU with equal id's!");

			return _rpu;
		}

		public IRPU GetRPUByIdOrType(Guid pRPUId, Guid pTypeId)
		{
			IRPU _rpu = GetRPUById(pRPUId);

			if (_rpu == null)
			{
				var q = from _r in AvailableRPUDevices
						where _r.RPUType == pTypeId
						select _r;

				if (q.Count() > 0)
					_rpu = q.First();
			}

			return _rpu;
		}

		#endregion

		#region ================ Приватные методы ================
#if false		
		/// <summary>
		/// Обработчик запросов 
		/// </summary>
		/// <param name="pParam"></param>
		void TaskProcessorMain(object pParam)
		{
			while (isTaskProcessorRunning)
			{
				while (taskRequestsQueue.Count == 0)
					evtTaskNew.WaitOne();

				TaskRequestItem _item = taskRequestsQueue.Peek();
				RPUTaskResponse _resp = ProcessRequest(_item.request);

				if (_resp == null)
					throw new InvalidOperationException("Задача " + _item.request.Id + " не обработана!");

				if (_item.callBack != null)
					_item.callBack(_resp);

				taskRequestsQueue.Dequeue();

				/// Установка признака окончания обработки запроса
				evtTaskProcessed.Set();
			}
		}

		/// <summary>
		/// Обработчик запросов
		/// </summary>
		/// <param name="pRequest"></param>
		/// <returns></returns>
		RPUTaskResponse ProcessRequest(RPUTaskRequest pRequest)
		{
			IRPU _rpu = AcquireRPU(pRequest.Frequency);
			if (_rpu == null)
				return null;

			double _power = _rpu.PowerMeter.MeasurePower(pRequest.Frequency);
			RPUTaskResponse _resp = new RPUTaskResponse(pRequest, _power);

			FreeRPU(_rpu);
			return _resp;
		}
#endif
		#endregion

		#region ================ Статические методы ================

		#endregion


		#region IRPUManager Members

		public void RegisterRPU(IRPU pRPU)
		{
			rpuDevices.Add(pRPU);
		}

		public IRPU AcquireRPU()
		{
			throw new NotImplementedException();
		}

		public IRPU AcquireRPU(long pFreq)
		{
			bool _freeRPUFound = false;
			IRPU _selectedRPU = null;

			while(!_freeRPUFound)
			{
				lock(AvailableRPUDevices)
				{
					foreach(IRPU _rpu in AvailableRPUDevices)
					{
						if(_rpu.IsBusy == false && 
							(_rpu.FreqMin >= pFreq && _rpu.FreqMax <= pFreq)
							)
						{
							_rpu.IsBusy = true;
							_selectedRPU = _rpu;
							break;
						}
					}
				}

//				if (!evtFreeRPU.WaitOne(1000, true))
//					return null;
			}

			return _selectedRPU;
		}

		public IRPU AcquireRPU(long pFreq, bool pHasPowerMeter, bool pHasDemodulator, bool pHasSpectrograph)
		{
			throw new NotImplementedException();
		}

		public void FreeRPU(IRPU pRPU)
		{
			pRPU.IsBusy = false;
		}

		#endregion

		/// <summary>
		/// Сохранение параметров менеджера РПУ
		/// </summary>
		/// <param name="pWriter"></param>
		internal void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("RPUDevices");

			foreach (IRPU _rpu in rpuDevices)
			{
				if (_rpu == rpuNull)
					continue;

				pWriter.WriteStartElement("RPU");

				pWriter.WriteAttributeString("disabled", _rpu.IsDisabled.ToString(CultureInfo.InvariantCulture));
				pWriter.WriteAttributeString("id", _rpu.Id.ToString());
				pWriter.WriteAttributeString("type", _rpu.GetType().AssemblyQualifiedName);
				pWriter.WriteAttributeString("assembly", _rpu.GetType().Assembly.FullName);

				_rpu.SaveToXmlWriter(pWriter);

				pWriter.WriteEndElement();
			}

			pWriter.WriteEndElement();
		}

		/// <summary>
		/// Удаление РПУ из списка
		/// </summary>
		/// <param name="_rpu"></param>
		internal void UnregisterRPU(IRPU _rpu)
		{
			try
			{
				if (_rpu.IsBusy || _rpu.IsTaskProcessorRunning)
				{
					MessageBox.Show(
						Locale.err_delete_rpu,
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);

					return;
				}

				_rpu.SwitchOff();

				rpuDevices.Remove(_rpu);
			}

			catch (Exception ex)
			{
			}
		}

		#region IRPUManager Members


		public event RPUAddedDelegate OnRPUAdded;

		public event RPUDeletedDelegate OnRPUDeleted;

		#endregion

		/// <summary>
		/// Проверка конфигурации РПУ
		/// </summary>
		internal void CheckConfiguration()
		{
			if (rpuDevices.Count == 0)
				throw new Exception(Locale.no_rpus_present);
			foreach (IRPU _rpu in rpuDevices)
			{
				_rpu.CheckConfiguration();
			}
		}

		#region ================ Статические методы для работы со списком классов РПУ ===============
		static List<RPUClass> rpuClasses;

		static public List<RPUClass> RPUClasses
		{
			get
			{
				if (rpuClasses == null)
				{
					CreateRPUClasses();
				}

				return rpuClasses;
			}
		}

		private static void CreateRPUClasses()
		{
			rpuClasses = new List<RPUClass>();

			DirectoryInfo _di = new DirectoryInfo(Application.StartupPath);

			try
			{
				FileInfo[] _files = _di.GetFiles("*.dll");

				foreach (FileInfo _fi in _files)
				{
					try
					{
						Assembly _as = Assembly.LoadFrom(_fi.FullName);

						foreach (Type _t in _as.GetTypes())
						{
							Type _rpuType = _t.GetInterface("IRPU");

							if (_rpuType == null)
								continue;

							//string _rpuName = (string)_t.InvokeMember("ClassName", BindingFlags.Static | BindingFlags.GetProperty, null, null, new object[] { });

							try
							{
								rpuClasses.Add(new RPUClass(_t));
							}

							catch
							{
								
							}
						}
					}

					catch
					{
					}
				}
			}
			catch (Exception pEx)
			{

			}
		}
		#endregion

	}
}
