namespace RPURPV18
{
	partial class RPURPV18Control
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPURPV18Control));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pnlAudioControl = new System.Windows.Forms.Panel();
			this.btnDemodM3 = new System.Windows.Forms.Button();
			this.chkMemDemod = new System.Windows.Forms.CheckBox();
			this.btnDemodM2 = new System.Windows.Forms.Button();
			this.cmbBand = new System.Windows.Forms.ComboBox();
			this.cmbMod = new System.Windows.Forms.ComboBox();
			this.btnDemodM1 = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnPowerMeterM3 = new System.Windows.Forms.Button();
			this.chkMemPowermeter = new System.Windows.Forms.CheckBox();
			this.cmbAverageTime = new System.Windows.Forms.ComboBox();
			this.btnPowerMeterM2 = new System.Windows.Forms.Button();
			this.cmbPowerFilterBand = new System.Windows.Forms.ComboBox();
			this.btnPowerMeterM1 = new System.Windows.Forms.Button();
			this.panel4 = new System.Windows.Forms.Panel();
			this.frequencyRadio1 = new ControlUtils.FrequencyRadio.FrequencyRadio();
			this.grpAntennas = new System.Windows.Forms.GroupBox();
			this.lstAntennas = new ControlUtils.ObjectListView.ObjectListView();
			this.olvName = new ControlUtils.ObjectListView.OLVColumn();
			this.chkATT20dB = new System.Windows.Forms.CheckBox();
			this.chkATT10dB = new System.Windows.Forms.CheckBox();
			this.lblPower = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmbSignalConverter = new System.Windows.Forms.ComboBox();
			this.graphPowerScale1 = new ControlUtils.GraphPowerScale.GraphPowerScale();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.panel4.SuspendLayout();
			this.grpAntennas.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lstAntennas)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = null;
			this.groupBox1.AccessibleName = null;
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackgroundImage = null;
			this.groupBox1.Controls.Add(this.pnlAudioControl);
			this.groupBox1.Controls.Add(this.btnDemodM3);
			this.groupBox1.Controls.Add(this.chkMemDemod);
			this.groupBox1.Controls.Add(this.btnDemodM2);
			this.groupBox1.Controls.Add(this.cmbBand);
			this.groupBox1.Controls.Add(this.cmbMod);
			this.groupBox1.Controls.Add(this.btnDemodM1);
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
			// btnDemodM3
			// 
			this.btnDemodM3.AccessibleDescription = null;
			this.btnDemodM3.AccessibleName = null;
			resources.ApplyResources(this.btnDemodM3, "btnDemodM3");
			this.btnDemodM3.BackgroundImage = null;
			this.btnDemodM3.Font = null;
			this.btnDemodM3.Name = "btnDemodM3";
			this.toolTip1.SetToolTip(this.btnDemodM3, resources.GetString("btnDemodM3.ToolTip"));
			this.btnDemodM3.UseVisualStyleBackColor = true;
			this.btnDemodM3.Click += new System.EventHandler(this.btnMemDemod_Click);
			// 
			// chkMemDemod
			// 
			this.chkMemDemod.AccessibleDescription = null;
			this.chkMemDemod.AccessibleName = null;
			resources.ApplyResources(this.chkMemDemod, "chkMemDemod");
			this.chkMemDemod.BackgroundImage = null;
			this.chkMemDemod.Font = null;
			this.chkMemDemod.Name = "chkMemDemod";
			this.toolTip1.SetToolTip(this.chkMemDemod, resources.GetString("chkMemDemod.ToolTip"));
			this.chkMemDemod.UseVisualStyleBackColor = true;
			// 
			// btnDemodM2
			// 
			this.btnDemodM2.AccessibleDescription = null;
			this.btnDemodM2.AccessibleName = null;
			resources.ApplyResources(this.btnDemodM2, "btnDemodM2");
			this.btnDemodM2.BackgroundImage = null;
			this.btnDemodM2.Font = null;
			this.btnDemodM2.Name = "btnDemodM2";
			this.toolTip1.SetToolTip(this.btnDemodM2, resources.GetString("btnDemodM2.ToolTip"));
			this.btnDemodM2.UseVisualStyleBackColor = true;
			this.btnDemodM2.Click += new System.EventHandler(this.btnMemDemod_Click);
			// 
			// cmbBand
			// 
			this.cmbBand.AccessibleDescription = null;
			this.cmbBand.AccessibleName = null;
			resources.ApplyResources(this.cmbBand, "cmbBand");
			this.cmbBand.BackgroundImage = null;
			this.cmbBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBand.Font = null;
			this.cmbBand.FormattingEnabled = true;
			this.cmbBand.Items.AddRange(new object[] {
            resources.GetString("cmbBand.Items"),
            resources.GetString("cmbBand.Items1"),
            resources.GetString("cmbBand.Items2"),
            resources.GetString("cmbBand.Items3"),
            resources.GetString("cmbBand.Items4")});
			this.cmbBand.Name = "cmbBand";
			this.toolTip1.SetToolTip(this.cmbBand, resources.GetString("cmbBand.ToolTip"));
			this.cmbBand.SelectedIndexChanged += new System.EventHandler(this.cmbBand_SelectedIndexChanged);
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
            resources.GetString("cmbMod.Items4")});
			this.cmbMod.Name = "cmbMod";
			this.toolTip1.SetToolTip(this.cmbMod, resources.GetString("cmbMod.ToolTip"));
			this.cmbMod.SelectedIndexChanged += new System.EventHandler(this.cmbMod_SelectedIndexChanged);
			// 
			// btnDemodM1
			// 
			this.btnDemodM1.AccessibleDescription = null;
			this.btnDemodM1.AccessibleName = null;
			resources.ApplyResources(this.btnDemodM1, "btnDemodM1");
			this.btnDemodM1.BackgroundImage = null;
			this.btnDemodM1.Font = null;
			this.btnDemodM1.Name = "btnDemodM1";
			this.toolTip1.SetToolTip(this.btnDemodM1, resources.GetString("btnDemodM1.ToolTip"));
			this.btnDemodM1.UseVisualStyleBackColor = true;
			this.btnDemodM1.Click += new System.EventHandler(this.btnMemDemod_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.AccessibleDescription = null;
			this.groupBox4.AccessibleName = null;
			resources.ApplyResources(this.groupBox4, "groupBox4");
			this.groupBox4.BackgroundImage = null;
			this.groupBox4.Controls.Add(this.btnPowerMeterM3);
			this.groupBox4.Controls.Add(this.chkMemPowermeter);
			this.groupBox4.Controls.Add(this.cmbAverageTime);
			this.groupBox4.Controls.Add(this.btnPowerMeterM2);
			this.groupBox4.Controls.Add(this.cmbPowerFilterBand);
			this.groupBox4.Controls.Add(this.btnPowerMeterM1);
			this.groupBox4.Font = null;
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
			// 
			// btnPowerMeterM3
			// 
			this.btnPowerMeterM3.AccessibleDescription = null;
			this.btnPowerMeterM3.AccessibleName = null;
			resources.ApplyResources(this.btnPowerMeterM3, "btnPowerMeterM3");
			this.btnPowerMeterM3.BackgroundImage = null;
			this.btnPowerMeterM3.Font = null;
			this.btnPowerMeterM3.Name = "btnPowerMeterM3";
			this.toolTip1.SetToolTip(this.btnPowerMeterM3, resources.GetString("btnPowerMeterM3.ToolTip"));
			this.btnPowerMeterM3.UseVisualStyleBackColor = true;
			this.btnPowerMeterM3.Click += new System.EventHandler(this.btnPowerMeterMem_Click);
			// 
			// chkMemPowermeter
			// 
			this.chkMemPowermeter.AccessibleDescription = null;
			this.chkMemPowermeter.AccessibleName = null;
			resources.ApplyResources(this.chkMemPowermeter, "chkMemPowermeter");
			this.chkMemPowermeter.BackgroundImage = null;
			this.chkMemPowermeter.Font = null;
			this.chkMemPowermeter.Name = "chkMemPowermeter";
			this.toolTip1.SetToolTip(this.chkMemPowermeter, resources.GetString("chkMemPowermeter.ToolTip"));
			this.chkMemPowermeter.UseVisualStyleBackColor = true;
			// 
			// cmbAverageTime
			// 
			this.cmbAverageTime.AccessibleDescription = null;
			this.cmbAverageTime.AccessibleName = null;
			resources.ApplyResources(this.cmbAverageTime, "cmbAverageTime");
			this.cmbAverageTime.BackgroundImage = null;
			this.cmbAverageTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAverageTime.Font = null;
			this.cmbAverageTime.FormattingEnabled = true;
			this.cmbAverageTime.Items.AddRange(new object[] {
            resources.GetString("cmbAverageTime.Items"),
            resources.GetString("cmbAverageTime.Items1"),
            resources.GetString("cmbAverageTime.Items2"),
            resources.GetString("cmbAverageTime.Items3"),
            resources.GetString("cmbAverageTime.Items4"),
            resources.GetString("cmbAverageTime.Items5"),
            resources.GetString("cmbAverageTime.Items6")});
			this.cmbAverageTime.Name = "cmbAverageTime";
			this.toolTip1.SetToolTip(this.cmbAverageTime, resources.GetString("cmbAverageTime.ToolTip"));
			this.cmbAverageTime.SelectedIndexChanged += new System.EventHandler(this.cmbAverageTime_SelectedIndexChanged);
			// 
			// btnPowerMeterM2
			// 
			this.btnPowerMeterM2.AccessibleDescription = null;
			this.btnPowerMeterM2.AccessibleName = null;
			resources.ApplyResources(this.btnPowerMeterM2, "btnPowerMeterM2");
			this.btnPowerMeterM2.BackgroundImage = null;
			this.btnPowerMeterM2.Font = null;
			this.btnPowerMeterM2.Name = "btnPowerMeterM2";
			this.toolTip1.SetToolTip(this.btnPowerMeterM2, resources.GetString("btnPowerMeterM2.ToolTip"));
			this.btnPowerMeterM2.UseVisualStyleBackColor = true;
			this.btnPowerMeterM2.Click += new System.EventHandler(this.btnPowerMeterMem_Click);
			// 
			// cmbPowerFilterBand
			// 
			this.cmbPowerFilterBand.AccessibleDescription = null;
			this.cmbPowerFilterBand.AccessibleName = null;
			resources.ApplyResources(this.cmbPowerFilterBand, "cmbPowerFilterBand");
			this.cmbPowerFilterBand.BackgroundImage = null;
			this.cmbPowerFilterBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPowerFilterBand.Font = null;
			this.cmbPowerFilterBand.FormattingEnabled = true;
			this.cmbPowerFilterBand.Items.AddRange(new object[] {
            resources.GetString("cmbPowerFilterBand.Items"),
            resources.GetString("cmbPowerFilterBand.Items1"),
            resources.GetString("cmbPowerFilterBand.Items2"),
            resources.GetString("cmbPowerFilterBand.Items3"),
            resources.GetString("cmbPowerFilterBand.Items4"),
            resources.GetString("cmbPowerFilterBand.Items5"),
            resources.GetString("cmbPowerFilterBand.Items6"),
            resources.GetString("cmbPowerFilterBand.Items7")});
			this.cmbPowerFilterBand.Name = "cmbPowerFilterBand";
			this.toolTip1.SetToolTip(this.cmbPowerFilterBand, resources.GetString("cmbPowerFilterBand.ToolTip"));
			this.cmbPowerFilterBand.SelectedIndexChanged += new System.EventHandler(this.cmbPowerFilterBand_SelectedIndexChanged);
			// 
			// btnPowerMeterM1
			// 
			this.btnPowerMeterM1.AccessibleDescription = null;
			this.btnPowerMeterM1.AccessibleName = null;
			resources.ApplyResources(this.btnPowerMeterM1, "btnPowerMeterM1");
			this.btnPowerMeterM1.BackgroundImage = null;
			this.btnPowerMeterM1.Font = null;
			this.btnPowerMeterM1.Name = "btnPowerMeterM1";
			this.toolTip1.SetToolTip(this.btnPowerMeterM1, resources.GetString("btnPowerMeterM1.ToolTip"));
			this.btnPowerMeterM1.UseVisualStyleBackColor = true;
			this.btnPowerMeterM1.Click += new System.EventHandler(this.btnPowerMeterMem_Click);
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
			this.frequencyRadio1.Max = ((long)(18000000000));
			this.frequencyRadio1.Min = ((long)(20000000));
			this.frequencyRadio1.Name = "frequencyRadio1";
			this.frequencyRadio1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			this.toolTip1.SetToolTip(this.frequencyRadio1, resources.GetString("frequencyRadio1.ToolTip"));
			this.frequencyRadio1.FrequencyChanged += new ControlUtils.FrequencyRadio.FrequencyRadio.FrequencyChangedHandler(this.frequencyRadio1_FrequencyChanged);
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
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AllColumns.Add(this.olvName);
			this.lstAntennas.AlternateRowBackColor = System.Drawing.Color.Empty;
			this.lstAntennas.BackgroundImage = null;
			this.lstAntennas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvName});
			this.lstAntennas.Font = null;
			this.lstAntennas.FullRowSelect = true;
			this.lstAntennas.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstAntennas.HideSelection = false;
			this.lstAntennas.Name = "lstAntennas";
			this.lstAntennas.OwnerDraw = true;
			this.lstAntennas.ShowGroups = false;
			this.toolTip1.SetToolTip(this.lstAntennas, resources.GetString("lstAntennas.ToolTip"));
			this.lstAntennas.UseCompatibleStateImageBehavior = false;
			this.lstAntennas.View = System.Windows.Forms.View.Details;
			// 
			// olvName
			// 
			this.olvName.AspectName = null;
			this.olvName.FillsFreeSpace = true;
			resources.ApplyResources(this.olvName, "olvName");
			// 
			// chkATT20dB
			// 
			this.chkATT20dB.AccessibleDescription = null;
			this.chkATT20dB.AccessibleName = null;
			resources.ApplyResources(this.chkATT20dB, "chkATT20dB");
			this.chkATT20dB.BackgroundImage = null;
			this.chkATT20dB.Font = null;
			this.chkATT20dB.Name = "chkATT20dB";
			this.toolTip1.SetToolTip(this.chkATT20dB, resources.GetString("chkATT20dB.ToolTip"));
			this.chkATT20dB.UseVisualStyleBackColor = true;
			this.chkATT20dB.CheckedChanged += new System.EventHandler(this.chkATT_CheckedChanged);
			// 
			// chkATT10dB
			// 
			this.chkATT10dB.AccessibleDescription = null;
			this.chkATT10dB.AccessibleName = null;
			resources.ApplyResources(this.chkATT10dB, "chkATT10dB");
			this.chkATT10dB.BackgroundImage = null;
			this.chkATT10dB.Font = null;
			this.chkATT10dB.Name = "chkATT10dB";
			this.toolTip1.SetToolTip(this.chkATT10dB, resources.GetString("chkATT10dB.ToolTip"));
			this.chkATT10dB.UseVisualStyleBackColor = true;
			this.chkATT10dB.CheckedChanged += new System.EventHandler(this.chkATT_CheckedChanged);
			// 
			// lblPower
			// 
			this.lblPower.AccessibleDescription = null;
			this.lblPower.AccessibleName = null;
			resources.ApplyResources(this.lblPower, "lblPower");
			this.lblPower.Name = "lblPower";
			this.toolTip1.SetToolTip(this.lblPower, resources.GetString("lblPower.ToolTip"));
			// 
			// groupBox2
			// 
			this.groupBox2.AccessibleDescription = null;
			this.groupBox2.AccessibleName = null;
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackgroundImage = null;
			this.groupBox2.Controls.Add(this.cmbSignalConverter);
			this.groupBox2.Font = null;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
			// 
			// cmbSignalConverter
			// 
			this.cmbSignalConverter.AccessibleDescription = null;
			this.cmbSignalConverter.AccessibleName = null;
			resources.ApplyResources(this.cmbSignalConverter, "cmbSignalConverter");
			this.cmbSignalConverter.BackgroundImage = null;
			this.cmbSignalConverter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSignalConverter.Font = null;
			this.cmbSignalConverter.FormattingEnabled = true;
			this.cmbSignalConverter.Name = "cmbSignalConverter";
			this.toolTip1.SetToolTip(this.cmbSignalConverter, resources.GetString("cmbSignalConverter.ToolTip"));
			this.cmbSignalConverter.SelectedIndexChanged += new System.EventHandler(this.cmbSignalConverter_SelectedIndexChanged);
			// 
			// graphPowerScale1
			// 
			this.graphPowerScale1.AccessibleDescription = null;
			this.graphPowerScale1.AccessibleName = null;
			resources.ApplyResources(this.graphPowerScale1, "graphPowerScale1");
			this.graphPowerScale1.BackColor = System.Drawing.SystemColors.Control;
			this.graphPowerScale1.Font = null;
			this.graphPowerScale1.Max = -20;
			this.graphPowerScale1.Min = -100;
			this.graphPowerScale1.Name = "graphPowerScale1";
			this.graphPowerScale1.Power = -120;
			this.toolTip1.SetToolTip(this.graphPowerScale1, resources.GetString("graphPowerScale1.ToolTip"));
			// 
			// RPURPV18Control
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lblPower);
			this.Controls.Add(this.graphPowerScale1);
			this.Controls.Add(this.chkATT10dB);
			this.Controls.Add(this.chkATT20dB);
			this.Controls.Add(this.grpAntennas);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox1);
			this.Font = null;
			this.Name = "RPURPV18Control";
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.groupBox1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.grpAntennas.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lstAntennas)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbBand;
		private System.Windows.Forms.ComboBox cmbMod;
		private System.Windows.Forms.Button btnDemodM3;
		private System.Windows.Forms.Button btnDemodM2;
		private System.Windows.Forms.Button btnDemodM1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnPowerMeterM3;
		private System.Windows.Forms.Button btnPowerMeterM2;
		private System.Windows.Forms.Button btnPowerMeterM1;
		private System.Windows.Forms.ComboBox cmbAverageTime;
		private System.Windows.Forms.ComboBox cmbPowerFilterBand;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.GroupBox grpAntennas;
		public ControlUtils.FrequencyRadio.FrequencyRadio frequencyRadio1;
		private System.Windows.Forms.CheckBox chkMemDemod;
		private System.Windows.Forms.CheckBox chkMemPowermeter;
		private System.Windows.Forms.CheckBox chkATT20dB;
		private System.Windows.Forms.CheckBox chkATT10dB;
		private ControlUtils.GraphPowerScale.GraphPowerScale graphPowerScale1;
		private System.Windows.Forms.Label lblPower;
		private System.Windows.Forms.Panel pnlAudioControl;
		private ControlUtils.ObjectListView.ObjectListView lstAntennas;
		private ControlUtils.ObjectListView.OLVColumn olvName;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox cmbSignalConverter;

	}
}
