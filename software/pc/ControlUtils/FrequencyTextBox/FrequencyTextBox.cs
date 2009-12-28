using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.FrequencyTextBox
{
    public partial class FrequencyTextBox : TextBox
    {
        //выделять ли весь текст по щелчку
        //(true если этим кликом контрол получил фокус)
        bool focusClick;


        public FrequencyTextBox()
        {
            InitializeComponent();
            Text = "0";
            focusClick = true;
            Min = Int64.MinValue;
            Max = Int64.MaxValue;
        }

        protected Int64 Conv(string pText)
        {
            if (pText.Length > 0)
            {
                char _last = pText[pText.Length - 1];
                if (_last == 'k' || _last == 'K')
                {
                    try
                    {
                        return Convert.ToInt64(1000 * Convert.ToDouble(pText.Substring(0, pText.Length - 1)));
                    }
                    catch (FormatException)
                    {                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    return 0;
                }
                else if (_last == 'm' || _last == 'M')
                {
                    try
                    {
                        return Convert.ToInt64(1000000 * Convert.ToDouble(pText.Substring(0, pText.Length - 1)));
                    }
                    catch (FormatException)
                    {                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    return 0;
                }
                else if (_last == 'g' || _last == 'G')
                {
                    try
                    {
                        return Convert.ToInt64(1000000000 * Convert.ToDouble(pText.Substring(0, pText.Length - 1)));
                    }
                    catch (FormatException)
                    {
                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    return 0;
                }
                //else if (_last == ',')
				else if (_last == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || 
					_last == CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0])
                {
                    try
                    {
                        return Convert.ToInt64(pText.Substring(0, pText.Length - 1));
                    }
                    catch (FormatException)
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        return (long)Convert.ToDouble(pText);
                    }
                    catch (FormatException)
                    {                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    catch (OverflowException)
                    {
                        return Int64.MaxValue;
                    }
                    return 0;
                }
            }
            else return 0;
        }

        
        
        protected override void OnTextChanged(EventArgs e)
        {
            if (this.Text.Length > 0)
            {
                char _last = this.Text[this.Text.Length - 1];
                if (_last == 'k' || _last == 'K')
                {
                    try
                    {
                        Frequency = Convert.ToInt64(1000 * Convert.ToDouble(this.Text.Substring(0, this.Text.Length - 1)));
                        this.SelectAll();
                    }
                    catch (FormatException)
                    {
                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    catch (OverflowException)
                    {
                        Frequency = Int64.MaxValue;
                        this.SelectAll();
                    }

                }
                else if (_last == 'm' || _last == 'M')
                {
                    try
                    {
                        Frequency = Convert.ToInt64(1000000 * Convert.ToDouble(this.Text.Substring(0, this.Text.Length - 1)));
                        this.SelectAll();
                    }
                    catch (FormatException)
                    {                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    catch (OverflowException)
                    {
                        Frequency = Int64.MaxValue;
                        this.SelectAll();
                    }
                }
                else if (_last == 'g' || _last == 'G')
                {
                    try
                    {
                        Frequency = Convert.ToInt64(1000000000 * Convert.ToDouble(this.Text.Substring(0, this.Text.Length - 1)));
                        this.SelectAll();
                    }
                    catch (FormatException)
                    {
                        
                        this.Frequency = 0;
                        this.Focus();
                        this.SelectAll();
                    }
                    catch (OverflowException)
                    {
                        Frequency = Int64.MaxValue;
                        this.SelectAll();
                    }
                }
                //else if (_last == ',')  //на запятую внимания не обращаем
				else if(_last == CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] || 
					_last == CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0])
                { }
                else
                {
                    long _value = Conv(this.Text);
                    if (_value > Max)
                        frequency = Max;
                    else if (_value < Min)
                        frequency = Min;
                    else
                        frequency = _value;
                }
            }
            else
            {
                Frequency = 0;
                this.SelectAll();
            }
            base.OnTextChanged(e);
        }


        protected override void OnLostFocus(EventArgs e)
        {
            if (this.Text == String.Empty)
            {
                this.Frequency = 0;
            }
            focusClick = true;
            Frequency = Conv(Text);
            base.OnLostFocus(e);
        }
        /// <summary>
        /// защита от ввода неверных символов
        /// </summary>
        /// <param name="e"></param>       
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            char _c = e.KeyChar;
            if (_c != 'k' && _c != 'K' &&
                _c != 'm' && _c != 'M' &&
                _c != 'g' && _c != 'G' &&
				_c != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] &&
				_c != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0] &&
                _c != (char)8 &&            //backspace
                _c != (char)3 &&            //Ctrl-C
                _c != (char)22 &&           //Ctrl-V
                _c != (char)24 &&           //Ctrl-X
                (_c < '0' || _c > '9'))
                e.KeyChar = (char)0;
            base.OnKeyPress(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        
        protected override void OnMouseClick(MouseEventArgs e)
        {            
            if (focusClick && this.SelectionLength == 0)
            {
                this.SelectAll();
                focusClick = false;
            }            
            base.OnMouseClick(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                this.SelectAll();
                this.focusClick = false;
            }
            base.OnKeyUp(e);
        }

        Int64 frequency;

        [Browsable(true)]
        public Int64 Frequency
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
                Text = frequency.ToString(CultureInfo.InvariantCulture);
                if (value != frequency)     //равно мин или макс
                    SelectAll();
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
    }
}
