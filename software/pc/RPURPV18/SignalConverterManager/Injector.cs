// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//			Maxim Anisenkov <anisenkov@niistt.ru>
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

using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RPURPV18.SignalConverterManager
{
	/// <summary>
	/// Инжектор, обеспечивающий доступ к конвертеру
	/// </summary>
	public class Injector
	{
		private object sync = new object();
		#region Команды и ответы инжектора
		private const byte CMD_GET_STATUS = 0x50;
		private const byte CMD_GET_CONFIG = 0x51;
		private const byte CMD_SWITCH_ON = 0x52;
		private const byte CMD_SWITCH_OFF = 0x53;
		private const byte CMD_SET_FREQ = 0x54;
		private const byte CMD_SET_BASE = 0x56;

		private const byte RESP_STATUS = 0x58;
		private const byte RESP_CONFIG = 0x59;
		private const byte RESP_CORRECT = 0x5A;
		private const byte RESP_INVALID_COMMAND = 0x5B;
		private const byte RESP_HARDWARE_ERROR = 0x5C;
		private const byte RESP_POWER_ERROR = 0x5D;
		#endregion

		#region Порт
		private const byte START_PREFIX = 0xAA;

		private SerialPort port = new SerialPort();	// Последовательный порт

		public string PortName
		{
			get { return port.PortName; }
			set
			{
				lock (sync)
				{
					if (port.IsOpen)
					{
						port.Close();
						//port.PortName = value;
						//port.Open();
					}
					port.PortName = value;
				}
			}
		}

		/// <summary>
		/// Оформляет пакет и пишет его в порт
		/// </summary>
		/// <param name="data">Буфер</param>
		/// <param name="offset">Смещение</param>
		/// <param name="length">Длина</param>
		private void write(byte[] data, int offset, int length)
		{
			lock (sync)
			{
				if (!port.IsOpen)
					port.Open();
				byte[] _buf = new byte[length + 3];
				_buf[0] = START_PREFIX;
				_buf[1] = (byte)length;
				byte crc = (byte)length;
				for (int i = 0; i < length; i++)
					crc += (_buf[2 + i] = data[offset + i]);
				_buf[length + 2] = crc;
				port.Write(_buf, 0, length + 3);
			}
		}

		/// <summary>
		/// Читает и разбирает пакет
		/// </summary>
		/// <returns>Данные пакета</returns>
		private byte[] readPacket()
		{
			lock (sync)
			{
				if (!port.IsOpen)
					port.Open();
				byte[] _result;
				if (port.ReadByte() != 0xAA)
					throw new IOException(I18N.Locale.invalid_packet);
				int _bCnt = port.ReadByte();
				if (_bCnt < 0)
					throw new IOException(I18N.Locale.invalid_packet);
				_result = new byte[_bCnt];
				byte _crc = (byte)_bCnt;
				try
				{
					port.Read(_result, 0, _bCnt);
				}
				catch (Exception ex)
				{
					throw new IOException(I18N.Locale.invalid_packet, ex);
				}
				for (int i = 0; i < _bCnt; i++)
					_crc += _result[i];
				int _cCRC = port.ReadByte();
				if (_cCRC < 0 || (byte)_cCRC != _crc)
					throw new IOException(I18N.Locale.packet_crc_invalid);
				return _result;
			}
		}
		#endregion

		#region Конструктор
		public Injector()
		{
			port.BaudRate = 57600;
			port.DataBits = 8;
			port.Parity = Parity.None;
			port.StopBits = StopBits.Two;
			port.ReadTimeout = 1500;
			port.WriteTimeout = 1500;
		}
		#endregion

		#region Управление статусом
		private bool started = false;	// Определяет, включен ли инжектор или нет

		/// <summary>
		/// Запущена ли процедура контроля инжектора
		/// </summary>
		public bool Started
		{
			get { return started; }
			private set { started = value; }
		}

		/// <summary>
		/// Запускает контроль состояния инжектора
		/// </summary>
		public void Start()
		{
			lock (sync)
			{
				Started = true;
				getStatusFromDevice();
				PowerOn();
				//if (pingThread == null)
				//    (pingThread = new Thread(new ThreadStart(pingThreadProc))).Start();
			}
		}

		/// <summary>
		/// Останавливает контроль состояния инжектора
		/// </summary>
		public void Stop()
		{
			//if (pingThread != null)
			//    pingThread.Abort();
			//while (pingThread != null) ;	// Ждем завершения потока пинга
			lock (sync)
			{
				PowerOff();
				if (status != null && status.Value.PowerOn)
					PowerOff();
				if (port.IsOpen)
					port.Close();
				Status = null;
				Started = false;
			}
		}
		#endregion

		#region Пинг
		private Thread pingThread = null;	// Пинг инжектора

		private void pingThreadProc()
		{
			try
			{
				while (true)
				{
					lock (sync)
					{
						try
						{
							getStatusFromDevice();
							if (status != null && !status.Value.IsError && !status.Value.PowerOn)
							{
								PowerOn();
								getStatusFromDevice();
							}
							if (status != null && status.Value.IsError && status.Value.PowerOn)
							{
								PowerOff();
								getStatusFromDevice();
							}
						}
						catch (ThreadAbortException ex)
						{
							throw ex;
						}
						catch (Exception)
						{
							port.Close();
							Status = null;
						}
					}
					Thread.Sleep(1000);
				}
			}
			catch (ThreadAbortException)
			{
			}
			finally
			{
				pingThread = null;
			}
		}
		#endregion

		#region Текущее состояние инжектора
		private InjectorStatus? status = null;

		/// <summary>
		/// Текущее состояние инжектора
		/// </summary>
		public InjectorStatus? Status
		{
			get { return status; }
			private set
			{
				status = value;
				StatusChanged(this, null);
			}
		}

		/// <summary>
		/// Проверка подключения
		/// </summary>
		/// <returns>Есть ли подключение?</returns>
		public bool TestConnection()
		{
			try
			{
				byte[] statusCmdBuf = new byte[] { CMD_GET_STATUS };
				byte[] statusRcvBuf;
				write(statusCmdBuf, 0, 1);
				statusRcvBuf = readPacket();
				getStatusFromDevice();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Устанавливает текущий статус 
		private void getStatusFromDevice()
		{
			lock (sync)
			{
				write(new byte[] { CMD_GET_STATUS }, 0, 1);
				byte[] statusRcvBuf = readPacket();
				parseStatus(statusRcvBuf);
				Thread.Sleep(300);
			}
		}

		private void parseStatus(byte[] data)
		{
			InjectorStatus _injStat = new InjectorStatus();
			if (data.Length < 10)
				throw new IndexOutOfRangeException();
			if (data[0] == RESP_STATUS)
			{
				_injStat.PowerOn = ((data[1] & 1) == 1);
				_injStat.InputVoltageOver = ((data[1] & 3) == 3);
				_injStat.CurrentOverload = ((data[1] & 5) == 5);
				_injStat.OutputError = ((data[1] & 9) == 9);
				_injStat.OutputCurrentOver = ((data[1] & 17) == 1);
				_injStat.Frequency = BitConverter.ToInt16(data, 2);
				_injStat.InputVoltage = BitConverter.ToInt16(data, 4);
				_injStat.OutputVoltage = BitConverter.ToInt16(data, 6);
				_injStat.OutputCurrent = BitConverter.ToInt16(data, 8);
			}
			else
				throw new IOException();
			Status = _injStat;
		}

		public event EventHandler StatusChanged = delegate(object sender, EventArgs e) { };
		#endregion

		#region Управление инжектором
		public bool PowerOn()
		{
			lock (sync)
			{
				write(new byte[] { CMD_SET_BASE, 0 }, 0, 2);
				byte[] _cmdResult = readPacket();
				write(new byte[] { CMD_SWITCH_ON }, 0, 1);
				_cmdResult = readPacket();
				Thread.Sleep(100);
				bool result = (_cmdResult[0] == RESP_CORRECT);
				getStatusFromDevice();
				return result;
			}
		}

		public bool PowerOff()
		{
			lock (sync)
			{
				write(new byte[] { CMD_SWITCH_OFF }, 0, 1);
				byte[] _cmdResult = readPacket();
				Thread.Sleep(100);
				bool result = (_cmdResult[0] == RESP_CORRECT);
				getStatusFromDevice();
				return result;
			}
		}

		public bool SetFrequency(long f)
		{
			lock (sync)
			{
				byte[] val = BitConverter.GetBytes((short)(f / 1000000));
				write(new byte[] { CMD_SET_FREQ, val[0], val[1] }, 0, 3);
				byte[] _cmdResult = readPacket();
				Thread.Sleep(250);
				bool result = (_cmdResult[0] == RESP_CORRECT);
				getStatusFromDevice();
				return result;
			}
		}
		#endregion
	}

	/// <summary>
	/// Текущее состояние инжектора
	/// </summary>
	public struct InjectorStatus
	{
		/// <summary>Указывает что питание включено</summary>
		public bool PowerOn { get; set; }
		/// <summary>Указывает что входное напряжение находится вне допустимых пределов</summary>
		public bool InputVoltageOver { get; set; }
		/// <summary>Указывает, что инжектор перегружен по току</summary>
		public bool CurrentOverload { get; set; }
		/// <summary>Указывает на ошибку выхода инжектора</summary>
		public bool OutputError { get; set; }
		/// <summary>Указывает что ток на выходе инжектора превышает пороговое значение</summary>
		public bool OutputCurrentOver { get; set; }
		/// <summary>Частота настройки конвертора, установленная последней командой, МГц</summary>
		public short Frequency { get; set; }
		/// <summary>Входное напряжение инжектора, мВ</summary>
		public short InputVoltage { get; set; }
		/// <summary>Выходное напряжение инжектора, мВ</summary>
		public short OutputVoltage { get; set; }
		/// <summary>Выходной ток инжектора, мА</summary>
		public short OutputCurrent { get; set; }

		/// <summary>Возвращает true, если статус извещает об ошибке</summary>
		public bool IsError
		{
			get
			{
				return (InputVoltageOver || CurrentOverload || OutputError || OutputCurrentOver);
			}
		}
	}
}