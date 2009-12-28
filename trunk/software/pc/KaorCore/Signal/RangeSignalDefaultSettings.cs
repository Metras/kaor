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

using KaorCore.RadioControlSystem;
using KaorCore.RPU;
using KaorCore.RPUManager;

namespace KaorCore.Signal
{
	public partial class RangeSignalDefaultSettings : Form
	{
		RangeSignalParams signalParams;
		List<IRPU> rpus;

		public RangeSignalDefaultSettings(RangeSignalParams pSignalParams)
		{
			signalParams = pSignalParams;

			InitializeComponent();

			

			chkFixDeltaPlus.Checked = signalParams.IsFixDeltaPlus;
			chkFixDeltaMinus.Checked = signalParams.IsFixDeltaMinus;
			chkIncreaseBand.Checked = signalParams.IncreaseRange;
			chkDemodulate.Checked = signalParams.IsRecord;
			numPauseTime.Value = signalParams.PauseTime;
			numBandMultiplier.Value = signalParams.BandMultiplier;

			numPmin.Value = (decimal)signalParams.Pmin;
			numPmax.Value = (decimal)signalParams.Pmax;

			rpus = BaseRadioControlSystem.Instance.RPUManager.AvailableRPUDevices;
			cmbRPU.Items.Clear();

			var qRPU = from _r in BaseRadioControlSystem.Instance.RPUManager.AvailableRPUDevices
					where _r.HasDemodulator == true
					select _r;

			cmbRPU.Items.AddRange(qRPU.ToArray());

			//cmbRPU.Items.AddRange(rpus.ToArray());

			if (cmbRPU.Items.Count > 0)
			{
				IEnumerable<IRPU> q = from _r in rpus
									  where _r.Id == signalParams.RecordRPUId
									  select _r;

				if (q.Count() > 0)
				{
					cmbRPU.SelectedItem = q.First();
				}
				else
				{
					cmbRPU.SelectedIndex = -1;
					numPauseTime.Enabled = false;
					btnRecordParams.Enabled = false;
					chkDemodulate.Enabled = false;

				}
			}
			else
			{
				cmbRPU.Enabled = false;
				numPauseTime.Enabled = false;
				btnRecordParams.Enabled = false;
				chkDemodulate.Enabled = false;
			}

			

			switch (signalParams.SignalType)
			{
				case ESignalType.Unknown:
					cmbSignalType.SelectedIndex = 0;
					break;

				case ESignalType.Red:
					cmbSignalType.SelectedIndex = 1;
					break;

				case ESignalType.Yellow:
					cmbSignalType.SelectedIndex = 2;
					break;

				case ESignalType.Green:
					cmbSignalType.SelectedIndex = 3;
					break;

				default:
					cmbSignalType.SelectedIndex = 0;
					break;
			}
		}

		private void chkDemodulate_CheckedChanged(object sender, EventArgs e)
		{
			if (chkDemodulate.Checked)
			{
				cmbRPU.Enabled = true;
				numPauseTime.Enabled = true;
				btnRecordParams.Enabled = true;
			}
			else
			{
				cmbRPU.Enabled = false;
				numPauseTime.Enabled = false;
				btnRecordParams.Enabled = false;
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			switch (cmbSignalType.SelectedIndex)
			{
				case 0:
					signalParams.SignalType = ESignalType.Unknown;
					break;

				case 1:
					signalParams.SignalType = ESignalType.Red;
					break;

				case 2:
					signalParams.SignalType = ESignalType.Yellow;
					break;

				case 3:
					signalParams.SignalType = ESignalType.Green;
					break;

				default:
					signalParams.SignalType = ESignalType.Unknown;
					break;
			}

			signalParams.IsRecord = chkDemodulate.Checked;

			if (signalParams.IsRecord)
			{
				IRPU _rpu = cmbRPU.SelectedItem as IRPU;

				if (_rpu != null)
				{
					signalParams.RecordRPUId = _rpu.Id;
				}
				else
					signalParams.IsRecord = false;

				signalParams.PauseTime = (int)numPauseTime.Value;
			}

			signalParams.IsFixDeltaPlus = chkFixDeltaPlus.Checked;
			signalParams.IsFixDeltaMinus = chkFixDeltaMinus.Checked;
			signalParams.IncreaseRange = chkIncreaseBand.Checked;
			signalParams.BandMultiplier = (int)numBandMultiplier.Value;
			signalParams.Pmin = (int)numPmin.Value;
			signalParams.Pmax = (int)numPmax.Value;
		}

		private void cmbRPU_SelectedIndexChanged(object sender, EventArgs e)
		{
			IRPU _rpu = cmbRPU.SelectedItem as IRPU;

			if (_rpu == null)
			{
				btnRecordParams.Enabled = false;
				numPauseTime.Enabled = false;
				btnRecordParams.Enabled = false;
				chkDemodulate.Enabled = false;
				return;
			}
			else
			{
				numPauseTime.Enabled = true;
				btnRecordParams.Enabled = true;
				chkDemodulate.Enabled = true;

				if(signalParams.RecordRPUId != _rpu.Id)
					signalParams.DefaultRecordParams = _rpu.DefaultSignalRecordParams;
			}
			
		}

		private void btnRecordParams_Click(object sender, EventArgs e)
		{
			IRPU _rpu = cmbRPU.SelectedItem as IRPU;

			if (_rpu == null)
				return;

			_rpu.ShowRecordParamsDialog(signalParams.DefaultRecordParams);
		}
	}
}
