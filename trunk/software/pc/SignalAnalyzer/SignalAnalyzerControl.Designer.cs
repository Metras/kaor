namespace SignalAnalyzer
{
	partial class SignalAnalyzerControl
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
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnDemod = new System.Windows.Forms.Button();
			this.btnOscilloscope = new System.Windows.Forms.Button();
			this.btnIQ = new System.Windows.Forms.Button();
			this.btnSpectrum = new System.Windows.Forms.Button();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.groupBox1);
			this.splitMain.Size = new System.Drawing.Size(868, 277);
			this.splitMain.SplitterDistance = 709;
			this.splitMain.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnDemod);
			this.groupBox1.Controls.Add(this.btnOscilloscope);
			this.groupBox1.Controls.Add(this.btnIQ);
			this.groupBox1.Controls.Add(this.btnSpectrum);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(155, 277);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Analyzer Mode";
			// 
			// btnDemod
			// 
			this.btnDemod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnDemod.Enabled = false;
			this.btnDemod.Location = new System.Drawing.Point(6, 127);
			this.btnDemod.Name = "btnDemod";
			this.btnDemod.Size = new System.Drawing.Size(143, 30);
			this.btnDemod.TabIndex = 3;
			this.btnDemod.Text = "Demodulation";
			this.btnDemod.UseVisualStyleBackColor = true;
			this.btnDemod.Click += new System.EventHandler(this.btnDemod_Click);
			// 
			// btnOscilloscope
			// 
			this.btnOscilloscope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnOscilloscope.Enabled = false;
			this.btnOscilloscope.Location = new System.Drawing.Point(6, 91);
			this.btnOscilloscope.Name = "btnOscilloscope";
			this.btnOscilloscope.Size = new System.Drawing.Size(143, 30);
			this.btnOscilloscope.TabIndex = 2;
			this.btnOscilloscope.Text = "Oscilliscope";
			this.btnOscilloscope.UseVisualStyleBackColor = true;
			this.btnOscilloscope.Click += new System.EventHandler(this.btnOscilloscope_Click);
			// 
			// btnIQ
			// 
			this.btnIQ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnIQ.Enabled = false;
			this.btnIQ.Location = new System.Drawing.Point(6, 55);
			this.btnIQ.Name = "btnIQ";
			this.btnIQ.Size = new System.Drawing.Size(143, 30);
			this.btnIQ.TabIndex = 1;
			this.btnIQ.Text = "IQ polar";
			this.btnIQ.UseVisualStyleBackColor = true;
			// 
			// btnSpectrum
			// 
			this.btnSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSpectrum.Enabled = false;
			this.btnSpectrum.Location = new System.Drawing.Point(6, 19);
			this.btnSpectrum.Name = "btnSpectrum";
			this.btnSpectrum.Size = new System.Drawing.Size(143, 30);
			this.btnSpectrum.TabIndex = 0;
			this.btnSpectrum.Text = "Spectrum";
			this.btnSpectrum.UseVisualStyleBackColor = true;
			this.btnSpectrum.Click += new System.EventHandler(this.btnSpectrum_Click);
			// 
			// SignalAnalyzerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Name = "SignalAnalyzerControl";
			this.Size = new System.Drawing.Size(868, 277);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnDemod;
		private System.Windows.Forms.Button btnOscilloscope;
		private System.Windows.Forms.Button btnIQ;
		private System.Windows.Forms.Button btnSpectrum;


	}
}
