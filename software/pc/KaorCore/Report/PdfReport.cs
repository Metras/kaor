using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing.Layout;

namespace KaorCore.Report
{
	public class PdfReport
	{
		public enum XStringAlign { Left, Center, Right };

		public struct PageMargins
		{
			public XUnit Left;
			public XUnit Right;
			public XUnit Top;
			public XUnit Bottom;

			public PageMargins(Double Left, Double Right, Double Top, Double Bottom)
			{
				this.Left = Left;
				this.Right = Right;
				this.Top = Top;
				this.Bottom = Bottom;
			}
		}

		public PageMargins Margins = new PageMargins(
			XUnit.FromCentimeter(1.5f), 
			XUnit.FromCentimeter(1.5f), 
			XUnit.FromCentimeter(1.5f), 
			XUnit.FromCentimeter(0.5f));

		protected PdfDocument Doc;
		protected PdfPage CurrentPage;
		public XUnit CaretY;
		private XUnit CellSpan = 0;

		protected XPdfFontOptions fontOpts = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Automatic);
		public XFont CurrentFont;
		public XBrush CurrentBrush = XBrushes.Transparent;
		public XPen CurrentPen = XPens.Black;

		protected struct HeaderInfo
		{
			public List<String> Lines;
			public Double UnderlWidth;
			public XImage Image;
			public Double ImgSize;
		}
		protected HeaderInfo hdrInfo = new HeaderInfo();

		public void Save(String FileName)
		{
			Doc.Save(FileName);
		}

		private void LoadPageHeader()
		{
			String XmlFName = AppDomain.CurrentDomain.BaseDirectory + @"\ReportHeader.xml";
			if (!System.IO.File.Exists(XmlFName))
				return;
			XmlReader xmlReader = XmlReader.Create(XmlFName);
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(xmlReader);
			String Org = xmlDoc.GetElementsByTagName("Org")[0].InnerText;
			String St = xmlDoc.GetElementsByTagName("Station")[0].InnerText;
			String Notes = xmlDoc.GetElementsByTagName("Notes")[0].InnerText;
			String ImgFileName = xmlDoc.GetElementsByTagName("FileName")[0].InnerText;
			Double ImgSize = Double.Parse(xmlDoc.GetElementsByTagName("ImgSize")[0].InnerText) / 100.0f;
			hdrInfo.UnderlWidth = Double.Parse(xmlDoc.GetElementsByTagName("USize")[0].InnerText) / 100.0f;
			xmlReader.Close();

			// Insert organization info
			hdrInfo.Lines = new List<String>();
			hdrInfo.Lines.Add(Org);
			hdrInfo.Lines.Add(St);
			hdrInfo.Lines.Add(Notes);

			// Add timestamp to header
			DateTime dt = DateTime.Now;
			hdrInfo.Lines.Add(String.Format("{0} {1}", dt.ToShortDateString(), dt.ToLongTimeString()));

			if ((ImgFileName != "") && (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\" + ImgFileName)))
				hdrInfo.Image = XImage.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\" + ImgFileName);
			hdrInfo.ImgSize = ImgSize;
		}

		public PdfReport()
		{
			// Initialize document
			Doc = new PdfDocument();
			CurrentPage = Doc.AddPage();
			CaretY = Margins.Top;

			CurrentFont = new XFont("Helvetica", 12, XFontStyle.Regular, fontOpts);
			CurrentBrush = XBrushes.Black;
			CurrentPen = new XPen(XColors.Black, 0.1f);
			// Load header info
			LoadPageHeader();
			DrawPageHeader();
		}

		public void PrintStrings(List<String> Strings, XStringAlign Align, XRect Rect, XUnit FirstIndent, XUnit OtherIndent)
		{
			XUnit tx, sw, th;
			XGraphics gfx = XGraphics.FromPdfPage(CurrentPage);
			XPen pen = new XPen(XColors.Black, 0.2);

			for (int Idx = 0; Idx < Strings.Count; Idx++)
			{
				String S = Strings[Idx];
				sw = PdfUtils.StringWidth(S, CurrentFont);
				switch (Align)
				{
					case (XStringAlign.Left):
						if (Idx == 0)
							tx = Rect.Left + FirstIndent;
						else
							tx = Rect.Left + OtherIndent;
						break;
					case (XStringAlign.Right):
						tx = Rect.Right - sw;
						break;
					case (XStringAlign.Center):
						tx = Rect.Left + (Rect.Width - sw) / 2;
						break;
					default:
						throw new Exception("Invalid cell align");
				}
				th = PdfUtils.StringHeight(S, CurrentFont);
				TestBottomSpace(th);
				gfx.DrawString(S, CurrentFont, CurrentBrush, tx, CaretY);
				CaretY += th;
			}
			gfx.Dispose();
		}

		public void PrintStrings(List<String> Strings, XStringAlign Align, XUnit FirstIndent, XUnit OtherIndent)
		{
			PrintStrings(
				Strings, Align, 
				new XRect(
					Margins.Left,
					Margins.Top,
					CurrentPage.Width - Margins.Left - Margins.Right, 
					CurrentPage.Height - Margins.Top - Margins.Bottom), 
				FirstIndent, OtherIndent);
		}

		public void PrintStrings(List<String> Strings, XStringAlign Align, XRect Rect)
		{
			PrintStrings(Strings, Align, Rect, 0, 0);
		}

		public void PrintStrings(List<String> Strings, XStringAlign Align)
		{
			PrintStrings(Strings, Align, 0, 0);
		}


		protected void DrawPageHeader()
		{
			XFont OldFont = CurrentFont;
			CurrentFont = new XFont("Helvetica", 12, XFontStyle.Regular, fontOpts);
			PrintStrings(hdrInfo.Lines, XStringAlign.Left);
			CaretY += XUnit.FromMillimeter(5);
			if (hdrInfo.Image != null)
			{
				Double imgW = XUnit.FromPoint(hdrInfo.Image.PointWidth) * hdrInfo.ImgSize;
				Double imgH = XUnit.FromPoint(hdrInfo.Image.PointHeight) * hdrInfo.ImgSize;
				XGraphics gfx = XGraphics.FromPdfPage(CurrentPage);
				gfx.DrawImage(
					hdrInfo.Image,
					CurrentPage.Width - Margins.Right - imgW,
					Margins.Top, imgW, imgH);
				gfx.Dispose();
			}
			CurrentFont = OldFont;
		}

		protected virtual void OnNextPage()
		{
			DrawPageHeader();
		}

		private bool TestBottomSpace(XUnit SpaceNeeded)
		{
			if ((CaretY.Point + SpaceNeeded.Point) > (CurrentPage.Height.Point - XUnit.FromMillimeter(Margins.Bottom).Point))
			{
				CurrentPage = Doc.AddPage();
				CaretY = Margins.Top;
				OnNextPage();
				return false;
			}
			else
				return true;
		}


		public void Numerate()
		{
			XFont NumFont = new XFont("Helvetica", 8, XFontStyle.Regular, fontOpts);
			Int32 page = 0;
			XGraphics gfx;

			while (page < Doc.PageCount)
			{
				gfx = XGraphics.FromPdfPage(Doc.Pages[page]);
				gfx.DrawString(
					String.Format("{0}/{1}", page + 1, Doc.PageCount),
					NumFont, XBrushes.Black,
					Doc.Pages[page].Width - XUnit.FromCentimeter(1.2),
					Doc.Pages[page].Height - XUnit.FromCentimeter(1));
				gfx.Dispose();
				page++;
			}
		}


		private void DrawTableCell(XGraphics Gfx, XRect R, List<String> Text, PdfTable.TableCell Cell)
		{
			// Draw cell background
			Gfx.DrawRectangle(new XSolidBrush(Cell.BkColor), R);

			XUnit X, Y, tW;
			// Calc text Y-point
			XUnit strH = Text.Count * PdfUtils.StringHeight("S", CurrentFont);
			switch (Cell.Align & PdfTable.CellTextAlign.VCenter)
			{
				case (PdfTable.CellTextAlign.VTop):
					Y = R.Top + CellSpan;
					break;
				case (PdfTable.CellTextAlign.VBottom):
					Y = R.Bottom - strH - CellSpan;
					break;
				case (PdfTable.CellTextAlign.VCenter):
					Y = R.Top + (R.Height - strH) / 2;
					break;
				default:
					throw new Exception("Invalid cell align");
			}
			Y += PdfUtils.StringHeight("S", CurrentFont) * 0.7f;
			// Draw separate strings
			Gfx.Save();
			Gfx.IntersectClip(R);
			foreach (String S in Text)
			{
				// Set left coord
				tW = PdfUtils.StringWidth(S, CurrentFont);
				switch (Cell.Align & PdfTable.CellTextAlign.HCenter)
				{
					case (PdfTable.CellTextAlign.HLeft):
						X = R.Left + CellSpan;
						break;
					case (PdfTable.CellTextAlign.HRight):
						X = R.Right - tW - CellSpan;
						break;
					case (PdfTable.CellTextAlign.HCenter):
						X = R.Left + (R.Width - tW) / 2;
						break;
					default:
						throw new Exception("Invalid cell align");
				}
				Gfx.DrawString(S, CurrentFont, CurrentBrush, X, Y);
				Y += PdfUtils.StringHeight("S", CurrentFont);
			}
			Gfx.Restore();
			// Draw cell borders
			if (Cell.Borders == PdfTable.CellBorders.All)
				Gfx.DrawRectangle(CurrentPen, R);
			else if (Cell.Borders != PdfTable.CellBorders.None)
			{
				if ((Cell.Borders & PdfTable.CellBorders.Left) != 0)
					Gfx.DrawLine(CurrentPen, R.BottomLeft, R.TopLeft);
				if ((Cell.Borders & PdfTable.CellBorders.Right) != 0)
					Gfx.DrawLine(CurrentPen, R.BottomRight, R.TopRight);
				if ((Cell.Borders & PdfTable.CellBorders.Top) != 0)
					Gfx.DrawLine(CurrentPen, R.TopLeft, R.TopRight);
				if ((Cell.Borders & PdfTable.CellBorders.Bottom) != 0)
					Gfx.DrawLine(CurrentPen, R.BottomLeft, R.BottomRight);
			}
		}

		private void DrawTableRow(PdfTable Table, Int32 Row)
		{
			XUnit X = Margins.Left;
			XRect R;

			// Allocate string list for each cell
			Object[] cellsText = new Object[Table.Rows[Row].Cells.Length];

			// Fill cell text according to cell width
			int maxLines = 0;
			for (int cCol = 0; cCol < cellsText.Length; cCol++)
			{
				cellsText[cCol] = new List<String>();
				// Split text string by lines
				cellsText[cCol] = PdfUtils.SplitString(
					Table.Rows[Row].Cells[cCol].Text,
					CurrentFont, Table.colWidths[cCol] - CellSpan * 2);
				// Find cell with max lines count
				if (((List<String>)cellsText[cCol]).Count > maxLines)
					maxLines = ((List<String>)cellsText[cCol]).Count;
			}

			// Calc maximum cell height
			XUnit cellH = PdfUtils.StringHeight("S", CurrentFont) * maxLines + CellSpan * 2;
			// Next page if needed
			if (!TestBottomSpace(cellH))
				if (Table.RepeatHeader)
					DrawTableRow(Table, 0);

			XGraphics Gfx = XGraphics.FromPdfPage(CurrentPage);
			for (int Col = 0; Col < cellsText.Length; Col++)
			{
				R = new XRect(X, CaretY, Table.colWidths[Col], cellH);
				DrawTableCell(Gfx, R, (List<String>)cellsText[Col], Table.Rows[Row].Cells[Col]);
				X += Table.colWidths[Col];
			}
			CaretY += cellH;
			Gfx.Dispose();
		}


		public void DrawTable(PdfTable Table)
		{
			XPen OldPen = CurrentPen;
			XBrush OldBrush = CurrentBrush;
			XFont OldFont = CurrentFont;
			XUnit OldSpan = CellSpan;
			CellSpan = Table.Span;

			CurrentFont = Table.Font;
			CurrentBrush = Table.Brush;
			CurrentPen = Table.Pen;

			for (int cRow = 0; cRow < Table.RowCount; cRow++)
			{
				DrawTableRow(Table, cRow);
			}
			CaretY += XUnit.FromMillimeter(5);

			CurrentPen = OldPen;
			CurrentBrush = OldBrush;
			CurrentFont = OldFont;
			CellSpan = OldSpan;
		}

	}
}
