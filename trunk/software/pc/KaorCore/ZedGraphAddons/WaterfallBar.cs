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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using ZedGraph;

namespace KaorCore.ZedGraphAddons
{
	public class WaterfallBar : Bar
	{
		int curveNum;
		int maxCurves;

		public WaterfallBar(int pCurveNum, int pMaxCurves)
			: this()
		{
			curveNum = pCurveNum;
			maxCurves = pMaxCurves;
		}

		public WaterfallBar()
			: base()
		{
		}

		public WaterfallBar(WaterfallBar rhs)
			: base(rhs)
		{
		}

		public WaterfallBar(Color color)
			 : base(color)
		{
		}
		
		protected WaterfallBar(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public int CurveNum
		{
			get { return curveNum; }
			set { curveNum = value; }
		}

		public int MaxCurves
		{
			get { return maxCurves; }
			set { maxCurves = value; }
		}
		/// <summary>
		/// Protected internal routine that draws the specified single bar (an individual "point")
		/// of this series to the specified <see cref="Graphics"/> device.
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="curve">A <see cref="CurveItem"/> object representing the
		/// <see cref="Bar"/>'s to be drawn.</param>
		/// <param name="index">
		/// The zero-based index number for the single bar to be drawn.
		/// </param>
		/// <param name="pos">
		/// The ordinal position of the this bar series (0=first bar, 1=second bar, etc.)
		/// in the cluster of bars.
		/// </param>
		/// <param name="baseAxis">The <see cref="Axis"/> class instance that defines the base (independent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="valueAxis">The <see cref="Axis"/> class instance that defines the value (dependent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="barWidth">
		/// The width of each bar, in pixels.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		protected override void DrawSingleBar(Graphics g, GraphPane pane,
										CurveItem curve,
										int index, int pos, Axis baseAxis, Axis valueAxis,
										float barWidth, float scaleFactor)
		{
			// pixBase = pixel value for the bar center on the base axis
			// pixHiVal = pixel value for the bar top on the value axis
			// pixLowVal = pixel value for the bar bottom on the value axis
			float pixBase, pixHiVal, pixLowVal;

			float clusterWidth = 0;// pane.BarSettings.GetClusterWidth();
			//float barWidth = curve.GetBarWidth( pane );
			float clusterGap = pane.BarSettings.MinClusterGap * barWidth;
			float barGap = barWidth * pane.BarSettings.MinBarGap;

			// curBase = the scale value on the base axis of the current bar
			// curHiVal = the scale value on the value axis of the current bar
			// curLowVal = the scale value of the bottom of the bar
			double curBase, curLowVal, curHiVal;
			ValueHandler valueHandler = new ValueHandler(pane, false);
			valueHandler.GetValues(curve, index, out curBase, out curLowVal, out curHiVal);

			// Any value set to double max is invalid and should be skipped
			// This is used for calculated values that are out of range, divide
			//   by zero, etc.
			// Also, any value <= zero on a log scale is invalid

			if (!curve.Points[index].IsInvalid)
			{
				// calculate a pixel value for the top of the bar on value axis
#if false				
				pixLowVal = valueAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curLowVal);
				pixHiVal = valueAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curHiVal);
#else
				pixHiVal = curveNum * (pane.Rect.Height / maxCurves);
				pixLowVal = (curveNum + 1) * (pane.Rect.Height / maxCurves);
#endif
				// calculate a pixel value for the center of the bar on the base axis
				pixBase = baseAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curBase);

				// Calculate the pixel location for the side of the bar (on the base axis)
				float pixSide = pixBase - clusterWidth / 2.0F + clusterGap / 2.0F +
								pos * (barWidth + barGap);

				// Draw the bar
				if (pane.BarSettings.Base == BarBase.X)
					this.Draw(g, pane, pixSide, pixSide + barWidth, pixLowVal,
							pixHiVal, scaleFactor, true, curve.IsSelected,
							curve.Points[index]);
				else
					this.Draw(g, pane, pixLowVal, pixHiVal, pixSide, pixSide + barWidth,
							scaleFactor, true, curve.IsSelected,
							curve.Points[index]);
			}
		}
#if false
		/// <summary>
		/// Draw the this <see cref="Bar"/> to the specified <see cref="Graphics"/>
		/// device as a bar at each defined point. This method
		/// is normally only called by the <see cref="BarItem.Draw"/> method of the
		/// <see cref="BarItem"/> object
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="curve">A <see cref="CurveItem"/> object representing the
		/// <see cref="Bar"/>'s to be drawn.</param>
		/// <param name="baseAxis">The <see cref="Axis"/> class instance that defines the base (independent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="valueAxis">The <see cref="Axis"/> class instance that defines the value (dependent)
		/// axis for the <see cref="Bar"/></param>
		/// <param name="barWidth">
		/// The width of each bar, in pixels.
		/// </param>
		/// <param name="pos">
		/// The ordinal position of the this bar series (0=first bar, 1=second bar, etc.)
		/// in the cluster of bars.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		public new void DrawBars(Graphics g, GraphPane pane, CurveItem curve,
								Axis baseAxis, Axis valueAxis,
								float barWidth, int pos, float scaleFactor)
		{
			// For non-cluster bar types, the position is always zero since the bars are on top
			// of eachother
			BarType barType = pane.BarSettings.Type;
			if (barType == BarType.Overlay || barType == BarType.Stack || barType == BarType.PercentStack ||
					barType == BarType.SortedOverlay)
				pos = 0;

			// Loop over each defined point and draw the corresponding bar                
			for (int i = 0; i < curve.Points.Count; i++)
				DrawSingleBar(g, pane, curve, i, pos, baseAxis, valueAxis, barWidth, scaleFactor);
		}

#endif

	}
}
