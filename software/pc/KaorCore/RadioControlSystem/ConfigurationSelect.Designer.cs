namespace KaorCore.RadioControlSystem
{
	partial class ConfigurationSelect
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
			this.lstConfigurations = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstConfigurations
			// 
			this.lstConfigurations.FormattingEnabled = true;
			this.lstConfigurations.Location = new System.Drawing.Point(12, 25);
			this.lstConfigurations.Name = "lstConfigurations";
			this.lstConfigurations.Size = new System.Drawing.Size(233, 95);
			this.lstConfigurations.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Available configurations:";
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(12, 126);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(100, 30);
			this.btnStart.TabIndex = 2;
			this.btnStart.Text = "button1";
			this.btnStart.UseVisualStyleBackColor = true;
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(145, 126);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(100, 30);
			this.btnExit.TabIndex = 3;
			this.btnExit.Text = "button2";
			this.btnExit.UseVisualStyleBackColor = true;
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(251, 25);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(30, 30);
			this.btnDelete.TabIndex = 4;
			this.btnDelete.Text = "button2";
			this.btnDelete.UseVisualStyleBackColor = true;
			// 
			// ConfigurationSelect
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(286, 164);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstConfigurations);
			this.Name = "ConfigurationSelect";
			this.Text = "Select system configuration";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstConfigurations;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnDelete;
	}
}