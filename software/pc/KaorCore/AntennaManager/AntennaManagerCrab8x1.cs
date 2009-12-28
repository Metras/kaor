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
using System.Text;
using System.Xml;
using System.IO.Ports;

using KaorCore.Antenna;

namespace KaorCore.AntennaManager
{
	public class AntennaManagerCrab8x1 : BaseAntennaManager
	{        
        SerialPort port;
        byte[] numbers;

		IAntenna[] antennas;

        public AntennaManagerCrab8x1()
        {
			antennas = new IAntenna[8];
            port = new SerialPort();

			isAvailable = true;

            port.ReadTimeout = 200;
            numbers = new byte[8]; //номер каждой антенны (чтобы выбирать)
            control = new Crab8x1Control();
            control.AntennaManager = this;
            control.Left = 10;
            control.Top = 10;
            control.Height = 70; //хар-ки контрола по умолчанию
            control.Width = 150;
            control.OnAntennaSelect += new Crab8x1Control.SelectAntennaEventHandler(SelectAntenna);
        }

        #region IAntennaManager Members

		public void SelectAntenna(int pAntennaNo)
		{
			byte _ant_num;

			byte[] _buffer = new byte[1];

			if (!port.IsOpen)
				return;

			_buffer[0] = (byte)(0xA0 + pAntennaNo);
			port.Write(_buffer, 0, 1); //отправляем запрос на переключение
			byte[] _answer = new byte[1];
	
			port.Read(_answer, 0, 1);  //ждем ответа

			if (_answer[0] - 0x60 != pAntennaNo)
				throw new Exception("Incorrect answer");
			
		}

        /// <summary>
        /// Отдать команду выбрать антенну
        /// </summary>
        /// <param name="pAntenna">антенна</param>
        public override void SelectAntenna(IAntenna pAntenna)
        {
			int _i;

			for (_i = 0; _i < antennas.Length; _i++)
			{
				if (antennas[_i] == pAntenna)
				{
					SelectAntenna(_i);
					break;
				}
			}
#if false
            int _index = antennas.GIndexOf(pAntenna);  //ищем
            if (_index == -1)
                throw new Exception("Antenna not found");

			SelectAntenna(_index);
#endif
        }

        /// <summary>
        /// Отдать команду выбрать антенну
        /// </summary>
        /// <param name="pAntenna">ID антенны</param>
        public override void SelectAntenna(Guid pAntennaGuid)
        {
            IAntenna _ant = BaseAntenna.GetAntennaByGuid(pAntennaGuid);
            SelectAntenna(_ant);
        }
        /// <summary>
        /// загрузить из XML node
        /// </summary>
        /// <param name="pNode"></param>
        public override void LoadFromXmlNode(System.Xml.XmlNode pNode)
        {
            int _count = 0;   //порядковый номер нода с антенной
                             //(может отличаться от номера для вызова)
            if (pNode == null)
                throw new ArgumentNullException("pNode can not be null!");

            foreach (XmlNode _xn in pNode.ChildNodes)
            {
                if (_xn.Name == "Antenna" && 
					_xn.Attributes["type"] != null && 
					_xn.Attributes["type"].Value == "antenna")
                {
                    try
                    {
                        Guid id = new Guid(_xn.InnerText);
                        IAntenna _antenna = BaseAntenna.GetAntennaByGuid(id);
						if (_xn.Attributes["managerIn"] == null)
							continue;

                        int _antennaNo = int.Parse(_xn.Attributes["managerIn"].Value);

						if (_antennaNo < 0 || _antennaNo > 7)
							continue;

						if (_xn.Attributes["state"] != null)
						{
							switch (_xn.Attributes["state"].Value)  //загрузка состояния антенны
							{
								case "OK":
									_antenna.State = EAntennaState.OK;
									break;
								case "BAD":
									_antenna.State = EAntennaState.BAD;
									break;
								case "FAULT":
									_antenna.State = EAntennaState.FAULT;
									break;
								default:
									break;
							}
						}

						_antenna.OnAntennaChanged += new AntennaChangedDelegate(_antenna_OnAntennaChanged);
						antennas[_antennaNo] = _antenna;
                        

                    }
                    catch
                    {
                        throw;
                    }
                }
                if (_xn.Name == "Serial") //создание порта
                {
                    if (_xn.Attributes["port"] != null)
                        port.PortName = _xn.Attributes["port"].InnerText;
                    if (_xn.Attributes["baudrate"] != null)
                        port.BaudRate = Convert.ToInt32(_xn.Attributes["baudrate"].InnerText);

					try
					{
						port.Open();
					}

					catch
					{
					}
                }
            }            
        }

		

		void _antenna_OnAntennaChanged(IAntenna pAntenna)
		{
			CallOnAntennaChanged(pAntenna);
		}


        Crab8x1Control control;
        /// <summary>
        /// Контрол для управления классом
        /// </summary>
        public override System.Windows.Forms.UserControl Control
        {
            get 
            {
                return control;            
            }
        }

        #endregion

		public override System.Windows.Forms.Form SettingsForm
		{
			get 
			{
				AntennaManagerCrab8x1Settings _dlg = new AntennaManagerCrab8x1Settings(this);

				return _dlg;
			}
		}

		/// <summary>
		/// Сохранение параметров менеджера
		/// </summary>
		/// <param name="pWriter"></param>
		public override void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("AntennaManager");

			pWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);
			pWriter.WriteAttributeString("assembly", this.GetType().Assembly.FullName);

			for (int _i = 0; _i < antennas.Length; _i++)
			{
				if (antennas[_i] == null)
					continue;

				pWriter.WriteStartElement("Antenna");
				pWriter.WriteAttributeString("managerIn", _i.ToString(CultureInfo.InvariantCulture));
				pWriter.WriteAttributeString("type", "antenna");
				pWriter.WriteAttributeString("state", "OK");
				pWriter.WriteValue(antennas[_i].Id.ToString());
				pWriter.WriteEndElement();

			}
			pWriter.WriteStartElement("Serial");
			pWriter.WriteAttributeString("port", port.PortName);
			pWriter.WriteAttributeString("baudrate", port.BaudRate.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteEndElement();

			pWriter.WriteEndElement();
		}

		public override List<IAntenna> Antennas
		{
			get 
			{
				var q = from _a in antennas
						where _a != null
						select _a;

				return q.ToList();
			}
		}

		internal IAntenna[] AntennasArray
		{
			get
			{
				return antennas;
			}
		}

		/// <summary>
		/// Включение коммутатора
		/// </summary>
		public override void SwitchOn()
		{
			try
			{
				if (port.IsOpen)
					port.Close();

				port.Open();

				SelectAntenna(0);
			}

			catch
			{
				port.Close();
				isAvailable = false;
				throw;
			}
		}

		/// <summary>
		/// Отключение коммутатора
		/// </summary>
		public override void SwitchOff()
		{
			if (port.IsOpen)
			{
				try
				{

					SelectAntenna(0);
				}
				catch 
				{ 
				}

				port.Close();
			}
		}

		/// <summary>
		/// Последовательный порт коммутатора
		/// </summary>
		internal SerialPort Port
		{
			get
			{
				return port;
			}
		}

		public override void CheckConfiguration()
		{
			
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

					catch
					{
					}

					port.Close();
				}

				port.PortName = value;
			}
		}
	}
}
