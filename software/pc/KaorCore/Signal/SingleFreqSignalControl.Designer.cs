namespace KaorCore.Signal
{
	partial class SingleFreqSignalControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleFreqSignalControl));
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.bndSignal = new System.Windows.Forms.BindingSource(this.components);
			this.label2 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.cmbSignalType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// textBox1
			// 
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Name", true));
			this.textBox1.Name = "textBox1";
			// 
			// bndSignal
			// 
			this.bndSignal.DataSource = typeof(KaorCore.Signal.SingleFreqSignal);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// textBox3
			// 
			resources.ApplyResources(this.textBox3, "textBox3");
			this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Description", true));
			this.textBox3.Name = "textBox3";
			// 
			// cmbSignalType
			// 
			resources.ApplyResources(this.cmbSignalType, "cmbSignalType");
			this.cmbSignalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSignalType.FormattingEnabled = true;
			this.cmbSignalType.Items.AddRange(new object[] {
            resources.GetString("cmbSignalType.Items"),
            resources.GetString("cmbSignalType.Items1"),
            resources.GetString("cmbSignalType.Items2"),
            resources.GetString("cmbSignalType.Items3")});
			this.cmbSignalType.Name = "cmbSignalType";
			this.cmbSignalType.SelectedIndexChanged += new System.EventHandler(this.cmbSignalType_SelectedIndexChanged);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// numericUpDown3
			// 
			resources.ApplyResources(this.numericUpDown3, "numericUpDown3");
			this.numericUpDown3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Frequency", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "0"));
			this.numericUpDown3.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown3.Maximum = new decimal(new int[] {
            -1294967296,
            0,
            0,
            0});
			this.numericUpDown3.Name = "numericUpDown3";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Power", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.numericUpDown1.DecimalPlaces = 1;
			resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
			this.numericUpDown1.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
			// 
			// SingleFreqSignalControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.numericUpDown3);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbSignalType);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Name = "SingleFreqSignalControl";
			this.VisibleChanged += new System.EventHandler(this.RangeSignalControl_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.ComboBox cmbSignalType;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.BindingSource bndSignal;
		private System.Windows.Forms.NumericUpDown numericUpDown3;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
	}
}
