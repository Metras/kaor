namespace SignalAnalyzer
{
	partial class SpectrumControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpectrumControl));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.peakMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deltaMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.rBWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hzToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.kHzToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.updateFrequencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
			this.clearWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.minHoldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maxHoldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.averageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.timesToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.timesToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.zedFFT = new ZedGraph.ZedGraphControl();
			this.bgwSpectrum = new System.ComponentModel.BackgroundWorker();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Right;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripSplitButton1,
            this.toolStripDropDownButton3});
			this.toolStrip1.Location = new System.Drawing.Point(748, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(33, 344);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(30, 20);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerToolStripMenuItem,
            this.leftToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(30, 20);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// centerToolStripMenuItem
			// 
			this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
			this.centerToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.centerToolStripMenuItem.Text = "Center";
			this.centerToolStripMenuItem.Click += new System.EventHandler(this.centerToolStripMenuItem_Click);
			// 
			// leftToolStripMenuItem
			// 
			this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
			this.leftToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.leftToolStripMenuItem.Text = "Span";
			// 
			// toolStripDropDownButton2
			// 
			this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.peakMarkerToolStripMenuItem,
            this.deltaMarkerToolStripMenuItem});
			this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
			this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
			this.toolStripDropDownButton2.Size = new System.Drawing.Size(30, 20);
			this.toolStripDropDownButton2.Text = "toolStripDropDownButton2";
			// 
			// peakMarkerToolStripMenuItem
			// 
			this.peakMarkerToolStripMenuItem.Name = "peakMarkerToolStripMenuItem";
			this.peakMarkerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.peakMarkerToolStripMenuItem.Text = "Peak marker";
			// 
			// deltaMarkerToolStripMenuItem
			// 
			this.deltaMarkerToolStripMenuItem.Name = "deltaMarkerToolStripMenuItem";
			this.deltaMarkerToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.deltaMarkerToolStripMenuItem.Text = "Delta marker";
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rBWToolStripMenuItem,
            this.updateFrequencyToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(30, 20);
			this.toolStripSplitButton1.Text = "toolStripSplitButton1";
			// 
			// rBWToolStripMenuItem
			// 
			this.rBWToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hzToolStripMenuItem,
            this.hzToolStripMenuItem1,
            this.kHzToolStripMenuItem,
            this.kHzToolStripMenuItem1,
            this.kHzToolStripMenuItem2,
            this.kHzToolStripMenuItem3,
            this.kHzToolStripMenuItem4,
            this.kHzToolStripMenuItem5});
			this.rBWToolStripMenuItem.Name = "rBWToolStripMenuItem";
			this.rBWToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.rBWToolStripMenuItem.Text = "RBW";
			// 
			// hzToolStripMenuItem
			// 
			this.hzToolStripMenuItem.Name = "hzToolStripMenuItem";
			this.hzToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.hzToolStripMenuItem.Text = "10Hz";
			// 
			// hzToolStripMenuItem1
			// 
			this.hzToolStripMenuItem1.Name = "hzToolStripMenuItem1";
			this.hzToolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
			this.hzToolStripMenuItem1.Text = "100Hz";
			// 
			// kHzToolStripMenuItem
			// 
			this.kHzToolStripMenuItem.Name = "kHzToolStripMenuItem";
			this.kHzToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem.Text = "1kHz";
			// 
			// kHzToolStripMenuItem1
			// 
			this.kHzToolStripMenuItem1.Name = "kHzToolStripMenuItem1";
			this.kHzToolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem1.Text = "3kHz";
			// 
			// kHzToolStripMenuItem2
			// 
			this.kHzToolStripMenuItem2.Name = "kHzToolStripMenuItem2";
			this.kHzToolStripMenuItem2.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem2.Text = "6kHz";
			// 
			// kHzToolStripMenuItem3
			// 
			this.kHzToolStripMenuItem3.Name = "kHzToolStripMenuItem3";
			this.kHzToolStripMenuItem3.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem3.Text = "10kHz";
			// 
			// kHzToolStripMenuItem4
			// 
			this.kHzToolStripMenuItem4.Name = "kHzToolStripMenuItem4";
			this.kHzToolStripMenuItem4.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem4.Text = "20kHz";
			// 
			// kHzToolStripMenuItem5
			// 
			this.kHzToolStripMenuItem5.Name = "kHzToolStripMenuItem5";
			this.kHzToolStripMenuItem5.Size = new System.Drawing.Size(115, 22);
			this.kHzToolStripMenuItem5.Text = "50kHz";
			// 
			// updateFrequencyToolStripMenuItem
			// 
			this.updateFrequencyToolStripMenuItem.Name = "updateFrequencyToolStripMenuItem";
			this.updateFrequencyToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.updateFrequencyToolStripMenuItem.Text = "Update time";
			// 
			// toolStripDropDownButton3
			// 
			this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearWriteToolStripMenuItem,
            this.minHoldToolStripMenuItem,
            this.maxHoldToolStripMenuItem,
            this.averageToolStripMenuItem});
			this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
			this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
			this.toolStripDropDownButton3.Size = new System.Drawing.Size(30, 20);
			this.toolStripDropDownButton3.Text = "toolStripDropDownButton3";
			// 
			// clearWriteToolStripMenuItem
			// 
			this.clearWriteToolStripMenuItem.Name = "clearWriteToolStripMenuItem";
			this.clearWriteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.clearWriteToolStripMenuItem.Text = "Clear && Write";
			this.clearWriteToolStripMenuItem.Click += new System.EventHandler(this.clearWriteToolStripMenuItem_Click);
			// 
			// minHoldToolStripMenuItem
			// 
			this.minHoldToolStripMenuItem.Name = "minHoldToolStripMenuItem";
			this.minHoldToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.minHoldToolStripMenuItem.Text = "Min Hold";
			this.minHoldToolStripMenuItem.Click += new System.EventHandler(this.minHoldToolStripMenuItem_Click);
			// 
			// maxHoldToolStripMenuItem
			// 
			this.maxHoldToolStripMenuItem.Name = "maxHoldToolStripMenuItem";
			this.maxHoldToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.maxHoldToolStripMenuItem.Text = "Max Hold";
			this.maxHoldToolStripMenuItem.Click += new System.EventHandler(this.maxHoldToolStripMenuItem_Click);
			// 
			// averageToolStripMenuItem
			// 
			this.averageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timesToolStripMenuItem,
            this.timesToolStripMenuItem1,
            this.timesToolStripMenuItem2,
            this.timesToolStripMenuItem3});
			this.averageToolStripMenuItem.Name = "averageToolStripMenuItem";
			this.averageToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.averageToolStripMenuItem.Text = "Average";
			this.averageToolStripMenuItem.Click += new System.EventHandler(this.averageToolStripMenuItem_Click);
			// 
			// timesToolStripMenuItem
			// 
			this.timesToolStripMenuItem.Name = "timesToolStripMenuItem";
			this.timesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.timesToolStripMenuItem.Tag = "5";
			this.timesToolStripMenuItem.Text = "5 times";
			this.timesToolStripMenuItem.Click += new System.EventHandler(this.timesToolStripMenuItem_Click);
			// 
			// timesToolStripMenuItem1
			// 
			this.timesToolStripMenuItem1.Name = "timesToolStripMenuItem1";
			this.timesToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.timesToolStripMenuItem1.Tag = "10";
			this.timesToolStripMenuItem1.Text = "10 times";
			this.timesToolStripMenuItem1.Click += new System.EventHandler(this.timesToolStripMenuItem_Click);
			// 
			// timesToolStripMenuItem2
			// 
			this.timesToolStripMenuItem2.Name = "timesToolStripMenuItem2";
			this.timesToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
			this.timesToolStripMenuItem2.Tag = "50";
			this.timesToolStripMenuItem2.Text = "50 times";
			this.timesToolStripMenuItem2.Click += new System.EventHandler(this.timesToolStripMenuItem_Click);
			// 
			// timesToolStripMenuItem3
			// 
			this.timesToolStripMenuItem3.Name = "timesToolStripMenuItem3";
			this.timesToolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
			this.timesToolStripMenuItem3.Tag = "100";
			this.timesToolStripMenuItem3.Text = "100 times";
			this.timesToolStripMenuItem3.Click += new System.EventHandler(this.timesToolStripMenuItem_Click);
			// 
			// zedFFT
			// 
			this.zedFFT.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zedFFT.IsAutoCursor = true;
			this.zedFFT.Location = new System.Drawing.Point(0, 0);
			this.zedFFT.Name = "zedFFT";
			this.zedFFT.ScrollGrace = 0;
			this.zedFFT.ScrollMaxX = 0;
			this.zedFFT.ScrollMaxY = 0;
			this.zedFFT.ScrollMaxY2 = 0;
			this.zedFFT.ScrollMinX = 0;
			this.zedFFT.ScrollMinY = 0;
			this.zedFFT.ScrollMinY2 = 0;
			this.zedFFT.SelectAppendModifierKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.None)));
			this.zedFFT.Size = new System.Drawing.Size(748, 344);
			this.zedFFT.TabIndex = 1;
			// 
			// bgwSpectrum
			// 
			this.bgwSpectrum.WorkerSupportsCancellation = true;
			this.bgwSpectrum.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSpectrum_DoWork);
			// 
			// SpectrumControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.zedFFT);
			this.Controls.Add(this.toolStrip1);
			this.Name = "SpectrumControl";
			this.Size = new System.Drawing.Size(781, 344);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
		private System.Windows.Forms.ToolStripMenuItem peakMarkerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deltaMarkerToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem rBWToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem updateFrequencyToolStripMenuItem;
		private ZedGraph.ZedGraphControl zedFFT;
		private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hzToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem kHzToolStripMenuItem5;
		private System.ComponentModel.BackgroundWorker bgwSpectrum;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
		private System.Windows.Forms.ToolStripMenuItem clearWriteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem minHoldToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maxHoldToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem averageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem timesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem timesToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem timesToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem timesToolStripMenuItem3;
	}
}
