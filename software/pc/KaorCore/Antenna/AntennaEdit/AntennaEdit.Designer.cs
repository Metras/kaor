namespace KaorCore.Antenna.AntennaEdit
{
    partial class AntennaEdit
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AntennaEdit));
			this.lblName = new System.Windows.Forms.Label();
			this.lblDescr = new System.Windows.Forms.Label();
			this.lblMinFreq = new System.Windows.Forms.Label();
			this.lblMaxFreq = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.txtDescr = new System.Windows.Forms.TextBox();
			this.frqMin = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.frqMax = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.lblDirection = new System.Windows.Forms.Label();
			this.cmbState = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDir = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.txtLon = new System.Windows.Forms.TextBox();
			this.txtLat = new System.Windows.Forms.TextBox();
			this.cmbLon = new System.Windows.Forms.ComboBox();
			this.cmbLat = new System.Windows.Forms.ComboBox();
			this.grbxCoord = new System.Windows.Forms.GroupBox();
			this.chkReadOnly = new System.Windows.Forms.CheckBox();
			this.txtAlt = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.grbxCoord.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblName
			// 
			resources.ApplyResources(this.lblName, "lblName");
			this.lblName.Name = "lblName";
			// 
			// lblDescr
			// 
			resources.ApplyResources(this.lblDescr, "lblDescr");
			this.lblDescr.Name = "lblDescr";
			// 
			// lblMinFreq
			// 
			resources.ApplyResources(this.lblMinFreq, "lblMinFreq");
			this.lblMinFreq.Name = "lblMinFreq";
			// 
			// lblMaxFreq
			// 
			resources.ApplyResources(this.lblMaxFreq, "lblMaxFreq");
			this.lblMaxFreq.Name = "lblMaxFreq";
			// 
			// txtName
			// 
			resources.ApplyResources(this.txtName, "txtName");
			this.txtName.Name = "txtName";
			// 
			// txtDescr
			// 
			resources.ApplyResources(this.txtDescr, "txtDescr");
			this.txtDescr.Name = "txtDescr";
			// 
			// frqMin
			// 
			resources.ApplyResources(this.frqMin, "frqMin");
			this.frqMin.Frequency = ((long)(0));
			this.frqMin.Max = ((long)(9223372036854775807));
			this.frqMin.Min = ((long)(-9223372036854775808));
			this.frqMin.Name = "frqMin";
			// 
			// frqMax
			// 
			resources.ApplyResources(this.frqMax, "frqMax");
			this.frqMax.Frequency = ((long)(0));
			this.frqMax.Max = ((long)(9223372036854775807));
			this.frqMax.Min = ((long)(-9223372036854775808));
			this.frqMax.Name = "frqMax";
			// 
			// lblDirection
			// 
			resources.ApplyResources(this.lblDirection, "lblDirection");
			this.lblDirection.Name = "lblDirection";
			// 
			// cmbState
			// 
			resources.ApplyResources(this.cmbState, "cmbState");
			this.cmbState.Items.AddRange(new object[] {
            resources.GetString("cmbState.Items"),
            resources.GetString("cmbState.Items1"),
            resources.GetString("cmbState.Items2")});
			this.cmbState.Name = "cmbState";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtDir
			// 
			resources.ApplyResources(this.txtDir, "txtDir");
			this.txtDir.Name = "txtDir";
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// txtLon
			// 
			resources.ApplyResources(this.txtLon, "txtLon");
			this.txtLon.Name = "txtLon";
			// 
			// txtLat
			// 
			resources.ApplyResources(this.txtLat, "txtLat");
			this.txtLat.Name = "txtLat";
			// 
			// cmbLon
			// 
			resources.ApplyResources(this.cmbLon, "cmbLon");
			this.cmbLon.Items.AddRange(new object[] {
            resources.GetString("cmbLon.Items"),
            resources.GetString("cmbLon.Items1")});
			this.cmbLon.Name = "cmbLon";
			// 
			// cmbLat
			// 
			resources.ApplyResources(this.cmbLat, "cmbLat");
			this.cmbLat.Items.AddRange(new object[] {
            resources.GetString("cmbLat.Items"),
            resources.GetString("cmbLat.Items1")});
			this.cmbLat.Name = "cmbLat";
			// 
			// grbxCoord
			// 
			this.grbxCoord.Controls.Add(this.chkReadOnly);
			this.grbxCoord.Controls.Add(this.txtAlt);
			this.grbxCoord.Controls.Add(this.label4);
			this.grbxCoord.Controls.Add(this.label3);
			this.grbxCoord.Controls.Add(this.label2);
			this.grbxCoord.Controls.Add(this.cmbLon);
			this.grbxCoord.Controls.Add(this.cmbLat);
			this.grbxCoord.Controls.Add(this.txtLon);
			this.grbxCoord.Controls.Add(this.txtLat);
			resources.ApplyResources(this.grbxCoord, "grbxCoord");
			this.grbxCoord.Name = "grbxCoord";
			this.grbxCoord.TabStop = false;
			// 
			// chkReadOnly
			// 
			resources.ApplyResources(this.chkReadOnly, "chkReadOnly");
			this.chkReadOnly.Name = "chkReadOnly";
			this.chkReadOnly.UseVisualStyleBackColor = true;
			// 
			// txtAlt
			// 
			resources.ApplyResources(this.txtAlt, "txtAlt");
			this.txtAlt.Name = "txtAlt";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// AntennaEdit
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grbxCoord);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbState);
			this.Controls.Add(this.lblDirection);
			this.Controls.Add(this.frqMax);
			this.Controls.Add(this.frqMin);
			this.Controls.Add(this.txtDir);
			this.Controls.Add(this.txtDescr);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblMaxFreq);
			this.Controls.Add(this.lblMinFreq);
			this.Controls.Add(this.lblDescr);
			this.Controls.Add(this.lblName);
			this.Name = "AntennaEdit";
			this.grbxCoord.ResumeLayout(false);
			this.grbxCoord.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.Label lblMinFreq;
        private System.Windows.Forms.Label lblMaxFreq;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescr;
        private ControlUtils.FrequencyTextBox.FrequencyTextBox frqMin;
        private ControlUtils.FrequencyTextBox.FrequencyTextBox frqMax;
        private System.Windows.Forms.Label lblDirection;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDir;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.ComboBox cmbLon;
        private System.Windows.Forms.ComboBox cmbLat;
        private System.Windows.Forms.GroupBox grbxCoord;
        private System.Windows.Forms.TextBox txtAlt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}
