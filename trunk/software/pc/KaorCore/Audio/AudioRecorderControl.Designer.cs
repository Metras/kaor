namespace KaorCore.Audio
{
	partial class AudioRecorderControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioRecorderControl));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnStopRecord = new System.Windows.Forms.Button();
			this.btnSettings = new System.Windows.Forms.Button();
			this.chkMute = new System.Windows.Forms.CheckBox();
			this.toolBtnRecord = new System.Windows.Forms.Button();
			this.txtName = new System.Windows.Forms.TextBox();
			this.pictTimeDomain = new System.Windows.Forms.PictureBox();
			this.trackVolume = new System.Windows.Forms.TrackBar();
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictTimeDomain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.AccessibleDescription = null;
			this.splitContainer1.AccessibleName = null;
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.BackgroundImage = null;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Font = null;
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.AccessibleDescription = null;
			this.splitContainer1.Panel1.AccessibleName = null;
			resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			this.splitContainer1.Panel1.BackgroundImage = null;
			this.splitContainer1.Panel1.Controls.Add(this.btnStopRecord);
			this.splitContainer1.Panel1.Controls.Add(this.btnSettings);
			this.splitContainer1.Panel1.Controls.Add(this.chkMute);
			this.splitContainer1.Panel1.Controls.Add(this.toolBtnRecord);
			this.splitContainer1.Panel1.Controls.Add(this.txtName);
			this.splitContainer1.Panel1.Font = null;
			this.toolTip1.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.AccessibleDescription = null;
			this.splitContainer1.Panel2.AccessibleName = null;
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.BackgroundImage = null;
			this.splitContainer1.Panel2.Controls.Add(this.pictTimeDomain);
			this.splitContainer1.Panel2.Controls.Add(this.trackVolume);
			this.splitContainer1.Panel2.Font = null;
			this.toolTip1.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
			this.toolTip1.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
			// 
			// btnStopRecord
			// 
			this.btnStopRecord.AccessibleDescription = null;
			this.btnStopRecord.AccessibleName = null;
			resources.ApplyResources(this.btnStopRecord, "btnStopRecord");
			this.btnStopRecord.BackgroundImage = null;
			this.btnStopRecord.Font = null;
			this.btnStopRecord.Name = "btnStopRecord";
			this.toolTip1.SetToolTip(this.btnStopRecord, resources.GetString("btnStopRecord.ToolTip"));
			this.btnStopRecord.UseVisualStyleBackColor = true;
			this.btnStopRecord.Click += new System.EventHandler(this.btnStopRecord_Click);
			// 
			// btnSettings
			// 
			this.btnSettings.AccessibleDescription = null;
			this.btnSettings.AccessibleName = null;
			resources.ApplyResources(this.btnSettings, "btnSettings");
			this.btnSettings.BackgroundImage = null;
			this.btnSettings.Font = null;
			this.btnSettings.Name = "btnSettings";
			this.toolTip1.SetToolTip(this.btnSettings, resources.GetString("btnSettings.ToolTip"));
			this.btnSettings.UseVisualStyleBackColor = true;
			this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
			// 
			// chkMute
			// 
			this.chkMute.AccessibleDescription = null;
			this.chkMute.AccessibleName = null;
			resources.ApplyResources(this.chkMute, "chkMute");
			this.chkMute.BackgroundImage = null;
			this.chkMute.Font = null;
			this.chkMute.Name = "chkMute";
			this.toolTip1.SetToolTip(this.chkMute, resources.GetString("chkMute.ToolTip"));
			this.chkMute.UseVisualStyleBackColor = true;
			this.chkMute.CheckedChanged += new System.EventHandler(this.chkMute_CheckedChanged);
			// 
			// toolBtnRecord
			// 
			this.toolBtnRecord.AccessibleDescription = null;
			this.toolBtnRecord.AccessibleName = null;
			resources.ApplyResources(this.toolBtnRecord, "toolBtnRecord");
			this.toolBtnRecord.BackgroundImage = null;
			this.toolBtnRecord.Font = null;
			this.toolBtnRecord.Name = "toolBtnRecord";
			this.toolTip1.SetToolTip(this.toolBtnRecord, resources.GetString("toolBtnRecord.ToolTip"));
			this.toolBtnRecord.UseVisualStyleBackColor = true;
			this.toolBtnRecord.Click += new System.EventHandler(this.toolBtnRecord_Click);
			// 
			// txtName
			// 
			this.txtName.AccessibleDescription = null;
			this.txtName.AccessibleName = null;
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.BackgroundImage = null;
			this.txtName.Font = null;
			this.txtName.Name = "txtName";
			this.toolTip1.SetToolTip(this.txtName, resources.GetString("txtName.ToolTip"));
			// 
			// pictTimeDomain
			// 
			this.pictTimeDomain.AccessibleDescription = null;
			this.pictTimeDomain.AccessibleName = null;
			resources.ApplyResources(this.pictTimeDomain, "pictTimeDomain");
			this.pictTimeDomain.BackColor = System.Drawing.Color.Black;
			this.pictTimeDomain.BackgroundImage = null;
			this.pictTimeDomain.Font = null;
			this.pictTimeDomain.ImageLocation = null;
			this.pictTimeDomain.Name = "pictTimeDomain";
			this.pictTimeDomain.TabStop = false;
			this.toolTip1.SetToolTip(this.pictTimeDomain, resources.GetString("pictTimeDomain.ToolTip"));
			// 
			// trackVolume
			// 
			this.trackVolume.AccessibleDescription = null;
			this.trackVolume.AccessibleName = null;
			resources.ApplyResources(this.trackVolume, "trackVolume");
			this.trackVolume.BackgroundImage = null;
			this.trackVolume.Font = null;
			this.trackVolume.Maximum = 100;
			this.trackVolume.Name = "trackVolume";
			this.trackVolume.TickFrequency = 5;
			this.trackVolume.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.toolTip1.SetToolTip(this.trackVolume, resources.GetString("trackVolume.ToolTip"));
			this.trackVolume.Value = 30;
			this.trackVolume.Scroll += new System.EventHandler(this.trackVolume_Scroll);
			// 
			// imgList
			// 
			this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			this.imgList.Images.SetKeyName(0, "stop.png");
			this.imgList.Images.SetKeyName(1, "record.png");
			// 
			// AudioRecorderControl
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.splitContainer1);
			this.Font = null;
			this.Name = "AudioRecorderControl";
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.VisibleChanged += new System.EventHandler(this.AudioRecorderControl_VisibleChanged);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictTimeDomain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.PictureBox pictTimeDomain;
		private System.Windows.Forms.TrackBar trackVolume;
		private System.Windows.Forms.Button toolBtnRecord;
		private System.Windows.Forms.ImageList imgList;
		private System.Windows.Forms.CheckBox chkMute;
		private System.Windows.Forms.Button btnSettings;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnStopRecord;
	}
}
