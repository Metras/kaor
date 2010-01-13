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
using System.Linq;
using System.Text;
using System.Xml;

using KaorCore.RadioControlSystem;
using KaorCore.Signal;
using KaorCore.I18N;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace KaorCore.Report
{
	class SingleFreqSignalReport
	{
		private class _ReportClass : PdfReport
		{
			public SingleFreqSignalReport Owner;

			protected override void OnNextPage()
			{
				base.OnNextPage();
				Owner._Report.DrawTable(Owner.HeaderTable);
			}
		}
		private _ReportClass _Report;
		
		private PdfTable Table;
		private PdfTable HeaderTable;
		private PdfTable FooterTable;

		private SingleFreqSignal signal;
		private XPdfFontOptions FontOpts;
		private XFont HeaderFont;

		public SingleFreqSignalReport(SingleFreqSignal signal)
		{
			FontOpts = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Automatic);
			HeaderFont = new XFont("Helvetica", 14, XFontStyle.Regular, FontOpts);
			this.signal = signal;
		}

		private void CreateHeader()
		{
			List<String> Str = new List<string>();
			XColor[] Colors = { XColors.Transparent, XColors.LightGreen, XColors.Cornsilk, XColors.Tomato };

			_Report.CurrentFont = HeaderFont;
			Str.Add(String.Format(
				Locale.report_signal,
				signal.Name, 
				Utils.FreqUtils.FreqToString(signal.Frequency)));
			_Report.PrintStrings(Str, PdfReport.XStringAlign.Center);

			HeaderTable = new PdfTable(515);
			HeaderTable.AddRow(Colors[(int)signal.SignalType]);
			HeaderTable.FillRowText(String.Format(
				Locale.report_signal_header,
				Utils.FreqUtils.FreqToString(signal.Frequency),
				Utils.FreqUtils.FreqToString(signal.Band),
				signal.Description,
				signal.HitsCount));

			HeaderTable.Font = new XFont("Helvetica", 12, XFontStyle.Regular, FontOpts);
			HeaderTable.Pen = new XPen(XColors.Black, 0.1);
			HeaderTable.SetRowAlign(0, PdfTable.CellTextAlign.HLeft | PdfTable.CellTextAlign.VCenter);
			HeaderTable.SetCellBorders(0, 0, PdfTable.CellBorders.None);
			HeaderTable.Span = 10;
			HeaderTable.RepeatHeader = false;
			_Report.DrawTable(HeaderTable);
		}

		Int32 sgCount = 0;
		private void CreateFooter()
		{
			FooterTable = new PdfTable(515);
			FooterTable.AddRow(XColors.Transparent);
			FooterTable.FillRowText(String.Format(
				Locale.report_total_hits +  " / " + 
				Locale.report_person + " ____________________________/__________/",
				sgCount));

			FooterTable.Font = new XFont("Helvetica", 12, XFontStyle.Regular, FontOpts);
			FooterTable.Pen = new XPen(XColors.Black, 0.1);
			FooterTable.SetRowAlign(0, PdfTable.CellTextAlign.HLeft | PdfTable.CellTextAlign.VCenter);
			FooterTable.SetCellBorders(0, 0, PdfTable.CellBorders.None);
			FooterTable.RepeatHeader = false;
			_Report.DrawTable(FooterTable);
		}

		private XUnit MakeTable()
		{
#if fase
			Table = new PdfTable(45, 70, 70, 40, 60, 90, 20, 120);
			Table.Font = new XFont("Helvetica", 8, XFontStyle.Regular, FontOpts);
			Table.Pen = new XPen(XColors.Black, 0.1);

			Table.AddRow(XColors.LightGray);
			Table.FillRowText(
				Locale.report_time, 
				Locale.report_frequency,
				Locale.report_band, 
				"P/Δ", 
				Locale.report_control, 
				Locale.report_AntRpu, 
				"Rec", 
				Locale.report_notes);
			

			foreach(RangeSignalHitPoint point in signal.SignalHitPoints)
			{
				Table.AddRow();
				Table.FillRowText(
					String.Format("{0} / {1}", point.Timestamp.ToShortDateString(), point.Timestamp.ToLongTimeString()),
					String.Format("{0}", Utils.FreqUtils.FreqToString(point.Frequency)),
					String.Format("{0} / {1} " + Locale.ms, Utils.FreqUtils.FreqToString(point.FilterBand), point.AverageTime),
					String.Format("{0} / {1}", point.Power.ToString("F1"), point.Delta.ToString("F1")),
					String.Format("{0}", point.TraceControlName),
					String.Format("{0} / {1}", point.AntennaName, point.RPUName),
					String.Format("{0}", point.HasRecord ? "V" : " "),
					String.Format(""));
				sgCount++;
			}

			//
			Table.SetColumnAlign(0, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HRight);
			Table.SetColumnAlign(1, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(2, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(3, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(4, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);
			Table.SetColumnAlign(5, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);
			Table.SetRowAlign(0, PdfTable.CellTextAlign.Center);
			return 0;
#else
			Table = new PdfTable(45, 70, 70, 40, 60, 90, 140);
			Table.Font = new XFont("Helvetica", 8, XFontStyle.Regular, FontOpts);
			Table.Pen = new XPen(XColors.Black, 0.1);

			Table.AddRow(XColors.LightGray);
			Table.FillRowText(
				Locale.report_time,
				Locale.report_frequency,
				Locale.report_band,
				"P/Δ",
				Locale.report_control,
				Locale.report_AntRpu,
				Locale.report_notes);

#if false
			foreach (SingleFreqSignalHitPoint point in signal.SignalHitPoints)
			{
				Table.AddRow();
				Table.FillRowText(
					String.Format("{0} / {1}", point.Timestamp.ToShortDateString(), point.Timestamp.ToLongTimeString()),
					String.Format("{0}", Utils.FreqUtils.FreqToString(point.Frequency)),
					String.Format("{0} / {1} " + Locale.ms, Utils.FreqUtils.FreqToString(point.FilterBand), point.AverageTime),
					String.Format("{0} / {1}", point.Power.ToString("F1"), point.Delta.ToString("F1")),
					String.Format("{0}", point.TraceControlName),
					String.Format("{0} / {1}", point.AntennaName, point.RPUName),
					String.Format(""));
				sgCount++;
			}
#endif

			//
			Table.SetColumnAlign(0, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HRight);
			Table.SetColumnAlign(1, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(2, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(3, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HCenter);
			Table.SetColumnAlign(4, PdfTable.CellTextAlign.VCenter | PdfTable.CellTextAlign.HLeft);

			Table.SetRowAlign(0, PdfTable.CellTextAlign.Center);
			return 0;
#endif
		}

		public bool Save(String Filename)
		{
			_Report = new _ReportClass();
			_Report.CurrentFont = HeaderFont;
			_Report.Owner = this;
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
