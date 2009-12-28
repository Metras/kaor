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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using KaorCore.I18N;
using KaorCore.Signal;
using KaorCore.Trace;
using ZedGraph;

namespace KaorCore.TraceControl
{
	/// <summary>
	/// Класс логики контроля по трассе
	/// Объект контроля по трассе создается на основе трассы
	/// </summary>
	public abstract class BaseTraceControl
	{
		#region ============== Поля ==============

		Guid id;
		protected string name;
		string description;

		/// <summary>
		/// Трасса, которая используется для сканирования
		/// </summary>
		protected BaseTrace scanTrace;



		//TraceControlItem controlItem;
		BaseTraceTriggerHandler triggerHandler;

		protected bool isVisible;


		#endregion

		#region ============== Конструктор ==============

		public BaseTraceControl(BaseTrace pTrace)
		{
			int _i;

			id = Guid.NewGuid();

			name = Locale.trace_control;

			scanTrace = pTrace;

			triggerHandler = new SignalTriggerHandler(this);

		}

		protected void CallOnTraceTrigger(TracePoint pPoint, double pOldPower, double pDelta)
		{
/*			if (OnTraceControlTrigger != null)
				OnTraceControlTrigger(this, pPoint, pOldPower);*/
			if(triggerHandler != null)
				triggerHandler.TraceControlTriggerHandler(this, pPoint, pOldPower, pDelta);
		}
		#endregion

		#region ============== Проперти ==============
		[Browsable(false)]
		public long Fstart
		{
			get { return scanTrace.Fstart; }
		}
		[Browsable(false)]
		public long Fstop
		{
			get { return scanTrace.Fstop; }
		}
		[Browsable(false)]
		[Description("Название объекта контроля")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		[Browsable(false)]
		[Description("Описание объекта контроля")]
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		[Browsable(false)]
		public BaseTrace ScanTrace
		{
			get { return scanTrace; }
		}
#if false
		[Browsable(false)]
		public BaseTraceTriggerHandler TriggerHandler
		{
			get { return triggerHandler; }
		}
#endif
		[Browsable(false)]
		public bool IsVisible
		{
			get { return isVisible; }
			set { isVisible = value; }
		}

		Type defaultSignalType;
		[Browsable(false)]
		public Type DefaultSignalType
		{
			get { return defaultSignalType; }
			set { defaultSignalType = value; }
		}

		BaseSignalParams defaultSignalParams;
		[Browsable(false)]
		public BaseSignalParams DefaultSignalParams
		{
			get { return defaultSignalParams; }
			set { defaultSignalParams = value; }
		}
		#endregion

		#region ============== Публичные методы ==============
		/// <summary>
		/// Обработка отсчета
		/// При обработке могут использоваться и модифицироваться точки трассы
		/// </summary>
		/// <param name="pPoint"></param>
		/// <returns></returns>
		//public abstract bool ProcessPoint(TracePoint pOldPoint, TracePoint pNewPoint, out TracePoint pResultPoint);

		public abstract void TraceChangedHandler(BaseTrace pTrace, TracePoint pPoint, double pOldPower);

		[Browsable(false)]
		public abstract Form ControlSettings { get; }

		/// <summary>
		/// Переиницлизация объекта контроля
		/// </summary>
		public abstract void ReInitialize();

		/// <summary>
		/// Загрузка парамтров контроля из XML
		/// </summary>
		/// <param name="pNode"></param>
		public virtual void LoadFromXml(XmlNode pNode)
		{
			XmlNode _node;
			string _t;

			_node = pNode.SelectSingleNode("defaultSignalType");

			if (_node != null)
			{
				try
				{
					_t = _node.InnerText;
					defaultSignalType = Type.GetType(_t);
				}

				catch
				{ 
				}
			}

			_node = pNode.SelectSingleNode("defaultSignalParams");

			if (_node != null)
			{
				try
				{
					_t = _node.Attributes["type"].Value;

					BaseSignalParams _sParams = Activator.CreateInstance(Type.GetType(_t)) as BaseSignalParams;
					if (_sParams != null)
					{
						_sParams.LoadFromXmlNode(_node);
					}
					defaultSignalParams = _sParams;
					//XmlSerializer _s = new XmlSerializer(Type.GetType(_t));

					//defaultSignalParams = _s.Deserialize(new XmlNodeReader(_node)) as BaseSignalParams;

				}

				catch
				{
				}
			}




		}

		public virtual void SaveToXml(XmlWriter pWriter)
		{
			if (defaultSignalType == null || defaultSignalParams == null)
				return;
				
			pWriter.WriteElementString("defaultSignalType", defaultSignalType.AssemblyQualifiedName.ToString());
			
			pWriter.WriteStartElement("defaultSignalParams");
			pWriter.WriteAttributeString("type", defaultSignalParams.GetType().AssemblyQualifiedName);

			try
			{

				defaultSignalParams.SaveToXmlWriter(pWriter);
				//XmlSerializer _s = new XmlSerializer(defaultSignalParams.GetType());

				//_s.Serialize(pWriter, defaultSignalParams);
			}
			catch
			{
			}

			pWriter.WriteEndElement();
		}

		//public event TraceControlTrigger OnTraceControlTrigger;

		public override string ToString()
		{
			return name;
		}
#if false
		public void AddTriggerHandler(BaseTraceTriggerHandler pHandler)
		{

			triggerHandler = pHandler;
		}

		public void RemoveTriggerHandler(BaseTraceTriggerHandler pHandler)
		{
			//OnTraceControlTrigger -= pHandler.TraceControlTriggerHandler;

			triggerHandler = null;
		}
#endif
		/// <summary>
		/// Отрисовка объекта контроля на ZedGraph
		/// </summary>
		/// <param name="pCtrl"></param>
		/// <param name="pDrawChilds"></param>
		public abstract void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds);

		/// <summary>
		/// Содание сигнала по-умолчанию для данного объекта контроля
		/// </summary>
		/// <returns></returns>
		public BaseSignal CreateDefaultSignal(TracePoint pPoint,
			double pOldPower, double pDelta)
		{
			BaseSignal _signal = null;

			if (defaultSignalType == null)
				return null;

			try
			{
				object _o = Activator.CreateInstance(defaultSignalType,
					defaultSignalParams, this, 
					pPoint, pOldPower, pDelta);

				_signal = _o as BaseSignal;
			}

			catch
			{

			}

			return _signal;
		}
		#endregion

		#region ============== Статические методы ==============

		/// <summary>
		/// Список дступных типов контроля
		/// Чтобы не городить динамическое определение, пока будет список
		/// </summary>
		static Dictionary<Type, string> controlTypes;
#if false
		public static Type UserSelectControl()
		{
			Type _t = null;

			if (controlTypes == null)
			{
				controlTypes = new Dictionary<Type, string>();
				controlTypes.Add(typeof(TraceControlDelta), "Контроль по порогу");
                controlTypes.Add(typeof(TraceControlAdaptiveBounds), "Адаптивный контроль");
			}

			SelectTraceControlForm _dlg = new SelectTraceControlForm(controlTypes);

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				_t = _dlg.SelectedTraceControlType;
			}

			return _t;
		}

		public static Dictionary<Type, string> ControlTypes
		{
			get
			{
				if (controlTypes == null)
				{
					controlTypes = new Dictionary<Type, string>();
					controlTypes.Add(typeof(TraceControlDelta), "Контроль по порогу");
					controlTypes.Add(typeof(TraceControlAdaptiveBounds), "Адаптивный контроль");
				}

				return controlTypes;
			}
		}
#endif
		#endregion
	}

	public delegate void TraceControlTrigger(BaseTraceControl pTrace, TracePoint pPoint, double pOldPower);
}
