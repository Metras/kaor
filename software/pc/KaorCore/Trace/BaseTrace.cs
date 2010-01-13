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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using KaorCore.Antenna;
using KaorCore.I18N;
using KaorCore.RadioControlSystem;
using KaorCore.RPU;
using KaorCore.Signal;
using KaorCore.Task;
using KaorCore.TraceControl;

using ZedGraph;

/// Трасса - это непрерывный диапазон значений мощности от Fstart до Fstop
/// Параметрами трассы являются:
///  - антенна, на которой снята трасса
///  - фильтр измерителя мощности РПУ, в Гц
///  - время усреднения измерителя мощности РПУ, в us
///  - ...
///  
/// Одним из методов трассы является оценка измерения мощности и выработка события
/// тревоги по внутреннему критерию
/// Базовая трасса определяет этот метод абстрактным
/// 

namespace KaorCore.Trace
{
	public class TracePoint : IComparable, ICloneable
	{
		/// <summary>
		/// Неопределенная мощность
		/// </summary>
		public const double POWER_UNDEFINED = -145.0d;

		bool locked;
		Int64 freq;
		double power;

		/// <summary>
		/// Время последней модификации
		/// </summary>
		//DateTime timestamp;



		public Int64 Freq
		{
			get { return freq; }
		}

		//public DateTime Timestamp
		//{
		//    get { return timestamp; }
		//}

		public double Power
		{
			get { return power; }
			set
			{
				if (!locked)
				{
					power = value;
					//timestamp = DateTime.Now;
				}
			}
		}

		public TracePoint(Int64 pFreq, double pPower)
		{
			//timestamp = DateTime.Now;
			freq = pFreq;
			power = pPower;
			locked = false;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			TracePoint _tp = obj as TracePoint;
			if (_tp == null)
				throw new ArgumentException("Can compare TracePoint with TracePoint only!");

			if (_tp.freq < freq)
				return -1;
			else if (_tp.freq > freq)
				return 1;
			else
				return 0;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			TracePoint _point = new TracePoint(this.Freq, this.Power);
			//_point.timestamp = this.timestamp;

			return _point;
		}

		#endregion
	}

	public enum ETraceScanMode
	{
		None,
		Scan,
		Control
	}

	/// <summary>
	/// Базовая трасса
	/// </summary>
	public class BaseTrace
	{
		public const int NUM_CYCLES = 10;

		#region ================ Внутренние поля ================

		protected Guid id;
		protected string name;
		protected string description;

		/// <summary>
		/// Точки измерения трассы
		/// </summary>
		protected TracePoint[] measurePoints;

		protected TracePoint[] maxHoldPoints;
		protected TracePoint[] minHoldPoints;

		/// <summary>
		/// Начальная частота трассы
		/// </summary>
		protected Int64 fstart;

		/// <summary>
		/// Конечная частота трассы
		/// </summary>
		protected Int64 fstop;

//		protected int filterBand;
		protected Int64 measureStep;
//		protected int averageTime;

//		protected IAntenna antenna;
//		protected IRPU rpu;

		protected ETraceScanMode scanMode;


		/// <summary>
		/// Элемент списка для отображения в CheckedListBox
		/// </summary>
		//protected ScanTraceItem checkedListItem;

		/// <summary>
		/// Список объектов контроля по трассе
		/// </summary>
		
		BaseTraceControl traceControl;

		/// <summary>
		/// Фильтрованные точки для отображения
		/// </summary>
		TraceFilteredPointList filteredPointList;
		TraceFilteredPointList filteredMaxHoldPointList;
		TraceFilteredPointList filteredMinHoldPointList;

		/// <summary>
		/// Хеш для сохранения значений пропертей для возможности отмены
		/// </summary>
		Hashtable props = null;

		/// <summary>
		/// Признак видимости трассы
		/// </summary>
		bool isVisible;

		bool isSelected;
		/// <summary>
		/// Линия для отрисовки на ZedGraph
		/// </summary>
		protected LineItem lineItem;

		protected LineItem maxHoldLineItem;
		protected LineItem minHoldLineItem;

		/// <summary>
		/// Идентификатор цикла сканирования
		/// </summary>
		protected Guid cycleGuid;

		/// <summary>
		/// Сохраненные трассы по сигналу
		/// </summary>
		/// 
		[NonSerialized]
		SignalTraceCycle[] signalCycles;

		/// <summary>
		/// Признак цикличной трассы
		/// </summary>
		bool isCycle;

		/// <summary>
		/// Признак сохранения каждого цикла трассы
		/// </summary>
		bool isSaveEachCycle;

		/// <summary>
		/// Путь сохранения каждого цикла трассы
		/// </summary>
		string tracesPath;

		TraceScanParams scanParams;

		TraceTaskProvider taskProvider;

		public bool IsLoadScanParamsFailed { get; private set; }

		/// <summary>
		/// Флаг измененной трассы. Устанавливается при изменении параметров трассы,
		/// либо при сканировании по трассе.
		/// Сбрасывается при сохранении трассы.
		/// </summary>
		/// 
		bool isChanged;

		double traceInitValue = TracePoint.POWER_UNDEFINED;

		public bool IsChanged
		{
			get { return isChanged; }
		}

		#endregion

		#region ================ Конструктор ================

		public BaseTrace(Int64 pFstart, Int64 pFstop, long pMeasureStep, double pInitVal, Color pColor)
			: this(pColor)
		{
			if (pMeasureStep == 0)
				throw new ArgumentException("pMeasure step == 0");

			if((pFstop - pFstart) / (Int64)pMeasureStep > 2000000000)
				throw new ArgumentException(Locale.err_points_limit);

			fstart = pFstart;
			fstop = pFstop;
//			filterBand = pFilterBand;
//			averageTime = pAverageTime;
			measureStep = pMeasureStep;
//			rpu = pRPU;
//			antenna = pAntenna;

			PrepareTracePoints(pInitVal);
		}

#if false
		public BaseTrace(Int64 pFstart, Int64 pFstop)
			: this()
		{
			fstart = pFstart;
			fstop = pFstop;
			measureStep = fstop-fstart;
			PrepareTracePoints(TracePoint.POWER_UNDEFINED);

		}
#endif
		public BaseTrace(Color pColor)
		{
			id = Guid.NewGuid();

			name = Locale.new_trace;
			description = Locale.new_trace;
			isVisible = true;

			traceControl = null;

			scanMode = ETraceScanMode.None;

			Random _rnd = new Random();

			lineItem = new LineItem(name,
				filteredPointList,
				pColor, SymbolType.None);

			lineItem.Line.StepType = StepType.ForwardStep;
			lineItem.Tag = this;
			lineItem.IsSelectable = true;

			maxHoldLineItem = new LineItem(name,
				filteredMaxHoldPointList,
				Color.FromArgb(196, Color.Red), SymbolType.None);
			maxHoldLineItem.Line.StepType = StepType.ForwardStep;
			maxHoldLineItem.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
			maxHoldLineItem.Tag = this;
			maxHoldLineItem.IsSelectable = true;

			minHoldLineItem = new LineItem(name,
				filteredMinHoldPointList,
				Color.FromArgb(196, Color.Blue), SymbolType.None);
			minHoldLineItem.Line.StepType = StepType.ForwardStep;
			minHoldLineItem.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
			minHoldLineItem.Tag = this;
			minHoldLineItem.IsSelectable = true;

			cycleGuid = Guid.NewGuid();

			signalCycles = new SignalTraceCycle[NUM_CYCLES];

			/// Трасса по-умолчания циклическая
			isCycle = true;

			/// Сохранение трасс отключено
			isSaveEachCycle = false;

			taskProvider = new TraceTaskProvider(this);
			taskProvider.OnAllTasksComplete += new AllTasksComplete(taskProvider_OnAllTasksComplete);
			taskProvider.OnNewTaskCycle += new NewTaskCycleDelegate(taskProvider_OnNewTaskCycle);

			isChanged = false;
		}

		void taskProvider_OnNewTaskCycle(ITaskProvider pTaskProvider, Guid pTaskCycleGuid)
		{
			cycleGuid = pTaskCycleGuid;
			CallOnNewTraceCycle(cycleGuid);
		}

		void taskProvider_OnAllTasksComplete(ITaskProvider pTaskProvider)
		{
			/// Сохранение трассы
			/// 
			if (isSaveEachCycle)
			{
				try
				{
					XmlTextWriter _writer = new XmlTextWriter(tracesPath + "\\" + Fstart.ToString(CultureInfo.InvariantCulture) + "_" +
						Fstop.ToString(CultureInfo.InvariantCulture) + "_" + DateTime.Now.ToFileTime() + ".ktr", Encoding.UTF8);

					_writer.Formatting = Formatting.Indented;

					SaveToXml(_writer);

					_writer.Close();
				}

				catch
				{
				}
			}
		}

		XmlNode XmlSelectAndCheckNode(XmlNode pNode, string pName)
		{
			XmlNode _node;
			
			_node = pNode.SelectSingleNode(pName);
			if (_node == null)
				throw new InvalidOperationException("Missing element " + pName + "!");

			return _node;

		}

		public BaseTrace(XmlNode pNode)
			: this(Color.LimeGreen)
		{
			try
			{
				XmlNode _node = pNode.SelectSingleNode("id");
				if (_node == null)
					throw new InvalidOperationException("Missign element id!");

				id = new Guid(_node.InnerText);

				name = XmlSelectAndCheckNode(pNode, "name").InnerText;
				description = XmlSelectAndCheckNode(pNode, "description").InnerText;

				if (!Int64.TryParse(XmlSelectAndCheckNode(pNode, "fstart").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out fstart))
					throw new InvalidOperationException("Error parsing fstart!");

				if (!Int64.TryParse(XmlSelectAndCheckNode(pNode, "fstop").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out fstop))
					throw new InvalidOperationException("Error parsing fstop!");

				if (!Int64.TryParse(XmlSelectAndCheckNode(pNode, "measurestep").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out measureStep))
					throw new InvalidOperationException("Error parsing measurestep!");

//				if (!int.TryParse(XmlSelectAndCheckNode(pNode, "filterband").InnerText, out filterBand))
//					throw new InvalidOperationException("Error parsing measurestep!");

//				if (!int.TryParse(XmlSelectAndCheckNode(pNode, "averagetime").InnerText, out averageTime))
//					throw new InvalidOperationException("Error parsing averagetime!");

				
				_node = pNode.SelectSingleNode("tracespath");
				if (_node != null)
					tracesPath = _node.InnerText;

				_node = pNode.SelectSingleNode("iscycle");
				if (_node != null)
					if (!bool.TryParse(_node.InnerText, out isCycle))
						isCycle = true;

				_node = pNode.SelectSingleNode("issaveeachcycle");
				if (_node != null)
					if (!bool.TryParse(_node.InnerText, out isSaveEachCycle))
						isSaveEachCycle = false;

				_node = pNode.SelectSingleNode("traceinitvalue");
				if (_node != null)
					if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out traceInitValue))
						traceInitValue = TracePoint.POWER_UNDEFINED;

				/// Здесь должна быть привязка к РПУ и антенне!!!!!
				/// 
//				antenna = BaseAntenna.GetAntennaByGuid(new Guid(XmlSelectAndCheckNode(pNode, "antenna").InnerText));
//				rpu = BaseRadioControlSystem.Instance.RPUManager.GetRPUById(new Guid(XmlSelectAndCheckNode(pNode, "rpu").InnerText));

				/// Загрузка параметров сканирования
				/// 
				/// Считывание параметров записи сигнала
				_node = pNode.SelectSingleNode("scanparams");

				if (_node != null)
				{
					if (_node.Attributes["type"] == null)
						throw new Exception("No scan params!");

					string _scanTypeName = _node.Attributes["type"].Value;

					Type _t = Type.GetType(_scanTypeName);
					if (_t == null)
						throw new Exception("Can't find type " + _scanTypeName);

					XmlSerializer _s = new XmlSerializer(_t);
					try
					{
						scanParams = _s.Deserialize(new XmlNodeReader(_node.FirstChild)) as TraceScanParams;
					}
					catch
					{
						throw;
					}

					if (scanParams != null)
					{
						IRPU _scanRPU = null;
						IAntenna _scanAntenna = null;
						IsLoadScanParamsFailed = false;

						if (_node.Attributes["rpu"] != null)
						{
							Guid _rpuId = (_node.Attributes["rpu"] != null) ?
								new Guid(_node.Attributes["rpu"].Value) :
								Guid.Empty;

							Guid _rpuTypeId = (_node.Attributes["rputype"] != null) ?
								new Guid(_node.Attributes["rputype"].Value) :
								Guid.Empty;

							_scanRPU = BaseRadioControlSystem.Instance.RPUManager.GetRPUByIdOrType(_rpuId, _rpuTypeId);

							if (_scanRPU == null)
								IsLoadScanParamsFailed = true;
						}

						if (_node.Attributes["antenna"] != null)
						{
							Guid _id = new Guid(_node.Attributes["antenna"].Value);

							_scanAntenna = BaseAntenna.GetAntennaByGuidOrFirst(_id);

							/// Проверка на подключение антенны к РПУ
							if (_scanRPU != null)
								if (!_scanRPU.Antennas.Contains(_scanAntenna))
								{
									_scanAntenna = null;
									IsLoadScanParamsFailed = true;
								}
						}

						if (_scanRPU == null || _scanAntenna == null)
						{
							scanParams = null;
						}
						else
						{
							scanParams.RPU = _scanRPU;
							scanParams.Antenna = _scanAntenna;
						}
					}
				}

				
				
				/// Загрузка точек
				/// 
				PrepareTracePoints(TracePoint.POWER_UNDEFINED);

				_node = XmlSelectAndCheckNode(pNode, "points");

				Random _rnd = new Random();

				foreach (XmlNode _pnt in _node.ChildNodes)
				{
					int _idx;
					double _power;

					if (!int.TryParse(_pnt.Attributes["i"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _idx))
						throw new InvalidOperationException("Error parsing attribute i!");

					if (!double.TryParse(_pnt.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _power))
						throw new InvalidOperationException("Error parsing value p!");

					if (_idx >= 0 && _idx < measurePoints.Length)
						//measurePoints[_idx].Power = _power + 5.0 * Math.Log10(-_power + _rnd.NextDouble() * _power + 0.0001);
						measurePoints[_idx].Power = _power;
				}

				/// Загрузка максимального холда
				_node = pNode.SelectSingleNode("maxholdpoints");
				if (_node != null)
				{
					foreach (XmlNode _pnt in _node.ChildNodes)
					{
						int _idx;
						double _power;

						if (!int.TryParse(_pnt.Attributes["i"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _idx))
							throw new InvalidOperationException("Error parsing attribute i!");

						if (!double.TryParse(_pnt.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _power))
							throw new InvalidOperationException("Error parsing value p!");

						if (_idx >= 0 && _idx < maxHoldPoints.Length)
							maxHoldPoints[_idx].Power = _power;
					}
				}
				/// Загрузка минимального холда
				_node = pNode.SelectSingleNode("minholdpoints");
				if (_node != null)
				{
					foreach (XmlNode _pnt in _node.ChildNodes)
					{
						int _idx;
						double _power;

						if (!int.TryParse(_pnt.Attributes["i"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _idx))
							throw new InvalidOperationException("Error parsing attribute i!");

						if (!double.TryParse(_pnt.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _power))
							throw new InvalidOperationException("Error parsing value p!");

						if (_idx >= 0 && _idx < minHoldPoints.Length)
							minHoldPoints[_idx].Power = _power;
					}
				}
				/// Создание элемента списка с нужным цветом
				/// Создание должно происходить после загрузки списка точек,
				/// т.к. элемент списка содержит линию, которая ссылается на эти самые точки
				_node = XmlSelectAndCheckNode(pNode, "color");
				if (_node != null)
				{
					byte _ta, _tr, _tg, _tb;

					if (!byte.TryParse(_node.Attributes["r"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tr))
						throw new InvalidOperationException("Invalid color R");

					if (!byte.TryParse(_node.Attributes["g"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tg))
						throw new InvalidOperationException("Invalid color G");

					if (!byte.TryParse(_node.Attributes["b"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _tb))
						throw new InvalidOperationException("Invalid color B");

					if (!byte.TryParse(_node.Attributes["a"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _ta))
						throw new InvalidOperationException("Invalid color A");

					lineItem.Color = Color.FromArgb(_ta, _tr, _tg, _tb);
				}

				/// Загрузка параметров контроля
				/// 
				_node = pNode.SelectSingleNode("tracecontrol");
				if (_node != null)
				{
					try
					{
						string _t = _node.Attributes["type"].Value;

						Type _tt = Type.GetType(_t);

						BaseTraceControl _tc = Activator.CreateInstance(_tt, this) as BaseTraceControl;

						if (_tc != null)
							_tc.LoadFromXml(_node);

						traceControl = _tc;
					}

					catch
					{
					}
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Конструктор копирования
		/// </summary>
		/// <param name="pRhs"></param>
		public BaseTrace(BaseTrace pRhs)
		{
			int _i;

			if (pRhs == null)
				throw new ArgumentException("pRhs == null");

			id = Guid.NewGuid();

			name = Locale.new_trace;
			description = Locale.new_trace;

			fstart = pRhs.fstart;
			fstop = pRhs.fstop;
//			filterBand = pRhs.filterBand;
//			averageTime = pRhs.averageTime;
			measureStep = pRhs.measureStep;
//			rpu = pRhs.rpu;

//			antenna = pRhs.antenna;

			/// Аллок массива точек трассы
			measurePoints = new TracePoint[pRhs.measurePoints.Length];

			for (_i = 0; _i < pRhs.measurePoints.Length; _i++)
			{
				measurePoints[_i] = (TracePoint)pRhs.measurePoints[_i].Clone();
			}

			maxHoldPoints = new TracePoint[pRhs.maxHoldPoints.Length];

			for (_i = 0; _i < pRhs.maxHoldPoints.Length; _i++)
			{
				maxHoldPoints[_i] = (TracePoint)pRhs.maxHoldPoints[_i].Clone();
			}
		}

		#endregion

		#region ================ Свойства трассы ================

		public Guid Id
		{
			get { return id; }
		}

		public string Name
		{
			get 
			{ 
				return name;
			}
			set 
			{
				isChanged = true;
				name = value; 
			}
		}

		public string Description
		{
			get { return description; }
			set 
			{
				isChanged = true;
				description = value; 
			}
		}

		/// <summary>
		/// Стартовая частота трассы
		/// </summary>
		public Int64 Fstart 
		{
			get { return fstart; }
			set 
			{ 
				fstart = value;
				isChanged = true;
			}
		}

		/// <summary>
		/// Конечная частота трассы
		/// </summary>
		public Int64 Fstop 
		{
			get { return fstop; }
			set
			{
				fstop = value;
				isChanged = true;
			}
		}

		/// <summary>
		/// Признак цикличной трассы
		/// </summary>
		public bool IsCycle
		{
			get { return isCycle; }
			set 
			{ 
				isCycle = value;
				isChanged = true;
			}
		}

		/// <summary>
		/// Признак сохранения каждого прохода трассы
		/// </summary>
		public bool IsSaveEachCycle
		{
			get { return isSaveEachCycle; }
			set 
			{ 
				isSaveEachCycle = value;
				isChanged = true;
			}
		}

		public string TracesPath
		{
			get { return tracesPath; }
			set 
			{ 
				tracesPath = value;
				isChanged = true;
			}
		}

		public TraceScanParams ScanParams
		{
			get { return scanParams; }
			set 
			{ 
				scanParams = value;
				isChanged = true;
			}
		}
/*
		public IAntenna Antenna 
		{
			get { return null; }
		}

		/// <summary>
		/// Полоса измерителя мощности
		/// </summary>
		public int FilterBand 
		{
			get { return filterBand; }
		}

		/// <summary>
		/// Время усреднения
		/// </summary>
		public int AverageTime 
		{
			get { return averageTime; }
		}
*/
		/// <summary>
		/// Шаг перестройки частоты
		/// </summary>
		public long MeasureStep 
		{
			get { return measureStep; }
			set
			{
				measureStep = value;

				isChanged = true;
			}
		}

		/// <summary>
		/// Получение объекта для работы с ZedGraph
		/// </summary>
		public TraceFilteredPointList FilteredPointList
		{
			get 
			{
				if(filteredPointList == null)
					filteredPointList = new TraceFilteredPointList(measurePoints, 
						(int)measureStep);

				return filteredPointList;
			}
		}


		/// <summary>
		/// Признак контроля по трассе
		/// </summary>

		public ETraceScanMode ScanMode
		{
			get
			{
				return scanMode;
			}

			set
			{
				if (value == ETraceScanMode.Control && traceControl == null)
					scanMode = ETraceScanMode.None;
				else if (value == ETraceScanMode.Scan && scanParams == null)
					scanMode = ETraceScanMode.None;
				else
					scanMode = value;
			}
		}

		/// <summary>
		/// Признак видимости трассы
		/// </summary>
		public bool IsVisible
		{
			get { return isVisible; }
			set 
			{ 
				isVisible = value;

				if (OnTraceVisibleChanged != null)
					OnTraceVisibleChanged(this);
			}
		}

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
			}
		}


		public LineItem LineItem
		{
			get { return lineItem; }
		}

		public TracePoint[] TracePoints
		{
			get { return measurePoints; }
		}
#if false
		public IRPU RPU
		{
			get { return rpu; }
			set
			{
				rpu = value;

/*				if (rpu != null)
				{
					/// Другое РПУ назначается только при совпадении типов и наличии антенн
					if (rpu.GetType() != value.GetType())
						throw new ArgumentException("Для трассы используется РПУ другого типа!");
				}

				if (antenna != null)
				{
					if (!value.Antennas.Contains(antenna))
						throw new ArgumentException("К РПУ не подключена антенна трассы!");
				}

				/// Вроде все ок, можно переключаться
				rpu = value;
 */
			}
		}
#endif
		/// <summary>
		/// Провайдер запросов по трассе
		/// </summary>
		public virtual ITaskProvider TaskProvider 
		{
			get
			{
				return taskProvider;
			}
		}

		/// <summary>
		/// Объект контроля по трассе
		/// </summary>
		public BaseTraceControl TraceControl
		{
			get { return traceControl; }
			set 
			{ 
				traceControl = value;
				isChanged = true;
			}
		}

		public Guid CycleGuid
		{
			get
			{
				return cycleGuid;
			}
		}

		public SignalTraceCycle[] SignalCycles
		{
			get { return signalCycles; }
		}
		#endregion

		#region ================ Методы трассы ================

		/// <summary>
		/// Запуск сканирования
		/// </summary>
		public void StartScan(bool pNeedReset)
		{
			//MessageBox.Show("Запуск сканирования");
			if (scanParams != null && scanParams.RPU != null)
			{
				if (pNeedReset)
				{
					taskProvider.Reset();

					// Сброс максисмального и минимального холда
					for (int _i = 0; _i < measurePoints.Length; _i++)
					{
						maxHoldPoints[_i].Power = measurePoints[_i].Power;
						minHoldPoints[_i].Power = measurePoints[_i].Power;
					}



					for (int _i = 0; _i < signalCycles.Length; _i++)
					{
						signalCycles[_i] = null;
					}
				}

				scanParams.RPU.StartTaskProcessor(taskProvider);
			}
		}

		/// <summary>
		/// Останов сканирования
		/// </summary>
		public void StopScan()
		{
			//MessageBox.Show("Останов сканирования");
			if (scanParams != null && scanParams.RPU != null)
				scanParams.RPU.StopTaskProcessor(taskProvider);
		}

		public bool FindMaxPower(long pFStart, long pFStop, out long pMaxFreq, out double pPower)
		{
			double _maxpower = -1000.0;
			long _maxfreq = 0;

			pMaxFreq = _maxfreq;
			pPower = _maxpower;

			if (pFStop < fstart || pFStart > fstop)
				return false;

			int _sp = (int)((pFStart - fstart) / measureStep - 1);
			if (_sp < 0)
				_sp = 0;

			int _ep = (int)((pFStop - fstart) / measureStep + 1);
			if (_ep > measurePoints.Length - 1)
				_ep = measurePoints.Length - 1;

			

			for (int _i = _sp; _i < _ep; _i++)
			{
				if (_maxpower < measurePoints[_i].Power)
				{
					_maxpower = measurePoints[_i].Power;
					_maxfreq = measurePoints[_i].Freq;
				}
			}

			pPower = _maxpower;
			pMaxFreq = _maxfreq;

			return true;
		}

		protected void CallOnNewTraceCycle(Guid pCycleGuid)
		{
			if(OnNewTraceCycle != null)
				OnNewTraceCycle(pCycleGuid);
		}

		protected void PrepareTracePoints()
		{
			PrepareTracePoints(traceInitValue);
		}

		protected void PrepareTracePoints(double pInitVal)
		{
			/// Расчет количества точек в трассе
			int _npoints = (int)((fstop - fstart) / (Int64)measureStep + 2);

			/// Аллок массива точек трассы
			measurePoints = new TracePoint[_npoints];
			maxHoldPoints = new TracePoint[_npoints];
			minHoldPoints = new TracePoint[_npoints];

			/// Заполнение точек частот измерения
			/// 
			traceInitValue = pInitVal;

			if (traceInitValue < -140.0 || traceInitValue > 30.0)
				traceInitValue = TracePoint.POWER_UNDEFINED;

			for (int _i = 0; _i < measurePoints.Length; _i++)
			{
				measurePoints[_i] = new TracePoint(fstart + (Int64)((Int64)_i * (Int64)measureStep),
					traceInitValue);

				maxHoldPoints[_i] = new TracePoint(fstart + (Int64)((Int64)_i * (Int64)measureStep),
					TracePoint.POWER_UNDEFINED);

				minHoldPoints[_i] = new TracePoint(fstart + (Int64)((Int64)_i * (Int64)measureStep),
					TracePoint.POWER_UNDEFINED);
			}

			lineItem.Points = new TraceFilteredPointList(measurePoints, (int)measureStep);
			maxHoldLineItem.Points = new TraceFilteredPointList(maxHoldPoints, (int)measureStep);
			minHoldLineItem.Points = new TraceFilteredPointList(minHoldPoints, (int)measureStep);

		}

		public delegate void NewTraceMemCycleDelegate(BaseTrace pTrace);
		public delegate void TraceMemCycleChangedDelegate(BaseTrace pTrace);

		public event NewTraceMemCycleDelegate OnNewTraceMemCycle;
		public event TraceMemCycleChangedDelegate OnTraceMemCycleChanged;

		public virtual void ProcessCyclePoint(TracePoint pTracePoint, double pOldPower)
		{
//			if (!(pTracePoint.Freq >= fstart - band / 2 && pTracePoint.Freq <= frequency + band / 2))
//				return;

			var q = from _st in signalCycles
					where (_st != null && _st.Id == CycleGuid)
					select _st;

			if (q.Count() == 0)
			{
				/// Создание новой трассы
				/// 
				SignalTraceCycle _stc = new SignalTraceCycle(cycleGuid,
					fstart, fstop, (int)measureStep, 0, signalCycles.Length);

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

				if (OnNewTraceMemCycle != null)
					OnNewTraceMemCycle(this);
			}
			else
			{
				foreach (SignalTraceCycle _stc in q)
				{
					_stc.ProcessMeasure(pTracePoint);
				}

				if (OnTraceMemCycleChanged != null)
					OnTraceMemCycleChanged(this);
			}
		}

		/// <summary>
		/// Обработка результата измерения
		/// Базовая трасса просто переписывает результат предыдущего измерения
		/// </summary>
		/// <param name="pFreq"></param>
		/// <param name="pPower"></param>
		/// <param name="pNeedUpdate"></param>
		//public void ProcessMeasure(Int64 pFreq, double pPower)
		public void ProcessMeasure(TracePoint pNewPoint)
		{
			/// Проверка на принадлежность точки измерения диапазону трассы
			if (pNewPoint.Freq < fstart || pNewPoint.Freq > fstop)
				return;

			int _pointIdx = (int)((pNewPoint.Freq - fstart) / measureStep);


			// Обработка максимального холда трассы
			TracePoint _maxTp = maxHoldPoints[_pointIdx];
			if (_maxTp.Power == TracePoint.POWER_UNDEFINED || 
				_maxTp.Power < pNewPoint.Power)
				_maxTp.Power = pNewPoint.Power;

			/// Обработка минимального холда трассы
			TracePoint _minTp = minHoldPoints[_pointIdx];
			if (_minTp.Power == TracePoint.POWER_UNDEFINED || 
				_minTp.Power > pNewPoint.Power)
				_minTp.Power = pNewPoint.Power;

			/// Обработка основоной трассы
			TracePoint _tp = measurePoints[_pointIdx];

			/// Перезапись точки трассы новым значением
			//if (_tp.Power != pNewPoint.Power)
			{
				double _oldPower = _tp.Power;
				_tp.Power = pNewPoint.Power;

				/// Обработка цикла трассы
				ProcessCyclePoint(_tp, _oldPower);

				CallOnTraceScanChanged(this, _tp, _oldPower);

				/// Здесь должна производиться оценка принадлежности отсчета мощности известным
				/// сигналам. 
				/// Если сигнал зеленый, то срадатывания триггера не происходит
				if (scanMode == ETraceScanMode.Control)
					CallOnTraceControlChanged(this, _tp, _oldPower);
			}
		}


		/// <summary>
		/// Отрисовка трассы на контроле zedGraph
		/// </summary>
		/// <param name="pCtrl"></param>
		/// <param name="pDrawChilds">Рисовать дочерние элементы (контроли, и т.д.)</param>
		public void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds)
		{
			GraphPane _graphPane = pCtrl.GraphPane;

			if (isVisible)
			{
				/// Отрисовка дочерних объектов, если надо
				if (pDrawChilds == true && traceControl != null)
				{
					if (scanMode == ETraceScanMode.Control)
						traceControl.IsVisible = true;
					else
						traceControl.IsVisible = false;

					traceControl.DrawZedGraph(pCtrl, pDrawChilds);
				}

				if(!_graphPane.CurveList.Contains(lineItem))
					_graphPane.CurveList.Add(lineItem);

			}
			else
			{
				_graphPane.CurveList.Remove(lineItem);


				if (traceControl != null)
				{
					traceControl.IsVisible = false;
					traceControl.DrawZedGraph(pCtrl, pDrawChilds);
				}
			}

			if (isVisible && isSelected)
			{
				if (!_graphPane.CurveList.Contains(maxHoldLineItem))
					_graphPane.CurveList.Add(maxHoldLineItem);

				if (!_graphPane.CurveList.Contains(minHoldLineItem))
					_graphPane.CurveList.Add(minHoldLineItem);
			}
			else
			{
				_graphPane.CurveList.Remove(maxHoldLineItem);
				_graphPane.CurveList.Remove(minHoldLineItem);
			}
		}

		/// <summary>
		/// Враппер события изменения трассы
		/// </summary>
		/// <param name="pTrace"></param>
		/// <param name="pPoint"></param>
		private void CallOnTraceScanChanged(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
		{
			isChanged = true;

			if (OnTraceScanChanged != null)
				OnTraceScanChanged(pTrace, pPoint, pOldPower);
		}

		private void CallOnTraceControlChanged(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
		{
			isChanged = true;

			if (OnTraceControlChanged != null)
				OnTraceControlChanged(pTrace, pPoint, pOldPower);
		}
#if false
		/// <summary>
		/// Добавление объекта контроля к трассе
		/// </summary>
		/// <param name="pTraceControl"></param>
		public void AddTraceControl(BaseTraceControl pTraceControl)
		{
			if (traceControl != null)
			{
				RemoveTraceControl(traceControl);
			}

			OnTraceControlChanged += pTraceControl.TraceChangedHandler;
			traceControl = pTraceControl;
		}

		/// <summary>
		/// Удаление объекта контроля по трасса
		/// </summary>
		/// <param name="pTraceControl"></param>
		public void RemoveTraceControl(BaseTraceControl pTraceControl)
		{
			if (traceControl == null)
				return;

			OnTraceControlChanged -= traceControl.TraceChangedHandler;
			traceControl = null;
		}

		/// <summary>
		/// Очистка списка объектов контроля
		/// </summary>
		public void ClearTraceControls()
		{
			
			OnTraceControlChanged -= traceControl.TraceChangedHandler;
			traceControl = null;
		}
#endif
		/// <summary>
		/// Сохранение трассы в файл
		/// </summary>
		/// <param name="pFileName"></param>
		public virtual void SaveToXml(XmlWriter pXmlWriter)
		{
			try
			{

				pXmlWriter.WriteStartElement("KaorTrace");

				/// Версия и тип трассы
				pXmlWriter.WriteAttributeString("version", "1.0");
				pXmlWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);

				/// Запись параметров трассы
				/// 
				pXmlWriter.WriteElementString("id", id.ToString());

				pXmlWriter.WriteElementString("name", Name);
				pXmlWriter.WriteElementString("description", Description);
//				pXmlWriter.WriteElementString("averagetime", AverageTime.ToString(CultureInfo.InvariantCulture));
				
				/// Цвет трассы
				/// 
				pXmlWriter.WriteStartElement("color");
				pXmlWriter.WriteAttributeString("r", lineItem.Color.R.ToString(CultureInfo.InvariantCulture));
				pXmlWriter.WriteAttributeString("g", lineItem.Color.G.ToString(CultureInfo.InvariantCulture));
				pXmlWriter.WriteAttributeString("b", lineItem.Color.B.ToString(CultureInfo.InvariantCulture));
				pXmlWriter.WriteAttributeString("a", lineItem.Color.A.ToString(CultureInfo.InvariantCulture));

				pXmlWriter.WriteValue(lineItem.Color.ToString());

				pXmlWriter.WriteEndElement(); /// color

				/// Антенна
//				pXmlWriter.WriteStartElement("antenna");
//				pXmlWriter.WriteValue(Antenna.Id.ToString(CultureInfo.InvariantCulture));
//				pXmlWriter.WriteEndElement();

				/// РПУ
//				pXmlWriter.WriteStartElement("rpu");
//				pXmlWriter.WriteValue(RPU.Id.ToString(CultureInfo.InvariantCulture));
//				pXmlWriter.WriteEndElement();

				/// Начальная частота трассы
				pXmlWriter.WriteElementString("fstart", Fstart.ToString(CultureInfo.InvariantCulture));
				/// Конечная частота трассы
				pXmlWriter.WriteElementString("fstop", Fstop.ToString(CultureInfo.InvariantCulture));

				/// Шаг трассы
				pXmlWriter.WriteElementString("measurestep", MeasureStep.ToString(CultureInfo.InvariantCulture));

				/// Полоса фильтра измерителя мощности
//				pXmlWriter.WriteElementString("filterband", FilterBand.ToString(CultureInfo.InvariantCulture));

				/// Пусть сохранения циклов трассы
				pXmlWriter.WriteElementString("tracespath", tracesPath);

				/// Признак цикличности трассы
				pXmlWriter.WriteElementString("iscycle", isCycle.ToString(CultureInfo.InvariantCulture));

				/// Признак сохранения каждого цикла трассы
				pXmlWriter.WriteElementString("issaveeachcycle", isSaveEachCycle.ToString(CultureInfo.InvariantCulture));

				pXmlWriter.WriteElementString("traceinitvalue", traceInitValue.ToString(CultureInfo.InvariantCulture));

				/// Сохранение параметров сканирования
				/// 
				if (scanParams != null && scanParams.RPU != null && scanParams.Antenna != null)
				{
					pXmlWriter.WriteStartElement("scanparams");
					pXmlWriter.WriteAttributeString("type", scanParams.GetType().AssemblyQualifiedName);
					pXmlWriter.WriteAttributeString("rpu", scanParams.RPU.Id.ToString());
					pXmlWriter.WriteAttributeString("rputype", scanParams.RPU.RPUType.ToString());
					pXmlWriter.WriteAttributeString("antenna", scanParams.Antenna.Id.ToString());
					pXmlWriter.WriteAttributeString("antennatype", scanParams.Antenna.AntennaType.ToString());
					/// Сохранение параметров записи сигнала
					/// 
					XmlSerializer _s = new XmlSerializer(scanParams.GetType());
					_s.Serialize(pXmlWriter, scanParams);

					pXmlWriter.WriteEndElement();
				}

				pXmlWriter.WriteStartElement("tracecontrol");

				if (traceControl != null)
				{
					pXmlWriter.WriteAttributeString("type", traceControl.GetType().AssemblyQualifiedName);
					traceControl.SaveToXml(pXmlWriter);
				}
				
				pXmlWriter.WriteEndElement();

				/// Запись точек трассы
				pXmlWriter.WriteStartElement("points");

				int _idx = 0;
				foreach (TracePoint _pnt in TracePoints)
				{
					if (_pnt.Power != TracePoint.POWER_UNDEFINED)
					{
						pXmlWriter.WriteStartElement("p");
						pXmlWriter.WriteAttributeString("i", _idx.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteString(_pnt.Power.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteEndElement();
					}

					_idx++;
				}

				pXmlWriter.WriteEndElement(); /// points

				pXmlWriter.WriteStartElement("maxholdpoints");

				_idx = 0;
				foreach (TracePoint _pnt in maxHoldPoints)
				{
					if (_pnt.Power != TracePoint.POWER_UNDEFINED)
					{
						pXmlWriter.WriteStartElement("p");
						pXmlWriter.WriteAttributeString("i", _idx.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteString(_pnt.Power.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteEndElement();
					}

					_idx++;
				}

				pXmlWriter.WriteEndElement(); /// maxholdpoints

				pXmlWriter.WriteStartElement("minholdpoints");

				_idx = 0;
				foreach (TracePoint _pnt in minHoldPoints)
				{
					if (_pnt.Power != TracePoint.POWER_UNDEFINED)
					{
						pXmlWriter.WriteStartElement("p");
						pXmlWriter.WriteAttributeString("i", _idx.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteString(_pnt.Power.ToString(CultureInfo.InvariantCulture));
						pXmlWriter.WriteEndElement();
					}

					_idx++;
				}

				pXmlWriter.WriteEndElement(); /// minholdpoints
				pXmlWriter.WriteEndElement(); /// KaorTrace

			}

			catch
			{
				throw;
			}
		}
#if false
		/// <summary>
		/// Заполнение параметров трассы из диалогового окна
		/// </summary>
		/// <returns></returns>
		public bool UserFillParamsFromDialog(bool pAsk)
		{
			bool _res = false;

			if (pAsk)
			{
				if (MessageBox.Show("Трасса содержит точки измерения, действительно продолжить?", "Подтверждение",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)

					return false;
			}

			NewTraceDialog _dlg = new NewTraceDialog(false);

			_dlg.Trace = this;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				PrepareTracePoints();
				lineItem.Color = _dlg.cmbTraceColor.Value;
				_res = true;
			}

			return _res;
		}
#endif

		public bool UserEditParams()
		{
			bool _res = false;

			NewTraceDialog _dlg = new NewTraceDialog(false);
			_dlg.FStart = fstart;
			_dlg.FStop = fstop;
			_dlg.TraceName = name;
			_dlg.Description = description;
			_dlg.MeasureStep = measureStep;
			_dlg.IsCyclic = isCycle;
			_dlg.IsNeedSave = isSaveEachCycle;
			_dlg.SavePath = tracesPath;
			_dlg.cmbTraceColor.Value = lineItem.Color;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				if (fstart != _dlg.FStart ||
					fstop != _dlg.FStop ||
					measureStep != _dlg.MeasureStep)
				{
					if (MessageBox.Show(Locale.confirm_trace_change_params, 
						Locale.confirmation,
						MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
					{
						/// Выход без изменения параметров трассы
						return false;
					}
					
					fstart = _dlg.FStart;
					fstop = _dlg.FStop;
					measureStep = _dlg.MeasureStep;
					PrepareTracePoints(_dlg.InitialValue);
				}

				/// Параметры, не требющие перестройки трассы
				name = _dlg.TraceName;
				description = _dlg.Description;
				isCycle = _dlg.IsCyclic;
				isSaveEachCycle = _dlg.IsNeedSave;
				tracesPath = _dlg.SavePath;
				lineItem.Color = _dlg.cmbTraceColor.Value;

				_res = true;
			}

			return _res;
		}
		#endregion

		#region ================ События трассы ================

		void CallOnTraceChanged()
		{
			if (OnTraceChanged != null)
				OnTraceChanged(this);
		}

		/// <summary>
		/// Событие триггера тревоги
		/// </summary>
		//public event TraceTrigger OnTraceTrigger;

		public event TraceChanged OnTraceScanChanged;
		public event TraceChanged OnTraceControlChanged;
		public event NewTraceCycleDelegate OnNewTraceCycle;
		public event TraceChangedDelegate OnTraceChanged;
		public event TraceChangedDelegate OnTraceVisibleChanged;


		#endregion

#if false
		#region IRPUAssignable Members

		/// <summary>
		/// Принудительное освобождение РПУ
		/// </summary>
		/// <param name="pRPU"></param>
		public void ForceReleaseRPU(IRPU pRPU)
		{
			ReleaseRPU();
		}

		#endregion
#endif

		#region IEditableObject Members

		public void BeginEdit()
		{
			//exit if in Edit mode
			//uncomment if  CancelEdit discards changes since the 
			//LAST BeginEdit call is desired action
			//otherwise CancelEdit discards changes since the 
			//FIRST BeginEdit call is desired action

			//if (null != props) return;
			//enumerate properties

			PropertyInfo[] properties = (this.GetType()).GetProperties
						(BindingFlags.Public | BindingFlags.Instance);

			props = new Hashtable(properties.Length - 1);

			for (int i = 0; i < properties.Length; i++)
			{
				//check if there is set accessor

				if (null != properties[i].GetSetMethod())
				{
					object value = properties[i].GetValue(this, null);
					props.Add(properties[i].Name, value);
				}
			}
		}

		public void CancelEdit()
		{
			//check for inappropriate call sequence

			if (null == props) return;

			//restore old values

			PropertyInfo[] properties = (this.GetType()).GetProperties
				(BindingFlags.Public | BindingFlags.Instance);
			for (int i = 0; i < properties.Length; i++)
			{
				//check if there is set accessor

				if (null != properties[i].GetSetMethod())
				{
					object value = props[properties[i].Name];
					properties[i].SetValue(this, value, null);
				}
			}

			//delete current values

			props = null;
		}

		public void EndEdit()
		{
			props = null;
		}

		#endregion

		internal void NewId()
		{
			id = Guid.NewGuid();
		}

		public void Reset()
		{
			PrepareTracePoints();

			if (taskProvider != null)
				taskProvider.Reset();

			if (traceControl != null)
				traceControl.ReInitialize();
		}
	}

	/// <summary>
	/// Делегат события триггера тревоги
	/// </summary>
	/// <param name="pTrace"></param>
	/// <param name="pFreq"></param>
	/// <param name="pPower"></param>
	/// <param name="pDelta"></param>
//	public delegate void TraceTrigger(BaseTrace pTrace, TracePoint pPoint, double pMeasuredPower);

	public delegate void TraceChanged(BaseTrace pTrace, TracePoint pPoint, double pOldPower);
	
	public delegate void NewTraceCycleDelegate(Guid pCycleGuid);
	public delegate void TraceChangedDelegate(BaseTrace pTrace);
}
