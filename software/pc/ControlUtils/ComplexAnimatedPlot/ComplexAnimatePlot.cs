using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.ComplexAnimatePlot
{
    public partial class ComplexAnimatePlot : UserControl
    {
        private DrawGrid grid;
        private DrawAnimate animate;
        public ComplexAnimatePlot()
        {
            initDrawObjects();
            InitializeComponent();
            defaultProperty();
            DoubleBuffered = true;
            
            timerAnimate.Tick+=new EventHandler(animate.Tick);
        }

        private void defaultProperty()
        {
            RealMax = 100;
            ImagMax = 100;
            TimeOut = 1000;
            FadeOut = 1; 
            EnableLine = false;
            BackColor = Color.White;
            GridColor = Color.Black;
            PointColor = Color.Black;
            LineColor = Color.Blue;
        }

        private void initDrawObjects()
        {
            grid = new DrawGrid(this);
            animate = new DrawAnimate(this);

        }

        /// <summary>
        /// Отрисовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplexAnimatePlot_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            grid.Paint(e.Graphics);
            animate.Paint(e.Graphics);
        }

        /// <summary>
        /// Отрисовка и анимация комплексной плоскости
        /// ----------------------------------------------------
        /// входные массивы мнимых и действительных частей чисел 
        /// должны иметь одинаковую длину
        /// </summary>
        /// <param name="Real">массив действительных частей</param>
        /// <param name="Imag">массив мнимых частей</param>
        public void Draw(double[] Real, double[] Imag)
        {
            if ((Real.Length==0)||(Imag.Length==0))
            {
                timerAnimate.Enabled = false;
                return;
            }            
            animate.Start(Real,Imag);
            timerAnimate.Enabled = true;
        }

        /// <summary>
        /// Максимальная действительная часть
        /// </summary>
        [DefaultValue(100)]
        public double RealMax
        {
            get; set;
        }

        /// <summary>
        /// Максимальная мнимая часть
        /// </summary>
        [DefaultValue(100)]
        public double ImagMax
        {
            get; set;
        }

        /// <summary>
        /// Задержка отрисовки следующей точки
        /// в миллисекундах
        /// </summary>
        [DefaultValue(1000)]
        public int TimeOut
        {
            get
            {
                return timerAnimate.Interval; 
            } 
            set
            {
                timerAnimate.Interval = value;
            }
        }

        private int fadeOut;
        /// <summary>
        /// Длина шлейфа исчезновения точек
        /// </summary>
        [DefaultValue(1)]
        public int FadeOut
        {
            get
            {
                return fadeOut;
            }
            set
            {
                if (value<1)
                {
                    value = 1;
                }
                fadeOut = value;
            }
        }

        /// <summary>
        /// Соединять смежные точки линиями
        /// </summary>
        [DefaultValue(false)]
        public bool EnableLine
        {
            get; set;
        }

        private Color backColor;

        /// <summary>
        /// Цвет фона
        /// </summary>
        [DefaultValue(typeof(Color),"White")]
        public override Color BackColor
        {
            get { return backColor; }
            set 
            {
                backColor = value;
                Invalidate();
            }
        }

        private Color gridColor;

        /// <summary>
        /// Цвет сетки - перекрестия
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        public Color GridColor
        {
            get { return gridColor; }
            set
            {
                gridColor = value;
                Invalidate();
            }
        }

        private Color pointColor;

        /// <summary>
        /// Цвет точек
        /// </summary>
        [DefaultValue(typeof(Color), "Black")]
        public Color PointColor
        {
            get { return pointColor; }
            set
            {
                pointColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Цвет линий которые соединяют точки
        /// </summary>
        [DefaultValue(typeof(Color), "Blue")]
        public Color LineColor
        {
            get; set;
        }

        private readonly Size minSize = new Size(100,100);
        public override Size MinimumSize
        {
            get
            {                
                return base.MinimumSize;
            }
            set
            {
                if (value.Height<minSize.Height)
                {
                    value.Height = minSize.Height; 
                }
                if (value.Width < minSize.Width)
                {
                    value.Width = minSize.Width;
                }
                base.MinimumSize = value;
            }
        }


        protected override void OnSizeChanged(EventArgs e)
        {
           base.OnSizeChanged(e);
            base.Invalidate();
        }

        

    }
}
