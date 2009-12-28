using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing.Layout;

namespace KaorCore.Report
{
	class PdfUtils
	{
		private static XGraphics _gfx;
		private static PdfDocument _doc;

		private static XGraphics gfx
		{
			get
			{
				if (_gfx != null)
					return _gfx;
				else
				{
					_doc = new PdfDocument();
					_gfx = XGraphics.FromPdfPage(_doc.AddPage());
					return _gfx;
				}
			}
		}

		public static Double StringWidth(String S, XFont Fnt)
		{
			return gfx.MeasureString('x' + S + 'x', Fnt).Width - gfx.MeasureString("xx", Fnt).Width;
		}

		public static Double StringHeight(String S, XFont Fnt)
		{
			return gfx.MeasureString(S, Fnt).Height;
		}

		public static List<String> SplitString(String S)
		{
			List<String> strings = new List<String>(16);
			Int32 Pos = 0;
			Int32 tPos = Pos;
			while (Pos < S.Length)
			{
				if (S[Pos].ToString(CultureInfo.InvariantCulture) != " ")
					Pos++;
				else
				{
					Pos++;
					if (Pos > tPos)
						strings.Add(S.Substring(tPos, Pos - tPos - 1));
					tPos = Pos;
				}
			}
			if (Pos != tPos)
				strings.Add(S.Substring(tPos, Pos - tPos));
			return strings;
		}

		public static List<String> SplitString(String S, XFont Fnt, Double Width, Double FirstIndent, Double OtherIndent)
		{
			// Split string to substrings
			List<String> sList = SplitString(S);
			XUnit[] sWidth = new XUnit[sList.Count];

			// Calculate width of all substrings
			for (Int32 i = 0; i < sList.Count; i++)
				sWidth[i] = StringWidth(sList[i], Fnt);
			
			// Try to fit subs in cell
			List<String> Lines = new List<String>();
			StringBuilder sb = new StringBuilder();
			Int32 SubIdx = 0;
			XUnit tWidth = FirstIndent;
			while (SubIdx < sList.Count)
			{
				if (sList[SubIdx] == "/")
				{
					Lines.Add(sb.ToString());
					sb.Remove(0, sb.Length);
					tWidth = OtherIndent;
					SubIdx++;
				}
				else if (sb.Length == 0)
				{
					tWidth += sWidth[SubIdx];
					sb.Append(sList[SubIdx]);
					SubIdx++;
				}
				else if ((tWidth.Point + sWidth[SubIdx].Point) > Width)
				{
					Lines.Add(sb.ToString());
					sb.Remove(0, sb.Length);
					tWidth = OtherIndent;
				}
				else
				{
					tWidth += sWidth[SubIdx];
					sb.Append(" ");
					sb.Append(sList[SubIdx]);
					SubIdx++;
				}
			}
			if (sb.Length > 0) Lines.Add(sb.ToString());
			return Lines;
		}

		public static List<String> SplitString(String S, XFont Fnt, Double Width)
		{
			return SplitString(S, Fnt, Width, 0, 0);
		}

	}
}
