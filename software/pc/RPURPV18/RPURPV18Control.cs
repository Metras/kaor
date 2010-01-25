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
// THIS SOFTWARE IS PROVIDED BY NII STT AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL NII STT OR ITS CONTRIBUTORS BE LIABLE FOR
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
using System.Threading;
using System.Windows.Forms;

using ControlUtils.AntennaPropsDialog;
using ControlUtils.FrequencyRadio;
using ControlUtils.ObjectListView;

using KaorCore.Antenna;
using KaorCore.Base;
using KaorCore.RPU;
using RPURPV18.Audio;
using RPURPV18.I18N;
using RPURPV18.SignalConverter;
using RPURPV18.SignalConverterManager;
//using RPURPV18.AntennaList;

namespace RPURPV18
{
	public partial class RPURPV18Control : UserControl
	{
		CRPURPV18 rpu;
		//AntennaList antennaList;

		int creatorId;

		public RPURPV18Control(CRPURPV18 pRPU)
		{
			creatorId = Thread.CurrentThread.ManagedThreadId;
			InitializeComponent();

			rpu = pRPU;

			if (rpu.Mode == RPUMode.Off)
				Enabled = false;

			this.Dock = DockStyle.Fill;

			if (rpu != null)
			{
				/// Установка нового приемника
				//audioRecorderControl1.Demodulator = (RPV18AudioDemodulator)rpu.Demodulator;
				rpu.PowerMeter.OnNewPowerMeasure += new KaorCore.RPU.NewPowerMeasure(PowerMeter_OnNewPowerMeasure);
				rpu.OnBaseFrequencyChanged += new KaorCore.RPU.BaseFrequencyChanged(rpu_OnBaseFrequencyChanged);
				rpu.OnModeChanged += new CRPURPV18.ModeChangedDelegate(rpu_OnModeChanged);
				rpu.OnRPUParamsChanged += new CRPURPV18.RPUParamsChangedDelegate(rpu_OnRPUParamsChanged);
				rpu.OnSignalConverterChanged += new EventHandler(rpu_OnSignalConverterChanged);
				// Заполнение контролов параметрами, которые поддерживаются текущим РПУ
				
				pnlAudioControl.Controls.Clear();
				rpu.Demodulator.Control.Dock = DockStyle.Fill;
				pnlAudioControl.Controls.Add(rpu.Demodulator.Control);

				//injectorConnectIndicator.UsedInjector = rpu.UsedInjector;

				InitializeAntennaListView();
				UpdateControl();
			}
		}

		private void InitializeAntennaListView()
		{
			lstAntennas.MouseDoubleClick += new MouseEventHandler(lstAntennas_MouseDoubleClick);
			lstAntennas.MouseClick += new MouseEventHandler(lstAntennas_MouseClick);
			olvName.AspectGetter = delegate(object pO) { return ((BaseAntenna)pO).Name; };
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

			antennaList_OnAntennaSelect(_antenna);
			
		}

		void rpu_OnRPUParamsChanged(CRPURPV18 pRPU)
		{
			if (!InvokeRequired)
			{
				cmbPowerFilterBand.SelectedIndexChanged -= cmbPowerFilterBand_SelectedIndexChanged;
				cmbBand.SelectedIndexChanged -= cmbBand_SelectedIndexChanged;
				cmbMod.SelectedIndexChanged -= cmbMod_SelectedIndexChanged;
				cmbAverageTime.SelectedIndexChanged -= cmbAverageTime_SelectedIndexChanged;
				
				UpdateControl();

				cmbPowerFilterBand.SelectedIndexChanged += cmbPowerFilterBand_SelectedIndexChanged;
				cmbBand.SelectedIndexChanged += cmbBand_SelectedIndexChanged;
				cmbMod.SelectedIndexChanged += cmbMod_SelectedIndexChanged;
				cmbAverageTime.SelectedIndexChanged += cmbAverageTime_SelectedIndexChanged;
			}
			else 
				Invoke(new CRPURPV18.RPUParamsChangedDelegate(rpu_OnRPUParamsChanged), pRPU);
		}

		bool signalConverterComboLoaded = false;

		void UpdateSignalConverterCombo()
		{
			if (!signalConverterComboLoaded)
			{
				// Загрузка списка конвертеров сигнала
				cmbSignalConverter.DataSource = BaseSignalConverterManager.ConvertersArray;
				signalConverterComboLoaded = true;
				cmbSignalConverter.SelectedItem = rpu.SignalConverter;
			}
		}

		void UpdateAntennaList()
		{
			if (!lstAntennas.InvokeRequired)
			{
				lstAntennas.BuildList(false);
			}
			else
				lstAntennas.Invoke(new MethodInvoker(UpdateAntennaList));

		}

		void UpdateControl()
		{
			if (!InvokeRequired)
			{
				switch (rpu.Demodulator.FilterBand)
				{
					case 3000:
						cmbBand.SelectedIndex = 0;
						break;

					case 6000:
						cmbBand.SelectedIndex = 1;
						break;

					case 15000:
						cmbBand.SelectedIndex = 2;
						break;

					case 50000:
						cmbBand.SelectedIndex = 3;
						break;

					case 230000:
						cmbBand.SelectedIndex = 4;
						break;

					default:
						cmbBand.SelectedIndex = 0;
						break;
				}

				switch (rpu.Demodulator.CurrentModulation)
				{
					case EAudioModulationType.AM:
						cmbMod.SelectedIndex = 0;
						break;

					case EAudioModulationType.FM:
						cmbMod.SelectedIndex = 1;
						break;

					case EAudioModulationType.CW:
						cmbMod.SelectedIndex = 2;
						break;

					case EAudioModulationType.LSB:
						cmbMod.SelectedIndex = 3;
						break;

					case EAudioModulationType.USB:
						cmbMod.SelectedIndex = 4;
						break;

					default:
						cmbMod.SelectedIndex = 0;
						break;
				}

				switch (rpu.PowerMeter.FilterBand)
				{
					case 1000:
						cmbPowerFilterBand.SelectedIndex = 0;
						break;

					case 3000:
						cmbPowerFilterBand.SelectedIndex = 1;
						break;

					case 7500:
						cmbPowerFilterBand.SelectedIndex = 2;
						break;

					case 10000:
						cmbPowerFilterBand.SelectedIndex = 3;
						break;

					case 30000:
						cmbPowerFilterBand.SelectedIndex = 4;
						break;

					case 100000:
						cmbPowerFilterBand.SelectedIndex = 5;
						break;

					case 120000:
						cmbPowerFilterBand.SelectedIndex = 6;
						break;

					case 280000:
						cmbPowerFilterBand.SelectedIndex = 7;
						break;

					default:
						cmbPowerFilterBand.SelectedIndex = 0;
						break;

				}

				switch (rpu.PowerMeter.AverageTime)
				{
					case 10:
						cmbAverageTime.SelectedIndex = 0;
						break;

					case 20:
						cmbAverageTime.SelectedIndex = 1;
						break;

					case 50:
						cmbAverageTime.SelectedIndex = 2;
						break;

					case 100:
						cmbAverageTime.SelectedIndex = 3;
						break;

					case 250:
						cmbAverageTime.SelectedIndex = 4;
						break;

					case 500:
						cmbAverageTime.SelectedIndex = 5;
						break;

					case 1000:
						cmbAverageTime.SelectedIndex = 6;
						break;

					default:
						cmbAverageTime.SelectedIndex = 2;
						break;

				}

				UpdateAntennaList();
				//antennaList.Antennas = rpu.Antennas;
				UpdateSignalConverterCombo();
			}
			else
				Invoke(new MethodInvoker(UpdateControl));
		}

		void antennaList_OnAntennaSelect(IAntenna pAntenna)
		{
			if (rpu != null)
				rpu.SwitchAntenna(pAntenna);
		}

		private void btnOn_Click(object sender, EventArgs e)
		{
			rpu.SwitchOn();
		}

		/// <summary>
		/// Отработка контрола в соответствии с изменением режима работы приемника
		/// </summary>
		/// <param name="pMode"></param>
		void rpu_OnModeChanged(RPUMode pMode)
		{
			switch (pMode)
			{
				case RPUMode.Free:
					Enabled = true;
					/// ПРиемник свободен
					/// 
					SetFreeModeControls();
					break;

				case RPUMode.Scan:
					Enabled = true;
					/// Приемник сканирует
					/// 
					SetScanModeControls();

					break;

				case RPUMode.Off:
					Enabled = false;
					break;

				default:
					Enabled = true;
					/// Приемник делает неизвестно что
					/// 
					break;
			}
		}

		private void SetScanModeControls()
		{
			if (!InvokeRequired)
			{
				if (Thread.CurrentThread.ManagedThreadId != creatorId)
					throw new InvalidOperationException();
				//audioRecorderControl1.Enabled = false;
				frequencyRadio1.ReadOnly = true;
				cmbAverageTime.Enabled = false;
				cmbBand.Enabled = false;
				cmbMod.Enabled = false;
				cmbPowerFilterBand.Enabled = false;
				groupBox1.Enabled = false;
				groupBox2.Enabled = false;
				groupBox4.Enabled = false;
				chkATT10dB.Enabled = false;
				chkATT20dB.Enabled = false;
				grpAntennas.Enabled = false;
				//lstAntennas.Enabled = false;

				//// Установка цвета индикатора частоты
				//frequencyRadio1.ForeColor = Color.Tomato;
				//frequencyRadio1.BackColor = Color.LightBlue;
				//this.Enabled = false;
			}
			else
				Invoke(new MethodInvoker(SetScanModeControls));
		}

		private void SetFreeModeControls()
		{
			if (!InvokeRequired)
			{
				//audioRecorderControl1.Enabled = true;
				frequencyRadio1.ReadOnly = false;
				cmbAverageTime.Enabled = true;
				cmbBand.Enabled = true;
				cmbMod.Enabled = true;
				cmbPowerFilterBand.Enabled = true;
				groupBox1.Enabled = true;
				groupBox2.Enabled = true;
				groupBox4.Enabled = true;
				chkATT10dB.Enabled = true;
				chkATT20dB.Enabled = true;
				grpAntennas.Enabled = true;
				//lstAntennas.Enabled = true;

				//frequencyRadio1.ForeColor = Color.LimeGreen;
				//this.Enabled = true;
			}
			else
				Invoke(new MethodInvoker(SetFreeModeControls));

		}

		public bool doNotEvent = false;

		void rpu_OnBaseFrequencyChanged(long pBaseFreq)
		{
			doNotEvent = true;
			frequencyRadio1.Frequency = pBaseFreq;
		}

		/// <summary>
		/// Принят новый отсчет мощности от приемника
		/// </summary>
		/// <param name="pRPU"></param>
		/// <param name="pFrequency"></param>
		/// <param name="pPower"></param>
		/// 

		delegate void SetPowerDelegate(int pPower);
		void SetPower(int pPower)
		{
			if (!InvokeRequired)
			{
				graphPowerScale1.Power = pPower;
				lblPower.Text = pPower.ToString(CultureInfo.InvariantCulture);
			}
			else
				BeginInvoke(new SetPowerDelegate(SetPower), pPower);
		}

		void PowerMeter_OnNewPowerMeasure(KaorCore.RPU.IRPU pRPU, long pFrequency, float pPower)
		{
			SetPower((int)pPower);
			//powerScale1.Power = pPower;
		}

		private void btnOff_Click(object sender, EventArgs e)
		{
			rpu.SwitchOff();
		}

		void SetRPUFrequency(long pParam)
		{
			if (rpu != null)
				rpu.BaseFreq = pParam;
			frequencyRadio1.Frequency = rpu.BaseFreq;
		}

		private void frequencyRadio1_FrequencyChanged(long pNewFrequency)
		{
			if (doNotEvent)
			{
				doNotEvent = false;
				return;
			}
			if (rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				new SetRPUParamDelegate(SetRPUFrequency).BeginInvoke(pNewFrequency, null, null);
		}

		void SetRPUAverageTime(long pParam)
		{
			switch (pParam)
			{
				case 0:
					rpu.PowerMeter.AverageTime = 10;
					break;

				case 1:
					rpu.PowerMeter.AverageTime = 20;
					break;

				case 2:
					rpu.PowerMeter.AverageTime = 50;
					break;

				case 3:
					rpu.PowerMeter.AverageTime = 100;
					break;

				case 4:
					rpu.PowerMeter.AverageTime = 250;
					break;

				case 5:
					rpu.PowerMeter.AverageTime = 500;
					break;

				case 6:
					rpu.PowerMeter.AverageTime = 1000;
					break;

				default:
					rpu.PowerMeter.AverageTime = 20;
					break;
			}

			//rpu.PowerMeter.AverageTime = (int)pParam;
		}

		private void cmbAverageTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				new SetRPUParamDelegate(SetRPUAverageTime).BeginInvoke(cmbAverageTime.SelectedIndex, null, null);
		}

		void SetRPUPowerFilterBand(long pParam)
		{
			switch (pParam)
			{
				case 0:
					rpu.PowerMeter.FilterBand = 1000;
					break;

				case 1:
					rpu.PowerMeter.FilterBand = 3000;
					break;

				case 2:
					rpu.PowerMeter.FilterBand = 7500;
					break;

				case 3:
					rpu.PowerMeter.FilterBand = 10000;
					break;

				case 4:
					rpu.PowerMeter.FilterBand = 30000;
					break;

				case 5:
					rpu.PowerMeter.FilterBand = 100000;
					break;

				case 6:
					rpu.PowerMeter.FilterBand = 120000;
					break;

				case 7:
					rpu.PowerMeter.FilterBand = 280000;
					break;

				default:
					rpu.PowerMeter.FilterBand = 10000;
					break;
			}
//			rpu.PowerMeter.FilterBand = (int)pParam;
		}

		private void cmbPowerFilterBand_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				new SetRPUParamDelegate(SetRPUPowerFilterBand).BeginInvoke(cmbPowerFilterBand.SelectedIndex, null, null);
		}

		/// <summary>
		/// Обработка события изменения текущей антенны в списке антенн
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstAntennas_SelectedIndexChanged(object sender, EventArgs e)
		{
			IAntenna _antenna = null; // lstAntennas.SelectedItem as IAntenna;

			if (rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
			{
				if (!rpu.SwitchAntenna(_antenna))
				{
					if (_antenna != null)
						MessageBox.Show("Error switching to " + _antenna.Name);
					else
						throw new Exception("Error switching to zero antenna");
				}
			}
		}

		delegate void SetRPUParamDelegate(long pParam);

		void SetRPUModulation(long pParam)
		{
			switch(pParam)
			{
				case 0:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.AM;
					break;
				case 1:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.FM;
					break;

				case 2:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.CW;
					break;

				case 3:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.LSB;
					break;

				case 4:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.USB;
					break;

				default:
					rpu.Demodulator.CurrentModulation = EAudioModulationType.FM;
					break;
			}
		}

		void SetRPUModulationBand(long pParam)
		{
			switch (pParam)
			{
				case 0:
					rpu.Demodulator.FilterBand = 3000;
					break;
				case 1:
					rpu.Demodulator.FilterBand = 6000;
					break;

				case 2:
					rpu.Demodulator.FilterBand = 15000;
					break;

				case 3:
					rpu.Demodulator.FilterBand = 50000;
					break;

				case 4:
					rpu.Demodulator.FilterBand = 230000;
					break;

				default :
					rpu.Demodulator.FilterBand = 6000;
					break;
			}
//			rpu.Demodulator.FilterBand = rpu.Demodulator.SupportedFilterBands[(int)pParam];
		}

		/// <summary>
		/// Изменение типа демодуляции
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmbMod_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				new SetRPUParamDelegate(SetRPUModulation).BeginInvoke(cmbMod.SelectedIndex, null, null);
			//comboBox1.Items;
		}

		private void cmbBand_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				new SetRPUParamDelegate(SetRPUModulationBand).BeginInvoke(cmbBand.SelectedIndex, null, null);
		}

		private void btnMemDemod_Click(object sender, EventArgs e)
		{
			Button _btn = sender as Button;

			if (_btn == null)
				return;

			int _memNo = 0;

			if (_btn == btnDemodM1)
				_memNo = 0;
			else if (_btn == btnDemodM2)
				_memNo = 1;
			else if (_btn == btnDemodM3)
				_memNo = 2;

			if (chkMemDemod.Checked)
				StoreDemodMem(_memNo);
			else
			{
				if(demodMem[_memNo].isStored)
					RecallDemodMem(_memNo);
				else
					MessageBox.Show(Locale.empty_mem, 
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			chkMemDemod.Checked = false;
		}

		struct SDemodMemItem
		{
			public EAudioModulationType modulation;
			public int filterBand;
			public bool isStored;
		}

		SDemodMemItem[] demodMem = new SDemodMemItem[3];

		void RecallDemodMem(int pMemNo)
		{
			if (pMemNo < 0 || pMemNo >= demodMem.Length)
				return;

			if (rpu.Demodulator.SupportedFilterBands.Contains(demodMem[pMemNo].filterBand) &&
				rpu.Demodulator.SupportedModulations.Contains(demodMem[pMemNo].modulation))
			{
				rpu.Demodulator.CurrentModulation = demodMem[pMemNo].modulation;
				rpu.Demodulator.FilterBand = demodMem[pMemNo].filterBand;
			}
		}

		void StoreDemodMem(int pMemNo)
		{
			if (pMemNo < 0 || pMemNo >= demodMem.Length)
				return;

			demodMem[pMemNo].modulation = rpu.Demodulator.CurrentModulation;
			demodMem[pMemNo].filterBand = rpu.Demodulator.FilterBand;
			demodMem[pMemNo].isStored = true;
		}


		struct SPowerMeterMemItem
		{
			public bool isStored;// = false;
			public int averageTime;
			public int filterBand;
		}

		SPowerMeterMemItem[] powerMeterMem = new SPowerMeterMemItem[3];

		void RecallPowerMeterMem(int pMemNo)
		{
			if (pMemNo < 0 || pMemNo >= powerMeterMem.Length)
				return;

			new SetRPUParamDelegate(SetRPUPowerFilterBand).BeginInvoke(powerMeterMem[pMemNo].filterBand, null, null);

			new SetRPUParamDelegate(SetRPUAverageTime).BeginInvoke(powerMeterMem[pMemNo].averageTime, null, null);
		}

		void StorePowerMeterMem(int pMemNo)
		{
			if (pMemNo < 0 || pMemNo >= powerMeterMem.Length)
				return;

			//powerMeterMem[pMemNo].averageTime = rpu.PowerMeter.AverageTime;
			//powerMeterMem[pMemNo].filterBand = rpu.PowerMeter.FilterBand;
			powerMeterMem[pMemNo].averageTime = cmbAverageTime.SelectedIndex;
			powerMeterMem[pMemNo].filterBand = cmbPowerFilterBand.SelectedIndex;

			powerMeterMem[pMemNo].isStored = true;
		}

		private void btnPowerMeterMem_Click(object sender, EventArgs e)
		{
			Button _btn = sender as Button;

			if (_btn == null)
				return;

			int _memNo = 0;

			if (_btn == btnPowerMeterM1)
				_memNo = 0;
			else if (_btn == btnPowerMeterM2)
				_memNo = 1;
			else if (_btn == btnPowerMeterM3)
				_memNo = 2;

			if (chkMemPowermeter.Checked)
				StorePowerMeterMem(_memNo);
			else
			{
				if (powerMeterMem[_memNo].isStored)
					RecallPowerMeterMem(_memNo);
				else
					MessageBox.Show(Locale.empty_mem, 
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			chkMemPowermeter.Checked = false;
		}

		private void chkATT_CheckedChanged(object sender, EventArgs e)
		{
			int _attenuation = 0;

			if (chkATT10dB.Checked)
				_attenuation += 10;

			if (chkATT20dB.Checked)
				_attenuation += 20;

			rpu.SetAttenuation(_attenuation, 0);
		}

		public void ShowStartSplash()
		{
			Splash.Splash.ShowSplash(0, 750);
			Splash.Splash.Fadeout();
		
		}

		private void cmbSignalConverter_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (doNotEvent)
			//{
			//    doNotEvent = false;
			//    return;
			//}
			if (rpu.Mode == RPUMode.Free || rpu.Mode == RPUMode.ZeroSpan || rpu.Mode == RPUMode.AudioRecord)
				rpu.SignalConverter = (ISignalConverter)cmbSignalConverter.SelectedItem;
			frequencyRadio1.Min = rpu.SignalConverter.MinFreq;
			frequencyRadio1.Max = rpu.SignalConverter.MaxFreq;
		}

		private void rpu_OnSignalConverterChanged(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new EventHandler(rpu_OnSignalConverterChanged), sender, e);
				return;
			}
			//doNotEvent = true;
			cmbSignalConverter.SelectedItem = rpu.SignalConverter;
		}
	}
}
