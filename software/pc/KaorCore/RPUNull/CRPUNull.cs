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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.I18N;
using KaorCore.RPU;

namespace KaorCore.RPUNull
{
	/// <summary>
	/// Класс нулевого РПУ
	/// Присутствует в системе  всегда
	/// Менеджер РПУ должен возвращать этот РПУ если ничего другого не найдено
	/// </summary>
	public class CRPUNull : IRPU
	{
		RPUNullControl rpuControl;
		NullPowerMeter powerMeter;
		Guid rpuType = new Guid("{2FD196B0-662E-410d-9BD9-F7CDD6AFADC0}");
		public CRPUNull()
		{
			rpuControl = new RPUNullControl();
			powerMeter = new NullPowerMeter();
		}
		public override string ToString()
		{
			return Locale.no_rpu;
		}

		#region IRPU Members

		public string Name
		{
			get { return Locale.no_rpu; }
		}

		public string Serial
		{
			get { return "000000000"; }
		}

		public string Description
		{
			get { return Locale.no_rpu; }
		}

		public Guid Id
		{
			get { return Guid.NewGuid(); }
		}

		public System.Windows.Forms.Form SettingsForm
		{
			get { throw new NotImplementedException(); }
		}

		public UserControl RPUControl
		{
			get 
			{
				return rpuControl;
			}
		}

		/// <summary>
		/// Заглушки методова
		/// </summary>
		public void SwitchOn()
		{
		}

		public void SwitchOff()
		{
		}


		public KaorCore.AntennaManager.IAntennaManager AntennaManager
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool IsBusy
		{
			get
			{
				return false;
			}
			set
			{
				
			}
		}

		public bool IsAvailable
		{
			get
			{
				return true;
			}
			set
			{
				
			}
		}

		public long FreqMin
		{
			get { return 0; }
		}

		public long FreqMax
		{
			get { return 3000000000; }
		}

		public bool HasDemodulator
		{
			get { return false; }
		}

		public bool HasSpectrograph
		{
			get { return false; }
		}

		public bool HasPowerMeter
		{
			get { return false; }
		}

		public IAudioDemodulator Demodulator
		{
			get { throw new NotImplementedException(); }
		}

		public IPowerMeter PowerMeter
		{
			get { return powerMeter; }
		}

		public ISpectrograph Spectrograph
		{
			get { throw new NotImplementedException(); }
		}

		public bool CanReleaseManual
		{
			get { return true; }
		}

		public bool CanSetManual
		{
			get { return true; }
		}

		public System.Windows.Forms.UserControl statusControl
		{
			get { throw new NotImplementedException(); }
		}

		public long BaseFreq
		{
			get
			{
				return 0;
			}
			set
			{
				
			}
		}

		public BaseRPUParams Parameters
		{
			get
			{
				return new BaseRPUParams();
			}
			set
			{
				
			}
		}

		public List<KaorCore.Antenna.IAntenna> Antennas
		{
			get 
			{
				return new List<KaorCore.Antenna.IAntenna>();
			}
		}

		public void LoadFromXmlNode(System.Xml.XmlNode pNode)
		{
			
		}

		public bool StartTaskProcessor(KaorCore.Task.ITaskProvider pTaskProvider)
		{
			return false;
		}

		public bool StopTaskProcessor(KaorCore.Task.ITaskProvider pTaskProvider)
		{
			return false;
		}

		public bool InterruptTaskProcessor(KaorCore.Task.ITaskProvider pTaskProvider)
		{
			return false;
		}

		public bool PauseTaskProcessors()
		{
			return false;
		}

		public bool ResumeTaskProcessors()
		{
			return false;
		}

		public bool IsTaskProcessorRunning
		{
			get { return false; }
		}

		public object ShowRecordParamsDialog(object pRecordParams)
		{
			return null;
		}

		public void StartRecordSignal(KaorCore.Signal.BaseSignal pSignal, object pRecordParams, NewRecordInfoDelegate pNewRecordInfo, KaorCore.Trace.BaseTrace pScanTrace)
		{
			
		}

		public string StopRecordSignal(object pRecordParams)
		{
			return "";
		}

		public object DefaultSignalRecordParams
		{
			get { return null; }
		}

		public event BaseFrequencyChanged OnBaseFrequencyChanged;

		public void SetParamsFromSignal(KaorCore.Signal.BaseSignal signalChart)
		{
			
		}

		public void ShowStartSplash()
		{
			
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{

		}

		#endregion

		#region IRPU Members


		public bool SwitchAntenna(KaorCore.Antenna.IAntenna pAntenna)
		{
			return true;
		}

		#endregion

		#region IRPU Members


		public bool SetupScanParams(KaorCore.Trace.BaseTrace pTrace)
		{
			MessageBox.Show(Locale.rpu_cant_scan, Locale.error, 
				MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}

		#endregion

		#region IRPU Members


		public bool IsDisabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		#endregion

		#region IRPU Members


		public void SaveToXmlWriter(System.Xml.XmlWriter pWriter)
		{
			
		}

		#endregion

		#region IRPU Members


		public event RPUParamsChangedDelegate OnRPUParamsChanged;

		#endregion

		#region IRPU Members


		public Guid RPUType
		{
			get 
			{
				return rpuType;
			}
		}

		#endregion

		#region IRPU Members

		/// <summary>
		/// Проверка конфигурации
		/// </summary>
		public void CheckConfiguration()
		{
			
		}

		#endregion
	}
}
