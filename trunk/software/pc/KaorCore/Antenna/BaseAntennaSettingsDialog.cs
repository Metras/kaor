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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.I18N;

namespace KaorCore.Antenna
{
	public partial class BaseAntennaSettingsDialog : Form
	{
		BaseAntenna antenna;

		public BaseAntennaSettingsDialog(BaseAntenna pAntenna)
		{
			antenna = pAntenna;

			InitializeComponent();

			txtName.Text = antenna.Name;
			txtDescription.Text = antenna.Description;

			freqStart.Min = freqStop.Min = antenna.FreqMin;
			freqStart.Max = freqStop.Max = antenna.FreqMax;
			freqStart.Frequency = antenna.FreqMin;
			freqStop.Frequency = antenna.FreqMax;

			string _s = antenna.Coordinates.Lat.ToString();

			txtLat.Text = antenna.Coordinates.Lat.ToString();
			txtLon.Text = antenna.Coordinates.Lon.ToString();
			txtAlt.Text = antenna.Coordinates.Alt.ToString();

			txtDir.Text = antenna.Direction.ToString();
			txtDNWidth.Text = antenna.DNWidth.ToString();

			switch (antenna.AntennaType)
			{
				case EAntennaType.Directional:
					cmbType.SelectedIndex = 0;
					break;

				case EAntennaType.Omni:
					cmbType.SelectedIndex = 1;
					break;

				default:
					cmbType.SelectedIndex = 1;
					break;
			}

			switch (antenna.State)
			{
				case EAntennaState.OK:
					cmbState.SelectedIndex = 0;
					break;

				case EAntennaState.BAD:
					cmbState.SelectedIndex = 1;
					break;

				case EAntennaState.FAULT:
					cmbState.SelectedIndex = 2;
					break;

				default:
					cmbState.SelectedIndex = 1;
					break;
			}
		}

		public BaseAntenna Antenna
		{
			get
			{
				return antenna;
			}

			set
			{
				antenna = value;
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			double _t;

			if (!double.TryParse(txtLat.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _t))
			{
				MessageBox.Show(Locale.invalid_latitude, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtLat.Focus();

				DialogResult = DialogResult.None;
				
				return;
			}

			antenna.Coordinates.Lat = _t;

			if (!double.TryParse(txtLon.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _t))
			{
				MessageBox.Show(Locale.invalid_longitude, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtLon.Focus();

				DialogResult = DialogResult.None;

				return;
			}

			antenna.Coordinates.Lon = _t;

			if (!double.TryParse(txtAlt.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _t))
			{
				MessageBox.Show(Locale.invalid_altitude, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtAlt.Focus();

				DialogResult = DialogResult.None;

				return;
			}

			antenna.Coordinates.Alt = _t;
			
			if (!double.TryParse(txtDir.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _t))
			{
				MessageBox.Show(Locale.invalid_direction, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtDir.Focus();

				DialogResult = DialogResult.None;

				return;
			}

			antenna.Direction = _t;

			if (!double.TryParse(txtDNWidth.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _t))
			{
				MessageBox.Show(Locale.invalid_dnwidth, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtDNWidth.Focus();

				DialogResult = DialogResult.None;

				return;
			}

			antenna.DNWidth = _t;

			antenna.Name = txtName.Text;
			antenna.Description = txtDescription.Text;
			antenna.FreqMin = freqStart.Frequency;
			antenna.FreqMax = freqStop.Frequency;

			//antenna.Coordinates.Lat = double.Parse(txtLat.Text, CultureInfo.InvariantCulture);
			//antenna.Coordinates.Lon = double.Parse(txtLon.Text, CultureInfo.InvariantCulture);
			//antenna.Coordinates.Alt = double.Parse(txtAlt.Text, CultureInfo.InvariantCulture);

			//antenna.Direction = double.Parse(txtDir.Text, CultureInfo.InvariantCulture);
			//antenna.DNWidth = double.Parse(txtDNWidth.Text, CultureInfo.InvariantCulture);

			switch (cmbType.SelectedIndex)
			{
				case 0:
					antenna.AntennaType = EAntennaType.Directional;
					break;

				case 1:
					antenna.AntennaType = EAntennaType.Omni;
					break;
			}

			switch (cmbState.SelectedIndex)
			{
				case 0:
					antenna.State = EAntennaState.OK;
					break;

				case 1:
					antenna.State = EAntennaState.BAD;
					break;

				case 2:
					antenna.State = EAntennaState.FAULT;
					break;
			}
		}

		private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbType.SelectedIndex == 0)
			{
				txtDir.Enabled = true;
				txtDNWidth.Enabled = true;
			}
			else
			{
				txtDir.Enabled = false;
				txtDNWidth.Enabled = false;
			}
		}
	}
}
