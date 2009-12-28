namespace KaorCore.Report
{
	partial class ReportSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportSettings));
			this.label1 = new System.Windows.Forms.Label();
			this.txtOrg = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtStation = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtNotes = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
			// 
			// txtOrg
			// 
			this.txtOrg.AccessibleDescription = null;
			this.txtOrg.AccessibleName = null;
			resources.ApplyResources(this.txtOrg, "txtOrg");
			this.txtOrg.BackgroundImage = null;
			this.txtOrg.Font = null;
			this.txtOrg.Name = "txtOrg";
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			// 
			// txtStation
			// 
			this.txtStation.AccessibleDescription = null;
			this.txtStation.AccessibleName = null;
			resources.ApplyResources(this.txtStation, "txtStation");
			this.txtStation.BackgroundImage = null;
			this.txtStation.Font = null;
			this.txtStation.Name = "txtStation";
			// 
			// label3
			// 
			this.label3.AccessibleDescription = null;
			this.label3.AccessibleName = null;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Font = null;
			this.label3.Name = "label3";
			// 
			// txtNotes
			// 
			this.txtNotes.AccessibleDescription = null;
			this.txtNotes.AccessibleName = null;
			resources.ApplyResources(this.txtNotes, "txtNotes");
			this.txtNotes.BackgroundImage = null;
			this.txtNotes.Font = null;
			this.txtNotes.Name = "txtNotes";
			// 
			// btnOk
			// 
			this.btnOk.AccessibleDescription = null;
			this.btnOk.AccessibleName = null;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackgroundImage = null;
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
			// ReportSettings
			// 
			this.AcceptButton = this.btnOk;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtNotes);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtStation);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtOrg);
			this.Controls.Add(this.label1);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ReportSettings";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOrg;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtStation;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtNotes;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}