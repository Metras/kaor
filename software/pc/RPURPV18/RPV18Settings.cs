// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//			Maxim Anisenkov <anisenkov@niistt.ru>
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
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RPURPV18.I18N;
using KaorCore.Antenna;
using KaorCore.AntennaManager;
using RPURPV18.SignalConverterManager;

namespace RPURPV18
{
	public partial class RPV18Settings : Form
	{
		CRPURPV18 rpu;
		IAntennaManager managerRF1, managerRF2;

		public RPV18Settings(CRPURPV18 pRPU)
		{
			rpu = pRPU;

			InitializeComponent();

			cmbPortName.Items.Clear();
			cmbPortName.Items.AddRange(SerialPort.GetPortNames());

			if (cmbPortName.Items.Count > 0)
			{
				if (cmbPortName.Items.Contains(rpu.PortName))
					cmbPortName.SelectedItem = rpu.PortName;
				else
					cmbPortName.SelectedIndex = 0;
			}

			cmbInjPortName.Items.AddRange(SerialPort.GetPortNames());

			if (cmbInjPortName.Items.Count > 0)
			{
				if (cmbInjPortName.Items.Contains(rpu.InjectorSerial))
					cmbInjPortName.SelectedItem = rpu.InjectorSerial;
				else
					cmbInjPortName.SelectedIndex = 0;
			}

			string _t = rpu.PortBaud.ToString(CultureInfo.InvariantCulture);
			if (cmbPortSpeed.Items.Contains(_t))
			{
				cmbPortSpeed.SelectedItem = _t;
			}
			else
			{
				cmbPortSpeed.SelectedItem = "57600";
			}

			cmbCom1.SelectedIndex = -1;
			chkCom1Enable.Checked = !rpu.RF1Disabled;

			if (!rpu.RF1Disabled)
			{
				managerRF1 = rpu.ManagerRF1;

				if (managerRF1 != null)
				{

					if (managerRF1.GetType() == typeof(AntennaManagerDirect))
					{
						chkCom1Enable.Checked = true;
						cmbCom1.SelectedIndex = 0;
					}
					else if (managerRF1.GetType() == typeof(AntennaManagerCrab8x1))
					{
						chkCom1Enable.Checked = true;
						cmbCom1.SelectedIndex = 1;
					}
				}
				//else
				//{
				//    managerRF1 = new AntennaManagerDirect();
				//    cmbCom1.SelectedIndex = 0;
				//}
			}

			chkCom2Enable.Checked = !rpu.RF2Disabled;
			cmbCom2.SelectedIndex = -1;

			if (!rpu.RF2Disabled)
			{
				managerRF2 = rpu.ManagerRF2;

				if (managerRF2 != null)
				{
					if (managerRF2.GetType() == typeof(AntennaManagerDirect))
					{
						chkCom2Enable.Checked = true;
						cmbCom2.SelectedIndex = 0;
					}
					else if (managerRF2.GetType() == typeof(AntennaManagerCrab8x1))
					{
						chkCom2Enable.Checked = true;
						cmbCom2.SelectedIndex = 1;
					}
				}
//				else
//					managerRF2 = new AntennaManagerDirect();
			}
		}

		private void chkCom1Enable_CheckedChanged(object sender, EventArgs e)
		{
			if (chkCom1Enable.Checked)
			{
				cmbCom1.Enabled = true;

				if (managerRF1 == null)
					btnCom1Params.Enabled = false;
				else
					btnCom1Params.Enabled = true;
			}
			else
			{
				cmbCom1.Enabled = false;
				btnCom1Params.Enabled = false;
			}
		}

		private void chkCom2Enable_CheckedChanged(object sender, EventArgs e)
		{
			if (chkCom2Enable.Checked)
			{
				cmbCom2.Enabled = true;

				if (managerRF2 == null)
					btnCom2Params.Enabled = false;
				else
					btnCom2Params.Enabled = true;
			}
			else
			{
				cmbCom2.Enabled = false;
				btnCom2Params.Enabled = false;
			}
		}

		delegate bool TestConnectionDelegate(string pPortName, int pSpeed);

		private bool TestConnection(string pPortName, int pSpeed)
		{
			bool _res = false;

			try
			{
				rpu.PortName = pPortName;
				//rpu.Port.BaudRate = int.Parse(cmbPortSpeed.SelectedItem as string);
				rpu.PortBaud = pSpeed;

				rpu.SwitchOn();
				rpu.SwitchOff();

				_res = true;
			}

			catch
			{
			}

			return _res;
		}

		private void btnTestInjectorConnection_Click(object sender, EventArgs e)
		{
			CheckConnectionWaitDialog _dlg = new CheckConnectionWaitDialog();
			_dlg.Show(this);
			_dlg.Refresh();
			bool _res = rpu.UsedInjector.TestConnection();
			_dlg.Hide();
		}

		private void btnTestConnection_Click(object sender, EventArgs e)
		{
			CheckConnectionWaitDialog _dlg = new CheckConnectionWaitDialog();
			_dlg.Show(this);
			_dlg.Refresh();

			//TestConnectionDelegate _mi = new TestConnectionDelegate(TestConnection);
			//IAsyncResult _iares = _mi.BeginInvoke(cmbPortName.SelectedItem as string, int.Parse(cmbPortSpeed.SelectedItem as string), null, null);
			//_mi.EndInvoke(_iares);

			bool _res = TestConnection(cmbPortName.SelectedItem as string, int.Parse(cmbPortSpeed.SelectedItem as string));
			_dlg.Hide();

			if (_res == true)
			{
				MessageBox.Show(Locale.connection_ok,
					Locale.confirmation,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show(String.Format("{0}\n{1}", Locale.connection_error, ""),
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCom1Params_Click(object sender, EventArgs e)
		{
			if (managerRF1.SettingsForm.ShowDialog() == DialogResult.OK)
			{
			}
		}

		private void btnCom2Params_Click(object sender, EventArgs e)
		{
			if (managerRF2.SettingsForm.ShowDialog() == DialogResult.OK)
			{
			}
		}

		private void cmbCom1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbCom1.SelectedIndex == -1)
			{
				btnCom1Params.Enabled = false;
			}
			else
			{
				IAntennaManager _manager;

				switch (cmbCom1.SelectedIndex)
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

				if (managerRF1 == null || managerRF1.GetType() != _manager.GetType())
				{
					managerRF1 = _manager;
				}

				btnCom1Params.Enabled = true;
			}
		}

		private void cmbCom2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbCom2.SelectedIndex == -1)
			{
				btnCom2Params.Enabled = false;
			}
			else
			{
				IAntennaManager _manager;

				switch (cmbCom2.SelectedIndex)
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

				if (managerRF2 == null || managerRF2.GetType() != _manager.GetType())
				{
					managerRF2 = _manager;
				}
				btnCom2Params.Enabled = true;
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			rpu.RF1Disabled = !chkCom1Enable.Checked;

			if (!rpu.RF1Disabled)
				rpu.ManagerRF1 = managerRF1;
			else
				rpu.ManagerRF1 = null;

			rpu.RF2Disabled = !chkCom2Enable.Checked;

			if (!rpu.RF2Disabled)
				rpu.ManagerRF2 = managerRF2;
			else
				rpu.ManagerRF2 = null;

			rpu.SwitchOff();

			rpu.PortName = cmbPortName.SelectedItem as string;
			rpu.PortBaud = int.Parse(cmbPortSpeed.SelectedItem as string);

			rpu.InjectorSerial = cmbInjPortName.SelectedItem as string;
		}
	}
}
