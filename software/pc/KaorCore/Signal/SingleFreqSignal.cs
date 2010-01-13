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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using KaorCore.I18N;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.RadioControlSystem;
using KaorCore.Trace;
using KaorCore.TraceControl;
using KaorCore.Report;
using KaorCore.Utils;
using ZedGraph;


namespace KaorCore.Signal
{
	public class SingleFreqSignalParams : BaseSignalParams
	{
		double power;

		public double Power
		{
			get { return power; }
			set { power = value; }
		}

		bool isFixDeltaPlus;

		public bool IsFixDeltaPlus
		{
			get { return isFixDeltaPlus; }
			set { isFixDeltaPlus = value; }
		}
		bool isFixDeltaMinus;

		public bool IsFixDeltaMinus
		{
			get { return isFixDeltaMinus; }
			set { isFixDeltaMinus = value; }
		}

		public override Type SignalClassType
		{
			get
			{
				return typeof(SingleFreqSignal);
			}
		}

		public SingleFreqSignalParams()
		{
			power = -140.0;
			isFixDeltaMinus = true;
			isFixDeltaPlus = true;
		}

		/// <summary>
		/// Форма для задания параметров по-умолчанию
		/// </summary>
		public override Form SettingsForm
		{
			get
			{
				throw new NotImplementedException();
//				RangeSignalDefaultSettings _dlg = new RangeSignalDefaultSettings(this);

//				return _dlg;
			}
		}

		internal override void SaveToXmlWriter(XmlWriter pWriter)
		{
			base.SaveToXmlWriter(pWriter);

			pWriter.WriteElementString("power", power.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixplus", isFixDeltaPlus.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixminus", isFixDeltaMinus.ToString(CultureInfo.InvariantCulture));
		}

		internal override void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			base.LoadFromXmlNode(pNode);

			_node = pNode.SelectSingleNode("power");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out power))
					throw new Exception("Error parsing pmin!");

			_node = pNode.SelectSingleNode("isfixplus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaPlus))
					throw new Exception("Error parsing isfixplus!");

			_node = pNode.SelectSingleNode("isfixminus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaMinus))
					throw new Exception("Error parsing isfixminus!");
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// 
	[Serializable]
	public class SingleFreqSignal : BaseSignal
	{
		double power;

		[NonSerialized]
		LineItem signalLineItem;

		Color signalColor;

		[NonSerialized]
		SingleFreqSignalControl signalControl;

		double[] signalX = new double[2];
		double[] signalY = new double[2];

		public SingleFreqSignal()
			: base()
		{
			name = Locale.single_freq_signal;
			description = Locale.single_freq_signal;
			power = -140;
			SignalType = ESignalType.Red;

#if USE_SIGNAL_REPORTS
			reports = new List<ReportItem>();
#endif
			signalColor = Color.FromArgb(128, SignalColor);

			canCreateReport = false;
			canCreateTrace = false;
		}

		public override void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds)
		{
			if (isVisible)
			{
				if (isSelected)
					LineItem.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
				else
					LineItem.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;

				LineItem.Points[0].X = frequency;
				LineItem.Points[0].Y = -200;
				LineItem.Points[1].X = frequency;
				LineItem.Points[1].Y = power;

				LineItem.Color = Color.FromArgb(196, SignalColor);
				if (!pCtrl.GraphPane.CurveList.Contains(LineItem))
				{	
					pCtrl.GraphPane.CurveList.Add(LineItem);
				}
			}
			else
			{
				pCtrl.GraphPane.CurveList.Remove(LineItem);
			}
		}

		void MakeLineItem()
		{
			/// Создание линии маркера
#if false
			double[] _cursorX = new double[400 / 5];
			double[] _cursorY = new double[400 / 5];
			int _i;

			for (_i = 0; _i < power / 5 - 1; _i++)
			{
				_cursorX[_i] = frequency;
				_cursorY[_i] = -200 + (_i * 5);
			}

			_cursorX[_i] = frequency;
			_cursorY[_i] = power;
#else

			int _i;

			signalX[0] = frequency;
			signalY[0] = -200;
			signalX[1] = frequency;
			signalY[1] = power;

#endif
			PointPairList _markerPoints = new PointPairList(signalX, signalY);
			if (signalLineItem == null)
			{
				signalLineItem = new LineItem(name, _markerPoints,
					Color.FromArgb(128, signalColor), SymbolType.None);
				signalLineItem.IsSelectable = true;
				signalLineItem.Tag = this;
			}
			else
			{
				signalLineItem.Points = _markerPoints;
				signalLineItem.Color = Color.FromArgb(128, signalColor);
			}

			signalLineItem.Line.Width = 2.0f;
		}

		public LineItem LineItem
		{
			get
			{
				if (signalLineItem == null)
				{
					MakeLineItem();
				}

				return signalLineItem;
			}
		}

		public override bool IsTracePointBelongs(TracePoint pTracePoint, int pFilterBand)
		{
			if (frequency - pFilterBand / 2 <= pTracePoint.Freq && frequency + pFilterBand / 2 > pTracePoint.Freq)
				return true;
			else
				return false;
		}

		public override void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			base.LoadFromXmlNode(pNode);

			_node = pNode.SelectSingleNode("power");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out power))
					throw new Exception("Error parsing power!");
		}

		protected override void SaveBodyToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteElementString("power", power.ToString(CultureInfo.InvariantCulture));
		}

		public override Form SignalForm
		{
			get { throw new NotImplementedException(); }
		}

		public override UserControl SignalControl
		{
			get 
			{
				if (signalControl == null)
				{
					signalControl = new SingleFreqSignalControl(this);
					signalControl.OnSignalEditComplete += new SingleFreqSignalControl.SignalEditComplete(signalControl_OnSignalEditComplete);
				}

				return signalControl;
			}
		}

		void signalControl_OnSignalEditComplete(SingleFreqSignal pSignal)
		{
			CallOnSignalChanged();
		}

		public override object SignalParams
		{
			get
			{
				return null;
			}
			set
			{
				
			}
		}

		public override EBaseSignalTriggerAction ProcessTrigger(BaseTraceControl pTraceControl, TracePoint pTracePoint, double pOldPower, double pDelta)
		{
			return EBaseSignalTriggerAction.None;
		}

		public override void CreateSignalReport()
		{
#if false
			String FileName = ReportsManager.NewReportName(
					ReportType.PDF,
					String.Format("{0} ({1})", this.Name, KaorCore.Utils.FreqUtils.FreqToString(this.Frequency)));
			new SingleFreqSignalReport(this).Save(FileName);
			System.Diagnostics.Process.Start(FileName);
#endif
		}


		public double Power
		{
			get { return power; }
			set 
			{ 
				power = value;
				CallOnSignalChanged();
			}
		}

		public override string Name
		{
			get 
			{
				return Locale.power +": " + power.ToString("F1");

//				return String.Format(CultureInfo.InvariantCulture, "{F0} " + Locale.dbm, power); 
			}
			set
			{
//				name = value;
//				CallOnSignalChanged();
			}
		}
	}
}
