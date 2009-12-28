namespace KaorCore.RPUZero
{
	partial class RPUZeroControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPUZeroControl));
			this.grpDemod = new System.Windows.Forms.GroupBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.frequencyRadio1 = new ControlUtils.FrequencyRadio.FrequencyRadio();
			this.grpAntenna = new System.Windows.Forms.GroupBox();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpDemod
			// 
			resources.ApplyResources(this.grpDemod, "grpDemod");
			this.grpDemod.Name = "grpDemod";
			this.grpDemod.TabStop = false;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.DimGray;
			this.panel4.Controls.Add(this.frequencyRadio1);
			resources.ApplyResources(this.panel4, "panel4");
			this.panel4.Name = "panel4";
			// 
			// frequencyRadio1
			// 
			this.frequencyRadio1.BackColor = System.Drawing.Color.DimGray;
			this.frequencyRadio1.DelayChange = 200;
			resources.ApplyResources(this.frequencyRadio1, "frequencyRadio1");
			this.frequencyRadio1.ForeColor = System.Drawing.Color.LightSkyBlue;
			this.frequencyRadio1.Frequency = ((long)(20000000));
			this.frequencyRadio1.Max = ((long)(3000000000));
			this.frequencyRadio1.Min = ((long)(20000000));
			this.frequencyRadio1.Name = "frequencyRadio1";
			this.frequencyRadio1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			this.frequencyRadio1.FrequencyChanged += new ControlUtils.FrequencyRadio.FrequencyRadio.FrequencyChangedHandler(this.frequencyRadio1_FrequencyChanged);
			// 
			// grpAntenna
			// 
			resources.ApplyResources(this.grpAntenna, "grpAntenna");
			this.grpAntenna.Name = "grpAntenna";
			this.grpAntenna.TabStop = false;
			// 
			// RPUZeroControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpDemod);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.grpAntenna);
			this.Name = "RPUZeroControl";
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpDemod;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.GroupBox grpAntenna;
		public ControlUtils.FrequencyRadio.FrequencyRadio frequencyRadio1;
	}
}
