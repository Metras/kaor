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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

using KaorCore.Trace;

namespace KaorCore.TraceControl
{
	/// <summary>
	/// Базовый обработчик события тревоги по трассе контроля
	/// </summary>
	public abstract class BaseTraceTriggerHandler
	{
		#region ================ Поля ================
		protected BaseTraceControl traceControl;
		protected string name;
		#endregion

		#region ================ Конструктор ================
		public BaseTraceTriggerHandler(BaseTraceControl pTraceControl)
		{
			traceControl = pTraceControl;
			name = "Базовый обработчик";
		}

		#endregion

		~BaseTraceTriggerHandler()
		{
#if false
			/// Сброс обработчика события
			/// 
			if (trace != null)
			{
				trace.OnTraceTrigger -= TraceTriggerHandler;
			}
#endif
		}

		#region ================ Проперти ================
		[Browsable(true)]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		#endregion

		#region ================ Публичные методы ================

		/// <summary>
		/// Обработчик события тревоги по трассе контроля
		/// </summary>
		/// <param name="pTrace"></param>
		/// <param name="pPoint"></param>
		/// <param name="pOldPower"></param>
		/// <param name="pDelta">Дельта</param>
		public abstract bool TraceControlTriggerHandler(BaseTraceControl pTrace, TracePoint pPoint, double pOldPower, double pDelta);

		/// <summary>
		/// Загрузка параметров из ноды XML
		/// </summary>
		/// <param name="pNode"></param>
		public abstract void LoadFromXmlNode(XmlNode pNode);

		public override string ToString()
		{
			return name;
		}
		#endregion

		#region ================ Статические методы ================
#if false
		static Dictionary<Type, string> triggerHandlerTypes;

		public static Dictionary<Type, string> TriggerHandlerTypes
		{
			get
			{
				Type _t = null;

				if (triggerHandlerTypes == null)
				{
					triggerHandlerTypes = new Dictionary<Type, string>();
					//triggerHandlerTypes.Add(typeof(LogTriggerHandler), "Запись событий в журнал");
					//triggerHandlerTypes.Add(typeof(RCSPauseTriggerHandler), "Пауза контроля");
					triggerHandlerTypes.Add(typeof(SignalTriggerHandler), "Анализатор сигналов");
				}

				return triggerHandlerTypes;
			}
		}
#endif
		public static BaseTraceTriggerHandler FromXmlNode(XmlNode pNode)
		{
			BaseTraceTriggerHandler _triggerHandler;
			string _type, _assName;

			if (pNode == null)
				throw new ArgumentNullException("XML NODE == NULL!");

			if (pNode.Attributes["type"] == null ||
				pNode.Attributes["assembly"] == null)
				return null;

			
			_type = pNode.Attributes["type"].Value;
			_assName = pNode.Attributes["assembly"].Value;
			Assembly _ass = Assembly.Load(_assName);
			Type _t = _ass.GetType(_type);
			object _o = Activator.CreateInstance(_t, null);

			_triggerHandler = _o as BaseTraceTriggerHandler;

			return _triggerHandler;
		}

		#endregion
	}
}
