namespace KaorCore.Antenna
{
    partial class AntennaList
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lstAntennaList = new System.Windows.Forms.ListBox();
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
            this.lstAntennaList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAntennaList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstAntennaList.FormattingEnabled = true;
            this.lstAntennaList.Location = new System.Drawing.Point(-3, -3);
            this.lstAntennaList.Name = "AntennaList";
            this.lstAntennaList.Size = new System.Drawing.Size(159, 160);
            this.lstAntennaList.TabIndex = 0;
            this.lstAntennaList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseClick);
            this.lstAntennaList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.lstAntennaList.SelectedIndexChanged += new System.EventHandler(this.AntennaList_SelectedIndexChanged);
            this.lstAntennaList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseMove);
            this.lstAntennaList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AntennaList_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowCheckMargin = true;
            this.contextMenuStrip1.Size = new System.Drawing.Size(157, 48);
            // 
            // stateToolStripMenuItem
            // 
            this.stateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oKToolStripMenuItem,
            this.bADToolStripMenuItem,
            this.fAULTToolStripMenuItem});
            this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
            this.stateToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.stateToolStripMenuItem.Text = "State";
            // 
            // oKToolStripMenuItem
            // 
            this.oKToolStripMenuItem.Name = "oKToolStripMenuItem";
            this.oKToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.oKToolStripMenuItem.Text = "OK";
            this.oKToolStripMenuItem.Click += new System.EventHandler(this.oKToolStripMenuItem_Click);
            // 
            // bADToolStripMenuItem
            // 
            this.bADToolStripMenuItem.Name = "bADToolStripMenuItem";
            this.bADToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.bADToolStripMenuItem.Text = "BAD";
            this.bADToolStripMenuItem.Click += new System.EventHandler(this.bADToolStripMenuItem_Click);
            // 
            // fAULTToolStripMenuItem
            // 
            this.fAULTToolStripMenuItem.Name = "fAULTToolStripMenuItem";
            this.fAULTToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.fAULTToolStripMenuItem.Text = "FAULT";
            this.fAULTToolStripMenuItem.Click += new System.EventHandler(this.fAULTToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(133, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 150);
            this.vScrollBar1.TabIndex = 1;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // RPV3AntennaList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.lstAntennaList);
            this.Name = "RPV3AntennaList";
            this.Resize += new System.EventHandler(this.RPV3AntennaList_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox lstAntennaList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bADToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fAULTToolStripMenuItem;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    }
}
