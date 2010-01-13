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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaorCore.Antenna.AntennaEdit
{
    public partial class AntennaEdit : UserControl
    {
        IAntenna antenna;

        public AntennaEdit()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text != string.Empty)
            {
                antenna.Name = txtName.Text;
            }
            if (txtDescr.Text != string.Empty)
            {
                antenna.Description = txtDescr.Text;
            }
            double _dir;
            if (double.TryParse(txtDir.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _dir))
            {
                antenna.Direction = _dir;
            }
            antenna.FreqMin = frqMin.Frequency;
            if (frqMax.Frequency > antenna.FreqMin)
                antenna.FreqMax = frqMax.Frequency;
            else
                MessageBox.Show("Максимум не может быть меньше минимума", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            switch (cmbState.SelectedText)
            {
                case "OK":
                    antenna.State = EAntennaState.OK;
                    break;
                case "BAD":
                    antenna.State = EAntennaState.BAD;
                    break;
                case "FAULT":
                    antenna.State = EAntennaState.FAULT;
                    break;
            }
            double _lat, _lon, _alt;
			if (double.TryParse(txtLat.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _lat) &&
				double.TryParse(txtLon.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _lon) &&
                cmbLat.SelectedIndex > -1 && cmbLon.SelectedIndex > -1 &&
				double.TryParse(txtAlt.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _alt))
            {
                antenna.Coordinates.Lon = _lon;
                antenna.Coordinates.Lat = _lat;
                antenna.Coordinates.Alt = _alt;
#if false
                switch (cmbLon.SelectedText)
                {
                    case "W":
                        antenna.Coordinates.EW = KaorCore.Base.EGPSEastWest.West;
                        break;
                    case "E":
                        antenna.Coordinates.EW = KaorCore.Base.EGPSEastWest.East;
                        break;
                }
                switch (cmbLat.SelectedText)
                {
                    case "N":
                        antenna.Coordinates.SN = KaorCore.Base.EGPSNorthSouth.North;
                        break;
                    case "S":
                        antenna.Coordinates.SN = KaorCore.Base.EGPSNorthSouth.South;
                        break;
                }
#endif
                //antenna.Coordinates.ReadOnly = chkReadOnly.Checked;
            }
 
        }

        IAntenna Antenna
        {
            get
            {
                return antenna;
            }
            set
            {
                antenna = value;
                if (antenna == null)
                {
                    foreach (Control _cont in this.Controls)
                    {
                        _cont.Enabled = false;
                    }
                }
                else
                {
                    foreach (Control _cont in this.Controls)
                    {
                        _cont.Enabled = true;
                    }
                }
            }
        }
    }
}
