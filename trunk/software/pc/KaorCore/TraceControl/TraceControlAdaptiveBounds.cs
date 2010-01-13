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
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

using KaorCore.I18N;
using KaorCore.Trace;

using ZedGraph;

namespace KaorCore.TraceControl
{
    public class TraceControlAdaptiveBounds : BaseTraceControl
    {
		#region ============== Поля ==============

		BaseTrace trace;
        /// <summary>
        /// коэффициент БИХ-фильтра для оценки математического ожидания сигнала
        /// </summary>
        double k_mean = 0.4;

        /// <summary>
        /// коэффициент БИХ-фильтра (на нарастание) для оценки верхней границы сигнала
        /// </summary>
        double k_upper_attack = 0.9;

        /// <summary>
        /// коэффициент БИХ-фильтра (на спад) для оценки верхней границы сигнала
        /// </summary>
        double k_upper_release = 0.6;

        /// <summary>
        /// коэффициент БИХ-фильтра (на нарастание) для оценки нижней границы сигнала
        /// </summary>
        double k_lower_attack = 0.9;

        /// <summary>
        /// коэффициент БИХ-фильтра (на спад) для оценки нижней границы сигнала
        /// </summary>
        double k_lower_release = 0.6;

        /// <summary>
        /// допуск по отклонению (мультипликативный коэффициент)
        /// </summary>
        double delta_m = 2.0;

        /// <summary>
        /// допуск по отклонению (аддитивный коэффициент)
        /// </summary>
        double delta_s = 6.0;

		/// <summary>
		/// Минимальный уровень сигнала, для которого производится контроль.
		/// Если сигнал ниже уровня, то события срабатывания не генерируются, однако в
		/// подстройке порогов значение участвует.
		/// </summary>
		double minPower = -110.0;

		/// <summary>
		/// Количество циклов адаптации трассы
		/// </summary>
		int adaptCycles = 5;

        /// <summary>
        /// Трасса оценки математического ожидания
        /// </summary>
        protected TracePoint[] meanPoints;

        /// <summary>
        /// Трасса оценки верхней границы сигнала (относительная)
        /// </summary>
        protected TracePoint[] upperBoundPoints;

        /// <summary>
        /// Трасса оценки нижней границы сигнала (относительная)
        /// </summary>
        protected TracePoint[] lowerBoundPoints;

        /// <summary>
        /// Трасса предыдущих точек (для фильтрации)
        /// </summary>
        protected TracePoint[] prevPoints;

        /// ПРИМЕЧАНИЕ: Оценки верхней и нижней границ определены относительно оценки 
        /// матетического ожидания. То есть, для получения реального значения оценки 
        /// верхней/нижней границы необходимо относительную оценку сложить/вычесть 
        /// с/из математическим/математического ожиданием/ожидания (с учетом 
        /// коэффициентов допуска delta_m и delta_s).

        /// <summary>
        /// счетчик числа тактов ожидания (время предварительного обучения)
        /// </summary>
        protected int waitCounter;

        /// <summary>
        /// Линия верхнего порога для отрисовки на ZedGraph
        /// </summary>
        LineItem lineUpperItem;
        /// <summary>
        /// Линия нижнего порога для отрисовки на ZedGraph
        /// </summary>
        LineItem lineLowerItem;

        /// <summary>
        /// Трасса, которая используется для контроля по верхней границе
        /// </summary>
        protected TracePoint[] controlUpperPoints;
        /// <summary>
        /// Трасса, которая используется для контроля по нижней границе
        /// </summary>
        protected TracePoint[] controlLowerPoints;
        protected long controlPointsStep;

		//TraceControlAdaptiveBoundsSettings controlSettings;

		#endregion

        #region ============== Проперти ==============

		[Browsable(true)]
		[Description("Initial adaptation cycles count")]
		public int AdaptCycles
		{
			get { return adaptCycles; }
			set { adaptCycles = value; }
		}
        /// <summary>
        /// коэффициент БИХ-фильтра для оценки математического ожидания сигнала
        /// </summary>
        [Browsable(false)]
        public double K_mean
        {
            get { return k_mean; }
            set { k_mean = value; }
        }

        /// <summary>
        /// коэффициент БИХ-фильтра (на нарастание) для оценки верхней границы сигнала
        /// </summary>
        [Browsable(true)]
		//[Description("Коэффициент БИХ-фильтра на нарастание (верхний порог)")]
		[Description("Upper attack")]
        public double K_upper_attack
        {
            get { return k_upper_attack; }
            set { k_upper_attack = value; }
        }

        /// <summary>
        /// коэффициент БИХ-фильтра (на спад) для оценки верхней границы сигнала
        /// </summary>
        [Browsable(true)]
        //[Description("Коэффициент БИХ-фильтра на спад (верхний порог)")]
		[Description("Upper release")]
        public double K_upper_release
        {
            get { return k_upper_release; }
            set { k_upper_release = value; }
        }

        /// <summary>
        /// коэффициент БИХ-фильтра (на нарастание) для оценки нижней границы сигнала
        /// </summary>
        [Browsable(true)]
        //[Description("Коэффициент БИХ-фильтра на нарастание (нижний порог)")]
		[Description("Lower attack")]
        public double K_lower_attack
        {
            get { return k_lower_attack; }
            set { k_lower_attack = value; }
        }

        /// <summary>
        /// коэффициент БИХ-фильтра (на спад) для оценки нижней границы сигнала
        /// </summary>
        [Browsable(true)]
        //[Description("Коэффициент БИХ-фильтра на спад (нижний порог)")]
		[Description("Lower release")]
        public double K_lower_release
        {
            get { return k_lower_release; }
            set { k_lower_release = value; }
        }

        /// <summary>
        /// допуск по отклонению (мультипликативный коэффициент)
        /// </summary>
        [Browsable(true)]
        //[Description("Допуск по отклонению (мультипликативный коэффициент)")]
		[Description("Multiplicative coefficient")]
        public double Delta_m
        {
            get { return delta_m; }
            set { delta_m = value; }
        }

        /// <summary>
        /// допуск по отклонению (аддитивный коэффициент)
        /// </summary>
        [Browsable(true)]
        //[Description("Допуск по отклонению (аддитивный коэффициент)")]
		[Description("Additive coefficient")]
        public double Delta_s
        {
            get { return delta_s; }
            set { delta_s = value; }
        }

		[Browsable(true)]
		[Description("Minimal signal power to control")]
		public double Min_power
		{
			get { return minPower; }
			set { minPower = value; }
		}

        #endregion

		#region ============== Конструктор ==============


		public TraceControlAdaptiveBounds(BaseTrace pTrace)
			: base(pTrace)
		{
			Name = Locale.adaptive_trace_control_name;
			Description = Locale.adaptive_trace_control_descr;

			pTrace.OnTraceControlChanged += TraceChangedHandler;

            lineUpperItem = new LineItem(name,
				null,
//                            new TraceFilteredPointList(controlUpperPoints, controlPointsStep),
                            Color.FromArgb(32, Color.Red), SymbolType.None);
            lineUpperItem.Line.StepType = StepType.ForwardStep;
            lineUpperItem.Tag = this;
            lineUpperItem.Line.IsOptimizedDraw = true;
            lineUpperItem.Line.IsAntiAlias = false;
            lineUpperItem.Line.Style = DashStyle.Solid;
			lineUpperItem.Tag = this;
			lineUpperItem.IsSelectable = false;

            lineLowerItem = new LineItem(name,
				null,
//                            new TraceFilteredPointList(controlLowerPoints, controlPointsStep),
                            Color.FromArgb(32, Color.Blue), SymbolType.None);
            lineLowerItem.Line.StepType = StepType.ForwardStep;
            lineLowerItem.Tag = this;
            lineLowerItem.Line.IsOptimizedDraw = true;
            lineLowerItem.Line.IsAntiAlias = false;
			lineLowerItem.Line.Style = DashStyle.Solid;
			lineLowerItem.Tag = this;
			lineLowerItem.IsSelectable = false;

			ReInitialize();
		}

		public override void ReInitialize()
		{
			k_mean = 0.4;
			k_upper_attack = 0.9;
			k_upper_release = 0.6;
			k_lower_attack = 0.9;
			k_lower_release = 0.6;
			delta_m = 2.0;
			delta_s = 6.0;

			meanPoints = new TracePoint[scanTrace.TracePoints.Length];
			upperBoundPoints = new TracePoint[scanTrace.TracePoints.Length];
			lowerBoundPoints = new TracePoint[scanTrace.TracePoints.Length];
			prevPoints = new TracePoint[scanTrace.TracePoints.Length];
			controlUpperPoints = new TracePoint[scanTrace.TracePoints.Length];
			controlLowerPoints = new TracePoint[scanTrace.TracePoints.Length];

			/// Инициализация трасс оценки математического ожидания и верхней границы сигнала
			for (int _i = 0; _i < controlUpperPoints.Length; _i++)
			{
				meanPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
				///upperBoundPoints[_i] = new TracePoint(pTrace.TracePoints[_i].Freq, 0);
				///lowerBoundPoints[_i] = new TracePoint(pTrace.TracePoints[_i].Freq, 0);
				upperBoundPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
				lowerBoundPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
				prevPoints[_i] = new TracePoint(scanTrace.TracePoints[_i].Freq, 0);
				controlUpperPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
				controlLowerPoints[_i] = (TracePoint)scanTrace.TracePoints[_i].Clone();
			}
			controlPointsStep = scanTrace.MeasureStep;

			/// инициализация счетчика тактов ожидания
			waitCounter = adaptCycles * controlUpperPoints.Length;

			lineLowerItem.Points = new TraceFilteredPointList(controlLowerPoints, controlPointsStep);
			lineUpperItem.Points = new TraceFilteredPointList(controlUpperPoints, controlPointsStep);
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
				if(!_graphPane.CurveList.Contains(lineUpperItem))
					_graphPane.CurveList.Add(lineUpperItem);
				if(!_graphPane.CurveList.Contains(lineLowerItem))
					_graphPane.CurveList.Add(lineLowerItem);
            }
            else
            {
                _graphPane.CurveList.Remove(lineUpperItem);
                _graphPane.CurveList.Remove(lineLowerItem);
            }
        }
		#endregion

        [Browsable(false)]
		public override Form ControlSettings
		{
			get 
			{
				throw new InvalidOperationException();
				//if (controlSettings == null)
				//	controlSettings = new TraceControlAdaptiveBoundsSettings(this);

				//return controlSettings;
			}
		}

		/// <summary>
		/// Загрузка параметров адаптивного контроля из Xml
		/// </summary>
		/// <param name="pNode"></param>
		public override void LoadFromXml(XmlNode pNode)
		{
			XmlNode _node;

			base.LoadFromXml(pNode);

			_node = pNode.SelectSingleNode("delta_s");

			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out delta_s);

			_node = pNode.SelectSingleNode("delta_m");

			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out delta_m);

			_node = pNode.SelectSingleNode("k_lower_attack");
			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out k_lower_attack);

			_node = pNode.SelectSingleNode("k_lower_release");
			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out k_lower_release);

			_node = pNode.SelectSingleNode("k_upper_attack");
			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out k_upper_attack);

			_node = pNode.SelectSingleNode("k_upper_release");
			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out k_upper_release);

			_node = pNode.SelectSingleNode("min_power");
			if (_node != null)
				double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out minPower);

			_node = pNode.SelectSingleNode("adapt_cycles");
			if (_node != null)
				int.TryParse(_node.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out adaptCycles);
					
		}


		/// <summary>
		/// Сохранение параметров адаптивного контроля в XML
		/// </summary>
		/// <param name="pWriter"></param>
		public override void SaveToXml(XmlWriter pWriter)
		{
			base.SaveToXml(pWriter);
			pWriter.WriteElementString("delta_s", delta_s.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("delta_m", delta_m.ToString(CultureInfo.InvariantCulture));

			pWriter.WriteElementString("k_lower_attack", k_lower_attack.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("k_lower_release", k_lower_release.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("k_upper_attack", k_upper_attack.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("k_upper_release", k_upper_release.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("min_power", minPower.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("adapt_cycles", adaptCycles.ToString(CultureInfo.InvariantCulture));
		}

        //public override void TraceChangedHandler(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
        //{
        //    if (waitCounter > 0)
        //        waitCounter--;

        //    int _idx = (int)((pPoint.Freq - Fstart) / (Int64)controlPointsStep);

        //    /// проверка счетчика ожидания
        //    if (waitCounter == 0)
        //    {
        //        if (pPoint.Power > meanPoints[_idx].Power + delta_m * upperBoundPoints[_idx].Power + delta_s)
        //        {
        //            TracePoint p = new TracePoint(pPoint.Freq, pPoint.Power - (meanPoints[_idx].Power + delta_m * upperBoundPoints[_idx].Power + delta_s));
        //            CallOnTraceTrigger(p, pOldPower);
        //        }
        //        else if (pPoint.Power < meanPoints[_idx].Power - delta_m * lowerBoundPoints[_idx].Power - delta_s)
        //        {
        //            TracePoint p = new TracePoint(pPoint.Freq, pPoint.Power - (meanPoints[_idx].Power - delta_m * lowerBoundPoints[_idx].Power - delta_s));
        //            CallOnTraceTrigger(p, pOldPower);
        //        }
        //    }

        //    meanPoints[_idx].Power = k_mean * pPoint.Power + (1 - k_mean) * meanPoints[_idx].Power;

        //    double delta = pPoint.Power - meanPoints[_idx].Power;
        //    if (delta > prevPoints[_idx].Power)
        //        upperBoundPoints[_idx].Power = k_upper_attack * delta + (1 - k_upper_attack) * upperBoundPoints[_idx].Power;
        //    else
        //        upperBoundPoints[_idx].Power = k_upper_release * delta + (1 - k_upper_release) * upperBoundPoints[_idx].Power;
        //    delta = -delta;
        //    if (delta > prevPoints[_idx].Power)
        //        lowerBoundPoints[_idx].Power = k_lower_attack * delta + (1 - k_lower_attack) * lowerBoundPoints[_idx].Power;
        //    else
        //        lowerBoundPoints[_idx].Power = k_lower_release * delta + (1 - k_lower_release) * lowerBoundPoints[_idx].Power;

        //    prevPoints[_idx].Power = -delta;

        //    /// обновление трасс контроля
        //    controlUpperPoints[_idx].Power = meanPoints[_idx].Power + delta_m * upperBoundPoints[_idx].Power + delta_s;
        //    controlLowerPoints[_idx].Power = meanPoints[_idx].Power - delta_m * lowerBoundPoints[_idx].Power - delta_s;
        //}

        public override void TraceChangedHandler(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
        {
            if (waitCounter > 0)
                waitCounter--;

            int _idx = (int)((pPoint.Freq - Fstart) / (Int64)controlPointsStep);

            /// проверка счетчика ожидания
            if (waitCounter == 0 && pPoint.Power > minPower)
            {
                if (pPoint.Power > controlUpperPoints[_idx].Power)
                {
                    //TracePoint p = new TracePoint(pPoint.Freq, pPoint.Power - controlUpperPoints[_idx].Power);
					CallOnTraceTrigger(pPoint, pOldPower, pPoint.Power - controlUpperPoints[_idx].Power);
                }
                else if (pPoint.Power < controlLowerPoints[_idx].Power)
                {
					//TracePoint p = new TracePoint(pPoint.Freq, pPoint.Power - controlLowerPoints[_idx].Power);
                    CallOnTraceTrigger(pPoint, pOldPower, pPoint.Power - controlLowerPoints[_idx].Power);
                }
            }

            //if (pPoint.Power > prevPoints[_idx].Power)
            if (pPoint.Power > upperBoundPoints[_idx].Power)
            {
                upperBoundPoints[_idx].Power = k_upper_attack * pPoint.Power + (1 - k_upper_attack) * upperBoundPoints[_idx].Power;
            }
            else
            {
                upperBoundPoints[_idx].Power = k_upper_release * pPoint.Power + (1 - k_upper_release) * upperBoundPoints[_idx].Power;
            }

            //if (pPoint.Power > prevPoints[_idx].Power)
            if (pPoint.Power > lowerBoundPoints[_idx].Power)
            {
                lowerBoundPoints[_idx].Power = k_lower_release * pPoint.Power + (1 - k_lower_release) * lowerBoundPoints[_idx].Power;
            }
            else
            {
                lowerBoundPoints[_idx].Power = k_lower_attack * pPoint.Power + (1 - k_lower_attack) * lowerBoundPoints[_idx].Power;
            }


            prevPoints[_idx].Power = pPoint.Power;

            double _delta = (upperBoundPoints[_idx].Power - lowerBoundPoints[_idx].Power)/2.0;
            double _mean = (upperBoundPoints[_idx].Power + lowerBoundPoints[_idx].Power)/2.0;

            controlUpperPoints[_idx].Power = _mean + delta_m * _delta + delta_s;
            controlLowerPoints[_idx].Power = _mean - delta_m * _delta - delta_s;

        }
    }
}
