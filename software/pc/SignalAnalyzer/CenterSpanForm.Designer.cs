namespace SignalAnalyzer
{
	partial class CenterSpanForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CenterSpanForm));
			this.freqCenter = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.freqSpan = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// freqCenter
			// 
			this.freqCenter.Frequency = ((long)(0));
			resources.ApplyResources(this.freqCenter, "freqCenter");
			this.freqCenter.Max = ((long)(9223372036854775807));
			this.freqCenter.Min = ((long)(-9223372036854775808));
			this.freqCenter.Name = "freqCenter";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// freqSpan
			// 
			this.freqSpan.Frequency = ((long)(0));
			resources.ApplyResources(this.freqSpan, "freqSpan");
			this.freqSpan.Max = ((long)(9223372036854775807));
			this.freqSpan.Min = ((long)(-9223372036854775808));
			this.freqSpan.Name = "freqSpan";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// CenterSpanForm
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.freqSpan);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.freqCenter);
			this.Name = "CenterSpanForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ControlUtils.FrequencyTextBox.FrequencyTextBox freqCenter;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox freqSpan;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}