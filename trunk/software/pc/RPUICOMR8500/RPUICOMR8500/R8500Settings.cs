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
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.AntennaManager;
using RPUICOMR8500.I18N;
namespace RPUICOMR8500
{
	public partial class R8500Settings : Form
	{
		RPUR8500 rpu;
		IAntennaManager managerRF;

		public R8500Settings(RPUR8500 pRPU)
		{
			rpu = pRPU;

			InitializeComponent();

			cmbPort.Items.Clear();
			cmbPort.Items.AddRange(SerialPort.GetPortNames());

			if (cmbPort.Items.Count > 0)
			{

				if (cmbPort.Items.Contains(rpu.PortName))
				{
					cmbPort.SelectedItem = rpu.PortName;
				}
				else
				{
					cmbPort.SelectedIndex = 0;
				}
			}

			if (cmbPortSpeed.Items.Count > 0)
			{
				if(cmbPortSpeed.Items.Contains(rpu.PortBaud.ToString(CultureInfo.InvariantCulture)))
				{
					cmbPortSpeed.SelectedItem = rpu.PortBaud.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					cmbPortSpeed.SelectedIndex = 0;
				}
			}

			cmbCom.SelectedIndex = 0;
			managerRF = rpu.ManagerRF;

			if (managerRF != null)
			{
				if (managerRF.GetType() == typeof(AntennaManagerDirect))
				{
					cmbCom.SelectedIndex = 0;
				}
				else if (managerRF.GetType() == typeof(AntennaManagerCrab8x1))
				{
					cmbCom.SelectedIndex = 1;
				}
			}
			else
				managerRF = new AntennaManagerDirect();
		}

		private void btnComParams_Click(object sender, EventArgs e)
		{
			IAntennaManager _manager;

			switch (cmbCom.SelectedIndex)
			{
				case 0:
					_manager = new AntennaManagerDirect();
					break;

				case 1:
					_manager = new AntennaManagerCrab8x1();
					break;

				default:
					_manager = new AntennaManagerDirect();
					break;
			}

			if (managerRF.GetType() != _manager.GetType())
			{
				managerRF = _manager;
			}

			if (managerRF.SettingsForm.ShowDialog() == DialogResult.OK)
			{
			}
		}

		private void btnCheckConnect_Click(object sender, EventArgs e)
		{
			CheckConnectionWaitDialog _dlg = new CheckConnectionWaitDialog();
			_dlg.Show();

			_dlg.Refresh();
			try
			{
				rpu.PortName = cmbPort.SelectedItem as string;
				rpu.PortBaud = int.Parse(cmbPortSpeed.SelectedItem as string);

				rpu.SwitchOn();
				rpu.SwitchOff();

				_dlg.Hide();

				MessageBox.Show(Locale.connection_ok,
					Locale.confirmation,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				_dlg.Hide();

				MessageBox.Show(String.Format("{0}\n{1}", Locale.connection_error, ex.Message),
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			rpu.ManagerRF = managerRF;

			try
			{

				rpu.SwitchOff();
			}

			catch 
			{ }

			rpu.PortName = cmbPort.SelectedItem as string;
			rpu.PortBaud = int.Parse(cmbPortSpeed.SelectedItem as string);
		}
	}
}
