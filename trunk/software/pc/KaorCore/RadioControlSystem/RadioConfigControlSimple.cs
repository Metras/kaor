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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ControlUtils.ObjectListView;
using KaorCore.Antenna;
using KaorCore.I18N;
using KaorCore.RPUManager;
using KaorCore.RadioControlSystem;
using KaorCore.Report;
using KaorCore.RPU;

namespace KaorCore.RadioControlSystem
{
	public partial class RadioConfigControlSimple : UserControl
	{
		BaseRadioControlSystem rcs;
		ObservableCollection<IRPU> rpus;
		IRPU rpu;
		ObservableCollection<IAntenna> antennas;

		public RadioConfigControlSimple()
		{
			InitializeComponent();

			cmbRPUType.Items.Clear();
			cmbRPUType.Items.AddRange(BaseRPUManager.RPUClasses.ToArray());
		}

		/// <summary>
		/// Инициализация списка антенн
		/// </summary>
		private void InitAntennaListView()
		{
			olvAntennaName.AspectGetter = delegate(object pO) { return ((BaseAntenna)pO).Name; };
			olvAntennaStatus.AspectGetter = delegate(object pO) { return ((BaseAntenna)pO).State; };

			olvAntennaStatus.Renderer = new MappedImageRenderer(EAntennaState.OK, "");

			lstAntenna.RowFormatter = delegate(OLVListItem pItem)
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
						_backColor = Color.White;
						break;
				}

				pItem.BackColor = _backColor;
				foreach (ListViewItem.ListViewSubItem _subItem in pItem.SubItems)
				{
					_subItem.BackColor = _backColor;
				}
			};

			lstAntenna.SetObjects(antennas);
		}

		/// <summary>
		/// Инициализация списка РПУ
		/// </summary>
		private void InitRPUListView()
		{
#if false
			olvRPUName.AspectGetter = delegate(object pO) { return ((IRPU)pO).Name; };
			olvRPUStatus.AspectGetter = delegate(object pO) { return ((IRPU)pO).IsDisabled; };

			olvRPUStatus.Renderer = new MappedImageRenderer(true, "disabled.png");
			
			lstRPU.RowFormatter = delegate(OLVListItem pItem)
			{
				/// Формирование цвета фона в зависимости от типа сигнала
				IRPU _rpu = (IRPU)pItem.RowObject;
				Color _backColor;

				if(_rpu.IsAvailable)
				{
						_backColor = Color.White;
				}
				else
				{
					_backColor = Color.Red;
				}

				pItem.BackColor = _backColor;
				foreach (ListViewItem.ListViewSubItem _subItem in pItem.SubItems)
				{
					_subItem.BackColor = _backColor;
				}
			};

			lstRPU.SetObjects(rpus);
#endif
			if (rpus.Count > 0)
			{
				rpu = rpus.First();
			}
			else
			{
				rpu = null;
			}

			if (rpu == null)
			{
				btnRPUEdit.Enabled = false;
				cmbRPUType.SelectedIndex = -1;
			}
			else
			{
				foreach(object _i in cmbRPUType.Items)
				{
					if(rpu.GetType() == ((RPUClass)_i).RPUType)
					{
						cmbRPUType.SelectedItem = _i;
						break;
					}
				}
			}
			/*
			else if(rpu.GetType().AssemblyQualifiedName.Contains("RPURPV3.CRPURPV3, RPURPV3"))
			{
				cmbRPUType.SelectedIndex = 0;
			}
			else if (rpu.GetType().AssemblyQualifiedName.Contains("RPUICOMR8500.RPUR8500"))
			{
				cmbRPUType.SelectedIndex = 1;
			}
			else
			{
				btnRPUEdit.Enabled = false;
				cmbRPUType.SelectedIndex = -1;
			}
			*/
			this.cmbRPUType.SelectedIndexChanged += new System.EventHandler(this.cmbRPUType_SelectedIndexChanged);
		}

		public BaseRadioControlSystem RCS
		{
			get
			{
				return rcs;
			}

			set
			{
				rcs = value;

				if (rcs != null)
				{
					//rpus = new List<IRPU>(rcs.RPUManager.RPUDevices);
					//antennas = new List<IAntenna>(BaseAntenna.AntennaList);
					rpus = rcs.RPUManager.RPUDevices;
					antennas = BaseAntenna.AntennaList;

					GetRCSConfig();

					InitRPUListView();
					InitAntennaListView();
				}
			}
		}

		/// <summary>
		/// Получение конфигурации СРК
		/// </summary>
		private void GetRCSConfig()
		{
			
		}

		private void btnAntennaAdd_Click(object sender, EventArgs e)
		{
			BaseAntenna _antenna = new BaseAntenna();


			Form _dlg = _antenna.SettingsDialog;

			if (_dlg == null)
				return;

			rcs.CallOnConfigurationChanged();

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				antennas.Add(_antenna);
				lstAntenna.BuildList(true);
			}
		}

		private void btnAntennaEdit_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstAntenna.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			IAntenna _antenna = _item.RowObject as IAntenna;

			if (_antenna == null)
				return;

			Form _dlg = _antenna.SettingsDialog;

			if (_dlg == null)
				return;

			rcs.CallOnConfigurationChanged();

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				lstAntenna.BuildList(true);
			}
		}

		private void btnRPUEdit_Click(object sender, EventArgs e)
		{
			//IRPU _rpu = cmbRPUType.SelectedItem as IRPU;

			if (rpu == null)
				return;

			rcs.CallOnConfigurationChanged();

			if (rpu.SettingsForm.ShowDialog() == DialogResult.OK)
			{
			}
		}


		public delegate void SaveButtonClickDelegate();
		public event SaveButtonClickDelegate OnSaveButtonClick;

		private void toolBtnSave_Click(object sender, EventArgs e)
		{
			if(OnSaveButtonClick != null)
				OnSaveButtonClick();
		}

		/// <summary>
		/// Удаление антенны из системы
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAntennaDelete_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstAntenna.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseAntenna _antenna = _item.RowObject as BaseAntenna;

			if (_antenna == null)
				return;

			if (MessageBox.Show(String.Format(Locale.confirm_delete_antenna, _antenna.Name),
				Locale.confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				BaseAntenna.DeleteAntenna(_antenna);
				lstAntenna.BuildList(false);

				rcs.CallOnConfigurationChanged();
			}
		}

		private void cmbRPUType_SelectedIndexChanged(object sender, EventArgs e)
		{
			RPUClass _rpuClass = cmbRPUType.SelectedItem as RPUClass;

			if (_rpuClass == null)
			{
				btnRPUEdit.Enabled = false;
				return;
			}
			
			if (rpu != null && rpu.GetType() == _rpuClass.RPUType)
				return;

			/*
			string _t = "";

			switch (cmbRPUType.SelectedIndex)
			{
				case 0:
					_t = "RPURPV3.CRPURPV3, RPURPV3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
					break;

				case 1:
					_t = "RPUICOMR8500.RPUR8500, RPUICOMR8500, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
					break;

				default:
					_t = "";
					break;

			}

			if (_t == "")
			{
				btnRPUEdit.Enabled = false;
				return;
			}

			if (rpu != null && rpu.GetType().AssemblyQualifiedName == _t)
				return;
			*/
			try
			{
				if (rpu != null)
				{
					rpu.SwitchOff();
				}
			}
			catch
			{
			}

			try
			{
				//Type _rpuType = Type.GetType(_t);
				Type _rpuType = _rpuClass.RPUType;

				object _o = Activator.CreateInstance(_rpuType);
				rpu = _o as IRPU;

				if (rpu == null)
					throw new Exception("Got NULL RPU!");

				/// РПУ включено
				rpu.IsDisabled = false;

				////if (rpu.SettingsForm != null)
				////{
				////    if (rpu.SettingsForm.ShowDialog() == DialogResult.OK)
				////    {
				////        rpus.Clear();
				////        rpus.Add(rpu);
				////    }
				////}
				////else
				{
					rpus.Clear();
					rpus.Add(rpu);
				}

				btnRPUEdit.Enabled = true;
			}

			catch (Exception ex)
			{
				MessageBox.Show("Error creating RPU object.\n" +
					ex.Message,
					Locale.error,
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Настройка отчетов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReportSettings_Click(object sender, EventArgs e)
		{
			ReportSettings _dlg = new ReportSettings();

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				MessageBox.Show(Locale.confirm_save_report, Locale.confirmation,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void toolBtnReset_Click(object sender, EventArgs e)
		{

		}
	}
}
