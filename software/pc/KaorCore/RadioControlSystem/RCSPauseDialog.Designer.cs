namespace KaorCore.RadioControlSystem
{
	partial class RCSPauseDialog
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RCSPauseDialog));
			this.progTimer = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFreq = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblPower = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.tmrTimeout = new System.Windows.Forms.Timer(this.components);
			this.btnContinue = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lblSignalName = new System.Windows.Forms.Label();
			this.lblCaption = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblDelta = new System.Windows.Forms.Label();
			this.btnDecline = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// progTimer
			// 
			resources.ApplyResources(this.progTimer, "progTimer");
			this.progTimer.Maximum = 5;
			this.progTimer.Name = "progTimer";
			this.progTimer.Value = 5;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lblFreq
			// 
			resources.ApplyResources(this.lblFreq, "lblFreq");
			this.lblFreq.Name = "lblFreq";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// lblPower
			// 
			resources.ApplyResources(this.lblPower, "lblPower");
			this.lblPower.Name = "lblPower";
			// 
			// btnStop
			// 
			this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Abort;
			resources.ApplyResources(this.btnStop, "btnStop");
			this.btnStop.Name = "btnStop";
			this.btnStop.UseVisualStyleBackColor = true;
			// 
			// tmrTimeout
			// 
			this.tmrTimeout.Enabled = true;
			this.tmrTimeout.Tick += new System.EventHandler(this.tmrTimeout_Tick);
			// 
			// btnContinue
			// 
			this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Yes;
			resources.ApplyResources(this.btnContinue, "btnContinue");
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// lblSignalName
			// 
			resources.ApplyResources(this.lblSignalName, "lblSignalName");
			this.lblSignalName.Name = "lblSignalName";
			// 
			// lblCaption
			// 
			resources.ApplyResources(this.lblCaption, "lblCaption");
			this.lblCaption.Name = "lblCaption";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// lblDelta
			// 
			resources.ApplyResources(this.lblDelta, "lblDelta");
			this.lblDelta.Name = "lblDelta";
			// 
			// btnDecline
			// 
			this.btnDecline.DialogResult = System.Windows.Forms.DialogResult.No;
			resources.ApplyResources(this.btnDecline, "btnDecline");
			this.btnDecline.Name = "btnDecline";
			this.btnDecline.UseVisualStyleBackColor = true;
			// 
			// RCSPauseDialog
			// 
			this.AcceptButton = this.btnStop;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.btnDecline);
			this.Controls.Add(this.lblDelta);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblCaption);
			this.Controls.Add(this.lblSignalName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.lblPower);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblFreq);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.progTimer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RCSPauseDialog";
			this.ShowInTaskbar = false;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RCSPauseDialog_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progTimer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFreq;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblPower;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Timer tmrTimeout;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblSignalName;
		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblDelta;
		private System.Windows.Forms.Button btnDecline;
	}
}