namespace SignalAnalyzer
{
	partial class OscilloscopeControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OscilloscopeControl));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.zedOscilloscope = new ZedGraph.ZedGraphControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnStartStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.showRealDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showImagDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bgwRunOscilloscope = new System.ComponentModel.BackgroundWorker();
			this.toolBtnFrequency = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.zedOscilloscope);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer1.Size = new System.Drawing.Size(951, 510);
			this.splitContainer1.SplitterDistance = 395;
			this.splitContainer1.TabIndex = 0;
			// 
			// zedOscilloscope
			// 
			this.zedOscilloscope.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zedOscilloscope.IsAutoCursor = true;
			this.zedOscilloscope.Location = new System.Drawing.Point(0, 0);
			this.zedOscilloscope.Name = "zedOscilloscope";
			this.zedOscilloscope.ScrollGrace = 0;
			this.zedOscilloscope.ScrollMaxX = 0;
			this.zedOscilloscope.ScrollMaxY = 0;
			this.zedOscilloscope.ScrollMaxY2 = 0;
			this.zedOscilloscope.ScrollMinX = 0;
			this.zedOscilloscope.ScrollMinY = 0;
			this.zedOscilloscope.ScrollMinY2 = 0;
			this.zedOscilloscope.SelectAppendModifierKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.None)));
			this.zedOscilloscope.Size = new System.Drawing.Size(919, 395);
			this.zedOscilloscope.TabIndex = 1;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Right;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartStop,
            this.toolBtnFrequency,
            this.toolStripDropDownButton1});
			this.toolStrip1.Location = new System.Drawing.Point(919, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(32, 395);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnStartStop
			// 
			this.btnStartStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnStartStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStartStop.Image")));
			this.btnStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(29, 20);
			this.btnStartStop.Text = "toolStripButton1";
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showRealDataToolStripMenuItem,
            this.showImagDataToolStripMenuItem});
			this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
			this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
			// 
			// showRealDataToolStripMenuItem
			// 
			this.showRealDataToolStripMenuItem.Checked = true;
			this.showRealDataToolStripMenuItem.CheckOnClick = true;
			this.showRealDataToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showRealDataToolStripMenuItem.Name = "showRealDataToolStripMenuItem";
			this.showRealDataToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.showRealDataToolStripMenuItem.Text = "Show Real data";
			this.showRealDataToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showRealDataToolStripMenuItem_CheckedChanged);
			// 
			// showImagDataToolStripMenuItem
			// 
			this.showImagDataToolStripMenuItem.Checked = true;
			this.showImagDataToolStripMenuItem.CheckOnClick = true;
			this.showImagDataToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showImagDataToolStripMenuItem.Name = "showImagDataToolStripMenuItem";
			this.showImagDataToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.showImagDataToolStripMenuItem.Text = "Show Imag data";
			this.showImagDataToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showImagDataToolStripMenuItem_CheckedChanged);
			// 
			// bgwRunOscilloscope
			// 
			this.bgwRunOscilloscope.WorkerSupportsCancellation = true;
			this.bgwRunOscilloscope.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRunOscilloscope_DoWork);
			// 
			// toolBtnFrequency
			// 
			this.toolBtnFrequency.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolBtnFrequency.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnFrequency.Image")));
			this.toolBtnFrequency.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolBtnFrequency.Name = "toolBtnFrequency";
			this.toolBtnFrequency.Size = new System.Drawing.Size(29, 20);
			this.toolBtnFrequency.Text = "toolStripButton1";
			this.toolBtnFrequency.Click += new System.EventHandler(this.toolBtnFrequency_Click);
			// 
			// OscilloscopeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "OscilloscopeControl";
			this.Size = new System.Drawing.Size(951, 510);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private ZedGraph.ZedGraphControl zedOscilloscope;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton btnStartStop;
		private System.ComponentModel.BackgroundWorker bgwRunOscilloscope;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem showRealDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showImagDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolBtnFrequency;
	}
}
