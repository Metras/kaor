// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//			Maxim Anisenkov <anisenkov@niistt.ru>
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

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.Trace;
using KaorCore.Utils;

using RPURPV18.I18N;
using RPURPV18.SignalConverter;
using RPURPV18.SignalConverterManager;

namespace RPURPV18.Trace
{
	public partial class SetupScanParams : Form
	{
		BaseTrace trace;
		RPU18TraceScanParams scanParams;

		public SetupScanParams()
		{
			InitializeComponent();
		}

		public BaseTrace Trace
		{
			get
			{
				return trace;
			}
			set
			{
				trace = value;

				UpdateControl();
			}
		}

		public TraceScanParams ScanParams
		{
			get
			{
				return scanParams;
			}

			set
			{
				if (value is RPU18TraceScanParams)
					scanParams = (RPU18TraceScanParams)value;
				else
					throw new InvalidCastException();
				UpdateControl();
			}
		}

		private void UpdateControl()
		{
			if (trace == null)
				return;

			lblFstart.Text = FreqUtils.FreqToString(trace.Fstart);
			lblFstop.Text = FreqUtils.FreqToString(trace.Fstop);

			lblMeasureStep.Text = FreqUtils.FreqToString(trace.MeasureStep);

			txtTraceName.Text = trace.Name + "\r\n" + trace.Description;


			if (scanParams != null)
			{
				switch (scanParams.FilterBand)
				{
					case 1000:
						cmbFilterBand.SelectedIndex = 0;
						break;

					case 3000:
						cmbFilterBand.SelectedIndex = 1;
						break;

					case 7500:
						cmbFilterBand.SelectedIndex = 2;
						break;

					case 10000:
						cmbFilterBand.SelectedIndex = 3;
						break;

					case 30000:
						cmbFilterBand.SelectedIndex = 4;
						break;

					case 100000:
						cmbFilterBand.SelectedIndex = 5;
						break;

					case 120000:
						cmbFilterBand.SelectedIndex = 6;
						break;

					case 280000:
						cmbFilterBand.SelectedIndex = 7;
						break;

					default:
						cmbFilterBand.SelectedIndex = 5;
						break;
				}

				switch (scanParams.AverageTime)
				{
					case 10:
						cmbAvgTime.SelectedIndex = 0;
						break;
					case 20:
						cmbAvgTime.SelectedIndex = 1;
						break;
					case 50:
						cmbAvgTime.SelectedIndex = 2;
						break;
					case 100:
						cmbAvgTime.SelectedIndex = 3;
						break;
					case 500:
						cmbAvgTime.SelectedIndex = 4;
						break;
					default:
						cmbAvgTime.SelectedIndex = 1;
						break;
				}

				cmbAntennas.Items.Clear();
				cmbAntennas.Items.AddRange(scanParams.RPU.Antennas.ToArray());

				if (scanParams.Antenna != null)
					cmbAntennas.SelectedItem = scanParams.Antenna;
				else if (cmbAntennas.Items.Count > 0)
					cmbAntennas.SelectedIndex = 0;

				// Загрузка списка конвертеров сигнала
				cmbSignalConverter.Items.Clear();
				cmbSignalConverter.Items.AddRange(BaseSignalConverterManager.ConvertersArray);

				if (scanParams.SignalConverter != null)
					cmbSignalConverter.SelectedItem = scanParams.SignalConverter;
				else if (cmbSignalConverter.Items.Count > 0)
				    cmbSignalConverter.SelectedIndex = 0;
			}
			else
			{
				cmbAvgTime.SelectedIndex = 1;
				cmbFilterBand.SelectedIndex = 5;
				cmbAntennas.Items.Clear();
				cmbSignalConverter.Items.Clear();
			}
		}

		/// <summary>
		/// Обработка нажатия ОК
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (trace.Fstart < 20000000 || trace.Fstart > 18000000000 ||
				trace.Fstop < 20000000 || trace.Fstop > 18000000000)
			{
				MessageBox.Show(Locale.invalid_trace, Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!(
				trace.Fstart >= ((ISignalConverter)cmbSignalConverter.SelectedItem).MinFreq &&
				trace.Fstart <= ((ISignalConverter)cmbSignalConverter.SelectedItem).MaxFreq &&
				trace.Fstop >= ((ISignalConverter)cmbSignalConverter.SelectedItem).MinFreq &&
				trace.Fstop <= ((ISignalConverter)cmbSignalConverter.SelectedItem).MaxFreq
				))
			{
				MessageBox.Show(Locale.invalid_trace_band, Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			switch (cmbAvgTime.SelectedIndex)
			{
				case 0:
					scanParams.AverageTime = 10;
					break;

				case 1:
					scanParams.AverageTime = 20;
					break;

				case 2:
					scanParams.AverageTime = 50;
					break;

				case 3:
					scanParams.AverageTime = 100;
					break;

				case 4:
					scanParams.AverageTime = 500;
					break;

				default:
					scanParams.AverageTime = 20;
					break;
			}

			switch (cmbFilterBand.SelectedIndex)
			{
				case 0:
					scanParams.FilterBand = 1000;
					break;

				case 1:
					scanParams.FilterBand = 3000;
					break;

				case 2:
					scanParams.FilterBand = 7500;
					break;

				case 3:
					scanParams.FilterBand = 10000;
					break;

				case 4:
					scanParams.FilterBand = 30000;
					break;

				case 5:
					scanParams.FilterBand = 100000;
					break;

				case 6:
					scanParams.FilterBand = 120000;
					break;

				case 7:
					scanParams.FilterBand = 280000;
					break;

				default:
					scanParams.FilterBand = 100000;
					break;
			}

			if (scanParams.FilterBand != trace.MeasureStep)
			{
				if (MessageBox.Show(String.Format(Locale.warn_step_change, 
					FreqUtils.FreqToString(trace.MeasureStep),
					FreqUtils.FreqToString(scanParams.FilterBand)),
					Locale.confirmation,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					//trace.UserEditParams();
					trace.MeasureStep = scanParams.FilterBand;
					trace.Reset();
				}
			}
			scanParams.Antenna = cmbAntennas.SelectedItem as IAntenna;
			scanParams.SignalConverter = (ISignalConverter)cmbSignalConverter.SelectedItem;
			if(scanParams.Antenna == null)
			{
				MessageBox.Show(Locale.err_invalid_antenna, Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Question);
				DialogResult = DialogResult.None;
			}
			DialogResult = DialogResult.OK;
			Close();
		}

		private void txtTraceName_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
