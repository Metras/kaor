﻿// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
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

using KaorCore.RPU;

namespace KaorCore.RPUManager
{
	/// <summary>
	/// Интерфейс менеджера РПУ
	/// </summary>
	public interface IRPUManager
	{
		#region ================ Проперти ================
		/// <summary>
		/// Список всех РПУ, находящихся в системе
		/// </summary>
		ObservableCollection<IRPU> RPUDevices { get; }

		/// <summary>
		/// Список доступных для работы РПУ
		/// </summary>
		List<IRPU> AvailableRPUDevices { get; }


		#endregion
		/// <summary>
		/// Регистрация нового РПУ
		/// </summary>
		/// <param name="pRPU"></param>
		void RegisterRPU(IRPU pRPU);


		/// <summary>
		/// Запрос РПУ для работы
		/// </summary>
		/// <returns></returns>
		IRPU AcquireRPU();
		
		/// <summary>
		/// Запрос РПУ на основе частоты
		/// </summary>
		/// <param name="pFreq"></param>
		/// <returns></returns>
		IRPU AcquireRPU(Int64 pFreq);

		/// <summary>
		/// Запрос РПУ по функцииям
		/// </summary>
		/// <param name="pFreq"></param>
		/// <param name="pHasPowerMeter"></param>
		/// <param name="pHasDemodulator"></param>
		/// <param name="pHasSpectrograph"></param>
		/// <returns></returns>
		IRPU AcquireRPU(Int64 pFreq, bool pHasPowerMeter, bool pHasDemodulator, bool pHasSpectrograph);

		/// <summary>
		/// Освобождение РПУ
		/// </summary>
		/// <param name="pRPU"></param>
		void FreeRPU(IRPU pRPU);

		event RPUAddedDelegate OnRPUAdded;
		event RPUDeletedDelegate OnRPUDeleted;
		event ManualRPUChangedDelegate OnManualRPUChanged;
	}

	public delegate void RPUAddedDelegate(IRPU pRPU);
	public delegate void RPUDeletedDelegate(IRPU pRPU);
	public delegate void ManualRPUChangedDelegate(IRPU pOldRPU, IRPU pNewRPU);
}
