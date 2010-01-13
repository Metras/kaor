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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ControlUtils.ObjectListView;
using KaorCore.I18N;

using KaorCore.Trace;
using KaorCore.Marker;
using ZedGraph;

namespace KaorCore.RadioControlSystem
{
	public partial class CreateTraceMarkersDialog : Form
	{
		const double powerStep = 1.0;

		BaseTrace trace;
		List<BaseRadioMarker> markers;
		double powerMin;
		double powerMax;
		BoxObj rangeBox;
		PointPairList barsPoints;
		BarItem bars;

		public CreateTraceMarkersDialog()
		{
			markers = new List<BaseRadioMarker>();

			InitializeComponent();
			olvFrequency.AspectGetter = delegate(object pO) { return ((BaseRadioMarker)pO).Frequency; };
			olvPower.AspectGetter = delegate(object pO) { return ((BaseRadioMarker)pO).Power; };

			olvMarkers.SetObjects(markers);

			zgPower.GraphPane.Title.Text = Locale.power_allocation;
			zgPower.GraphPane.XAxis.Title.Text = Locale.power;// +", " + Locale.dbm;
			zgPower.GraphPane.YAxis.Title.Text = Locale.measures_count;
		}

		/// <summary>
		/// Обновление контрола
		/// </summary>
		void UpdateControl()
		{
			int _i;
			TracePoint[] _points = trace.TracePoints;

			powerMin = 1000.0;
			powerMax = -1000.0;

			for (_i = 0; _i < _points.Length; _i++)
			{
				if (_points[_i].Power == TracePoint.POWER_UNDEFINED)
					continue;

				if (powerMin > _points[_i].Power)
					powerMin = _points[_i].Power;

				if (powerMax < _points[_i].Power)
					powerMax = _points[_i].Power;
			}
			powerMin = (Math.Round((powerMin / 10.0)) - 1.0) * 10.0;
			powerMax = (Math.Round(powerMax / 10.0) + 1.0) * 10.0;
			
			zgPower.GraphPane.XAxis.Scale.Min = powerMin;
			zgPower.GraphPane.XAxis.Scale.Max = powerMax;

			rangeBox = new BoxObj(powerMin, trace.TracePoints.Length, powerMax - powerMin, trace.TracePoints.Length);
			rangeBox.Location.CoordinateFrame = CoordType.AxisXYScale;
			rangeBox.Fill.Type = FillType.Solid;
			rangeBox.Border.IsVisible = false;
			rangeBox.IsVisible = true;
			rangeBox.IsClippedToChartRect = true;
			rangeBox.Fill.Color = Color.FromArgb(64, Color.Tomato);

			pwrMin.Minimum = (decimal)powerMin;
			pwrMin.Maximum = (decimal)powerMax;

			pwrMax.Minimum = (decimal)powerMin;
			pwrMax.Maximum = (decimal)powerMax;

			pwrMax.Value = (decimal)powerMax;
			pwrMin.Value = (decimal)powerMin;

			barsPoints = new PointPairList();

			for (_i = 0; _i < (powerMax - powerMin + 1) * powerStep; _i++)
			{
				var q = from _pnt in trace.TracePoints
						where _pnt.Power > (powerMin + _i*powerStep) &&
						_pnt.Power < (powerMin + (_i + 1) * powerStep)
						select _pnt;

				barsPoints.Add(powerMin + _i*powerStep, (double)q.Count());
			}

			LineItem _line = zgPower.GraphPane.AddCurve("", barsPoints, 
				Color.LimeGreen, SymbolType.None);
			_line.Line.StepType = StepType.ForwardStep;
			
			zgPower.GraphPane.GraphObjList.Add(rangeBox);

			zgPower.AxisChange();
			zgPower.Invalidate();
			

		}

		public BaseTrace Trace
		{
			get { return trace; }
			set 
			{ 
				trace = value;
				UpdateControl();
			}
		}

		public List<BaseRadioMarker> Markers
		{
			get { return markers; }
		}

		private void pwrMin_ValueChanged(object sender, EventArgs e)
		{
			rangeBox.Location.X = (double)pwrMin.Value;
			rangeBox.Location.Width = (double)(pwrMax.Value - pwrMin.Value);
			pwrMax.Minimum = pwrMin.Value;
			zgPower.Invalidate();
		}

		private void pwrMax_ValueChanged(object sender, EventArgs e)
		{
			rangeBox.Location.Width = (double)(pwrMax.Value - pwrMin.Value);
			pwrMin.Maximum = pwrMax.Value;
			zgPower.Invalidate();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			var q = from _pnt in trace.TracePoints
					where _pnt.Power > (double)(pwrMin.Value) &&
					_pnt.Power < (double)(pwrMax.Value)
					select _pnt;

			markers.Clear();

			foreach (TracePoint _pnt in q)
			{
				markers.Add(new BaseRadioMarker(_pnt.Freq, trace.Name, _pnt.Power, 
					clrMarker.Value));
			}

			olvMarkers.BuildList(false);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if(olvMarkers.SelectedItems.Count == 0)
				return;

			if (MessageBox.Show(Locale.confirm_delete_selected_markers,
				Locale.confirmation,
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				return;

			foreach (OLVListItem _item in olvMarkers.SelectedItems)
			{
				markers.Remove(_item.RowObject as BaseRadioMarker);
			}

			olvMarkers.BuildList(true);
		}

		private void btnClearAll_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Locale.confirm_delete_all_markers, 
				Locale.confirmation,
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				return;

			markers.Clear();
			olvMarkers.BuildList(false);
		}

		/// <summary>
		///  Изменение цвета маркеров
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clrMarker_ValueChanged(object sender, EventArgs e)
		{
			foreach (BaseRadioMarker _marker in markers)
				_marker.LineItem.Color = clrMarker.Value;
		}
	}

}
