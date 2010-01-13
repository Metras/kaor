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
using System.Text;

using ZedGraph;

namespace KaorCore.Trace
{
	public class WaterfallFilteredPointList  : IPointList
	{
	#region Fields
		/// <summary>
		/// Точки трассы
		/// </summary>
		TracePoint[] points;
		int pointStep;
		Int64 fstart;

		/*
		/// <summary>
		/// This is the minimum value of the range of interest (typically the minimum of
		/// the range that you have zoomed into)
		/// </summary>
		//private double _xMinBound = double.MinValue;
		/// <summary>
		/// This is the maximum value of the range of interest (typically the maximum of
		/// the range that you have zoomed into)
		/// </summary>
		//private double _xMaxBound = double.MaxValue;
		*/
		/// <summary>
		/// This is the maximum number of points that you want to see in the filtered dataset
		/// </summary>
		private int _maxPts = -1;

		/// <summary>
		/// The index of the xMinBound above
		/// </summary>
		private int _minBoundIndex = -1;
		/// <summary>
		/// The index of the xMaxBound above
		/// </summary>
		private int _maxBoundIndex = -1;

//		/// <summary>
//		/// Switch used to indicate if the next filtered point should be the high point or the
//		/// low point within the current range.
//		/// </summary>
//		private bool _upDown = false;

//		/// <summary>
//		/// Determines if the high/low logic will be used.
//		/// </summary>
//		private bool _isApplyHighLowLogic = true;

	#endregion

	#region Properties

		/// <summary>
		/// Indexer to access the specified <see cref="PointPair"/> object by
		/// its ordinal position in the list.
		/// </summary>
		/// <remarks>
		/// Returns <see cref="PointPairBase.Missing" /> for any value of <see paramref="index" />
		/// that is outside of its corresponding array bounds.
		/// </remarks>
		/// <param name="index">The ordinal position (zero-based) of the
		/// <see cref="PointPair"/> object to be accessed.</param>
		/// <value>A <see cref="PointPair"/> object reference.</value>
		public PointPair this[ int index ]  
		{
			get
			{
				int _idx = index;
				int _startIdx = index;
				int _stopIdx = index;

				// See if the array should be bounded
				if ( _minBoundIndex >= 0 && _maxBoundIndex >= 0 && _maxPts >= 0 )
				{
					// get number of points in bounded range
					int nPts = _maxBoundIndex - _minBoundIndex + 1;

					if ( nPts > _maxPts )
					{
						// if we're skipping points, then calculate the new index
						_startIdx = _minBoundIndex + (int)/*Math.Round*/((double)index * (double)nPts / (double)_maxPts);
						_stopIdx = _minBoundIndex + (int)/*Math.Round*/(((double)(index+1) * (double)nPts / (double)_maxPts));

						index = _minBoundIndex + (int) ( (double) index * (double) nPts / (double) _maxPts );
					}
					else
					{
						// otherwise, index is just offset by the start of the bounded range
						index += _minBoundIndex;
						_startIdx = index;
						_stopIdx = index;
					}
				}

				double xVal, yVal;

				if ( index >= 0 && index < points.Length )
					xVal = (double)(points[index].Freq);
				else
					xVal = PointPair.Missing;

				if (index >= 0 && index < points.Length)
				{
					if (_startIdx < 0)
						_startIdx = 0;

					if (_stopIdx > points.Length)
						_stopIdx = points.Length;

					yVal = points[_startIdx].Power;
						
					/// Поиск максимального значения на интервале
					for (int i = _startIdx; i < _stopIdx; i++)
						if (yVal < points[i].Power)
							yVal = points[i].Power;
				}
				else
					yVal = PointPair.Missing;

				if (yVal == TracePoint.POWER_UNDEFINED)
				{
					//xVal = PointPair.Missing;
					//yVal = PointPair.Missing;
				}

				return new PointPair( xVal - (Int64)pointStep / 2, yVal, PointPair.Missing, null );
			}

			/*
			set
			{
				// See if the array should be bounded
				if ( _minBoundIndex >= 0 && _maxBoundIndex >= 0 && _maxPts >= 0 )
				{
					// get number of points in bounded range
					int nPts = _maxBoundIndex - _minBoundIndex + 1;

					if ( nPts > _maxPts )
					{
						// if we're skipping points, then calculate the new index
						index = _minBoundIndex + (int) ( (double) index * (double) nPts / (double) _maxPts );
					}
					else
					{
						// otherwise, index is just offset by the start of the bounded range
						index += _minBoundIndex;
					}
				}

				if ( index >= 0 && index < points.Length )
					points[index].Freq = (Int64)(value.X);

				if ( index >= 0 && index < points.Length )
					points[index].Power = value.Y;
			}
			 */
		}

		/// <summary>
		/// Returns the number of points according to the current state of the filter.
		/// </summary>
		public int Count
		{
			get
			{
				int arraySize = points.Length;

				// Is the filter active?
				if ( _minBoundIndex >= 0 && _maxBoundIndex >= 0 && _maxPts > 0 )
				{
					// get the number of points within the filter bounds
					int boundSize = _maxBoundIndex - _minBoundIndex + 1;

					// limit the point count to the filter bounds
					if ( boundSize < arraySize )
						arraySize = boundSize;

					// limit the point count to the declared max points
					if ( arraySize > _maxPts )
						arraySize = _maxPts;
				}

				return arraySize;
			}
		}

		/// <summary>
		/// Gets the desired number of filtered points to output.  You can set this value by
		/// calling <see cref="SetBounds" />.
		/// </summary>
		public int MaxPts
		{
			get { return _maxPts; }
		}


	#endregion

	#region Constructors

		/// <summary>
		/// Constructor to initialize the PointPairList from two arrays of
		/// type double.
		/// </summary>
		public WaterfallFilteredPointList( TracePoint[] pPoints, int pPointsStep )
		{
			points = pPoints;
			pointStep = pPointsStep;
			fstart = points[0].Freq;
		}

		/// <summary>
		/// The Copy Constructor
		/// </summary>
		/// <param name="rhs">The FilteredPointList from which to copy</param>
		public WaterfallFilteredPointList( WaterfallFilteredPointList rhs )
		{
			points = (TracePoint[]) rhs.points.Clone();
			_minBoundIndex = rhs._minBoundIndex;
			_maxBoundIndex = rhs._maxBoundIndex;
			_maxPts = rhs._maxPts;

		}

		/// <summary>
		/// Deep-copy clone routine
		/// </summary>
		/// <returns>A new, independent copy of the FilteredPointList</returns>
		virtual public object Clone()
		{ 
			return new WaterfallFilteredPointList( this ); 
		}
		

	#endregion

	#region Methods

		/// <summary>
		/// Set the data bounds to the specified minimum, maximum, and point count.  Use values of
		/// min=double.MinValue and max=double.MaxValue to get the full range of data.  Use maxPts=-1
		/// to not limit the number of points.  Call this method anytime the zoom range is changed.
		/// </summary>
		/// <param name="min">The lower bound for the X data of interest</param>
		/// <param name="max">The upper bound for the X data of interest</param>
		/// <param name="maxPts">The maximum number of points allowed to be
		/// output by the filter</param>
		// New code mods by ingineer
		public void SetBounds( double min, double max, int maxPts )
		{
			_maxPts = maxPts;

			// find the index of the start and end of the bounded range
			Int64 first = ((Int64)min - (Int64)fstart) / (Int64)pointStep;
			Int64 last = ((Int64)max - (Int64)fstart) / (Int64)pointStep;

			//int first = Array.BinarySearch( points, new TracePoint((Int64)min, 0));
			//int last = Array.BinarySearch( points, new TracePoint((Int64)max, 0));

			// Make sure the bounded indices are legitimate
			// if BinarySearch() doesn't find the value, it returns the bitwise
			// complement of the index of the 1st element larger than the sought value

			if (first < 0)
				first = 0;
			else if (first > points.Length)
				first = points.Length;

			if (last > points.Length)
				last = points.Length;
			else if (last < 0)
				last = 0;

			_minBoundIndex = (int)first;
			_maxBoundIndex = (int)last;
		}

		// The old version, as of 21-Oct-2007
		//public void SetBounds2(double min, double max, int maxPts)
		//{
		//    _maxPts = maxPts;

		//    // assume data points are equally spaced, and calculate the X step size between
		//    // each data point
		//    double step = (_x[_x.Length - 1] - _x[0]) / (double)_x.Length;

		//    if (min < _x[0])
		//        min = _x[0];
		//    if (max > _x[_x.Length - 1])
		//        max = _x[_x.Length - 1];

		//    // calculate the index of the start of the bounded range
		//    int first = (int)((min - _x[0]) / step);

		//    // calculate the index of the last point of the bounded range
		//    int last = (int)((max - min) / step + first);

		//    // Make sure the bounded indices are legitimate
		//    first = Math.Max(Math.Min(first, _x.Length), 0);
		//    last = Math.Max(Math.Min(last, _x.Length), 0);

		//    _minBoundIndex = first;
		//    _maxBoundIndex = last;
		//}

	#endregion

	}
}
