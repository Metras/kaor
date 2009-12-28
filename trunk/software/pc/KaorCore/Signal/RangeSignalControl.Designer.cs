namespace KaorCore.Signal
{
	partial class RangeSignalControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeSignalControl));
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.bndSignal = new System.Windows.Forms.BindingSource(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.cmbSignalType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.btnClearHits = new System.Windows.Forms.Button();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.chkRecord = new System.Windows.Forms.CheckBox();
			this.lstRPU = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.btnRecordParams = new System.Windows.Forms.Button();
			this.chkIncRange = new System.Windows.Forms.CheckBox();
			this.numDemodPause = new System.Windows.Forms.NumericUpDown();
			this.olvRecordTime = new ControlUtils.ObjectListView.OLVColumn();
			this.olvRecordName = new ControlUtils.ObjectListView.OLVColumn();
			this.lstRecords = new ControlUtils.ObjectListView.ObjectListView();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numDemodPause)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lstRecords)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// textBox1
			// 
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Name", true));
			this.textBox1.Name = "textBox1";
			// 
			// bndSignal
			// 
			this.bndSignal.DataSource = typeof(KaorCore.Signal.RangeSignal);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// textBox3
			// 
			resources.ApplyResources(this.textBox3, "textBox3");
			this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Description", true));
			this.textBox3.Name = "textBox3";
			// 
			// cmbSignalType
			// 
			resources.ApplyResources(this.cmbSignalType, "cmbSignalType");
			this.cmbSignalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSignalType.FormattingEnabled = true;
			this.cmbSignalType.Items.AddRange(new object[] {
            resources.GetString("cmbSignalType.Items"),
            resources.GetString("cmbSignalType.Items1"),
            resources.GetString("cmbSignalType.Items2"),
            resources.GetString("cmbSignalType.Items3")});
			this.cmbSignalType.Name = "cmbSignalType";
			this.cmbSignalType.SelectedIndexChanged += new System.EventHandler(this.cmbSignalType_SelectedIndexChanged);
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
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// numericUpDown1
			// 
			resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
			this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Pmin", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "-140"));
			this.numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			// 
			// numericUpDown2
			// 
			resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
			this.numericUpDown2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Pmax", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "0"));
			this.numericUpDown2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numericUpDown2.Name = "numericUpDown2";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// textBox4
			// 
			resources.ApplyResources(this.textBox4, "textBox4");
			this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "HitsCount", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "0"));
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			// 
			// btnClearHits
			// 
			resources.ApplyResources(this.btnClearHits, "btnClearHits");
			this.btnClearHits.Name = "btnClearHits";
			this.btnClearHits.UseVisualStyleBackColor = true;
			this.btnClearHits.Click += new System.EventHandler(this.btnClearHits_Click);
			// 
			// numericUpDown3
			// 
			resources.ApplyResources(this.numericUpDown3, "numericUpDown3");
			this.numericUpDown3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Frequency", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "0"));
			this.numericUpDown3.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown3.Maximum = new decimal(new int[] {
            -1294967296,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			// 
			// numericUpDown4
			// 
			resources.ApplyResources(this.numericUpDown4, "numericUpDown4");
			this.numericUpDown4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Band", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "0"));
			this.numericUpDown4.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown4.Maximum = new decimal(new int[] {
            -1294967296,
            0,
            0,
            0});
			this.numericUpDown4.Name = "numericUpDown4";
			// 
			// checkBox1
			// 
			resources.ApplyResources(this.checkBox1, "checkBox1");
			this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSignal, "IsFixDeltaPlus", true));
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			resources.ApplyResources(this.checkBox2, "checkBox2");
			this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSignal, "IsFixDeltaMinus", true));
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// chkRecord
			// 
			resources.ApplyResources(this.chkRecord, "chkRecord");
			this.chkRecord.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSignal, "IsRecord", true));
			this.chkRecord.Name = "chkRecord";
			this.chkRecord.UseVisualStyleBackColor = true;
			this.chkRecord.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			// 
			// lstRPU
			// 
			resources.ApplyResources(this.lstRPU, "lstRPU");
			this.lstRPU.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bndSignal, "RecordRPU", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lstRPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.lstRPU.FormattingEnabled = true;
			this.lstRPU.Name = "lstRPU";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// btnRecordParams
			// 
			resources.ApplyResources(this.btnRecordParams, "btnRecordParams");
			this.btnRecordParams.Name = "btnRecordParams";
			this.btnRecordParams.UseVisualStyleBackColor = true;
			this.btnRecordParams.Click += new System.EventHandler(this.btnRecordParams_Click);
			// 
			// chkIncRange
			// 
			resources.ApplyResources(this.chkIncRange, "chkIncRange");
			this.chkIncRange.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bndSignal, "IncreaseRange", true));
			this.chkIncRange.Name = "chkIncRange";
			this.chkIncRange.UseVisualStyleBackColor = true;
			// 
			// numDemodPause
			// 
			this.numDemodPause.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "PauseTime", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "3", "N0"));
			resources.ApplyResources(this.numDemodPause, "numDemodPause");
			this.numDemodPause.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
			this.numDemodPause.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.numDemodPause.Name = "numDemodPause";
			this.numDemodPause.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// olvRecordTime
			// 
			this.olvRecordTime.AspectName = null;
			resources.ApplyResources(this.olvRecordTime, "olvRecordTime");
			// 
			// olvRecordName
			// 
			this.olvRecordName.AspectName = null;
			this.olvRecordName.FillsFreeSpace = true;
			this.olvRecordName.MaximumWidth = 80;
			this.olvRecordName.MinimumWidth = 80;
			resources.ApplyResources(this.olvRecordName, "olvRecordName");
			// 
			// lstRecords
			// 
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AllColumns.Add(this.olvRecordTime);
			this.lstRecords.AllColumns.Add(this.olvRecordName);
			this.lstRecords.AlternateRowBackColor = System.Drawing.Color.Empty;
			resources.ApplyResources(this.lstRecords, "lstRecords");
			this.lstRecords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvRecordTime,
            this.olvRecordName});
			this.lstRecords.FullRowSelect = true;
			this.lstRecords.MultiSelect = false;
			this.lstRecords.Name = "lstRecords";
			this.lstRecords.OwnerDraw = true;
			this.lstRecords.SelectColumnsMenuStaysOpen = false;
			this.lstRecords.SelectColumnsOnRightClick = false;
			this.lstRecords.UseCompatibleStateImageBehavior = false;
			this.lstRecords.View = System.Windows.Forms.View.Details;
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// RangeSignalControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.numDemodPause);
			this.Controls.Add(this.lstRecords);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.chkIncRange);
			this.Controls.Add(this.btnRecordParams);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.lstRPU);
			this.Controls.Add(this.chkRecord);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.numericUpDown4);
			this.Controls.Add(this.numericUpDown3);
			this.Controls.Add(this.btnClearHits);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.numericUpDown2);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbSignalType);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Name = "RangeSignalControl";
			this.VisibleChanged += new System.EventHandler(this.RangeSignalControl_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numDemodPause)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lstRecords)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.ComboBox cmbSignalType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Button btnClearHits;
		private System.Windows.Forms.BindingSource bndSignal;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.NumericUpDown numericUpDown4;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox chkRecord;
		private System.Windows.Forms.ComboBox lstRPU;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnRecordParams;
		private System.Windows.Forms.CheckBox chkIncRange;
		private System.Windows.Forms.NumericUpDown numDemodPause;
		private ControlUtils.ObjectListView.OLVColumn olvRecordTime;
		private ControlUtils.ObjectListView.OLVColumn olvRecordName;
		private ControlUtils.ObjectListView.ObjectListView lstRecords;
		private System.Windows.Forms.Label label10;
	}
}
