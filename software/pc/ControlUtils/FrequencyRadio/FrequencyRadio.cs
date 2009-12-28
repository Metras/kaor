using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ControlUtils.FrequencyRadio
{
    public partial class FrequencyRadio : MaskedTextBox
    {
        public delegate void FrequencyChangedHandler(Int64 _newFrequency);
        public event FrequencyChangedHandler FrequencyChanged;

        AutoResetEvent evtFreq;
        object needRedrawLock;

        public FrequencyRadio()
        {
            InitializeComponent();
            this.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            this.FrequencyChanged += new FrequencyChangedHandler(OnFrequencyChanged);
            this.Min = 20000000;
            this.Max = 3000000000;
            evtFreq = new AutoResetEvent(false);
            this.Frequency = 20000000;
            base.Mask = "0.000.000.000";
            this.Text = "0020000000";
            
            needRedrawLock = new object();
            bgwRedraw.RunWorkerAsync();
        }

        protected void RaiseFrequencyChanged()
        {
			if (!InvokeRequired)
			{
				tmr_ChangedDelay.Stop();
				tmr_ChangedDelay.Start();
			}
			else
				Invoke(new MethodInvoker(RaiseFrequencyChanged));
        }

        //изменение цифры колесиком
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.SelectionStart <= 12)
            {
				long _newFreq = frequency;

                //запомнить текущую позицию курсора
                int _prePos = this.SelectionStart;
                //рассчет текущего разряда
                //с пропуском запятых
                int _position = (int)Math.Ceiling((12 - this.SelectionStart) / 4.0 * 3);
                if (e.Delta > 0)
                    _newFreq += (Int64)Math.Pow(10, _position);
                if (e.Delta < 0)
                {
                    if ((Int64)Math.Pow(10, _position) <= _newFreq)
                        _newFreq -= (Int64)Math.Pow(10, _position);
                }
                
				//Text = Frequency.ToString(CultureInfo.InvariantCulture);
				InternalFrequency = _newFreq;

                //восстановить предыдущую позицию курсора
                this.SelectionStart = _prePos;
                this.SelectionLength = 1;
            }
            base.OnMouseWheel(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (SelectionStart >= 13)
                SelectionStart = 12;
            //не выделять запятые
            if (((this.SelectionStart - 1) % 4) == 0)
                this.SelectionStart--;
            //выделить один символ
            this.SelectionLength = 1;

        }
        //защита от выделения нескольких символов
        //(выход за границы контрола с зажатой кнопкой)
        protected override void OnMouseLeave(EventArgs e)
        {
            this.SelectionLength = 1;
            base.OnMouseLeave(e);
        }
        //защита от выделения нескольких символов (двойной щелчок)
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this.SelectionLength = 1;
        }
        //движение стрелками
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Down)
            {
                if (this.SelectionStart > 0)
                {
                    this.SelectionStart--;
                    if (SelectionStart >= 13)
                        SelectionStart = 12;
                    this.SelectionLength = 1;
                    
                }
            }
            if (e.KeyData == Keys.Left || e.KeyData == Keys.Up)
            {
                if (this.SelectionStart > 0)
                {
                    this.SelectionStart--;
                    this.SelectionLength = 1;
                    if (((this.SelectionStart - 1) % 4) == 0)
                        this.SelectionStart--;
                }
            }
            
            base.OnKeyDown(e);
        }
        
        //защита от ввода "пустых" символов пробелом
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
                e.KeyChar = '0';
            base.OnKeyPress(e);
            if (((this.SelectionStart - 1) % 4) == 0)
                this.SelectionStart++;
            this.SelectionLength = 1;

            this.InternalFrequency = Convert.ToInt64(this.Text);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.SelectionLength = 1;
            if (((this.SelectionStart - 1) % 4) == 0)
                this.SelectionStart++;
            base.OnKeyUp(e);
        }

        protected virtual void OnFrequencyChanged(Int64 _newFreq)
        {
        }
               
        string mask;

        //защита от изменения маски
        [Browsable(false)]
        new public string Mask
        {
            get
            {
                return mask;
            }
            set
            {
                mask = value;
            }
        }


        long frequency;

		long InternalFrequency
		{
			get
			{
				return frequency;
			}

			set
			{
				Frequency = value;
				RaiseFrequencyChanged();
			}
		}

        /// <summary>
        /// текущее значение частоты
        /// </summary>
        [Browsable(true)]
        public long Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                if (value > Max)
                    frequency = Max;
                else if (value < Min)
                    frequency = Min;
                else
                    frequency = value;

                ChangeText(frequency.ToString());

                ////RaiseFrequencyChanged();

                //FrequencyChanged(frequency);                
            }
        }

        //смена текста без изменения частоты
        private void ChangeText(string pValue)
        {
            if (!InvokeRequired)
            {
                int _curPos = SelectionStart;
                txt = pValue;
                while (txt.Length < 10)
                    txt = "0" + txt;
                SelectionStart = _curPos;
                SelectionLength = 1;
				Text = txt;
                //needRedraw = true;
            }
            else
            {
                Invoke(new SetTextDelegate(ChangeText), pValue);
            }
        }

        string txt;
        bool needRedraw;

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                SetText(value);
            }
        }

        delegate void SetTextDelegate(string pValue);

        void SetText(string pValue)
        {
            if (!InvokeRequired)
            {
                base.Text = pValue;
                try
                {
                    frequency = Convert.ToInt64(pValue);
                }
                catch
                {
                }
                while (base.Text.Length < 10)
                    base.Text = "0" + base.Text;
            }
            else
            {
                Invoke(new SetTextDelegate(SetText), pValue);
            }
        }

        Int64 min;
        /// <summary>
        /// минимальное значение
        /// </summary>
        public Int64 Min
        {
            get
            {
                return min;
            }
            set
            {
                if (value > Max)
                    Max = value;
                if (Frequency < value)
                    Frequency = value;
                min = value;
            }
        }
        Int64 max;
        /// <summary>
        /// максмимальное значение
        /// </summary>
        public Int64 Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value < Min)
                    Min = value;
                max = value;
                if (Frequency > value)
                    Frequency = value;
            }
        }

        public int DelayChange
        {
            get
            {
                return tmr_ChangedDelay.Interval;
            }
            set
            {
                tmr_ChangedDelay.Interval = value;
            }
        }

        private void tmr_ChangedDelay_Tick(object sender, EventArgs e)
        {
            FrequencyChanged(frequency);
            tmr_ChangedDelay.Stop();
        }

        private void bgwRedraw_DoWork(object sender, DoWorkEventArgs e)
        {
            bool _evtRes;

            while (!bgwRedraw.CancellationPending)
            {
                /// Ждем запуска отрисовки
                _evtRes = evtFreq.WaitOne(500, true);
                
                if (_evtRes || needRedraw == true)
                {
                    lock (needRedrawLock)
                    {
                        needRedraw = false;
                    }

                    /// Рисуем
                    Text = txt;
                }
            }
        }
    }
}
