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
using System.Resources;
using System.IO.Ports;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Xml;
using System.Timers;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using KaorCore.Interfaces;
using KaorCore.Antenna;
using KaorCore.AntennaManager;
using KaorCore.Report;
using KaorCore.RPU;
using KaorCore.Task;
using KaorCore.Trace;
using KaorCore.Signal;
using KaorCore.Utils;

using RPURPV3.Audio;
using RPURPV3.I18N;
using RPURPV3.PowerMeter;

namespace RPURPV3
{
	public partial class CRPURPV3 : BaseRPU
	{

		#region ================ Внутренние поля ================

		SerialPort port;
		int baudRate;
		Int64 baseFreq;

		RPV3PowerMeter powerMeter;

		IAntenna currentAntenna;
		IAntennaManager managerRF1;
		IAntennaManager managerRF2;

		RPV3AudioDemodulator demodulator;

		/// <summary>
		/// Список антенн, подключенных к РПУ
		/// </summary>
		List<IAntenna> antennas = null;

		RPURPV3Control rpuControl;
#if BASE
		bool isBusy;
#endif
		//string portName;
		int portBaud;

		public int PortBaud
		{
			get { return portBaud; }
			set 
			{
				if (port.IsOpen)
				{
					try
					{
						SwitchOff();

						port.Close();
					}

					catch 
					{ 
					}
				}
				portBaud = value;
				CallOnRPUParamsChanged();
			}
		}
#if BASE
		/// <summary>
		/// Доступность приемника
		/// </summary>
		bool isAvailable;

		object cmdLock;

		AutoResetEvent evtStopRecordSignal;

		RPV3Mode mode;
#endif

		bool rf1Disabled;
		bool rf2Disabled;

		Guid rpuType = new Guid("{074D1AF0-BDA8-464f-95A3-D0F38DB16695}");
		#endregion

		#region ================ Конструктор ================
		/// <summary>
		///  Базовый конструктор РПУ РПВ-3
		/// </summary>
		public CRPURPV3()
			: base()
		{
			
			name = Locale.rpv3_name;
			description = Locale.rpv3_descr;
			serial = "000000";

			powerMeter = new RPV3PowerMeter(this);
			demodulator = new RPV3AudioDemodulator(this);
			evtCommandAck = new ManualResetEvent(false);

			TempRx = Marshal.AllocHGlobal(260);
			TempTx = Marshal.AllocHGlobal(260);

			hasDemodulator = true;
			hasPowerMeter = true;
			hasSpectrograph = false;

			port = new SerialPort();//portName, 9600, Parity.None, 8, StopBits.One);

			port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
			port.ReceivedBytesThreshold = 1;

#if BASE
			isBusy = false;

			/// Изначально считаем приемник доступным
			isAvailable = true;

			cmdLock = new object();

			taskProcessors = new List<TaskProcessorItem>();
			currentAntenna = null;

			evtStopRecordSignal = new AutoResetEvent(false);
			mode = RPV3Mode.Off;
#endif
			rpuControl = new RPURPV3Control(this);

			rf1Disabled = true;
			rf2Disabled = true;
		}

		~CRPURPV3()
		{
			
		}

		#region IDisposable Members

		public override void Dispose()
		{
			Marshal.FreeHGlobal(TempRx);
			Marshal.FreeHGlobal(TempTx);

			if (demodulator != null)
				demodulator.Dispose();
		}

		#endregion
		#endregion

		#region ================ Методы РПВ-3 ================

		public bool TestConnection(string pSerialPortName, int pBaudRate)
		{
			bool _res = false;

			/* if (port.IsOpen)
				port.Close();

			port.PortName = pSerialPortName;
			// port.

			try
			{
				port.Open();
				port.BaudRate = pBaudRate;

				port.RtsEnable = true;
				_res = true;
			}

			catch
			{
			}

			finally
			{
				if (port.IsOpen)
					port.Close();
			}
			*/
			return _res;
		}

		/// <summary>
		/// Выбор антенного входа РПУ
		/// </summary>
		/// <param name="pInput"></param>
		/// <returns></returns>
		private bool SelectRFInput(int pInput)
		{
			if (Mode == RPUMode.Off)
				return false; ;

			SendCommand(new RPV_CmdSelectAntenna(pInput));
			return true;
		}

		public override string ToString()
		{
			return Name;
		}
		#endregion
		#region ================ IRPU Members ================

		public override Form SettingsForm
		{
			get 
			{
				RPV3Settings _dlg = new RPV3Settings(this);

				return _dlg;
			}
		}

		public override UserControl RPUControl
		{
			get 
			{
				return rpuControl;
			}
		}

		/// <summary>
		/// Список антенн, подключенных к РПУ
		/// </summary>
		public override List<IAntenna> Antennas
		{
			get 
			{
				if (antennas == null)
				{
					antennas = new List<IAntenna>();
				}

				return antennas;
			}
		}

		void UpdateAntennasList()
		{
			if (antennas == null)
			{
				antennas = new List<IAntenna>();
			}

			antennas.Clear();

			if (managerRF1 != null)
				antennas.AddRange(managerRF1.Antennas);

			if (managerRF2 != null)
				antennas.AddRange(managerRF2.Antennas);
		}

		/// <summary>
		/// Переключение на указанную антенну
		/// </summary>
		/// <param name="pAntenna"></param>
		/// <returns></returns>
		public override bool SwitchAntenna(IAntenna pAntenna)
		{
			if (pAntenna == null)
				return false;

			if (Mode == RPUMode.Off)
				return false;

			lock (cmdLock)
			{

				if (managerRF1 != null)
				{
					if (managerRF1.Antennas.Contains(pAntenna))
					{
						/// Выбор первого антенного входа РПУ
						SelectRFInput(1);

						managerRF1.SelectAntenna(pAntenna);

						currentAntenna = pAntenna;

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

						currentAntenna = pAntenna;

						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Настройка аттенюаторов приемника
		/// </summary>
		/// <param name="HFAtt">
		/// Ослабление входного ВЧ сигнала (0..31 дБ, 127 - выключить)
		/// </param>
		/// <param name="MFAtt">
		/// Ослабление ПЧ сигнала перед измерителем мощности (0..63 *0,5дБ, 128 - автоматически)
		/// </param>
		public void SetAttenuation(Int32 HFAtt, Int32 MFAtt)
		{
			int _hfAtt;
			int _mfAtt;

			if (Mode == RPUMode.Off)
				return;

			if (HFAtt == 0)
				_hfAtt = 255;
			else
				_hfAtt = HFAtt + 128;

			/// Аттенюятор ПЧ управляется блоком АРУ
			_mfAtt = 128;

			SendCommand(new RPV_CmdSetAttenuator((Byte)_hfAtt, (Byte)_mfAtt));
		}

		/// <summary>
		/// Выбор антенного входа приемника
		/// </summary>
		/// <param name="AntInput">Вход: 1 либо 2</param>
		public void SelectAntenna(Int32 AntInput)
		{
			if (Mode == RPUMode.Off)
				return;

			SendCommand(new RPV_CmdSelectAntenna(AntInput));
		}

		public override void SwitchOn()
		{
			lock (cmdLock)
			{
				try
				{
					port.BaudRate = 9600;
					port.Open();
					port.DiscardInBuffer();
					port.DiscardOutBuffer();

					port.RtsEnable = true;

					Console.OpenStandardOutput();
					//RxTimer.Elapsed += new ElapsedEventHandler(RxTimer_Elapsed);

					RxBuffBytes = 0;
					RxPktSize = -1;
					//RxTimer.Start();
					Thread.Sleep(500);
					port.RtsEnable = false;
					Thread.Sleep(2000);

					SendCommand(new RPV_CmdSetPortSpeed(portBaud));
					//SendCommand(new RPV_CmdSetPortSpeed(port.BaudRate));
					//port.BaudRate = 57600;
					//port.BaudRate = portBaud;

					port.DiscardInBuffer();
					port.DiscardOutBuffer();
					
					port.BaudRate = portBaud;

					SendCommand(new RPV_CmdOnOff(true));
					Mode = RPUMode.Free;


					demodulator.AudioVolume = 20;
					demodulator.SetNoiseThreshold(-128);
					SetAttenuation(127, 128);
					BaseFreq = 144000000;
					demodulator.CurrentModulation = EAudioModulationType.FM;
					demodulator.FilterBand = 6000;
					powerMeter.FilterBand = 100000;
					powerMeter.AverageTime = 50;

					try
					{
						if (rf1Disabled == false && managerRF1 != null)
							managerRF1.SwitchOn();
					}
					catch
					{
						foreach (IAntenna _a in managerRF1.Antennas)
						{
							_a.State = EAntennaState.FAULT;
						}
					}

					try
					{
						if (rf2Disabled == false && managerRF2 != null)
							managerRF2.SwitchOn();
					}
					catch
					{
						foreach (IAntenna _a in managerRF2.Antennas)
						{
							_a.State = EAntennaState.FAULT;
						}
					}
				}
				catch
				{
					if (port.IsOpen)
						port.Close();

					isAvailable = false;
					throw;
				}
			}
		}

		public override void SwitchOff()
		{
			lock (cmdLock)
			{
				port.RtsEnable = true;
				//RxTimer.Stop();
				RxBuffBytes = 0;
				RxPktSize = -1;
				//port.DataReceived -= port_DataReceived;
				//port.DiscardInBuffer();
				port.Close();

				Mode = RPUMode.Off;

				if (demodulator != null)
					demodulator.Dispose();

				if (rf1Disabled == false && managerRF1 != null)
					managerRF1.SwitchOff();

				if (rf2Disabled == false && managerRF2 != null)
					managerRF2.SwitchOff();
				
			}
		}

		public override System.Windows.Forms.UserControl statusControl
		{
			get { throw new NotImplementedException(); }
		}

	
		public override IAudioDemodulator Demodulator
		{
			get 
			{
				return demodulator;
			}
		}

		public override IPowerMeter PowerMeter
		{
			get 
			{ 
				return powerMeter; 
			}
		}

		public override ISpectrograph Spectrograph
		{
			get 
			{ 
				throw new NotImplementedException(); 
			}
		}

		internal void SetFrequency(long pFreq)
		{
			if (Mode == RPUMode.Off)
				return;

			lock (cmdLock)
			{
				baseFreq = pFreq;
				unchecked
				{
					UInt32 _frq = (UInt32)pFreq;
					SendCommand(new RPV_CmdSetFreq(_frq));
				}
			}
			if (OnBaseFrequencyChanged != null)
				OnBaseFrequencyChanged(baseFreq);
		}

		public override long BaseFreq
		{
			get
			{
				return baseFreq;
			}
			set
			{
				/// Установка значения частоты через пропертю возможно только если 
				/// приемник свободен
				if (Mode == RPUMode.Free)
					SetFrequency(value);
			}
		}

		#endregion

		#region IRPU Members

		/// <summary>
		/// Загрузка параметров РПУ из XML-описания
		/// </summary>
		/// <param name="pNode"></param>
		public override void LoadFromXmlNode(XmlNode pNode)
		{
			int _baud = 57600;

			if (pNode == null || pNode.Name != "RPU" ||
				pNode.Attributes["type"].Value != this.GetType().AssemblyQualifiedName)
				throw new ArgumentException("Некорректное значение типа нода или типа РПУ");

			id = new Guid(pNode.Attributes["id"].Value);

			if (pNode.Attributes["disabled"] != null)
				if (!bool.TryParse(pNode.Attributes["disabled"].Value, out isDisabled))
					isDisabled = true;

			XmlNode _node;

			_node = pNode.SelectSingleNode("Serial");

			/// Загрузка параметров последовательного порта из XML
			if (_node != null)
			{
				if (_node.Attributes["port"] != null)
					port.PortName = _node.Attributes["port"].Value;

				if (_node.Attributes["baudrate"] != null)
					int.TryParse(_node.Attributes["baudrate"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out portBaud);

				port.BaudRate = portBaud;
			}
			
			/// Антенный вход №1 РПУ
			/// 
			try
			{
				managerRF1 = LoadAntennaManager(pNode, "RFInput1", out rf1Disabled);
				
				if(managerRF1 != null)
					managerRF1.OnAntennaChanged += new AntennaChangedDelegate(managerRF_OnAntennaChanged);

			}
			catch
			{
				/// RFInput1 не заполнен
				/// 
				rf1Disabled = true;
			}
			
			/// Антенный вход №2 РПУ
			/// 
			try
			{
				managerRF2 = LoadAntennaManager(pNode, "RFInput2", out rf2Disabled);

				if(managerRF2 != null)
					managerRF2.OnAntennaChanged += new AntennaChangedDelegate(managerRF_OnAntennaChanged);
			}
			catch
			{
				/// RFInput2 не заполнен
				/// 
				rf2Disabled = true;
			}

			UpdateAntennasList();

			/// Вызов событи изменения параметров РПУ
			/// 
			CallOnRPUParamsChanged();
		}

		void managerRF_OnAntennaChanged(IAntenna pAntenna)
		{
			CallOnRPUParamsChanged();
		}

		#endregion

		public string PortName
		{
			get
			{
				return port.PortName;
			}

			set
			{
				if (port.IsOpen)
				{
					try
					{
						SwitchOff();
					}

					catch
					{
					}

					port.Close();
				}

				port.PortName = value;

				CallOnRPUParamsChanged();
			}
		}

		IAntennaManager LoadAntennaManager(XmlNode pNode, string pAntennaManagerName, out bool pDisabled)
		{
			XmlNode _node;
			
			pDisabled = true;

			_node = pNode.SelectSingleNode(pAntennaManagerName);

			if (_node == null)
				throw new Exception();

			if (_node.Attributes["disabled"] != null)
				pDisabled = bool.Parse(_node.Attributes["disabled"].Value);

			XmlNode _managerNode = _node.SelectSingleNode("AntennaManager");

			if (_managerNode == null)
				throw new Exception();

			if (_managerNode.Attributes["type"] == null)
				throw new Exception();

			string _typeName = _managerNode.Attributes["type"].Value;

			Type _t = Type.GetType(_typeName);

			object _o = Activator.CreateInstance(_t);

			IAntennaManager _antennaManager = _o as IAntennaManager;

			if (_antennaManager == null)
				throw new Exception();

			_antennaManager.LoadFromXmlNode(_managerNode);

			return _antennaManager;
		}

		#region IRPU Members


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

		
		#endregion

		public override event BaseFrequencyChanged OnBaseFrequencyChanged;

		#region ================ Процессор запросов ================

		int init = 0;
		protected override double MeasurePowerTask(TaskMeasurePower pTask)
		{
			double _power;
			/// Переключение антенны
			if (currentAntenna != pTask.ScanParams.Antenna)
			{
#if DEBUG
				Console.WriteLine("Antenna switch to {0}", pTask.ScanParams.Antenna.Name);
#endif
				SwitchAntenna(pTask.ScanParams.Antenna);
			}

			/// Установка параметров измерителя мощности из параметров задачи на измерение
			/// 
			if (init == 0 || powerMeter.FilterBand != pTask.ScanParams.FilterBand ||
				powerMeter.AverageTime != pTask.ScanParams.AverageTime)
			{
				powerMeter.FilterBand = pTask.ScanParams.FilterBand;
				powerMeter.AverageTime = pTask.ScanParams.AverageTime;
				powerMeter.MeasurePower();

				init++;
			}

			_power = powerMeter.MeasurePower(pTask.Frequency);

			return _power;
		}
		#endregion


		#region IRPU Members

		public KaorCore.Task.ITaskProvider ScanTraceTaskProvider(KaorCore.Trace.BaseTrace pTrace)
		{
			throw new NotImplementedException();
		}

		#endregion


		#region IRPU Members


		

		public override object ShowRecordParamsDialog(object pRecordParams)
		{
			RPV3RecordSignalParams _recParams;

			_recParams = pRecordParams as RPV3RecordSignalParams;

			if (_recParams == null)
				_recParams = new RPV3RecordSignalParams();

			RPV3RecordParamsDialog _dlg = new RPV3RecordParamsDialog(_recParams, this);

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
			}

			return _recParams;
				
		}

		/// <summary>
		/// Запись сигнала
		/// </summary>
		/// <param name="pSignal"></param>
		/// <param name="pRecordTime"></param>
		/// <param name="pRecordParams"></param>
		/// <returns></returns>
		public override void StartRecordSignal(BaseSignal pSignal,
			object pRecordParams, NewRecordInfoDelegate pNewRecordInfo, BaseTrace pTrace)
		{
			RPV3RecordSignalParams _params = pRecordParams as RPV3RecordSignalParams;
			if (_params == null)
				return;

			RecordSignalMainDelegate _recMain = new RecordSignalMainDelegate(RecordSignalMain);
			_recMain.Invoke(pSignal, _params, pNewRecordInfo, pTrace);

		}

		public override string StopRecordSignal(object pRecordParams)
		{
			RPV3RecordSignalParams _params = pRecordParams as RPV3RecordSignalParams;
			if (_params == null)
				return "";
			string _res = "";

//			evtStopRecordSignal.Set();

			/// Отключение сохранялки
			_res = demodulator.Stop();

			return _res;
		}

		delegate void RecordSignalMainDelegate(BaseSignal pSignal, 
			RPV3RecordSignalParams pRecordParams, 
			NewRecordInfoDelegate pNewRecordInfo, BaseTrace pTrace);
		/// <summary>
		/// Основное тело записи
		/// </summary>
		/// 
		NewRecordInfoDelegate recInfoDelegate;
		AudioRecordInfo recInfo;

		void RecordSignalMain(BaseSignal pSignal, RPV3RecordSignalParams pRecordParams, 
			NewRecordInfoDelegate pNewRecordInfo, BaseTrace pScanTrace)
		{
			recInfo = new AudioRecordInfo();

			recInfoDelegate = pNewRecordInfo;

			BaseFreq = pSignal.Frequency;

			demodulator.FilterBand = pRecordParams.FilterBand;
			demodulator.CurrentModulation = pRecordParams.Modulation;

			if (pRecordParams.NeedSave)
			{
				/// Если надо сохранять запись, то вклчюаем сохранялку
//				demodulator.OnNewAudioRecord += new NewAudioRecord(demodulator_OnNewAudioRecord);

				/// Сохранение записи сигнала
				demodulator.Start(FileSystem.GetTempFileName());
			}

#if false
			if (pRecordParams.RecordTime != -1)
			{
				evtStopRecordSignal.WaitOne(pRecordParams.RecordTime * 1000, true);

				//Thread.Sleep(pRecordParams.RecordTime * 1000);

				if (pRecordParams.NeedSave)
				{
					/// Отключение сохранялки
					demodulator.Stop();
				}
			}
#endif
		}

#if false
		void demodulator_OnNewAudioRecord(string pRecord)
		{
			demodulator.OnNewAudioRecord -= demodulator_OnNewAudioRecord;

			string _reportName = ReportsManager.NewReportName(ReportType.SignalAudio, pSignal.Name);

			///recInfoDelegate.Invoke(recInfo);
		}
#endif
		public override object DefaultSignalRecordParams
		{
			get
			{
				return new RPV3RecordSignalParams();
			}
		}
		#endregion

		#region IRPU Members


		public override void SetParamsFromSignal(BaseSignal signalChart)
		{
			RPV3RecordSignalParams _recParams = signalChart.SignalParams as RPV3RecordSignalParams;

			if (_recParams == null)
				return;

			BaseFreq = signalChart.Frequency;
			demodulator.CurrentModulation = _recParams.Modulation;
			demodulator.FilterBand = _recParams.FilterBand;
		}

		#endregion

		#region =============== Режимы работы ===============
#if BASE
		public RPV3Mode Mode
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

		public delegate void ModeChangedDelegate(RPV3Mode pMode);

		public event ModeChangedDelegate OnModeChanged;

		public void CallOnModeChanged(RPV3Mode pMode)
		{
			if (OnModeChanged != null)
				OnModeChanged(pMode);
		}
#endif

		#endregion

		public delegate void RPUParamsChangedDelegate(CRPURPV3 pRPU);

		public event RPUParamsChangedDelegate OnRPUParamsChanged;

		internal void CallOnRPUParamsChanged()
		{
			if (OnRPUParamsChanged != null)
				OnRPUParamsChanged(this);
		}

		public override bool SetupScanParams(BaseTrace pTrace)
		{
			bool _res = false;
			Trace.SetupScanParams _dlg = new RPURPV3.Trace.SetupScanParams();
			TraceScanParams _scanParams = pTrace.ScanParams;

			if (_scanParams == null)
			{
				_scanParams = new TraceScanParams(this);
			}

			_dlg.Trace = pTrace;
			_dlg.ScanParams = _scanParams;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				pTrace.ScanParams = _scanParams;
				_res = true;
			}
			else
			{
				//pTrace.ScanParams = null;
			}

			return _res;
		}

		/// <summary>
		/// Сохранение параметров РПУ в XML
		/// </summary>
		/// <param name="pWriter"></param>
		public override void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("RFInput1");
			pWriter.WriteAttributeString("disabled", rf1Disabled.ToString(CultureInfo.InvariantCulture));

			if (!rf1Disabled)
			{
				if (managerRF1 != null)
					managerRF1.SaveToXmlWriter(pWriter);
			}

			pWriter.WriteEndElement();

			pWriter.WriteStartElement("RFInput2");
			pWriter.WriteAttributeString("disabled", rf2Disabled.ToString(CultureInfo.InvariantCulture));

			if (!rf2Disabled)
			{
				if (managerRF2 != null)
					managerRF2.SaveToXmlWriter(pWriter);
			}

			pWriter.WriteEndElement();

			pWriter.WriteStartElement("Serial");
			pWriter.WriteAttributeString("port", port.PortName);
			pWriter.WriteAttributeString("baudrate", port.BaudRate.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteEndElement();
		}


		public IAntennaManager ManagerRF1
		{
			get
			{
				return managerRF1;
			}

			set
			{
				managerRF1 = value;
			}
		}

		public IAntennaManager ManagerRF2
		{
			get
			{
				return managerRF2;
			}

			set
			{
				managerRF2 = value;
			}
		}

		public bool RF1Disabled
		{
			get { return rf1Disabled; }
			set { rf1Disabled = value; }
		}

		public bool RF2Disabled
		{
			get { return rf2Disabled; }
			set { rf2Disabled = value; }
		}

		#region IRPU Members


		public override void ShowStartSplash()
		{
#if !DEBUG
			rpuControl.ShowStartSplash();
#endif
		}

		#endregion

		/// <summary>
		/// Guid типа РПУ
		/// </summary>
		public override Guid RPUType
		{
			get 
			{
				return rpuType;
			}
		}

		/// <summary>
		/// Проверка конфигурации РПУ
		/// </summary>
		public override void CheckConfiguration()
		{
			if (rf1Disabled == false)
			{
				if(managerRF1 == null)
					throw new Exception(Locale.err_no_managerRF1);

				managerRF1.CheckConfiguration();
			}

			if(rf2Disabled == false)
			{
				if(managerRF2 == null)
					throw new Exception(Locale.err_no_managerRF2);

				managerRF2.CheckConfiguration();
			}
		}

		public static string ClassName
		{
			get { return Locale.rpv3_name; }
		}

		public static bool InternalUseOnly
		{
			get { return false; }
		}

	}
}
