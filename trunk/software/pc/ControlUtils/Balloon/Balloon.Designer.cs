namespace ControlUtils.Balloon
{
    partial class Balloon
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
            this.AntennaName = new System.Windows.Forms.Label();
            this.Description = new System.Windows.Forms.Label();
            this.GPS = new System.Windows.Forms.Label();
            this.tmrHide = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // AntennaName
            // 
            this.AntennaName.AutoSize = true;
            this.AntennaName.Location = new System.Drawing.Point(12, 9);
            this.AntennaName.Name = "AntennaName";
            this.AntennaName.Size = new System.Drawing.Size(35, 13);
            this.AntennaName.TabIndex = 0;
            this.AntennaName.Text = "Name";
            // 
            // Description
            // 
            this.Description.AutoSize = true;
            this.Description.Location = new System.Drawing.Point(12, 37);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(60, 13);
            this.Description.TabIndex = 1;
            this.Description.Text = "Description";
            // 
            // GPS
            // 
            this.GPS.AutoSize = true;
            this.GPS.Location = new System.Drawing.Point(12, 64);
            this.GPS.Name = "GPS";
            this.GPS.Size = new System.Drawing.Size(29, 13);
            this.GPS.TabIndex = 2;
            this.GPS.Text = "GPS";
            // 
            // tmrHide
            // 
            this.tmrHide.Interval = 3000;
            this.tmrHide.Tick += new System.EventHandler(this.tmrHide_Tick);
            // 
            // Balloon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 96);
            this.Controls.Add(this.GPS);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.AntennaName);
            this.Name = "Balloon";
            this.Text = "4565556";
            this.Shown += new System.EventHandler(this.Balloon_Shown);
            this.VisibleChanged += new System.EventHandler(this.Balloon_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label AntennaName;
        public System.Windows.Forms.Label Description;
        public System.Windows.Forms.Label GPS;
        private System.Windows.Forms.Timer tmrHide;
    }
}
