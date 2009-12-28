using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.PowerScale
{
    public partial class PowerScale : UserControl
    {
        public PowerScale()
        {
            InitializeComponent();
            Min = -110;
            Max = 0;
            Width = (int)(Height * widthRatio);
            Arrow = new Pen(Color.Black);
        }
        const double firstAngl = 1.81;  //крайнее левое положение стрелки рад
        const double lastAngl = 1.32;   //крайнее правое
        const float firstY_scale = 3;   //насколько низко точка вращения стрелки
        const float widthRatio = 1.9f;  //соотношение длины и ширины контрола
        const int numOfTick = 16;       //кол-во тиков таймера до установления стрелки
        

        
        double power;
        /// <summary>
        /// Отображаемая мощность
        /// </summary>
        public double Power
        {
            get
            {
                return power;
            }
            set
            {
                SetPower(value);
            }
        }

        delegate void SetPowerDelegate(double pValue);

        void SetPower(double pValue)
        {
            if (!InvokeRequired)
            {
                power = pValue;
                tick = 0;
                tmrSet.Enabled = true;
                tmrFall.Enabled = false;
                Invalidate();
            }
            else
            {
                BeginInvoke(new SetPowerDelegate(SetPower), pValue);
            }
        }

        double min;
        /// <summary>
        /// минимальная мощность
        /// </summary>        
        public double Min
        {
            get
            {
                return min;
            }
            set
            {
                if (value > Max)
                    Max = value + 1;
                min = value;
            }
        }
        double max;
        /// <summary>
        /// максимальная мощность
        /// </summary>
        public double Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value < Min)
                    Min = value - 1;
                max = value;
            }
        }


        Pen arrow;
        Pen Arrow
        {
            get
            {
                return arrow;
            }
            set
            {
                arrow = value;
            }
        }
        /// <summary>
        /// цвет стрелки
        /// </summary>
        public Color ArrowColor
        {
            get
            {
                return Arrow.Color;
            }
            set
            {
                Arrow.Color = value;
            }
        }
        /// <summary>
        /// размер стрелки
        /// </summary>
        public float ArrowWidth
        {
            get
            {
                return Arrow.Width;
            }
            set
            {
                Arrow.Width = value;
            }
        }

        [Description("Задает скорость спада мощности")]
        public int TimerFall
        {
            get
            {
                return tmrFall.Interval;
            }
            set
            {
                tmrFall.Interval = value;
            }
        }

        [Description("Задает скорость установки мощности")]
        public int TimerSet
        {
            get
            {
                return tmrSet.Interval;
            }
            set
            {
                tmrSet.Interval = value;
            }
        }

        double curPower;
        /// <summary>
        /// Мощность, показываемая стрелкой
        /// </summary>
        double CurPower
        {
            get
            {
                return curPower;
            }
            set
            {
                curPower = value;
                Invalidate();
            }
        }

        
        float arrowDrawTopBound_percentage;
        /// <summary>
        /// верхняя граница прорисовки стрелки
        /// </summary>
        [DefaultValue(0)]
        public float ArrowDrawTopBound_percentage
        {
            get
            {
                return arrowDrawTopBound_percentage;
            }
            set
            {
                 arrowDrawTopBound_percentage = value;
            }
        }
        [DefaultValue(100)]
        float arrowDrawSize_percentage;
        /// <summary>
        /// размер области прорисовки
        /// </summary>
        public float ArrowDrawSize_percentage
        {
            get
            {
                return arrowDrawSize_percentage;
            }
            set
            {
                arrowDrawSize_percentage = value;
            }
        }

        
        //сохранение пропорций
        protected override void OnResize(EventArgs e)
        {
            //this.Width = (int)(this.Height * widthRatio);
            base.OnResize(e);
        }

        
        //отрисовка стрелки
        private void PowerScale_Paint(object sender, PaintEventArgs e)
        {
            float _x1 = this.Width / 2;
            float _y1 = this.Height * firstY_scale;
            float _arrowLength = _y1 - this.Height / 4;
            double _powerRate = (CurPower - Min) / (Max - Min);
            if (_powerRate < 0)
                _powerRate = 0;
            if (_powerRate > 1)
                _powerRate = 1;
            double _angl = (float)(firstAngl + _powerRate * (lastAngl - firstAngl));
            float _x2 = _x1 + _arrowLength * (float)Math.Cos(_angl);
            float _y2 = _y1 - _arrowLength * (float)Math.Sin(_angl);
            e.Graphics.Clip = new Region(new RectangleF(0, ArrowDrawTopBound_percentage * Height / 100, Width, ArrowDrawSize_percentage * Height / 100));
            e.Graphics.DrawLine(Arrow, _x1, _y1, _x2, _y2);
        }
        //спад показываемой мощности
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurPower <= Min)
            {
                tmrFall.Enabled = false;
                return;
            }
            CurPower -= (Max - Min) * 0.01;
            Invalidate();
        }

        int tick;   //текущий тик
        //установка мощности
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (tick < numOfTick)
            {
                if (Power < CurPower)
                    CurPower = (3 * CurPower + Power) / 4;
                else
                    CurPower = (2 * CurPower + Power) / 3;
                tick++;
                Invalidate();
            }
            else
            {
                tmrSet.Enabled = false;
                tmrFall.Enabled = true;
            }
        }
    }
}