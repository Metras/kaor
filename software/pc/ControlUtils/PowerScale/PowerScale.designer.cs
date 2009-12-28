namespace ControlUtils.PowerScale
{
    partial class PowerScale
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
            this.tmrFall = new System.Windows.Forms.Timer(this.components);
            this.tmrSet = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrFall
            // 
            this.tmrFall.Enabled = true;
            this.tmrFall.Interval = 700;
            this.tmrFall.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tmrSet
            // 
            this.tmrSet.Interval = 50;
            this.tmrSet.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // PowerScale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PowerScale";
            this.Size = new System.Drawing.Size(236, 124);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PowerScale_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrFall;
        private System.Windows.Forms.Timer tmrSet;
    }
}
