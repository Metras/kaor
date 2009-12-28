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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorMainView.I18N;
using ControlUtils.Splash;
using KaorCore.Audio;
using KaorCore.Antenna;
using KaorCore.RadioControlSystem;
using KaorCore.RPU;
using KaorCore.Utils;

namespace KaorMainView
{
	public partial class KaorMain : Form
	{
		BaseRadioControlSystem rcs;

		public KaorMain()
		{
			InitializeComponent();

			//this.Opacity = 0;

			Text = String.Format(Locale.kaor_name,  KaorVersion.Version, KaorVersion.Revision);
			rcs = BaseRadioControlSystem.Instance;
			rcsView.RCS = rcs;
			rcsView.OnCursorFrequencyChanged += new BaseRCSView.CursorFrequencyChangedDelegate(cTraceView1_OnCursorFrequencyChanged);
			rcsView.OnQuitClick += new BaseRCSView.QuitClickDelegate(cTraceView1_OnQuitClick);
			this.mainSplitContainer.SplitterDistance = this.mainSplitContainer.Height - 122;


			rcs.OnConfigurationChanged += new BaseRadioControlSystem.ConfigurationChangedDelegate(rcs_OnConfigurationChanged);
			BaseAntenna.AntennaList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(AntennaList_CollectionChanged);

			if (rcs.RPUManager.AvailableRPUDevices.Count > 1)
				rcs.RPUManager.ManualRPU = rcs.RPUManager.AvailableRPUDevices[1];

			/// Установка ручного РПУ
			SelectManualRPU();

			RefreshRPUList();
			
			/// Определение обработчика события сохранения
			/// 
			rcs.OnSaveProcess += new BaseRadioControlSystem.SaveProcessDelegate(Instance_OnSaveProcess);
		}

		void rcs_OnConfigurationChanged()
		{
			RestartRequest();
		}

		void AntennaList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			RestartRequest();
		}

		void RPUManager_Changed(IRPU pRPU)
		{
			RestartRequest();
		}

		void RestartRequest()
		{
			SelectManualRPU(null);
			toolRPUList.Enabled = false;

			mainSplitContainer.Panel2.Controls.Clear();
			lblNoRPU.Text = Locale.restart;

			mainSplitContainer.Panel2.Controls.Add(lblNoRPU);

		}

		void RefreshRPUList()
		{
			toolRPUList.DropDownItems.Clear();

			/// Заполнение списка доступных РПУ
			/// 
			foreach (IRPU _rpu in rcs.RPUManager.AvailableRPUDevices)
			{
				ToolStripMenuItem _item = new ToolStripMenuItem(_rpu.Name);
				_item.Tag = _rpu;
				_item.Click += new EventHandler(RPUListItem_Click);
				this.toolRPUList.DropDownItems.Add(_item);

				if (_rpu == rcs.RPUManager.ManualRPU)
					this.toolRPUList.Text = _rpu.Name;
			}
		}

		void cTraceView1_OnQuitClick()
		{
			this.Close();
		}

		delegate void StatusTextDelegate(string pText);

		void Instance_OnSaveProcess(string pText)
		{
			if (!InvokeRequired)
			{
				toolStripStatus.Text = pText;
			}
			else
				Invoke(new StatusTextDelegate(Instance_OnSaveProcess), pText);
		}

		void cTraceView1_OnCursorFrequencyChanged(long pFrequency)
		{
			if (rcs.RPUManager.ManualRPU != null)
				rcs.RPUManager.ManualRPU.BaseFreq = pFrequency;
		}

		void RPUListItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _item = sender as ToolStripMenuItem;

			if (_item == null)
				return;

			IRPU _rpu = _item.Tag as IRPU;

			if (_rpu == null)
				return;

			this.toolRPUList.Text = _rpu.Name;

			SelectManualRPU(_rpu);
		}

		void SelectManualRPU()
		{
			mainSplitContainer.Panel2.Controls.Clear();

			if (rcs.RPUManager.ManualRPU == null)
			{
				mainSplitContainer.Panel2.Controls.Add(lblNoRPU);
				return;
			}

			
			mainSplitContainer.Panel2.Controls.Add(BaseRadioControlSystem.Instance.RPUManager.ManualRPU.RPUControl);
			rcs.RPUManager.ManualRPU.RPUControl.Dock = DockStyle.Fill;
			rcs.RPUManager.ManualRPU.OnBaseFrequencyChanged += new BaseFrequencyChanged(manualRPU_OnBaseFrequencyChanged);
			//rcs.RPUManager.ManualRPU.PowerMeter.OnNewPowerMeasure += new NewPowerMeasure(PowerMeter_OnNewPowerMeasure);
		}


		void SelectManualRPU(IRPU pRPU)
		{
			IRPU _oldRPU = rcs.RPUManager.ManualRPU;
			
			if (_oldRPU != null)
			{
				/// Освобождение ручного РПУ
				/// 
				_oldRPU.OnBaseFrequencyChanged -= manualRPU_OnBaseFrequencyChanged;
			}

			rcs.RPUManager.ManualRPU = pRPU;
			rcs.RPUManager.ManualRPU.ShowStartSplash();

			SelectManualRPU();
		}

		void manualRPU_OnBaseFrequencyChanged(long pBaseFreq)
		{
			rcsView.CursorFrequency = pBaseFreq;
		}

		private void KaorMain_Shown(object sender, EventArgs e)
		{
			Splash.Status = Locale.status_system_loaded;
			Splash.Fadeout();

			if (!rcs.IsConfigured)
			{
				if (MessageBox.Show(Locale.not_configured,
					Locale.question,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
				{
					rcsView.OpenConfigPanel();
				}
			}

			/// Если было некорктное завершение работы, то спрашиваем 
			/// про восстановление из автосохраненной копии
			if (rcs.IsCrashed)
			{
				if (MessageBox.Show(Locale.system_crashed, Locale.question,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button1) == DialogResult.Yes)
				{
					rcs.LoadAutoSaveState();
				}

				rcs.IsCrashed = false;
			}
		}

		private void KaorMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey)
				rcsView.ShiftPressed = true;
			else if (e.KeyCode == Keys.ControlKey)
				rcsView.CtrlPressed = true;

			e.Handled = false;
		}

		private void KaorMain_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey)
				rcsView.ShiftPressed = false;
			else if (e.KeyCode == Keys.ControlKey)
				rcsView.CtrlPressed = false;

			e.Handled = false;
		}

		private void KaorMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MessageBox.Show(Locale.confirm_quit,
				Locale.confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.No)
			{
				e.Cancel = true;
			}
			else
			{
				rcs.SwitchOff();
			}
		}

		private void KaorMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			AudioRecorder.Instance.Dispose();
		}
	}
}
