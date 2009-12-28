namespace KaorCore.Antenna
{
	partial class BaseAntennaSettingsDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseAntennaSettingsDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.cmbState = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.freqStop = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.freqStart = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtAlt = new System.Windows.Forms.TextBox();
			this.txtLon = new System.Windows.Forms.TextBox();
			this.txtLat = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.txtDNWidth = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// txtDescription
			// 
			resources.ApplyResources(this.txtDescription, "txtDescription");
			this.txtDescription.Name = "txtDescription";
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
			// cmbType
			// 
			this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1")});
			resources.ApplyResources(this.cmbType, "cmbType");
			this.cmbType.Name = "cmbType";
			this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// cmbState
			// 
			this.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbState.FormattingEnabled = true;
			this.cmbState.Items.AddRange(new object[] {
            resources.GetString("cmbState.Items"),
            resources.GetString("cmbState.Items1"),
            resources.GetString("cmbState.Items2")});
			resources.ApplyResources(this.cmbState, "cmbState");
			this.cmbState.Name = "cmbState";
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
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// freqStop
			// 
			this.freqStop.Frequency = ((long)(3000000000));
			resources.ApplyResources(this.freqStop, "freqStop");
			this.freqStop.Max = ((long)(3000000000));
			this.freqStop.Min = ((long)(0));
			this.freqStop.Name = "freqStop";
			// 
			// freqStart
			// 
			this.freqStart.Frequency = ((long)(3000000000));
			resources.ApplyResources(this.freqStart, "freqStart");
			this.freqStart.Max = ((long)(3000000000));
			this.freqStart.Min = ((long)(0));
			this.freqStart.Name = "freqStart";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtAlt);
			this.groupBox1.Controls.Add(this.txtLon);
			this.groupBox1.Controls.Add(this.txtLat);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label7);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// txtAlt
			// 
			resources.ApplyResources(this.txtAlt, "txtAlt");
			this.txtAlt.Name = "txtAlt";
			// 
			// txtLon
			// 
			resources.ApplyResources(this.txtLon, "txtLon");
			this.txtLon.Name = "txtLon";
			// 
			// txtLat
			// 
			resources.ApplyResources(this.txtLat, "txtLat");
			this.txtLat.Name = "txtLat";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.txtDNWidth);
			this.groupBox2.Controls.Add(this.txtDir);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.cmbType);
			this.groupBox2.Controls.Add(this.label8);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// txtDir
			// 
			resources.ApplyResources(this.txtDir, "txtDir");
			this.txtDir.Name = "txtDir";
			// 
			// txtDNWidth
			// 
			resources.ApplyResources(this.txtDNWidth, "txtDNWidth");
			this.txtDNWidth.Name = "txtDNWidth";
			// 
			// BaseAntennaSettingsDialog
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cmbState);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.freqStop);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.freqStart);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BaseAntennaSettingsDialog";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label3;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox freqStart;
		private System.Windows.Forms.Label label4;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox freqStop;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cmbType;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cmbState;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtAlt;
		private System.Windows.Forms.TextBox txtLon;
		private System.Windows.Forms.TextBox txtLat;
		private System.Windows.Forms.TextBox txtDNWidth;
		private System.Windows.Forms.TextBox txtDir;
	}
}