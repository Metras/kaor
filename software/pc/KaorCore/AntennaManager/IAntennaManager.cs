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
using KaorCore.RPU;

namespace KaorCore.AntennaManager
{
	/// <summary>
	/// Менеджер антенн (коммутатор)
	/// В перспективе, в одном из потомков, будет описываться топология подключения
	/// Антенна -- РПУ
	/// с возможностью указания допустимых типов соединений
	/// </summary>
	public interface IAntennaManager
	{
		/// <summary>
		/// Регистрация антенны в менеджере
		/// </summary>
		/// <param name="pAntenna"></param>
		//void RegisterAntenna(IAntenna pAntenna);

		/// <summary>
		/// Дерегистрация антенна в иенеджере
		/// Вероятно этот метод не нужен
		/// </summary>
		/// <param name="pAntenna"></param>
		//void UnRegisterAntenna(IAntenna pAntenna);

		/// <summary>
		/// Выбор антенны
		/// </summary>
		/// <param name="pAntenna"></param>
		void SelectAntenna(IAntenna pAntenna);

		/// <summary>
		/// Выбор антенны по ее Guid'у
		/// </summary>
		/// <param name="pAntennaGuid"></param>
		void SelectAntenna(Guid pAntennaGuid);

		/// <summary>
		/// Загрузка параметров из XML
		/// </summary>
		/// <param name="pNode"></param>
		void LoadFromXmlNode(XmlNode pNode);

		/// <summary>
		/// Список подключенных к менеджеру антенн
		/// </summary>
		List<IAntenna> Antennas { get; }

		/// <summary>
		/// Контрол управления менеджером антенн
		/// </summary>
		UserControl Control { get; }

		/// <summary>
		/// Диалог задания параметров менеджера антенн
		/// </summary>
		Form SettingsForm { get; }

		void SaveToXmlWriter(XmlWriter pWriter);

		void SwitchOn();
		void SwitchOff();

		bool IsAvailable { get; }

		void CheckConfiguration();

		event AntennaChangedDelegate OnAntennaChanged;
	}

	
}
