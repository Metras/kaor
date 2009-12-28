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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.I18N;

namespace KaorCore.AntennaManager
{
	public class AntennaManagerDirect : BaseAntennaManager
	{
		IAntenna antenna;
		AntennaManagerDirectUserControl control;

		#region ================ Конструктор ================
		
		public AntennaManagerDirect()
			: base()
		{
		}

		#endregion


		/// <summary>
		/// Выбор указанной антенны
		/// Т.к. антенна подключена всего одна, то просто ничего не делаем, кроме проверки 
		/// правильности переданной антенны
		/// </summary>
		/// <param name="pAntenna"></param>
		public override void SelectAntenna(IAntenna pAntenna)
		{
			if (pAntenna != antenna)
				throw new ArgumentException("Invalid antenna!");
		}

		public override void SelectAntenna(Guid pAntennaGuid)
		{
			if (antenna.Id != pAntennaGuid)
				throw new ArgumentException("Invalid antenna!");
		}

		#region IAntennaManager Members


		/// <summary>
		/// Загрузка описания менеджера из ноды XML
		/// </summary>
		/// <param name="pNode"></param>
		public override void LoadFromXmlNode(XmlNode pNode)
		{
			if (pNode == null)
				throw new ArgumentNullException("pNode can not be null!");

			XmlNode _node = pNode.SelectSingleNode("Antenna");

			if (_node == null)
				throw new Exception();

			Guid _antennaId = new Guid(_node.InnerText);

			antenna = BaseAntenna.GetAntennaByGuid(_antennaId);

			//RegisterAntenna(antenna);
			//Antennas.Add(antenna);
		}

		public override UserControl Control
		{
			get 
			{
				if (control == null)
				{
					control = new AntennaManagerDirectUserControl(antenna);
				}

				return control;
			}
		}
		#endregion

		public IAntenna Antenna
		{
			get { return antenna; }
			set { antenna = value; }
		}

		
		public override Form SettingsForm
		{
			get 
			{
				AntennaManagerDirectSettings _dlg = new AntennaManagerDirectSettings(this);

				return _dlg;
			}
		}

		/// <summary>
		/// Сохранение настроек менеджера антенн в XML writer
		/// </summary>
		/// <param name="pWriter"></param>
		public override void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("AntennaManager");
			pWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);
			pWriter.WriteAttributeString("assembly", this.GetType().Assembly.FullName);
			pWriter.WriteStartElement("Antenna");
			pWriter.WriteValue(antenna.Id.ToString());
			pWriter.WriteEndElement();
			pWriter.WriteEndElement();
		}

		public override List<IAntenna> Antennas
		{
			get 
			{
				List<IAntenna> _antennas = new List<IAntenna>();
				_antennas.Add(antenna);
				return _antennas;
			}
		}

		public override void SwitchOn()
		{
		}

		public override void SwitchOff()
		{
		}

		/// <summary>
		/// Проверка конфигурации антенного менеджера
		/// </summary>
		public override void CheckConfiguration()
		{
			if (antenna == null)
				throw new Exception(Locale.err_manager_invalid_antenna);
		}
	}
}
