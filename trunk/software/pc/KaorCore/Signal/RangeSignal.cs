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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using KaorCore.I18N;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.RadioControlSystem;
using KaorCore.Trace;
using KaorCore.TraceControl;
using KaorCore.Report;
using KaorCore.Utils;
using ZedGraph;

namespace KaorCore.Signal
{
	public class RangeSignalParams : BaseSignalParams
	{
		double pmin;

		public double Pmin
		{
			get { return pmin; }
			set { pmin = value; }
		}
		double pmax;

		public double Pmax
		{
			get { return pmax; }
			set { pmax = value; }
		}
		int bandMultiplier;

		public int BandMultiplier
		{
			get { return bandMultiplier; }
			set { bandMultiplier = value; }
		}

		bool isRecord;

		public bool IsRecord
		{
			get { return isRecord; }
			set { isRecord = value; }
		}
		bool isFixDeltaPlus;

		public bool IsFixDeltaPlus
		{
			get { return isFixDeltaPlus; }
			set { isFixDeltaPlus = value; }
		}
		bool isFixDeltaMinus;

		public bool IsFixDeltaMinus
		{
			get { return isFixDeltaMinus; }
			set { isFixDeltaMinus = value; }
		}

		bool increaseRange;

		public bool IncreaseRange
		{
			get { return increaseRange; }
			set { increaseRange = value; }
		}

		Guid recordRPUId;

		public Guid RecordRPUId
		{
			get { return recordRPUId; }
			set { recordRPUId = value; }
		}

		
		object defaultRecordParams;

		public object DefaultRecordParams
		{
			get { return defaultRecordParams; }
			set { defaultRecordParams = value; }
		}

		public override Type SignalClassType
		{
			get 
			{
				return typeof(RangeSignal);
			}
		}

		public RangeSignalParams()
		{
			pmin = -140.0;
			pmax = 30.0;
			isRecord = true;
			isFixDeltaMinus = true;
			isFixDeltaPlus = true;
			increaseRange = false;
			bandMultiplier = 2;
			increaseRange = false;
			defaultRecordParams = null;
		}

		/// <summary>
		/// Форма для задания параметров по-умолчанию
		/// </summary>
		public override Form SettingsForm
		{
			get
			{
				RangeSignalDefaultSettings _dlg = new RangeSignalDefaultSettings(this);

				return _dlg;
			}
		}

		internal override void SaveToXmlWriter(XmlWriter pWriter)
		{
			base.SaveToXmlWriter(pWriter);

			pWriter.WriteElementString("pmin", pmin.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("pmax", pmax.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixplus", isFixDeltaPlus.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixminus", isFixDeltaMinus.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isrecord", isRecord.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("incrange", increaseRange.ToString(CultureInfo.InvariantCulture));

			if (recordRPUId != Guid.Empty && defaultRecordParams != null)
			{
				pWriter.WriteElementString("recrpu", recordRPUId.ToString());

				/// Сохранение параметров записи сигнала
				/// 
				XmlSerializer _s = new XmlSerializer(defaultRecordParams.GetType());
				pWriter.WriteStartElement("recparams");
				pWriter.WriteAttributeString("type", defaultRecordParams.GetType().AssemblyQualifiedName);

				_s.Serialize(pWriter, defaultRecordParams);

				pWriter.WriteEndElement();
			}
		}

		internal override void LoadFromXmlNode(XmlNode pNode)
		{
			XmlNode _node;

			base.LoadFromXmlNode(pNode);

			_node = pNode.SelectSingleNode("pmin");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out pmin))
					throw new Exception("Error parsing pmin!");

			_node = pNode.SelectSingleNode("pmax");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out pmax))
					throw new Exception("Error parsing pmax!");

			_node = pNode.SelectSingleNode("isfixplus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaPlus))
					throw new Exception("Error parsing isfixplus!");

			_node = pNode.SelectSingleNode("isfixminus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaMinus))
					throw new Exception("Error parsing isfixminus!");

			_node = pNode.SelectSingleNode("isrecord");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isRecord))
					throw new Exception("Error parsing isrecord!");

			_node = pNode.SelectSingleNode("incrange");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out increaseRange))
					throw new Exception("Error parsing incrange!");

			_node = pNode.SelectSingleNode("recrpu");

			if (_node != null)
			{
				string _rpuId;

				if (_node != null)
				{
					recordRPUId = new Guid(_node.InnerText);

					//recordRPU = BaseRadioControlSystem.Instance.RPUManager.GetRPUById(new Guid(_rpuId));
				}
			}
			/// Считывание параметров записи сигнала
			_node = pNode.SelectSingleNode("recparams");
			if (_node != null)
			{
				if (_node.Attributes["type"] == null)
					throw new Exception("NULL recparams type!");

				string _recTypeName = _node.Attributes["type"].Value;

				Type _t = Type.GetType(_recTypeName);
				if (_t == null)
					throw new Exception("Can't find type " + _recTypeName);

				XmlSerializer _s = new XmlSerializer(_t);
				try
				{
					defaultRecordParams = _s.Deserialize(new XmlNodeReader(_node.FirstChild));
				}
				catch
				{
					throw;
				}
			}
		}
	}
	/// <summary>
	/// Класс сигнала, описываемого параметрами:
	/// FSTART - начальная частота
	/// FSTOP - конечная частота
	/// PMAX - максимальная мощность
	/// PMIN - минимальная мощность
	/// </summary>
	/// 
	[Serializable]
	public class RangeSignal : BaseSignal
	{
		#region ================ Поля ================
		/// <summary>
		/// Максимальная мощность в полосе сигнала
		/// </summary>
		double pmax;
		/// <summary>
		/// Минимальная мощность в полосе сигнала
		/// </summary>
		double pmin;

		[NonSerialized]
		BoxObj signalBox;

		[NonSerialized]
		RangeSignalControl signalControl;

		//[NonSerialized]
		//Guid currentCycleGuid;

		bool isRecord;
		bool isFixDeltaPlus;
		bool isFixDeltaMinus;

		[NonSerialized]
		IRPU recordRPU;
		
		Guid recordRPUId;

		object recordRPUParams;
		//int recordTime;

		bool increaseRange;

		List<RangeSignalHitPoint> signalHitPoints;

#if USE_SIGNAL_REPOTRS
		[NonSerialized]
		List<ReportItem> reports;
#endif
		#endregion

		#region ================ Конструктор ================
		public RangeSignal()
			: base()
		{
			name = Locale.range_signal;
			description = Locale.range_signal;
			pmin = 0;
			pmax = 0;
			SignalType = ESignalType.Unknown;
			isRecord = true;
			isFixDeltaMinus = true;
			isFixDeltaPlus = true;
			increaseRange = false;

			signalHitPoints = new List<RangeSignalHitPoint>();
#if USE_SIGNAL_REPORTS
			reports = new List<ReportItem>();
#endif
			signalBox = new BoxObj(0, 0, 1.0, 1.0);
			signalBox.Location.CoordinateFrame = CoordType.AxisXYScale;
			signalBox.Fill.Type = FillType.Solid;
			signalBox.Border.IsVisible = false;
			signalBox.IsClippedToChartRect = true;
			signalBox.Tag = this;

			canCreateReport = true;
			canCreateTrace = true;
		}

		/// <summary>
		/// Конструктор создания сигнала с использованием объекта параметров
		/// по-умолчанию
		/// </summary>
		/// <param name="pDefaultParams"></param>
		/// <param name="pTraceControl"></param>
		/// <param name="pTracePoint"></param>
		/// <param name="pOldPower"></param>
		/// <param name="pDelta"></param>
		public RangeSignal(object pDefaultParams, BaseTraceControl pTraceControl,
			TracePoint pTracePoint, double pOldPower, double pDelta)
			: this()
		{
			RangeSignalParams _signalParams = pDefaultParams as RangeSignalParams;

			if (_signalParams == null)
				return;

			pmin = _signalParams.Pmin;
			pmax = _signalParams.Pmax;
			isFixDeltaPlus = _signalParams.IsFixDeltaPlus;
			isFixDeltaMinus = _signalParams.IsFixDeltaMinus;
			isRecord = _signalParams.IsRecord;
			frequency = pTracePoint.Freq;
			band = pTraceControl.ScanTrace.ScanParams.FilterBand * _signalParams.BandMultiplier;
			signalType = _signalParams.SignalType;
			pauseTime = _signalParams.PauseTime;
			increaseRange = _signalParams.IncreaseRange;

			recordRPU = pTraceControl.ScanTrace.ScanParams.RPU;

			if (recordRPU.Id == _signalParams.RecordRPUId)
			{
				recordRPUParams = _signalParams.DefaultRecordParams;
			}
			else
			{
				recordRPUParams = recordRPU.DefaultSignalRecordParams;
			}
		}

		public RangeSignal(BaseTraceControl pTraceControl)
			: this()
		{
			recordRPU = pTraceControl.ScanTrace.ScanParams.RPU;
			recordRPUParams = recordRPU.DefaultSignalRecordParams;
		}

		#endregion

		#region ================ Проперти ================

		public double Pmax
		{
			get { return pmax; }
			set 
			{ 
				pmax = value;
				CallOnSignalChanged();
			}
		}

		public double Pmin
		{
			get { return pmin; }
			set 
			{ 
				pmin = value;
				CallOnSignalChanged();
			}
		}

		public override ESignalType SignalType
		{
			get
			{
				return signalType;
			}
			set
			{
				signalType = value;
//				SignalBox.Fill.Color = Color.FromArgb(196, SignalColor);
				CallOnSignalChanged();
			}
		}

		public override System.Windows.Forms.Form SignalForm
		{
			get 
			{
				throw new NotImplementedException();
			}
		}

		public override UserControl SignalControl
		{
			get
			{
				if (signalControl == null)
				{
					signalControl = new RangeSignalControl(this);
					signalControl.OnSignalEditComplete += new RangeSignalControl.SignalEditComplete(signalControl_OnSignalEditComplete);
				}

				return signalControl;
			}
		}

		void signalControl_OnSignalEditComplete(RangeSignal pSignal)
		{
			CallOnSignalChanged();
		}

		public bool IsRecord
		{
			get
			{
				return isRecord;
			}

			set
			{
				isRecord = value;
			}
		}

		public bool IsFixDeltaPlus
		{
			get { return isFixDeltaPlus; }
			set { isFixDeltaPlus = value; }
		}

		public bool IsFixDeltaMinus
		{
			get { return isFixDeltaMinus; }
			set { isFixDeltaMinus = value; }
		}

		public IRPU RecordRPU
		{
			get 
			{
				if (recordRPU == null && recordRPUId != null)
				{
					/// Выбор приемника по Id
					/// 
					BaseRadioControlSystem.Instance.RPUManager.GetRPUById(recordRPUId);
				}
				return recordRPU; 
			}
			set 
			{ 
				recordRPU = value;
				if (recordRPU != null)
				{
					RecordRPUParams = recordRPU.DefaultSignalRecordParams;
					recordRPUId = recordRPU.Id;
				}
				else
				{
					recordRPUId = Guid.NewGuid();
					RecordRPUParams = null;
				}
			}
		}

		public object RecordRPUParams
		{
			get { return recordRPUParams; }
			set { recordRPUParams = value; }
		}

		public bool IncreaseRange
		{
			get { return increaseRange; }
			set { increaseRange = value; }
		}

		public override object SignalParams
		{
			get { return recordRPUParams; }
			set { recordRPUParams = value; }
		}

		public List<RangeSignalHitPoint> SignalHitPoints
		{
			get { return signalHitPoints; }
		}

#if USE_SIGNAL_REPORTS
		public List<ReportItem> Reports
		{
			get 
			{ 
				return reports; 
			}
		}
#endif
		#endregion

		#region ================ Методы ================

		public override void ClearHitsCount()
		{
			base.ClearHitsCount();

			signalHitPoints.Clear();
		}

		/// <summary>
		/// Отрисовка сигнала на ZedGraph
		/// </summary>
		/// <param name="pCtrl"></param>
		/// <param name="pDrawChilds"></param>
		public override void DrawZedGraph(ZedGraphControl pCtrl, bool pDrawChilds)
		{

			if (isVisible)
			{

				/// Расчет положения сигнала
				/// 
				signalBox.Location.X = frequency - band / 2;
				signalBox.Location.Y = pmax;
				signalBox.Location.Width = band;
				signalBox.Location.Height = pmin - pmax;
				
				signalBox.IsVisible = true;
				signalBox.Fill.Color = Color.FromArgb(196, SignalColor);

				if (!pCtrl.GraphPane.GraphObjList.Contains(signalBox))
				{
					pCtrl.GraphPane.GraphObjList.Add(signalBox);
				}
			}
			else
			{
				pCtrl.GraphPane.GraphObjList.Remove(signalBox);
			}
		}

		/// <summary>
		/// Определение попадания точки отсчета в сигнал
		/// </summary>
		/// <param name="pTracePoint"></param>
		/// <param name="pFilterBand"></param>
		/// <returns></returns>
		public override bool IsTracePointBelongs(TracePoint pTracePoint, int pFilterBand)
		{
			bool _res = false;

			/// Инкремент общего числа обработанных точек
			pointsCount++;

			if (increaseRange)
			{
				/// Анализ расширения сигнала
				if ((pTracePoint.Freq >= frequency - band / 2 - pFilterBand &&
					pTracePoint.Freq <= frequency - band / 2) ||
					(pTracePoint.Freq >= frequency + band / 2 + pFilterBand &&
					pTracePoint.Freq <= frequency + band / 2))
				{
					/// Точка попадает в прилежащую к сигналу область 
					/// на +-полосы фильтра
					band += pFilterBand;
				}
			}

			if (pTracePoint.Freq >= frequency - band / 2 &&
				pTracePoint.Freq <= frequency + band / 2 &&
				pTracePoint.Power >= pmin &&
				pTracePoint.Power <= pmax)
			{
				_res = true;
			}
			else
			{
				_res = false;
			}

			return _res;
		}

		public override void LoadFromXmlNode(System.Xml.XmlNode pNode)
		{
			XmlNode _node;

			base.LoadFromXmlNode(pNode);

			_node = pNode.SelectSingleNode("pmin");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out pmin))
					throw new Exception("Error parsing pmin!");

			_node = pNode.SelectSingleNode("pmax");
			if (_node != null)
				if (!double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out pmax))
					throw new Exception("Error parsing pmax!");

			_node = pNode.SelectSingleNode("isfixplus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaPlus))
					throw new Exception("Error parsing isfixplus!");

			_node = pNode.SelectSingleNode("isfixminus");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isFixDeltaMinus))
					throw new Exception("Error parsing isfixminus!");

			_node = pNode.SelectSingleNode("isrecord");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out isRecord))
					throw new Exception("Error parsing isrecord!");

			_node = pNode.SelectSingleNode("incrange");
			if (_node != null)
				if (!bool.TryParse(_node.InnerText, out increaseRange))
					throw new Exception("Error parsing incrange!");

			_node = pNode.SelectSingleNode("recrpu");

			if(_node != null)
			{
				string _rpuId;

				if (_node != null)
				{
					_rpuId = _node.InnerText;

					recordRPU = BaseRadioControlSystem.Instance.RPUManager.GetRPUById(new Guid(_rpuId));
				}
			}
			/// Считывание параметров записи сигнала
			_node = pNode.SelectSingleNode("recparams");
			if (_node != null)
			{
				if (_node.Attributes["type"] == null)
					throw new Exception("NULL recparams type!");

				string _recTypeName = _node.Attributes["type"].Value;

				Type _t = Type.GetType(_recTypeName);
				if (_t == null)
					throw new Exception("Can't find type " + _recTypeName);

				XmlSerializer _s = new XmlSerializer(_t);
				try
				{
					recordRPUParams = _s.Deserialize(new XmlNodeReader(_node.FirstChild));
				}
				catch
				{
					throw;
				}
			}

			_node = pNode.SelectSingleNode("hitpoints");
			if (_node != null)
			{
				XmlSerializer _pntSerializer = new XmlSerializer(typeof(RangeSignalHitPoint));

				foreach (XmlNode _pntNode in _node.ChildNodes)
				{
					XmlNodeReader _reader = new XmlNodeReader(_pntNode);
					RangeSignalHitPoint _hitPoint = (RangeSignalHitPoint)_pntSerializer.Deserialize(_reader);

					signalHitPoints.Add(_hitPoint);
				}
			}
		}

		#endregion

		/// <summary>
		/// Сохранение тела сигнала в xml
		/// </summary>
		/// <param name="pWriter"></param>
		protected override void SaveBodyToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteElementString("pmin", pmin.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("pmax", pmax.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixplus", isFixDeltaPlus.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isfixminus", isFixDeltaMinus.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("isrecord", isRecord.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("incrange", increaseRange.ToString(CultureInfo.InvariantCulture));

			if (recordRPU != null && recordRPUParams != null)
			{
				pWriter.WriteElementString("recrpu", recordRPU.Id.ToString());

				/// Сохранение параметров записи сигнала
				/// 
				XmlSerializer _s = new XmlSerializer(recordRPUParams.GetType());
				pWriter.WriteStartElement("recparams");
				pWriter.WriteAttributeString("type", recordRPUParams.GetType().AssemblyQualifiedName);

				_s.Serialize(pWriter, recordRPUParams);

				pWriter.WriteEndElement();
			}

			pWriter.WriteStartElement("hitpoints");

			XmlSerializer _pntSerializer = new XmlSerializer(typeof(RangeSignalHitPoint));

			foreach (RangeSignalHitPoint _pnt in signalHitPoints)
			{
				_pntSerializer.Serialize(pWriter, _pnt);
			}

			pWriter.WriteEndElement();
		}

		public override void ProcessPoint(BaseTrace pTrace, TracePoint pTracePoint, double pOldPower)
		{
			base.ProcessPoint(pTrace, pTracePoint, pOldPower);
		}

		/// <summary>
		/// Обработка срабатывания по контролю трассы
		/// </summary>
		/// <param name="pTraceControl"></param>
		/// <param name="pTracePoint"></param>
		/// <param name="pOldPower"></param>
		public override EBaseSignalTriggerAction ProcessTrigger(BaseTraceControl pTraceControl, TracePoint pTracePoint, 
			double pOldPower, double pDelta)
		{
			EBaseSignalTriggerAction _action = EBaseSignalTriggerAction.None;

			/// Обработка фиксации положительного срабатывания (появление)
			if (!((isFixDeltaPlus && pDelta >= 0) || (isFixDeltaMinus && pDelta <= 0)))
				return _action;

			bool _needRecord = (isRecord && 
			    recordRPU != null && 
			    recordRPUParams != null );

			if (_needRecord == false)
			{

				hitsCount++;

				/// Создание записи о попадании
				/// 
				//			throw new NotImplementedException("old traces");
				RangeSignalHitPoint _hitPoint = new RangeSignalHitPoint(pTracePoint.Freq,
					pTraceControl.ScanTrace.ScanParams.FilterBand,
					pTraceControl.ScanTrace.ScanParams.AverageTime,
					pTraceControl.ScanTrace.ScanParams.Antenna,
					pTraceControl.ScanTrace.ScanParams.RPU,
					pTraceControl, pTracePoint.Power, pDelta, "", _needRecord);

				signalHitPoints.Add(_hitPoint);
				_action = EBaseSignalTriggerAction.Accept;
			}
			else
			{
				/// Запись сигнала
				/// 
				recordRPU.StartRecordSignal(this, recordRPUParams, new NewRecordInfoDelegate(NewRecordInfo), pTraceControl.ScanTrace);
				
				DialogResult _res = BaseRadioControlSystem.Instance.PauseArmedMode(pauseTime, 
					pTracePoint.Freq, 
					pTracePoint.Power,
					name, 
					SignalColor, 
					pDelta);

				switch (_res)
				{
					case DialogResult.Yes:
						_action = EBaseSignalTriggerAction.Accept;
						break;

					case DialogResult.No:
						_action = EBaseSignalTriggerAction.Decline;
						break;

					default:
						_action = EBaseSignalTriggerAction.ScanStop;
						break;
				}

				string _tmpRecordName = recordRPU.StopRecordSignal(recordRPUParams);

				if (_action == EBaseSignalTriggerAction.Accept)
				{
					hitsCount++;

					/// Создание записи о попадании
					/// 
					//			throw new NotImplementedException("old traces");
					RangeSignalHitPoint _hitPoint = new RangeSignalHitPoint(pTracePoint.Freq,
						pTraceControl.ScanTrace.ScanParams.FilterBand,
						pTraceControl.ScanTrace.ScanParams.AverageTime,
						pTraceControl.ScanTrace.ScanParams.Antenna,
						pTraceControl.ScanTrace.ScanParams.RPU,
						pTraceControl, pTracePoint.Power, pDelta, "", _needRecord);

					signalHitPoints.Add(_hitPoint);

					if (_tmpRecordName != null && _tmpRecordName != "")
					{
						/// Получение файла отчета
						ReportItem _report = ReportsManager.NewReport(ReportType.SignalAudio, name);

						/// Перемещение временного файла в отчет
						/// 
						File.Move(_tmpRecordName, _report.FullFileName);
#if USE_SIGNAL_REPORTS
					reports.Add(_report);
#endif
						CallOnSignalChanged();
					}
				}
			}

			return _action;
		}

		/// <summary>
		/// Метод обработки новой информации о записи
		/// </summary>
		/// <param name="pRecordInfo"></param>
		void NewRecordInfo(ReportItem pRecordInfo)
		{
#if USE_SIGNAL_REPORTS
			reports.Add(pRecordInfo);
#endif
			CallOnSignalChanged();
		}

		public void ShowRecordParams()
		{
			if (recordRPU != null)
			{
				object _recP = recordRPU.ShowRecordParamsDialog(recordRPUParams);

				if (_recP != null)
				{
					recordRPUParams = _recP;
				}
			}
		}

		/// <summary>
		/// Создание отчета по сигналу
		/// </summary>
		public override void CreateSignalReport()
		{
			String FileName = ReportsManager.NewReportName(
					ReportType.PDF,
					String.Format("{0} ({1})", this.Name, KaorCore.Utils.FreqUtils.FreqToString(this.Frequency)));
			new RangeSignalReport(this).Save(FileName);
			System.Diagnostics.Process.Start(FileName);
		}
	}
}
