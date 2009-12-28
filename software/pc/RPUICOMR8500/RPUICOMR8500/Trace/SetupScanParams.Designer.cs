namespace RPUICOMR8500.Trace
{
	partial class SetupScanParams
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupScanParams));
			this.label1 = new System.Windows.Forms.Label();
			this.txtTraceName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblFstart = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblFstop = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblMeasureStep = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cmbFilterBand = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbAntennas = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtTraceName
			// 
			resources.ApplyResources(this.txtTraceName, "txtTraceName");
			this.txtTraceName.Name = "txtTraceName";
			this.txtTraceName.ReadOnly = true;
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblFstart
			// 
			resources.ApplyResources(this.lblFstart, "lblFstart");
			this.lblFstart.Name = "lblFstart";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// lblFstop
			// 
			resources.ApplyResources(this.lblFstop, "lblFstop");
			this.lblFstop.Name = "lblFstop";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// lblMeasureStep
			// 
			resources.ApplyResources(this.lblMeasureStep, "lblMeasureStep");
			this.lblMeasureStep.Name = "lblMeasureStep";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// cmbFilterBand
			// 
			this.cmbFilterBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFilterBand.FormattingEnabled = true;
			this.cmbFilterBand.Items.AddRange(new object[] {
            resources.GetString("cmbFilterBand.Items"),
            resources.GetString("cmbFilterBand.Items1"),
            resources.GetString("cmbFilterBand.Items2"),
            resources.GetString("cmbFilterBand.Items3")});
			resources.ApplyResources(this.cmbFilterBand, "cmbFilterBand");
			this.cmbFilterBand.Name = "cmbFilterBand";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtTraceName);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.lblMeasureStep);
			this.groupBox1.Controls.Add(this.lblFstart);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.lblFstop);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.cmbAntennas);
			this.groupBox2.Controls.Add(this.cmbFilterBand);
			this.groupBox2.Controls.Add(this.label8);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// cmbAntennas
			// 
			this.cmbAntennas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAntennas.FormattingEnabled = true;
			resources.ApplyResources(this.cmbAntennas, "cmbAntennas");
			this.cmbAntennas.Name = "cmbAntennas";
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
			// SetupScanParams
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetupScanParams";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTraceName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblFstart;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblFstop;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblMeasureStep;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cmbFilterBand;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbAntennas;
	}
}