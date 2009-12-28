using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Balloon.NET;

namespace ControlUtils.Balloon
{
    public partial class Balloon : BalloonWindow
    {
        public Balloon()
        {
            InitializeComponent();
        }

        private void Balloon_Shown(object sender, EventArgs e)
        {
            tmrHide.Stop();
            tmrHide.Start();
        }

        private void tmrHide_Tick(object sender, EventArgs e)
        {
            this.Hide();
            tmrHide.Stop();
        }

        
       
    }
}
