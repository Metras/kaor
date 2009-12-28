// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1.  Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.
// 2.  Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
// 3.  Neither the name of CJSC NII STT ("NII STT") nor the names of
//     its contributors may be used to endorse or promote products derived
//     from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY APPLE AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL APPLE OR ITS CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlUtils.Balloon;

using KaorCore.Antenna;
#if false
namespace RPURPV3.AntennaList
{
    public partial class RPV3AntennaList : UserControl
    {
        List<IAntenna> antennas;
        Point mouse;
        ControlUtils.AntennaPropsDialog.AntennaPropsDialog bll;

        public RPV3AntennaList()
        {
            InitializeComponent();
            antennas = new List<IAntenna>();
            AntennaList.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            bll = new ControlUtils.AntennaPropsDialog.AntennaPropsDialog();
            bll.MouseDown += new MouseEventHandler(AntennaList_MouseDown);
            bll.LostFocus += new EventHandler(listBox1_LostFocus);
        }

        void listBox1_LostFocus(object sender, EventArgs e)
        {
            bll.Hide();
        }


        void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index < 0)
                return;
            if (AntennaList.Items[e.Index] as IAntenna != null)
            {
                IAntenna _ant = AntennaList.Items[e.Index] as IAntenna;
                if (e.Index == AntennaList.SelectedIndex)
                {
                    if (antennas[e.Index].State != EAntennaState.FAULT)
                        e.Graphics.FillRectangle(SystemBrushes.Highlight,
                            0, AntennaList.ItemHeight * e.Index,
                            AntennaList.Width, AntennaList.ItemHeight);
                    else
                        e.Graphics.FillRectangle(Brushes.LightBlue, 0, AntennaList.ItemHeight * e.Index,
                        AntennaList.Width, AntennaList.ItemHeight);
                    Brush _fontColor = Brushes.Black;
                    if (antennas[e.Index].State == EAntennaState.BAD)
                        _fontColor = Brushes.Gray;
                    else if (antennas[e.Index].State == EAntennaState.FAULT)
                        _fontColor = Brushes.Black;
                    else if (antennas[e.Index].State == EAntennaState.OK)
                        _fontColor = Brushes.White;
                    Font _font = AntennaList.Font;
                    if (e.Index == SelectedAntenna)
                        _font = new Font(AntennaList.Font, FontStyle.Bold);
                    e.Graphics.DrawString(_ant.Name, _font, _fontColor,
                         0, AntennaList.ItemHeight * e.Index);

                }
                else
                {
                    if (antennas[e.Index].State == EAntennaState.FAULT)
                        e.Graphics.FillRectangle(Brushes.Red, 0, AntennaList.ItemHeight * e.Index,
                            AntennaList.Width, AntennaList.ItemHeight);
                    Brush _fontColor = Brushes.Black;
                    if (antennas[e.Index].State == EAntennaState.BAD)
                        _fontColor = Brushes.Gray;
                    else if (antennas[e.Index].State == EAntennaState.FAULT)
                        _fontColor = Brushes.White;
                    else if (antennas[e.Index].State == EAntennaState.OK)
                        _fontColor = Brushes.Black;
                    Font _font = AntennaList.Font;
                    if (e.Index == SelectedAntenna)
                        _font = new Font(AntennaList.Font, FontStyle.Bold);
                    e.Graphics.DrawString(_ant.Name, _font, _fontColor,
                         0, AntennaList.ItemHeight * e.Index);
                }
            }
        }

		[Browsable(false)]
		public List<IAntenna> Antennas
		{
			get { return antennas; }
			set
			{
				antennas.Clear();

				if (value == null)
					return;

				foreach (IAntenna _ant in value)
				{
					AddAntenna(_ant);
				}
			}
		}

		delegate void SetVScrollBarDelegate(bool pVisible);

		void SetVScrollBar(bool pVisible)
		{
			if (!vScrollBar1.InvokeRequired)
			{
				vScrollBar1.Visible = pVisible;
			}
			else
				vScrollBar1.Invoke(new SetVScrollBarDelegate(SetVScrollBar), pVisible);
		}

        public void AddAntenna(IAntenna antenna)
        {
            antennas.Add(antenna);
            AntennaList.Items.Add(antenna);
            AntennaList.Height = Math.Max(this.Height, AntennaList.Items.Count * AntennaList.ItemHeight) + 6;
            if (AntennaList.Height > this.Height + 7)
            {
				SetVScrollBar(true);

            }
        }

        public void RemoveAntenna(IAntenna pAntenna)
        {
            int _ind = antennas.IndexOf(pAntenna);
            antennas.RemoveAt(_ind);
            AntennaList.Items.RemoveAt(_ind);
            AntennaList.Height = Math.Max(this.Height, AntennaList.Items.Count * AntennaList.ItemHeight) + 6;
            if (AntennaList.Height <= this.Height + 7)
            {
                vScrollBar1.Visible = false;
            }
        }

        int selectedAntenna;
        public int SelectedAntenna
        {
            get
            {
                return selectedAntenna;
            }
            set
            {
                selectedAntenna = value;
                RaiseSelectAntenna(antennas[value]);
                AntennaList.Invalidate();
            }
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = e.Location;
        }

        public delegate void SelectAntennaEventHandler(IAntenna pAntenna);
        public event SelectAntennaEventHandler OnAntennaSelect;
        void RaiseSelectAntenna(IAntenna pAntenna)
        {
            if (OnAntennaSelect != null)
                OnAntennaSelect(pAntenna);
        }


        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && mouse.Y < AntennaList.ItemHeight * AntennaList.Items.Count)
            {
                mouse = e.Location;
                contextMenuStrip1.Show(AntennaList ,e.Location);
            }
        }

        private void oKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int _ind = mouse.Y / AntennaList.ItemHeight;
            antennas[_ind].State = EAntennaState.OK;
            Invalidate();
            AntennaList.Invalidate();
        }

        private void bADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int _ind = mouse.Y / AntennaList.ItemHeight;
            antennas[_ind].State = EAntennaState.BAD;
            Invalidate();
            AntennaList.Invalidate();
        }

        private void fAULTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int _ind = mouse.Y / AntennaList.ItemHeight;
            antennas[_ind].State = EAntennaState.FAULT;
            Invalidate();
            AntennaList.Invalidate();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            SelectedAntenna = AntennaList.SelectedIndex;
        }

        string GPStoString(KaorCore.Base.GPSCoordinates pGPS)
        {
            string _gps = "";
            _gps += ((int)pGPS.Lon).ToString(CultureInfo.InvariantCulture);
            _gps += '\u00BA';
            _gps += (Math.Truncate(pGPS.Lon) / 100 * 60).ToString(CultureInfo.InvariantCulture);
            _gps += '\'';
#if false
            if (pGPS.EW == KaorCore.Base.EGPSEastWest.East)
                _gps += " W ";
            else
                _gps += " E ";
#endif
            _gps += ((int)pGPS.Lat).ToString(CultureInfo.InvariantCulture);
            _gps += '\u00BA';
            _gps += (Math.Truncate(pGPS.Lat) / 100 * 60).ToString(CultureInfo.InvariantCulture);
            _gps += '\'';
#if false
            if (pGPS.SN == KaorCore.Base.EGPSNorthSouth.North)
                _gps += " N ";
            else
                _gps += " S ";
#endif
            return _gps;
        }

        private void AntennaList_MouseDown(object sender, MouseEventArgs e)
        {
            bll.Hide();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            AntennaList.Top = -(int)((AntennaList.Height - this.Height) *
                (double)vScrollBar1.Value / (double)(vScrollBar1.Maximum - 8));
            AntennaList.Invalidate();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int _ind = mouse.Y / AntennaList.ItemHeight;

            IAntenna _ant = antennas[_ind];
            bll.lbl_AntennaName.Text = _ant.Name;
            bll.lbl_Description.Text = _ant.Description;
            bll.lbl_GPS.Text = GPStoString(_ant.Coordinates);
            bll.StartPosition = FormStartPosition.Manual;
            bll.Location = AntennaList.PointToScreen(mouse);
            bll.Top -= (bll.Height + 15);
            bll.Left -= (bll.Width / 3);
            bll.Show(); 
        }

        private void AntennaList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AntennaList.Invalidate();
        }

        private void RPV3AntennaList_Resize(object sender, EventArgs e)
        {
            AntennaList.Height = Math.Max(this.Height, AntennaList.Items.Count * AntennaList.ItemHeight) + 6;
            if (AntennaList.Height > this.Height + 7)
            {
                vScrollBar1.Visible = true;
            }
            else
            {
                vScrollBar1.Visible = false;
            }
        }
    }
}
#endif