namespace ControlUtils.FrequencyRadio
{
    partial class FrequencyRadio
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
            this.tmr_ChangedDelay = new System.Windows.Forms.Timer(this.components);
            this.bgwRedraw = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // tmr_ChangedDelay
            // 
            this.tmr_ChangedDelay.Interval = 1000;
            this.tmr_ChangedDelay.Tick += new System.EventHandler(this.tmr_ChangedDelay_Tick);
            // 
            // bgwRedraw
            // 
            this.bgwRedraw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRedraw_DoWork);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmr_ChangedDelay;
        private System.ComponentModel.BackgroundWorker bgwRedraw;
    }
}
