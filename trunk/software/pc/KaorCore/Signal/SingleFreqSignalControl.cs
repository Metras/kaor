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
using System.Windows.Forms;

using KaorCore.I18N;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.RadioControlSystem;
using KaorCore.Report;
using KaorCore.Undo;

namespace KaorCore.Signal
{
	public partial class SingleFreqSignalControl : UserControl
	{
		SingleFreqSignal signal;

		/// <summary>
		/// Стек анду для редактирования сигналов
		/// </summary>
		UndoStack<SingleFreqSignal> undoStack;

		public SingleFreqSignalControl(SingleFreqSignal pSignal)
		{
			
			undoStack = new UndoStack<SingleFreqSignal>(pSignal);

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

			signal.OnSignalChanged += new BaseSignal.SignalChangedDelegate(signal_OnSignalChanged);
		}

		delegate void OnSignalChangedDelegate(BaseSignal pSignal);

		/// <summary>
		/// Обработчик изменения сигнала
		/// </summary>
		/// <param name="pSignal"></param>
		void signal_OnSignalChanged(BaseSignal pSignal)
		{

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

		public SingleFreqSignal Signal
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

		public delegate void SignalEditComplete(SingleFreqSignal pSignal);
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
