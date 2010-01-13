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
using System.Xml;

namespace RPUICOMR8500.CalibrationForm
{
    public partial class CalibrationForm : Form
    {
        R8500Control _contr;
        public CalibrationForm(R8500Control pContr)
        {
            InitializeComponent(); 
            _contr = pContr;
            writer = XmlWriter.Create("D:\\tables.xml");
            writer.WriteStartDocument();
            writer.WriteStartElement("Tables");
            numericUpDown1.Value = -115;
            //this.Controls.Add(_contr.frequencyRadio1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double d = _contr.RPU.PowerMeter.MeasurePower();

            writer.WriteStartElement("TableItem");
            writer.WriteAttributeString("rig", d.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("val", numericUpDown1.Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            numericUpDown1.Value += 5;
        }

        XmlWriter writer;
        private void button7_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = -115;
            //writer.WriteStartElement("Tables");
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Fmin", (_contr.frequencyRadio1.Frequency - 50000000).ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Fmax", (_contr.frequencyRadio1.Frequency + 50000000).ToString(CultureInfo.InvariantCulture));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            writer.WriteEndElement();
            writer.Flush();
        }

        private void CalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_contr.Controls.Add(_contr.frequencyRadio1);
        }
    }
}
