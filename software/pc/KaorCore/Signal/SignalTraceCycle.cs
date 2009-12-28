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
using System.Drawing;
using System.Linq;
using System.Text;

using KaorCore.Base;
using KaorCore.Trace;
using KaorCore.ZedGraphAddons;
using ZedGraph;

namespace KaorCore.Signal
{
	/// <summary>
	/// Запись прохода трассы по сигналу
	/// </summary>
	public class SignalTraceCycle
	{
		#region ================ Поля ================
		Guid id;
		TracePoint[] points;
		Int64 fstart;
		Int64 fstop;
		Int64 measureStep;
		LineItem lineItem;
		int cycleNum;
		int maxCycles;

		//WaterfallItem barItem;
		GradientLineItem barItem;
		#endregion

		public SignalTraceCycle(Guid pCycleId, Int64 pFstart, Int64 pFstop, 
			int pMeasureStep, int pCycleNum, int pMaxCycles)
		{
			id = pCycleId;
			fstart = pFstart;
			fstop = pFstop;
			measureStep = pMeasureStep;
			cycleNum = pCycleNum;
			maxCycles = pMaxCycles;

			PreparePoints();

			lineItem = new LineItem("",
				new TraceFilteredPointList(points, (int)measureStep),
				Color.Gray, SymbolType.None);

			lineItem.Line.StepType = StepType.ForwardStep;

			/// Создание баритема
			/// 
			/*barItem = new WaterfallItem("",
						new TraceFilteredPointList(points, (int)measureStep),
						Color.Red, cycleNum, maxCycles);
			*/

			barItem = new GradientLineItem("", new TraceFilteredPointList(points, (int)measureStep),
						Color.Red, SymbolType.None);
			
			Fill _barFill = new GradientFill(Color.Black, Color.Red);
			_barFill.RangeMin = -100;
			_barFill.RangeMax = -20;
			//_barFill.IsGradientValueType = true;
			_barFill.Type = FillType.GradientByY;
			_barFill.SecondaryValueGradientColor = Color.Empty;
			barItem.Line.GradientFill = _barFill;
			//barItem.Line.Fill = _barFill;
			barItem.Line.StepType = StepType.ForwardStep;
			barItem.Line.Width = 5;

			//barItem.Bar.Border.IsVisible = true;
		}

		public int CycleNum
		{
			get { return cycleNum; }
			set 
			{ 
				cycleNum = value;
				/*if(barItem != null)
					barItem.CurveNum = cycleNum;*/
			}
		}
		private void PreparePoints()
		{
			/// Расчет количества точек в трассе
			int _npoints = (int)((fstop - fstart) / (Int64)measureStep + 1);

			/// Аллок массива точек трассы
			points = new TracePoint[_npoints];

			for (int _i = 0; _i < points.Length; _i++)
			{
				points[_i] = new TracePoint(fstart + (Int64)((Int64)_i * (Int64)measureStep),
					TracePoint.POWER_UNDEFINED);
			}

			//lineItem.Points = new TraceFilteredPointList(measurePoints, (int)measureStep);
		}

		/// <summary>
		/// Обработка точки измерения
		/// </summary>
		/// <param name="pNewPoint"></param>
		public void ProcessMeasure(TracePoint pNewPoint)
		{
			/// Проверка на принадлежность точки измерения диапазону
			if (pNewPoint.Freq < fstart || pNewPoint.Freq > fstop)
				return;

			int _pointIdx = (int)((pNewPoint.Freq - fstart) / measureStep);

			TracePoint _tp = points[_pointIdx];
			_tp.Power = pNewPoint.Power;
		}

		#region =============== Проперти ===============

		public Guid Id
		{
			get { return id; }
		}

		public TracePoint[] Points
		{
			get { return points; }
		}

		public LineItem LineItem
		{
			get { return lineItem; }
		}

		#endregion

		#region =============== Бар итем ===============


		public GradientLineItem BarItem
		{
			get
			{
				return barItem;
			}
		}
		#endregion
		
	}
}
