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
using System.Windows.Forms;

using KaorCore.I18N;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.RadioControlSystem;
using KaorCore.Report;
using KaorCore.Undo;

namespace KaorCore.Signal
{
	public partial class RangeSignalControl : UserControl
	{
		RangeSignal signal;

		/// <summary>
		/// Стек анду для редактирования сигналов
		/// </summary>
		UndoStack<RangeSignal> undoStack;

		public RangeSignalControl(RangeSignal pSignal)
		{
			
			undoStack = new UndoStack<RangeSignal>(pSignal);

			InitializeComponent();

			signal = pSignal;

			bndSignal.DataSource = signal;
			bndSignal.ResetBindings(false);

			switch (signal.SignalType)
			{
				case ESignalType.Green:
					cmbSignalType.SelectedIndex = 3;
					break;
				case ESignalType.Yellow:
					cmbSignalType.SelectedIndex = 2;
					break;
				case ESignalType.Red:
					cmbSignalType.SelectedIndex = 1;
					break;
				default:
					cmbSignalType.SelectedIndex = 0;
					break;

			}

			if (!signal.IsRecord)
			{
				lstRPU.Enabled = false;
				btnRecordParams.Enabled = false;
			}

			/// Заполнение списка РПУ с поддержкой демодулятора
			/// 
			var q = from _r in BaseRadioControlSystem.Instance.RPUManager.AvailableRPUDevices
					where _r.HasDemodulator == true
					select _r;

			lstRPU.Items.AddRange(q.ToArray());
			lstRPU.SelectedItem = signal.RecordRPU;

			/// Заполнение списка записей сигнала
			/// 
			olvRecordTime.AspectGetter = delegate(object pO) { return ((ReportItem)pO).Timestamp.ToLongTimeString(); };
			olvRecordTime.GroupKeyGetter = delegate(object pO) { return ((ReportItem)pO).Timestamp.Date; };
			olvRecordTime.GroupKeyToTitleConverter = delegate(object pKey)
			{
				DateTime _date = (DateTime)pKey;
				if (_date == DateTime.Now.Date)
					return Locale.today;
				else
					return _date.ToShortDateString();
			};

			olvRecordName.AspectGetter = delegate(object pO) { return ((ReportItem)pO).Name; };
#if USE_SIGNAL_REPORTS
			lstRecords.SetObjects(signal.Reports);
#endif
			signal.OnSignalChanged += new BaseSignal.SignalChangedDelegate(signal_OnSignalChanged);
		}

		delegate void OnSignalChangedDelegate(BaseSignal pSignal);

		/// <summary>
		/// Обработчик изменения сигнала
		/// </summary>
		/// <param name="pSignal"></param>
		void signal_OnSignalChanged(BaseSignal pSignal)
		{
			if (!lstRecords.InvokeRequired)
			{
				bndSignal.ResetBindings(false);
				lstRecords.BuildList(true);
			}
			else
				lstRecords.Invoke(new OnSignalChangedDelegate(signal_OnSignalChanged), pSignal);
		}

		private void frequencyTextBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void RangeSignalControl_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
				bndSignal.ResetBindings(false);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			/// Запись полей
			/// 
			if (OnSignalEditComplete != null)
				OnSignalEditComplete(signal);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			signal = undoStack.OriginalObject;
			undoStack.Reset();

			bndSignal.DataSource = signal;
			bndSignal.ResetBindings(false);
		}

		public RangeSignal Signal
		{
			get
			{
				return signal;
			}

			set
			{
				signal = value;
				bndSignal.DataSource = signal;
				bndSignal.ResetBindings(false);
			}
		}

		public delegate void SignalEditComplete(RangeSignal pSignal);
		public event SignalEditComplete OnSignalEditComplete;

		private void cmbSignalType_SelectedIndexChanged(object sender, EventArgs e)
		{
			int _idx = cmbSignalType.SelectedIndex;

			switch (_idx)
			{
				case 0:
					signal.SignalType = ESignalType.Unknown;
					break;
				case 1:
					signal.SignalType = ESignalType.Red;
					break;
				case 2:
					signal.SignalType = ESignalType.Yellow;
					break;
				case 3:
					signal.SignalType = ESignalType.Green;
					break;
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (chkRecord.Checked == true)
			{
				lstRPU.Enabled = true;
				btnRecordParams.Enabled = true;
				numDemodPause.Enabled = true;
			}
			else
			{
				lstRPU.Enabled = false;
				btnRecordParams.Enabled = false;
				numDemodPause.Enabled = false;
			}
		}

		private void btnRecordParams_Click(object sender, EventArgs e)
		{
			signal.ShowRecordParams();
		}

		private void btnClearHits_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Locale.confirm_clear_hits,
				Locale.confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				signal.ClearHitsCount();

				bndSignal.ResetBindings(false);
			}
		}
	}
}
