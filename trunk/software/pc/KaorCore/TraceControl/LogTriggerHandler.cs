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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using KaorCore.Trace;
#if false
namespace KaorCore.TraceControl
{
	public class LogTriggerHandler : BaseTraceTriggerHandler
	{
		#region ================ Поля ================
		TextWriter logFile;
		#endregion

		#region ================ Конструктор ================
		public LogTriggerHandler(BaseTraceControl pTraceControl)
			: base(pTraceControl)
		{
			name = "Запись событий в журнал";
			logFile = new StreamWriter("triggers_" + DateTime.Now.ToFileTime() + ".log", true);
		}

		#endregion

		#region ================ Публичные методы ================
		public override void LoadFromXmlNode(XmlNode pNode)
		{
			/// 
		}
		#endregion


		#region ================ Проперти ================
		#endregion

		#region ================ Приватные методы ================

		/// <summary>
		///  Обработчик события срабатывания триггера по трассе контроля
		/// </summary>
		/// <param name="pTrace"></param>
		/// <param name="pPoint"></param>
		/// <param name="pMeasuredPower"></param>
		public override bool TraceControlTriggerHandler(BaseTraceControl pTraceControl, 
			TracePoint pPoint, double pOldPower, double pDelta)
		{
			lock (logFile)
			{
				AddLogRecord(pPoint.Freq, pPoint.Power, pOldPower);
			}
			return true;
		}

		void AddLogRecord(Int64 pFreq, double pNewPower, double pOldPower)
		{
			logFile.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + ": " + "F=" + pFreq.ToString(CultureInfo.InvariantCulture) +
				", OP=" + pOldPower.ToString(CultureInfo.InvariantCulture) + ", NP=" + pNewPower.ToString(CultureInfo.InvariantCulture));

			logFile.Flush();
		}
		#endregion




	}
}
#endif