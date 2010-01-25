namespace RPURPV18
{
	partial class RPV18RecordParamsDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPV18RecordParamsDialog));
			this.cmbDemod = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbFilter = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkSaveRecord = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cmbAntenna = new System.Windows.Forms.ComboBox();
			this.rPV3RecordSignalParamsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.rPV3RecordSignalParamsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbDemod
			// 
			this.cmbDemod.AccessibleDescription = null;
			this.cmbDemod.AccessibleName = null;
			resources.ApplyResources(this.cmbDemod, "cmbDemod");
			this.cmbDemod.BackgroundImage = null;
			this.cmbDemod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDemod.Font = null;
			this.cmbDemod.FormattingEnabled = true;
			this.cmbDemod.Items.AddRange(new object[] {
            resources.GetString("cmbDemod.Items"),
            resources.GetString("cmbDemod.Items1"),
            resources.GetString("cmbDemod.Items2"),
            resources.GetString("cmbDemod.Items3"),
            resources.GetString("cmbDemod.Items4")});
			this.cmbDemod.Name = "cmbDemod";
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
			// 
			// cmbFilter
			// 
			this.cmbFilter.AccessibleDescription = null;
			this.cmbFilter.AccessibleName = null;
			resources.ApplyResources(this.cmbFilter, "cmbFilter");
			this.cmbFilter.BackgroundImage = null;
			this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbFilter.Font = null;
			this.cmbFilter.FormattingEnabled = true;
			this.cmbFilter.Items.AddRange(new object[] {
            resources.GetString("cmbFilter.Items"),
            resources.GetString("cmbFilter.Items1"),
            resources.GetString("cmbFilter.Items2"),
            resources.GetString("cmbFilter.Items3"),
            resources.GetString("cmbFilter.Items4")});
			this.cmbFilter.Name = "cmbFilter";
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			// 
			// btnOk
			// 
			this.btnOk.AccessibleDescription = null;
			this.btnOk.AccessibleName = null;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackgroundImage = null;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Font = null;
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleDescription = null;
			this.btnCancel.AccessibleName = null;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackgroundImage = null;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = null;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// chkSaveRecord
			// 
			this.chkSaveRecord.AccessibleDescription = null;
			this.chkSaveRecord.AccessibleName = null;
			resources.ApplyResources(this.chkSaveRecord, "chkSaveRecord");
			this.chkSaveRecord.BackgroundImage = null;
			this.chkSaveRecord.Font = null;
			this.chkSaveRecord.Name = "chkSaveRecord";
			this.chkSaveRecord.UseVisualStyleBackColor = true;
			this.chkSaveRecord.CheckedChanged += new System.EventHandler(this.chkSaveRecord_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AccessibleDescription = null;
			this.label5.AccessibleName = null;
			resources.ApplyResources(this.label5, "label5");
			this.label5.Font = null;
			this.label5.Name = "label5";
			// 
			// cmbAntenna
			// 
			this.cmbAntenna.AccessibleDescription = null;
			this.cmbAntenna.AccessibleName = null;
			resources.ApplyResources(this.cmbAntenna, "cmbAntenna");
			this.cmbAntenna.BackgroundImage = null;
			this.cmbAntenna.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAntenna.Font = null;
			this.cmbAntenna.FormattingEnabled = true;
			this.cmbAntenna.Name = "cmbAntenna";
			// 
			// rPV3RecordSignalParamsBindingSource
			// 
			this.rPV3RecordSignalParamsBindingSource.DataSource = typeof(RPURPV18.RPV18RecordSignalParams);
			// 
			// RPV18RecordParamsDialog
			// 
			this.AcceptButton = this.btnOk;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.cmbAntenna);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.chkSaveRecord);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbFilter);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbDemod);
			this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.rPV3RecordSignalParamsBindingSource, "RecordPath", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "\"\""));
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RPV18RecordParamsDialog";
			((System.ComponentModel.ISupportInitialize)(this.rPV3RecordSignalParamsBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbDemod;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbFilter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource rPV3RecordSignalParamsBindingSource;
		private System.Windows.Forms.CheckBox chkSaveRecord;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbAntenna;
	}
}