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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.I18N;
using ZedGraph;

namespace KaorCore.Marker
{
	/// <summary>
	/// Радиомаркеры
	/// Маркер отображается вертикальной линией с символом.
	/// </summary>
	public class BaseRadioMarker : IComparable
	{
		#region ================ Поля ================
		
		Guid id;
		string name;
		string description;
		DateTime timeStamp;
		Int64 freq;
		LineItem markerLineItem;
		Color markerColor;

		bool isVisible;
		double power;

		#endregion

		#region ================ Конструктор ================
		public BaseRadioMarker()
			: this(0, Locale.marker, Color.Green)
		{
		}

		public BaseRadioMarker(Int64 pFreq)
			: this(pFreq, Locale.marker, Color.Green)
		{
		}

		public BaseRadioMarker(Int64 pFreq, string pName)
			: this(pFreq, pName, Color.Green)
		{
			
		}

		public BaseRadioMarker(Int64 pFreq, string pName, Color pColor)
			: this(pFreq, pName, 0.0, pColor)
		{
		}

		public BaseRadioMarker(Int64 pFreq, string pName, double pPower, Color pColor)
		{
			timeStamp = DateTime.Now;
			freq = pFreq;
			power = pPower;
			id = Guid.NewGuid();
			markerColor = pColor;
			name = pName;
			isVisible = true;

			markerColor = Color.FromArgb(128, markerColor);

		}

		/// <summary>
		/// Редактирование параметров маркера
		/// </summary>
		/// <returns></returns>
		public bool UserEditMarker()
		{
			bool _res = false;

			NewMarkerDialog _dlg = new NewMarkerDialog();
			_dlg.MarkerFrequency = freq;
			_dlg.MarkerName = name;
			_dlg.cmbColor.Value = Color.FromArgb(255, markerColor.R, markerColor.G, markerColor.B);


			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				name = _dlg.MarkerName;
				markerColor = _dlg.cmbColor.Value;
				
				freq = _dlg.MarkerFrequency;

				MakeLineItem();

				_res = true;
			}

			return _res;
		}

		#endregion

		#region ================ Проперти ================

		public Guid Id
		{
			get { return id; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public DateTime TimeStamp
		{
			get { return timeStamp; }
		}

		public Int64 Frequency
		{
			get { return freq; }
		}

		public double Power
		{
			get { return power; }
			set { power = value; }
		}

		void MakeLineItem()
		{
			/// Создание линии маркера

			double[] _cursorX = new double[400 / 5];
			double[] _cursorY = new double[400 / 5];

			for (int _i = 0; _i < 400 / 5; _i++)
			{
				_cursorX[_i] = freq;
				_cursorY[_i] = -200 + (_i * 5);
			}


			PointPairList _markerPoints = new PointPairList(_cursorX, _cursorY);
			if (markerLineItem == null)
			{
				markerLineItem = new LineItem(name, _markerPoints,
					Color.FromArgb(128, markerColor), SymbolType.None);
				markerLineItem.IsSelectable = true;
				markerLineItem.Tag = this;
			}
			else
			{
				markerLineItem.Points = _markerPoints;
				markerLineItem.Color = Color.FromArgb(128, markerColor);
			}

			markerLineItem.Line.Width = 2.0f;
		}

		public LineItem LineItem
		{
			get
			{
				if (markerLineItem == null)
				{
					MakeLineItem();
				}

				return markerLineItem;
			}
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set 
			{ 
				isVisible = value;
				CallOnMarkerChanged();
			}
		}
		/// <summary>
		/// Отрисовка маркера на ZedGraph
		/// </summary>
		/// <param name="pControl"></param>
		public void DrawZedGraph(ZedGraphControl pControl)
		{
			if (isVisible)
			{
				if (!pControl.GraphPane.CurveList.Contains(LineItem))
				{
					pControl.GraphPane.CurveList.Add(LineItem);
				}
			}
			else
			{
				pControl.GraphPane.CurveList.Remove(LineItem);
			}
		}
	
		#endregion


		#region ================ Статические методы ================
		public static BaseRadioMarker FromDialog(Int64 pFrequency)
		{
			BaseRadioMarker _marker = null;
			NewMarkerDialog _dlg = new NewMarkerDialog();

			_dlg.MarkerFrequency = pFrequency;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				_marker = new BaseRadioMarker(_dlg.MarkerFrequency, _dlg.txtName.Text, 
					_dlg.cmbColor.Value);
			}

			return _marker;
		}
		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			BaseRadioMarker _m = obj as BaseRadioMarker;

			if (_m == null)
				return 0;

			if (freq < _m.freq)
				return -1;
			else if (freq > _m.freq)
				return 1;
			else 
				return 0;
		}

		#endregion
		
		XmlNode XmlSelectAndCheckNode(XmlNode pNode, string pName)
		{
			XmlNode _node;

			_node = pNode.SelectSingleNode(pName);
			if (_node == null)
				throw new InvalidOperationException("Missing element " + pName + "!");

			return _node;

		}

		public void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			_node = XmlSelectAndCheckNode(pNode, "id");
			id = new Guid(_node.InnerText);

			name = XmlSelectAndCheckNode(pNode, "name").InnerText;
			description = XmlSelectAndCheckNode(pNode, "description").InnerText;

			if (!Int64.TryParse(XmlSelectAndCheckNode(pNode, "freq").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out freq))
				throw new Exception("Error parsing parameter freq");

			if (!bool.TryParse(XmlSelectAndCheckNode(pNode, "isvisible").InnerText, out isVisible))
				throw new Exception("Error parsing parameter isvisible");

			_node = XmlSelectAndCheckNode(pNode, "color");
			if (_node != null)
			{
				byte _ta, _tr, _tg, _tb;

				if (!byte.TryParse(_node.Attributes["r"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tr))
					throw new InvalidOperationException("Invalid color R");

				if (!byte.TryParse(_node.Attributes["g"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tg))
					throw new InvalidOperationException("Invalid color G");

				if (!byte.TryParse(_node.Attributes["b"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tb))
					throw new InvalidOperationException("Invalid color B");

				if (!byte.TryParse(_node.Attributes["a"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _ta))
					throw new InvalidOperationException("Invalid color A");

				markerColor = Color.FromArgb(_ta, _tr, _tg, _tb);
			}
		}

		public void SaveToXmlWriter(XmlWriter pXmlWriter)
		{
			pXmlWriter.WriteStartElement("Marker");
			pXmlWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);

			pXmlWriter.WriteElementString("id", id.ToString());
			pXmlWriter.WriteElementString("name", name);
			pXmlWriter.WriteElementString("description", description);
			pXmlWriter.WriteElementString("freq", freq.ToString(CultureInfo.InvariantCulture));
			pXmlWriter.WriteElementString("isvisible", isVisible.ToString(CultureInfo.InvariantCulture));

			/// Цвет трассы
			/// 
			pXmlWriter.WriteStartElement("color");
			pXmlWriter.WriteAttributeString("r", markerColor.R.ToString(CultureInfo.InvariantCulture));
			pXmlWriter.WriteAttributeString("g", markerColor.G.ToString(CultureInfo.InvariantCulture));
			pXmlWriter.WriteAttributeString("b", markerColor.B.ToString(CultureInfo.InvariantCulture));
			pXmlWriter.WriteAttributeString("a", markerColor.A.ToString(CultureInfo.InvariantCulture));

			pXmlWriter.WriteValue(markerColor.ToString());

			pXmlWriter.WriteEndElement(); /// color
										  /// 
			pXmlWriter.WriteEndElement();
		}

		private void CallOnMarkerChanged()
		{
			if (OnMarkerChanged != null)
				OnMarkerChanged(this);
		}
		public delegate void MarkerChangedDelegate(BaseRadioMarker pSignal);

		[field: NonSerialized]
		public virtual event MarkerChangedDelegate OnMarkerChanged;
	}
}
