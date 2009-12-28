namespace SignalAnalyzer.Demodulators
{
	partial class GSMDemodulatorControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.frequencyTextBox1 = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.iqPlot = new ControlUtils.ComplexAnimatePlot.ComplexAnimatePlot();
			this.iqPlotEqualized = new ControlUtils.ComplexAnimatePlot.ComplexAnimatePlot();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Freq:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.frequencyTextBox1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(134, 96);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Radio control";
			// 
			// frequencyTextBox1
			// 
			this.frequencyTextBox1.Frequency = ((long)(0));
			this.frequencyTextBox1.Location = new System.Drawing.Point(43, 18);
			this.frequencyTextBox1.Max = ((long)(9223372036854775807));
			this.frequencyTextBox1.Min = ((long)(-9223372036854775808));
			this.frequencyTextBox1.Name = "frequencyTextBox1";
			this.frequencyTextBox1.Size = new System.Drawing.Size(85, 20);
			this.frequencyTextBox1.TabIndex = 6;
			this.frequencyTextBox1.Text = "0";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(75, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "0Hz";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(75, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "SB SYNC";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "FB SYNC";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Freq offset:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(143, 14);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(110, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// iqPlot
			// 
			this.iqPlot.EnableLine = true;
			this.iqPlot.FadeOut = 16;
			this.iqPlot.ImagMax = 0.1;
			this.iqPlot.Location = new System.Drawing.Point(3, 105);
			this.iqPlot.MinimumSize = new System.Drawing.Size(100, 100);
			this.iqPlot.Name = "iqPlot";
			this.iqPlot.RealMax = 0.1;
			this.iqPlot.Size = new System.Drawing.Size(384, 384);
			this.iqPlot.TabIndex = 4;
			this.iqPlot.TimeOut = 100;
			// 
			// iqPlotEqualized
			// 
			this.iqPlotEqualized.EnableLine = true;
			this.iqPlotEqualized.FadeOut = 16;
			this.iqPlotEqualized.ImagMax = 0.1;
			this.iqPlotEqualized.LineColor = System.Drawing.Color.Red;
			this.iqPlotEqualized.Location = new System.Drawing.Point(530, 105);
			this.iqPlotEqualized.MinimumSize = new System.Drawing.Size(100, 100);
			this.iqPlotEqualized.Name = "iqPlotEqualized";
			this.iqPlotEqualized.RealMax = 0.1;
			this.iqPlotEqualized.Size = new System.Drawing.Size(384, 384);
			this.iqPlotEqualized.TabIndex = 5;
			this.iqPlotEqualized.TimeOut = 100;
			// 
			// GSMDemodulatorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.iqPlotEqualized);
			this.Controls.Add(this.iqPlot);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Name = "GSMDemodulatorControl";
			this.Size = new System.Drawing.Size(917, 509);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox frequencyTextBox1;
		private System.Windows.Forms.Button button1;
		private ControlUtils.ComplexAnimatePlot.ComplexAnimatePlot iqPlot;
		private ControlUtils.ComplexAnimatePlot.ComplexAnimatePlot iqPlotEqualized;
	}
}
