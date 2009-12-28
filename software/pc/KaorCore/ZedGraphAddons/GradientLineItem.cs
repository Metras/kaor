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

using ZedGraph;

namespace KaorCore.ZedGraphAddons
{
	public class GradientLineItem : LineItem, ICloneable
	{
		double drawY;

		public double DrawY
		{
			get { return ((GradientLine)_line).DrawY; }
			set { ((GradientLine)_line).DrawY = value; }
		}
		#region Constructors
		/// <summary>
		/// Create a new <see cref="LineItem"/>, specifying only the legend <see cref="CurveItem.Label" />.
		/// </summary>
		/// <param name="label">The _label that will appear in the legend.</param>
		public GradientLineItem( string label ) : base( label )
		{
			_symbol = new Symbol();
			_line = new GradientLine();
		}
		
		/// <summary>
		/// Create a new <see cref="LineItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The _label that will appear in the legend.</param>
		/// <param name="x">An array of double precision values that define
		/// the independent (X axis) values for this curve</param>
		/// <param name="y">An array of double precision values that define
		/// the dependent (Y axis) values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="Line"/> and <see cref="Symbol"/> properties.
		/// </param>
		/// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
		/// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
		/// to hide the symbols.</param>
		/// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
		/// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
		/// hide the line (see <see cref="ZedGraph.LineBase.IsVisible"/>).</param>
		public GradientLineItem( string label, double[] x, double[] y, Color color, SymbolType symbolType, float lineWidth )
			: this( label, new PointPairList( x, y ), color, symbolType, lineWidth )
		{
		}

		/// <summary>
		/// Create a new <see cref="LineItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The _label that will appear in the legend.</param>
		/// <param name="x">An array of double precision values that define
		/// the independent (X axis) values for this curve</param>
		/// <param name="y">An array of double precision values that define
		/// the dependent (Y axis) values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="Line"/> and <see cref="Symbol"/> properties.
		/// </param>
		/// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
		/// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
		/// to hide the symbols.</param>
		public GradientLineItem( string label, double[] x, double[] y, Color color, SymbolType symbolType )
			: this( label, new PointPairList( x, y ), color, symbolType )
		{
		}

		/// <summary>
		/// Create a new <see cref="LineItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The _label that will appear in the legend.</param>
		/// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
		/// the X and Y values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="Line"/> and <see cref="Symbol"/> properties.
		/// </param>
		/// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
		/// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
		/// to hide the symbols.</param>
		/// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
		/// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
		/// hide the line (see <see cref="ZedGraph.LineBase.IsVisible"/>).</param>
		public GradientLineItem( string label, IPointList points, Color color, SymbolType symbolType, float lineWidth )
			: base( label, points, color, symbolType)
		{
			_line = new GradientLine( color );
			if ( lineWidth == 0 )
				_line.IsVisible = false;
			else
				_line.Width = lineWidth;

			_symbol = new Symbol( symbolType, color );
		}

		/// <summary>
		/// Create a new <see cref="LineItem"/> using the specified properties.
		/// </summary>
		/// <param name="label">The _label that will appear in the legend.</param>
		/// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
		/// the X and Y values for this curve</param>
		/// <param name="color">A <see cref="Color"/> value that will be applied to
		/// the <see cref="Line"/> and <see cref="Symbol"/> properties.
		/// </param>
		/// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
		/// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
		/// to hide the symbols.</param>
		public GradientLineItem( string label, IPointList points, Color color, SymbolType symbolType )
			: this( label, points, color, symbolType, ZedGraph.LineBase.Default.Width )
		{
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The <see cref="LineItem"/> object from which to copy</param>
		public GradientLineItem( GradientLineItem rhs ) : base( rhs )
		{
			_symbol = new Symbol( rhs.Symbol );
			_line = new GradientLine( rhs.Line as GradientLine);
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
		public GradientLineItem Clone()
		{
			return new GradientLineItem( this );
		}

	#endregion
	}
}
