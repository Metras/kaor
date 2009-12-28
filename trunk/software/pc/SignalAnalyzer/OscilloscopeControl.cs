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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SignalAnalyzer.Core;
using KaorCore.ZedGraphAddons;
using ZedGraph;

namespace SignalAnalyzer
{
	public partial class OscilloscopeControl : UserControl
	{
		IOscilloscope device;
		LineItem realDataLine;
		LineItem imagDataLine;
		PointPairList realDataPoints;
		PointPairList imagDataPoints;
		
		double[] realData;
		double[] imagData;

		const int NSamples = 1024;

		long frequency = 145000000;

		public OscilloscopeControl(IOscilloscope pDevice)
		{
			device = pDevice;
			device.SamplesLength = NSamples;

			InitializeComponent();

			zedOscilloscope.GraphPane.Legend.IsVisible = false;
			zedOscilloscope.GraphPane.Title.IsVisible = false;

			zedOscilloscope.GraphPane.XAxis.Scale.FontSpec.Size = 8;
			zedOscilloscope.GraphPane.XAxis.Scale.Min = 0;
			zedOscilloscope.GraphPane.XAxis.Scale.Max = NSamples;
			zedOscilloscope.GraphPane.XAxis.Scale.MinScrollRange = 0;
			zedOscilloscope.GraphPane.XAxis.Scale.MaxScrollRange = NSamples;
			zedOscilloscope.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			zedOscilloscope.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;

			zedOscilloscope.GraphPane.XAxis.Title.IsVisible = false;

			zedOscilloscope.GraphPane.YAxis.Scale.FontSpec.Size = 8;
			zedOscilloscope.GraphPane.YAxis.Scale.Min = -1.0;
			zedOscilloscope.GraphPane.YAxis.Scale.Max = 1.0;
			zedOscilloscope.GraphPane.YAxis.Scale.MinScrollRange = -1.0;
			zedOscilloscope.GraphPane.YAxis.Scale.MaxScrollRange = 1.0;
			zedOscilloscope.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			zedOscilloscope.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zedOscilloscope.GraphPane.YAxis.Title.IsVisible = false;


			double []_x = new double[NSamples];
			realData = new double[NSamples];
			imagData = new double[NSamples];

			for(int _i = 0; _i<NSamples; _i++)
			{
				_x[_i] = _i;
				realData[_i] = 0.5;
				imagData[_i] = 0.8;
			}

			realDataPoints = new PointPairList(_x, realData);
			imagDataPoints = new PointPairList(_x, imagData);

			realDataLine = zedOscilloscope.GraphPane.AddCurve("Real Data", realDataPoints, Color.Blue);
			imagDataLine = zedOscilloscope.GraphPane.AddCurve("Imag Data", imagDataPoints, Color.Red);

			realDataLine.Symbol.Type = SymbolType.None;
			imagDataLine.Symbol.Type = SymbolType.None;
#if false
			realDataLine.Line.IsSmooth = true;
			imagDataLine.Line.IsSmooth = true;
			realDataLine.Line.SmoothTension = 0.5f;
			imagDataLine.Line.SmoothTension = 0.5f;
#endif

			zedOscilloscope.AxisChange();
			zedOscilloscope.Invalidate();
		}

		private void bgwRunOscilloscope_DoWork(object sender, DoWorkEventArgs e)
		{
			while (!bgwRunOscilloscope.CancellationPending)
			{
				double []_iqData = device.GetData(frequency);
				for (int _i = 0; _i < _iqData.Length / 2; _i++)
				{
					realData[_i] = _iqData[_i * 2];
					imagData[_i] = _iqData[_i * 2 + 1];
				}

				realDataPoints.SetY(realData);
				imagDataPoints.SetY(imagData);

				zedOscilloscope.Invalidate();
				Thread.Sleep(50);
			}
		}

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			if (!bgwRunOscilloscope.IsBusy)
			{
				bgwRunOscilloscope.RunWorkerAsync();
			}
			else
			{
				bgwRunOscilloscope.CancelAsync();
			}
		}

		private void showImagDataToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (showImagDataToolStripMenuItem.Checked)
			{
				imagDataLine.IsVisible = true;
			}
			else
			{
				imagDataLine.IsVisible = false;
			}
			zedOscilloscope.Invalidate();
		}

		private void showRealDataToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			if (showRealDataToolStripMenuItem.Checked)
			{
				realDataLine.IsVisible = true;
			}
			else
			{
				realDataLine.IsVisible = false;
			}
			zedOscilloscope.Invalidate();
		}

		private void toolBtnFrequency_Click(object sender, EventArgs e)
		{
			CenterSpanForm _frm = new CenterSpanForm();
			_frm.CenterFrequency = frequency;
			_frm.Span = 0;

			if (_frm.ShowDialog() == DialogResult.OK)
			{
				frequency = _frm.CenterFrequency;
			}
		}
	}
}
