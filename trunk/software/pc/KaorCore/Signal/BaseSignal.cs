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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.I18N;
using KaorCore.Trace;
using KaorCore.TraceControl;
using ZedGraph;

namespace KaorCore.Signal
{
	public enum ESignalType
	{
		Unknown,
		Green,
		Yellow,
		Red
	};

	public abstract class BaseSignalParams
	{
		/// <summary>
		/// Время паузы на сигнале при срабатывании
		/// </summary>
		private int pauseTime;

		public BaseSignalParams()
		{
			pauseTime = 10;
			signalType = ESignalType.Unknown;
		}

		private ESignalType signalType;

		public ESignalType SignalType
		{
			get { return signalType; }
			set { signalType = value; }
		}

		public int PauseTime
		{
			get { return pauseTime; }
			set { pauseTime = value; }
		}

		public abstract Type SignalClassType { get; }
		public abstract Form SettingsForm { get; }

		/// <summary>
		/// Сохранение параметров сигнала в XML
		/// </summary>
		/// <param name="pWriter"></param>
		internal virtual void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteElementString("pauseTime", pauseTime.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("signalType", signalType.ToString(CultureInfo.InvariantCulture));
		}

		internal virtual void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			_node = pNode.SelectSingleNode("pauseTime");
			if (_node != null)
				int.TryParse(_node.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out pauseTime);

			_node = pNode.SelectSingleNode("signalType");
			if (_node != null)
			{
				switch (_node.InnerText.ToLowerInvariant())
				{
					case "red":
						signalType = ESignalType.Red;
						break;

					case "green":
						signalType = ESignalType.Green;
						break;

					case "yellow":
						signalType = ESignalType.Yellow;
						break;

					default:
						signalType = ESignalType.Unknown;
						break;
				}
			}
		}
	}

	public enum EBaseSignalTriggerAction
	{
		None,
		ScanStop,
		Accept,
		Decline
	}

	[Serializable]
	public abstract class BaseSignal : IComparable
	{
		#region ================ Поля ================
		Guid id;
		DateTime dateCreated;

		protected string name;
		protected string description;
		protected ESignalType signalType;
		
		protected bool isVisible;

		protected long frequency;
		protected long band;

		/// <summary>
		/// Счетчик общего числа обработанных точек
		/// </summary>
		protected long pointsCount;

		/// <summary>
		/// Счетчик количества попадания в сигнал
		/// </summary>
		protected long hitsCount;

		/// <summary>
		/// Сохраненные трассы по сигналу
		/// </summary>
		/// 
		[NonSerialized]
		SignalTraceCycle[] signalCycles;

		/// <summary>
		/// Время паузы на сигнале при срабатывании
		/// </summary>
		protected int pauseTime;

		[NonSerialized]
		protected bool isSelected;
		#endregion

		protected bool canCreateTrace;


		protected bool canCreateReport;



		#region ================ Конструктор ================
		public BaseSignal()
		{
			id = Guid.NewGuid();
			signalType = ESignalType.Unknown;
			dateCreated = DateTime.Now;
			isVisible = false;

			pointsCount = 0;
			hitsCount = 0;
			frequency = 0;
			band = 0;
			signalCycles = new SignalTraceCycle[50];
			pauseTime = 10;
			isSelected = false;

			canCreateReport = false;
			canCreateTrace = false;
		}
		#endregion

		#region ================ Методы ================
		/// <summary>
		/// Отрисовка сигнала на ZedGraph
		/// </summary>
		/// <param name="pCtrl"></param>
		/// <param name="pDrawChilds"></param>
		public abstract void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds);

		/// <summary>
		/// Определение принадлежности отсчета мощности к сигналу
		/// </summary>
		/// <param name="pTracePoint">Отсчет мощности</param>
		/// <param name="pFilterBand">Полоса фильтра измерителя мощности</param>
		/// <returns></returns>
		public abstract bool IsTracePointBelongs(TracePoint pTracePoint, int pFilterBand);

		public virtual void ClearHitsCount()
		{
			hitsCount = 0;
		}

		/// <summary>
		/// Загрузка параметров сигнала из XML-описания
		/// </summary>
		/// <param name="pNode"></param>
		public virtual void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			if (pNode.Attributes["id"] != null)
			{
				id = new Guid(pNode.Attributes["id"].Value);
			}

			if (pNode.Attributes["frequency"] != null)
				if (!long.TryParse(pNode.Attributes["frequency"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out frequency))
					throw new Exception("Error parsing frequency!");

			if (pNode.Attributes["band"] != null)
				if (!long.TryParse(pNode.Attributes["band"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out band))
					throw new Exception("Error parsing band!");

			if (pNode.Attributes["signaltype"] != null)
			{
				switch (pNode.Attributes["signaltype"].Value.ToLowerInvariant())
				{
					case "red":
						SignalType = ESignalType.Red;
						break;

					case "yellow":
						SignalType = ESignalType.Yellow;
						break;

					case "green":
						SignalType = ESignalType.Green;
						break;

					default:
						SignalType = ESignalType.Unknown;
						break;
				}
			}

			_node = pNode.SelectSingleNode("name");
			name = _node.InnerText;

			_node = pNode.SelectSingleNode("description");
			description = _node.InnerText;

			_node = pNode.SelectSingleNode("pointscount");
			if (_node != null)
				if (!long.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out pointsCount))
					throw new Exception("Error parsing fstop!");

			_node = pNode.SelectSingleNode("hitscount");
			if (_node != null)
				if (!long.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out hitsCount))
					throw new Exception("Error parsing hitsCount!");

		}

		/// <summary>
		/// Сохранение тела сигнала
		/// </summary>
		/// <param name="pWriter"></param>
		protected abstract void SaveBodyToXmlWriter(XmlWriter pWriter);

		/// <summary>
		/// Сохранение параметров сигнала в XML-writer
		/// </summary>
		/// <param name="pWriter"></param>
		public virtual void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteStartElement("Signal");
			pWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);
			pWriter.WriteAttributeString("id", id.ToString());
			pWriter.WriteAttributeString("frequency", frequency.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteAttributeString("band", band.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteAttributeString("signaltype", signalType.ToString());

			pWriter.WriteElementString("name", name);
			pWriter.WriteElementString("description", description);
			pWriter.WriteElementString("pointscount", pointsCount.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("hitscount", hitsCount.ToString(CultureInfo.InvariantCulture));

			SaveBodyToXmlWriter(pWriter);

			pWriter.WriteEndElement();
		}

		protected virtual void CallOnSignalChanged()
		{
			if (OnSignalChanged != null)
				OnSignalChanged(this);
		}

		#endregion

		#region ================ Проперти ================

		/// <summary>
		/// Возможность создания трассы по сигналу
		/// </summary>
		public bool CanCreateTrace
		{
			get { return canCreateTrace; }
			set { canCreateTrace = value; }
		}

		/// <summary>
		/// Возможность создания отчета по сигналу
		/// </summary>
		public bool CanCreateReport
		{
			get { return canCreateReport; }
			set { canCreateReport = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set 
			{ 
				name = value;
				CallOnSignalChanged();
			}
		}

		public string Description
		{
			get { return description; }
			set 
			{ 
				description = value;
				CallOnSignalChanged();
			}
		}

		public Guid Id
		{
			get { return id; }
		}


		public virtual ESignalType SignalType
		{
			get { return signalType; }
			set 
			{ 
				signalType = value;
				CallOnSignalChanged();
			}
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set 
			{ 
				isVisible = value;
				CallOnSignalChanged();
			}
		}

		public long HitsCount
		{
			get 
			{ 
				return hitsCount; 
			}
		}

		public long PointsCount
		{
			get { return pointsCount; }
		}

		public long Frequency
		{
			get { return frequency; }
			set 
			{ 
				frequency = value;
				CallOnSignalChanged();
			}
		}

		public long Band
		{
			get { return band; }
			set 
			{ 
				band = value;
				CallOnSignalChanged();
			}
		}

		public SignalTraceCycle[] SignalCycles
		{
			get { return signalCycles; }
		}

		public int PauseTime
		{
			get { return pauseTime; }
			set
			{
				if (value < 3)
					pauseTime = 3;
				else
					pauseTime = value;
			}
		}

		public Color SignalColor
		{
			get
			{
				Color _color;

				switch (signalType)
				{
					case ESignalType.Red:
						_color = Color.Red;
						break;

					case ESignalType.Yellow:
						_color = Color.Gold;
						break;

					case ESignalType.Green:
						//_color = Color.LightGreen;
						_color = Color.Green;
						break;

					default:
						//_color = Color.LightGray;
						_color = Color.Gray;
						break;

				}

				return _color;
			}
		}

		public bool Selected
		{
			get { return isSelected; }
			set 
			{ 
				isSelected = value;
				CallOnSignalChanged();
			}
		}
		public abstract Form SignalForm { get; }
		public abstract UserControl SignalControl { get; }

		public abstract object SignalParams { get; set; }
		#endregion

		#region ================ Статические методы ================
		public delegate void SignalChangedDelegate(BaseSignal pSignal);

		[field: NonSerialized]
		public virtual event SignalChangedDelegate OnSignalChanged;
		#endregion

		#region ================ Статические методы ================
		static Dictionary<Type, string> signalTypes;

		public static Dictionary<Type, string> SignalTypes
		{
			get
			{
				Type _t = null;

				if (signalTypes == null)
				{
					signalTypes = new Dictionary<Type, string>();
					signalTypes.Add(typeof(RangeSignal), Locale.range_signal);
					signalTypes.Add(typeof(SingleFreqSignal), Locale.single_freq_signal);
				}

				return signalTypes;
			}
		}

		//public static abstract Form DefaultParams { get; }
		
		#endregion

		#region IComparable Members

		public virtual int CompareTo(object obj)
		{
			BaseSignal _s = (BaseSignal)obj;


			if (Frequency < _s.Frequency)
				return -1;
			else if (Frequency > _s.Frequency)
				return 1;
			else return 0;
		}

		#endregion

		public virtual void ProcessPoint(BaseTrace pTrace, TracePoint pTracePoint, double pOldPower)
		{
#if false
			if (!(pTracePoint.Freq >= frequency - band / 2 && pTracePoint.Freq <= frequency + band / 2))
				return;
			var q = from _st in signalCycles
					where (_st != null && _st.Id == pTrace.CycleGuid)
					select _st;

			if (q.Count() == 0)
			{
				/// Создание новой трассы
				/// 
				SignalTraceCycle _stc = new SignalTraceCycle(pTrace.CycleGuid,
					pTrace.Fstart, pTrace.Fstop, pTrace.MeasureStep, 0, signalCycles.Length);

				for (int _i = 0; _i < signalCycles.Length - 1; _i++)
				{
					signalCycles[_i] = signalCycles[_i + 1];
					if (signalCycles[_i] != null)
					{
						signalCycles[_i].CycleNum = signalCycles.Length - _i - 1;
					}
				}

				signalCycles[signalCycles.Length - 1] = _stc;

				_stc.ProcessMeasure(pTracePoint);
			}
			else
			{
				foreach (SignalTraceCycle _stc in q)
				{
					_stc.ProcessMeasure(pTracePoint);
				}
			}
#endif
		}

		public abstract EBaseSignalTriggerAction ProcessTrigger(BaseTraceControl pTraceControl, TracePoint pTracePoint, double pOldPower, double pDelta);

		public abstract void CreateSignalReport();
	}
}
