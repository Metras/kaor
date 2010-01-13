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
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.AntennaManager;
using KaorCore.Base;
using KaorCore.RPU;
using KaorCore.Report;
using KaorCore.Signal;
using KaorCore.Task;
using KaorCore.Trace;

namespace KaorCore.RPU
{
	/// <summary>
	/// Интерфейс базового радиоприемного устройства
	/// Реализации РПУ должны обеспечивать минимальный уровень функциональности
	/// </summary>
	public interface IRPU : IDisposable
	{
		string Name { get; }
		string Serial { get; }
		string Description { get; }

		Guid Id { get; }

		/// <summary>
		/// Контрол настроек РПУ
		/// </summary>
		Form SettingsForm { get; }

		/// <summary>
		/// Контрол РПУ в нормальных режимах
		/// </summary>
		UserControl RPUControl { get; }

		void SwitchOn();
		void SwitchOff();

		/// <summary>
		/// Менеджер антенн, к которому подключено РПУ
		/// </summary>
		//IAntennaManager AntennaManager { get; set; }

/*		IRPUAssignable AssignetTo { get; }
		void Assign(IRPUAssignable pAssigned);
		void Release(IRPUAssignable pAssigned);*/
		bool IsBusy { get; set; }

		/// <summary>
		/// Признак доступности РПУ для работы
		/// Метод set, по-идее, не нужен
		/// </summary>
		bool IsAvailable { get; set; }

		#region ================ Параметры РПУ ================

		Int64 FreqMin { get; }
		Int64 FreqMax { get; }
		bool HasDemodulator { get; }
		bool HasSpectrograph { get; }
		bool HasPowerMeter { get; }
		Guid RPUType { get; }

		#endregion

		#region ================ Свойства для режимов работы ================

		/// <summary>
		/// Интерфейс демодулятора РПУ
		/// </summary>
		IAudioDemodulator Demodulator { get; }

		/// <summary>
		/// Интерфейс измерителя мощности РПУ
		/// </summary>
		IPowerMeter PowerMeter { get; }

		/// <summary>
		/// Интерфейс спектрографа РПУ
		/// </summary>
		ISpectrograph Spectrograph { get; }

		//bool CanReleaseManual { get; }
		//bool CanSetManual { get; }

		#endregion

		#region ================ Общие методы и проперти ================

		/// Контрол статуса РПУ
		UserControl statusControl { get; }

		/// Текущая частота РПУ
		Int64 BaseFreq { get; set; }

		/// <summary>
		/// Параметры РПУ
		/// </summary>
		BaseRPUParams Parameters { get; set; }

		List<IAntenna> Antennas { get; }

		bool IsDisabled { get; set; }
		#endregion

		#region ================ Методы загрузки параметров из XML ================
		/// <summary>
		/// Загрузка параметров РПУ из XML
		/// </summary>
		/// <param name="pNode"></param>
		void LoadFromXmlNode(XmlNode pNode);

		#endregion

		#region ================ Работа с задачами ================
		
		bool StartTaskProcessor(ITaskProvider pTaskProvider);
		bool StopTaskProcessor(ITaskProvider pTaskProvider);
		bool InterruptTaskProcessor(ITaskProvider pTaskProvider);
		bool PauseTaskProcessors();
		bool ResumeTaskProcessors();
		bool IsTaskProcessorRunning { get; }
//		ITaskProvider ScanTraceTaskProvider(BaseTrace pTrace);

		#endregion

		#region ================ Работа с сигналами ================

		/// <summary>
		/// Диалог параметров записи сигнала
		/// Возвращает объект параметров
		/// </summary>
		/// <returns></returns>
		object ShowRecordParamsDialog(object pRecordParams);

		/// <summary>
		/// Запись сигнала с указанным временем и параметрами
		/// </summary>
		/// <param name="pSignal"></param>
		/// <param name="pRecordTime"></param>
		/// <param name="pRecordParams"></param>
		/// <returns></returns>
		void StartRecordSignal(BaseSignal pSignal, object pRecordParams, 
			NewRecordInfoDelegate pNewRecordInfo, BaseTrace pScanTrace);

		string StopRecordSignal(object pRecordParams);

		object DefaultSignalRecordParams { get; }
		#endregion

		#region ================ События ================

		/// <summary>
		/// Событие изменения базовой частоты РПУ
		/// </summary>
		event BaseFrequencyChanged OnBaseFrequencyChanged;

		event RPUParamsChangedDelegate OnRPUParamsChanged;
		#endregion

		void SetParamsFromSignal(BaseSignal signalChart);

		/// <summary>
		/// Отображение стартовой заставки РПУ
		/// </summary>
		void ShowStartSplash();

		bool SwitchAntenna(IAntenna pAntenna);

		bool SetupScanParams(BaseTrace pTrace);

		void SaveToXmlWriter(XmlWriter pWriter);

		void CheckConfiguration();
	}

	public delegate void BaseFrequencyChanged(Int64 pBaseFreq);
	public delegate void NewRecordInfoDelegate(ReportItem pRecordInfo);
	public delegate void RPUParamsChangedDelegate(IRPU pRPU);
}
