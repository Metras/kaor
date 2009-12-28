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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Base;
using KaorCore.AntennaManager;
using KaorCore.I18N;

namespace KaorCore.Antenna
{
	public class BaseAntenna : IAntenna
	{
		#region ================ Поля ================

		/// <summary>
		/// Идентификатор антенны
		/// </summary>
		Guid id;

		/// <summary>
		/// Координаты положения антенны
		/// </summary>
		GPSCoordinates coordinates;
		string name;
		string description;
		double direction;
		long freqMin;
		long freqMax;
		EAntennaType antennaType;
		double antennaDNWidth;
		EAntennaState antennaState;
		/// <summary>
		/// Менеджер антенн, к которому подключена антенна
		/// </summary>
		//IAntennaManager antennaManager;

		#endregion

		#region ================ Конструктор ================

		public BaseAntenna()
		{
			id = Guid.NewGuid();
			coordinates = new GPSCoordinates();
			name = Locale.str_new_antenna;
			description = Locale.str_antenna_descr;
		}

		public BaseAntenna(GPSCoordinates pCoord, //IAntennaManager pAntennaManager, 
			string pName, string pDescr, EAntennaType pType,
			double pDirection, long pFreqMin, long pFreqMax, double pDNWidth,
			EAntennaState pAntennaState)
			: this()
		{

			coordinates = pCoord.Clone() as GPSCoordinates;

			/// Фиксация координат антенны
			/// Дальнейшее изменение координат невозможно, т.к. если поменялись координаты - 
			/// это уже другая антенна
			//coordinates.ReadOnly = true;

			/// Заполнение параметров антенны
			name = pName;
			description = pDescr;

			freqMin = pFreqMin;
			freqMax = pFreqMax;

			antennaType = pType;
			antennaDNWidth = pDNWidth;
			direction = pDirection;

			antennaState = pAntennaState;
			/// Регистрация в соответствующем менеджере антенн
			/// 
			//antennaManager = pAntennaManager;
			//antennaManager.RegisterAntenna(this);

			antennaList.Add(this);
		}

		public BaseAntenna(GPSCoordinates pCoord)
			: this(pCoord, Locale.str_new_antenna, 
			Locale.str_antenna_descr,
			EAntennaType.Omni, 0, 0, 3000000000, 0, EAntennaState.BAD)
		{
		}


		public BaseAntenna(GPSCoordinates pCoord, //IAntennaManager pAntennaManager, 
			string pName, EAntennaType pType, double pDirection)
			: this(pCoord, pName, Locale.str_antenna_descr, 
			pType, pDirection, 0, 3000000000, 0, EAntennaState.BAD)
		{
		}

		#endregion

		#region ================ IAntenna Members ================

		public Guid Id
		{
			get { return id; }
		}

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
			}
		}

		/// <summary>
		/// Координаты положения антенны
		/// </summary>
		public GPSCoordinates Coordinates
		{
			get { return coordinates; }
			set { coordinates = value; }
		}

		/// <summary>
		/// Описание
		/// </summary>
		public string Description
		{
			get { return description; }
			set
			{
				description = value;
			}
		}

		public double Direction
		{
			get { return direction; }
			set { direction = value; }
		}

		public double DNWidth
		{
			get { return antennaDNWidth; }
			set { antennaDNWidth = value; }
		}

		public EAntennaType AntennaType
		{
			get { return antennaType; }
			set { antennaType = value; }
		}

		public long FreqMin
		{
			get { return freqMin; }
			set
			{
				freqMin = value;
			}
		}

		public long FreqMax
		{
			get { return freqMax; }
			set
			{
				freqMax = value;
			}
		}

		public EAntennaState State
		{
			get
			{
				return antennaState;
			}
			set
			{
				antennaState = value;
				
				/// Вызов события изменения антенны
				CallOnAntennaChanged();
			}
		}

		public KaorCore.RPU.IRPU RPU
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
		/// Тустринг возвращает имя антенны
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return name;
		}

		#endregion

		/// <summary>
		/// Удаление антенны
		/// </summary>
		public void Remove()
		{
			//antennaManager.UnRegisterAntenna(this);
		}

		public static BaseAntenna FromXmlNode(XmlNode pNode)
		{
			BaseAntenna _antenna;
			XmlNode _node;

			if (pNode == null)
				throw new ArgumentNullException("XML NODE == NULL!");

			/// Создание списка антенн из XML-описания
			/// 
			GPSCoordinates _coord = GPSCoordinates.FromXmlNode(pNode.SelectSingleNode("Location"));

			_antenna = new BaseAntenna(_coord);

			_node = pNode.SelectSingleNode("id");
			if(_node != null)
				_antenna.id = new Guid(_node.InnerText);


			_node = pNode.SelectSingleNode("type");
			if (_node != null)
			{
				if (_node.InnerText.ToLowerInvariant() == "omni")
					_antenna.antennaType = EAntennaType.Omni;
				else
					_antenna.antennaType = EAntennaType.Directional;
			}

			_node = pNode.SelectSingleNode("state");
			if (_node != null)
			{
				switch (_node.InnerText.ToLowerInvariant())
				{
					case "fault":
						_antenna.antennaState = EAntennaState.FAULT;
						break;

					case "ok":
						_antenna.antennaState = EAntennaState.OK;
						break;

					default:
						_antenna.antennaState = EAntennaState.BAD;
						break;
				}
			}

			_node = pNode.SelectSingleNode("name");
			if(_node != null)
				_antenna.name = _node.InnerText;

			/// Добавление в список известных антенн
			//antennasList.Add(_antenna);

			_node = pNode.SelectSingleNode("Location");
			if (_node != null)
			{
				_antenna.coordinates = GPSCoordinates.FromXmlNode(_node);
			}

			return _antenna;
		}

		

		public Form SettingsDialog
		{
			get
			{
				BaseAntennaSettingsDialog _settingsDialog = new BaseAntennaSettingsDialog(this);
				
				return _settingsDialog;
			}
		}


		static ObservableCollection<IAntenna> antennaList = new ObservableCollection<IAntenna>();

		public static IAntenna GetAntennaByGuid(Guid pId)
		{
			IAntenna _antenna = null;

			var q = from _a in antennaList
					where _a.Id == pId
					select _a;

			if (q.Count() == 1)
				_antenna = q.First();

			
			return _antenna;
		}


		public static IAntenna GetAntennaByGuidOrFirst(Guid pId)
		{
			IAntenna _ant = GetAntennaByGuid(pId);

			if (_ant == null)
			{
				if (antennaList.Count() > 0)
					_ant = antennaList.First();
			}

			return _ant;
		}

		/// <summary>
		/// Адаление антенны из списка зарегистрированных антенн
		/// </summary>
		/// <param name="pAntenna"></param>
		public static void DeleteAntenna(IAntenna pAntenna)
		{
			if (antennaList.Contains(pAntenna))
				antennaList.Remove(pAntenna);
		}

		public static ObservableCollection<IAntenna> AntennaList
		{
			get { return antennaList; }
		}

		#region IAntenna Members

		/// <summary>
		/// Сохранение параметров антенны в XML
		/// </summary>
		/// <param name="pWriter"></param>
		public void SaveToXmlWriter(XmlWriter pWriter)
		{

			pWriter.WriteElementString("name", name);
			pWriter.WriteElementString("description", description);
			pWriter.WriteElementString("state", antennaState.ToString());
			pWriter.WriteElementString("type", antennaType.ToString());
			pWriter.WriteElementString("id", id.ToString());

			pWriter.WriteStartElement("Location");
			coordinates.SaveToXmlWriter(pWriter);
			pWriter.WriteEndElement();

		}

		#endregion

		#region IAntenna Members

		void CallOnAntennaChanged()
		{
			if (OnAntennaChanged != null)
				OnAntennaChanged(this);
		}
		public event AntennaChangedDelegate OnAntennaChanged;

		#endregion
	}
}
