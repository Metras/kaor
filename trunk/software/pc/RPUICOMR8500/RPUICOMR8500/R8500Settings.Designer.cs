namespace RPUICOMR8500
{
	partial class R8500Settings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(R8500Settings));
			this.label1 = new System.Windows.Forms.Label();
			this.cmbPort = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbPortSpeed = new System.Windows.Forms.ComboBox();
			this.btnCheckConnect = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbCom = new System.Windows.Forms.ComboBox();
			this.btnComParams = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cmbPort
			// 
			this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPort.FormattingEnabled = true;
			resources.ApplyResources(this.cmbPort, "cmbPort");
			this.cmbPort.Name = "cmbPort";
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
            resources.GetString("cmbPortSpeed.Items2")});
			resources.ApplyResources(this.cmbPortSpeed, "cmbPortSpeed");
			this.cmbPortSpeed.Name = "cmbPortSpeed";
			// 
			// btnCheckConnect
			// 
			resources.ApplyResources(this.btnCheckConnect, "btnCheckConnect");
			this.btnCheckConnect.Name = "btnCheckConnect";
			this.btnCheckConnect.UseVisualStyleBackColor = true;
			this.btnCheckConnect.Click += new System.EventHandler(this.btnCheckConnect_Click);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// cmbCom
			// 
			this.cmbCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCom.FormattingEnabled = true;
			this.cmbCom.Items.AddRange(new object[] {
            resources.GetString("cmbCom.Items"),
            resources.GetString("cmbCom.Items1")});
			resources.ApplyResources(this.cmbCom, "cmbCom");
			this.cmbCom.Name = "cmbCom";
			// 
			// btnComParams
			// 
			resources.ApplyResources(this.btnComParams, "btnComParams");
			this.btnComParams.Name = "btnComParams";
			this.btnComParams.UseVisualStyleBackColor = true;
			this.btnComParams.Click += new System.EventHandler(this.btnComParams_Click);
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
			// R8500Settings
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnComParams);
			this.Controls.Add(this.cmbCom);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCheckConnect);
			this.Controls.Add(this.cmbPortSpeed);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbPort);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "R8500Settings";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbPortSpeed;
		private System.Windows.Forms.Button btnCheckConnect;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbCom;
		private System.Windows.Forms.Button btnComParams;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}