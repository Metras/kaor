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
using System.Text;

using ZedGraph;

namespace KaorCore.ZedGraphAddons
{
	public class WaterfallItem : BarItem
	{

		public WaterfallItem(string label, IPointList points, Color color, int pCurveNum, int pMaxCurves)
			: base(label, points, color)
		{
			_bar = new WaterfallBar(pCurveNum, pMaxCurves);
		}

		public int CurveNum
		{
			get { return ((WaterfallBar)_bar).CurveNum; }
			set { ((WaterfallBar)_bar).CurveNum = value; }
		}

		/// <summary>
		/// Calculate the width of each bar, depending on the actual bar type
		/// </summary>
		/// <returns>The width for an individual bar, in pixel units</returns>
		public override float GetBarWidth(GraphPane pane)
		{
			// Total axis width = 
			// npts * ( nbars * ( bar + bargap ) - bargap + clustgap )
			// cg * bar = cluster gap
			// npts = max number of points in any curve
			// nbars = total number of curves that are of type IsBar
			// bar = bar width
			// bg * bar = bar gap
			// therefore:
			// totwidth = npts * ( nbars * (bar + bg*bar) - bg*bar + cg*bar )
			// totwidth = bar * ( npts * ( nbars * ( 1 + bg ) - bg + cg ) )
			// solve for bar

			float barWidth;

			// For stacked bar types, the bar width will be based on a single bar
			float numBars = 1.0F;
			if (pane.BarSettings.Type == BarType.Cluster)
				numBars = pane.CurveList.NumClusterableBars;

			float denom = numBars * (1.0F + pane.BarSettings.MinBarGap) -
						pane.BarSettings.MinBarGap + pane.BarSettings.MinClusterGap;
			if (denom <= 0)
				denom = 1;

			double _dx = Points[1].X - Points[0].X;
			double _pointcount = (pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) / _dx;

			barWidth = (float)(pane.Rect.Width / _pointcount);

			//barWidth = pane.BarSettings.GetClusterWidth() / denom;

			if (barWidth < 1)
				return 1;

			return barWidth;
		}

	}
}
