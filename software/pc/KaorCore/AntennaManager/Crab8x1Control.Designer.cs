namespace KaorCore.AntennaManager
{
    partial class Crab8x1Control
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Crab8x1Control));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.AntennaList = new System.Windows.Forms.ListBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.stateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bADToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fAULTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.ShowAlways = true;
			// 
			// AntennaList
			// 
			resources.ApplyResources(this.AntennaList, "AntennaList");
			this.AntennaList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.AntennaList.FormattingEnabled = true;
			this.AntennaList.Name = "AntennaList";
			this.AntennaList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AntennaList_MouseClick);
			this.AntennaList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
			this.AntennaList.SelectedIndexChanged += new System.EventHandler(this.AntennaList_SelectedIndexChanged);
			this.AntennaList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseMove);
			this.AntennaList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateToolStripMenuItem,
            this.propertiesToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.ShowCheckMargin = true;
			resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
			// 
			// stateToolStripMenuItem
			// 
			this.stateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem,
            this.bADToolStripMenuItem,
            this.fAULTToolStripMenuItem});
			this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
			resources.ApplyResources(this.stateToolStripMenuItem, "stateToolStripMenuItem");
			// 
			// oKToolStripMenuItem
			// 
			this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
			resources.ApplyResources(this.oKToolStripMenuItem, "oKToolStripMenuItem");
			this.oKToolStripMenuItem.Click += new System.EventHandler(this.oKToolStripMenuItem_Click);
			// 
			// bADToolStripMenuItem
			// 
			this.bADToolStripMenuItem.Name = "bADToolStripMenuItem";
			resources.ApplyResources(this.bADToolStripMenuItem, "bADToolStripMenuItem");
			this.bADToolStripMenuItem.Click += new System.EventHandler(this.bADToolStripMenuItem_Click);
			// 
			// fAULTToolStripMenuItem
			// 
			this.fAULTToolStripMenuItem.Name = "fAULTToolStripMenuItem";
			resources.ApplyResources(this.fAULTToolStripMenuItem, "fAULTToolStripMenuItem");
			this.fAULTToolStripMenuItem.Click += new System.EventHandler(this.fAULTToolStripMenuItem_Click);
			// 
			// propertiesToolStripMenuItem
			// 
			this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
			resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
			this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
			// 
			// vScrollBar1
			// 
			resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			// 
			// Crab8x1Control
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.AntennaList);
			this.Name = "Crab8x1Control";
			this.Resize += new System.EventHandler(this.Crab8x1Control_Resize);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox AntennaList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bADToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fAULTToolStripMenuItem;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    }
}
