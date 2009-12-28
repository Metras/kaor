namespace ControlUtils.AntennaPropsDialog
{
    partial class AntennaPropsDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntennaPropsDialog));
			this.lbl_AntennaName = new System.Windows.Forms.Label();
			this.lbl_Description = new System.Windows.Forms.Label();
			this.lbl_GPS = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbl_AntennaName
			// 
			this.lbl_AntennaName.AccessibleDescription = null;
			this.lbl_AntennaName.AccessibleName = null;
			resources.ApplyResources(this.lbl_AntennaName, "lbl_AntennaName");
			this.lbl_AntennaName.Font = null;
			this.lbl_AntennaName.Name = "lbl_AntennaName";
			// 
			// lbl_Description
			// 
			this.lbl_Description.AccessibleDescription = null;
			this.lbl_Description.AccessibleName = null;
			resources.ApplyResources(this.lbl_Description, "lbl_Description");
			this.lbl_Description.Font = null;
			this.lbl_Description.Name = "lbl_Description";
			// 
			// lbl_GPS
			// 
			this.lbl_GPS.AccessibleDescription = null;
			this.lbl_GPS.AccessibleName = null;
			resources.ApplyResources(this.lbl_GPS, "lbl_GPS");
			this.lbl_GPS.Font = null;
			this.lbl_GPS.Name = "lbl_GPS";
			// 
			// btnOK
			// 
			this.btnOK.AccessibleDescription = null;
			this.btnOK.AccessibleName = null;
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.BackgroundImage = null;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Font = null;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
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
			// AntennaPropsDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lbl_GPS);
			this.Controls.Add(this.lbl_Description);
			this.Controls.Add(this.lbl_AntennaName);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AntennaPropsDialog";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Label lbl_AntennaName;
        public System.Windows.Forms.Label lbl_Description;
        public System.Windows.Forms.Label lbl_GPS;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.Label label2;
		public System.Windows.Forms.Label label3;
    }
}