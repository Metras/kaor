using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace ControlUtils.ComplexAnimatePlot
{
    class DrawAnimate
    {
        private ComplexAnimatePlot parent;
        private Timer timer;

        private double[] real;
        private double[] imag;

        private int curentIndex;

        private SolidBrush pointBrush;
        private Pen linePen;

        private struct scomplex
        {
            public double Real;
            public double Image;
        }

        private Queue<scomplex> drawQueue;

        public DrawAnimate(ComplexAnimatePlot parent)
        {
            this.parent = parent;
            pointBrush = new SolidBrush(Color.Black);
            linePen = new Pen(Color.Blue,3);
            curentIndex = 0;
            drawQueue = new Queue<scomplex>();
        }

        /// <summary>
        /// Запуск отрисовки с начала
        /// передаем новые масивы
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imag"></param>
        public void Start(double[] real, double[] imag)
        {
            this.real = real;
            this.imag = imag;
            curentIndex = 0;
            pointBrush = new SolidBrush(parent.PointColor);
            linePen = new Pen(parent.LineColor,3);
        }


        /// <summary>
        /// отрисовка
        /// </summary>
        /// <param name="e"></param>
        public void Paint(Graphics e)
        {
            if ((real==null)||(imag==null)) return;
            if ((real.Count()==0)||(imag.Count()==0)) return;

            scomplex lastitem;
            lastitem.Image = 0;
            lastitem.Real = 0;
            bool f = true;
            int alfa = parent.FadeOut;
            while (drawQueue.Count > 0)
            {
                pointBrush.Color = Color.FromArgb(alfa * 255 / parent.FadeOut, parent.PointColor);
                scomplex item = drawQueue.Dequeue();

                if (parent.EnableLine)
                {
                    if (f)
                    {
                        lastitem = item;
                        f = false;
                        
                    }
                    else
                    {
                        linePen.Color = Color.FromArgb(alfa*255/parent.FadeOut, parent.LineColor);
                        e.DrawLine(linePen, getDrawReal(lastitem.Real),
                                   getDrawImage(lastitem.Image),
                                   getDrawReal(item.Real),
                                   getDrawImage(item.Image));


                        
                    }

                }

                e.FillRectangle(pointBrush, getDrawReal(item.Real) - 1,
                getDrawImage(item.Image) - 1,
                5, 5);
                lastitem = item;
                
                if (alfa < 1)
                {
                    alfa = 1;
                }
                alfa--;
            }







        }

        /// <summary>
        /// Обработка события таймера
        /// формируем буфер для отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Tick(object sender, EventArgs e)
        {
            int curent;

            for (int i = 0; i < parent.FadeOut; i++)
            {
                curent = curentIndex - i;
                if (curent<0)continue;
                if (curent>=real.Count()) continue;
                drawQueue.Enqueue(new scomplex() { Image = imag[curent], Real = real[curent] });
            }

            ///т.к. масивы должны быть одной длины то
            /// проверяем по первому
            if (curentIndex >= (real.Count() + parent.FadeOut)-2)
            {
                curentIndex = -1;
            }
            curentIndex++;

            parent.Invalidate();
        }

        /// <summary>
        /// пересчет значения в экранные
        /// </summary>
        /// <param name="real"></param>
        /// <returns></returns>
        private int getDrawReal(double real)
        {
            int ret = (int)((real+parent.RealMax)* parent.Width / (parent.RealMax*2));            
            return ret;
        }

        /// <summary>
        /// пересчет значения в экранные
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private int getDrawImage(double image)
        {
            int ret =(int)((image + parent.ImagMax) * parent.Height / (parent.ImagMax*2));

            return ret;
        }

    }
}
