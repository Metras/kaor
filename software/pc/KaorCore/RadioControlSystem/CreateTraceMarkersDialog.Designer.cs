namespace KaorCore.RadioControlSystem
{
	partial class CreateTraceMarkersDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateTraceMarkersDialog));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pwrMax = new System.Windows.Forms.NumericUpDown();
			this.zgPower = new ZedGraph.ZedGraphControl();
			this.label2 = new System.Windows.Forms.Label();
			this.pwrMin = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.clrMarker = new Com.Windows.Forms.ColorPicker();
			this.btnClearAll = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.olvMarkers = new ControlUtils.ObjectListView.ObjectListView();
			this.olvFrequency = new ControlUtils.ObjectListView.OLVColumn();
			this.olvPower = new ControlUtils.ObjectListView.OLVColumn();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pwrMax)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pwrMin)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.olvMarkers)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.AccessibleDescription = null;
			this.groupBox1.AccessibleName = null;
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.BackgroundImage = null;
			this.groupBox1.Controls.Add(this.pwrMax);
			this.groupBox1.Controls.Add(this.zgPower);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.pwrMin);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.btnRefresh);
			this.groupBox1.Font = null;
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
			// 
			// pwrMax
			// 
			this.pwrMax.AccessibleDescription = null;
			this.pwrMax.AccessibleName = null;
			resources.ApplyResources(this.pwrMax, "pwrMax");
			this.pwrMax.Font = null;
			this.pwrMax.Name = "pwrMax";
			this.toolTip1.SetToolTip(this.pwrMax, resources.GetString("pwrMax.ToolTip"));
			this.pwrMax.ValueChanged += new System.EventHandler(this.pwrMax_ValueChanged);
			// 
			// zgPower
			// 
			this.zgPower.AccessibleDescription = null;
			this.zgPower.AccessibleName = null;
			resources.ApplyResources(this.zgPower, "zgPower");
			this.zgPower.BackgroundImage = null;
			this.zgPower.Font = null;
			this.zgPower.IsAutoCursor = true;
			this.zgPower.IsEnableHEdit = true;
			this.zgPower.IsEnableHPan = false;
			this.zgPower.IsEnableHZoom = false;
			this.zgPower.IsEnableVEdit = true;
			this.zgPower.IsEnableVPan = false;
			this.zgPower.IsEnableVZoom = false;
			this.zgPower.IsEnableWheelZoom = false;
			this.zgPower.IsShowContextMenu = false;
			this.zgPower.Name = "zgPower";
			this.zgPower.ScrollGrace = 0;
			this.zgPower.ScrollMaxX = 0;
			this.zgPower.ScrollMaxY = 0;
			this.zgPower.ScrollMaxY2 = 0;
			this.zgPower.ScrollMinX = 0;
			this.zgPower.ScrollMinY = 0;
			this.zgPower.ScrollMinY2 = 0;
			this.zgPower.SelectAppendModifierKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.None)));
			this.toolTip1.SetToolTip(this.zgPower, resources.GetString("zgPower.ToolTip"));
			// 
			// label2
			// 
			this.label2.AccessibleDescription = null;
			this.label2.AccessibleName = null;
			resources.ApplyResources(this.label2, "label2");
			this.label2.Font = null;
			this.label2.Name = "label2";
			this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
			// 
			// pwrMin
			// 
			this.pwrMin.AccessibleDescription = null;
			this.pwrMin.AccessibleName = null;
			resources.ApplyResources(this.pwrMin, "pwrMin");
			this.pwrMin.Font = null;
			this.pwrMin.Name = "pwrMin";
			this.toolTip1.SetToolTip(this.pwrMin, resources.GetString("pwrMin.ToolTip"));
			this.pwrMin.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.pwrMin.ValueChanged += new System.EventHandler(this.pwrMin_ValueChanged);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = null;
			this.label1.AccessibleName = null;
			resources.ApplyResources(this.label1, "label1");
			this.label1.Font = null;
			this.label1.Name = "label1";
			this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			// 
			// btnRefresh
			// 
			this.btnRefresh.AccessibleDescription = null;
			this.btnRefresh.AccessibleName = null;
			resources.ApplyResources(this.btnRefresh, "btnRefresh");
			this.btnRefresh.BackgroundImage = null;
			this.btnRefresh.Font = null;
			this.btnRefresh.Name = "btnRefresh";
			this.toolTip1.SetToolTip(this.btnRefresh, resources.GetString("btnRefresh.ToolTip"));
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.AccessibleDescription = null;
			this.groupBox2.AccessibleName = null;
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.BackgroundImage = null;
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.clrMarker);
			this.groupBox2.Controls.Add(this.btnClearAll);
			this.groupBox2.Controls.Add(this.btnDelete);
			this.groupBox2.Controls.Add(this.olvMarkers);
			this.groupBox2.Font = null;
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
			// 
			// label3
			// 
			this.label3.AccessibleDescription = null;
			this.label3.AccessibleName = null;
			resources.ApplyResources(this.label3, "label3");
			this.label3.Font = null;
			this.label3.Name = "label3";
			this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
			// 
			// clrMarker
			// 
			this.clrMarker.AccessibleDescription = null;
			this.clrMarker.AccessibleName = null;
			resources.ApplyResources(this.clrMarker, "clrMarker");
			this.clrMarker.BackColor = System.Drawing.SystemColors.Window;
			this.clrMarker.BackgroundImage = null;
			this.clrMarker.Context = null;
			this.clrMarker.Font = null;
			this.clrMarker.ForeColor = System.Drawing.SystemColors.WindowText;
			this.clrMarker.Name = "clrMarker";
			this.clrMarker.ReadOnly = false;
			this.toolTip1.SetToolTip(this.clrMarker, resources.GetString("clrMarker.ToolTip"));
			this.clrMarker.Value = System.Drawing.Color.Tomato;
			this.clrMarker.ValueChanged += new System.EventHandler(this.clrMarker_ValueChanged);
			// 
			// btnClearAll
			// 
			this.btnClearAll.AccessibleDescription = null;
			this.btnClearAll.AccessibleName = null;
			resources.ApplyResources(this.btnClearAll, "btnClearAll");
			this.btnClearAll.BackgroundImage = null;
			this.btnClearAll.Font = null;
			this.btnClearAll.Name = "btnClearAll";
			this.toolTip1.SetToolTip(this.btnClearAll, resources.GetString("btnClearAll.ToolTip"));
			this.btnClearAll.UseVisualStyleBackColor = true;
			this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.AccessibleDescription = null;
			this.btnDelete.AccessibleName = null;
			resources.ApplyResources(this.btnDelete, "btnDelete");
			this.btnDelete.BackgroundImage = null;
			this.btnDelete.Font = null;
			this.btnDelete.Name = "btnDelete";
			this.toolTip1.SetToolTip(this.btnDelete, resources.GetString("btnDelete.ToolTip"));
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// olvMarkers
			// 
			this.olvMarkers.AccessibleDescription = null;
			this.olvMarkers.AccessibleName = null;
			resources.ApplyResources(this.olvMarkers, "olvMarkers");
			this.olvMarkers.AllColumns.Add(this.olvFrequency);
			this.olvMarkers.AllColumns.Add(this.olvPower);
			this.olvMarkers.AllColumns.Add(this.olvFrequency);
			this.olvMarkers.AllColumns.Add(this.olvPower);
			this.olvMarkers.AlternateRowBackColor = System.Drawing.Color.Empty;
			this.olvMarkers.BackgroundImage = null;
			this.olvMarkers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvFrequency,
            this.olvPower});
			this.olvMarkers.Font = null;
			this.olvMarkers.FullRowSelect = true;
			this.olvMarkers.Name = "olvMarkers";
			this.olvMarkers.ShowGroups = false;
			this.toolTip1.SetToolTip(this.olvMarkers, resources.GetString("olvMarkers.ToolTip"));
			this.olvMarkers.UseCompatibleStateImageBehavior = false;
			this.olvMarkers.View = System.Windows.Forms.View.Details;
			// 
			// olvFrequency
			// 
			this.olvFrequency.AspectName = null;
			this.olvFrequency.FillsFreeSpace = true;
			resources.ApplyResources(this.olvFrequency, "olvFrequency");
			// 
			// olvPower
			// 
			this.olvPower.AspectName = null;
			this.olvPower.FillsFreeSpace = true;
			resources.ApplyResources(this.olvPower, "olvPower");
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleDescription = null;
			this.btnCancel.AccessibleName = null;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.BackgroundImage = null;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = null;
			this.btnCancel.Name = "btnCancel";
			this.toolTip1.SetToolTip(this.btnCancel, resources.GetString("btnCancel.ToolTip"));
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.AccessibleDescription = null;
			this.btnOk.AccessibleName = null;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.BackgroundImage = null;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Font = null;
			this.btnOk.Name = "btnOk";
			this.toolTip1.SetToolTip(this.btnOk, resources.GetString("btnOk.ToolTip"));
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// CreateTraceMarkersDialog
			// 
			this.AcceptButton = this.btnOk;
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = null;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = null;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateTraceMarkersDialog";
			this.ShowInTaskbar = false;
			this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pwrMax)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pwrMin)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.olvMarkers)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown pwrMax;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown pwrMin;
		private System.Windows.Forms.Label label1;
		private ZedGraph.ZedGraphControl zgPower;
		private System.Windows.Forms.GroupBox groupBox2;
		private ControlUtils.ObjectListView.ObjectListView olvMarkers;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private ControlUtils.ObjectListView.OLVColumn olvFrequency;
		private ControlUtils.ObjectListView.OLVColumn olvPower;
		private Com.Windows.Forms.ColorPicker clrMarker;
		private System.Windows.Forms.Button btnClearAll;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}