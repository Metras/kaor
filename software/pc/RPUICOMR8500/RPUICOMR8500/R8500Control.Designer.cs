namespace RPUICOMR8500
{
    partial class R8500Control
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(R8500Control));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pnlAudioControl = new System.Windows.Forms.Panel();
			this.cmb_average = new System.Windows.Forms.ComboBox();
			this._mem_button3 = new System.Windows.Forms.Button();
			this.chk_memory = new System.Windows.Forms.CheckBox();
			this.mem_button2 = new System.Windows.Forms.Button();
			this.mem_button1 = new System.Windows.Forms.Button();
			this.cmbMod = new System.Windows.Forms.ComboBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.frequencyRadio1 = new ControlUtils.FrequencyRadio.FrequencyRadio();
			this.chkAtt10 = new System.Windows.Forms.CheckBox();
			this.chkAtt20 = new System.Windows.Forms.CheckBox();
			this.grpAntennas = new System.Windows.Forms.GroupBox();
			this.lstAntennas = new ControlUtils.ObjectListView.ObjectListView();
			this.olvTitle = new ControlUtils.ObjectListView.OLVColumn();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lblPower = new System.Windows.Forms.Label();
			this.graphPowerScale1 = new ControlUtils.GraphPowerScale.GraphPowerScale();
			this.groupBox1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.grpAntennas.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lstAntennas)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = null;
			this.groupBox1.AccessibleName = null;
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackgroundImage = null;
			this.groupBox1.Controls.Add(this.pnlAudioControl);
			this.groupBox1.Controls.Add(this.cmb_average);
			this.groupBox1.Controls.Add(this._mem_button3);
			this.groupBox1.Controls.Add(this.chk_memory);
			this.groupBox1.Controls.Add(this.mem_button2);
			this.groupBox1.Controls.Add(this.mem_button1);
			this.groupBox1.Controls.Add(this.cmbMod);
			this.groupBox1.Font = null;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
			// 
			// pnlAudioControl
			// 
			this.pnlAudioControl.AccessibleDescription = null;
			this.pnlAudioControl.AccessibleName = null;
			resources.ApplyResources(this.pnlAudioControl, "pnlAudioControl");
			this.pnlAudioControl.BackgroundImage = null;
			this.pnlAudioControl.Font = null;
			this.pnlAudioControl.Name = "pnlAudioControl";
			this.toolTip1.SetToolTip(this.pnlAudioControl, resources.GetString("pnlAudioControl.ToolTip"));
			// 
			// cmb_average
			// 
			this.cmb_average.AccessibleDescription = null;
			this.cmb_average.AccessibleName = null;
			resources.ApplyResources(this.cmb_average, "cmb_average");
			this.cmb_average.BackgroundImage = null;
			this.cmb_average.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmb_average.Font = null;
			this.cmb_average.FormatString = "N0";
			this.cmb_average.FormattingEnabled = true;
			this.cmb_average.Items.AddRange(new object[] {
            resources.GetString("cmb_average.Items"),
            resources.GetString("cmb_average.Items1"),
            resources.GetString("cmb_average.Items2"),
            resources.GetString("cmb_average.Items3")});
			this.cmb_average.Name = "cmb_average";
			this.toolTip1.SetToolTip(this.cmb_average, resources.GetString("cmb_average.ToolTip"));
			this.cmb_average.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
			// 
			// _mem_button3
			// 
			this._mem_button3.AccessibleDescription = null;
			this._mem_button3.AccessibleName = null;
			resources.ApplyResources(this._mem_button3, "_mem_button3");
			this._mem_button3.BackgroundImage = null;
			this._mem_button3.Font = null;
			this._mem_button3.Name = "_mem_button3";
			this.toolTip1.SetToolTip(this._mem_button3, resources.GetString("_mem_button3.ToolTip"));
			this._mem_button3.UseVisualStyleBackColor = true;
			this._mem_button3.Click += new System.EventHandler(this.mem_button3_Click);
			// 
			// chk_memory
			// 
			this.chk_memory.AccessibleDescription = null;
			this.chk_memory.AccessibleName = null;
			resources.ApplyResources(this.chk_memory, "chk_memory");
			this.chk_memory.BackgroundImage = null;
			this.chk_memory.Font = null;
			this.chk_memory.Name = "chk_memory";
			this.toolTip1.SetToolTip(this.chk_memory, resources.GetString("chk_memory.ToolTip"));
			this.chk_memory.UseVisualStyleBackColor = true;
			// 
			// mem_button2
			// 
			this.mem_button2.AccessibleDescription = null;
			this.mem_button2.AccessibleName = null;
			resources.ApplyResources(this.mem_button2, "mem_button2");
			this.mem_button2.BackgroundImage = null;
			this.mem_button2.Font = null;
			this.mem_button2.Name = "mem_button2";
			this.toolTip1.SetToolTip(this.mem_button2, resources.GetString("mem_button2.ToolTip"));
			this.mem_button2.UseVisualStyleBackColor = true;
			this.mem_button2.Click += new System.EventHandler(this.mem_button2_Click);
			// 
			// mem_button1
			// 
			this.mem_button1.AccessibleDescription = null;
			this.mem_button1.AccessibleName = null;
			resources.ApplyResources(this.mem_button1, "mem_button1");
			this.mem_button1.BackgroundImage = null;
			this.mem_button1.Font = null;
			this.mem_button1.Name = "mem_button1";
			this.toolTip1.SetToolTip(this.mem_button1, resources.GetString("mem_button1.ToolTip"));
			this.mem_button1.UseVisualStyleBackColor = true;
			this.mem_button1.Click += new System.EventHandler(this.mem_button1_Click);
			// 
			// cmbMod
			// 
			this.cmbMod.AccessibleDescription = null;
			this.cmbMod.AccessibleName = null;
			resources.ApplyResources(this.cmbMod, "cmbMod");
			this.cmbMod.BackgroundImage = null;
			this.cmbMod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbMod.Font = null;
			this.cmbMod.FormattingEnabled = true;
			this.cmbMod.Items.AddRange(new object[] {
            resources.GetString("cmbMod.Items"),
            resources.GetString("cmbMod.Items1"),
            resources.GetString("cmbMod.Items2"),
            resources.GetString("cmbMod.Items3"),
            resources.GetString("cmbMod.Items4"),
            resources.GetString("cmbMod.Items5"),
            resources.GetString("cmbMod.Items6"),
            resources.GetString("cmbMod.Items7"),
            resources.GetString("cmbMod.Items8")});
			this.cmbMod.Name = "cmbMod";
			this.toolTip1.SetToolTip(this.cmbMod, resources.GetString("cmbMod.ToolTip"));
			this.cmbMod.SelectedIndexChanged += new System.EventHandler(this.ModulationComboBox_SelectedIndexChanged);
			// 
			// panel4
			// 
			this.panel4.AccessibleDescription = null;
			this.panel4.AccessibleName = null;
			resources.ApplyResources(this.panel4, "panel4");
			this.panel4.BackColor = System.Drawing.Color.Transparent;
			this.panel4.Controls.Add(this.frequencyRadio1);
			this.panel4.Font = null;
			this.panel4.Name = "panel4";
			this.toolTip1.SetToolTip(this.panel4, resources.GetString("panel4.ToolTip"));
			// 
			// frequencyRadio1
			// 
			this.frequencyRadio1.AccessibleDescription = null;
			this.frequencyRadio1.AccessibleName = null;
			resources.ApplyResources(this.frequencyRadio1, "frequencyRadio1");
			this.frequencyRadio1.BackColor = System.Drawing.Color.Gainsboro;
			this.frequencyRadio1.BackgroundImage = null;
			this.frequencyRadio1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.frequencyRadio1.DelayChange = 200;
			this.frequencyRadio1.ForeColor = System.Drawing.Color.RoyalBlue;
			this.frequencyRadio1.Frequency = ((long)(20000000));
			this.frequencyRadio1.Max = ((long)(2000000000));
			this.frequencyRadio1.Min = ((long)(100000));
			this.frequencyRadio1.Name = "frequencyRadio1";
			this.frequencyRadio1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			this.toolTip1.SetToolTip(this.frequencyRadio1, resources.GetString("frequencyRadio1.ToolTip"));
			this.frequencyRadio1.FrequencyChanged += new ControlUtils.FrequencyRadio.FrequencyRadio.FrequencyChangedHandler(this.frequencyRadio1_FrequencyChanged);
			// 
			// chkAtt10
			// 
			this.chkAtt10.AccessibleDescription = null;
			this.chkAtt10.AccessibleName = null;
			resources.ApplyResources(this.chkAtt10, "chkAtt10");
			this.chkAtt10.BackgroundImage = null;
			this.chkAtt10.Font = null;
			this.chkAtt10.Name = "chkAtt10";
			this.toolTip1.SetToolTip(this.chkAtt10, resources.GetString("chkAtt10.ToolTip"));
			this.chkAtt10.UseVisualStyleBackColor = true;
			this.chkAtt10.CheckedChanged += new System.EventHandler(this.chkAtt_CheckedChanged);
			// 
			// chkAtt20
			// 
			this.chkAtt20.AccessibleDescription = null;
			this.chkAtt20.AccessibleName = null;
			resources.ApplyResources(this.chkAtt20, "chkAtt20");
			this.chkAtt20.BackgroundImage = null;
			this.chkAtt20.Font = null;
			this.chkAtt20.Name = "chkAtt20";
			this.toolTip1.SetToolTip(this.chkAtt20, resources.GetString("chkAtt20.ToolTip"));
			this.chkAtt20.UseVisualStyleBackColor = true;
			this.chkAtt20.CheckedChanged += new System.EventHandler(this.chkAtt_CheckedChanged);
			// 
			// grpAntennas
			// 
			this.grpAntennas.AccessibleDescription = null;
			this.grpAntennas.AccessibleName = null;
			resources.ApplyResources(this.grpAntennas, "grpAntennas");
			this.grpAntennas.BackgroundImage = null;
			this.grpAntennas.Controls.Add(this.lstAntennas);
			this.grpAntennas.Font = null;
			this.grpAntennas.Name = "grpAntennas";
			this.grpAntennas.TabStop = false;
			this.toolTip1.SetToolTip(this.grpAntennas, resources.GetString("grpAntennas.ToolTip"));
			// 
			// lstAntennas
			// 
			this.lstAntennas.AccessibleDescription = null;
			this.lstAntennas.AccessibleName = null;
			resources.ApplyResources(this.lstAntennas, "lstAntennas");
			this.lstAntennas.AllColumns.Add(this.olvTitle);
			this.lstAntennas.AllColumns.Add(this.olvTitle);
			this.lstAntennas.AlternateRowBackColor = System.Drawing.Color.Empty;
			this.lstAntennas.BackgroundImage = null;
			this.lstAntennas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvTitle});
			this.lstAntennas.Font = null;
			this.lstAntennas.FullRowSelect = true;
			this.lstAntennas.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstAntennas.HideSelection = false;
			this.lstAntennas.MultiSelect = false;
			this.lstAntennas.Name = "lstAntennas";
			this.lstAntennas.OwnerDraw = true;
			this.lstAntennas.ShowGroups = false;
			this.toolTip1.SetToolTip(this.lstAntennas, resources.GetString("lstAntennas.ToolTip"));
			this.lstAntennas.UseCompatibleStateImageBehavior = false;
			this.lstAntennas.View = System.Windows.Forms.View.Details;
			// 
			// olvTitle
			// 
			this.olvTitle.AspectName = null;
			this.olvTitle.FillsFreeSpace = true;
			resources.ApplyResources(this.olvTitle, "olvTitle");
			// 
			// lblPower
			// 
			this.lblPower.AccessibleDescription = null;
			this.lblPower.AccessibleName = null;
			resources.ApplyResources(this.lblPower, "lblPower");
			this.lblPower.Name = "lblPower";
			this.toolTip1.SetToolTip(this.lblPower, resources.GetString("lblPower.ToolTip"));
			// 
			// graphPowerScale1
			// 
			this.graphPowerScale1.AccessibleDescription = null;
			this.graphPowerScale1.AccessibleName = null;
			resources.ApplyResources(this.graphPowerScale1, "graphPowerScale1");
			this.graphPowerScale1.BackColor = System.Drawing.SystemColors.Control;
			this.graphPowerScale1.Font = null;
			this.graphPowerScale1.Max = 0;
			this.graphPowerScale1.Min = -110;
			this.graphPowerScale1.Name = "graphPowerScale1";
			this.graphPowerScale1.Power = -110;
			this.toolTip1.SetToolTip(this.graphPowerScale1, resources.GetString("graphPowerScale1.ToolTip"));
			// 
			// R8500Control
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.lblPower);
			this.Controls.Add(this.graphPowerScale1);
			this.Controls.Add(this.grpAntennas);
			this.Controls.Add(this.chkAtt20);
			this.Controls.Add(this.chkAtt10);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.groupBox1);
			this.Font = null;
			this.Name = "R8500Control";
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.groupBox1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.grpAntennas.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lstAntennas)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        public System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbMod;
        private System.Windows.Forms.CheckBox chk_memory;
		private System.Windows.Forms.Button _mem_button3;
		private System.Windows.Forms.Button mem_button2;
        private System.Windows.Forms.Button mem_button1;
        private System.Windows.Forms.Panel panel4;
        public ControlUtils.FrequencyRadio.FrequencyRadio frequencyRadio1;
        private System.Windows.Forms.CheckBox chkAtt10;
        private System.Windows.Forms.CheckBox chkAtt20;
        public System.Windows.Forms.GroupBox grpAntennas;
        private ControlUtils.GraphPowerScale.GraphPowerScale graphPowerScale1;
		private System.Windows.Forms.ComboBox cmb_average;
		private System.Windows.Forms.Panel pnlAudioControl;
		private ControlUtils.ObjectListView.ObjectListView lstAntennas;
		private ControlUtils.ObjectListView.OLVColumn olvTitle;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label lblPower;

	}
}
