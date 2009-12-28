using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.AntennaPropsDialog
{
    public partial class AntennaPropsDialog : Form
    {
        public AntennaPropsDialog()
        {
            InitializeComponent();
            //this.LostFocus += new EventHandler(AntennaPropsDialog_LostFocus);
            //this.FormClosing += new FormClosingEventHandler(AntennaPropsDialog_FormClosing);
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.OK;
        }
    }
}
