namespace KaorMainView
{
	partial class KaorMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KaorMain));
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolRPUList = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.rcsView = new KaorCore.RadioControlSystem.BaseRCSView();
			this.lblNoRPU = new System.Windows.Forms.Label();
			this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.Panel2.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			this.toolStripContainer1.AccessibleDescription = null;
			this.toolStripContainer1.AccessibleName = null;
			resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
			// 
			// toolStripContainer1.BottomToolStripPanel
			// 
			this.toolStripContainer1.BottomToolStripPanel.AccessibleDescription = null;
			this.toolStripContainer1.BottomToolStripPanel.AccessibleName = null;
			this.toolStripContainer1.BottomToolStripPanel.BackgroundImage = null;
			resources.ApplyResources(this.toolStripContainer1.BottomToolStripPanel, "toolStripContainer1.BottomToolStripPanel");
			this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
			this.toolStripContainer1.BottomToolStripPanel.Font = null;
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.AccessibleDescription = null;
			this.toolStripContainer1.ContentPanel.AccessibleName = null;
			resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
			this.toolStripContainer1.ContentPanel.BackgroundImage = null;
			this.toolStripContainer1.ContentPanel.Controls.Add(this.mainSplitContainer);
			this.toolStripContainer1.ContentPanel.Font = null;
			this.toolStripContainer1.Font = null;
			// 
			// toolStripContainer1.LeftToolStripPanel
			// 
			this.toolStripContainer1.LeftToolStripPanel.AccessibleDescription = null;
			this.toolStripContainer1.LeftToolStripPanel.AccessibleName = null;
			this.toolStripContainer1.LeftToolStripPanel.BackgroundImage = null;
			resources.ApplyResources(this.toolStripContainer1.LeftToolStripPanel, "toolStripContainer1.LeftToolStripPanel");
			this.toolStripContainer1.LeftToolStripPanel.Font = null;
			this.toolStripContainer1.LeftToolStripPanelVisible = false;
			this.toolStripContainer1.Name = "toolStripContainer1";
			// 
			// toolStripContainer1.RightToolStripPanel
			// 
			this.toolStripContainer1.RightToolStripPanel.AccessibleDescription = null;
			this.toolStripContainer1.RightToolStripPanel.AccessibleName = null;
			this.toolStripContainer1.RightToolStripPanel.BackgroundImage = null;
			resources.ApplyResources(this.toolStripContainer1.RightToolStripPanel, "toolStripContainer1.RightToolStripPanel");
			this.toolStripContainer1.RightToolStripPanel.Font = null;
			this.toolStripContainer1.RightToolStripPanelVisible = false;
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.AccessibleDescription = null;
			this.toolStripContainer1.TopToolStripPanel.AccessibleName = null;
			this.toolStripContainer1.TopToolStripPanel.BackgroundImage = null;
			resources.ApplyResources(this.toolStripContainer1.TopToolStripPanel, "toolStripContainer1.TopToolStripPanel");
			this.toolStripContainer1.TopToolStripPanel.Font = null;
			this.toolStripContainer1.TopToolStripPanelVisible = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.AccessibleDescription = null;
			this.statusStrip1.AccessibleName = null;
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.BackgroundImage = null;
			this.statusStrip1.Font = null;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolRPUList,
            this.toolStripStatus});
			this.statusStrip1.Name = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.AccessibleDescription = null;
			this.toolStripStatusLabel1.AccessibleName = null;
			resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripStatusLabel1.BackgroundImage = null;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			// 
			// toolRPUList
			// 
			this.toolRPUList.AccessibleDescription = null;
			this.toolRPUList.AccessibleName = null;
			resources.ApplyResources(this.toolRPUList, "toolRPUList");
			this.toolRPUList.BackgroundImage = null;
			this.toolRPUList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolRPUList.Name = "toolRPUList";
			// 
			// toolStripStatus
			// 
			this.toolStripStatus.AccessibleDescription = null;
			this.toolStripStatus.AccessibleName = null;
			resources.ApplyResources(this.toolStripStatus, "toolStripStatus");
			this.toolStripStatus.BackgroundImage = null;
			this.toolStripStatus.Name = "toolStripStatus";
			this.toolStripStatus.Spring = true;
			// 
			// mainSplitContainer
			// 
			this.mainSplitContainer.AccessibleDescription = null;
			this.mainSplitContainer.AccessibleName = null;
			resources.ApplyResources(this.mainSplitContainer, "mainSplitContainer");
			this.mainSplitContainer.BackgroundImage = null;
			this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.mainSplitContainer.Font = null;
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.AccessibleDescription = null;
			this.mainSplitContainer.Panel1.AccessibleName = null;
			resources.ApplyResources(this.mainSplitContainer.Panel1, "mainSplitContainer.Panel1");
			this.mainSplitContainer.Panel1.BackgroundImage = null;
			this.mainSplitContainer.Panel1.Controls.Add(this.rcsView);
			this.mainSplitContainer.Panel1.Font = null;
			// 
			// mainSplitContainer.Panel2
			// 
			this.mainSplitContainer.Panel2.AccessibleDescription = null;
			this.mainSplitContainer.Panel2.AccessibleName = null;
			resources.ApplyResources(this.mainSplitContainer.Panel2, "mainSplitContainer.Panel2");
			this.mainSplitContainer.Panel2.BackgroundImage = null;
			this.mainSplitContainer.Panel2.Controls.Add(this.lblNoRPU);
			this.mainSplitContainer.Panel2.Font = null;
			// 
			// rcsView
			// 
			this.rcsView.AccessibleDescription = null;
			this.rcsView.AccessibleName = null;
			resources.ApplyResources(this.rcsView, "rcsView");
			this.rcsView.BackgroundImage = null;
			this.rcsView.CtrlPressed = false;
			this.rcsView.CursorFrequency = ((long)(0));
			this.rcsView.Font = null;
			this.rcsView.Name = "rcsView";
			this.rcsView.RCS = null;
			this.rcsView.ShiftPressed = false;
			// 
			// lblNoRPU
			// 
			this.lblNoRPU.AccessibleDescription = null;
			this.lblNoRPU.AccessibleName = null;
			resources.ApplyResources(this.lblNoRPU, "lblNoRPU");
			this.lblNoRPU.Name = "lblNoRPU";
			// 
			// KaorMain
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.Controls.Add(this.toolStripContainer1);
			this.Font = null;
			this.KeyPreview = true;
			this.Name = "KaorMain";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Shown += new System.EventHandler(this.KaorMain_Shown);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.KaorMain_FormClosed);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KaorMain_KeyUp);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KaorMain_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KaorMain_KeyDown);
			this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			this.mainSplitContainer.Panel2.ResumeLayout(false);
			this.mainSplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.SplitContainer mainSplitContainer;
		private KaorCore.RadioControlSystem.BaseRCSView rcsView;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripDropDownButton toolRPUList;
		private System.Windows.Forms.Label lblNoRPU;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
	}
}