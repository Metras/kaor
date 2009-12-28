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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.AntennaManager;
using KaorCore.Base;
using KaorCore.Interfaces;
using KaorCore.I18N;
using KaorCore.Marker;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.Signal;
using KaorCore.Trace;
using KaorCore.TraceControl;
using KaorCore.User;

using ControlUtils.Splash;

namespace KaorCore.RadioControlSystem
{
	public delegate void OnManualFrequencyChanged(Int64 pFrequency);

	/// <summary>
	/// Режим радиоконтроля
	/// Система всегда находится в одном из режимов
	/// </summary>
	public enum ERadioControlMode
	{
		/// <summary>
		/// Режим ожидания
		/// </summary>
		Idle,

		/// <summary>
		/// Сканирование
		/// </summary>
		Scan,

		/// <summary>
		/// Пауза сканирования
		/// </summary>
		Pause
	}

	/// <summary>
	/// Асбтрактная система радиоконтроля
	/// Осуществляет управление комплексом РПУ, топологически связанных с антеннами
	/// На основании параметров сканирования формирует внутренния задания
	/// Выходом системы являются:
	///  - события тревоги,
	///  - трассы
	/// </summary>
	public class BaseRadioControlSystem
	{
		#region ================ Поля ================
		
		#region ================ Описание системы ================
		
		Guid id;
		
		string name;
		string description;
		
		GPSCoordinates location;

		#endregion

		List<IAntenna> antennas;

		/// <summary>
		/// Менеджер РПУ
		/// </summary>
		BaseRPUManager managerRPU;

		/// <summary>
		/// Трассы, по которым ведется контроль
		/// </summary>
		//List<BaseTraceControl> traceControls;

		/// <summary>
		/// Отсканированные трассы
		/// </summary>
		List<BaseTrace> scanTraces;

		/// <summary>
		/// Режим работы СРК
		/// </summary>
		ERadioControlMode operationMode;

		/// <summary>
		/// Контрол управления СРК
		/// </summary>
		BaseRCSView control;

		/// <summary>
		/// Список подключенных к системе пользователей
		/// </summary>
		List<BaseUser> users;

		/// <summary>
		/// Список маркеров, находящихся в системе
		/// </summary>
		List<BaseRadioMarker> markers;

		/// <summary>
		/// РПУ ручного управления
		/// При остановке автоматического режима, СРК делает запрос на РПУ для ручного режима работы
		/// При выходе из ручного режима работы, РПУ освобождается и помещается в пул РПУ
		/// </summary>
		IRPU manualRPU;

		/// <summary>
		/// Список РПУ автоматического режима
		/// При старте автоматического режима, СРК делает запрос на все имеющиеся РПУ
		/// При останове автоматического режима, все РПУ освобождаются и помещаются в пул РПУ
		/// </summary>
		List<IRPU> autoRPUs;

		/// <summary>
		/// Сигналы, известные системе
		/// </summary>
		List<BaseSignal> signals;

		int autoSaveTimeout;
		bool isCrashed;
		string sessionPath = Application.StartupPath + "\\autosave";
		string sessionFile = Application.StartupPath + "\\autosave\\.session";

		#endregion

		#region ================ Конструктор ================
		/// <summary>
		/// Базовый параметерлесс конструктор
		/// Производит инициализацию внутренних переменных
		/// </summary>
		/// <param name="pFileName"></param>
		public BaseRadioControlSystem()
		{
			Splash.Status = Locale.status_initializing_base;

			id = Guid.NewGuid();
			name = Locale.rcs_name;
			description = Locale.rcs_descr;

			/// Инициализация внутренних структур
			/// 
			antennas = new List<IAntenna>();
			scanTraces = new List<BaseTrace>();
			//traceControls = new List<BaseTraceControl>();

			users = new List<BaseUser>();
			markers = new List<BaseRadioMarker>();

			signals = new List<BaseSignal>();

			autoSaveTimeout = 600000;

			autoSaveTimer = new System.Timers.Timer(autoSaveTimeout);
			autoSaveTimer.AutoReset = true;
			autoSaveTimer.Elapsed += new System.Timers.ElapsedEventHandler(autoSaveTimer_Elapsed);

			if (!Directory.Exists(sessionPath))
				Directory.CreateDirectory(sessionPath);

			if (File.Exists(sessionFile) && File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/autosave/fullstate.ks"))
				isCrashed = true;
			else
			{
				FileStream _fs = File.Create(sessionFile);
				_fs.Close();

				isCrashed = false;
			}

			managerRPU = new BaseRPUManager();

			managerRPU.OnRPUAdded += new KaorCore.RPUManager.RPUAddedDelegate(RPUManager_Changed);
			managerRPU.OnRPUDeleted += new KaorCore.RPUManager.RPUDeletedDelegate(RPUManager_Changed);


			operationMode = ERadioControlMode.Idle;
		}

		~BaseRadioControlSystem()
		{
			File.Delete(sessionFile);
		}

		void RPUManager_Changed(IRPU pRPU)
		{
			CallOnConfigurationChanged();
		}

		void autoSaveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			AutoSaveFullState();
		}
		
		/// <summary>
		/// Конструктор СРК
		/// Производит формирование СРК на основании данных, содержащихся в файле pFileName
		/// В качестве рабочей версии будет использоваться XML-описание СРК, 
		/// описывающее топологию системы (антенна, менеджеры антенн, и т.д.)
		/// 
		/// После создания СРК в списке трасс пусто, система работает в ручном режиме.
		/// Возможно (даже не возможно, а ОБЯЗАТЕЛЬНО) потребуется дополнение конструктора 
		/// для автоматической загрузки последнего состояния СРК, включая трассы, и т.д.
		/// </summary>
		/// <param name="pFileName"></param>
		public BaseRadioControlSystem(string pXMLFile)
			: this()
		{
			try
			{
				Splash.Status = Locale.status_loading_configuration;

				XmlNode _mainNode, _node;
				XmlDocument _doc = new XmlDocument();
				_doc.Load(pXMLFile);

				_mainNode = _doc.SelectSingleNode("RadioControlSystem");

				if(_mainNode.Attributes["id"] != null)
					id = new Guid(_mainNode.Attributes["id"].Value);
				
				if(_mainNode.Attributes["name"] != null)
					name = _mainNode.Attributes["name"].Value;

				_node = _mainNode.SelectSingleNode("autosave");
				if (_node != null)
					if (!int.TryParse(_node.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out autoSaveTimeout))
						autoSaveTimeout = -1;

				if (!isCrashed && autoSaveTimeout > 30000)
				{
					autoSaveTimer.Enabled = false;
					autoSaveTimer.Interval = autoSaveTimeout;
					autoSaveTimer.Enabled = true;
				}
				else
				{
					autoSaveTimer.Enabled = false;
				}

				_node = _mainNode.SelectSingleNode("Antennas");

				Splash.Status = Locale.status_loading_antennas;

				foreach (XmlNode _antennaNode in _node.ChildNodes)
				{
					antennas.Add(BaseAntenna.FromXmlNode(_antennaNode));
				}

				Splash.Status = Locale.status_loading_rpu;

				_node = _mainNode.SelectSingleNode("RPUManager");

				managerRPU.LoadFromXmlNode(_node);
			}

			catch (Exception e)
			{

			}
		}

		#endregion

		#region ================ Свойства ================

		/// <summary>
		/// Режим работы СРК
		/// </summary>
		public ERadioControlMode OperationMode
		{
			get
			{
				return operationMode;
			}

			private set
			{
				ERadioControlMode _oldMode = operationMode;

				operationMode = value;

				CallOnOperationModeChanged(_oldMode, operationMode);
				
			}
		}

		void CallOnOperationModeChanged(ERadioControlMode pOldMode, ERadioControlMode pNewMode)
		{
			if (OnOperationModeChanged != null)
				OnOperationModeChanged(pOldMode, pNewMode);
		}

		public event OperationModeChangedDelegate OnOperationModeChanged;
		public delegate void OperationModeChangedDelegate(ERadioControlMode pOldMode, ERadioControlMode pNewMode);


		/// Контрол упралвения СРК
		public BaseRCSView Control
		{
			get
			{
				return control;
			}

			set
			{
				control = value;
			}
		}
		/*
		public List<BaseTraceControl> TraceControls
		{
			get
			{
				return traceControls;
			}
		}
		*/
		public List<BaseTrace> ScanTraces
		{
			get
			{
				return scanTraces;
			}
		}

		public BaseRPUManager RPUManager
		{
			get
			{
				return managerRPU;
			}
		}

		public List<BaseRadioMarker> Markers
		{
			get
			{
				return markers;
			}
		}

		public Int64 ManualFrequency
		{
			get
			{
				return 0;
			}

			set
			{
			}
		}
		#endregion

		#region ================ Публичные методы ================
		/// <summary>
		/// Подключение нового пользователя к СРК
		/// </summary>
		/// <param name="pUser"></param>
		public void UserConnect(BaseUser pUser)
		{
			users.Add(pUser);
		}

		/// <summary>
		/// Отключение пользователя от системы
		/// </summary>
		/// <param name="pUser"></param>
		public void UserDisconnect(BaseUser pUser)
		{
			users.Remove(pUser);
		}

		/// <summary>
		/// Добавление трассы в список трасс сканирования
		/// </summary>
		/// <param name="pTrace"></param>
		public void AddScanTrace(BaseTrace pTrace)
		{
			var q = from _s in scanTraces
					where _s.Id == pTrace.Id
					select _s;

			bool _alreadyHere = (q.Count() > 0);
			if (_alreadyHere)
			{
				if (MessageBox.Show(String.Format(Locale.confirm_delete_cur_trace, pTrace.Name, pTrace.Id),
					Locale.confirmation,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					DeleteScanTrace(q.First());
				}
				else
				{
					pTrace.NewId();
				}
			}

			lock (scanTraces)
			{
				scanTraces.Add(pTrace);
				pTrace.OnTraceScanChanged += new TraceChanged(pTrace_OnTraceScanChanged);
				pTrace.OnTraceMemCycleChanged += new BaseTrace.TraceMemCycleChangedDelegate(pTrace_OnTraceMemCycleChanged);
				pTrace.OnNewTraceMemCycle += new BaseTrace.NewTraceMemCycleDelegate(pTrace_OnNewTraceMemCycle);
			}
		}

		void pTrace_OnNewTraceMemCycle(BaseTrace pTrace)
		{
			control.RedrawWaterfallFull(pTrace);
		}

		void pTrace_OnTraceMemCycleChanged(BaseTrace pTrace)
		{
			//control.RedrawWaterfall();
		}

		void pTrace_OnTraceScanChanged(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
		{
			SignalProcessPoint(pTrace, pPoint, pOldPower);
		}

		/// <summary>
		/// Удаление трассы сканирования
		/// </summary>
		/// <param name="pTrace"></param>
		public void DeleteScanTrace(BaseTrace pTrace)
		{
			lock (scanTraces)
			{
				pTrace.OnTraceScanChanged -= pTrace_OnTraceScanChanged;
				scanTraces.Remove(pTrace);
			}
		}

		/// <summary>
		/// Запись трассы в файл
		/// </summary>
		/// <param name="pFileName"></param>
		/// <param name="pTrace"></param>
		public void SaveTraceToFile(string pFileName, BaseTrace pTrace)
		{
			XmlTextWriter _xmlWriter = new XmlTextWriter(pFileName, Encoding.UTF8);
			_xmlWriter.Formatting = Formatting.Indented;
			_xmlWriter.WriteStartDocument(true);

			pTrace.SaveToXml(_xmlWriter);

			_xmlWriter.WriteEndDocument(); /// Конец документа
			_xmlWriter.Close();

		}

		BaseTrace LoadTraceFromNode(XmlNode pTraceNode)
		{
			BaseTrace _trace;
			string _typeName = pTraceNode.Attributes["type"].Value;
			//string _assName = _node.Attributes["assembly"].Value;

			if (_typeName == null)
				return null;

			//Assembly _ass = Assembly.Load(_assName);
			Type _t = Type.GetType(_typeName);

			if (_t == null)
			{
				throw new Exception("Unsupported trace type: " + _typeName + "!");
			}

			object _o = Activator.CreateInstance(_t, pTraceNode);

			_trace = _o as BaseTrace;

			/// Проверка корректности загрузки параметров сканирования
			if (_trace.IsLoadScanParamsFailed)
			{
				/// В трассе были сохранены параметры сканирвания, но их
				/// загрузка не получилась, т.к. не найден РПУ или Антенна
				/// 
				MessageBox.Show(Locale.err_loading_scan_params,
					Locale.warning,
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			return _trace;
		}
		/// <summary>
		/// Загрузка трассы из файла
		/// </summary>
		/// <param name="pFilename"></param>
		/// <returns></returns>
		public BaseTrace LoadTraceFromFile(string pFilename)
		{
			BaseTrace _trace = null;

			try
			{
				XmlDocument _xmlDoc = new XmlDocument();
				_xmlDoc.Load(pFilename);

				XmlNode _node = _xmlDoc.SelectSingleNode("KaorTrace");
				if (_node == null)
					return null;

				_trace = LoadTraceFromNode(_node);
			}
			catch(Exception e)
			{
				_trace = null;
				MessageBox.Show(String.Format(Locale.err_loading_trace, pFilename),
					Locale.error, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return _trace;
		}

		///// <summary>
		///// Запуск сканирования трассы
		///// </summary>
		///// <param name="pTrace"></param>
		//public void StartTraceScan(BaseTrace pTrace)
		//{
		//    pTrace.StartScan();

		//    /// old traces
		//    //pTrace.RPU.StartTaskProcessor(pTrace.TaskProvider);
		//}

		///// <summary>
		///// Останов сканирования трассы
		///// </summary>
		///// <param name="pTrace"></param>
		//public void StopTraceScan(BaseTrace pTrace)
		//{
		//    pTrace.StopScan();
		//    /// Old traces
		//    //IRPU rpu = pTrace.RPU;
		//    //rpu.StopTaskProcessor(pTrace.TaskProvider);
		//}

		/// <summary>
		/// Включение контроля по трассе
		/// </summary>
		/// <param name="pTrace"></param>
		public void ArmTrace(BaseTrace pTrace)
		{
			BaseTraceControl _traceControl;

			if (scanTraces.Contains(pTrace) == false)
				return;

			if (pTrace.TraceControl == null)
				_traceControl = new TraceControlAdaptiveBounds(pTrace);
			else
				_traceControl = pTrace.TraceControl;

			BaseTraceControlsEditor _dlgEditor = new BaseTraceControlsEditor(_traceControl);

			if (_dlgEditor.ShowDialog() == DialogResult.OK)
			{
				pTrace.TraceControl = _traceControl;
				pTrace.ScanMode = ETraceScanMode.Control;
			}
		}

		/// <summary>
		/// Снятие трассы с контроля
		/// </summary>
		/// <param name="pTrace"></param>
		public void DisarmTraceControl(BaseTraceControl pTraceControl)
		{
#if false
			if (!traceControls.Contains(pTraceControl))
				return;

			traceControls.Remove(pTraceControl);
			RemoveTriggerHandlers(pTraceControl);
#endif
		}

		public delegate void SwitchChangedDelegate(bool pOn);
		public event SwitchChangedDelegate OnSwitchChanged;

		/// <summary>
		/// Включение СРК
		/// </summary>
		public void SwitchOn()
		{
			/// Включенеи антенных устройств
			/// 
			Splash.Status = Locale.status_turning_on;

			RPUManager.SwitchOn();
			if (OnSwitchChanged != null)
				OnSwitchChanged(true);
		}


		/// <summary>
		/// Отключение СРК
		/// </summary>
		public void SwitchOff()
		{
			StopArmedMode();

			RPUManager.SwitchOff();

			if (OnSwitchChanged != null)
				OnSwitchChanged(false);
		}

		public delegate void StartArmedModeDelegate(bool pNeedReset);

		/// <summary>
		/// Запуск режима контроля по трассам
		/// </summary>
		public void StartArmedMode(bool pNeedReset)
		{
			foreach (BaseTrace _trace in scanTraces)
			{
				_trace.StopScan();
				/// old traces
				//_trace.RPU.StopTaskProcessor(_trace.TaskProvider);
			}

			var q = from _trace in scanTraces
					where _trace.ScanMode == ETraceScanMode.Scan ||
					_trace.ScanMode == ETraceScanMode.Control
					select _trace;

			foreach(BaseTrace _trace in q)
			{
				_trace.StartScan(pNeedReset);

				/// old traces
				//_trace.TaskProvider.Reset();
				//_trace.RPU.StartTaskProcessor(_trace.TaskProvider);
			}

			OperationMode = ERadioControlMode.Scan;
		}

		/// <summary>
		/// Пауза режима контроля
		/// </summary>
		/// <returns>true, если не было отмены сканирования
		/// false, если была отмена
		/// </returns>
		public DialogResult PauseArmedMode(int pTime, long pFreq, double pPower, string pSignalName, Color pBackColor, double pDelta)
		{
			DialogResult _res;

			if (managerRPU.ManualRPU.IsTaskProcessorRunning)
			{
				managerRPU.ManualRPU.PauseTaskProcessors();
			}

			OperationMode = ERadioControlMode.Pause;

			string _caption = Locale.signal_appear;

			if (pDelta < 0.0)
			{
				_caption = Locale.signal_disappear;
			}

			_res = control.ShowPauseWindow(pTime, _caption, KaorCore.Utils.FreqUtils.FreqToString(pFreq), 
				pPower.ToString("0.0") + " " + Locale.dbm, 
				pSignalName, 
				pDelta.ToString("0.0") + " " + Locale.db, 
				pBackColor);

			if (_res == DialogResult.Abort)
			{
				/// Останов сканирования на ручном РПУ
				/// Остаеmsя в режиме паузы
				/// 
				new MethodInvoker(StopArmedMode).BeginInvoke(null, null);
				//StopArmedMode();
				//_res = false;
			}
			else
			{
				/// Продолжаем сканирование
				/// 
				if (managerRPU.ManualRPU.IsTaskProcessorRunning)
					managerRPU.ManualRPU.ResumeTaskProcessors();

				OperationMode = ERadioControlMode.Scan;
			}

			return _res;
		}

#if false
		/// <summary>
		/// Возобновление сканирования на ручном РПУ
		/// </summary>
		/// <returns></returns>
		public bool ResumeArmedMode()
		{
			if (managerRPU.ManualRPU.IsTaskProcessorRunning)
			{
				managerRPU.ManualRPU.ResumeTaskProcessors();
			}

			return true;
		}
#endif

		/// <summary>
		/// Выход из режима контроля
		/// </summary>
		/// <returns></returns>
		public void StopArmedMode()
		{

			foreach (BaseTrace _trace in scanTraces)
			{
				_trace.StopScan();
				/// old traces
				//_trace.RPU.StopTaskProcessor(_trace.TaskProvider);
			}

			//control.SetNormalModeView();
			OperationMode = ERadioControlMode.Idle;
		}


		#endregion

		#region =============== Работа с сигналами ===============

		private void LoadSignals()
		{
		}

		public void SaveSignalsToXml(XmlWriter pXmlWriter)
		{
			pXmlWriter.WriteStartElement("Signals");

			lock (signals)
			{
				foreach (BaseSignal _signal in signals)
				{
					_signal.SaveToXmlWriter(pXmlWriter);
				}
			}
			pXmlWriter.WriteEndElement();

		}

		public void SaveSignalsToFile(string pFileName)
		{
			XmlTextWriter _xmlWriter = new XmlTextWriter(pFileName, Encoding.UTF8);
			_xmlWriter.Formatting = Formatting.Indented;
			_xmlWriter.WriteStartDocument(true);

			SaveSignalsToXml(_xmlWriter);

			_xmlWriter.WriteEndDocument(); 
			_xmlWriter.Close();
		}

		/// <summary>
		/// Очистка списка сигналов
		/// </summary>
		public void ClearSignals()
		{
			lock (signals)
			{
				List<BaseSignal> _tmpSignalList = new List<BaseSignal>(signals);

				foreach (BaseSignal _signal in _tmpSignalList)
				{
					_signal.IsVisible = false;
					_signal.OnSignalChanged -= signal_OnSignalChanged;
				}

				signals.Clear();
			}
		}

		void LoadSignalsFromNode(XmlNode pSignalsNode)
		{
			/// Загрузка всех сигналов в списке
			foreach (XmlNode _signalNode in pSignalsNode.ChildNodes)
			{
				string _typeName = null;

				if (_signalNode.Attributes["type"] != null)
					_typeName = _signalNode.Attributes["type"].Value;

				if (_typeName == null)
					continue;

				Type _t = Type.GetType(_typeName);
				if (_t == null)
					throw new Exception("Unsupported signal type: " + _typeName + "!");

				BaseSignal _signal = (BaseSignal)Activator.CreateInstance(_t);
				if (_signal == null)
					continue;

				/// Загрузка параметров сигнала самим сигналом
				_signal.LoadFromXmlNode(_signalNode);

				AddSignal(_signal);

				_signal.IsVisible = true;
			}

		}

		public void LoadSignalsFromFile(string pFileName, bool pNeedClear)
		{
			try
			{
				XmlDocument _xmlDoc = new XmlDocument();
				_xmlDoc.Load(pFileName);

				XmlNode _node = _xmlDoc.SelectSingleNode("Signals");
				if (_node == null)
					return;

				/// Очистка списка сигналов при необходимости
				if (pNeedClear)
					ClearSignals();
					//signals.Clear();

				LoadSignalsFromNode(_node);
			}
			catch
			{
				throw;
			}
		}

		public List<BaseSignal> Signals
		{
			get { return signals; }
		}

		/// <summary>
		/// Добавление сигнала
		/// </summary>
		/// <param name="pSignal"></param>
		public void AddSignal(BaseSignal pSignal)
		{
			lock (signals)
			{
				pSignal.OnSignalChanged += new BaseSignal.SignalChangedDelegate(signal_OnSignalChanged);
				signals.Add(pSignal);
				signals.Sort();
			}
		}

		void signal_OnSignalChanged(BaseSignal pSignal)
		{
//			signals.Sort();
			control.UpdateSignal(pSignal);
			//control.UpdateSignals();
		}

		public void RemoveSignal(BaseSignal pSignal)
		{
			lock (signals)
			{
				pSignal.OnSignalChanged -= signal_OnSignalChanged;
				signals.Remove(pSignal);
				signals.Sort();
			}
		}

		public bool SignalProcessPoint(BaseTrace pTrace, TracePoint pTracePoint,
			double pOldPower)
		{
			lock (signals)
			{
				var q = from _s in signals
						where _s.Frequency - _s.Band / 2 < pTracePoint.Freq &&
						_s.Frequency + _s.Band / 2 > pTracePoint.Freq
						select _s;

				foreach (BaseSignal _signal in q)
				{
					_signal.ProcessPoint(pTrace, pTracePoint, pOldPower);
				}

			}
			return true;
		}

		/// <summary>
		/// Анализ точки на вхождение в один из сигналов
		/// </summary>
		/// <param name="pTraceControl"></param>
		/// <param name="pPoint"></param>
		/// <param name="pOldPower"></param>
		public bool SignalAnalyzePoint(BaseTraceControl pTraceControl, TracePoint pPoint, 
			double pOldPower, double pDelta)
		{
			IEnumerable<BaseSignal> q = from _s in signals
					where _s.IsTracePointBelongs(pPoint, 
						pTraceControl.ScanTrace.ScanParams.FilterBand)
					select _s;

			if (pDelta > 0 && q.Count() == 0)
			{
				/// Добавление нового сигнала, если было превышение порога
				/// 
				BaseSignal _s;

#if Old_signal_creation
				RangeSignal _s = new RangeSignal();

				_s.Frequency = pPoint.Freq;
				_s.Band = pTraceControl.ScanTrace.ScanParams.FilterBand * 2;
				_s.Pmax = 0;
				_s.Pmin = -140;
#else
				_s = pTraceControl.CreateDefaultSignal(pPoint, pOldPower, pDelta);
#endif		
				if (_s != null)
				{
					

#if Old_signal_creation
				_s.RecordRPU = pTraceControl.ScanTrace.ScanParams.RPU;
#endif

					if (_s.ProcessTrigger(pTraceControl, pPoint, pOldPower, pDelta) == 
						EBaseSignalTriggerAction.Accept)
					{
						/// Добавление сигнала в список
						AddSignal(_s);

						/// Делаем сигнал видимым
						_s.IsVisible = true;
					}
				}
			}
			else
			{
				lock (signals)
				{

					List<BaseSignal> _tmpList = q.ToList();

					foreach (BaseSignal _signal in _tmpList)
					{
						_signal.ProcessTrigger(pTraceControl, pPoint, pOldPower, pDelta);
					}
				}
			}

			control.UpdateSignals();

			return true;
		}

		#endregion


		#region ================ Работа с маркерами ================

		/// <summary>
		/// Добавление маркера в список маркеров
		/// </summary>
		/// <param name="pMarker"></param>
		public void AddMarker(BaseRadioMarker pMarker)
		{
			lock (markers)
			{
				pMarker.OnMarkerChanged += new BaseRadioMarker.MarkerChangedDelegate(pMarker_OnMarkerChanged);
				markers.Add(pMarker);
				markers.Sort();

			}
		}

		void pMarker_OnMarkerChanged(BaseRadioMarker pMarker)
		{
			control.UpdateMarker(pMarker);
		}

		/// <summary>
		/// Удаление маркера из списка маркеров
		/// </summary>
		/// <param name="pMarker"></param>
		public void RemoveMarker(BaseRadioMarker pMarker)
		{
			lock (markers)
			{
				pMarker.IsVisible = false;

				pMarker.OnMarkerChanged -= pMarker_OnMarkerChanged;
				markers.Remove(pMarker);
				markers.Sort();
			}
		}


		void LoadMarkersFromNode(XmlNode pMarkersNode)
		{
			/// Загрузка всех сигналов в списке
			foreach (XmlNode _markerNode in pMarkersNode.ChildNodes)
			{
				string _typeName = null;

				if (_markerNode.Attributes["type"] != null)
					_typeName = _markerNode.Attributes["type"].Value;

				if (_typeName == null)
					continue;

				Type _t = Type.GetType(_typeName);
				if (_t == null)
					throw new Exception("Unsupported marker type: " + _typeName + "!");

				BaseRadioMarker _marker = (BaseRadioMarker)Activator.CreateInstance(_t);
				if (_marker == null)
					continue;

				/// Загрузка параметров маркера самим маркером
				_marker.LoadFromXmlNode(_markerNode);

				AddMarker(_marker);

				_marker.IsVisible = true;
			}

		}

		/// <summary>
		/// Загрузка маркеров из файла
		/// </summary>
		/// <param name="pFileName"></param>
		/// <param name="pNeedClear"></param>
		public void LoadMarkersFromFile(string pFileName, bool pNeedClear)
		{
			try
			{
				XmlDocument _xmlDoc = new XmlDocument();
				_xmlDoc.Load(pFileName);

				XmlNode _node = _xmlDoc.SelectSingleNode("Markers");
				if (_node == null)
					return;

				/// Очистка списка сигналов при необходимости
				if (pNeedClear)
					markers.Clear();

				LoadMarkersFromNode(_node);
			}
			catch
			{
				throw;
			}

		}

		public void SaveMarkersToXml(XmlWriter pXmlWriter)
		{
			pXmlWriter.WriteStartElement("Markers");

			lock (markers)
			{
				foreach (BaseRadioMarker _marker in markers)
				{
					_marker.SaveToXmlWriter(pXmlWriter);
				}

			}
			pXmlWriter.WriteEndElement();

		}

		/// <summary>
		/// Сохранение маркеров в файл
		/// </summary>
		/// <param name="pFileName"></param>
		public void SaveMarkersToFile(string pFileName)
		{
			XmlTextWriter _xmlWriter = new XmlTextWriter(pFileName, Encoding.UTF8);
			_xmlWriter.Formatting = Formatting.Indented;
			_xmlWriter.WriteStartDocument(true);
			
			SaveMarkersToXml(_xmlWriter);

			_xmlWriter.WriteEndDocument();
			_xmlWriter.Close();
		}

		/// <summary>
		/// Очистка списка сигналов
		/// </summary>
		public void ClearMarkers()
		{
			List<BaseRadioMarker> _tmpList = new List<BaseRadioMarker>(markers);

			foreach (BaseRadioMarker _marker in _tmpList)
				RemoveMarker(_marker);

			control.UpdateMarkers();
				//markers.Clear();
		}


		/// <summary>
		/// Сохранение конфигурации СРК в XmlWriter
		/// </summary>
		/// <param name="pWriter"></param>
		public void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("RadioControlSystem");

			pWriter.WriteStartElement("RPUManager");
			managerRPU.SaveToXmlWriter(pWriter);
			pWriter.WriteEndElement();

			/// Сохранение антенн
			pWriter.WriteStartElement("Antennas");

			foreach (IAntenna _a in BaseAntenna.AntennaList)
			{
				pWriter.WriteStartElement("Antenna");
				_a.SaveToXmlWriter(pWriter);
				pWriter.WriteEndElement();
			}

			pWriter.WriteEndElement();
			pWriter.WriteEndElement();
		}

		#endregion

		#region ================ Работа с сессиями ================
		public bool IsCrashed
		{
			get { return isCrashed; }
			set
			{
				isCrashed = value;

				if (isCrashed == false && autoSaveTimeout > 30000)
				{
					autoSaveTimer.Interval = autoSaveTimeout;
					autoSaveTimer.Enabled = true;
				}
			}
		}

		public void LoadAutoSaveState()
		{
			string _autoPathName = AppDomain.CurrentDomain.BaseDirectory + "/autosave";


			if (!Directory.Exists(_autoPathName))
				return;

			LoadMain(_autoPathName + "/fullstate.ks");
		}

		#endregion
		
		#region ================ Синглтон ================
		static BaseRadioControlSystem instance;

		static public BaseRadioControlSystem Instance
		{
			get
			{
				if (instance == null)
					throw new InvalidOperationException("Null instance! Use CreateInstance!");

				return instance;
			}
		}

		static public void CreateInstance(string pXmlFile)
		{
			instance = new BaseRadioControlSystem(pXmlFile);
		}

		#endregion

		#region ================ Работа с полным состоянием ================

		object saveLockObject = new object();
		string fullStateFilename = "";

		System.Timers.Timer autoSaveTimer;

		public void SaveFullState()
		{
			SaveFileDialog _dlg = new SaveFileDialog();
			_dlg.Title = Locale.save_rcs_state;
			_dlg.Filter = Locale.ks_filter;
			_dlg.InitialDirectory = Application.StartupPath + "\\states";

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				//fullStateFilename = _dlg.FileName;
				new SaveProcessDelegate(SaveMain).BeginInvoke(_dlg.FileName, null, null);
			}
		}

		public void LoadFullState()
		{
			OpenFileDialog _dlg = new OpenFileDialog();
			_dlg.Title = Locale.load_rcs_state;
			_dlg.Filter = Locale.ks_filter;
			_dlg.InitialDirectory = Application.StartupPath + "\\states";

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				if (!File.Exists(_dlg.FileName))
				{
					MessageBox.Show(String.Format(Locale.err_ks_not_found, _dlg.FileName),
						Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if (MessageBox.Show(Locale.confirm_ks_load,
					Locale.warning,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2) == DialogResult.No)
					return;

				new LoadProcessDelegate(LoadMain).BeginInvoke(_dlg.FileName, null, null);
			}
		}

		/// <summary>
		/// Автоматическое сохранение полного состояния
		/// </summary>
		internal void AutoSaveFullState()
		{
			string _autoPathName = AppDomain.CurrentDomain.BaseDirectory + "/autosave";

			if (!Directory.Exists(_autoPathName))
				Directory.CreateDirectory(_autoPathName);

			new SaveProcessDelegate(SaveMain).BeginInvoke(_autoPathName + "/fullstate.ks", null, null);
		}

		public delegate void SaveProcessDelegate(string pText);
		public delegate void LoadProcessDelegate(string pText);

		public event SaveProcessDelegate OnSaveProcess;

		private void CallOnSaveProcess(string pText)
		{
			if (OnSaveProcess != null)
				OnSaveProcess(pText);
		}

		/// <summary>
		/// Тело процедуры загрузки состояния системы
		/// </summary>
		/// <param name="pFileName"></param>
		private void LoadMain(string pFileName)
		{
			/// Все делаем в локе, чтобы не было сохранения переходных состояний системы
			lock (saveLockObject)
			{

				try
				{
					/// Делаем клинап
					ClearMarkers();
					ClearSignals();
					List<BaseTrace> _tmpList = new List<BaseTrace>(scanTraces);

					foreach (BaseTrace _trace in _tmpList)
					{
						DeleteScanTrace(_trace);
					}

					control.UpdateControl();

					/// Загружаем новые объекты из файла состояния
					/// 
					XmlDocument _doc = new XmlDocument();
					_doc.Load(pFileName);

					XmlNode _node;
					_node = _doc.SelectSingleNode("KaorFullState");

					if (_node == null)
						throw new Exception("Error loading KaorFullState");

					XmlNode _cn = _node.SelectSingleNode("Traces");

					if (_cn != null)
					{
						foreach (XmlNode _ct in _cn.ChildNodes)
						{
							BaseTrace _trace = LoadTraceFromNode(_ct);
							if (_trace != null)
							{
								AddScanTrace(_trace);
								control.UpdateTrace(_trace);
//								_trace.OnTraceScanChanged += new TraceChanged(_trace_OnTraceChanged);
//								rcs.AddScanTrace(_trace);
							}
						}

						control.UpdateControl();
					}

					_cn = _node.SelectSingleNode("Signals");

					if (_cn != null)
					{
						LoadSignalsFromNode(_cn);

						control.UpdateControl();
					}

					_cn = _node.SelectSingleNode("Markers");

					if (_cn != null)
					{
						LoadMarkersFromNode(_cn);
						control.UpdateControl();
					}
				}

				catch(Exception e)
				{
					MessageBox.Show(Locale.err_loading_state,
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		/// <summary>
		/// Тело процедуры сохранения состояния
		/// </summary>
		/// 
		
		private void SaveMain(string pFileName)
		{
			lock (saveLockObject)
			{
				CallOnSaveProcess(String.Format(Locale.status_saving, Path.GetFileName(pFileName)));

				if (File.Exists(pFileName + ".bak"))
					File.Delete(pFileName + ".bak");

				if (File.Exists(pFileName))
					File.Move(pFileName, pFileName + ".bak");

				/// Сохранение состояния системы
				/// 

				XmlTextWriter _xmlWriter = new XmlTextWriter(pFileName, Encoding.Unicode);
				_xmlWriter.Formatting = Formatting.Indented;

				_xmlWriter.WriteStartDocument();
				_xmlWriter.WriteStartElement("KaorFullState");

				/// Сохранение трасс
				/// 
				_xmlWriter.WriteStartElement("Traces");
				lock (scanTraces)
				{
					foreach (BaseTrace _trace in scanTraces)
					{
						_trace.SaveToXml(_xmlWriter);
					}
				}
				_xmlWriter.WriteEndElement();

				SaveSignalsToXml(_xmlWriter);
				SaveMarkersToXml(_xmlWriter);

				_xmlWriter.WriteEndElement();
				_xmlWriter.WriteEndDocument();

				_xmlWriter.Flush();
				_xmlWriter.Close();

				CallOnSaveProcess(Locale.status_save_ok);
			}
		}

		#endregion

		/// <summary>
		/// Установка параметров сканирования трассы
		/// </summary>
		/// <param name="_trace"></param>
		public void SetupTraceScan(BaseTrace pTrace)
		{
			IRPU _rpu = null;

			if (pTrace.ScanParams != null)
				_rpu = pTrace.ScanParams.RPU;

			if (_rpu == null)
				_rpu = RPUManager.UserSelectAvailableRPU();

			if (_rpu != null)
			{
				if (_rpu.SetupScanParams(pTrace))
					pTrace.ScanMode = ETraceScanMode.Scan;
			}
		}

		public bool IsConfigured
		{
			get
			{
				return (RPUManager.RPUDevices.Count > 0 && BaseAntenna.AntennaList.Count > 0);
			}
		}

		/// <summary>
		/// Проверка конфигурации системы
		/// </summary>
		internal void CheckConfiguration()
		{
			managerRPU.CheckConfiguration();
		}

		public delegate void ConfigurationChangedDelegate();
		public event ConfigurationChangedDelegate OnConfigurationChanged;

		public void CallOnConfigurationChanged()
		{
			if (OnConfigurationChanged != null)
				OnConfigurationChanged();
		}
	}
}
