using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace ControlUtils.Splash
{
	/// <summary>
	/// Summary description for MarqueeLabel.
	/// </summary>
	public class MarqueeLabel : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;

		public MarqueeLabel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.tmrScroll = new System.Windows.Forms.Timer(this.components);
			// 
			// tmrScroll
			// 
			this.tmrScroll.Tick += new System.EventHandler(this.tmrScroll_Tick);
			// 
			// MarqueeLabel
			// 
			this.Name = "MarqueeLabel";
			this.Size = new System.Drawing.Size(360, 104);
			this.Load += new System.EventHandler(this.MarqueeLabel_Load);

		}
		#endregion

		private string text;
		private int scrollAmount = 10;
		internal System.Windows.Forms.Timer tmrScroll;
		private int position = 0;

		private void MarqueeLabel_Load(object sender, System.EventArgs e)
		{
			this.ResizeRedraw = true;
			if (!this.DesignMode)
			{
				tmrScroll.Enabled = true;
			}

		}

		private void tmrScroll_Tick(object sender, System.EventArgs e)
		{
			position -= scrollAmount;

			// Force a refresh.
			this.Invalidate();

		}

		[Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				this.Invalidate();
			}
		}

		public int ScrollTimeInterval
		{
			get
			{
				return tmrScroll.Interval;
			}
			set
			{
				tmrScroll.Interval = value;
			}
		}

		[DefaultValue(10)]
		public int ScrollPixelAmount
		{
			get
			{
				return scrollAmount;
			}
			set
			{
				scrollAmount = value;
			}
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			// Do nothing.
			// To prevent flicker, we will draw both the background and the text
			// to a buffered image, and draw it to the control all at once.
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams _c = base.CreateParams;
				_c.ExStyle |= 0x00000020;

				return _c;
			}
		}
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// The following line avoids a design-time error that would
			// otherwise occur when the control is first loaded (but does not yet
			// have a defined size).
			if (e.ClipRectangle.Height == 0)
			{
				return;
			}

			base.OnPaint(e);
			if (position < 0)
			{
				// Reset the text to scroll back onto the control.
				position = (int)this.Height;// (int)e.Graphics.MeasureString(text, this.Font).Height;
			}

			// Create the drawing area in memory.
			// Double buffering is used to prevent flicker.
			Bitmap blt = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
			Graphics g = Graphics.FromImage(blt);

			g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
			//g.FillRectangle(new SolidBrush(this.BackColor), e.ClipRectangle);
			g.DrawString(text, this.Font, new SolidBrush(this.ForeColor), 0, position);

			// Render the finished image on the form.
			e.Graphics.DrawImageUnscaled(blt, 0, 0);

			g.Dispose();
		}
	}
}
