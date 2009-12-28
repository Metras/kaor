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
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using kaor.I18N;

namespace kaor
{
    public partial class ApplicationCrashForm : Form
    {
        public ApplicationCrashForm()
        {
            InitializeComponent();

			lblSystemState.Text = String.Format(Locale.system_state, Application.StartupPath + "\\autosave\\autosave.xml");
        }

        internal void FillData(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            FillExceptionInfo(ex, sb);
            textBox1.Text = sb.ToString();
            textBox1.SelectionLength = 0;
            AppDomain.CurrentDomain.SetData("ApplicationCrash", true);
        }
        internal void FillData(object ex)
        {
            StringBuilder sb = new StringBuilder();
            FillExceptionInfo(ex, sb);
            textBox1.Text = sb.ToString();
            textBox1.SelectionLength = 0;
            AppDomain.CurrentDomain.SetData("ApplicationCrash", true);
        }
        private static void FillExceptionInfo(object ex, StringBuilder sb)
        {
            sb.AppendFormat(ex.ToString());

            //if (ex.InnerException != null) FillExceptionInfo(ex.InnerException, sb);
        }
        private void FillExceptionInfo(Exception ex, StringBuilder sb)
        {
			sb.AppendFormat(Locale.ex_type + "\r\n", ex.GetType());
			sb.AppendFormat(Locale.ex_source + "\r\n", ex.Source);
			sb.AppendFormat(Locale.ex_method + "\r\n", ex.TargetSite);
            sb.AppendFormat(Locale.ex_information + "\r\n", ex.Message);
            sb.AppendFormat(Locale.ex_stack + "\r\n\r\n", ex.StackTrace);

            if (ex.InnerException != null) 
				FillExceptionInfo(ex.InnerException, sb);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("mailto:techsupport@niistt.ru?subject=Program_Bugbody=");
            string ts = textBox1.Text.Replace("\r\n", "‹br›");
            string amp = Uri.HexEscape(' ');
            ts = ts.Replace("", amp);
            sb.Append(ts);
            Process.Start(sb.ToString());
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.niistt.ru");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}