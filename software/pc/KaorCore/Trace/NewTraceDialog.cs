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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.Antenna;
using KaorCore.I18N;

namespace KaorCore.Trace
{
	public partial class NewTraceDialog : Form
	{
		bool newTrace;

		public NewTraceDialog(bool pNewTrace)
		{
			newTrace = pNewTrace;

			InitializeComponent();

			
		}

		#region ================ Проперти ================

		public string TraceName
		{
			get { return txtTraceName.Text; }
			set { txtTraceName.Text = value; }
		}

		public string Description
		{
			get { return txtTraceDescr.Text; }
			set { txtTraceDescr.Text = value; }
		}

		public int InitialValue
		{
			get { return (int)numInitial.Value; }
			set { numInitial.Value = value; }
		}

		public bool IsCyclic
		{
			get { return true; }
			set { }
		}

		public bool IsNeedSave
		{
			get { return false; }
			set { }
		}

		public string SavePath
		{
			get { return ""; }
			set { }
		}
		/// <summary>
		/// Начальная частота сканирования
		/// </summary>
		public Int64 FStart
		{
			get
			{
				return txtFstart.Frequency;
			}

			set
			{
				txtFstart.Frequency = value;
			}
		}

		/// <summary>
		/// Конечная частота сканирования
		/// </summary>
		public Int64 FStop
		{
			get
			{
				return txtFstop.Frequency;
			}
			set
			{
				txtFstop.Frequency = value;
			}
		}

		public long MeasureStep
		{
			get
			{
				return txtMeasureStep.Frequency;
			}

			set
			{
				txtMeasureStep.Frequency = value;
			}
		}

		#endregion

		private void cmbScanStep_SelectedIndexChanged(object sender, EventArgs e)
		{

		}


		public bool NeedSaveTraces
		{
			get { return false; }
		}

		private void txtFstart_TextChanged(object sender, EventArgs e)
		{
			numInitial.Enabled = true;
		}

		private void txtFstop_TextChanged(object sender, EventArgs e)
		{
			numInitial.Enabled = true;
		}

		private void txtMeasureStep_TextChanged(object sender, EventArgs e)
		{
			numInitial.Enabled = true;
		}

		private void NewTraceDialog_Shown(object sender, EventArgs e)
		{
			if (newTrace)
				numInitial.Enabled = true;
			else
				numInitial.Enabled = false;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (txtFstart.Frequency >= txtFstop.Frequency)
			{
				MessageBox.Show(Locale.err_fstart_fstop,
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				DialogResult = DialogResult.None;
			}
		}
	}
}
