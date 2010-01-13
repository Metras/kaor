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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ControlUtils.ObjectListView;
using KaorCore.Antenna;
using KaorCore.RadioControlSystem;
using KaorCore.RPU;

namespace KaorCore.RadioControlSystem
{
	public partial class RadioConfigControl : UserControl
	{
		BaseRadioControlSystem rcs;
		ObservableCollection<IRPU> rpus;
		ObservableCollection<IAntenna> antennas;

		public RadioConfigControl()
		{
			InitializeComponent();

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

		private void btnRPUAdd_Click(object sender, EventArgs e)
		{
			SelectRPUTypeDialog _dlg = new SelectRPUTypeDialog();

			if (_dlg.ShowDialog() != DialogResult.OK)
				return;

			string _rpuTypeName = _dlg.RPUType;

			if(_rpuTypeName == "")
				return;

			try
			{
				Type _rpuType = Type.GetType(_rpuTypeName);
				object _o = Activator.CreateInstance(_rpuType);
				IRPU _rpu = _o as IRPU;

				if (_rpu == null)
					throw new Exception("Получен нулевой РПУ!");

				if (_rpu.SettingsForm != null)
				{
					if (_rpu.SettingsForm.ShowDialog() == DialogResult.OK)
					{
						rpus.Add(_rpu);
					}
				}
				else
				{
					rpus.Add(_rpu);
				}
			}

			catch (Exception ex)
			{
				MessageBox.Show("Ошибка создания объекта РПУ.\n" +
					ex.Message,
					"Ошибка",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}

			lstRPU.BuildList(false);
		}

		private void btnAntennaAdd_Click(object sender, EventArgs e)
		{
			BaseAntenna _antenna = new BaseAntenna();


			Form _dlg = _antenna.SettingsDialog;

			if (_dlg == null)
				return;

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

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				lstAntenna.BuildList(true);
			}
		}

		private void btnRPUEdit_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstRPU.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			IRPU _rpu = _item.RowObject as IRPU;

			if (_rpu == null)
				return;

			if (_rpu.SettingsForm.ShowDialog() == DialogResult.OK)
			{
			}
		}

		private void lstRPU_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstRPU.HitTest(e.Location);

			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;
			if (_item == null)
				return;

			IRPU _rpu = (IRPU)_item.RowObject;

			if (_rpu == null)
				return;

			_rpu.IsDisabled = !_rpu.IsDisabled;

			lstRPU.BuildList(true);
		}

		public delegate void SaveButtonClickDelegate();
		public event SaveButtonClickDelegate OnSaveButtonClick;

		private void toolBtnSave_Click(object sender, EventArgs e)
		{
			if(OnSaveButtonClick != null)
				OnSaveButtonClick();
		}

		private void btnRPUDelete_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstRPU.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			IRPU _rpu = _item.RowObject as IRPU;

			if (_rpu == null)
				return;

			if (MessageBox.Show("Действительно удалить РПУ \"" + _rpu.Name + "\"?",
				"Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				rcs.RPUManager.UnregisterRPU(_rpu);
				lstRPU.BuildList(false);
			}
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

			if (MessageBox.Show("Действительно удалить антенну \"" + _antenna.Name + "\"?",
				"Подтверждение",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				BaseAntenna.DeleteAntenna(_antenna);
				lstAntenna.BuildList(false);
			}
		}
	}
}
