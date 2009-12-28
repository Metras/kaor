using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing.Layout;

namespace KaorCore.Report
{
	public class PdfTable
	{
		[Flags]
		public enum CellTextAlign 
		{ 
			HLeft	= 0x01,
			HRight	= 0x02,
			HCenter = 0x03,
			VTop	= 0x04,
			VBottom	= 0x08,
			VCenter	= 0x0C,
			Center	= 0x0F
		}

		[Flags]
		public enum CellBorders
		{
			None	= 0x00,
			Left	= 0x01,
			Right	= 0x02,
			Top		= 0x04,
			Bottom	= 0x08,
			All		= 0x0F
		}

		/// <summary>
		/// Одна ячейка таблицы
		/// </summary>
		public struct TableCell
		{
			public String Text;
			public XColor TextColor;
			public XColor BkColor;
			public CellTextAlign Align;
			public CellBorders Borders;

			public TableCell(String Text)
			{
				this.Text = Text;
				this.TextColor = XColors.Black;
				this.BkColor = XColors.Transparent;
				this.Align = CellTextAlign.Center;
				this.Borders = CellBorders.All;
			}
		}

		public struct TableRow
		{
			public TableCell[] Cells;
			public Int32 Height;
		}

		public XUnit Span = XUnit.FromMillimeter(1);
		internal Int32[] colWidths;
		internal TableRow[] Rows;
		internal Int32 currentRow = -1;

		public XColor DefTextColor = XColors.Black;
		public XColor DefBkColor = XColors.Transparent;
		public CellBorders DefBorders = CellBorders.All;
		public CellTextAlign DefTextAlign = CellTextAlign.Center;

		public Int32 DefRowHeight = 20;
		public XFont Font;
		public XBrush Brush = XBrushes.Black;
		public XPen Pen = XPens.Black;

		public bool RepeatHeader = true;

		public Int32 RowCount
		{ get { return currentRow + 1; } }

		public PdfTable(params Int32[] ColumnWidths)
		{
			colWidths = new Int32[ColumnWidths.Length];
			Array.Copy(ColumnWidths, colWidths, ColumnWidths.Length);
			Rows = new TableRow[16];
		}

		public void ModifyCell(Int32 Column, TableCell Cell)
		{
			Rows[currentRow].Cells[Column] = Cell;
		}

		public void ModifyCell(Int32 Column, String Text)
		{
			Rows[currentRow].Cells[Column].Text = Text;
		}

		public TableCell GetCell(Int32 Column)
		{
			return Rows[currentRow].Cells[Column];
		}

		public void FillRowText(params String[] Text)
		{
			for (int i = 0; i < Text.Length; i++)
				Rows[currentRow].Cells[i].Text = Text[i];
		}

		public void AddRow(XColor BkColor)
		{
			currentRow++;
			if (Rows.Length <= currentRow)
				Array.Resize<TableRow>(ref Rows, Rows.Length + 16);
			TableRow row = new TableRow();
			row.Cells = new TableCell[colWidths.Length];
			Rows[currentRow] = row;
			for (int i = 0; i < colWidths.Length; i++)
			{
				TableCell cell = new TableCell("");
				cell.BkColor = BkColor;
				cell.TextColor = DefTextColor;
				cell.Borders = DefBorders;
				cell.Align = DefTextAlign;
				Rows[currentRow].Cells[i] = cell;
			}
		}

		public void AddRow()
		{
			AddRow(DefBkColor);
		}

		public void SetColumnAlign(Int32 Column, CellTextAlign Align)
		{
			for (int i = 0; i <= currentRow; i++)
				Rows[i].Cells[Column].Align = Align;
		}

		public void SetRowAlign(Int32 Row, CellTextAlign Align)
		{
			for (int i = 0; i < Rows[Row].Cells.Length; i++)
				Rows[Row].Cells[i].Align = Align;
		}

		public void SetCellBorders(Int32 Row, Int32 Col, CellBorders Borders)
		{
			Rows[Row].Cells[Col].Borders = Borders;
		}
	}
}
