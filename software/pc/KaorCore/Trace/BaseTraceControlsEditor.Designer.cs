namespace KaorCore.Trace
{
	partial class BaseTraceControlsEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseTraceControlsEditor));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.propTraceControl = new System.Windows.Forms.PropertyGrid();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnReinit = new System.Windows.Forms.Button();
			this.btnDefaultSignalParams = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDefaultSignal = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.propTraceControl);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// propTraceControl
			// 
			resources.ApplyResources(this.propTraceControl, "propTraceControl");
			this.propTraceControl.Name = "propTraceControl";
			// 
			// btnOk
			// 
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnReinit
			// 
			resources.ApplyResources(this.btnReinit, "btnReinit");
			this.btnReinit.Name = "btnReinit";
			this.btnReinit.UseVisualStyleBackColor = true;
			this.btnReinit.Click += new System.EventHandler(this.btnReinit_Click);
			// 
			// btnDefaultSignalParams
			// 
			resources.ApplyResources(this.btnDefaultSignalParams, "btnDefaultSignalParams");
			this.btnDefaultSignalParams.Name = "btnDefaultSignalParams";
			this.btnDefaultSignalParams.UseVisualStyleBackColor = true;
			this.btnDefaultSignalParams.Click += new System.EventHandler(this.btnDefaultSignalParams_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// cmbDefaultSignal
			// 
			this.cmbDefaultSignal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDefaultSignal.FormattingEnabled = true;
			this.cmbDefaultSignal.Items.AddRange(new object[] {
            resources.GetString("cmbDefaultSignal.Items")});
			resources.ApplyResources(this.cmbDefaultSignal, "cmbDefaultSignal");
			this.cmbDefaultSignal.Name = "cmbDefaultSignal";
			this.cmbDefaultSignal.SelectedIndexChanged += new System.EventHandler(this.cmbDefaultSignal_SelectedIndexChanged);
			// 
			// BaseTraceControlsEditor
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.cmbDefaultSignal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnDefaultSignalParams);
			this.Controls.Add(this.btnReinit);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BaseTraceControlsEditor";
			this.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.PropertyGrid propTraceControl;
		private System.Windows.Forms.Button btnReinit;
		private System.Windows.Forms.Button btnDefaultSignalParams;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDefaultSignal;
	}
}