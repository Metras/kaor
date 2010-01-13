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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using KaorCore.Utils;
using KaorCore.ZedGraphAddons;
using SignalAnalyzer.Core;
using SignalAnalyzer.I18N;
using ZedGraph;

namespace SignalAnalyzer
{
	public enum ESpectrumMode
	{
		ClearWrite,
		MinHold,
		MaxHold,
		Average
	}

	public partial class SpectrumControl : UserControl
	{
		ISpectrumAnalyzer analyzer;
		int spectrumTimeout;
		LineItem fftLine;
		MyFilteredPointList fftPoints;
		TextObj textLabels;
		double[] fftPower;
		ESpectrumMode spectrumMode;

		double averageCoeff = 0.1;

		public SpectrumControl(ISpectrumAnalyzer pAnalyzer)
		{
			analyzer = pAnalyzer;
			spectrumTimeout = 50;
			InitializeComponent();

			AverageCoeff = 0.1;
			SpectrumMode = ESpectrumMode.ClearWrite;
			

			double[] _x = new double[(int)(analyzer.Span / analyzer.RBW) + 1];
			fftPower = new double[(int)(analyzer.Span / analyzer.RBW) + 1];
			for (int _i = 0; _i < _x.Length; _i++)
			{
				_x[_i] = -analyzer.Span / 2.0 + analyzer.RBW * _i;
				fftPower[_i] = -125.0;
			}

			fftPoints = new MyFilteredPointList(_x, fftPower);
			fftLine = zedFFT.GraphPane.AddCurve("FFT", fftPoints, Color.BlueViolet);
			fftLine.Symbol.Type = SymbolType.None;

			zedFFT.GraphPane.XAxis.Scale.Min = -analyzer.Span / 2;
			zedFFT.GraphPane.XAxis.Scale.Max = analyzer.Span / 2;
			zedFFT.GraphPane.YAxis.Scale.Max = -30.0;
			zedFFT.GraphPane.YAxis.Scale.Min = -100.0;
			zedFFT.GraphPane.YAxis.Title.IsVisible = false;
			zedFFT.GraphPane.XAxis.Title.IsVisible = false;
			zedFFT.GraphPane.XAxis.MajorGrid.IsVisible = true;
			zedFFT.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zedFFT.GraphPane.YAxis.Scale.FontSpec.Size = 8;
			zedFFT.GraphPane.XAxis.Scale.FontSpec.Size = 8;
			zedFFT.GraphPane.XAxis.Scale.MinScrollRange = -analyzer.Span / 2;
			zedFFT.GraphPane.XAxis.Scale.MaxScrollRange = analyzer.Span / 2;
			zedFFT.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			zedFFT.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;

			zedFFT.GraphPane.YAxis.Scale.MinScrollRange = -125.0;
			zedFFT.GraphPane.YAxis.Scale.MaxScrollRange = 30.0;
			zedFFT.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			zedFFT.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zedFFT.IsEnableVZoom = false;
			zedFFT.GraphPane.Legend.IsVisible = false;
			zedFFT.IsAntiAlias = false;

			zedFFT.GraphPane.Title.FontSpec.Size = 6;
			zedFFT.GraphPane.Title.FontSpec.StringAlignment = StringAlignment.Near;
			zedFFT.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zedFFT_ZoomEvent);
			//zedFFT.GraphPane.Title.IsVisible = false;
			//zedFFT.GraphPane.TitleGap = 0.0f;

			//zedFFT.GraphPane.GraphObjList.Add(textLabels);
			
			UpdateTextLabels();

			zedFFT.AxisChange();
			zedFFT.Invalidate();

			RescaleFFT();
		}

		void zedFFT_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{
			RescaleFFT();
		}


		void RescaleFFT()
		{
			fftPoints.SetBounds(zedFFT.GraphPane.XAxis.Scale.Min, zedFFT.GraphPane.XAxis.Scale.Max, 512);
		}

		string SpectrumModeString
		{
			get
			{
				string _res;

				_res = Locale.unknown;

				switch (spectrumMode)
				{
					case ESpectrumMode.ClearWrite:
						_res = Locale.clear_write;
						break;

					case ESpectrumMode.MinHold:
						_res = Locale.min_hold;
						break;

					case ESpectrumMode.MaxHold:
						_res = Locale.max_hold;
						break;

					case ESpectrumMode.Average:
						_res = Locale.average;
						break;
				}

				return _res;
			}
		}

		void UpdateTextLabels()
		{
			zedFFT.GraphPane.Title.Text = String.Format("Freq: {0} Span: {1} Mode: {2}",
				FreqUtils.FreqToString(analyzer.BaseFreq), 
				FreqUtils.FreqToString(analyzer.Span), 
				SpectrumModeString);

			zedFFT.Invalidate();
		}

		private void centerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CenterSpanForm _form = new CenterSpanForm();
			_form.CenterFrequency = analyzer.BaseFreq;
			_form.Span = analyzer.Span;

			if (_form.ShowDialog() == DialogResult.OK)
			{
				analyzer.BaseFreq = _form.CenterFrequency;
				analyzer.Span = _form.Span;

				UpdateTextLabels();
			}
		}

		delegate void UpdateFFTDataDelegate(double[] _pData);

		/// <summary>
		/// Обновление спектрограммы в зависимости от режима работы спектрографа
		/// </summary>
		/// <param name="pFFTData">Полученные отсчеты спектрографа</param>
		void UpdateFFTData(double[] pFFTData)
		{
			if (!InvokeRequired)
			{
				int _min = (pFFTData.Length < fftPower.Length) ? pFFTData.Length : fftPower.Length;

				switch (spectrumMode)
				{
					case ESpectrumMode.MaxHold:
						for (int _i = 0; _i < _min; _i++)
						{
							if (fftPower[_i] < pFFTData[_i])
								fftPower[_i] = pFFTData[_i];
						}
						break;

					case ESpectrumMode.MinHold:
						for (int _i = 0; _i < _min; _i++)
						{
							if(fftPower[_i] > pFFTData[_i])
								fftPower[_i] = pFFTData[_i];
						}
						break;

					case ESpectrumMode.Average:
						for (int _i = 0; _i < _min; _i++)
						{
							fftPower[_i] = fftPower[_i] * (1 - averageCoeff) + pFFTData[_i] * averageCoeff;
						}
						break;

					default:
						for (int _i = 0; _i < _min; _i++)
						{
							fftPower[_i] = pFFTData[_i];
						}
						break;
				}

				zedFFT.Invalidate();
			}
			else
				Invoke(new UpdateFFTDataDelegate(UpdateFFTData), pFFTData);
		}

		void SpectrumRun()
		{
			double[] _fftData = analyzer.GetFFTData(analyzer.BaseFreq);

			UpdateFFTData(_fftData);
		}

		private void bgwSpectrum_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				while (!bgwSpectrum.CancellationPending)
				{
					SpectrumRun();
					Thread.Sleep(spectrumTimeout);
				}
			}
			catch(Exception pEx)
			{
				MessageBox.Show(pEx.Message);
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (bgwSpectrum.IsBusy)
			{
				bgwSpectrum.CancelAsync();
			}
			else
			{
				bgwSpectrum.RunWorkerAsync();
			}
		}

		void ResetFFTData()
		{
		}

		ESpectrumMode SpectrumMode
		{
			get
			{
				return spectrumMode;
			}

			set
			{
				spectrumMode = value;

				switch (spectrumMode)
				{
					case ESpectrumMode.Average:
						averageToolStripMenuItem.Checked = true;
						clearWriteToolStripMenuItem.Checked = false;
						minHoldToolStripMenuItem.Checked = false;
						maxHoldToolStripMenuItem.Checked = false;
						break;

					case ESpectrumMode.ClearWrite:
						averageToolStripMenuItem.Checked = false;
						clearWriteToolStripMenuItem.Checked = true;
						minHoldToolStripMenuItem.Checked = false;
						maxHoldToolStripMenuItem.Checked = false;
						break;

					case ESpectrumMode.MaxHold:
						averageToolStripMenuItem.Checked = false;
						clearWriteToolStripMenuItem.Checked = false;
						minHoldToolStripMenuItem.Checked = false;
						maxHoldToolStripMenuItem.Checked = true;
						break;

					case ESpectrumMode.MinHold:
						averageToolStripMenuItem.Checked = false;
						clearWriteToolStripMenuItem.Checked = false;
						minHoldToolStripMenuItem.Checked = true;
						maxHoldToolStripMenuItem.Checked = false;
						break;
				}

				UpdateTextLabels();
			}
		}

		private void clearWriteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SpectrumMode = ESpectrumMode.ClearWrite;
		}

		private void minHoldToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SpectrumMode = ESpectrumMode.MinHold;
		}

		private void maxHoldToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SpectrumMode = ESpectrumMode.MaxHold;
		}

		private void averageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SpectrumMode = ESpectrumMode.Average;
		}

		double AverageCoeff
		{
			get
			{
				return averageCoeff;
			}

			set
			{
				averageCoeff = value;

				SpectrumMode = ESpectrumMode.Average;

				timesToolStripMenuItem.Checked = false;
				timesToolStripMenuItem1.Checked = false;
				timesToolStripMenuItem2.Checked = false;
				timesToolStripMenuItem3.Checked = false;

				if (averageCoeff == 0.2)
				{
					timesToolStripMenuItem.Checked = true;
				}
				else if (averageCoeff == 0.1)
				{
					timesToolStripMenuItem1.Checked = true;
				}
				else if (averageCoeff == 0.02)
				{
					timesToolStripMenuItem2.Checked = true;
				}
				else if (averageCoeff == 0.01)
				{
					timesToolStripMenuItem3.Checked = true;
				}

				UpdateTextLabels();
			}
		}

		private void timesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _item = sender as ToolStripMenuItem;

			if (_item == null)
				return;

			try
			{
				double _times = double.Parse((string)_item.Tag);
				AverageCoeff = 1.0 / (double)_times;
			}
			catch
			{
				AverageCoeff = 0.1;
			}
		}
	}
}
