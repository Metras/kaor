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
using System.Text;
using System.Windows.Forms;
using System.Xml;

using ControlUtils.AntennaPropsDialog;
using ControlUtils.ObjectListView;
using ControlUtils.FrequencyRadio;

using KaorCore.Antenna;
using KaorCore.RPU;

using RPUICOMR8500.Audio;
using RPUICOMR8500.I18N;
using RPUICOMR8500.PowerMeter;


namespace RPUICOMR8500
{
	public partial class R8500Control : UserControl
	{
		RPUR8500 rpu;
        int mem1_ind, mem2_ind, mem3_ind;
        //CalibrationForm.CalibrationForm calf;
		//AntennaList lstAntennas;

        public R8500Control(RPUR8500 pRPU)
		{
			rpu = pRPU;

			InitializeComponent();

/*            calf = new RPUICOMR8500.CalibrationForm.CalibrationForm(this);
            calf.Show();
            calf.Activate();*/
            cmb_average.SelectedIndex = 0;

			mem1_ind = -1;
			mem2_ind = -1;
			mem3_ind = -1;

			if (rpu != null)
			{
				rpu.PowerMeter.OnNewPowerMeasure += new KaorCore.RPU.NewPowerMeasure(PowerMeter_OnNewPowerMeasure);
				rpu.OnBaseFrequencyChanged += new KaorCore.RPU.BaseFrequencyChanged(rpu_OnBaseFrequencyChanged);
				rpu.OnRPUParamsChanged += new RPUR8500.RPUParamsChangedDelegate(rpu_OnRPUParamsChanged);
				rpu.OnModeChanged += new BaseRPU.ModeChangedDelegate(rpu_OnModeChanged);

				InitializeAntennaListView();

				rpu.Demodulator.Control.Dock = DockStyle.Fill;
				pnlAudioControl.Controls.Clear();
				pnlAudioControl.Controls.Add(rpu.Demodulator.Control);

			}
		}

		private void InitializeAntennaListView()
		{
			lstAntennas.MouseDoubleClick += new MouseEventHandler(lstAntennas_MouseDoubleClick);
			lstAntennas.MouseClick += new MouseEventHandler(lstAntennas_MouseClick);
			olvTitle.AspectGetter = delegate(object pO) { return ((BaseAntenna)pO).Name; };
			//olvLocation.AspectGetter = delegate(object pO) { return ((GPSCoordinates)((BaseAntenna)pO).Coordinates).ToString(); };

			lstAntennas.RowFormatter = delegate(OLVListItem pItem)
			{
				/// Формирование цвета фона в зависимости от типа сигнала
				BaseAntenna _antenna = (BaseAntenna)pItem.RowObject;
				Color _backColor;

				switch (_antenna.State)
				{
					case EAntennaState.OK:
						_backColor = Color.White;
						break;

					case EAntennaState.BAD:
						_backColor = Color.Yellow;
						break;

					case EAntennaState.FAULT:
						_backColor = Color.Red;
						break;

					default:
						_backColor = Color.Red;
						break;
				}

				pItem.BackColor = _backColor;
				foreach (ListViewItem.ListViewSubItem _subItem in pItem.SubItems)
				{
					_subItem.BackColor = _backColor;
				}
			};

			lstAntennas.SetObjects(rpu.Antennas);
		}

		void lstAntennas_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			OLVListItem _item = lstAntennas.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseAntenna _antenna = _item.RowObject as BaseAntenna;

			if (_antenna == null)
				return;

			AntennaPropsDialog _dlg = new AntennaPropsDialog();
			_dlg.lbl_AntennaName.Text = _antenna.Name;
			_dlg.lbl_Description.Text = _antenna.Description;
			_dlg.lbl_GPS.Text = _antenna.Coordinates.ToString();
			_dlg.StartPosition = FormStartPosition.Manual;
			_dlg.Location = PointToScreen(lstAntennas.Location);
			_dlg.Top -= (_dlg.Height + 15);
			//_dlg.Left -= (_dlg.Width / 3);
			_dlg.ShowDialog();
		}

		void lstAntennas_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			OLVListItem _item = lstAntennas.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			IAntenna _antenna = _item.RowObject as IAntenna;
			if (_antenna == null)
				return;

			lstAntennas_OnAntennaSelect(_antenna);

		}

		/// <summary>
		/// Установка вида режима сканирования
		/// </summary>
		void SetScanModeControls()
		{
			if (!InvokeRequired)
			{
				lstAntennas.Enabled = false;
				chkAtt10.Enabled = false;
				chkAtt20.Enabled = false;
				cmbMod.Enabled = false;
				cmb_average.Enabled = false;
				frequencyRadio1.ReadOnly = true;
				//mem_button1.Enabled = false;
				//mem_button2.Enabled = false;
				//_mem_button3.Enabled = false;
				groupBox1.Enabled = false;
			}
			else
				Invoke(new MethodInvoker(SetScanModeControls));
		}


		void SetFreeModeControls()
		{
			if (!InvokeRequired)
			{
				lstAntennas.Enabled = true;
				chkAtt10.Enabled = true;
				chkAtt20.Enabled = true;
				cmbMod.Enabled = true;
				cmb_average.Enabled = true;
				frequencyRadio1.ReadOnly = false;
				//mem_button1.Enabled = false;
				//mem_button2.Enabled = false;
				//_mem_button3.Enabled = false;
				groupBox1.Enabled = true;
			}
			else
				Invoke(new MethodInvoker(SetFreeModeControls));
		}

		void rpu_OnModeChanged(RPUMode pMode)
		{
			switch (pMode)
			{
				case RPUMode.Free:
					SetFreeModeControls();
					Enabled = true;
					break;

				case RPUMode.Scan:
					SetScanModeControls();
					Enabled = true;
					break;

				case RPUMode.Off:
					Enabled = false;
					break;

				case RPUMode.AudioRecord:
					SetFreeModeControls();
					Enabled = true;
					break;

				default:
					Enabled = true;
					break;
			}
		}      
        
        /// <summary>
        /// Приемник
        /// </summary>
        public RPUR8500 RPU
		{
			get { return rpu; }
			set 
			{ 
			}
		}

		void UpdateControl()
		{
			//cmbBand.SelectedItem = rpu.Demodulator.FilterBand.ToString(CultureInfo.InvariantCulture);
			switch (rpu.Demodulator.CurrentModulation)
			{
				case EAudioModulationType.FM:
					cmbMod.SelectedIndex = 0;
					break;

				case EAudioModulationType.WFM:
					cmbMod.SelectedIndex = 1;
					break;

				case EAudioModulationType.AM:
					cmbMod.SelectedIndex = 2;
					break;

				case EAudioModulationType.CW:
					cmbMod.SelectedIndex = 3;
					break;

				case EAudioModulationType.FM_narrow:
					cmbMod.SelectedIndex = 4;
					break;

				case EAudioModulationType.AM_narrow:
					cmbMod.SelectedIndex = 5;
					break;

				case EAudioModulationType.AM_wide:
					cmbMod.SelectedIndex = 6;
					break;

				case EAudioModulationType.LSB:
					cmbMod.SelectedIndex = 7;
					break;

				case EAudioModulationType.USB:
					cmbMod.SelectedIndex = 8;
					break;

				default:
					cmbMod.SelectedIndex = 0;
					break;
			}
			//cmbMod.SelectedItem = rpu.Demodulator.CurrentModulation.ToString(CultureInfo.InvariantCulture);
			//cmbPowerFilterBand.SelectedItem = rpu.PowerMeter.FilterBand.ToString(CultureInfo.InvariantCulture);
			//cmbAverageTime.SelectedItem = rpu.PowerMeter.AverageTime.ToString(CultureInfo.InvariantCulture);

			lstAntennas.BuildList(false);
		}

		void rpu_OnRPUParamsChanged(RPUR8500 pRPU)
		{
			UpdateControl();
		}

		void lstAntennas_OnAntennaSelect(IAntenna pAntenna)
		{
			rpu.SwitchAntenna(pAntenna);
		}

		void rpu_OnBaseFrequencyChanged(long pBaseFreq)
		{
			frequencyRadio1.Frequency = pBaseFreq;
		}

        //измерена мощность
		delegate void ProcessPowerMeasureDelegate(float pPower);

		void ProcessPowerMeasure(float pPower)
		{
			if (!InvokeRequired)
			{
				graphPowerScale1.Power = pPower;
				lblPower.Text = pPower.ToString("F0", CultureInfo.InvariantCulture);
				//lblPower.Text = String.Format("F0", pPower);
			}
			else
				Invoke(new ProcessPowerMeasureDelegate(ProcessPowerMeasure), pPower);
		}
		void PowerMeter_OnNewPowerMeasure(IRPU pRPU, long pFrequency, float pPower)
		{
            R8500PowerMeter _pwm = rpu.PowerMeter as R8500PowerMeter;
            if (_pwm != null)
            {
				ProcessPowerMeasure(pPower);
            }
        }
 

        //изменяет частоту приемника
		private void frequencyRadio1_FrequencyChanged(long pNewFrequency)
		{
			if(rpu != null)
				rpu.BaseFreq = pNewFrequency;
		}
        //выбор модуляции
        private void ModulationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (rpu.Mode != RPUMode.Free && rpu.Mode != RPUMode.ZeroSpan && rpu.Mode != RPUMode.AudioRecord)
				return;

			switch (cmbMod.SelectedIndex)
			{
				case 0:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.FM;
					break;

				case 1:
	                if (rpu.BaseFreq < 30000000)
		            {
			            MessageBox.Show(Locale.wfm_error,
				            Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
					    cmbMod.SelectedIndex = -1;
					}
					else
						rpu.Demodulator.CurrentModulation = EAudioModulationType.WFM;
					break;

				case 2:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.AM;
					break;

				case 3:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.CW;
					break;

				case 4:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.FM_narrow;
					break;

				case 5:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.AM_narrow;
					break;

				case 6:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.AM_wide;
					break;

				case 7:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.LSB;
					break;

				case 8:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.USB;
					break;

				default:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.FM;
					break;
			}
        }

        //запоминание модуляции на ModulationComboBox
        private void mem_button1_Click(object sender, EventArgs e)
        {
            if (chk_memory.Checked)
            {
                mem1_ind = cmbMod.SelectedIndex;
                chk_memory.Checked = false;
            }
            else
            {
				if (mem1_ind == -1)
					MessageBox.Show(Locale.empty_mem,
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					cmbMod.SelectedIndex = mem1_ind; 
            }
        }

        private void mem_button2_Click(object sender, EventArgs e)
        {
            if (chk_memory.Checked)
            {
                mem2_ind = cmbMod.SelectedIndex;
                chk_memory.Checked = false;
            }
            else
            {
				if (mem2_ind == -1)
					MessageBox.Show(Locale.empty_mem,
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
	                cmbMod.SelectedIndex = mem2_ind;
            }
        }

        private void mem_button3_Click(object sender, EventArgs e)
        {
            if (chk_memory.Checked)
            {
                mem3_ind = cmbMod.SelectedIndex;
                chk_memory.Checked = false;
            }
            else
            {
				if (mem3_ind == -1)
					MessageBox.Show(Locale.empty_mem,
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
	                cmbMod.SelectedIndex = mem3_ind;
            }
        }

        //переключение аттенуатора
        private void chkAtt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAtt10.Checked)
            {
                if (chkAtt20.Checked)
                {
                    rpu.Attenuator(RPUR8500.EAttenuator.dB30);
                }
                else
                {
                    rpu.Attenuator(RPUR8500.EAttenuator.dB10);
                }
            }
            else
            {
                if (chkAtt20.Checked)
                {
                    rpu.Attenuator(RPUR8500.EAttenuator.dB20);
                }
                else
                {
                    rpu.Attenuator(RPUR8500.EAttenuator.OFF);
                }
            }
        }


        //переключение времени усреднения
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            int _aver;
			if (!int.TryParse(cmb_average.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _aver))
            {
                MessageBox.Show("Неверный числовой формат", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmb_average.SelectedIndex = 0;
            }
            else
            {
                if (rpu != null)
                    rpu.PowerMeter.AverageTime = _aver;
            }
        }

          
	}
}
