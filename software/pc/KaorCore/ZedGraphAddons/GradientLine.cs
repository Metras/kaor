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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

using ZedGraph;

namespace KaorCore.ZedGraphAddons
{
	public class GradientLine : Line, ICloneable
	{
		double drawY;

		public double DrawY
		{
			get { return drawY; }
			set { drawY = value; }
		}
		#region Constructors

		/// <summary>
		/// Default constructor that sets all <see cref="Line"/> properties to default
		/// values as defined in the <see cref="Default"/> class.
		/// </summary>
		public GradientLine()
			: this( Color.Empty )
		{
		}

		/// <summary>
		/// Constructor that sets the color property to the specified value, and sets
		/// the remaining <see cref="Line"/> properties to default
		/// values as defined in the <see cref="Default"/> class.
		/// </summary>
		/// <param name="color">The color to assign to this new Line object</param>
		public GradientLine( Color color )
		{
			_color = color.IsEmpty ? Default.Color : color;
			_stepType = Default.StepType;
			_isSmooth = Default.IsSmooth;
			_smoothTension = Default.SmoothTension;
			_fill = new Fill( Default.FillColor, Default.FillBrush, Default.FillType );
			_isOptimizedDraw = Default.IsOptimizedDraw;

			drawY = 0;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The Line object from which to copy</param>
		public GradientLine( GradientLine rhs )
		{
			_color = rhs._color;
			_stepType = rhs._stepType;
			_isSmooth = rhs._isSmooth;
			_smoothTension = rhs._smoothTension;
			_fill = rhs._fill.Clone();
			_isOptimizedDraw = rhs._isOptimizedDraw;
		}

		/// <summary>
		/// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
		/// calling the typed version of <see cref="Clone" />
		/// </summary>
		/// <returns>A deep copy of this object</returns>
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>
		/// Typesafe, deep-copy clone method.
		/// </summary>
		/// <returns>A new, independent copy of this class</returns>
		public GradientLine Clone()
		{
			return new GradientLine( this );
		}

	#endregion
			
		/// <summary>
		/// Draw the this <see cref="CurveItem"/> to the specified <see cref="Graphics"/>
		/// device.  The format (stair-step or line) of the curve is
		/// defined by the <see cref="StepType"/> property.  The routine
		/// only draws the line segments; the symbols are drawn by the
		/// <see cref="Symbol.Draw"/> method.  This method
		/// is normally only called by the Draw method of the
		/// <see cref="CurveItem"/> object
		/// </summary>
		/// <param name="g">
		/// A graphic device object to be drawn into.  This is normally e.Graphics from the
		/// PaintEventArgs argument to the Paint() method.
		/// </param>
		/// <param name="scaleFactor">
		/// The scaling factor to be used for rendering objects.  This is calculated and
		/// passed down by the parent <see cref="GraphPane"/> object using the
		/// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
		/// font sizes, etc. according to the actual size of the graph.
		/// </param>
		/// <param name="pane">
		/// A reference to the <see cref="GraphPane"/> object that is the parent or
		/// owner of this object.
		/// </param>
		/// <param name="curve">A <see cref="LineItem"/> representing this
		/// curve.</param>
		public override void DrawCurve( Graphics g, GraphPane pane,
                                CurveItem curve, float scaleFactor )
		{
			Line source = this;
			if ( curve.IsSelected )
				source = Selection.Line;

			// switch to int to optimize drawing speed (per Dale-a-b)
			int	tmpX, tmpY,
					lastX = int.MaxValue,
					lastY = int.MaxValue;

			double curX, curY, lowVal;
			PointPair curPt, lastPt = new PointPair();

			bool lastBad = true;
			IPointList points = curve.Points;
			ValueHandler valueHandler = new ValueHandler( pane, false );
			Axis yAxis = curve.GetYAxis( pane );
			Axis xAxis = curve.GetXAxis( pane );

			bool xIsLog = xAxis.Scale.IsLog;
			bool yIsLog = yAxis.Scale.IsLog;

			// switch to int to optimize drawing speed (per Dale-a-b)
			int minX = (int)pane.Chart.Rect.Left;
			int maxX = (int)pane.Chart.Rect.Right;
			int minY = (int)pane.Chart.Rect.Top;
			int maxY = (int)pane.Chart.Rect.Bottom;

			using ( Pen pen = source.GetPen( pane, scaleFactor ) )
			{
				if ( points != null && !_color.IsEmpty && this.IsVisible )
				{
					/// Установка ширины линии в зависимости от скейла оси
					_width = yAxis.Scale.GetClusterWidth(pane) * 1.1f;

					//bool lastOut = false;
					bool isOut;

					bool isOptDraw = _isOptimizedDraw && points.Count > 1000;

					// (Dale-a-b) we'll set an element to true when it has been drawn	
					bool[,] isPixelDrawn = null;
					
					if ( isOptDraw )
						isPixelDrawn = new bool[maxX + 1, maxY + 1]; 
					
					// Loop over each point in the curve
					for ( int i = 0; i < points.Count; i++ )
					{
						curPt = points[i];
						if ( pane.LineType == LineType.Stack )
						{
							if ( !valueHandler.GetValues( curve, i, out curX, out lowVal, out curY ) )
							{
								curX = PointPair.Missing;
								curY = PointPair.Missing;
							}
						}
						else
						{
							curX = curPt.X;
							curY = curPt.Y;
						}

						// Any value set to double max is invalid and should be skipped
						// This is used for calculated values that are out of range, divide
						//   by zero, etc.
						// Also, any value <= zero on a log scale is invalid
						if ( curX == PointPair.Missing ||
								curY == PointPair.Missing ||
								System.Double.IsNaN( curX ) ||
								System.Double.IsNaN( curY ) ||
								System.Double.IsInfinity( curX ) ||
								System.Double.IsInfinity( curY ) ||
								( xIsLog && curX <= 0.0 ) ||
								( yIsLog && curY <= 0.0 ) )
						{
							// If the point is invalid, then make a linebreak only if IsIgnoreMissing is false
							// LastX and LastY are always the last valid point, so this works out
							lastBad = lastBad || !pane.IsIgnoreMissing;
							isOut = true;
						}
						else
						{
							// Transform the current point from user scale units to
							// screen coordinates
							tmpX = (int) xAxis.Scale.Transform( curve.IsOverrideOrdinal, i, curX );
							//tmpY = (int)yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, curY);
							tmpY = (int)yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, drawY);

							// Maintain an array of "used" pixel locations to avoid duplicate drawing operations
							// contributed by Dale-a-b
							if ( isOptDraw && tmpX >= minX && tmpX <= maxX &&
										tmpY >= minY && tmpY <= maxY ) // guard against the zoom-in case
							{
								if ( isPixelDrawn[tmpX, tmpY] )
									continue;
								isPixelDrawn[tmpX, tmpY] = true;
							}

							isOut = ( tmpX < minX && lastX < minX ) || ( tmpX > maxX && lastX > maxX ) ||
								( tmpY < minY && lastY < minY ) || ( tmpY > maxY && lastY > maxY );

							if ( !lastBad )
							{
								try
								{
									// GDI+ plots the data wrong and/or throws an exception for
									// outrageous coordinates, so we do a sanity check here
									if ( lastX > 5000000 || lastX < -5000000 ||
											lastY > 5000000 || lastY < -5000000 ||
											tmpX > 5000000 || tmpX < -5000000 ||
											tmpY > 5000000 || tmpY < -5000000 )
										InterpolatePoint( g, pane, curve, lastPt, scaleFactor, pen,
														lastX, lastY, tmpX, tmpY );
									else if ( !isOut )
									{
										if ( !curve.IsSelected && this._gradientFill.IsGradientValueType )
										{
											using ( Pen tPen = GetPen( pane, scaleFactor, lastPt ) )
											{
												if (tPen.Color.A == 0)
												{
													throw new Exception("Invalid alpha!");
												}

												if ( this.StepType == StepType.NonStep )
												{
													g.DrawLine( tPen, lastX, lastY, tmpX, tmpY );
												}
												else if ( this.StepType == StepType.ForwardStep )
												{
													g.DrawLine( tPen, lastX, lastY, tmpX, lastY );
													g.DrawLine( tPen, tmpX, lastY, tmpX, tmpY );
												}
												else if ( this.StepType == StepType.RearwardStep )
												{
													g.DrawLine( tPen, lastX, lastY, lastX, tmpY );
													g.DrawLine( tPen, lastX, tmpY, tmpX, tmpY );
												}
												else if ( this.StepType == StepType.ForwardSegment )
												{
													g.DrawLine( tPen, lastX, lastY, tmpX, lastY );
												}
												else
												{
													g.DrawLine( tPen, lastX, tmpY, tmpX, tmpY );
												}
											}
										}
										else
										{
											if ( this.StepType == StepType.NonStep )
											{
												g.DrawLine( pen, lastX, lastY, tmpX, tmpY );
											}
											else if ( this.StepType == StepType.ForwardStep )
											{
												g.DrawLine( pen, lastX, lastY, tmpX, lastY );
												g.DrawLine( pen, tmpX, lastY, tmpX, tmpY );
											}
											else if ( this.StepType == StepType.RearwardStep )
											{
												g.DrawLine( pen, lastX, lastY, lastX, tmpY );
												g.DrawLine( pen, lastX, tmpY, tmpX, tmpY );
											}
											else if ( this.StepType == StepType.ForwardSegment )
											{
												g.DrawLine( pen, lastX, lastY, tmpX, lastY );
											}
											else if ( this.StepType == StepType.RearwardSegment )
											{
												g.DrawLine( pen, lastX, tmpY, tmpX, tmpY );
											}
										}
									}

								}
								catch
								{
									InterpolatePoint( g, pane, curve, lastPt, scaleFactor, pen,
												lastX, lastY, tmpX, tmpY );
								}

							}

							lastPt = curPt;
							lastX = tmpX;
							lastY = tmpY;
							lastBad = false;
							//lastOut = isOut;
						}
					}
				}
			}
		}
	}
}
