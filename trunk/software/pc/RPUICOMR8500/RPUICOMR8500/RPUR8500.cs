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
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;

using KaorCore.RPU;
using KaorCore.Task;
using KaorCore.Trace;
using KaorCore.AntennaManager;
using KaorCore.Signal;
using KaorCore.Utils;

using RPUICOMR8500.I18N;
using RPUICOMR8500.PowerMeter;
using RPUICOMR8500.Demodulator;
using RPUICOMR8500.Audio;

namespace RPUICOMR8500
{
    public class RPUR8500 : BaseRPU
    {
        #region   ====Поля====

        SerialPort port;  //СОМ-порт
        public object locking;  //объект для lock
        AutoResetEvent evtAnswerRecv;   //событие приема данных
        R8500Control control;       //контрол управленя приемником
        R8500Demodulator demo;  
        R8500PowerMeter powerMeter;
		IAntennaManager antennaManager;
                
        byte[] outBuffer; 
        IAntenna currentAntenna;

        #endregion

        const byte adress = 0x4A; //адрес приемника

        /// <summary>
        /// псевдоантенна
        /// </summary>
        public KaorCore.Antenna.BaseAntenna antenna;
		List<IAntenna> antennas; //список доступных антенн

		Guid rpuType = new Guid("{7E3CD8B9-AAED-4fdd-A9A8-ED2E683210DF}");

        /// <summary>
        /// Создает новый экземпляр класса
        /// </summary>
        public RPUR8500()
			: base()
        {
			name = Locale.icom_name;
			description = Locale.icom_descr;

            port = new SerialPort();
            PortReadTimeout = 2000; 
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            locking = new object();            
            isAvailable = true;
            evtAnswerRecv = new AutoResetEvent(false);

			////antenna = new KaorCore.Antenna.BaseAntenna(new KaorCore.Base.GPSCoordinates(
			////    0, 0, 
			////    0, false));

            powerMeter = new R8500PowerMeter(this);
            demo = new R8500Demodulator(this);
			antennas = new List<IAntenna>();
            

			freqMin = 100000;
			freqMax = 2000000000;
			hasDemodulator = true;
			hasPowerMeter = true;
			hasSpectrograph = false;

			
			Mode = RPUMode.Off;
			control = new R8500Control(this);

        }

        /// <summary>
        /// Отправка команды приемнику
        /// </summary>
        /// <param name="buffer">команда для отправки</param>
        /// <returns></returns>
        public byte[] SendCommand(byte[] pBuffer)
        {
			lock (port)
			{
				int _trycount = 0;

				if (!port.IsOpen)
					return null;

				while (_trycount < 3)
				{
					/// Передача команды приемнику
					port.Write(pBuffer, 0, pBuffer.Length);

					/// Ожидание ответа
					if (evtAnswerRecv.WaitOne(1000, true))
						break;

					_trycount++;
				}

				if(_trycount == 3)
					throw new InvalidOperationException("Receive timeout");

				if (outBuffer[2] == 0xfa)
					throw new InvalidOperationException("NG received!");

                return outBuffer;
            }
        }        

        /// <summary>
        /// конвертирует данные из формата приемника
        /// в целое число
        /// </summary>
        /// <param name="buffer">пакет данных</param>
        /// <param name="offset">начало данных в пакете</param>
        /// <returns></returns>
        public int BCDtoInt(byte[] pBuffer, int pOffset)
        {
            int _return = 0;
            for (int _i = pOffset; _i < pBuffer.Length; _i++)
            {
                _return += pBuffer[pBuffer.Length - 1 - _i + pOffset] % 16 * (int)Math.Pow(10, (_i - pOffset) * 2);
                _return += pBuffer[pBuffer.Length - 1 - _i + pOffset] / 16 * 10 * (int)Math.Pow(10, (_i - pOffset) * 2);
            }
            return _return;
            
        }

        //буферы приема
		byte[] tmpBuf = new byte[4096];
		byte[] tmpRecvBuf = new byte[4096];

        //длина пакета (только адреса и данные)
		int recvLen;

		enum EReceiverState
		{
			IDLE, //пакет еще не начался
			START, //пакет начался (было 0xFE)
			DATA  //область данных в пакете
		}
        /// <summary>
        /// текущее состояние обработки пакета
        /// </summary>
		EReceiverState recvState;
 
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int _count = port.Read(tmpBuf, 0, port.BytesToRead); //считиываем то что в порту
            for (int _i = 0; _i < _count; _i++) //обрабатываем каждый байт
            {
                byte _b = tmpBuf[_i];
                switch (recvState)
                {
                    case EReceiverState.IDLE:
                        if (_b == 0xfe) 
                            recvState = EReceiverState.START;
                        break;

                    case EReceiverState.START:
                        if (_b == 0xfe) //пакет начался
                        {
                            recvState = EReceiverState.DATA;
                            recvLen = 0;
                        }
                        else
                            recvState = EReceiverState.IDLE;
                        break;

                    case EReceiverState.DATA:
                        if (_b == 0xfd) //пакет закончился
                        {
                            outBuffer = new byte[recvLen];
                            Array.Copy(tmpRecvBuf, outBuffer, recvLen);
                            ProcessCurrentPack();
                            recvState = EReceiverState.IDLE;
                        }
                        else
                            tmpRecvBuf[recvLen++] = _b;
                        break;

                    default:
                        throw new InvalidOperationException("Ошибочное состояние цикла приема!");


                }
            }
        }

		/// <summary>
        /// обрабатывает полностью принятый пакет
        /// </summary>
        void ProcessCurrentPack()
        {
            if (outBuffer[0] == adress && outBuffer[1] == 0xE0) //from PC
            {

            }
            else if (outBuffer[1] == adress)
            {
				if (outBuffer[0] == 0xE0)
				{
					if (outBuffer[2] == 0xFA) //ошибка
					{
					}
					else if (outBuffer[2] == 0xFB)   //OK
					{
					}
					else
					{

					}
					//устанавливаем событие приема данных
					evtAnswerRecv.Set();
				}
            }
            else
                throw new Exception("Неверные адреса");
        }

        /// <summary>
        /// время ожидания данных
        /// </summary>
        public int PortReadTimeout
        {
            get
            {
                return port.ReadTimeout;
            }
            set
            {
                port.ReadTimeout = value;
            }
        }
        //возможные состояния аттенуатора
        public enum EAttenuator : byte
        {
            dB10 = 0x10,
            dB20 = 0x20,
            dB30 = 0x30,
            OFF = 0x00
        }

        //переключить аттенуатор
        public void Attenuator(EAttenuator pAtten)
        {            
            byte[] _buffer = { 0xFE, 0xFE, adress, 0xE0, 0x11, (byte)pAtten, 0xFD };
            SendCommand(_buffer);
        }

        #region IRPU Members


        public override System.Windows.Forms.Form SettingsForm
        {
            get 
			{
				R8500Settings _dlg = new R8500Settings(this);

				return _dlg;
			}
        }

        /// <summary>
        /// Контрол для управления приемником
        /// </summary>
		public override System.Windows.Forms.UserControl RPUControl
        {
            get
            {
                return control;
            }
        }
        /// <summary>
        /// Power ON (from sleep active)
        /// </summary>
		public override void SwitchOn()
        {            
            //команда "вкл"
            byte[] _buffer = { 0xFE, 0xFE, adress, 0xE0, 0x18, 0x01, 0xFD };
			byte[] _agcBuf = { 0xfe, 0xfe, adress, 0xe0, 0x16, 0x11, 0xfd };
			byte[] _nbBuf = { 0xfe, 0xfe, adress, 0xe0, 0x16, 0x20, 0xfd };
			byte[] _apfBuf = { 0xfe, 0xfe, adress, 0xe0, 0x16, 0x30, 0xfd };
            byte[] _answer;
            try
            {
                if (port.IsOpen)
                    port.Close();

                port.Open(); 
				port.DiscardInBuffer();
				port.DiscardOutBuffer();
                port.RtsEnable = false;
                _answer = this.SendCommand(_buffer);
				
				//_answer = SendCommand(_nbBuf);
				//_answer = SendCommand(_apfBuf);

				BaseFreq = 145000000;
				powerMeter.FilterBand = 12000;
				
				Mode = RPUMode.Free;

				demo.CurrentModulation = EAudioModulationType.AM;
				_answer = SendCommand(_agcBuf);

				if (antennaManager != null)
					antennaManager.SwitchOn();

				/// Измеритель мощности
				/// 
				powerMeter.StartThread();
            }
            catch 
			{
				isAvailable = false;

				if (port.IsOpen)
					port.Close();

				throw;
			}
        }
        /// <summary>
        /// Power OFF (activating sleep)
        /// </summary>
		public override void SwitchOff()
        {
			lock (port)
			{
				powerMeter.Stop();
				powerMeter.StopThread();

				byte[] buffer = { 0xFE, 0xFE, adress, 0xE0, 0x18, 0x00, 0xFD };
				byte[] answer = this.SendCommand(buffer);

				port.Close();

				Mode = RPUMode.Off;

				if (antennaManager != null)
					antennaManager.SwitchOff();
			}
        }

        /// <summary>
        /// Gets demodulator
        /// </summary>
        public override IAudioDemodulator Demodulator
        {
            get 
			{ 
				return demo; 
			}
        }
        /// <summary>
        /// Gets Power meter
        /// </summary>
        public override IPowerMeter PowerMeter
        {
            get { return powerMeter; }
        }
        /// <summary>
        /// NotImplemented
        /// </summary>
        public override ISpectrograph Spectrograph
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// NotImplemented
        /// </summary>
        public override System.Windows.Forms.UserControl statusControl
        {
            get { throw new NotImplementedException(); }
        }

        long baseFreq;
        /// <summary>
        /// текущая частота
        /// </summary>
        public override long BaseFreq
        {
            get
            {
                return baseFreq;
            }
            set
            {
                baseFreq = value;

				if (baseFreq < 100000)
					baseFreq = 100000;
				else if (baseFreq > 1999999999)
					baseFreq = 1999999999;

                //устанавливаем частоту на приемнике
                byte[] buffer = new byte[11];
                buffer[0] = 0xFE;
                buffer[1] = 0xFE;
                //
                buffer[2] = adress;  //адрес
                buffer[3] = 0xE0;
                buffer[4] = 0x05;  
                
                //IntToBCD
                int _count = 5; //кол-во байтов (номер следующего пустого байта для 0xFD)
                for (int i = 0; i < 5; i++) //преобразуем частоту в формат приемника
                {                    
                    byte c = (byte)(baseFreq % Math.Pow(10, 2 * i + 2) / Math.Pow(10, 2 * i + 1));
                    byte b = (byte)(baseFreq % Math.Pow(10, 2 * i + 1) / Math.Pow(10, 2 * i));
                    byte a = (byte)(c * 16 + b); 
                    buffer[i + 5] = a;
                    _count++;                   
                }
                buffer[_count] = 0xFD;
                byte[] answer = this.SendCommand(buffer);
                RaiseBaseFrequencyChanged(baseFreq);
            }
        }
        /// <summary>
        /// NotImplemented
        /// </summary>
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

        /// <summary>
        /// Загружает класс из XML node
        /// </summary>
        /// <param name="pNode"></param>
        public override void LoadFromXmlNode(System.Xml.XmlNode pNode)
        {
            //если нода подходит
            if (pNode == null || pNode.Name != "RPU" ||
                pNode.Attributes["type"].Value != this.GetType().AssemblyQualifiedName)
                throw new ArgumentException("Некорректное значение типа нода или типа РПУ");
            //считываем id
            id = new Guid(pNode.Attributes["id"].Value);

			if (pNode.Attributes["disabled"].Value != null)
				if (!bool.TryParse(pNode.Attributes["disabled"].Value, out isDisabled))
					isDisabled = true;

			//if (pNode.Attributes["calibration"] != null)
			{
				XmlDocument _doc = new System.Xml.XmlDocument();

				//string _path = pNode.Attributes["calibration"].Value;
                string _path = Application.StartupPath + "\\ICOMR8500.xml";

				try  //загрузка калибровочных таблиц
				{
					_doc.Load(_path);
					if (this.PowerMeter as RPUICOMR8500.PowerMeter.R8500PowerMeter != null)
						(this.PowerMeter as RPUICOMR8500.PowerMeter.R8500PowerMeter).LoadCalibrationFromXML(_doc);
				}
				catch 
				{ 
				}
			}
            foreach (System.Xml.XmlNode xn in pNode.ChildNodes)
            {
                //хар-ки порта
                if (xn.Name == "Serial")
                {
                    if (port != null)
                    {
                        //имя порта
                        if (xn.Attributes["port"] != null)
                            port.PortName = xn.Attributes["port"].Value;
                        if (xn.Attributes["baudrate"] != null)
                        {
                            int baudrate;
							if (int.TryParse(xn.Attributes["baudrate"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out  baudrate))
                            {
                                port.BaudRate = baudrate;
                            }
                            else
                            {
                                throw new InvalidOperationException("Error parsing baudrate!");
                            }
                        }
                    }
                }
                else if (xn.Name == "RFInput") //Antenna manager
                {
                    foreach (System.Xml.XmlNode chn in xn.ChildNodes)
                    {
                        System.Xml.XmlNode _managerNode = xn.SelectSingleNode("AntennaManager");

                        if (_managerNode == null)
                            throw new Exception();

                        if (_managerNode.Attributes["type"] == null)
                            throw new Exception();

                        string _typeName = _managerNode.Attributes["type"].Value;

                        Type _t = Type.GetType(_typeName);

                        object _o = Activator.CreateInstance(_t);

                        antennaManager = _o as IAntennaManager;

                        if (antennaManager == null)
                            throw new Exception();

                        antennaManager.LoadFromXmlNode(_managerNode);

						if(antennaManager != null)
							antennaManager.OnAntennaChanged += new AntennaChangedDelegate(antennaManager_OnAntennaChanged);
                    }
                }
            }

			UpdateAntenasList();
			CallOnRPUParamsChanged();
        }

		void antennaManager_OnAntennaChanged(IAntenna pAntenna)
		{
			CallOnRPUParamsChanged();
		}

        
        #region ================ Процессор запросов ================
		int init = 0;
		protected override double MeasurePowerTask(TaskMeasurePower pTask)
		{
			double _power;
			/// Переключение антенны
			if (currentAntenna != pTask.ScanParams.Antenna)
				SwitchAntenna(pTask.ScanParams.Antenna);

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

        
        /// <summary>
        /// Изменена базовая частота
        /// </summary>
        public override event BaseFrequencyChanged OnBaseFrequencyChanged;

        /// <summary>
        /// Вызывает OnBaseFrequencyChanged если он не равен null
        /// (если к нему привязан делегат)
        /// </summary>
        /// <param name="pBaseFreq"></param>
        void RaiseBaseFrequencyChanged(long pBaseFreq)
        {
            if (OnBaseFrequencyChanged != null)
                OnBaseFrequencyChanged(pBaseFreq);
        }

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
					catch { }

					port.Close();
				}

				port.PortName = value;

				CallOnRPUParamsChanged();
			}
		}

		public int PortBaud
		{
			get
			{
				return port.BaudRate;
			}

			set
			{
				if (port.IsOpen)
				{
					try
					{
						SwitchOff();
					}
					catch { }

					port.Close();
				}

				port.BaudRate = value;

				CallOnRPUParamsChanged();
			}
		}

        public delegate void PowerMeasuredEventHandler(double pPower);
        /// <summary>
        /// пришел отчет на измерение мощности
        /// </summary>
        public event PowerMeasuredEventHandler OnPowerMeasured;

        void RaisePowerMeasured(double pPower)
        {
            if (OnPowerMeasured != null)
            {
                OnPowerMeasured(pPower);
            }
        }

        /// <summary>
        /// список антенн приемника
        /// </summary>
		public override List<KaorCore.Antenna.IAntenna> Antennas
		{
			get 
			{
				return antennas;
			}
		}

		public void UpdateAntenasList()
		{
			antennas.Clear();

			if (antennaManager != null)
			{
				antennas.AddRange(antennaManager.Antennas);
			}
		}
		#endregion


		#region IRPU Members


        public override object ShowRecordParamsDialog(object pRecordParams)
        {
			R8500RecordSignalParams _rsp = pRecordParams as R8500RecordSignalParams;
			
			if(_rsp == null)
				_rsp = new R8500RecordSignalParams();

			R8500RecordParamsDialog _dlg =
				new R8500RecordParamsDialog(_rsp, this);

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
			}

			return _rsp;
        }

		public override object DefaultSignalRecordParams
		{
			get 
			{
				return new R8500RecordSignalParams();
			}
		}

		#endregion

		#region IRPU Members


		public override void SetParamsFromSignal(BaseSignal signalChart)
		{
			R8500RecordSignalParams _recParams = signalChart.SignalParams as R8500RecordSignalParams;

			if (_recParams == null)
				return;
			BaseFreq = signalChart.Frequency;
			demo.CurrentModulation = _recParams.Modulation;
		}

		
		public override void StartRecordSignal(BaseSignal pSignal, object pRecordParams, NewRecordInfoDelegate pNewRecordInfo, BaseTrace pScanTrace)
		{
			R8500RecordSignalParams _params = pRecordParams as R8500RecordSignalParams;
			if (_params == null)
				return;

			RecordSignalMainDelegate _recMain = new RecordSignalMainDelegate(RecordSignalMain);
			_recMain.Invoke(pSignal, _params, pNewRecordInfo, pScanTrace);
		}

		delegate void RecordSignalMainDelegate(BaseSignal pSignal,
			R8500RecordSignalParams pRecordParams,
			NewRecordInfoDelegate pNewRecordInfo, BaseTrace pTrace);
		/// <summary>
		/// Основное тело записи
		/// </summary>
		/// 
		NewRecordInfoDelegate recInfoDelegate;
		AudioRecordInfo recInfo;

		void RecordSignalMain(BaseSignal pSignal, R8500RecordSignalParams pRecordParams,
			NewRecordInfoDelegate pNewRecordInfo, BaseTrace pScanTrace)
		{
			recInfo = new AudioRecordInfo();

			recInfoDelegate = pNewRecordInfo;

			BaseFreq = pSignal.Frequency;

			demo.CurrentModulation = pRecordParams.Modulation;

			if (pRecordParams.NeedSave)
			{
				/// Если надо сохранять запись, то вклчюаем сохранялку
				//				demodulator.OnNewAudioRecord += new NewAudioRecord(demodulator_OnNewAudioRecord);

				/// Сохранение записи сигнала
				demo.Start(FileSystem.GetTempFileName());
			}
		}

		public override string StopRecordSignal(object pRecordParams)
		{
			R8500RecordSignalParams _params = pRecordParams as R8500RecordSignalParams;
			if (_params == null)
				return "";
			string _res = "";

			//			evtStopRecordSignal.Set();
			/// Отключение сохранялки
			_res = demo.Stop();


			return _res;
		}
		

		public override void ShowStartSplash()
		{
#if !DEBUG
			Splash.Splash.ShowSplash(0, 750);
			Splash.Splash.Fadeout();
#endif
		}

		#endregion

		#region IDisposable Members

		public override void Dispose()
		{

		}

		#endregion

		#region IRPU Members

		public delegate void RPUParamsChangedDelegate(RPUR8500 pRPU);

		public event RPUParamsChangedDelegate OnRPUParamsChanged;

		void CallOnRPUParamsChanged()
		{
			if (OnRPUParamsChanged != null)
				OnRPUParamsChanged(this);
		}

		public override bool SwitchAntenna(IAntenna pAntenna)
		{
			if (pAntenna == null)
				return false;

			if (Mode == RPUMode.Off)
				return false;

			lock (cmdLock)
			{
				if (antennaManager != null)
				{
					if (antennaManager.Antennas.Contains(pAntenna))
					{
						antennaManager.SelectAntenna(pAntenna);

						currentAntenna = pAntenna;

						return true;
					}
				}
			}

			return false;
		}

		public override bool SetupScanParams(BaseTrace pTrace)
		{
			bool _res = false;
			Trace.SetupScanParams _dlg = new Trace.SetupScanParams();
			TraceScanParams _scanParams = pTrace.ScanParams;

			if (_scanParams == null)
			{
				_scanParams = new TraceScanParams(this);
			}

			_dlg.Trace = pTrace;
			_dlg.ScanParams = _scanParams;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				_scanParams.AverageTime = 1;
				pTrace.ScanParams = _scanParams;
				_res = true;
			}
			else
			{
				//pTrace.ScanParams = null;
			}

			return _res;
		}

		#endregion

		public override void SaveToXmlWriter(System.Xml.XmlWriter pWriter)
		{
			pWriter.WriteStartElement("RFInput");
			if (antennaManager != null)
			{
				antennaManager.SaveToXmlWriter(pWriter);
			}
			pWriter.WriteEndElement();

			pWriter.WriteStartElement("Serial");
			pWriter.WriteAttributeString("port", port.PortName);
			pWriter.WriteAttributeString("baudrate", port.BaudRate.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteEndElement();

		}

		public IAntennaManager ManagerRF
		{
			get
			{
				return antennaManager;
			}

			set
			{
				antennaManager = value;
			}
		}

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

		public override void CheckConfiguration()
		{
			if (antennaManager == null)
				throw new Exception(Locale.err_invalid_manager);

			antennaManager.CheckConfiguration();
		}

		public static string ClassName
		{
			get { return Locale.icom_name; }
		}

		public static bool InternalUseOnly
		{
			get { return false; }
		}
	}
}
