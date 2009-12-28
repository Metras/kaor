namespace KaorCore.Marker
{
	partial class NewMarkerDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMarkerDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.cmbColor = new Com.Windows.Forms.ColorPicker();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtFrequency = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
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
			// cmbColor
			// 
			this.cmbColor.AccessibleDescription = null;
			this.cmbColor.AccessibleName = null;
			resources.ApplyResources(this.cmbColor, "cmbColor");
			this.cmbColor.BackColor = System.Drawing.SystemColors.Window;
			this.cmbColor.BackgroundImage = null;
			this.cmbColor.Context = null;
			this.cmbColor.Font = null;
			this.cmbColor.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cmbColor.Name = "cmbColor";
			this.cmbColor.ReadOnly = false;
			this.cmbColor.Value = System.Drawing.Color.Green;
			// 
			// txtName
			// 
			this.txtName.AccessibleDescription = null;
			this.txtName.AccessibleName = null;
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.BackgroundImage = null;
			this.txtName.Font = null;
			this.txtName.Name = "txtName";
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			// 
			// label3
			// 
			this.label3.AccessibleDescription = null;
			this.label3.AccessibleName = null;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Font = null;
			this.label3.Name = "label3";
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
			// txtFrequency
			// 
			this.txtFrequency.AccessibleDescription = null;
			this.txtFrequency.AccessibleName = null;
			resources.ApplyResources(this.txtFrequency, "txtFrequency");
			this.txtFrequency.BackgroundImage = null;
			this.txtFrequency.Font = null;
			this.txtFrequency.Frequency = ((long)(100));
			this.txtFrequency.Max = ((long)(3000000000));
			this.txtFrequency.Min = ((long)(100));
			this.txtFrequency.Name = "txtFrequency";
			// 
			// NewMarkerDialog
			// 
			this.AcceptButton = this.btnOk;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.txtFrequency);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.cmbColor);
			this.Controls.Add(this.label1);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewMarkerDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		public Com.Windows.Forms.ColorPicker cmbColor;
		public System.Windows.Forms.TextBox txtName;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox txtFrequency;
	}
}