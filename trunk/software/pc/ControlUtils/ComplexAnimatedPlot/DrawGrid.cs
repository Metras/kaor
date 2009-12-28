using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ControlUtils.ComplexAnimatePlot
{
    class DrawGrid
    {
        private int width;
        private int height;
        private ComplexAnimatePlot parent;



        public DrawGrid(ComplexAnimatePlot parent)
        {
            this.parent = parent;
     
        }

        /// <summary>
        /// отрисовка
        /// </summary>
        /// <param name="e"></param>
        public void Paint(Graphics e)
        {
            width = parent.Width;
            height = parent.Height;
            e.DrawLine(new Pen(this.parent.GridColor), 0, height / 2, width, height / 2);
            e.DrawLine(new Pen(this.parent.GridColor), width / 2, 0, width / 2, height);
        }


    }
}
