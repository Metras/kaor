using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.GraphPowerScale
{
    public partial class GraphPowerScale : UserControl
    {
        public GraphPowerScale()
        {
            InitializeComponent();
        }

        private void GraphPowerScale_Paint(object sender, PaintEventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphPowerScale));
            Image _img = ((System.Drawing.Image)(resources.GetObject("$this.Scale"))); //картинка заполненной шкалы
            //коэффициент мощности
            //(какую часть данная мощность составляет от максимальной
            //и какая часть шкалы будет заполненной)
            double _powerRate = (power - Min) / (Max - Min);
            if (_powerRate < 0)   //(мощность ниже минимума)
                _powerRate = 0;
            if (_powerRate > 1)   //(больше максимума)
                _powerRate = 1;
            //количество пикселей, на которых шкала будет заполнена
            int _drawingWidth = (int)(this.Width * _powerRate); 
            //рисуем часть заполненной шкалы поверх пустой
            e.Graphics.DrawImageUnscaledAndClipped(_img, new Rectangle(0, 0, 
                _drawingWidth, this.Height));
            
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

        /// <summary>
        /// устанавливает мощность, перерисовывает соответствующую часть картинки
        /// </summary>
        /// <param name="pValue"></param>
        void SetPower(double pValue)
        {
            if (!InvokeRequired)
            {
                double _prePowerRate = (power - Min) / (Max - Min);
                if (_prePowerRate < 0)
                    _prePowerRate = 0;
                if (_prePowerRate > 1)
                    _prePowerRate = 1;
                int _preDrawingWidth = (int)(this.Width * _prePowerRate); //предыдущее положение шкалы

                power = pValue;

                double _powerRate = (power - Min) / (Max - Min);
                if (_powerRate < 0)
                    _powerRate = 0;
                if (_powerRate > 1)
                    _powerRate = 1;
                int _drawingWidth = (int)(this.Width * _powerRate); //текущее положение
                Invalidate(new Rectangle(Math.Min(_drawingWidth, _preDrawingWidth), 0,
                    Math.Abs(_drawingWidth - _preDrawingWidth), this.Height));
            }
            else
            {
                BeginInvoke(new SetPowerDelegate(SetPower), pValue);
            }
        }
    }
}
