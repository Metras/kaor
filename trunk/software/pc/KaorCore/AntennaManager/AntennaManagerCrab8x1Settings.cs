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
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.Antenna;
using KaorCore.I18N;
namespace KaorCore.AntennaManager
{
	public partial class AntennaManagerCrab8x1Settings : Form
	{
		AntennaManagerCrab8x1 manager;

		public AntennaManagerCrab8x1Settings(AntennaManagerCrab8x1 pManager)
		{
			manager = pManager;
			InitializeComponent();

			InitPortAntennaList(cmbPort1);
			InitPortAntennaList(cmbPort2);
			InitPortAntennaList(cmbPort3);
			InitPortAntennaList(cmbPort4);
			InitPortAntennaList(cmbPort5);
			InitPortAntennaList(cmbPort6);
			InitPortAntennaList(cmbPort7);
			InitPortAntennaList(cmbPort8);

			cmbSerialPort.Items.Clear();
			cmbSerialPort.Items.AddRange(SerialPort.GetPortNames());

			if (cmbSerialPort.Items.Count > 0)
			{
				if (cmbSerialPort.Items.Contains(manager.PortName))
					cmbSerialPort.SelectedItem = manager.PortName;
				else
					cmbSerialPort.SelectedIndex = 0;
			}

			////if (cmbSerialPort.Items.Count > 0)
			////    cmbSerialPort.SelectedIndex = 0;

			if (cmbPortSpeed.Items.Count > 0)
				cmbPortSpeed.SelectedIndex = 0;

			/// Заполнение окна параметрами менеджера
			/// 
			if (manager.AntennasArray[0] != null &&
				cmbPort1.Items.Contains(manager.AntennasArray[0]))
			{
				cmbPort1.SelectedItem = manager.AntennasArray[0];
				chkIn1.Checked = true;
			}
			else
				chkIn1.Checked = false;

			if (manager.AntennasArray[1] != null &&
				cmbPort2.Items.Contains(manager.AntennasArray[1]))
			{
				cmbPort2.SelectedItem = manager.AntennasArray[1];
				chkIn2.Checked = true;
			}
			else
				chkIn2.Checked = false;

			if (manager.AntennasArray[2] != null &&
				cmbPort3.Items.Contains(manager.AntennasArray[2]))
			{
				cmbPort3.SelectedItem = manager.AntennasArray[2];
				chkIn3.Checked = true;
			}
			else
				chkIn3.Checked = false;

			if (manager.AntennasArray[3] != null &&
				cmbPort4.Items.Contains(manager.AntennasArray[3]))
			{
				cmbPort4.SelectedItem = manager.AntennasArray[3];
				chkIn4.Checked = true;
			}
			else
				chkIn4.Checked = false;

			if (manager.AntennasArray[4] != null &&
				cmbPort5.Items.Contains(manager.AntennasArray[4]))
			{
				cmbPort5.SelectedItem = manager.AntennasArray[4];
				chkIn5.Checked = true;
			}
			else
				chkIn5.Checked = false;

			if (manager.AntennasArray[5] != null &&
				cmbPort6.Items.Contains(manager.AntennasArray[5]))
			{
				cmbPort6.SelectedItem = manager.AntennasArray[5];
				chkIn6.Checked = true;
			}
			else
				chkIn6.Checked = false;

			if (manager.AntennasArray[6] != null &&
				cmbPort7.Items.Contains(manager.AntennasArray[6]))
			{
				cmbPort7.SelectedItem = manager.AntennasArray[6];
				chkIn7.Checked = true;
			}
			else
				chkIn7.Checked = false;

			if (manager.AntennasArray[7] != null &&
				cmbPort8.Items.Contains(manager.AntennasArray[7]))
			{
				cmbPort8.SelectedItem = manager.AntennasArray[7];
				chkIn8.Checked = true;
			}
			else
				chkIn8.Checked = false;

		}

		private void InitPortAntennaList(ComboBox _cmbPort)
		{
			_cmbPort.Items.Clear();

			_cmbPort.Items.AddRange(BaseAntenna.AntennaList.ToArray());

			if (_cmbPort.Items.Count > 0)
				_cmbPort.SelectedIndex = 0;
		}

		private void chkIn_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox _chk = sender as CheckBox;

			if (_chk == null)
				return;

			ComboBox _cmbPort = null;

			if (_chk == chkIn1)
				_cmbPort = cmbPort1;
			else if (_chk == chkIn2)
				_cmbPort = cmbPort2;
			else if (_chk == chkIn2)
				_cmbPort = cmbPort2;
			else if (_chk == chkIn3)
				_cmbPort = cmbPort3;
			else if (_chk == chkIn4)
				_cmbPort = cmbPort4;
			else if (_chk == chkIn5)
				_cmbPort = cmbPort5;
			else if (_chk == chkIn6)
				_cmbPort = cmbPort6;
			else if (_chk == chkIn7)
				_cmbPort = cmbPort7;
			else if (_chk == chkIn8)
				_cmbPort = cmbPort8;

			if (_cmbPort == null)
				return;

			if (_chk.Checked)
				_cmbPort.Enabled = true;
			else
				_cmbPort.Enabled = false;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (chkIn1.Checked)
				manager.AntennasArray[0] = cmbPort1.SelectedItem as IAntenna;
			else
				manager.AntennasArray[0] = null;

			if (chkIn2.Checked)
				manager.AntennasArray[1] = cmbPort2.SelectedItem as IAntenna;
			else
				manager.AntennasArray[1] = null;

			if (chkIn3.Checked)
				manager.AntennasArray[2] = cmbPort3.SelectedItem as IAntenna;
			else
				manager.AntennasArray[2] = null;

			if (chkIn4.Checked)
				manager.AntennasArray[3] = cmbPort4.SelectedItem as IAntenna;
			else
				manager.AntennasArray[3] = null;

			if (chkIn5.Checked)
				manager.AntennasArray[4] = cmbPort5.SelectedItem as IAntenna;
			else
				manager.AntennasArray[4] = null;

			if (chkIn6.Checked)
				manager.AntennasArray[5] = cmbPort6.SelectedItem as IAntenna;
			else
				manager.AntennasArray[5] = null;

			if (chkIn7.Checked)
				manager.AntennasArray[6] = cmbPort7.SelectedItem as IAntenna;
			else
				manager.AntennasArray[6] = null;

			if (chkIn8.Checked)
				manager.AntennasArray[7] = cmbPort8.SelectedItem as IAntenna;
			else
				manager.AntennasArray[7] = null;

			if (manager.Port.IsOpen)
				manager.SwitchOff();

			manager.Port.PortName = cmbSerialPort.SelectedItem as string;
			manager.Port.BaudRate = int.Parse(cmbPortSpeed.SelectedItem as string);
		}

		/// <summary>
		/// Проверка соединения с коммутатором
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTest_Click(object sender, EventArgs e)
		{
			try
			{
				manager.Port.PortName = cmbSerialPort.SelectedItem as string;
				manager.Port.BaudRate = int.Parse(cmbPortSpeed.SelectedItem as string);

				manager.SwitchOn();
				manager.SwitchOff();

				MessageBox.Show(Locale.comm_conn_ok,
					Locale.information,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			catch
			{
				MessageBox.Show(Locale.comm_conn_err, 
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
