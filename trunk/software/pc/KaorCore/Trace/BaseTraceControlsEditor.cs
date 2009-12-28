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

using KaorCore.I18N;
using KaorCore.Signal;
using KaorCore.TraceControl;

namespace KaorCore.Trace
{
	public partial class BaseTraceControlsEditor : Form
	{
		BaseTraceControl traceControl;
		//BaseTrace trace;

		public BaseTraceControlsEditor(BaseTraceControl pTraceControl)
		{
			//trace = pTrace;
			traceControl = pTraceControl;

			InitializeComponent();
			propTraceControl.SelectedObject = traceControl;

			if (traceControl.DefaultSignalType == typeof(RangeSignal))
			{
				cmbDefaultSignal.SelectedIndex = 0;
			}
			else
				cmbDefaultSignal.SelectedIndex = -1;
			//cmbDefaultSignal.SelectedIndex = 0;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			//trace.CancelEdit();
			traceControl = null;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			//trace.EndEdit();

			//trace.ClearTraceControls();

			//trace.AddTraceControl(traceControl);

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnReinit_Click(object sender, EventArgs e)
		{
			traceControl.ReInitialize();

			propTraceControl.Refresh();

			MessageBox.Show(Locale.control_reset_ok,
				Locale.information,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnDefaultSignalParams_Click(object sender, EventArgs e)
		{
#if false
			BaseSignalParams _signalParams;

			switch (cmbDefaultSignal.SelectedIndex)
			{
				case 0:
					_signalParams = new RangeSignalParams();
					break;

				default:
					_signalParams = null;
					break;
			}

			if (_signalParams == null)
			{
				MessageBox.Show("Неизвестный тип сигнала!",
					"Ошибка",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
#endif

			if (traceControl.DefaultSignalParams == null)
			{
				MessageBox.Show(Locale.no_default_signal, 
					Locale.error, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else
			{
				if (((BaseSignalParams)(traceControl.DefaultSignalParams)).SettingsForm.ShowDialog() == DialogResult.OK)
				{
					//traceControl.DefaultSignalParams = _signalParams;
					//traceControl.DefaultSignalType = _signalParams.SignalClassType;
				}
			}
		}

		private void cmbDefaultSignal_SelectedIndexChanged(object sender, EventArgs e)
		{
			BaseSignalParams _signalParams;

			switch (cmbDefaultSignal.SelectedIndex)
			{
				case 0:
					if (traceControl.DefaultSignalParams == null ||
						traceControl.DefaultSignalParams.GetType() != typeof(RangeSignalParams))
					{
						traceControl.DefaultSignalParams = new RangeSignalParams();
						traceControl.DefaultSignalType = traceControl.DefaultSignalParams.SignalClassType;
					}
					break;

				default:
					traceControl.DefaultSignalParams = null;
					_signalParams = null;
					break;
			}
		}
	}
}
