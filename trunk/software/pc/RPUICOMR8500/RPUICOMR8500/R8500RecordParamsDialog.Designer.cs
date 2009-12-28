namespace RPUICOMR8500
{
	partial class R8500RecordParamsDialog
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(R8500RecordParamsDialog));
			this.cmbDemod = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkSaveRecords = new System.Windows.Forms.CheckBox();
			this.cmbAntenna = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rPV3RecordSignalParamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.rPV3RecordSignalParamsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbDemod
			// 
			this.cmbDemod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDemod.FormattingEnabled = true;
			this.cmbDemod.Items.AddRange(new object[] {
            resources.GetString("cmbDemod.Items"),
            resources.GetString("cmbDemod.Items1"),
            resources.GetString("cmbDemod.Items2"),
            resources.GetString("cmbDemod.Items3"),
            resources.GetString("cmbDemod.Items4"),
            resources.GetString("cmbDemod.Items5"),
            resources.GetString("cmbDemod.Items6"),
            resources.GetString("cmbDemod.Items7"),
            resources.GetString("cmbDemod.Items8")});
			resources.ApplyResources(this.cmbDemod, "cmbDemod");
			this.cmbDemod.Name = "cmbDemod";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
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
			// chkSaveRecords
			// 
			resources.ApplyResources(this.chkSaveRecords, "chkSaveRecords");
			this.chkSaveRecords.Name = "chkSaveRecords";
			this.chkSaveRecords.UseVisualStyleBackColor = true;
			// 
			// cmbAntenna
			// 
			this.cmbAntenna.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAntenna.FormattingEnabled = true;
			resources.ApplyResources(this.cmbAntenna, "cmbAntenna");
			this.cmbAntenna.Name = "cmbAntenna";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// rPV3RecordSignalParamsBindingSource
			// 
			this.rPV3RecordSignalParamsBindingSource.DataSource = typeof(RPUICOMR8500.R8500RecordSignalParams);
			// 
			// R8500RecordParamsDialog
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbAntenna);
			this.Controls.Add(this.chkSaveRecords);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbDemod);
			this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rPV3RecordSignalParamsBindingSource, "RecordPath", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "\"\""));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "R8500RecordParamsDialog";
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.rPV3RecordSignalParamsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbDemod;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource rPV3RecordSignalParamsBindingSource;
		private System.Windows.Forms.CheckBox chkSaveRecords;
		private System.Windows.Forms.ComboBox cmbAntenna;
		private System.Windows.Forms.Label label2;
	}
}