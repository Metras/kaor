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
using System.Xml;


using KaorCore.I18N;
namespace KaorCore.Report
{
	public partial class ReportSettings : Form
	{
		public ReportSettings()
		{
			InitializeComponent();

			if(File.Exists(Application.StartupPath + "\\reportheader.xml"))
			{
				XmlDocument _doc = new XmlDocument();
				_doc.Load(Application.StartupPath + "\\reportheader.xml");

				XmlNode _node = _doc.SelectSingleNode("Body/Org");
				if (_node != null)
					txtOrg.Text = _node.InnerText;

				_node = _doc.SelectSingleNode("Body/Station");
				if (_node != null)
					txtStation.Text = _node.InnerText;

				_node = _doc.SelectSingleNode("Body/Notes");
				if (_node != null)
					txtNotes.Text = _node.InnerText;

			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			try
			{
				if (File.Exists(Application.StartupPath + "\\reportheader.xml.bak"))
				{
					File.Delete(Application.StartupPath + "\\reportheader.xml.bak");
				}

				if (File.Exists(Application.StartupPath + "\\reportheader.xml"))
					File.Move(Application.StartupPath + "\\reportheader.xml", 
						Application.StartupPath + "\\reportheader.xml.bak");

				XmlTextWriter _writer = new XmlTextWriter(Application.StartupPath + "\\reportheader.xml", 
					Encoding.UTF8);

				_writer.Formatting = Formatting.Indented;

				_writer.WriteStartElement("Body");

				_writer.WriteElementString("Org", txtOrg.Text);
				_writer.WriteElementString("Station", txtStation.Text);
				_writer.WriteElementString("Notes", txtNotes.Text);

				_writer.WriteElementString("FileName", "Logo.png");
				_writer.WriteElementString("ImgSize", "60");
				_writer.WriteElementString("USize", "30");

				_writer.WriteEndElement();
				_writer.Close();
			}

			catch (Exception ex)
			{
				MessageBox.Show(String.Format("{0}\n{1}", Locale.err_saving_report_settings, ex.Message),
					Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			DialogResult = DialogResult.OK;
		}
	}
}
