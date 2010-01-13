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
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;

using KaorCore.I18N;
namespace KaorCore.RadioControlSystem
{
	public partial class FrequencyInputDialog : Form
	{
		Int64 inputFreq;
		Int64 freqMin;
		Int64 freqMax;

		public FrequencyInputDialog()
		{
			InitializeComponent();

			inputFreq = 0;
			freqMin = 0;
			freqMax = 3000000000;
		}

		public long FreqMin
		{
			get { return freqMin; }
			set { freqMin = value; }
		}

		public long FreqMax
		{
			get { return freqMax; }
			set { freqMax = value; }
		}

		public Int64 InputFrequency
		{
			get { return inputFreq; }
		}

		private void btnAccept_Click(object sender, EventArgs e)
		{
			Regex _regex = new Regex(@"^([\.\d]+)([kKmMgG])*$");
			string _inputFreq = cmbInputFreq.Text;
			Int64 _mult = 1;
			double _freq;

			Match _match = _regex.Match(_inputFreq);
			
			if(_match.Success == false)
			{
				MessageBox.Show(Locale.wrong_frequency_format, 
					Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			switch (_match.Groups[2].Value.ToLower())
			{
				case "k":
					_mult = 1000;
					break;
				case "m":
					_mult = 1000000;
					break;
				case "g":
					_mult = 1000000000;
					break;
				default:
					_mult = 1;
					break;
			}

			if (!double.TryParse(_match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _freq))
			{
				MessageBox.Show(Locale.wrong_frequency_format, 
					Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			inputFreq = (Int64) (_mult * _freq);

			if (inputFreq < freqMin)
			{
				MessageBox.Show(String.Format(Locale.limit_min_freq, freqMin), Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				return;
			}

			if (inputFreq > freqMax)
			{
				MessageBox.Show(String.Format(Locale.limit_max_freq, freqMax), Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);

				return;
			}

			if(!cmbInputFreq.Items.Contains(_inputFreq))
				cmbInputFreq.Items.Add(_inputFreq);

			DialogResult = DialogResult.OK;

			Close();
		}
	}
}
