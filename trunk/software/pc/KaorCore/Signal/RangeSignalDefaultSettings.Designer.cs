namespace KaorCore.Signal
{
	partial class RangeSignalDefaultSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeSignalDefaultSettings));
			this.label1 = new System.Windows.Forms.Label();
			this.cmbSignalType = new System.Windows.Forms.ComboBox();
			this.chkFixDeltaPlus = new System.Windows.Forms.CheckBox();
			this.chkFixDeltaMinus = new System.Windows.Forms.CheckBox();
			this.chkDemodulate = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.numBandMultiplier = new System.Windows.Forms.NumericUpDown();
			this.numPauseTime = new System.Windows.Forms.NumericUpDown();
			this.btnRecordParams = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.chkIncreaseBand = new System.Windows.Forms.CheckBox();
			this.cmbRPU = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.numPmax = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.numPmin = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numBandMultiplier)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPauseTime)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPmax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numPmin)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cmbSignalType
			// 
			this.cmbSignalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSignalType.FormattingEnabled = true;
			this.cmbSignalType.Items.AddRange(new object[] {
            resources.GetString("cmbSignalType.Items"),
            resources.GetString("cmbSignalType.Items1"),
            resources.GetString("cmbSignalType.Items2"),
            resources.GetString("cmbSignalType.Items3")});
			resources.ApplyResources(this.cmbSignalType, "cmbSignalType");
			this.cmbSignalType.Name = "cmbSignalType";
			// 
			// chkFixDeltaPlus
			// 
			resources.ApplyResources(this.chkFixDeltaPlus, "chkFixDeltaPlus");
			this.chkFixDeltaPlus.Name = "chkFixDeltaPlus";
			this.chkFixDeltaPlus.UseVisualStyleBackColor = true;
			// 
			// chkFixDeltaMinus
			// 
			resources.ApplyResources(this.chkFixDeltaMinus, "chkFixDeltaMinus");
			this.chkFixDeltaMinus.Name = "chkFixDeltaMinus";
			this.chkFixDeltaMinus.UseVisualStyleBackColor = true;
			// 
			// chkDemodulate
			// 
			resources.ApplyResources(this.chkDemodulate, "chkDemodulate");
			this.chkDemodulate.Name = "chkDemodulate";
			this.chkDemodulate.UseVisualStyleBackColor = true;
			this.chkDemodulate.CheckedChanged += new System.EventHandler(this.chkDemodulate_CheckedChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// numBandMultiplier
			// 
			resources.ApplyResources(this.numBandMultiplier, "numBandMultiplier");
			this.numBandMultiplier.Name = "numBandMultiplier";
			// 
			// numPauseTime
			// 
			resources.ApplyResources(this.numPauseTime, "numPauseTime");
			this.numPauseTime.Name = "numPauseTime";
			// 
			// btnRecordParams
			// 
			resources.ApplyResources(this.btnRecordParams, "btnRecordParams");
			this.btnRecordParams.Name = "btnRecordParams";
			this.btnRecordParams.UseVisualStyleBackColor = true;
			this.btnRecordParams.Click += new System.EventHandler(this.btnRecordParams_Click);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// chkIncreaseBand
			// 
			resources.ApplyResources(this.chkIncreaseBand, "chkIncreaseBand");
			this.chkIncreaseBand.Name = "chkIncreaseBand";
			this.chkIncreaseBand.UseVisualStyleBackColor = true;
			// 
			// cmbRPU
			// 
			this.cmbRPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRPU.FormattingEnabled = true;
			resources.ApplyResources(this.cmbRPU, "cmbRPU");
			this.cmbRPU.Name = "cmbRPU";
			this.cmbRPU.SelectedIndexChanged += new System.EventHandler(this.cmbRPU_SelectedIndexChanged);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkDemodulate);
			this.groupBox1.Controls.Add(this.cmbRPU);
			this.groupBox1.Controls.Add(this.numPauseTime);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.btnRecordParams);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.numPmax);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.numPmin);
			this.groupBox2.Controls.Add(this.label4);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// numPmax
			// 
			resources.ApplyResources(this.numPmax, "numPmax");
			this.numPmax.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numPmax.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numPmax.Name = "numPmax";
			this.numPmax.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// numPmin
			// 
			resources.ApplyResources(this.numPmin, "numPmin");
			this.numPmin.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.numPmin.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numPmin.Name = "numPmin";
			this.numPmin.Value = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			// 
			// RangeSignalDefaultSettings
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.chkIncreaseBand);
			this.Controls.Add(this.numBandMultiplier);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chkFixDeltaMinus);
			this.Controls.Add(this.chkFixDeltaPlus);
			this.Controls.Add(this.cmbSignalType);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RangeSignalDefaultSettings";
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.numBandMultiplier)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPauseTime)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPmax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numPmin)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbSignalType;
		private System.Windows.Forms.CheckBox chkFixDeltaPlus;
		private System.Windows.Forms.CheckBox chkFixDeltaMinus;
		private System.Windows.Forms.CheckBox chkDemodulate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numBandMultiplier;
		private System.Windows.Forms.NumericUpDown numPauseTime;
		private System.Windows.Forms.Button btnRecordParams;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkIncreaseBand;
		private System.Windows.Forms.ComboBox cmbRPU;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown numPmax;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numPmin;
	}
}