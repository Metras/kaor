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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using KaorCore.RadioControlSystem;
using KaorCore.I18N;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace KaorCore.Report
{
	class BaseSignalReport
	{
		private class _ReportClass : PdfReport
		{
		}

		private PdfTable Table;
		private BaseRadioControlSystem rcs;
		private XPdfFontOptions FontOpts;
		private XFont HeaderFont;
		private Int32 Red, Yellow, Green, Gray;

		private _ReportClass _Report;

		public BaseSignalReport(BaseRadioControlSystem rcs)
		{
			FontOpts = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Automatic);
			HeaderFont = new XFont("Helvetica", 12, XFontStyle.Regular, FontOpts);
			this.rcs = rcs;
		}

		private void CreateHeader()
		{
			List<String> sList = new List<String>();
			sList.Add(Locale.report_signals_table);
			DateTime dt = DateTime.Now;
			sList.Add(String.Format(Locale.report_signals_table2, dt.ToLongDateString(), dt.ToLongTimeString()));
			_Report.PrintStrings(sList, PdfReport.XStringAlign.Center);
		}

		private void CreateFooter()
		{
			List<String> sList = new List<String>();
			PdfTable FTable = new PdfTable(80, 120, 70);
			FTable.Font = HeaderFont;
			FTable.DefTextAlign = PdfTable.CellTextAlign.HLeft | PdfTable.CellTextAlign.VCenter;
			FTable.DefBorders = PdfTable.CellBorders.None;
			FTable.Span = XUnit.FromMillimeter(0);

			FTable.AddRow(XColors.Transparent);
			FTable.FillRowText(Locale.report_total, (Red + Green + Yellow + Gray).ToString(CultureInfo.InvariantCulture), "");
			FTable.AddRow(XColors.Transparent);
			FTable.FillRowText(Locale.report_among_them, Locale.report_danger, Red.ToString(CultureInfo.InvariantCulture));
			FTable.AddRow(XColors.Transparent);
			FTable.FillRowText("", Locale.report_suspic, Yellow.ToString(CultureInfo.InvariantCulture));
			FTable.AddRow(XColors.Transparent);
			FTable.FillRowText("", Locale.report_unknown, Gray.ToString(CultureInfo.InvariantCulture));
			FTable.AddRow(XColors.Transparent);
			FTable.FillRowText("", Locale.report_safe, Green.ToString(CultureInfo.InvariantCulture));
			_Report.DrawTable(FTable);

			sList.Add(Locale.report_person + @" ____________________________ /__________ /");
			_Report.PrintStrings(sList, PdfReport.XStringAlign.Left);
		}

		private XUnit MakeTable()
		{
			Int32 sgCount = 0;
			Table = new PdfTable(20, 70, 70, 110, 220, 35);
			Table.Font = new XFont("Helvetica", 8, XFontStyle.Regular, FontOpts);
			Table.Pen = new XPen(XColors.Black, 0.1);

			Table.AddRow(XColors.LightGray);
			Table.FillRowText(
				"N", 
				Locale.report_frequency, 
				Locale.report_band, 
				Locale.report_name, 
				Locale.report_description, 
				Locale.report_hit_count);
			foreach (Signal.BaseSignal Signal in rcs.Signals)
			{
				sgCount++;
				switch (Signal.SignalType)
				{
					case KaorCore.Signal.ESignalType.Red:
						Table.AddRow(XColors.Tomato);
						Red++;
						break;
					case KaorCore.Signal.ESignalType.Yellow:
						Table.AddRow(XColors.Cornsilk);
						Yellow++;
						break;
					case KaorCore.Signal.ESignalType.Green:
						Table.AddRow(XColors.LightGreen);
						Green++;
						break;
					case KaorCore.Signal.ESignalType.Unknown:
						Table.AddRow(XColors.Transparent);
						Gray++;
						break;
					default:
						throw new Exception("Invalid signal type");
				}

				Table.FillRowText(
					String.Format("{0}", sgCount),
					String.Format("{0}", Utils.FreqUtils.FreqToString(Signal.Frequency)),
					String.Format("{0}", Utils.FreqUtils.FreqToString(Signal.Band)),
					String.Format("{0}", Signal.Name),
					String.Format("{0}", Signal.Description),
					String.Format("{0}", Signal.HitsCount));
			}
			Table.SetColumnAlign(0, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HRight);
			Table.SetColumnAlign(1, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(2, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(3, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);
			Table.SetColumnAlign(4, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);
			Table.SetColumnAlign(5, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);
			Table.SetRowAlign(0, PdfTable.CellTextAlign.Center);
			return 0;
		}

		public bool Save(String Filename)
		{
			if (rcs.Signals.Count <= 0)
			{
				System.Windows.Forms.MessageBox.Show(
					Locale.report_no_signals_selected, Locale.error, 
					System.Windows.Forms.MessageBoxButtons.OK, 
					System.Windows.Forms.MessageBoxIcon.Warning);
				return false;
			}
			_Report = new _ReportClass();
			_Report.CurrentFont = HeaderFont;
			MakeTable();

			CreateHeader();
			_Report.DrawTable(Table);
			CreateFooter();
			_Report.Numerate();

			if (System.IO.File.Exists(Filename))
				System.IO.File.Delete(Filename);
			_Report.Save(Filename);
			return true;
		}
	}
}
