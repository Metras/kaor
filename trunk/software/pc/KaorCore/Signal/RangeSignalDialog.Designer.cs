namespace KaorCore.Signal
{
	partial class RangeSignalDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RangeSignalDialog));
			this.frequencyTextBox1 = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.bndSignal = new System.Windows.Forms.BindingSource(this.components);
			this.frequencyTextBox2 = new ControlUtils.FrequencyTextBox.FrequencyTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.btnClearHits = new System.Windows.Forms.Button();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			this.SuspendLayout();
			// 
			// frequencyTextBox1
			// 
			this.frequencyTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Frequency", true));
			this.frequencyTextBox1.Frequency = ((long)(0));
			resources.ApplyResources(this.frequencyTextBox1, "frequencyTextBox1");
			this.frequencyTextBox1.Max = ((long)(3000000000));
			this.frequencyTextBox1.Min = ((long)(0));
			this.frequencyTextBox1.Name = "frequencyTextBox1";
			// 
			// bndSignal
			// 
			this.bndSignal.DataSource = typeof(KaorCore.Signal.RangeSignal);
			// 
			// frequencyTextBox2
			// 
			this.frequencyTextBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Band", true));
			this.frequencyTextBox2.Frequency = ((long)(3000000000));
			resources.ApplyResources(this.frequencyTextBox2, "frequencyTextBox2");
			this.frequencyTextBox2.Max = ((long)(9223372036854775807));
			this.frequencyTextBox2.Min = ((long)(-9223372036854775808));
			this.frequencyTextBox2.Name = "frequencyTextBox2";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Pmin", true));
			resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
			this.numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numericUpDown1.Name = "numericUpDown1";
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
			// textBox1
			// 
			this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Name", true));
			resources.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// textBox2
			// 
			this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "Description", true));
			resources.ApplyResources(this.textBox2, "textBox2");
			this.textBox2.Name = "textBox2";
			// 
			// comboBox1
			// 
			this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "SignalType", true));
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items"),
            resources.GetString("comboBox1.Items1"),
            resources.GetString("comboBox1.Items2"),
            resources.GetString("comboBox1.Items3")});
			resources.ApplyResources(this.comboBox1, "comboBox1");
			this.comboBox1.Name = "comboBox1";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// textBox3
			// 
			this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bndSignal, "HitsCount", true));
			resources.ApplyResources(this.textBox3, "textBox3");
			this.textBox3.Name = "textBox3";
			// 
			// btnClearHits
			// 
			resources.ApplyResources(this.btnClearHits, "btnClearHits");
			this.btnClearHits.Name = "btnClearHits";
			this.btnClearHits.UseVisualStyleBackColor = true;
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bndSignal, "Pmax", true));
			resources.ApplyResources(this.numericUpDown2, "numericUpDown2");
			this.numericUpDown2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.numericUpDown2.Minimum = new decimal(new int[] {
            140,
            0,
            0,
            -2147483648});
			this.numericUpDown2.Name = "numericUpDown2";
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.btnOk, "btnOk");
			this.btnOk.Name = "btnOk";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// RangeSignalDialog
			// 
			this.AcceptButton = this.btnOk;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.numericUpDown2);
			this.Controls.Add(this.btnClearHits);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.frequencyTextBox2);
			this.Controls.Add(this.frequencyTextBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RangeSignalDialog";
			((System.ComponentModel.ISupportInitialize)(this.bndSignal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ControlUtils.FrequencyTextBox.FrequencyTextBox frequencyTextBox1;
		private ControlUtils.FrequencyTextBox.FrequencyTextBox frequencyTextBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button btnClearHits;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource bndSignal;
	}
}