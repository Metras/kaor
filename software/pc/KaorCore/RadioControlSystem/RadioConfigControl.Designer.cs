namespace KaorCore.RadioControlSystem
{
	partial class RadioConfigControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadioConfigControl));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolBtnReset = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolBtnSave = new System.Windows.Forms.ToolStripButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnRPUDelete = new System.Windows.Forms.Button();
			this.btnRPUEdit = new System.Windows.Forms.Button();
			this.btnRPUAdd = new System.Windows.Forms.Button();
			this.lstRPU = new ControlUtils.ObjectListView.ObjectListView();
			this.olvRPUStatus = new ControlUtils.ObjectListView.OLVColumn();
			this.olvRPUName = new ControlUtils.ObjectListView.OLVColumn();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnAntennaDelete = new System.Windows.Forms.Button();
			this.btnAntennaEdit = new System.Windows.Forms.Button();
			this.btnAntennaAdd = new System.Windows.Forms.Button();
			this.lstAntenna = new ControlUtils.ObjectListView.ObjectListView();
			this.olvAntennaStatus = new ControlUtils.ObjectListView.OLVColumn();
			this.olvAntennaName = new ControlUtils.ObjectListView.OLVColumn();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.toolStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lstRPU)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lstAntenna)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnReset,
            this.toolStripSeparator1,
            this.toolBtnSave});
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// toolBtnReset
			// 
			this.toolBtnReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.toolBtnReset, "toolBtnReset");
			this.toolBtnReset.Name = "toolBtnReset";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// toolBtnSave
			// 
			this.toolBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.toolBtnSave, "toolBtnSave");
			this.toolBtnSave.Name = "toolBtnSave";
			this.toolBtnSave.Click += new System.EventHandler(this.toolBtnSave_Click);
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.btnRPUDelete);
			this.groupBox1.Controls.Add(this.btnRPUEdit);
			this.groupBox1.Controls.Add(this.btnRPUAdd);
			this.groupBox1.Controls.Add(this.lstRPU);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// btnRPUDelete
			// 
			resources.ApplyResources(this.btnRPUDelete, "btnRPUDelete");
			this.btnRPUDelete.Name = "btnRPUDelete";
			this.toolTip1.SetToolTip(this.btnRPUDelete, resources.GetString("btnRPUDelete.ToolTip"));
			this.btnRPUDelete.UseVisualStyleBackColor = true;
			this.btnRPUDelete.Click += new System.EventHandler(this.btnRPUDelete_Click);
			// 
			// btnRPUEdit
			// 
			resources.ApplyResources(this.btnRPUEdit, "btnRPUEdit");
			this.btnRPUEdit.Name = "btnRPUEdit";
			this.toolTip1.SetToolTip(this.btnRPUEdit, resources.GetString("btnRPUEdit.ToolTip"));
			this.btnRPUEdit.UseVisualStyleBackColor = true;
			this.btnRPUEdit.Click += new System.EventHandler(this.btnRPUEdit_Click);
			// 
			// btnRPUAdd
			// 
			resources.ApplyResources(this.btnRPUAdd, "btnRPUAdd");
			this.btnRPUAdd.Name = "btnRPUAdd";
			this.toolTip1.SetToolTip(this.btnRPUAdd, resources.GetString("btnRPUAdd.ToolTip"));
			this.btnRPUAdd.UseVisualStyleBackColor = true;
			this.btnRPUAdd.Click += new System.EventHandler(this.btnRPUAdd_Click);
			// 
			// lstRPU
			// 
			this.lstRPU.AllColumns.Add(this.olvRPUStatus);
			this.lstRPU.AllColumns.Add(this.olvRPUName);
			this.lstRPU.AlternateRowBackColor = System.Drawing.Color.Empty;
			resources.ApplyResources(this.lstRPU, "lstRPU");
			this.lstRPU.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvRPUStatus,
            this.olvRPUName});
			this.lstRPU.FullRowSelect = true;
			this.lstRPU.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstRPU.HideSelection = false;
			this.lstRPU.MultiSelect = false;
			this.lstRPU.Name = "lstRPU";
			this.lstRPU.OwnerDraw = true;
			this.lstRPU.ShowGroups = false;
			this.lstRPU.SmallImageList = this.images;
			this.lstRPU.UseCompatibleStateImageBehavior = false;
			this.lstRPU.View = System.Windows.Forms.View.Details;
			this.lstRPU.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstRPU_MouseDoubleClick);
			// 
			// olvRPUStatus
			// 
			this.olvRPUStatus.AspectName = null;
			this.olvRPUStatus.IsEditable = false;
			this.olvRPUStatus.MaximumWidth = 20;
			this.olvRPUStatus.MinimumWidth = 20;
			resources.ApplyResources(this.olvRPUStatus, "olvRPUStatus");
			// 
			// olvRPUName
			// 
			this.olvRPUName.AspectName = null;
			this.olvRPUName.FillsFreeSpace = true;
			this.olvRPUName.IsEditable = false;
			resources.ApplyResources(this.olvRPUName, "olvRPUName");
			// 
			// images
			// 
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			this.images.Images.SetKeyName(0, "hand.png");
			this.images.Images.SetKeyName(1, "disabled.png");
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.btnAntennaDelete);
			this.groupBox2.Controls.Add(this.btnAntennaEdit);
			this.groupBox2.Controls.Add(this.btnAntennaAdd);
			this.groupBox2.Controls.Add(this.lstAntenna);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// btnAntennaDelete
			// 
			resources.ApplyResources(this.btnAntennaDelete, "btnAntennaDelete");
			this.btnAntennaDelete.Name = "btnAntennaDelete";
			this.toolTip1.SetToolTip(this.btnAntennaDelete, resources.GetString("btnAntennaDelete.ToolTip"));
			this.btnAntennaDelete.UseVisualStyleBackColor = true;
			this.btnAntennaDelete.Click += new System.EventHandler(this.btnAntennaDelete_Click);
			// 
			// btnAntennaEdit
			// 
			resources.ApplyResources(this.btnAntennaEdit, "btnAntennaEdit");
			this.btnAntennaEdit.Name = "btnAntennaEdit";
			this.toolTip1.SetToolTip(this.btnAntennaEdit, resources.GetString("btnAntennaEdit.ToolTip"));
			this.btnAntennaEdit.UseVisualStyleBackColor = true;
			this.btnAntennaEdit.Click += new System.EventHandler(this.btnAntennaEdit_Click);
			// 
			// btnAntennaAdd
			// 
			resources.ApplyResources(this.btnAntennaAdd, "btnAntennaAdd");
			this.btnAntennaAdd.Name = "btnAntennaAdd";
			this.toolTip1.SetToolTip(this.btnAntennaAdd, resources.GetString("btnAntennaAdd.ToolTip"));
			this.btnAntennaAdd.UseVisualStyleBackColor = true;
			this.btnAntennaAdd.Click += new System.EventHandler(this.btnAntennaAdd_Click);
			// 
			// lstAntenna
			// 
			this.lstAntenna.AllColumns.Add(this.olvAntennaStatus);
			this.lstAntenna.AllColumns.Add(this.olvAntennaName);
			this.lstAntenna.AlternateRowBackColor = System.Drawing.Color.Empty;
			resources.ApplyResources(this.lstAntenna, "lstAntenna");
			this.lstAntenna.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvAntennaStatus,
            this.olvAntennaName});
			this.lstAntenna.FullRowSelect = true;
			this.lstAntenna.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstAntenna.HideSelection = false;
			this.lstAntenna.MultiSelect = false;
			this.lstAntenna.Name = "lstAntenna";
			this.lstAntenna.OwnerDraw = true;
			this.lstAntenna.ShowGroups = false;
			this.lstAntenna.SmallImageList = this.images;
			this.lstAntenna.UseCompatibleStateImageBehavior = false;
			this.lstAntenna.View = System.Windows.Forms.View.Details;
			// 
			// olvAntennaStatus
			// 
			this.olvAntennaStatus.AspectName = null;
			this.olvAntennaStatus.MaximumWidth = 20;
			this.olvAntennaStatus.MinimumWidth = 20;
			resources.ApplyResources(this.olvAntennaStatus, "olvAntennaStatus");
			// 
			// olvAntennaName
			// 
			this.olvAntennaName.AspectName = null;
			this.olvAntennaName.FillsFreeSpace = true;
			resources.ApplyResources(this.olvAntennaName, "olvAntennaName");
			// 
			// RadioConfigControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "RadioConfigControl";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lstRPU)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.lstAntenna)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolBtnReset;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolBtnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnRPUDelete;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button btnRPUEdit;
		private System.Windows.Forms.Button btnRPUAdd;
		private ControlUtils.ObjectListView.ObjectListView lstRPU;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnAntennaDelete;
		private System.Windows.Forms.Button btnAntennaEdit;
		private System.Windows.Forms.Button btnAntennaAdd;
		private ControlUtils.ObjectListView.ObjectListView lstAntenna;
		private ControlUtils.ObjectListView.OLVColumn olvRPUStatus;
		private ControlUtils.ObjectListView.OLVColumn olvRPUName;
		private ControlUtils.ObjectListView.OLVColumn olvAntennaStatus;
		private ControlUtils.ObjectListView.OLVColumn olvAntennaName;
		private System.Windows.Forms.ImageList images;

	}
}
