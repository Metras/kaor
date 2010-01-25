namespace KaorCore.Trace
{
	partial class NewTraceDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTraceDialog));
			this.txtTraceName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTraceDescr = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbTraceColor = new Com.Windows.Forms.ColorPicker();
			this.dlgFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.txtFstop = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.txtFstart = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMeasureStep = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.numInitial = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numInitial)).BeginInit();
			this.SuspendLayout();
			// 
			// txtTraceName
			// 
			resources.ApplyResources(this.txtTraceName, "txtTraceName");
			this.txtTraceName.Name = "txtTraceName";
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
			// txtTraceDescr
			// 
			resources.ApplyResources(this.txtTraceDescr, "txtTraceDescr");
			this.txtTraceDescr.Name = "txtTraceDescr";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// cmbTraceColor
			// 
			this.cmbTraceColor.BackColor = System.Drawing.SystemColors.Window;
			this.cmbTraceColor.Context = null;
			this.cmbTraceColor.ForeColor = System.Drawing.SystemColors.WindowText;
			resources.ApplyResources(this.cmbTraceColor, "cmbTraceColor");
			this.cmbTraceColor.Name = "cmbTraceColor";
			this.cmbTraceColor.ReadOnly = false;
			this.cmbTraceColor.Value = System.Drawing.Color.LimeGreen;
			// 
			// txtFstop
			// 
			this.txtFstop.Frequency = ((long)(20000000));
			resources.ApplyResources(this.txtFstop, "txtFstop");
			this.txtFstop.Max = ((long)(40000000000));
			this.txtFstop.Min = ((long)(20000000));
			this.txtFstop.Name = "txtFstop";
			this.txtFstop.TextChanged += new System.EventHandler(this.txtFstop_TextChanged);
			// 
			// txtFstart
			// 
			this.txtFstart.Frequency = ((long)(20000000));
			resources.ApplyResources(this.txtFstart, "txtFstart");
			this.txtFstart.Max = ((long)(40000000000));
			this.txtFstart.Min = ((long)(20000000));
			this.txtFstart.Name = "txtFstart";
			this.txtFstart.TextChanged += new System.EventHandler(this.txtFstart_TextChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// txtMeasureStep
			// 
			resources.ApplyResources(this.txtMeasureStep, "txtMeasureStep");
			this.txtMeasureStep.Frequency = ((long)(100000));
			this.txtMeasureStep.Max = ((long)(3000000000));
			this.txtMeasureStep.Min = ((long)(0));
			this.txtMeasureStep.Name = "txtMeasureStep";
			this.txtMeasureStep.TextChanged += new System.EventHandler(this.txtMeasureStep_TextChanged);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// numInitial
			// 
			resources.ApplyResources(this.numInitial, "numInitial");
			this.numInitial.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numInitial.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numInitial.Name = "numInitial";
			this.numInitial.Value = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			// 
			// NewTraceDialog
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.numInitial);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtMeasureStep);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtFstop);
			this.Controls.Add(this.txtFstart);
			this.Controls.Add(this.cmbTraceColor);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtTraceDescr);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtTraceName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewTraceDialog";
			this.Shown += new System.EventHandler(this.NewTraceDialog_Shown);
			((System.ComponentModel.ISupportInitialize)(this.numInitial)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label9;
		public Com.Windows.Forms.ColorPicker cmbTraceColor;
		public System.Windows.Forms.TextBox txtTraceName;
		public System.Windows.Forms.TextBox txtTraceDescr;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox txtFstart;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox txtFstop;
		private System.Windows.Forms.FolderBrowserDialog dlgFolderBrowser;
		private System.Windows.Forms.Label label5;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox txtMeasureStep;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numInitial;
	}
}