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

using KaorCore.Audio;
using KaorCore.Interfaces;
using KaorCore.Report;

namespace KaorCore.RPU
{

    /// Вид модуляции
    public enum EAudioModulationType
    {
        AM,
        AM_narrow,
        AM_wide,
        CW,
        FM,
        FM_narrow,
        WFM,
        LSB,
        USB
    }

    /// <summary>
    /// Интерфейс устройства - демодулятора аудио-сигнала
    /// </summary>
    /// 

    public interface IAudioDemodulator
    {
        /// <summary>
        /// Поддерживаемые виды модуляций
        /// </summary>
        List<EAudioModulationType> SupportedModulations { get; }

		/// <summary>
		/// Поддерживаемые полосы фильтров
		/// </summary>
		List<Int32> SupportedFilterBands { get; }

		/// <summary>
        /// Текущая модуляция
        /// </summary>
        EAudioModulationType CurrentModulation { get; set; }

        /// <summary>
        /// Полоса демодулятора
        /// </summary>
        int FilterBand { get; set; }

        /// <summary>
        /// Параметры выходных аудиоданных
        /// </summary>
        int AudioSampleRate { get; }
        int AudioBits { get; }
        int AudioChannels { get; }

        /// <summary>
        /// Номер входа для треда получения семплов демодулятора
        /// </summary>
        int DemodulatorInput { get; set; }

        /// <summary>
        /// Флаг состояния треда получения семплов демодулятора
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Запуск треда получения семплов демодулятора
        /// </summary>
        void Start(string pRecordName);

        /// <summary>
        /// Останов треда получения семплов демодулятора
        /// </summary>
        string Stop();

        /// <summary>
        /// Поддержка наушников (параллельно с получением семплов)
        /// </summary>
        bool IsHeadphonesSupported { get; }

        /// <summary>
        /// Включение наушников
        /// </summary>
        void HeadPhonesOn();

        /// <summary>
        /// Отключение наушников
        /// </summary>
        void HeadPhonesOff();

        /// <summary>
        /// Номер выхода наушников блока демодулятора
        /// </summary>
        int HeadPhonesOutput { get; set; }

		/// <summary>
		/// Управление громкостью аудио-демодулятора
		/// Возможные значения - 0:100
		/// </summary>
		int AudioVolume { get; set; }

        /// <summary>
        /// Событие получения новых аудио-данных
        /// </summary>
        event NewAudioData OnNewAudioData;

		/// <summary>
		/// Событие получения новой ауио-записи для отчета
		/// </summary>
		event NewAudioRecord OnNewAudioRecord;

		UserControl Control { get; }
    }

    /// <summary>
    /// Делегат события получения новых аудио-данных
    /// </summary>
    /// <param name="data"></param>
    //public delegate void NewAudioData(AudioData pData);

	/// <summary>
	/// Делегат события появления новой аудио-записи для включения в отчет
	/// </summary>
	/// <param name="pRecord"></param>
	public delegate void NewAudioRecord(string pRecordName);

	
}
