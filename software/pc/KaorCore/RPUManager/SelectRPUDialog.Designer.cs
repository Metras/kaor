namespace KaorCore.RPUManager
{
	partial class SelectRPUDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRPUDialog));
			this.lstRPU = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstRPU
			// 
			this.lstRPU.FormattingEnabled = true;
			resources.ApplyResources(this.lstRPU, "lstRPU");
			this.lstRPU.Name = "lstRPU";
			this.lstRPU.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstRPU_MouseDoubleClick);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// btnSelect
			// 
			resources.ApplyResources(this.btnSelect, "btnSelect");
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// SelectRPUDialog
			// 
			this.AcceptButton = this.btnSelect;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstRPU);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectRPUDialog";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstRPU;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;
	}
}