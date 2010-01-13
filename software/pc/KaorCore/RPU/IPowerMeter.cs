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
using System.Linq;
using System.Text;

using KaorCore.Trace;

namespace KaorCore.RPU
{
	/// <summary>
	/// Измеритель мощности
	/// Предназначен для оценки мощности сигнала
	/// </summary>
	public interface IPowerMeter
	{
		
		int FilterBand { get; set; }

		/// <summary>
		/// Время усреднения в us
		/// </summary>
		int AverageTime { get; set; }

		/// <summary>
		/// Одиночные измерения мощности
		/// </summary>
		/// <returns></returns>
		double MeasurePower();

		/// <summary>
		/// Одиночное измерение мощности на указанной частоте
		/// </summary>
		/// <param name="pBaseFreq"></param>
		/// <returns></returns>
		double MeasurePower(Int64 pBaseFreq);

		/// <summary>
		/// Методы для работы измерителя мощности в отдельном треде
		/// </summary>
		bool IsRunning { get; }
		void Start();
		void Stop();
#if OLD_TRACES
		/// <summary>
		/// Вывод диалогового окна для создания новой трассы пользователем
		/// </summary>
		/// <returns></returns>
		BaseTrace UserCreateNewTrace(long pFstart, long pFstop);

		/// <summary>
		/// Быстрое создание новой трассы с текущими параметрами приемника
		/// </summary>
		/// <returns></returns>
		BaseTrace UserQuickCreateNewTrace(Int64 pFstart, Int64 pFstop);
#endif
		event NewPowerMeasure OnNewPowerMeasure;
	}

	public delegate void NewPowerMeasure(IRPU pRPU, Int64 pFrequency, float pPower);
}
