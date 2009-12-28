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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

using ZedGraph;

namespace KaorCore.ZedGraphAddons
{
	public class GradientFill : Fill
	{
		/// <summary>
		/// Constructor that creates a linear gradient color-fill, setting <see cref="Type"/> to
		/// <see cref="FillType.Brush"/> using the specified colors.
		/// </summary>
		/// <param name="color1">The first color for the gradient fill</param>
		/// <param name="color2">The second color for the gradient fill</param>
		public GradientFill( Color color1, Color color2 ) 
			: base( color1, color2, 0.0F )
		{
		}

		protected override Color GetGradientColor(double val)
		{
			double valueFraction;

			if (Double.IsInfinity(val) || double.IsNaN(val) || val == PointPair.Missing)
				val = _rangeDefault;

			if (_rangeMax - _rangeMin < 1e-20 || val == double.MaxValue)
				valueFraction = 0.5;
			else
				valueFraction = (val - _rangeMin) / (_rangeMax - _rangeMin);

			if (valueFraction < 0.0)
				valueFraction = 0.0;
			else if (valueFraction > 1.0)
				valueFraction = 1.0;

			if (_gradientBM == null)
			{

				RectangleF rect;
				LinearGradientBrush _brush;

				_gradientBM = new Bitmap(100, 1);
				Graphics gBM = Graphics.FromImage(_gradientBM);

				rect = new RectangleF(0, 0, 21, 1);
				_brush = new LinearGradientBrush(rect, Color.FromArgb(0, 0, 131),
					Color.FromArgb(0, 76, 255), LinearGradientMode.Horizontal);
				gBM.FillRectangle(_brush, rect);

				rect = new RectangleF(21, 0, 41, 1);
				_brush = new LinearGradientBrush(rect, Color.FromArgb(0, 76, 255),
					Color.FromArgb(20, 255, 235), LinearGradientMode.Horizontal);
				gBM.FillRectangle(_brush, rect);

				rect = new RectangleF(41, 0, 61, 1);
				_brush = new LinearGradientBrush(rect, Color.FromArgb(20, 255, 235),
					Color.FromArgb(219, 255, 36), LinearGradientMode.Horizontal);
				gBM.FillRectangle(_brush, rect);

				rect = new RectangleF(61, 0, 81, 1);
				_brush = new LinearGradientBrush(rect, Color.FromArgb(219, 255, 36),
					Color.FromArgb(255, 92, 0), LinearGradientMode.Horizontal);
				gBM.FillRectangle(_brush, rect);

				rect = new RectangleF(81, 0, 100, 1);
				_brush = new LinearGradientBrush(rect, Color.FromArgb(255, 92, 0),
					Color.FromArgb(147, 0, 0), LinearGradientMode.Horizontal);
				gBM.FillRectangle(_brush, rect);
#if false
				// Create a horizontal linear gradient with four stops.   

				myHorizontalGradient.GradientStops.Add(
					new GradientStop(Colors.Yellow, 0.0));
				myHorizontalGradient.GradientStops.Add(
					new GradientStop(Colors.Red, 0.25));
				myHorizontalGradient.GradientStops.Add(
					new GradientStop(Colors.Blue, 0.75));
				myHorizontalGradient.GradientStops.Add(
					new GradientStop(Colors.LimeGreen, 1.0));
#endif
			}

			Color _c =  _gradientBM.GetPixel((int)(99.9 * valueFraction), 0);
			if (_c.A == 0)
				throw new Exception("Invalid alpha!");

			return _c;
		}
	}
}
