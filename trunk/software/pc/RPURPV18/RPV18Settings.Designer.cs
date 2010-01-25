namespace RPURPV18
{
	partial class RPV18Settings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPV18Settings));
			this.label1 = new System.Windows.Forms.Label();
			this.cmbPortName = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbPortSpeed = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkCom1Enable = new System.Windows.Forms.CheckBox();
			this.btnCom1Params = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbCom1 = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.chkCom2Enable = new System.Windows.Forms.CheckBox();
			this.btnCom2Params = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbCom2 = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnTestConnection = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnTestInjectorConnection = new System.Windows.Forms.Button();
			this.cmbInjPortName = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cmbPortName
			// 
			this.cmbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPortName.FormattingEnabled = true;
			resources.ApplyResources(this.cmbPortName, "cmbPortName");
			this.cmbPortName.Name = "cmbPortName";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// cmbPortSpeed
			// 
			this.cmbPortSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPortSpeed.FormattingEnabled = true;
			this.cmbPortSpeed.Items.AddRange(new object[] {
            resources.GetString("cmbPortSpeed.Items"),
            resources.GetString("cmbPortSpeed.Items1"),
            resources.GetString("cmbPortSpeed.Items2"),
            resources.GetString("cmbPortSpeed.Items3")});
			resources.ApplyResources(this.cmbPortSpeed, "cmbPortSpeed");
			this.cmbPortSpeed.Name = "cmbPortSpeed";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkCom1Enable);
			this.groupBox1.Controls.Add(this.btnCom1Params);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cmbCom1);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// chkCom1Enable
			// 
			resources.ApplyResources(this.chkCom1Enable, "chkCom1Enable");
			this.chkCom1Enable.Name = "chkCom1Enable";
			this.chkCom1Enable.UseVisualStyleBackColor = true;
			this.chkCom1Enable.CheckedChanged += new System.EventHandler(this.chkCom1Enable_CheckedChanged);
			// 
			// btnCom1Params
			// 
			resources.ApplyResources(this.btnCom1Params, "btnCom1Params");
			this.btnCom1Params.Name = "btnCom1Params";
			this.btnCom1Params.UseVisualStyleBackColor = true;
			this.btnCom1Params.Click += new System.EventHandler(this.btnCom1Params_Click);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// cmbCom1
			// 
			this.cmbCom1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cmbCom1, "cmbCom1");
			this.cmbCom1.FormattingEnabled = true;
			this.cmbCom1.Items.AddRange(new object[] {
            resources.GetString("cmbCom1.Items"),
            resources.GetString("cmbCom1.Items1")});
			this.cmbCom1.Name = "cmbCom1";
			this.cmbCom1.SelectedIndexChanged += new System.EventHandler(this.cmbCom1_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.chkCom2Enable);
			this.groupBox2.Controls.Add(this.btnCom2Params);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.cmbCom2);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// chkCom2Enable
			// 
			resources.ApplyResources(this.chkCom2Enable, "chkCom2Enable");
			this.chkCom2Enable.Name = "chkCom2Enable";
			this.chkCom2Enable.UseVisualStyleBackColor = true;
			this.chkCom2Enable.CheckedChanged += new System.EventHandler(this.chkCom2Enable_CheckedChanged);
			// 
			// btnCom2Params
			// 
			resources.ApplyResources(this.btnCom2Params, "btnCom2Params");
			this.btnCom2Params.Name = "btnCom2Params";
			this.btnCom2Params.UseVisualStyleBackColor = true;
			this.btnCom2Params.Click += new System.EventHandler(this.btnCom2Params_Click);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// cmbCom2
			// 
			this.cmbCom2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.cmbCom2, "cmbCom2");
			this.cmbCom2.FormattingEnabled = true;
			this.cmbCom2.Items.AddRange(new object[] {
            resources.GetString("cmbCom2.Items"),
            resources.GetString("cmbCom2.Items1")});
			this.cmbCom2.Name = "cmbCom2";
			this.cmbCom2.SelectedIndexChanged += new System.EventHandler(this.cmbCom2_SelectedIndexChanged);
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
			// btnTestConnection
			// 
			resources.ApplyResources(this.btnTestConnection, "btnTestConnection");
			this.btnTestConnection.Name = "btnTestConnection";
			this.btnTestConnection.UseVisualStyleBackColor = true;
			this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnTestInjectorConnection);
			this.groupBox3.Controls.Add(this.cmbInjPortName);
			this.groupBox3.Controls.Add(this.label6);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// btnTestInjectorConnection
			// 
			resources.ApplyResources(this.btnTestInjectorConnection, "btnTestInjectorConnection");
			this.btnTestInjectorConnection.Name = "btnTestInjectorConnection";
			this.btnTestInjectorConnection.UseVisualStyleBackColor = true;
			this.btnTestInjectorConnection.Click += new System.EventHandler(this.btnTestInjectorConnection_Click);
			// 
			// cmbInjPortName
			// 
			this.cmbInjPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbInjPortName.FormattingEnabled = true;
			resources.ApplyResources(this.cmbInjPortName, "cmbInjPortName");
			this.cmbInjPortName.Name = "cmbInjPortName";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// RPV18Settings
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.btnTestConnection);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmbPortSpeed);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbPortName);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RPV18Settings";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbPortName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbPortSpeed;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkCom1Enable;
		private System.Windows.Forms.Button btnCom1Params;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbCom1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnCom2Params;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbCom2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkCom2Enable;
		private System.Windows.Forms.Button btnTestConnection;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox cmbInjPortName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnTestInjectorConnection;
	}
}