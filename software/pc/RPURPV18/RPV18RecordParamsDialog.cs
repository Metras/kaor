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

using KaorCore.Antenna;
using KaorCore.Report;
using KaorCore.RPU;

namespace RPURPV18
{
	public partial class RPV18RecordParamsDialog : Form
	{
		RPV18RecordSignalParams recordParams;

		public RPV18RecordParamsDialog(RPV18RecordSignalParams pParams, CRPURPV18 pRPU)
		{
			recordParams = pParams;

			InitializeComponent();

			/// Установка параметров диалогового окна
			switch (recordParams.FilterBand)
			{
				case 3000:
					cmbFilter.SelectedIndex = 0;
					break;
				case 6000:
					cmbFilter.SelectedIndex = 1;
					break;
				case 15000:
					cmbFilter.SelectedIndex = 2;
					break;
				case 50000:
					cmbFilter.SelectedIndex = 3;
					break;
				case 230000:
					cmbFilter.SelectedIndex = 4;
					break;

				default:
					cmbFilter.SelectedIndex = 0;
					break;
			}

			switch (recordParams.Modulation)
			{
				case EAudioModulationType.FM:
					cmbDemod.SelectedIndex = 0;
					break;
				case EAudioModulationType.AM:
					cmbDemod.SelectedIndex = 1;
					break;
				case EAudioModulationType.LSB:
					cmbDemod.SelectedIndex = 2;
					break;
				case EAudioModulationType.USB:
					cmbDemod.SelectedIndex = 3;
					break;
				case EAudioModulationType.CW:
					cmbDemod.SelectedIndex = 4;
					break;

				default:
					cmbDemod.SelectedIndex = 0;
					break;

			}

			cmbAntenna.Items.AddRange(pRPU.Antennas.ToArray());
			if (cmbAntenna.Items.Count > 0)
				cmbAntenna.SelectedIndex = 0;

			chkSaveRecord.Checked = recordParams.NeedSave;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			switch (cmbFilter.SelectedIndex)
			{
				case 0:
					recordParams.FilterBand = 3000;
					break;

				case 1:
					recordParams.FilterBand = 6000;
					break;

				case 2:
					recordParams.FilterBand = 15000;
					break;

				case 3:
					recordParams.FilterBand = 50000;
					break;

				case 4:
					recordParams.FilterBand = 230000;
					break;

				default:
					recordParams.FilterBand = 3000;
					break;
			}

			switch (cmbDemod.SelectedIndex)
			{
				case 0:
					recordParams.Modulation = EAudioModulationType.FM;
					break;

				case 1:
					recordParams.Modulation = EAudioModulationType.AM;
					break;

				case 2:
					recordParams.Modulation = EAudioModulationType.LSB;
					break;

				case 3:
					recordParams.Modulation = EAudioModulationType.USB;
					break;

				case 4:
					recordParams.Modulation = EAudioModulationType.CW;
					break;
			}

//			recordParams.RecordPath = txtPath.Text;

			recordParams.NeedSave = chkSaveRecord.Checked;
			recordParams.Antenna = (IAntenna)cmbAntenna.SelectedItem;
		}

		private void chkSaveRecord_CheckedChanged(object sender, EventArgs e)
		{
#if false
			if (chkSaveRecord.Checked)
			{
				btnSelectPath.Enabled = true;
				txtPath.Enabled = true;

			}
			else
			{
				btnSelectPath.Enabled = false;
				txtPath.Enabled = false;
			}
#endif
		}
	}
}
