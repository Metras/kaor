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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.Trace;
using KaorCore.Utils;

using RPUICOMR8500.I18N;

namespace RPUICOMR8500.Trace
{
	public partial class SetupScanParams : Form
	{
		BaseTrace trace;
		TraceScanParams scanParams;

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
				scanParams = value;
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
					case 2200:
						cmbFilterBand.SelectedIndex = 0;
						break;

					case 5500:
						cmbFilterBand.SelectedIndex = 1;
						break;

					case 12000:
						cmbFilterBand.SelectedIndex = 2;
						break;

					case 150000:
						cmbFilterBand.SelectedIndex = 3;
						break;

					default:
						cmbFilterBand.SelectedIndex = 3;
						break;
				}

				cmbAntennas.Items.Clear();
				cmbAntennas.Items.AddRange(scanParams.RPU.Antennas.ToArray());

				if (scanParams.Antenna != null)
					cmbAntennas.SelectedItem = scanParams.Antenna;
				else if (cmbAntennas.Items.Count > 0)
				{
					cmbAntennas.SelectedIndex = 0;
				}
			}
			else
			{
				cmbFilterBand.SelectedIndex = 3;
				cmbAntennas.Items.Clear();
			}


		}

		/// <summary>
		/// Обработка нажатия ОК
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (trace.Fstart < 100000 || trace.Fstart > 2000000000 ||
					trace.Fstop < 100000 || trace.Fstop > 2000000000)
			{
				MessageBox.Show(Locale.invalid_trace, Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			switch (cmbFilterBand.SelectedIndex)
			{
				case 0:
					scanParams.FilterBand = 2200;
					break;

				case 1:
					scanParams.FilterBand = 5500;
					break;

				case 2:
					scanParams.FilterBand = 12000;
					break;

				case 3:
					scanParams.FilterBand = 150000;
					break;

				default:
					scanParams.FilterBand = 12000;
					break;
			}

			/// Проверка на правильность заполнения диапазона трассы и ширины измерителя мощности
			if (scanParams.FilterBand == 150000 &&
				(trace.Fstart < 30000000 ||
				trace.Fstop < 30000000))
			{
				MessageBox.Show(String.Format(Locale.wfm_error_long, 
					FreqUtils.FreqToString(trace.Fstart), FreqUtils.FreqToString(trace.Fstop)), 
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				DialogResult = DialogResult.None;
				return;
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

			if(scanParams.Antenna == null)
			{
				MessageBox.Show(Locale.err_invalid_antenna,
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Question);

				DialogResult = DialogResult.None;
			}
		}
	}
}
