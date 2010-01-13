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
using System.Text;
using System.Windows.Forms;

using KaorCore.Antenna;
using KaorCore.RPU;

using RPUICOMR8500;

namespace RPUICOMR8500
{
	public partial class R8500RecordParamsDialog : Form
	{
		R8500RecordSignalParams recordParams;
		RPUR8500 rpu;

		public R8500RecordParamsDialog(R8500RecordSignalParams pParams, RPUR8500 pRPU)
		{
			recordParams = pParams;
			rpu = pRPU;

			InitializeComponent();

			switch (recordParams.Modulation)
			{
				case EAudioModulationType.FM:
					cmbDemod.SelectedIndex = 0;
					break;
                case EAudioModulationType.WFM:
                    cmbDemod.SelectedIndex = 1;
                    break;
                case EAudioModulationType.AM:
					cmbDemod.SelectedIndex = 2;
					break;
				case EAudioModulationType.CW:
					cmbDemod.SelectedIndex = 3;
					break;
                case EAudioModulationType.FM_narrow:
                    cmbDemod.SelectedIndex = 4;
                    break;
                case EAudioModulationType.AM_narrow:
                    cmbDemod.SelectedIndex = 5;
                    break;
				case EAudioModulationType.AM_wide:
					cmbDemod.SelectedIndex = 6;
					break;
				case EAudioModulationType.LSB:
					cmbDemod.SelectedIndex = 7;
					break;
				case EAudioModulationType.USB:
					cmbDemod.SelectedIndex = 8;
					break;

				default:
					cmbDemod.SelectedIndex = 0;
					break;
			}

			cmbAntenna.Items.AddRange(pRPU.Antennas.ToArray());
			if (cmbAntenna.Items.Count > 0)
				cmbAntenna.SelectedIndex = 0;

			if (pParams.NeedSave)
				chkSaveRecords.Checked = true;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			switch (cmbDemod.SelectedIndex)
			{
				case 0:
					recordParams.Modulation = EAudioModulationType.FM;
					break;
				case 1:
					recordParams.Modulation = EAudioModulationType.WFM;
					break;
				case 2:
					recordParams.Modulation = EAudioModulationType.AM;
					break;
				case 3:
					recordParams.Modulation = EAudioModulationType.CW;
					break;
				case 4:
					recordParams.Modulation = EAudioModulationType.FM_narrow;
					break;
				case 5:
					recordParams.Modulation = EAudioModulationType.AM_narrow;
					break;
				case 6:
					recordParams.Modulation = EAudioModulationType.AM_wide;
					break;
				case 7:
					recordParams.Modulation = EAudioModulationType.LSB;
					break;
				case 8:
					recordParams.Modulation = EAudioModulationType.USB;
					break;
				default:
					recordParams.Modulation = EAudioModulationType.FM;
					break;
			}

			recordParams.NeedSave = chkSaveRecords.Checked;
			recordParams.Antenna = (IAntenna)cmbAntenna.SelectedItem;
		}



        public R8500RecordSignalParams RecordParams
        {
            get
            {
                return recordParams;
            }
            set
            {
                recordParams = value;
            }
        }

	}
}
