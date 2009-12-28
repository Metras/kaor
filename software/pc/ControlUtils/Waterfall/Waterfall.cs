using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.Waterfall
{
    public partial class Waterfall : UserControl
    {
        public Waterfall()
        {
            InitializeComponent();
            curTrace = -1;
            SelectedTrace = -1;
            Fmax = 100050000;
            Fmin = 100000000;
            DisplayFmax = Fmax;
            DisplayFmin = Fmin;
            Fstep = 10;
            TraceCount = 10;
            Ymax = 0;
            Ymin = -110;
            MinColor = Color.Olive;
            MaxColor = Color.Red;
            vScrollBar1.ValueChanged += new EventHandler(vScrollBar1_ValueChanged);
        }



        int curTrace;            //текущая трасса
        int selectedTrace;          //выбранная мышкой трасса
        const int lineWidth = 5;    //ширина линии
        const int lineSpace = 10;    //расстояние между линиями
        Color[][] colors;           //цвета всех точек трасс

        public delegate void OnTraceSelectEventHandler(double[] pTrace);
        public event OnTraceSelectEventHandler OnTraceSelect;

        /// <summary>
        /// начать новую трассу
        /// </summary>
        public void BeginTrace()
        {
            if (!InvokeRequired)
            {
                if (curTrace < TraceCount - 1)
                {
                    curTrace++;
                }
                else
                {
                    //если трасс больше чем можно
                    //удаляем первую, сдвигаем остальные
                    for (int i = 0; i < curTrace; i++)
                    {
                        Power[i] = Power[i + 1];
                    }
                }
                Power[curTrace] = new double[PointCount];
                for (long i = 0; i < PointCount; i++)
                    Power[curTrace][i] = Ymin;
                //Power[curTrace][i] = Ymin + (Ymax - Ymin) * (i + 1) / (PointCount);
                ShowHideScrollBar();
                ResetColors();
                Invalidate();
            }
            else
            {
                Invoke(new BeginTraceDelegate(BeginTrace));
            }
        }

        delegate void BeginTraceDelegate();

        /// <summary>
        /// изменить мощность точки
        /// </summary>
        /// <param name="pFreq">частота</param>
        /// <param name="pPower">мощность</param>
        public void NewPoint(Int64 pFreq, double pPower)
        {
            if (!InvokeRequired)
            {
                if (pFreq <= Fmin || pFreq >= Fmax) //если выходит за границы частоты
                    return;
                if (pPower >= Ymax)
                    Power[curTrace][(pFreq - Fmin) / Fstep] = Ymax;
                if (pPower <= Ymin)
                    return;   //по умолчанию уже нарисован 0
                //если несколько, выбираем наибольшую
                Power[curTrace][(pFreq - Fmin) / Fstep] = Math.Max(pPower, Power[curTrace][(pFreq - Fmin) / Fstep]);
                ResetColors(curTrace, (pFreq - Fmin) / Fstep);
                Invalidate();
            }
            else
            {
                Invoke(new NewPointDelegate(NewPoint), pFreq, pPower);
            }
        }

        delegate void NewPointDelegate(Int64 pFreq, double pPower);
        

        /// <summary>
        /// очистить контрол, удалить все трассы
        /// </summary>
        public void Clear()
        {
            if (!InvokeRequired)
            {
                Power = new double[traceCount][];
                curTrace = -1;
                ShowHideScrollBar();
                ResetColors();
                Invalidate();
            }
            else
            {
                Invoke(new ClearDelegate(Clear));
            }
        }

        delegate void ClearDelegate();
 

        /// <summary>
        /// перерисовать всю картинку
        /// </summary>
        void ResetColors()
        {
            if (curTrace < 0)
            {
                return;
            }
            //создать массив заново
            colors = new Color[curTrace + 1][];
            int _pixelCount;
            _pixelCount = this.Width - MarginLeft - MarginRight;
            //меняется от 0 до 1
            //0 - минимум, 1 - максимум
            double _powerRate;
            for (int i = 0; i <= curTrace; i++)
            {
                colors[i] = new Color[_pixelCount];
                double _relat = (double)(DisplayFmax - DisplayFmin) / Fstep / (double)_pixelCount;
                for (int j = 0; j < _pixelCount; j++)  //прорисовываем каждую точку
                {
                    double _maxPowerRate = 0;
                    for (double l = (DisplayFmin - Fmin) / Fstep + (j * _relat);
                        (l < (DisplayFmin - Fmin) / Fstep + (j + 1) * _relat); l++)
                    {
                        if ((int)l < 0 || (int)l >= Power[i].Length)
                        {
                            _powerRate = 0.0;
                        }
                        else
                        {
                            _powerRate = (power[i][(int)l] - Ymin) / (Ymax - Ymin);
                        }
                        //выбираем наибольшую
                        _maxPowerRate = _powerRate > _maxPowerRate ? _powerRate : _maxPowerRate;
                    }
                    Color color = new Color();
                    color = Color.FromArgb(
                        (byte)(MinColor.R + ((MaxColor.R - MinColor.R) * _maxPowerRate)),
                        (byte)(MinColor.G + ((MaxColor.G - MinColor.G) * _maxPowerRate)),
                        (byte)(MinColor.B + ((MaxColor.B - MinColor.B) * _maxPowerRate)));
                    if (i == SelectedTrace)
                    {
                        color = Color.FromArgb(       //подсвечиваем
                            (byte)(127 + color.R / 2),
                            (byte)(127 + color.G / 2),
                            (byte)(127 + color.B / 2));
                    }
                    colors[i][j] = color;
                }
            }
        }
        /// <summary>
        /// перерисовать одну трассу
        /// </summary>
        /// <param name="pTrace"></param>
        void ResetColors(int pTrace)
        {
            int _pixelCount;  //ширина поля для рисования
            _pixelCount = this.Width - MarginLeft - MarginRight;
            //соотношение поля и количества часот для рисования
            double _relat = (double)(DisplayFmax - DisplayFmin) / Fstep / (double)_pixelCount;
            for (int j = 0; j < _pixelCount; j++)
            {
                double _maxPowerRate = 0;
                double _powerRate;
                for (double l = (DisplayFmin - Fmin) / Fstep + (j * _relat);
                    (l < (DisplayFmin - Fmin) / Fstep + (j + 1) * _relat); l++)
                {
                    if ((int)l < 0 || (int)l >= Power[pTrace].Length)
                    {
                        _powerRate = 0.0;
                    }
                    else
                    {
                        _powerRate = (power[pTrace][(int)l] - Ymin) / (Ymax - Ymin);
                    }
                    _maxPowerRate = _powerRate > _maxPowerRate ? _powerRate : _maxPowerRate;
                }
                Color color = new Color();
                color = Color.FromArgb(
                    (byte)(MinColor.R + ((MaxColor.R - MinColor.R) * _maxPowerRate)),
                    (byte)(MinColor.G + ((MaxColor.G - MinColor.G) * _maxPowerRate)),
                    (byte)(MinColor.B + ((MaxColor.B - MinColor.B) * _maxPowerRate)));
                if (pTrace == SelectedTrace)
                {
                    color = Color.FromArgb(
                        (byte)(127 + color.R / 2),
                        (byte)(127 + color.G / 2),
                        (byte)(127 + color.B / 2));
                }
                colors[pTrace][j] = color;
                //e.Graphics.DrawLine(new Pen(color), j, i * (lineSpace + lineWidth), j, i * (lineSpace + lineWidth) + lineWidth);
            }
        }
        /// <summary>
        /// изменить одну точку
        /// </summary>
        /// <param name="pTrace"></param>
        /// <param name="pFreq">номер точки в массиве</param>
        void ResetColors(int pTrace, long pFreq)
        {
            int _pixelCount;
            _pixelCount = this.Width - MarginLeft  - MarginRight;
            double _relat = (double)(DisplayFmax - DisplayFmin) / Fstep / (double)_pixelCount;
            double _powerRate;
            int j = (int)((pFreq - (DisplayFmin - Fmin) / Fstep) / _relat);
            if (j < 0 || j >= _pixelCount)
            {
                return;
            }
            double _maxPowerRate = 0;
            for (double l = (DisplayFmin - Fmin) / Fstep + (j * _relat);
                    (l < (DisplayFmin - Fmin) / Fstep + (j + 1) * _relat); l++)
            {
                if ((int)l < 0 || (int)l >= Power[pTrace].Length)
                {
                    _powerRate = 0.0;
                }
                else
                {
                    _powerRate = (power[pTrace][(int)l] - Ymin) / (Ymax - Ymin);
                }
                _maxPowerRate = _powerRate > _maxPowerRate ? _powerRate : _maxPowerRate;
            }
            Color color = new Color();
            color = Color.FromArgb(
                (byte)(MinColor.R + ((MaxColor.R - MinColor.R) * _maxPowerRate)),
                (byte)(MinColor.G + ((MaxColor.G - MinColor.G) * _maxPowerRate)),
                (byte)(MinColor.B + ((MaxColor.B - MinColor.B) * _maxPowerRate)));
            if (pTrace == SelectedTrace)
            {
                color = Color.FromArgb(
                    (byte)(127 + color.R / 2),
                    (byte)(127 + color.G / 2),
                    (byte)(127 + color.B / 2));
            }
            colors[pTrace][j] = color;
        }

        private void Waterfall_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, -(float)vScrollBar1.Value); //скроллинг
            for (int i = 0; i <= curTrace; i++)
            {
                if (colors == null)
                    return;
                for (int j = 0; j < colors[i].Length; j++)
                    e.Graphics.DrawLine(new Pen(colors[i][j]), j + MarginLeft, i * (lineSpace + lineWidth), j + MarginLeft, i * (lineSpace + lineWidth) + lineWidth);
            }
            e.Graphics.ResetTransform();
        }
        /// <summary>
        /// вызывает событие выделения трассы 
        /// </summary>
        /// <param name="pTrace"></param>
        void TraceSelectRaise(double[] pTrace)
        {
            if (OnTraceSelect != null)
                OnTraceSelect(pTrace);
        }
        /// <summary>
        /// показывает или прячет СкроллБар,
        /// в зависимости от размера картинки
        /// </summary>
        void ShowHideScrollBar()
        {
            if (PictureSize > Height)
            {
                vScrollBar1.Visible = true;
                vScrollBar1.Maximum = PictureSize - Height;
            }
            else
            {
                vScrollBar1.Visible = false;
                vScrollBar1.Value = 0;
            }
        }

        void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {            
            Invalidate();
        }
        /// <summary>
        /// выделение трассы мышкой
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {            
            base.OnMouseClick(e);
            SelectedTrace = -1;
            for (int i = 0; i <= curTrace; i++)  //выделение трассы
            {
                if (e.Y >= i * (lineSpace + lineWidth) && e.Y <= i * (lineSpace + lineWidth) + lineWidth)
                {
                    SelectedTrace = i;
                    TraceSelectRaise(power[i]);
                }
            }
            this.Focus();
            
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SelectedTrace = -1;
        }

        Int64 fmin;
        /// <summary>
        /// начальная частота
        /// </summary>
        public Int64 Fmin
        {
            get
            {

                return fmin;
            }
            set
            {
                if (value >= Fmax)
                    Fmax = value + 1;
                fmin = value;
            }
        }
        Int64 fmax;
        /// <summary>
        /// конечная частота
        /// </summary>
        public Int64 Fmax
        {
            get
            {
                return fmax;
            }
            set
            {
                if (value <= Fmin)
                    Fmin = value - 1;
                fmax = value;
            }
        }
        Int64 fstep;
        public Int64 Fstep
        {
            get
            {
                return fstep;
            }
            set
            {
                if (value <= 0)
                {
                    fstep = 1;
                    return;
                }
                fstep = value;
            }
        }
        /// <summary>
        /// количество точек (расстояние от мин до макс деленное на шаг)
        /// </summary>
        public Int64 PointCount
        {
            get
            {
                return (Fmax - Fmin) / Fstep;
            }
        }
        int traceCount;
        /// <summary>
        /// максимальное кол-во трасс, 
        /// изменение значения удалит все трассы
        /// </summary>
        int TraceCount
        {
            get
            {
                return traceCount;
            }
            set
            {
                traceCount = value;
                Power = new double[value][];
            }
        }
        double ymin;
        /// <summary>
        /// минимальная мощность
        /// </summary>
        public double Ymin
        {
            get
            {
                return ymin;
            }
            set
            {
                if (value >= Ymax)
                    Ymax = value + 1;
                for (int i = 0; i <= curTrace; i++)
                    for (long j = 0; j < PointCount; j++)
                    {
                        if (Power[i][j] < value)
                            Power[i][j] = value;
                    }
                ymin = value;
            }
        }
        double ymax;
        /// <summary>
        /// максимальная мощность
        /// </summary>
        public double Ymax
        {
            get
            {
                return ymax;
            }
            set
            {
                if (value <= Ymin)
                    Ymin = value - 1;
                for (int i = 0; i <= curTrace; i++)
                    for (long j = 0; j < PointCount; j++)
                    {
                        if (Power[i][j] > value)
                            Power[i][j] = value;
                    }
                ymax = value;
            }
        }
        Color minColor;
        /// <summary>
        /// цвет минимальной мощности
        /// </summary>
        public Color MinColor
        {
            get
            {
                return minColor;
            }
            set
            {
                minColor = value;
                ResetColors();
                Invalidate();
            }
        }
        Color maxColor;
        /// <summary>
        /// цвет максимальной мощности
        /// </summary>
        public Color MaxColor
        {
            get
            {
                return maxColor;
            }
            set
            {
                maxColor = value;
                ResetColors();
                Invalidate();
            }
        }
        double[][] power;
        double[][] Power
        {
            get
            {
                return power;
            }
            set
            {
                power = value;

            }
        }
        int SelectedTrace
        {
            get
            {
                return selectedTrace;
            }
            set
            {
                if (value < -1 || value > curTrace)
                    return;
                selectedTrace = value;
                ResetColors();
                Invalidate();
            }
        }
        int PictureSize
        {
            get
            {
                return (lineSpace + lineWidth) * curTrace + lineWidth;
            }
        }
        long displayFmax;
        /// <summary>
        /// первая частота, которая будет отображена на экране
        /// </summary>
        public long DisplayFmax
        {
            get
            {
                return displayFmax;
            }
            set
            {
                if (value <= displayFmin)
                    displayFmin = value - 1;
                displayFmax = value;
                ResetColors();
                Invalidate();
            }
        }
        long displayFmin;
        /// <summary>
        /// последняя частота, которая будет отображена на экране
        /// </summary>
        public long DisplayFmin
        {
            get
            {
                return displayFmin;
            }
            set
            {
                if (value >= displayFmax)
                    displayFmax = value - 1;
                displayFmin = value;
                ResetColors();
                Invalidate();
            }
        }
        int marginLeft;
        /// <summary>
        /// отступ слева от края контрола
        /// </summary>
        public int MarginLeft
        {
            get
            {
                return marginLeft;
            }
            set
            {
                marginLeft = value;
            }
        }
        int marginRight;
        /// <summary>
        /// отступ справа
        /// </summary>
        public int MarginRight
        {
            get
            {
                return marginRight;
            }
            set
            {
                marginRight = value;
            }
        }
    }
}
