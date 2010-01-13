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
using System.Linq;
using System.Text;
using System.Threading;

using KaorCore.Base;
using KaorCore.RPU;
using KaorCore.Trace;

namespace KaorCore.RPUManager
{
	/// <summary>
	/// Тред сканирования по диапазону
	/// Сканирование осуществляется в отдельном треде через классы RPUTaskRequest, RPUTaskResponse
	/// Обмен с тредом ведется через очереди
	/// </summary>
	public class ScanThread
	{
		#region ================ Поля ================
		
		/// <summary>
		/// Менеджер РПУ, с которым работает тред сканирования
		/// </summary>
		IRPUManager rpuManager;

		/// <summary>
		/// Тело треда сканирования
		/// </summary>
		Thread thread;

		/// <summary>
		/// Трассы, к которым привязан тред сканирования
		/// </summary>
		List<BaseTrace> traces;

		#endregion

		#region ================ Конструктор ================

		/// <summary>
		/// Базовый конструктор треда сканирования
		/// </summary>
		/// <param name="pRPUManager"></param>
		/// <param name="FStart"></param>
		/// <param name="FStop"></param>
		public ScanThread(IRPUManager pRPUManager)
		{
			rpuManager = pRPUManager;

			/// Создание тредов для каждого из РПУ
			foreach (IRPU _rpu in pRPUManager.AvailableRPUDevices)
			{
				thread = new Thread(RPUThread);
				thread.Start(_rpu);

			}
		}

		#endregion

		#region ================ Приватные методы ================

		/// <summary>
		/// Тело треда
		/// Тред принимает запросы из соответстующей очереди, обрабатывает их и 
		/// отправляет ответы в очередь
		/// </summary>
		/// <param name="pParam"></param>
		void RPUThread(object pParam)
		{
			IRPU _rpu = pParam as IRPU;

			if (_rpu == null)
				throw new ArgumentException("Invalid IRPU param (NULL) !");

			
		}

		#endregion
		
		#region ================ Публичные методы ================
		
		public void Start()
		{
		}

		public void Stop()
		{
		}

		#endregion

	}
}