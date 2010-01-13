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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

using KaorCore.Trace;

using ZedGraph;

#if false
namespace KaorCore.TraceControl
{
	public class TraceControlDelta : BaseTraceControl
	{
		#region ============== Поля ==============
		
		double delta;
		TraceControlDeltaSettings controlSettings;

		/// <summary>
		/// Трасса, которая используется для контроля
		/// </summary>
		protected TracePoint[] controlPoints;
		protected long controlPointsStep;
		/// <summary>
		/// Линия для отрисовки на ZedGraph
		/// </summary>
		LineItem lineItem;

		#endregion

		#region ============== Конструктор ==============
		
		public TraceControlDelta(BaseTrace pTrace)
			: base(pTrace)
		{
			Name = "Контроль по порогу";
			delta = 3.0;

			controlPoints = new TracePoint[scanTrace.TracePoints.Length];


			/// Инициализация точек контроля точками трассы
			for (int _i = 0; _i < controlPoints.Length; _i++)
			{
				controlPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
				controlPoints[_i].Power += delta;
			}

			controlPointsStep = scanTrace.MeasureStep;

			lineItem = new LineItem(name,
				new TraceFilteredPointList(controlPoints, controlPointsStep),
				Color.Gray, SymbolType.None);

			lineItem.Line.StepType = StepType.ForwardStep;
			lineItem.Tag = this;
			lineItem.Line.IsOptimizedDraw = true;
			lineItem.Line.IsAntiAlias = false;
			lineItem.Line.Style = DashStyle.Dash;

		}

		#endregion

		#region ============== Методы ==============
		/// <summary>
		/// Отрисовка объекта контроля на ZedGraph
		/// </summary>
		/// <param name="pCtrl"></param>
		/// <param name="pDrawChilds"></param>
		public override void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds)
		{
			GraphPane _graphPane = pCtrl.GraphPane;

			if (isVisible == true)
			{
				_graphPane.CurveList.Add(lineItem);
			}
			else
			{
				_graphPane.CurveList.Remove(lineItem);
			}
		}

		#endregion

		public override Form ControlSettings
		{
			get 
			{
				if (controlSettings == null)
					controlSettings = new TraceControlDeltaSettings(this);

				return controlSettings;
			}
		}

		public override void LoadFromXml(System.Xml.XmlNode pNode)
		{
			throw new NotImplementedException();
		}

		public override void SaveToXml(System.Xml.XmlWriter pWriter)
		{
			throw new NotImplementedException();
		}

		#region ============== Проперти ==============

		public double Delta
		{
			get { return delta; }
			set 
			{
				double _oldDelta = delta;
				delta = value;

				for (int _i = 0; _i < controlPoints.Length; _i++)
				{
					controlPoints[_i].Power += (_oldDelta - delta);
				}
			}
		}

		#endregion

		public override void TraceChangedHandler(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
		{
			int _idx = (int)((pPoint.Freq - Fstart) / (Int64)controlPointsStep);

			double _controlPower = controlPoints[_idx].Power;

			if (pPoint.Power - _controlPower > delta)
			{
				/// Превышение в плюс
				/// 
				CallOnTraceTrigger(pPoint, pOldPower, pPoint.Power - _controlPower);
			}
		}

		public override void ReInitialize()
		{
			throw new NotImplementedException();
		}
	}
}
#endif