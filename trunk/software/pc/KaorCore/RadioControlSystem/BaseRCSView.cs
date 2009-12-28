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
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using ControlUtils.ObjectListView;
using KaorCore.Antenna;
using KaorCore.I18N;
using KaorCore.Marker;
using KaorCore.RPU;
using KaorCore.RPUManager;
using KaorCore.Signal;
using KaorCore.Trace;
using KaorCore.TraceControl;
using KaorCore.Utils;

using ZedGraph;
using KaorCore.ZedGraphAddons;
using KaorCore.Report;

namespace KaorCore.RadioControlSystem
{
	enum EWaterfallMode
	{
		Replay,
		ZeroSpan,
		Waterfall
	};

	struct SignalAddParams
	{
		public double power;
		public long frequency;
		public BaseTrace trace;
	};

	public partial class BaseRCSView : UserControl
	{

		public delegate void CursorFrequencyChangedDelegate(Int64 pFrequency);
		public event CursorFrequencyChangedDelegate OnCursorFrequencyChanged;

		#region Тестовые массивы
		MyFilteredPointList list, list2, list3;
		#endregion

		const int GRAPHPANE_N_POINTS = 800;

		#region ================ Поля ================

		EWaterfallMode waterfallMode;

		BaseRadioControlSystem rcs;
		FrequencyInputDialog freqInputDialog;
		BoxObj navigatorHighlight;
		LineItem cursorLine;
		PointPairList cursorPoints;
		Int64 cursorFrequency;
		
		/// <summary>
		/// Событие запуска отрисовки
		/// </summary>
		AutoResetEvent evtRedraw;

		bool needRedraw;
		object needRedrawLock;

		bool shiftPressed;
		bool ctrlPressed;

		BaseSignal signalChart;
		
		/// <summary>
		/// Менеджер ресурсов для локализции
		/// </summary>
		ResourceManager rm;

		/// <summary>
		/// Выбранный сигнал
		/// </summary>
		BaseSignal selectedSignal;

		private const long _rcsFMin = 0;
		private const long _rcsFMax = 40000000000;

		#endregion

		#region ================ Конструктор ================
		public BaseRCSView()
		{
			InitializeComponent();
			
			
			this.xPanderPanelList1.Expand(xPanelTraces);
			this.splitContainer1.Panel1MinSize = 600;
			this.splitContainer1.Panel2MinSize = 250;
			this.xPanderPanelList1.Padding = new Padding(0);

			/// Установка соотношения панелей ZedGraph на 80% к 20%
			this.splitZgViews.SplitterDistance = this.splitZgViews.Height * 8 / 10;

			InitializeTraceListView();
			InitializeSignalsListView();
			InitializeMarkersListView();
			InitializeReportsListView();

			/// Событие отрисовки
			/// 
			evtRedraw = new AutoResetEvent(false);
			shiftPressed = false;
			needRedraw = false;
			needRedrawLock = new object();

			/// Запуск треда отрисовщика
			bgwRedraw.RunWorkerAsync();
			bgwWaterfall.RunWorkerAsync();
			#region Настройка основного вида zgMain
			/// Настройка zgMain
			/// 
			zgMain.GraphPane.Title.IsVisible = false;
			zgMain.GraphPane.TitleGap = 0;
			
//			zgMain.GraphPane.Margin.All = 0;
			
			zgMain.GraphPane.XAxis.MajorGrid.IsVisible = true;
			//zgMain.GraphPane.XAxis.MajorGrid.Color = Color.Green;
			zgMain.GraphPane.XAxis.MajorGrid.DashOn = 1f;
			zgMain.GraphPane.XAxis.MajorGrid.DashOff = 9f;
			zgMain.GraphPane.XAxis.Scale.Min = 0.0;
			zgMain.GraphPane.XAxis.Scale.Max = _rcsFMax;
			//zgMain.GraphPane.XAxis.Title.Text = "Частота";
			zgMain.GraphPane.XAxis.Title.Text =  Locale.frequency;

			zgMain.GraphPane.XAxis.Scale.IsSkipCrossLabel = true;
			zgMain.GraphPane.XAxis.Scale.IsUseTenPower = true;
			zgMain.GraphPane.XAxis.Scale.FontSpec.Size = 8;
			zgMain.GraphPane.XAxis.Title.FontSpec.Size = 8;

			zgMain.GraphPane.YAxis.MajorGrid.IsVisible = true;
			//zgMain.GraphPane.YAxis.MajorGrid.Color = Color.Green;
			zgMain.GraphPane.YAxis.MajorGrid.DashOn = 1f;
			zgMain.GraphPane.YAxis.MajorGrid.DashOff = 5f;

			//zgMain.GraphPane.X2Axis.IsVisible = true;

			zgMain.GraphPane.YAxis.Scale.Min = -130.0;
			zgMain.GraphPane.YAxis.Scale.Max = -20.0;
			zgMain.GraphPane.YAxis.Title.Text = Locale.power;
			zgMain.GraphPane.YAxis.Title.Gap = 0;
			zgMain.GraphPane.YAxis.Scale.TextLabels = new string[] { "1", "a" };
			zgMain.GraphPane.YAxis.Scale.FontSpec.Size = 8;
			zgMain.GraphPane.YAxis.Title.FontSpec.Size = 8;

			zgMain.IsEnableVZoom = false;
			//zgMain.ZoomModifierKeys = Keys.Shift;
			zgMain.ZoomModifierKeys = Keys.None;
			zgMain.ZoomButtons = MouseButtons.None;
//			zgMain.IsZoomOnMouseCenter = true;

			zgMain.PanModifierKeys = Keys.Shift;

			zgMain.IsEnableWheelZoom = true;
			zgMain.IsAutoScrollRange = false;
//			zgMain.IsShowHScrollBar = true;

			zgMain.GraphPane.Legend.IsVisible = false;
			zgMain.ZoomStepFraction = 0.5;
			zgMain.IsAutoScrollRange = false;
			zgMain.IsShowPointValues = false;

			zgMain.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;
			zgMain.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			zgMain.GraphPane.XAxis.Scale.MinScrollRange = 0.0;
			zgMain.GraphPane.XAxis.Scale.MaxScrollRange = (double)_rcsFMax;

			zgMain.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			zgMain.GraphPane.YAxis.Scale.MinScrollRange = -140.0;
			zgMain.GraphPane.YAxis.Scale.MaxScrollRange = 30.0;
			zgMain.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zgMain.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(zgMain_ContextMenuBuilder);

			zgMain.MouseMoveEvent += new ZedGraphControl.ZedMouseEventHandler(zgMain_MouseMoveEvent);

#if false
			zgMain.IsEnableSelection = true;
			zgMain.SelectButtons = MouseButtons.Left;
			zgMain.Selection.SelectionChangedEvent += new EventHandler(Selection_SelectionChangedEvent);
			zgMain.SelectAppendModifierKeys = Keys.Control;
			zgMain.IsAutoCursor = false;
			zgMain.Cursor = Cursors.Cross;
#endif
			//zgMain.IsZoomOnMouseCenter = true;
			#endregion

			/// Включение вида спектрограммы
			SetupReplayView(0, _rcsFMax);

			#region Настройка вида zgSmall

			zgSmall.GraphPane.Title.IsVisible = false;
			zgSmall.GraphPane.XAxis.IsVisible = false;
			zgSmall.GraphPane.YAxis.IsVisible = false;
			zgSmall.GraphPane.XAxis.Scale.Min = 0.0;
			zgSmall.GraphPane.XAxis.Scale.Max = _rcsFMax;

			zgSmall.GraphPane.Margin.All = 0;
			zgSmall.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zgSmall.GraphPane.YAxis.Scale.Min = -125.0;
			zgSmall.GraphPane.YAxis.Scale.Max = -20.0;

			//zgSmall.GraphPane.YAxis.MajorGrid.Color = Color.Green;
			//zgSmall.GraphPane.Chart.Fill.Color = Color.DimGray;
			//zgSmall.GraphPane.Chart.Fill.Type = ZedGraph.FillType.Solid;
			//zgSmall.GraphPane.Fill.Color = Color.DimGray;

			zgSmall.GraphPane.Legend.IsVisible = false;
			#endregion


			zgMain.Paint += new PaintEventHandler(zgMain_Paint);
			zgMain.KeyDown += new KeyEventHandler(zgMain_KeyDown);
			zgMain.GraphPane.XAxis.ScaleTitleEvent += new Axis.ScaleTitleEventHandler(XAxis_ScaleTitleEvent);
			zgMain.GraphPane.AxisChangeEvent += new GraphPane.AxisChangeEventHandler(GraphPane_AxisChangeEvent);
			zgMain.MouseDoubleClick += new MouseEventHandler(zgMain_MouseDoubleClick);
			zgMain.MouseClick += new MouseEventHandler(zgMain_MouseClick);
			zgMain.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zgMain_ZoomEvent);

			zgSmall.Paint += new PaintEventHandler(zgSmall_Paint);
			zgSmall.MouseClick += new MouseEventHandler(zgSmall_MouseClick);

			zgWaterfall.Paint += new PaintEventHandler(zgWaterfall_Paint);
			zgWaterfall.GraphPane.XAxis.ScaleTitleEvent += new Axis.ScaleTitleEventHandler(XAxis_ScaleTitleEvent);
			/// Индикатора на мелком навигаторе
			/// 
			navigatorHighlight = new BoxObj(0, 0, 1.0, 1.0);
			navigatorHighlight.Location.CoordinateFrame = CoordType.ChartFraction;
			navigatorHighlight.Fill.Type = FillType.Solid;
			navigatorHighlight.Fill.Color = Color.FromArgb(50, Color.Blue);
			navigatorHighlight.Border.IsVisible = false;
		
			/// Курсор на основном окне
			/// 
			double []_cursorX = new double[] {0, 0};
			double[] _cursorY = new double[] {-200, 200};


			cursorPoints = new PointPairList(_cursorX, _cursorY);
			cursorLine = new LineItem(Locale.cursor_name, cursorPoints, Color.FromArgb(100, Color.Aqua), SymbolType.None);
			cursorLine.Line.Width = 3.0f;

			zgMain.GraphPane.CurveList.Add(cursorLine);
			zgSmall.GraphPane.CurveList.Add(cursorLine);

			zgSmall.GraphPane.GraphObjList.Add(navigatorHighlight);

			///
			evtRedraw.Set();

			zgWaterfall.AxisChange();
			zgWaterfall.Invalidate();
			/*
			zgSmall.AxisChange();
			zgSmall.Invalidate();
			zgMain.AxisChange();
			zgMain.Invalidate();
			*/

			/// Связывание контролов
			/// 
			UpdateControl();

			/// Диалог ввода частоты
			/// 
			freqInputDialog = new FrequencyInputDialog();

			ShowSignalChart(null, false);

		}

		bool zgMain_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
		{
			double _freq, _power;

			zgMain.GraphPane.ReverseTransform(new PointF(e.X, e.Y), out _freq, out _power);
			lblFreq.Text = String.Format("Freq: {0}", Utils.FreqUtils.FreqToString((long)_freq));
			lblPower.Text = String.Format("Power: {0:F1} {1}", _power, Locale.dbm);

#if false
			mouseCoordText.Text = \n,
				Utils.FreqUtils.FreqToString((long)_freq),
				_power, Locale.dbm);

			zgMain.Invalidate();
#endif
			return false;
		}

		void zgWaterfall_Paint(object sender, PaintEventArgs e)
		{
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;

			double _min, _max;

			_min = _ctrl.GraphPane.XAxis.Scale.Min;
			if (_min < _rcsFMin)
				_min = _rcsFMin;

			_max = _ctrl.GraphPane.XAxis.Scale.Max;
			if (_max > _rcsFMax)
				_max = _rcsFMax;

			foreach (CurveItem _item in _pane.CurveList)
			{
				TraceFilteredPointList _pntList = _item.Points as TraceFilteredPointList;

				if (_pntList != null)
				{
					_pntList.SetBounds(_min, _max, GRAPHPANE_N_POINTS);
				}
			}
		}

		void SetupSpectrogramView()
		{
			#region Настройка вида zgWaterfall

			zgWaterfall.GraphPane.Title.IsVisible = false;
			//zgWaterfall.GraphPane.XAxis.IsVisible = false;
			//zgWaterfall.GraphPane.YAxis.IsVisible = false;
			zgWaterfall.GraphPane.XAxis.Scale.Min = 0.0;
			zgWaterfall.GraphPane.XAxis.Scale.Max = _rcsFMax;
			zgWaterfall.GraphPane.XAxis.Title.IsVisible = true;

			zgWaterfall.GraphPane.YAxis.Title.IsVisible = true;
			zgWaterfall.GraphPane.YAxis.IsVisible = false;

//			zgWaterfall.GraphPane.Margin.All = 0;
			zgWaterfall.GraphPane.XAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.XAxis.MajorGrid.DashOn = 1f;
			zgWaterfall.GraphPane.XAxis.MajorGrid.DashOff = 9f;
			zgWaterfall.GraphPane.XAxis.Scale.FontSpec.Size = 8;
			zgWaterfall.GraphPane.XAxis.Scale.IsVisible = true;

			zgWaterfall.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOn = 1f;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOff = 5f;

			zgWaterfall.GraphPane.YAxis.Scale.MinScrollRange = 0.0;
			zgWaterfall.GraphPane.YAxis.Scale.MaxScrollRange = BaseTrace.NUM_CYCLES;
			zgWaterfall.GraphPane.YAxis.Scale.Min = 0.0;
			zgWaterfall.GraphPane.YAxis.Scale.Max = BaseTrace.NUM_CYCLES;
			zgWaterfall.GraphPane.YAxis.Scale.FontSpec.Size = 8;

			zgWaterfall.GraphPane.Legend.IsVisible = false;

			zgWaterfall.GraphPane.BarSettings.Type = BarType.Stack;
			zgWaterfall.GraphPane.BarSettings.MinBarGap = 0.0f;
			zgWaterfall.GraphPane.BarSettings.MinClusterGap = 0.0f;

			zgWaterfall.IsEnableVZoom = false;
			zgWaterfall.IsEnableVPan = false;
			zgWaterfall.IsEnableHZoom = true;
			zgWaterfall.IsEnableHPan = true;

			//zgWaterfall.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			//zgWaterfall.GraphPane.YAxis.Scale.MinScrollRange = -140.0;
			//zgWaterfall.GraphPane.YAxis.Scale.MaxScrollRange = 50.0;
			//zgWaterfall.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;
			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			zgWaterfall.GraphPane.XAxis.Scale.MinScrollRange = 0.0;
			zgWaterfall.GraphPane.XAxis.Scale.MaxScrollRange = _rcsFMax;

			zgWaterfall.AxisChange();
			zgWaterfall.Invalidate();
			#endregion

//			toolBtnWaterfallWaterfall.Checked = true;
			toolBtnWaterfallZeroSpan.Checked = false;
			toolBtnWaterfallZeroSpan.Enabled = false;

			toolBtnWaterfallPan.Enabled = true;
			toolBtnWaterfallZoom.Enabled = true;
			toolBtnWaterfallUnzoomAll.Enabled = false;
			toolBtnWaterfallUnZoom.Enabled = false;

			waterfallMode = EWaterfallMode.Waterfall;
			xPanelConfig.Enabled = true;

			foreach (BaseTrace _t in rcs.ScanTraces)
			{
				RedrawWaterfallFull(_t);
			}

			lock (needRedrawWaterfallLock)
				needRedrawWaterfall = true;
		}

		void SetupZeroSpanView()
		{
			#region Настройка вида zgWaterfall

			zgWaterfall.GraphPane.CurveList.Clear();
			zgWaterfall.GraphPane.GraphObjList.Clear();

			zgWaterfall.GraphPane.YAxis.Title.IsVisible = false;
			zgWaterfall.GraphPane.YAxis.IsVisible = true;

			zgWaterfall.GraphPane.XAxis.Scale.MinScrollRange = 0;
			zgWaterfall.GraphPane.XAxis.Scale.MaxScrollRange = zeroSpanPointCount * 2;
			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMinScrollRange = false;
			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = false;
			zgWaterfall.GraphPane.XAxis.Scale.Min = 0;
			zgWaterfall.GraphPane.XAxis.Scale.Max = zeroSpanPointCount * 2;
			zgWaterfall.IsEnableHPan = false;
			zgWaterfall.IsEnableHZoom = false;
			zgWaterfall.IsEnableVZoom = false;
			zgWaterfall.IsEnableVPan = true;

			zgWaterfall.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.XAxis.Scale.IsVisible = false;
			zgWaterfall.GraphPane.XAxis.Title.IsVisible = false;
			zgWaterfall.GraphPane.Title.IsVisible = false;

			zgWaterfall.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOn = 1f;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOff = 5f;

			zgWaterfall.GraphPane.YAxis.Scale.Min = -125.0;
			zgWaterfall.GraphPane.YAxis.Scale.Max = -20.0;
			zgWaterfall.GraphPane.YAxis.Scale.FontSpec.Size = 8;

			zgWaterfall.GraphPane.Legend.IsVisible = false;

			zgWaterfall.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			zgWaterfall.GraphPane.YAxis.Scale.MinScrollRange = -140.0;
			zgWaterfall.GraphPane.YAxis.Scale.MaxScrollRange = 30.0;
			zgWaterfall.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zgWaterfall.AxisChange();
			zgWaterfall.Invalidate();

			//zgWaterfall.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;
			//zgWaterfall.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			//zgWaterfall.GraphPane.XAxis.Scale.MinScrollRange = 0.0;
			//zgWaterfall.GraphPane.XAxis.Scale.MaxScrollRange = 3000000000.0;
			#endregion

			toolBtnWaterfallWaterfall.Checked = false;
			toolBtnWaterfallWaterfall.Enabled = false;

			toolBtnWaterfallPan.Enabled = true;
			toolBtnWaterfallZoom.Enabled = false;
			toolBtnWaterfallUnzoomAll.Enabled = false;
			toolBtnWaterfallUnZoom.Enabled = false;

			waterfallMode = EWaterfallMode.ZeroSpan;

			xPanelConfig.Enabled = false;
//			toolBtnWaterfallZeroSpan.Checked = true;
		}

		void SetupReplayView(double pMin, double pMax)
		{
			#region Настройка вида zgWaterfall

			zgWaterfall.GraphPane.Title.IsVisible = false;
			//zgWaterfall.GraphPane.XAxis.IsVisible = false;
			//zgWaterfall.GraphPane.YAxis.IsVisible = false;
			//zgWaterfall.GraphPane.XAxis.Scale.Min = 0.0;
			//zgWaterfall.GraphPane.XAxis.Scale.Max = 3000000000;
			zgWaterfall.GraphPane.XAxis.Title.IsVisible = true;
			zgWaterfall.GraphPane.XAxis.Title.Text = "Частота";
			zgWaterfall.GraphPane.YAxis.Title.IsVisible = false;
			zgWaterfall.GraphPane.YAxis.IsVisible = true;

			//			zgWaterfall.GraphPane.Margin.All = 0;
			zgWaterfall.GraphPane.XAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.XAxis.MajorGrid.DashOn = 1f;
			zgWaterfall.GraphPane.XAxis.MajorGrid.DashOff = 9f;
			zgWaterfall.GraphPane.XAxis.Scale.FontSpec.Size = 8;
			zgWaterfall.GraphPane.XAxis.Title.FontSpec.Size = 8;
			zgWaterfall.GraphPane.XAxis.Scale.IsVisible = true;

			zgWaterfall.GraphPane.YAxis.MajorGrid.IsVisible = true;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOn = 1f;
			zgWaterfall.GraphPane.YAxis.MajorGrid.DashOff = 5f;

			zgWaterfall.GraphPane.YAxis.Scale.FontSpec.Size = 8;
			zgWaterfall.GraphPane.YAxis.Title.FontSpec.Size = 8;

			zgWaterfall.GraphPane.Legend.IsVisible = false;

			//				zgWaterfall.GraphPane.BarSettings.Type = BarType.Stack;
			//				zgWaterfall.GraphPane.BarSettings.MinBarGap = 0.0f;
			//				zgWaterfall.GraphPane.BarSettings.MinClusterGap = 0.0f;

			zgWaterfall.IsEnableVZoom = false;
			zgWaterfall.IsEnableVPan = true;
			zgWaterfall.IsEnableHZoom = true;
			zgWaterfall.IsEnableHPan = true;

			
			
			zgWaterfall.GraphPane.YAxis.Scale.MinScrollRange = -140.0;
			zgWaterfall.GraphPane.YAxis.Scale.MaxScrollRange = 30.0;
			zgWaterfall.GraphPane.YAxis.Scale.IsLimitMinScrollRange = true;
			zgWaterfall.GraphPane.YAxis.Scale.IsLimitMaxScrollRange = true;

			zgWaterfall.GraphPane.YAxis.Scale.Min = -120.0;
			zgWaterfall.GraphPane.YAxis.Scale.Max = -20.0;
			

			//				zgWaterfall.AxisChange();
			//				zgWaterfall.Invalidate();

			
			zgWaterfall.GraphPane.XAxis.Scale.MinScrollRange = pMin;
			zgWaterfall.GraphPane.XAxis.Scale.MaxScrollRange = pMax;
			zgWaterfall.GraphPane.XAxis.Scale.Min = pMin;
			zgWaterfall.GraphPane.XAxis.Scale.Max = pMax;
			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMinScrollRange = true;
			zgWaterfall.GraphPane.XAxis.Scale.IsLimitMaxScrollRange = true;
			
			zgWaterfall.AxisChange();
			zgWaterfall.Invalidate();

			#endregion

			toolBtnWaterfallWaterfall.Checked = false;
			toolBtnWaterfallWaterfall.Enabled = true;
			toolBtnWaterfallZeroSpan.Checked = false;
			toolBtnWaterfallZeroSpan.Enabled = true;


			toolBtnWaterfallPan.Enabled = true;
			toolBtnWaterfallZoom.Enabled = true;
			toolBtnWaterfallUnzoomAll.Enabled = false;
			toolBtnWaterfallUnZoom.Enabled = false;

			xPanelConfig.Enabled = true;
			waterfallMode = EWaterfallMode.Replay;
//			toolBtnWaterfallReplay.Checked = true;
		}

		/// <summary>
		/// Обработка выбора объектов в ZedGraph
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
		TextObj textSelected = new TextObj("", 0, 0, CoordType.ChartFraction, AlignH.Left, AlignV.Top);

		void Selection_SelectionChangedEvent(object sender, EventArgs e)
		{

			Selection _sel = sender as Selection;
			lstMarkers.DeselectAll();

			if (_sel.Count != 0)
			{
				/// Есть выбранные объекты
				/// 
				var _q = from _ci in _sel
						 where _ci.Tag is BaseRadioMarker
						 select _ci.Tag as BaseRadioMarker;

				lstMarkers.SelectObjects(_q.ToList());

				if (_q.Count() > 0)
				{
					textSelected.FontSpec.Size = 8;

					string _txt = Locale.selected_markers + "\n";

					foreach (BaseRadioMarker _m in _q)
					{
						_txt += String.Format("{0} ({1})\n", _m.Name, FreqUtils.FreqToString(_m.Frequency));
					}

					textSelected.Text = _txt;

					zgMain.GraphPane.GraphObjList.Remove(textSelected);
					zgMain.GraphPane.GraphObjList.Add(textSelected);
				}
				else
					zgMain.GraphPane.GraphObjList.Remove(textSelected);
			}
			else
				zgMain.GraphPane.GraphObjList.Remove(textSelected);

			lstMarkers.Invalidate();
		}


		void zgMain_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
		{
			menuStrip.Items.Clear();

			double _rawFreq, _y;
			zgMain.GraphPane.ReverseTransform(mousePt, out _rawFreq, out _y);
			long _freq = ((long)_rawFreq / 1000) * 1000;

			ToolStripMenuItem _addMarkerMenuItem = new ToolStripMenuItem(String.Format(Locale.add_marker, 
				KaorCore.Utils.FreqUtils.FreqToString(_freq)));

			_addMarkerMenuItem.Click += new EventHandler(_addMarkerMenuItem_Click);
			_addMarkerMenuItem.Tag = _freq;
			menuStrip.Items.Add(_addMarkerMenuItem);

			ToolStripMenuItem _addMaxMarkerMenuItem;
			ToolStripMenuItem _addSignalMenu;

			if (sender.Selection.Count == 1)
			{
				CurveItem _item = sender.Selection[0];
				BaseTrace _trace = _item.Tag as BaseTrace;

				if (_trace == null)
				{
					_addMaxMarkerMenuItem = new ToolStripMenuItem(Locale.add_marker_max);
					_addMaxMarkerMenuItem.Enabled = false;
				}
				else
				{
					_addMaxMarkerMenuItem = new ToolStripMenuItem(String.Format(Locale.add_marker_max_format, _trace.Name));
					_addMaxMarkerMenuItem.Tag = _trace;
					_addMaxMarkerMenuItem.Click += new EventHandler(_addMaxMarkerMenuItem_Click);

					//_addSignalMenu = new ToolStripMenuItem(Locale.add_signal_menu);
					
					/// Добавлеие простого сигнала
					/// 
					SignalAddParams _sigAddParams = new SignalAddParams();
					_sigAddParams.frequency = _freq;
					_sigAddParams.power = double.NaN;
					_sigAddParams.trace = _trace;

					for(int _i = 0; _i < _trace.TracePoints.Count() - 2; _i++)
					{
						if (_trace.TracePoints[_i].Freq - _trace.MeasureStep / 2 <= (long)_rawFreq &&
							_trace.TracePoints[_i].Freq + _trace.MeasureStep / 2 > (long)_rawFreq)
						{
							//_sigAddParams.frequency = _trace.TracePoints[_i].Freq;
							_sigAddParams.power = _trace.TracePoints[_i].Power;
							break;
						}
					}

					if (!double.IsNaN(_sigAddParams.power))
					{
						_addSignalMenu = new ToolStripMenuItem(String.Format(Locale.add_signal_menu, 
							FreqUtils.FreqToString(_sigAddParams.frequency),
							_sigAddParams.power));

						_addSignalMenu.Tag = _sigAddParams;
						_addSignalMenu.Click += new EventHandler(_addSignalMenu_Click);
						menuStrip.Items.Add(_addSignalMenu);

						ToolStripMenuItem _addMarkerAtMenu = new ToolStripMenuItem(String.Format(Locale.add_marker_at,
							FreqUtils.FreqToString(_sigAddParams.frequency),
							_sigAddParams.power));

						_addMarkerAtMenu.Tag = _sigAddParams;
						_addMarkerAtMenu.Click += new EventHandler(_addMarkerAtMenu_Click);
						menuStrip.Items.Add(_addMarkerAtMenu);
					}
				}
			}
			else
			{
				_addMaxMarkerMenuItem = new ToolStripMenuItem(Locale.add_marker_max);
				_addMaxMarkerMenuItem.Enabled = false;
			}

			menuStrip.Items.Add(_addMaxMarkerMenuItem);
			
		}

		void _addMarkerAtMenu_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _menuItem = sender as ToolStripMenuItem;

			if (_menuItem == null)
				return;

			SignalAddParams _sigAddParams = (SignalAddParams)_menuItem.Tag;


			// Add a text item to decorate the graph
			TextObj text = new TextObj(FreqUtils.FreqToString(_sigAddParams.frequency) + "\n" 
				+ _sigAddParams.power + " " + Locale.dbm, 
				0.0, 0.0, CoordType.AxisXYScale);


			// Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
			//text.Location.AlignH = AlignH.Center;
			//text.Location.AlignV = AlignV.Bottom;
			//text.FontSpec.Fill = new Fill(Color.White, Color.PowderBlue, 45F);
			//text.FontSpec.StringAlignment = StringAlignment.Near;
			zgMain.MasterPane.GraphObjList.Add(text);

			// Add an arrow pointer for the above text item
			//ArrowObj arrow = new ArrowObj(Color.Black, 12F, 175F, 77F, 100F, 45F);
			//arrow.Location.CoordinateFrame = CoordType.AxisXYScale;
			//myPane.GraphObjList.Add(arrow);
		}

		/// <summary>
		/// Обработка добавления сигнала по трассе
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _addSignalMenu_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _menuItem = sender as ToolStripMenuItem;

			if (_menuItem == null)
				return;

			SignalAddParams _sigAddParams = (SignalAddParams)_menuItem.Tag;

			/// Добавление сигнала на трассу
			/// 
			AddSingleFreqSignal(_sigAddParams.frequency, _sigAddParams.power);
		}


		void _addMarkerMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _item = sender as ToolStripMenuItem;

			if (_item == null)
				return;

			long _freq = (long)_item.Tag;

			BaseRadioMarker _marker = BaseRadioMarker.FromDialog(_freq);

			if (_marker != null)
			{
				rcs.AddMarker(_marker);
				UpdateControl();
			}
		}

		void _addMaxMarkerMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem _item = sender as ToolStripMenuItem;

			if (_item == null)
				return;

			BaseTrace _trace = _item.Tag as BaseTrace;

			if (_trace == null)
				return;

			double _maxPower;
			long _maxFreq;

			if (_trace.FindMaxPower((long)zgMain.GraphPane.XAxis.Scale.Min,
				(long)zgMain.GraphPane.XAxis.Scale.Max,
				out _maxFreq, out _maxPower))
			{
				BaseRadioMarker _m = BaseRadioMarker.FromDialog(_maxFreq);
				if (_m != null)
				{
					rcs.AddMarker(_m);
					UpdateControl();
				}
			}
			else
				MessageBox.Show(String.Format(Locale.trace_no_max, _trace.Name),
					Locale.error, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		void zgMain_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{
			if (zgMain.GraphPane.ZoomStack.Count == 0)
			{
				/// Отключение кнопок анзума, если анзумить нечего
				toolBtnUnZoom.Enabled = false;
				toolBtnUnZoomAll.Enabled = false;
			}
			else
			{
				/// Включение кноок анзума
				toolBtnUnZoom.Enabled = true;
				toolBtnUnZoomAll.Enabled = true;
			}
		}

		/// <summary>
		/// Инициализация списка трасс
		/// </summary>
		void InitializeTraceListView()
		{
			lstScanTraces.ShowGroups = false;
			lstScanTraces.OwnerDraw = true;
			lstScanTraces.MouseDoubleClick += new MouseEventHandler(lstScanTraces_MouseDoubleClick);
			/// Настройка колонки видимости трассы
			clmVisible.AspectGetter = delegate(object pO) { return ((BaseTrace)pO).IsVisible; };
			clmVisible.Renderer = new MappedImageRenderer(true, "eye1.png");

			clmControl.AspectGetter = delegate(object pO) { return ((BaseTrace)pO).ScanMode; };
			clmControl.Renderer = new MappedImageRenderer(ETraceScanMode.Scan, "scan.png",
				ETraceScanMode.Control, "control.png");

			/// Настройка колонка имени трассы
			clmName.AspectGetter = delegate(object pO) 
			{ 
				BaseTrace _bt = (BaseTrace)pO;
				
				//return _bt.Name + ((_bt.IsChanged) ? "*" : "") + " (" +
				return _bt.Name + " (" +
					KaorCore.Utils.FreqUtils.FreqToString(_bt.Fstart) + "-" +
					KaorCore.Utils.FreqUtils.FreqToString(_bt.Fstop) + "/" +
					KaorCore.Utils.FreqUtils.FreqToString(_bt.MeasureStep) + ")";
			};

			/*
			clmName.AspectPutter = delegate(object pO, object pV) 
			{
				if (pV is string)
					((BaseTrace)pO).Name = (string)pV;
			};
			clmName.IsEditable = true;
			 */

			lstScanTraces.RowFormatter = delegate(OLVListItem pItem)
			{
				BaseTrace _trace = (BaseTrace)pItem.RowObject;
				
				pItem.BackColor = Color.FromArgb(255, _trace.LineItem.Color);
			};
		}

		/// <summary>
		/// Фильтрация сигналов по видимости
		/// </summary>
		/// <param name="pUnknown"></param>
		/// <param name="pGreen"></param>
		/// <param name="pYellow"></param>
		/// <param name="pRed"></param>
		/// <returns></returns>
		List<BaseSignal> FilteredSignals()
		{
			var q = from _s in rcs.Signals
					where
					(unknownToolStripMenuItem.Checked == true && _s.SignalType == ESignalType.Unknown) ||
					(greenToolStripMenuItem.Checked == true && _s.SignalType == ESignalType.Green) ||
					(yellowToolStripMenuItem.Checked == true && _s.SignalType == ESignalType.Yellow) ||
					(redToolStripMenuItem.Checked == true && _s.SignalType == ESignalType.Red)
					select _s;

			List<BaseSignal> _signals = q.ToList();

			return _signals;
		}
		
		/// <summary>
		/// Инициализация списка сигналов
		/// </summary>
		void InitializeSignalsListView()
		{

			lstSignals.OwnerDraw = true;
			lstSignals.MouseDoubleClick += new MouseEventHandler(lstSignals_MouseDoubleClick);
			lstSignals.ObjectsGetter = FilteredSignals;
			
			/// Настройка колонки видимости сигнала
			/// 
			olvSignalVisible.AspectGetter = delegate(object pO) { return ((BaseSignal)pO).IsVisible; };
			olvSignalVisible.Renderer = new MappedImageRenderer(true, "eye1.png");

			olvSignalFStart.AspectGetter = delegate(object pO) { return FreqUtils.FreqToString(((BaseSignal)pO).Frequency); };
			olvSignalFStop.AspectGetter = delegate(object pO) { return FreqUtils.FreqToString(((BaseSignal)pO).Band); };
			//olvSignalHits.AspectGetter = delegate(object pO) { return ((BaseSignal)pO).HitsCount; };
			olvSignalTitle.AspectGetter = delegate(object pO) { return ((BaseSignal)pO).Name; };
			
			olvHits.AspectGetter = delegate(object pO) { return ((BaseSignal)pO).HitsCount; };

			lstSignals.CustomSorter = delegate(OLVColumn pCol, SortOrder pOrder)
			{
				rcs.Signals.Sort(new SignalsComparer(pCol, pOrder));
				this.lstSignals.BuildList(true, false);
			};

			lstSignals.RowFormatter = delegate(OLVListItem pItem)
			{
				/// Формирование цвета фона в зависимости от типа сигнала
				BaseSignal _signal = (BaseSignal)pItem.RowObject;
				Color _backColor;
	
				switch (_signal.SignalType)
				{
					case ESignalType.Red:
						_backColor = Color.Red;
						break;
					
					case ESignalType.Yellow:
						_backColor = Color.Yellow;
						break;
					
					case ESignalType.Green:
						_backColor = Color.Green;
						break;
					
					default:
						_backColor = Color.White;
						break;
				}

				pItem.BackColor = _backColor;
				foreach (ListViewItem.ListViewSubItem _subItem in pItem.SubItems)
				{
					_subItem.BackColor = _backColor;
				}
			};
 
		}

		internal class SignalsComparer : Comparer<BaseSignal>
		{
			OLVColumn col;
			SortOrder order;

			public SignalsComparer(OLVColumn pCol, SortOrder pOrder)
			{
				col = pCol;
				order = pOrder;
			}


			#region IComparer<BaseSignal> Members

			public override int Compare(BaseSignal x, BaseSignal y)
			{
				int _res = 0;

				if (col.Index == 0)
				{
					if (col.Index == 0)
					{
						if (x.Frequency < y.Frequency)
							return -1;
						else if (x.Frequency > y.Frequency)
							return 1;
						else
							return 0;
					}
				}
				else if (col.Index == 1)
				{
					if (x.Frequency < y.Frequency)
						_res = -1;
					else if (x.Frequency > y.Frequency)
						_res = 1;
					else
						_res = 0;
				}
				else if (col.Index == 2)
				{
					if (x.Band < y.Band)
						_res = -1;
					else if (x.Band > y.Band)
						_res = 1;
					else
						_res = 0;
				}
				else
				{
					IComparable _vx = col.GetValue(x) as IComparable;
					IComparable _vy = col.GetValue(y) as IComparable;
					_res = _vx.CompareTo(_vy);
				}

				if (order == SortOrder.Ascending)
					return _res;
				else
					return -_res;
			}

			#endregion
		}
		/// <summary>
		/// Инициализация списка трасс
		/// </summary>
		void InitializeMarkersListView()
		{
			lstMarkers.ShowGroups = false;
			lstMarkers.OwnerDraw = true;
			lstMarkers.MouseDoubleClick += new MouseEventHandler(lstMarkers_MouseDoubleClick);
			
			/// Настройка колонки видимости маркера
			olvMarkerVisible.AspectGetter = delegate(object pO) { return ((BaseRadioMarker)pO).IsVisible; };
			olvMarkerVisible.Renderer = new MappedImageRenderer(true, "eye1.png");

			olvMarkerFreq.AspectGetter = delegate(object pO) { return FreqUtils.FreqToString(((BaseRadioMarker)pO).Frequency); };
			olvMarkerName.AspectGetter = delegate(object pO) { return ((BaseRadioMarker)pO).Name; };
			olvMarkerName.AspectPutter = delegate(object pO, object pV)
			{
				if(pV is string)
					((BaseRadioMarker)pO).Name = (string)pV;
			};
			olvMarkerName.IsEditable = true;

			lstMarkers.CustomSorter = delegate(OLVColumn pCol, SortOrder pOrder)
			{
				rcs.Markers.Sort(new MarkersComparer(pCol, pOrder));
				this.lstMarkers.BuildList(true, false);
			};

			lstMarkers.RowFormatter = delegate(OLVListItem pItem)
			{
				/// Формирование цвета фона в зависимости от типа сигнала
				BaseRadioMarker _marker = (BaseRadioMarker)pItem.RowObject;
				Color _backColor = Color.FromArgb(255, _marker.LineItem.Color.R, 
					_marker.LineItem.Color.G, _marker.LineItem.Color.B);

				pItem.BackColor = _backColor;

				foreach (ListViewItem.ListViewSubItem _subItem in pItem.SubItems)
				{
					_subItem.BackColor = _backColor;
				}
			};
		}

		internal class MarkersComparer : Comparer<BaseRadioMarker>
		{
			OLVColumn col;
			SortOrder order;

			public MarkersComparer(OLVColumn pCol, SortOrder pOrder)
			{
				col = pCol;
				order = pOrder;
			}


			#region IComparer<BaseSignal> Members

			public override int Compare(BaseRadioMarker x, BaseRadioMarker y)
			{
				int _res = 0;

				if (col.Index == 0)
				{
					if (x.Frequency < y.Frequency)
						return -1;
					else if (x.Frequency > y.Frequency)
						return 1;
					else
						return 0;
				}
				else if(col.Index == 1)
				{
					if (x.Frequency < y.Frequency)
						_res = -1;
					else if (x.Frequency > y.Frequency)
						_res = 1;
					else
						_res = 0;
				}
				else
				{
					IComparable _vx = col.GetValue(x) as IComparable;
					IComparable _vy = col.GetValue(y) as IComparable;
					_res = _vx.CompareTo(_vy);
				}

				if (order == SortOrder.Ascending)
					return _res;
				else
					return -_res;
			}

			#endregion
		}

		void InitializeReportsListView()
		{

			olvReportTime.AspectGetter = delegate(object pO) { return ((ReportItem)pO).Timestamp.ToLongTimeString(); };
			olvReportTime.GroupKeyGetter = delegate(object pO) { return ((ReportItem)pO).Timestamp.Date; };
			olvReportTime.GroupKeyToTitleConverter = delegate(object pKey)
			{
				DateTime _date = (DateTime)pKey;
				if (_date == DateTime.Now.Date)
					return Locale.today;
				else
					return _date.ToShortDateString();
			};


			olvReportType.AspectGetter = delegate(object pO) { return ((ReportItem)pO).TypeReport; };
			olvReportType.Renderer = new MappedImageRenderer(new object[] 
			{ReportType.Audio, "repaudio1.png", 
				ReportType.SignalAudio, "repaudio2.png", 
				ReportType.PDF, "reppdf.png"});
			
			olvReportName.AspectGetter = delegate(object pO) { return ((ReportItem)pO).Name; };
			olvReportName.AspectPutter = delegate(object pO, object pV)
			{
				if (pV is string)
					((ReportItem)pO).Name = (string)pV;
			};

			olvReportType.GroupKeyGetter = delegate(object pO) { return ((ReportItem)pO).Timestamp.Date; };
			olvReportType.GroupKeyToTitleConverter = delegate(object pKey)
			{
				DateTime _date = (DateTime)pKey;
				if (_date == DateTime.Now.Date)
					return Locale.today;
				else
					return _date.ToShortDateString();
			};

			olvReportName.IsEditable = true;

			lstReports.SortGroupItemsByPrimaryColumn = false;
			lstReports.Sorting = SortOrder.Descending;

			ReportsManager.OnReportsChanged += new ReportsManager.ReportsChangedDelegate(ReportsManager_OnReportsChanged);

			lstReports.ObjectsGetter = FilteredReports;
			lstReports.SetObjects(ReportsManager.Reports);

		}

		/// <summary>
		/// Фильтрация отчетов по типам в зависимости от выбранных пунктов меню
		/// </summary>
		/// <returns></returns>
		List<ReportItem> FilteredReports()
		{
			var q = from _r in ReportsManager.Reports
					where
					(reportsAudioFilter.Checked == true && _r.TypeReport == ReportType.Audio) ||
					(reportsSignalAudioFilter.Checked == true && _r.TypeReport == ReportType.SignalAudio) ||
					(reportsPDFFilter.Checked == true && _r.TypeReport == ReportType.PDF)
					select _r;

			List<ReportItem> _reports = q.ToList();

			return _reports;
		}

		void ReportsManager_OnReportsChanged()
		{
			if (!InvokeRequired)
			{
				lstReports.BuildList(true);
			}
			else
				Invoke(new MethodInvoker(ReportsManager_OnReportsChanged));
		}
		/// <summary>
		/// Обработка двойного щелчка мыши по списку маркеров
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lstMarkers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstMarkers.HitTest(e.Location);

			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;

			int _subItemIndex = _item.SubItems.IndexOf(_info.SubItem);

			if (_subItemIndex == 0)
			{
				/// Клик по колонке с глазом
				ProcessMarkerVisibleClick(_item);
			}
			else
			{
				/// Клик по другой колонке, переключаеmsя к карточке сигнала
				/// 

				BaseRadioMarker _marker = (BaseRadioMarker)_item.RowObject;
				if (_marker == null)
					return;

				CursorFrequency = (Int64)_marker.Frequency;
				rcs.RPUManager.ManualRPU.BaseFreq = _marker.Frequency;

				
			}
			lstMarkers.RefreshItem(_item);
		}

		/// <summary>
		/// Обработчик двойного нажатия по списку сигналов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lstSignals_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstSignals.HitTest(e.Location);

			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;

			int _subItemIndex = _item.SubItems.IndexOf(_info.SubItem);

			if (_subItemIndex == 0)
			{
				/// Клик по колонке с глазом
				ProcessSignalVisibleClick(_item);
			}
			else
			{
				/// Клик по другой колонке, переключаеmsя к карточке сигнала
				/// 
				BaseSignal _signal = (BaseSignal)_item.RowObject;
				if (_signal != null)
				{
					ShowSignalChart(_signal, true);
				}
			}
			lstSignals.RefreshItem(_item);
		}

		/// <summary>
		/// Обработчик двойного клика по списку трасс
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lstScanTraces_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstScanTraces.HitTest(e.Location);
			
			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;

			int _subItemIndex = _item.SubItems.IndexOf(_info.SubItem);

			if (_subItemIndex == 0)
			{
				/// Клик по колонке с глазом
				/// 
				

				ProcessTraceVisibleClick(_item);
			}
			else if (rcs.OperationMode == ERadioControlMode.Idle && _subItemIndex == 1)
			{
				/// Клик по колонке контроля
				ProcessTraceControlClick(_item);
			}

			lstScanTraces.RefreshItem(_item);
		}

		/// <summary>
		/// Обработка двойного клика по колонке с глазом
		/// </summary>
		/// <param name="_item"></param>
		void ProcessMarkerVisibleClick(OLVListItem _item)
		{
			/// Изменение состояния
			/// 
			BaseRadioMarker _marker = (BaseRadioMarker)_item.RowObject;

			if (_marker == null)
				return;

			_marker.IsVisible = !_marker.IsVisible;
			_marker.DrawZedGraph(zgMain);
			//_marker.DrawZedGraph(zgSmall);

			zgMain.Invalidate();
			zgSmall.Invalidate();
			//needRedraw = true;

			UpdateControl();

		}

		/// <summary>
		/// Обработка двойного клика по колонке с глазом
		/// </summary>
		/// <param name="_item"></param>
		void ProcessTraceVisibleClick(OLVListItem _item)
		{
			/// Изменение состояния
			/// 
			BaseTrace _trace = (BaseTrace)_item.RowObject;

			if (_trace == null)
				return;

			zgMain.Selection.ClearSelection(zgMain.MasterPane);

			if (shiftPressed)
			{
				/// Если нажата кнопка Shift, то убираем  все трассы, кроме выбранной
				foreach (object _o in lstScanTraces.Objects)
				{
					BaseTrace _otherTrace = (BaseTrace)_o;
					if (_otherTrace == null || _otherTrace == _trace)
						continue;

					_otherTrace.IsVisible = false;
					_otherTrace.DrawZedGraph(zgMain, true);
					_otherTrace.DrawZedGraph(zgSmall, false);

				}

				_trace.IsVisible = true;
				zgMain.Selection.AddToSelection(zgMain.MasterPane, _trace.LineItem);

				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

			}
			else
			{
				_trace.IsVisible = !_trace.IsVisible;

				if(_trace.IsVisible)
					zgMain.Selection.AddToSelection(zgMain.MasterPane, _trace.LineItem);
					
				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);
			}
			zgMain.Invalidate();
			zgSmall.Invalidate();
			//needRedraw = true;

			UpdateControl();

		}

		/// <summary>
		/// Обработка двойного клика по колонке контроля трассы
		/// </summary>
		/// <param name="_item"></param>
		void ProcessTraceControlClick(OLVListItem _item)
		{
			BaseTrace _trace = (BaseTrace)_item.RowObject;

			if (_trace == null)
				return;

			switch (_trace.ScanMode)
			{
				case ETraceScanMode.None:
					if (_trace.ScanParams == null)
					{
						if (MessageBox.Show(String.Format(Locale.no_scan_params, _trace.Name),
							Locale.question,
							MessageBoxButtons.YesNo, MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button1) == DialogResult.Yes)
						{
							toolBtnScanParams_Click(lstScanTraces, null);
						}
					}

					_trace.ScanMode = ETraceScanMode.Scan;
					break;

				case ETraceScanMode.Scan:
					if (_trace.TraceControl != null)
					{
						_trace.ScanMode = ETraceScanMode.Control;
					}
					else
					{
						_trace.ScanMode = ETraceScanMode.None;
					}
					break;

				case ETraceScanMode.Control:
					_trace.ScanMode = ETraceScanMode.None;
					break;
			}

			/// Перерисовка трассы на основном окне
			_trace.DrawZedGraph(zgMain, true);
		}

		void ProcessSignalVisibleClick(OLVListItem _item)
		{
			/// Изменение состояния
			/// 
			BaseSignal _signal = (BaseSignal)_item.RowObject;

			if (_signal == null)
				return;

			_signal.IsVisible = !_signal.IsVisible;

			UpdateControl();
		}

		/// <summary>
		/// Обработчик нажатия кнопок
		/// Сюда надо будет вставить обработку F1...F12
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgMain_KeyDown(object sender, KeyEventArgs e)
		{
			Keys _c = e.KeyCode;
		}


		#endregion

		#region ================ Публичные методы ================
#if false
		public void RemoveTrace(BaseTrace pTrace)
		{
			//int _idx = pTrace.CheckedListItem.CurveListIndex;
			//if (_idx != -1)
			//{
				zgMain.GraphPane.CurveList.Remove(pTrace.CheckedListItem.Line);
				zgSmall.GraphPane.CurveList.Remove(pTrace.CheckedListItem.Line);
			//}
		}
#endif
		public void SetSpan(Int64 pSpan)
		{
			if (pSpan < 10000)
				pSpan = 10000;

			if (pSpan >= 0)
			{
				/// Спан основого окна
				/// 
				double _oldSpan = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;
				double _centerFreq = zgMain.GraphPane.XAxis.Scale.Min + _oldSpan / 2.0;

				/// Перемещение основного окна
				/// 

				zgMain.GraphPane.XAxis.Scale.Min = _centerFreq - pSpan / 2.0;
				zgMain.GraphPane.XAxis.Scale.Max = _centerFreq + pSpan / 2.0;

				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}

		/// <summary>
		/// Установка центральной частоты
		/// </summary>
		/// <param name="pFreq"></param>
		public void SetCenterFreq(Int64 pFreq)
		{
			if (pFreq >= 0 && pFreq <= _rcsFMax)
			{
				/// Перемещение основного окна
				/// 
				double _span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;

				zgMain.GraphPane.XAxis.Scale.Min = pFreq - _span / 2.0;
				zgMain.GraphPane.XAxis.Scale.Max = pFreq + _span / 2.0;

				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}

		/// <summary>
		/// Установка стартовой частоты
		/// </summary>
		/// <param name="pFreq"></param>
		public void SetStartFreq(Int64 pFreq)
		{
			if (pFreq >= 0 && pFreq <= _rcsFMax)
			{
				/// Перемещение основного окна
				/// 
				double _span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;
				zgMain.GraphPane.XAxis.Scale.Min = pFreq;

				if (pFreq > zgMain.GraphPane.XAxis.Scale.Max - 100)
					zgMain.GraphPane.XAxis.Scale.Max = pFreq + _span;

				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}

		/// <summary>
		/// Установка конечной частоты
		/// </summary>
		/// <param name="pFreq"></param>
		public void SetStopFreq(Int64 pFreq)
		{
			if (pFreq >= 0 && pFreq <= _rcsFMax)
			{
				/// Перемещение основного окна
				/// 
				double _span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;
				zgMain.GraphPane.XAxis.Scale.Max = pFreq;

				if (pFreq < zgMain.GraphPane.XAxis.Scale.Min - 100)
					zgMain.GraphPane.XAxis.Scale.Min = pFreq - _span;

				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}


		#endregion

		#region ================ Приватные методы ================

		/// <summary>
		/// Обновление контрола в соответствии с обновление данных СРК
		/// </summary>
		public void UpdateControl()
		{
			if (!InvokeRequired)
			{
				UpdateScanTraces();
				UpdateTraceControls();
				UpdateMarkers();
				UpdateZedGraph();

				UpdateSignals();
			}
			else
				Invoke(new MethodInvoker(UpdateControl));
//			UpdateTriggers();
		}

		void UpdateScanTraces()
		{
			lstScanTraces.BuildList(true);
			lstScanTraces.Update();

			if(lstScanTraces.SelectedItem == null)
			{
				toolBtnDeleteTrace.Enabled = false;
				toolBtnSaveTrace.Enabled = false;
				toolBtnArmScanTrace.Enabled = false;
				toolBtnEditTrace.Enabled = false;
				toolBtnTraceGo.Enabled = false;
				toolBtnTraceMarkers.Enabled = false;
				toolBtnScanParams.Enabled = false;
				toolBtnReplayTrace.Enabled = false;
			}
			else
			{
				if (rcs.OperationMode == ERadioControlMode.Idle)
				{
					toolBtnDeleteTrace.Enabled = true;
					toolBtnSaveTrace.Enabled = true;
					toolBtnArmScanTrace.Enabled = true;
					toolBtnEditTrace.Enabled = true;
					toolBtnTraceMarkers.Enabled = true;
					toolBtnScanParams.Enabled = true;
					toolBtnReplayTrace.Enabled = true;
				}

				toolBtnTraceGo.Enabled = true;
			}
		}

		/// <summary>
		/// Обработка события изменения трассы
		/// </summary>
		/// <param name="pTrace"></param>
		/// <param name="pPoint"></param>
		/// 
		void _trace_OnTraceChanged(BaseTrace pTrace, TracePoint pPoint, double pOldPower)
		{
			lock (needRedrawLock)
			{
				needRedraw = true;
			}
		}

		/// <summary>
		/// Обновление списка трасс на контроле
		/// </summary>
		void UpdateTraceControls()
		{
		}

		delegate void UpdateSignalDelegate(BaseSignal pSignal);

		public void UpdateSignal(BaseSignal pSignal)
		{
			if (!InvokeRequired)
			{
				pSignal.DrawZedGraph(zgMain, true);
				evtRedraw.Set();

				lstSignals.BuildList(true);

//				_s.DrawZedGraph(zgSmall, false);
//				zgMain.Invalidate();
			}
			else
				Invoke(new UpdateSignalDelegate(UpdateSignal), pSignal);
		}

		public void UpdateSignals()
		{
			if (!InvokeRequired)
			{
				lstSignals.BuildList(true);
				//lstSignals.Update();

				if (rcs != null)
				{
					foreach (BaseSignal _s in rcs.Signals)
					{
						_s.DrawZedGraph(zgMain, true);
						_s.DrawZedGraph(zgSmall, false);
					}

					evtRedraw.Set();
					//zgMain.Invalidate();

					if (rcs.OperationMode != ERadioControlMode.Idle ||
						lstSignals.SelectedItem == null)
					{
						toolBtnCreateSignalTraces.Enabled = false;
						toolBtnDeleteSignal.Enabled = false;
						toolBtnEditSignal.Enabled = false;
					}
					else
					{
						toolBtnCreateSignalTraces.Enabled = false;
						toolBtnDeleteSignal.Enabled = true;
						toolBtnEditSignal.Enabled = true;
					}

					if (rcs.OperationMode != ERadioControlMode.Idle ||
						rcs.Signals.Count == 0)
					{
						toolBtnSaveSignals.Enabled = false;
						toolBtnClearSignals.Enabled = false;
						toolBtnSignalsReport.Enabled = false;
						toolBtnSignalFilter.Enabled = false;
					}
					else
					{
						toolBtnSaveSignals.Enabled = true;
						toolBtnClearSignals.Enabled = true;
						toolBtnSignalsReport.Enabled = true;
						toolBtnSignalFilter.Enabled = true;
					}

				}
			}
			else
				Invoke(new MethodInvoker(UpdateSignals), null);

		}

		public delegate void UpdateMarkerDelegate(BaseRadioMarker pMarker);

		public void UpdateMarker(BaseRadioMarker pMarker)
		{
			if (!InvokeRequired)
			{
				pMarker.DrawZedGraph(zgMain);
				evtRedraw.Set();
			}
			else
				Invoke(new UpdateMarkerDelegate(UpdateMarker), pMarker);
		}

		public delegate void UpdateTraceDelegate(BaseTrace pTrace);

		public void UpdateTrace(BaseTrace pTrace)
		{
			if (!InvokeRequired)
			{
				pTrace.DrawZedGraph(zgMain, true);
				pTrace.DrawZedGraph(zgSmall, false);
			}
			else
				Invoke(new UpdateTraceDelegate(UpdateTrace), pTrace);

		}

		public void UpdateMarkers()
		{
			if (rcs == null)
				return;

			if (!InvokeRequired)
			{
				lstMarkers.BuildList(true);

				foreach (BaseRadioMarker _marker in rcs.Markers)
				{
					UpdateMarker(_marker);
					//_marker.DrawZedGraph(zgMain);
				}

				evtRedraw.Set();

				if (lstMarkers.SelectedItem == null)
				{
					toolBtnMarkerGo.Enabled = false;
					toolBtnEditMarker.Enabled = false;
					toolBtnDeleteMarker.Enabled = false;
				}
				else
				{
					toolBtnMarkerGo.Enabled = true;
					toolBtnEditMarker.Enabled = true;
					toolBtnDeleteMarker.Enabled = true;
				}

				if (lstMarkers.Items.Count > 0)
				{
					toolBtnClearMarkers.Enabled = true;
					toolBtnSaveMarkers.Enabled = true;
				}
				else
				{
					toolBtnClearMarkers.Enabled = false;
					toolBtnSaveMarkers.Enabled = false;
				}
			}
			else
				Invoke(new MethodInvoker(UpdateMarkers));
		}
		/// <summary>
		/// Обновление трасс
		/// </summary>
		void UpdateZedGraph()
		{
			evtRedraw.Set();
		}

		#endregion

		#region ================ Обработчики событий интерфейса ================

		/// <summary>
		/// Метод перерисовки основного окна
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgMain_Paint(object sender, PaintEventArgs e)
		{
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;

			double _min, _max;

			_min = _ctrl.GraphPane.XAxis.Scale.Min;
			if (_min < 0)
				_min = 0;

			_max = _ctrl.GraphPane.XAxis.Scale.Max;
			if (_max > _rcsFMax)
				_max = _rcsFMax;

			foreach (CurveItem _item in _pane.CurveList)
			{
				TraceFilteredPointList _pntList = _item.Points as TraceFilteredPointList;

				if (_pntList != null)
				{
					_pntList.SetBounds(_min, _max, GRAPHPANE_N_POINTS);
				}
			}

			/// Обновление индикатора на мелком навигаторе
			/// TODO: Сделать нормальные условия!
			if (_pane.XAxis.Scale.Min > 0)
				navigatorHighlight.Location.X = _pane.XAxis.Scale.Min / (double)_rcsFMax;
			else
				navigatorHighlight.Location.X = 0;

			if(_pane.XAxis.Scale.Max < _rcsFMax)
				navigatorHighlight.Location.Width = (_pane.XAxis.Scale.Max - _pane.XAxis.Scale.Min) / (double)_rcsFMax;
			else
				navigatorHighlight.Location.Width = ((double)_rcsFMax - _pane.XAxis.Scale.Min) / (double)_rcsFMax;

			if (navigatorHighlight.Location.Width < 0.002)
				navigatorHighlight.Location.Width = 0.002;

			//zgSmall.Invalidate();
			//zgMain.AxisChange();

			zgSmall.Invalidate();
		}

		/// <summary>
		///  Отрисовка мелкого навигатора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgSmall_Paint(object sender, PaintEventArgs e)
		{
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;

			double _min, _max;

			_min = _ctrl.GraphPane.XAxis.Scale.Min;
			if (_min < 0)
				_min = 0;

			_max = _ctrl.GraphPane.XAxis.Scale.Max;
			if (_max > _rcsFMax)
				_max = _rcsFMax;

			foreach (CurveItem _item in _pane.CurveList)
			{
				TraceFilteredPointList _pntList = _item.Points as TraceFilteredPointList;

				if (_pntList != null)
				{
					_pntList.SetBounds(_min, _max, GRAPHPANE_N_POINTS);
				}
			}
		}

		/// <summary>
		/// Клик по мелкому навигатору. Перестановка основного окна в место клика
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgSmall_MouseClick(object sender, MouseEventArgs e)
		{
			PointF _pt = new PointF(e.X, e.Y);
			double _x, _y;
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;

			_pane.ReverseTransform(_pt, out _x, out _y);

			double _span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;

			zgMain.GraphPane.XAxis.Scale.Max = _x + _span / 2.0;
			zgMain.GraphPane.XAxis.Scale.Min = _x - _span / 2.0;

			evtRedraw.Set();

//			zgMain.AxisChange();
//			zgMain.Invalidate();
		}

		/// <summary>
		/// Обработка события вывода надписи на оси частоты
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		string XAxis_ScaleTitleEvent(Axis axis)
		{
			int _mag = axis.Scale.Mag;
			string _magTxt = "";

			if (_mag == 3)
				_magTxt = ", " + Locale.khz;
			else if (_mag == 6)
				_magTxt = ", " + Locale.mhz;
			else if (_mag == 9)
				_magTxt = ", " + Locale.ghz;

			return Locale.frequency + _magTxt;
		}
		/// <summary>
		/// Обработка клика мышкой
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgMain_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PointF _pntF = new PointF(e.X, e.Y);
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;

			double _x, _y;
			
			_pane.ReverseTransform(_pntF, out _x, out _y);
			_x = Math.Round(_x);

			CursorFrequency = (Int64)_x;
#if NO_EVENTS
			rcs.RPUManager.ManualRPU.BaseFreq = cursorFrequency;
#endif
			if (OnCursorFrequencyChanged != null)
				OnCursorFrequencyChanged.Invoke(cursorFrequency);

			evtRedraw.Set();

//			zgMain.Invalidate();
		}

		/// <summary>
		///  Обработка изменения оси
		/// </summary>
		/// <param name="pane"></param>
		void GraphPane_AxisChangeEvent(GraphPane pane)
		{
//			waterfall1.Fmin = (Int64)pane.XAxis.Scale.Min;
//			waterfall1.Fmax = (Int64)pane.XAxis.Scale.Max;
		}

		/// <summary>
		/// Обработка клика мышкой
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void zgMain_MouseClick(object sender, MouseEventArgs e)
		{
#if false
			ZedGraphControl _ctrl = sender as ZedGraphControl;
			GraphPane _pane = _ctrl.GraphPane;
			object _o;
			int _idx;

			double _scale = Math.Sqrt((_pane.XAxis.Scale.Max - _pane.XAxis.Scale.Min) / (_pane.Rect.Width * _pane.Rect.Height));

			GraphPane.Default.NearestTol = 20;

			_pane.FindNearestObject(new PointF(e.X, e.Y), Graphics.FromHwnd(_ctrl.Handle), out _o, out _idx);

			if (!(_o is CurveItem))
				return;

			CurveItem _item = _o as CurveItem;

			if (_item.Tag is BaseTrace)
			{
				BaseTrace _trace = _item.Tag as BaseTrace;
				MessageBox.Show("Выбрана трасса " + _trace.Name);
			}
			else if (_item.Tag is BaseRadioMarker)
			{
				BaseRadioMarker _marker = _item.Tag as BaseRadioMarker;
				MessageBox.Show("Выбран маркер " + _marker.Name);
			}
			else
			{
				MessageBox.Show("Выбрано что-то еще");
			}
#endif
		}

		/// <summary>
		/// /////////////////////////////////////////////////////////////////
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkBtnMode_CheckedChanged(object sender, EventArgs e)
		{

			if (rcs.OperationMode == ERadioControlMode.Idle)
			{
				/// Запуск контроля по трасса на контроле
				/// 

				var q = from _s in rcs.ScanTraces 
						where _s.ScanMode == ETraceScanMode.Scan ||
						_s.ScanMode == ETraceScanMode.Control
						select _s;

				if(q.Count() == 0)
				{
					MessageBox.Show(Locale.no_scan_traces, 
						Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
					chkBtnMode.Checked = false;
					return;
				}

				bool _needReset = true;

				if (MessageBox.Show(Locale.trace_scan_reset,
					Locale.question,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button1) == DialogResult.No)
				{
					_needReset = false;
				}

////				SetArmedModeView();

				//new BaseRadioControlSystem.StartArmedModeDelegate(rcs.StartArmedMode).BeginInvoke(_needReset, null, null);
				rcs.StartArmedMode(_needReset);
			}
			else if(rcs.OperationMode == ERadioControlMode.Scan)
			{
				//new MethodInvoker(rcs.StopArmedMode).BeginInvoke(null, null);
				rcs.StopArmedMode();

				//SetNormalModeView();
//				chkBtnMode.Text = "Сканирование";
			}
		}

		Dictionary<ToolStripButton, bool> buttonStates = new Dictionary<ToolStripButton, bool>();

		void SetToolStripButtonState(ToolStripButton pControl, bool pState)
		{
			//if (controlStates.Keys.Contains(pControl))
			buttonStates[pControl] = pControl.Enabled;
			pControl.Enabled = pState;
			//else
			//	controlStates.Add(pControl, pControl.Enabled);
		}

		void SetLastControlsState()
		{
			foreach (ToolStripButton _c in buttonStates.Keys)
			{
				_c.Enabled = buttonStates[_c];
			}
			//pControl.Enabled = controlStates[pControl];
		}
		/// <summary>
		/// Изменения отображения для режима сканирования
		/// </summary>
		public void SetArmedModeView()
		{
			if (!InvokeRequired)
			{
				chkBtnMode.Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.button_scan.png"));

				btnLoadRCSState.Enabled = false;
				btnSaveRCSState.Enabled = false;
				toolBtnNewSignal.Enabled = false;

				//chkZeroSpan.Enabled = false;
				xPanelConfig.Enabled = false;

				SetToolStripButtonState(toolBtnNewTrace, false);
				SetToolStripButtonState(toolBtnOpenTrace, false);
				SetToolStripButtonState(toolBtnSaveTrace, false);
				SetToolStripButtonState(toolBtnDeleteTrace, false);
				SetToolStripButtonState(toolBtnEditTrace, false);
				SetToolStripButtonState(toolBtnScanParams, false);
				SetToolStripButtonState(toolBtnArmScanTrace, false);
				SetToolStripButtonState(toolBtnTraceMarkers, false);
				SetToolStripButtonState(toolBtnReplayTrace, false);
				SetToolStripButtonState(toolBtnLoadSignals, false);
				SetToolStripButtonState(toolBtnSaveSignals, false);
				SetToolStripButtonState(toolBtnClearSignals, false);
				

				//SetToolStripButtonState(toolBtnWaterfallWaterfall, false);
				SetToolStripButtonState(toolBtnWaterfallZeroSpan, false);

			}
			else
				Invoke(new MethodInvoker(SetArmedModeView));
		}

		/// <summary>
		/// Изменение отображения для основного режима
		/// </summary>
		public void SetNormalModeView()
		{
			if (!InvokeRequired)
			{
				//chkBtnMode.BackColor = Color.FromName("ControlLight");
				chkBtnMode.Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.button_idle.png"));
				//chkZeroSpan.Enabled = true;
				xPanelConfig.Enabled = true;

				btnLoadRCSState.Enabled = true;
				btnSaveRCSState.Enabled = true;
				toolBtnNewSignal.Enabled = true;

				SetLastControlsState();
			}
			else
				Invoke(new MethodInvoker(SetNormalModeView));
		}
#if false
		private void toolsBtnNewTrace_Click(object sender, EventArgs e)
		{
			IRPU _rpu = rcs.RPUManager.UserSelectAvailableRPU();

			if (_rpu == null)
				return;

			if (_rpu.PowerMeter == null || _rpu.HasPowerMeter == false)
			{
				MessageBox.Show("Выбранное РПУ не поддерживает режим измерения мощности");
				return;
			}

			BaseTrace _trace = _rpu.PowerMeter.UserCreateNewTrace((long)(zgMain.GraphPane.XAxis.Scale.Min / 1000) * 1000,
				(long)(zgMain.GraphPane.XAxis.Scale.Max / 1000) * 1000);
			
			if (_trace != null)
			{
				rcs.AddScanTrace(_trace);

				_trace.OnTraceScanChanged += new TraceChanged(_trace_OnTraceChanged);

				/// Отрисовка трассы на контролах
				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				UpdateControl();
			}
		}
#endif

		private void toolsBtnNewTrace_Click(object sender, EventArgs e)
		{
			NewTraceDialog _dlg = new NewTraceDialog(true);
			_dlg.FStart = (long)(zgMain.GraphPane.XAxis.Scale.Min / 1000) * 1000;
			_dlg.FStop = (long)(zgMain.GraphPane.XAxis.Scale.Max / 1000) * 1000;
			_dlg.MeasureStep = 100000;
			_dlg.IsCyclic = true;

			Random _rnd = new Random();
			_dlg.cmbTraceColor.Value = Color.FromArgb(_rnd.Next(192) + 63, _rnd.Next(192) + 63, _rnd.Next(192) + 63);

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				BaseTrace _trace = new BaseTrace(_dlg.FStart, _dlg.FStop, _dlg.MeasureStep, 
					_dlg.InitialValue, _dlg.cmbTraceColor.Value);

				_trace.Name = _dlg.TraceName;
				_trace.Description = _dlg.Description;

				rcs.AddScanTrace(_trace);

				_trace.OnTraceScanChanged += new TraceChanged(_trace_OnTraceChanged);

				/// Отрисовка трассы на контролах
				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				UpdateControl();
			}
		}

		#endregion

		#region ================ Свойства интерфейса ================

		public bool CtrlPressed
		{
			get { return ctrlPressed; }
			set { ctrlPressed = value; }
		}

		public bool ShiftPressed
		{
			get { return shiftPressed; }
			set { shiftPressed = value; }
		}

		public BaseRadioControlSystem RCS
		{
			get
			{
				return rcs;
			}

			set
			{
				rcs = value;

				if (rcs != null)
				{
					rcs.Control = this;
#if NO_EVENTS
					rcs.RPUManager.ManualRPU.OnBaseFrequencyChanged += new BaseFrequencyChanged(ManualRPU_OnBaseFrequencyChanged);
#endif
					lstScanTraces.SetObjects(rcs.ScanTraces);
					lstSignals.SetObjects(rcs.Signals);
					lstMarkers.SetObjects(rcs.Markers);

					ctrlRadioConfig.RCS = rcs;
					rcs.RPUManager.OnManualRPUChanged += new ManualRPUChangedDelegate(RPUManager_OnManualRPUChanged);
					rcs.OnOperationModeChanged += new BaseRadioControlSystem.OperationModeChangedDelegate(rcs_OnOperationModeChanged);
					SetupRPUView(rcs.RPUManager.ManualRPU);

					UpdateControl();
				}
			}
		}

		/// <summary>
		/// Обработка смены режима СРК
		/// </summary>
		/// <param name="pOldMode"></param>
		/// <param name="pNewMode"></param>
		void rcs_OnOperationModeChanged(ERadioControlMode pOldMode, ERadioControlMode pNewMode)
		{
			switch (pNewMode)
			{
				case ERadioControlMode.Idle:
					SetNormalModeView();
//					xPanderPanelList1.Enabled = true;
					break;

				case ERadioControlMode.Scan:
					SetArmedModeView();
//					xPanderPanelList1.Enabled = false;
					break;

				case ERadioControlMode.Pause:
					SetNormalModeView();
//					xPanderPanelList1.Enabled = false;
					break;
			}
		}

		/// <summary>
		/// Обработка события изменения ручного РПУ
		/// </summary>
		/// <param name="pOldRPU"></param>
		/// <param name="pNewRPU"></param>
		void RPUManager_OnManualRPUChanged(IRPU pOldRPU, IRPU pNewRPU)
		{
			SetupRPUView(pNewRPU);
		}

		void SetupRPUView(IRPU pRPU)
		{
			if (waterfallMode == EWaterfallMode.ZeroSpan)
				StopZeroSpan();

			/// Анализ возможностей нового РПУ
			/// 
			if (pRPU.HasPowerMeter)
			{
				toolBtnWaterfallZeroSpan.Enabled = true;
				//				toolBtnWaterfallWaterfall.Enabled = true;
			}
			else
			{
				toolBtnWaterfallZeroSpan.Enabled = false;
			}
		}

#if NO_EVENTS
		delegate void ManualRPU_OnBaseFrequencyChangedDelegate(long pBaseFreq);

		void ManualRPU_OnBaseFrequencyChanged(long pBaseFreq)
		{
//			if (!InvokeRequired)
			{
				CursorFrequency = pBaseFreq;
			}
//			else
			{
//				Invoke(new ManualRPU_OnBaseFrequencyChangedDelegate(ManualRPU_OnBaseFrequencyChanged), pBaseFreq);
			}
		}
#endif

		public Int64 CursorFrequency
		{
			get
			{
				return cursorFrequency;
			}

			set
			{
				/// Округление до целого

				cursorPoints[0].X = value;
				cursorPoints[1].X = value;

				/// Установка частоты курсора
				cursorFrequency = value;
				lock (needRedrawLock)
				{
					needRedraw = true;
				}
//				evtRedraw.Set();

/*				if (!tmrRedraw.Enabled)
				{
					tmrRedraw.Interval = 500;
					tmrRedraw.Start();
				}
*/
			}
		}

		#endregion

		/// <summary>
		/// Загрузка трассы из файла
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnOpenTrace_Click(object sender, EventArgs e)
		{
			OpenFileDialog _openDlg = new OpenFileDialog();

			_openDlg.Filter = Locale.ktr_filter;
			_openDlg.FilterIndex = 0;
			_openDlg.Title = Locale.open_trace;
			_openDlg.InitialDirectory = Application.StartupPath + "\\traces";

			if (_openDlg.ShowDialog() == DialogResult.OK)
			{
				/// Загрузка трассы из выбранного файла
				/// 
				BaseTrace _trace = rcs.LoadTraceFromFile(_openDlg.FileName);

				/// Если трасса не загрузилась - выходим
				if (_trace == null)
					return;

				_trace.OnTraceScanChanged += new TraceChanged(_trace_OnTraceChanged);

				rcs.AddScanTrace(_trace);

				/// Отрисовка трассы на контролах
				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				

				UpdateControl();
			}
		}

		/// <summary>
		/// Сохранение выбранной трассы в файл
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnSaveTrace_Click(object sender, EventArgs e)
		{
			OLVListItem _listItem = (OLVListItem)lstScanTraces.SelectedItem;
			
			if (_listItem == null)
				return;

			BaseTrace _trace = (BaseTrace)_listItem.RowObject;

			if (_trace == null)
				return;

			SaveFileDialog _saveDlg = new SaveFileDialog();

			_saveDlg.Filter = Locale.ktr_filter;
			_saveDlg.FilterIndex = 0;
			_saveDlg.Title = Locale.save_trace;
			_saveDlg.InitialDirectory = Application.StartupPath + "\\traces";

			if (_saveDlg.ShowDialog() == DialogResult.OK)
			{
				/// Сохранение трассы в выбранный файл
				/// 
				try
				{
					rcs.SaveTraceToFile(_saveDlg.FileName, _trace);
				}
				catch
				{
					throw;
				}

			}
		}

		private void toolBtnDeleteTrace_Click(object sender, EventArgs e)
		{
			OLVListItem _listItem = (OLVListItem)lstScanTraces.SelectedItem;

			if (_listItem == null)
				return;

			BaseTrace _trace = (BaseTrace)_listItem.RowObject;

			if (_trace == null)
				return;

			if (MessageBox.Show(String.Format(Locale.confirm_delete_trace, _trace.Name,
				FreqUtils.FreqToString(_trace.Fstart), FreqUtils.FreqToString(_trace.Fstop),
				FreqUtils.FreqToString(_trace.MeasureStep)),
				Locale.confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				/// Удаление выбранной трассы
				/// 
				_trace.IsVisible = false;
				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				rcs.DeleteScanTrace(_trace);
				
				UpdateControl();
			}
		}

		/// <summary>
		/// Изменение индекса выбранной трассы в списке трасс
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstScanTraces_SelectedIndexChanged(object sender, EventArgs e)
		{
			OLVListItem _item = (OLVListItem)lstScanTraces.SelectedItem;

			if (_item == null)
			{

				toolBtnDeleteTrace.Enabled = false;
				toolBtnSaveTrace.Enabled = false;
				toolBtnArmScanTrace.Enabled = false;
				toolBtnEditTrace.Enabled = false;
				
				toolBtnScanParams.Enabled = false;
				toolBtnTraceMarkers.Enabled = false;
				toolBtnReplayTrace.Enabled = false;

				toolBtnTraceGo.Enabled = false;

				foreach (CurveItem _i in zgMain.Selection)
				{
					BaseTrace _tr = _i.Tag as BaseTrace;
					if (_tr != null)
					{
						_tr.IsSelected = false;
						_tr.DrawZedGraph(zgMain, false);
					}
				}
				zgMain.Selection.ClearSelection(zgMain.MasterPane);
				zgMain.Invalidate();
			}
			else
			{
				if (rcs.OperationMode == ERadioControlMode.Idle)
				{
					toolBtnDeleteTrace.Enabled = true;
					toolBtnSaveTrace.Enabled = true;
					toolBtnArmScanTrace.Enabled = true;
					toolBtnEditTrace.Enabled = true;

					toolBtnScanParams.Enabled = true;
					toolBtnTraceMarkers.Enabled = true;
					toolBtnReplayTrace.Enabled = true;
				}
				toolBtnTraceGo.Enabled = true;

				BaseTrace _trace = _item.RowObject as BaseTrace;

				if (_trace != null && _trace.IsVisible)
				{
					zgMain.Selection.Select(zgMain.MasterPane, _trace.LineItem);
					_trace.IsSelected = true;
					_trace.DrawZedGraph(zgMain, false);
					zgMain.Invalidate();
				}

			}
		}

		/// <summary>
		/// Обработка нажатия на кнопку контроля по трассе
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnArmScanTrace_Click(object sender, EventArgs e)
		{
			OLVListItem _listItem = (OLVListItem)lstScanTraces.SelectedItem;

			if (_listItem == null)
				return;

			BaseTrace _trace = (BaseTrace)_listItem.RowObject;

			if (_trace == null)
				return;

			if (_trace.ScanParams == null)
			{
				if (MessageBox.Show(String.Format(Locale.no_scan_params, _trace.Name),
					Locale.question, MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
				{
					toolBtnScanParams_Click(sender, e);

					/// Если произошла ошибка задания параметров сканирования, то выходим
					if (_trace.ScanParams == null)
						return;
				}
				else
				{
					return;
				}			
			}
/*			if (MessageBox.Show(@"Включить контроль по трассе""" + _trace.Name + @"""?",
				"Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1) == DialogResult.Yes)*/
			{
				rcs.ArmTrace(_trace);

				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				UpdateControl();
			}
		}

		/// <summary>
		/// Создание нового маркера
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnNewMarker_Click(object sender, EventArgs e)
		{
			BaseRadioMarker _marker = BaseRadioMarker.FromDialog(cursorFrequency);

			if (_marker != null)
			{
				/// Добавление маркера в список маркеров и отображение его на навигаторе
				/// 
				rcs.AddMarker(_marker);
				UpdateControl();

			}
		}

		/// <summary>
		/// Тело отрисовщика трасс 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bgwRedraw_DoWork(object sender, DoWorkEventArgs e)
		{
			bool _evtRes;

			while (!bgwRedraw.CancellationPending)
			{
				/// Ждем запуска отрисовки
				_evtRes = evtRedraw.WaitOne(500, true);

				//Application.DoEvents();

				if (_evtRes || needRedraw == true)
				{
					lock (needRedrawLock)
					{
						needRedraw = false;
					}

					/// Рисуем
					zgMain.AxisChange();
					zgMain.Invalidate();

					//zgSmall.AxisChange();
					zgSmall.Invalidate();

					//// Waterfall

					//if (!zgWaterfallInvalidateRect.IsEmpty)
					//{
					//    zgWaterfall.Invalidate(zgWaterfallInvalidateRect);
					//}
					//else
					//{
					//    zgWaterfall.AxisChange();
					//    zgWaterfall.Invalidate();
					//}
				}
			}
		}

		/// <summary>
		/// Диалоговое окно с ожиданием реакции пользователя на событие тревоги
		/// </summary>
		/// <param name="pTimeout"></param>
		/// <param name="pFreq"></param>
		/// <param name="pPower"></param>
		/// <returns></returns>
		delegate DialogResult ShowPauseWindowDelegate(int pTimeout, string pCaption, 
			string pFreq, string pPower, string pName, string pDelta, Color pBackColor);

		public DialogResult ShowPauseWindow(int pTimeout, string pCaption, string pFreq, string pPower, 
			string pName, string pDelta, Color pBackColor)
		{
			if (!InvokeRequired)
			{
				RCSPauseDialog _dlg = new RCSPauseDialog(pTimeout, pCaption, pFreq, pPower, pName, pDelta, pBackColor);
				_dlg.Location = new Point(this.ParentForm.Width - _dlg.Width - 5, 
					Height - _dlg.Height - 5);

				
				/*_dlg.Location = new Point(this.ParentForm.Location.X + splitContainer1.SplitterDistance 
					- _dlg.Width - 5, Height - 40);
				 */
				return _dlg.ShowDialog();
			}
			else
			{
				return (DialogResult)Invoke(new ShowPauseWindowDelegate(ShowPauseWindow), 
					pTimeout, pCaption, pFreq, pPower, pName, pDelta, pBackColor);
			}
		}

		private void lstScanTraces_MouseClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstScanTraces.HitTest(e.Location);

			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;
			if (_item == null) 
				return;
			
			BaseTrace _trace = (BaseTrace)_item.RowObject;
			if (_trace == null)
				return;

			/// Если при щелчке нажата кнопка Ctrl, то масштабируем ось
			/// частоты под трассу
			if (ctrlPressed)
			{
				zgMain.GraphPane.XAxis.Scale.Min = _trace.Fstart - _trace.MeasureStep * 2;
				zgMain.GraphPane.XAxis.Scale.Max = _trace.Fstop + _trace.MeasureStep * 2;
				
				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}

		/// <summary>
		/// Добавление нового сигнала
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnNewSignal_Click(object sender, EventArgs e)
		{
			
		}

		private void toolBtnSaveSignals_Click(object sender, EventArgs e)
		{
			SaveFileDialog _saveDlg = new SaveFileDialog();

			_saveDlg.Filter = Locale.ksi_filter;
			_saveDlg.FilterIndex = 0;
			_saveDlg.Title = Locale.save_signals;
			_saveDlg.InitialDirectory = Application.StartupPath + "\\signals";

			if (_saveDlg.ShowDialog() == DialogResult.OK)
			{
				/// Сохранение сигналов в выбранный файл
				/// 
				try
				{
					rcs.SaveSignalsToFile(_saveDlg.FileName);

				}
				catch
				{
					throw;
				}
				finally
				{
					UpdateControl();
				}

			}
		}

		private void toolBtnLoadSignals_Click(object sender, EventArgs e)
		{
			OpenFileDialog _openDlg = new OpenFileDialog();
			_openDlg.Filter = Locale.ksi_filter;
			_openDlg.FilterIndex = 0;
			_openDlg.Title = Locale.load_signals;
			_openDlg.InitialDirectory = Application.StartupPath + "\\signals";

			if (_openDlg.ShowDialog() == DialogResult.OK)
			{
				/// Сохранение сигналов в выбранный файл
				/// 
				try
				{
					bool _needClear = false;

					if (MessageBox.Show(Locale.confirm_delete_cur_signals, 
						Locale.confirmation,
						MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						_needClear = true;

					rcs.LoadSignalsFromFile(_openDlg.FileName, _needClear);
					
					//lstSignals.SetObjects(FilteredSignals);
					
				}
				catch
				{
					throw;
				}

				finally
				{
					UpdateControl();
				}
			}


		}

		private void toolBtnDeleteSignal_Click(object sender, EventArgs e)
		{
			if (lstSignals.SelectedItems.Count == 0)
				return;

			if (MessageBox.Show(Locale.confirm_delete_marked_signals,
					Locale.confirmation,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2) == DialogResult.No)
				return;

			foreach (ListViewItem _item in lstSignals.SelectedItems)
			{
				OLVListItem _listItem = (OLVListItem)_item;

				if (_listItem == null)
					continue;

				BaseSignal _signal = (BaseSignal)_listItem.RowObject;

				if (_signal == null)
					continue;

				/// Удаление выбранного сигнала
				/// 
				_signal.IsVisible = false;
				rcs.RemoveSignal(_signal);

				if (_signal == signalChart)
					ShowSignalChart(null, false);

			}

			UpdateControl();
		}

		/// <summary>
		/// Кнопка очистки списка сигналов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBthnClearSignals_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Locale.confirm_clear_signals,
				Locale.confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				/*foreach (BaseSignal _s in rcs.Signals)
				{
					_s.IsVisible = false;
				}
				*/
				rcs.ClearSignals();

				/// Очистка панели параметров сигнала
				ShowSignalChart(null, false);

				UpdateControl();
			}

		}

		/// <summary>
		/// Обновление списка сигналов при переключении фильтров
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void signalsToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			//lstSignals.SetObjects(FilteredSignals);
			UpdateControl();

			if (!unknownToolStripMenuItem.Checked || 
				!redToolStripMenuItem.Checked || 
				!yellowToolStripMenuItem.Checked || 
				!greenToolStripMenuItem.Checked)
			{
				toolBtnSignalFilter.BackColor = Color.FromName("LightSalmon");
			}
			else
			{
				toolBtnSignalFilter.BackColor = Color.FromName("Transparent");
			}
		}

		private void RangeSignalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			long _freq;
			long _band;

			RangeSignal _signal = new RangeSignal();

			_freq = (long)((zgMain.GraphPane.XAxis.Scale.Max + 
				zgMain.GraphPane.XAxis.Scale.Min) / 2000.0) * 1000;
			_band = (long)((zgMain.GraphPane.XAxis.Scale.Max - 
				zgMain.GraphPane.XAxis.Scale.Min) / 1000.0) * 1000;
			_signal.Frequency = _freq;
			_signal.Band = _band;
			_signal.Pmin = -140;
			_signal.Pmax = 0;

			rcs.AddSignal(_signal);

			_signal.IsVisible = true;

			ShowSignalChart(_signal, true);

			UpdateSignals();
		}

		private void btnPrevSignalChart_Click(object sender, EventArgs e)
		{
			int _idx = rcs.Signals.IndexOf(signalChart);
			if (_idx > 0)
			{
				ShowSignalChart(rcs.Signals[_idx - 1], true);
			}
		}

		void ShowSignalChart(BaseSignal pSignal, bool pNeedExpand)
		{
			splitContainer4.Panel2.Controls.Clear();

			if (selectedSignal != null)
				selectedSignal.Selected = false;

			if (pSignal != null)
			{

				selectedSignal = pSignal;
				selectedSignal.Selected = true;

				

				splitContainer4.Panel2.VerticalScroll.Visible = true;
				splitContainer4.Panel2.Controls.Add(pSignal.SignalControl);
				pSignal.SignalControl.Width = splitContainer4.Panel2.Width - 18;
				pSignal.SignalControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

				signalChart = pSignal;

				if (pNeedExpand)
				{
					xPanderPanelList1.Expand(xPanelSignalChart);
				}

				/// Отрисовка сигнала на zgWaterfall
				/// 
				zgWaterfall.GraphPane.XAxis.Scale.Min = pSignal.Frequency - pSignal.Band / 2;
				zgWaterfall.GraphPane.XAxis.Scale.Max = pSignal.Frequency + pSignal.Band / 2;

				zgWaterfall.GraphPane.CurveList.Clear();

				/// Отрисовка нулевого цикла сигнала
				SignalTraceCycle _cycle = pSignal.SignalCycles.Last();

				if (_cycle != null)
				{
					zgWaterfall.GraphPane.CurveList.Add(_cycle.LineItem);
				}

				zgWaterfall.AxisChange();
				zgWaterfall.Invalidate();

				btnCreateReport.Enabled = pSignal.CanCreateReport;
				btnReplaySignalCycles.Enabled = true;
				btnSpectrogram.Enabled = true;
				btnSetSignalFreq.Enabled = true;

				if (rcs.Signals.IndexOf(signalChart) > 0)
					btnPrevSignalChart.Enabled = true;
				else
					btnPrevSignalChart.Enabled = false;

				if (rcs.Signals.IndexOf(signalChart) < rcs.Signals.Count - 1)
					btnNextSignalChart.Enabled = true;
				else
					btnNextSignalChart.Enabled = false;

			}
			else
			{
				btnCreateReport.Enabled = false;
				btnPrevSignalChart.Enabled = false;
				btnNextSignalChart.Enabled = false;
				btnReplaySignalCycles.Enabled = false;
				btnSpectrogram.Enabled = false;
				btnSetSignalFreq.Enabled = false;
			}
		}

		private void btnNextSignalChart_Click(object sender, EventArgs e)
		{
			int _idx = rcs.Signals.IndexOf(signalChart);
			if (_idx < rcs.Signals.Count - 1)
			{
				ShowSignalChart(rcs.Signals[_idx + 1], true);
			}

		}

		private void btnSetSignalFreq_Click(object sender, EventArgs e)
		{
			double _span;

			if (signalChart == null)
				return;

			if (signalChart.IsVisible && ctrlPressed)
			{
				/// Если нажа Ctrl, то спан ставим размером в сигнал
				_span = signalChart.Band * 1.1;
				
//				zgMain.GraphPane.XAxis.Scale.Min = signalChart.Frequency - _span / 2.0;
//				zgMain.GraphPane.XAxis.Scale.Max = signalChart.Frequency + _span / 2.0;
			}
			else
			{
				/// Отрисовка сигнала без изменения спана
				/// 
				_span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;
//				zgMain.GraphPane.XAxis.Scale.Min = signalChart.Frequency - _span / 2.0;
//				zgMain.GraphPane.XAxis.Scale.Max = signalChart.Frequency + _span / 2.0;
			}

			SetFreqSpan(zgMain.GraphPane.XAxis.Scale, signalChart.Frequency, _span);

			CursorFrequency = (Int64)signalChart.Frequency;
			rcs.RPUManager.ManualRPU.SetParamsFromSignal(signalChart);
			evtRedraw.Set();
		}

		private void toolBtnMarkerDelta_Click(object sender, EventArgs e)
		{
			BaseRadioMarker _marker = BaseRadioMarker.FromDialog(cursorFrequency);

			if (_marker != null)
			{
				/// Добавление маркера в список маркеров и отображение его на навигаторе
				/// 
				rcs.AddMarker(_marker);
				UpdateControl();

			}
		}

		private void toolBtnLoadMarkers_Click(object sender, EventArgs e)
		{
			OpenFileDialog _openDlg = new OpenFileDialog();
			_openDlg.Filter = Locale.kma_filter;
			_openDlg.FilterIndex = 0;
			_openDlg.Title = Locale.load_markers;
			_openDlg.InitialDirectory = Application.StartupPath + "\\markers";

			if (_openDlg.ShowDialog() == DialogResult.OK)
			{
				/// Сохранение сигналов в выбранный файл
				/// 
				try
				{
					bool _needClear = false;

					if (MessageBox.Show(Locale.confirm_delete_cur_markers, 
						Locale.confirmation,
						MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						_needClear = true;

					rcs.LoadMarkersFromFile(_openDlg.FileName, _needClear);
				}
				catch
				{
					throw;
				}

				finally
				{
					UpdateControl();
				}
			}

		}

		private void toolBtnSaveMarkers_Click(object sender, EventArgs e)
		{
			SaveFileDialog _saveDlg = new SaveFileDialog();

			_saveDlg.Filter = Locale.kma_filter;
			_saveDlg.FilterIndex = 0;
			_saveDlg.Title = Locale.save_markers;
			_saveDlg.InitialDirectory = Application.StartupPath + "\\markers";

			if (_saveDlg.ShowDialog() == DialogResult.OK)
			{
				/// Сохранение маркеров в выбранный файл
				/// 
				try
				{
					rcs.SaveMarkersToFile(_saveDlg.FileName);

				}
				catch
				{
					throw;
				}
				finally
				{
					UpdateControl();
				}

			}
		}

		/// <summary>
		/// Сохранение отчета по списку сигналов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnSignalsReport_Click(object sender, EventArgs e)
		{
			if (rcs.Signals.Count <= 0)
			{
				System.Windows.Forms.MessageBox.Show(
					Locale.no_selected_signals, 
					Locale.error,
					System.Windows.Forms.MessageBoxButtons.OK,
					System.Windows.Forms.MessageBoxIcon.Warning);
				return;
			}

			BaseSignalReport report = new BaseSignalReport(this.rcs);
			String FileName = ReportsManager.NewReportName(
				ReportType.PDF,
				String.Format(Locale.report_signals));
			report.Save(FileName);
			System.Diagnostics.Process.Start(FileName);
		}

		private void bgwDrawCycles_DoWork(object sender, DoWorkEventArgs e)
		{
			BaseTrace _trace = e.Argument as BaseTrace;

			if (_trace == null)
				return;

			foreach(SignalTraceCycle _stc in _trace.SignalCycles.ToList())
			{
				if (_stc == null)
					continue;

				zgWaterfall.GraphPane.CurveList.Clear();
				zgWaterfall.GraphPane.CurveList.Add(_stc.LineItem);
				//zgWaterfall.GraphPane.CurveList.Add(_stc.BarItem);
				zgWaterfall.Invalidate();

				Thread.Sleep(250);

				if (bgwDrawCycles.CancellationPending)
					return;
			}
		}

		private void btnReplaySignalCycles_Click(object sender, EventArgs e)
		{
			if (!bgwDrawCycles.IsBusy)
			{
				/// Запуск проигрывания трасс
				OLVListItem _item = (OLVListItem)lstSignals.SelectedItem;

				if (_item == null)
					return;

				BaseSignal _signal = (BaseSignal)_item.RowObject;

				if (_signal == null)
					return;

				bgwDrawCycles.RunWorkerAsync(_signal);
			}
			else
			{
				/// Останов проигрывателя сигнала
				bgwDrawCycles.CancelAsync();
			}
		}

		private void bgwDrawCycles_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			btnReplaySignalCycles.Enabled = true;

			toolBtnReplayTrace.Image = imageList1.Images["play.png"];
		}

		private void btnSpectrogram_Click(object sender, EventArgs e)
		{

			/// Запуск спектрограммы
			OLVListItem _item = (OLVListItem)lstSignals.SelectedItem;

			if (_item == null)
				return;

			BaseSignal _signal = (BaseSignal)_item.RowObject;

			if (_signal == null)
				return;

			zgWaterfall.GraphPane.CurveList.Clear();

			int _i = 0;

			foreach (SignalTraceCycle _stc in _signal.SignalCycles.ToList())
			{
				if (_stc == null)
					continue;
				((GradientLineItem)_stc.BarItem).DrawY = _i * 5;
				_i++;

				zgWaterfall.GraphPane.CurveList.Add(_stc.BarItem);
			}

			zgWaterfall.Invalidate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCreateReport_Click(object sender, EventArgs e)
		{
			if (signalChart == null)
				return;

			signalChart.CreateSignalReport();
		}

		#region ================ Зеро спан ================

		double[] zeroSpanX;
		double[] zeroSpanY;
		LineItem zeroSpanLine;
		int zeroSpanPointNo;
//		Rectangle zgWaterfallInvalidateRect;
		const int zeroSpanPointCount = 200;
		PointPairList zeroSpanPoints;
		GraphPane zgWaterfallPane;

//		AutoResetEvent evtRedrawWaterfall = new AutoResetEvent(false);
		object needRedrawWaterfallLock = new object();
		bool needRedrawWaterfall;

#if false
		/// <summary>
		/// Запуск|останов режима ZeroSpan
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSpanZero_Click(object sender, EventArgs e)
		{
			if (!rcs.RPUManager.ManualRPU.PowerMeter.IsRunning)
			{
				/// Запуск режима ZeroSpan
				/// 
				zeroSpanX = new double[zeroSpanPointCount];
				zeroSpanY = new double[zeroSpanPointCount];

				for (int _i = 0; _i < zeroSpanPointCount; _i++)
				{
					zeroSpanX[_i] = _i*2;
					zeroSpanY[_i] = -125;
				}
				zeroSpanPointNo = 0;

				SetupZeroSpanView();

				zeroSpanPoints = new PointPairList(zeroSpanX, zeroSpanY);
				zeroSpanLine = zgWaterfall.GraphPane.AddCurve("", 
					zeroSpanPoints, 
					Color.LimeGreen, SymbolType.None);

				zeroSpanLine.Line.StepType = StepType.ForwardStep;
				zeroSpanLine.Line.IsOptimizedDraw = true;
				zeroSpanLine.Line.IsAntiAlias = false;
				zeroSpanLine.Symbol.IsVisible = false;

				zgWaterfallPane = zgWaterfall.GraphPane;

				zgWaterfall.AxisChange();
				zgWaterfall.Invalidate();

				rcs.RPUManager.ManualRPU.PowerMeter.OnNewPowerMeasure += new NewPowerMeasure(PowerMeter_OnNewPowerMeasure);
				rcs.RPUManager.ManualRPU.PowerMeter.Start();

				bgwWaterfall.RunWorkerAsync(null);
			}
			else
			{
				rcs.RPUManager.ManualRPU.PowerMeter.Stop();
				rcs.RPUManager.ManualRPU.PowerMeter.OnNewPowerMeasure -= PowerMeter_OnNewPowerMeasure;

				zgWaterfall.GraphPane.CurveList.Remove(zeroSpanLine);
				bgwWaterfall.CancelAsync();
			}

		}
#endif
		/// <summary>
		/// Обработка получения нового отсчета мощности
		/// </summary>
		/// <param name="pRPU"></param>
		/// <param name="pFrequency"></param>
		/// <param name="pPower"></param>
		void PowerMeter_OnNewPowerMeasure(IRPU pRPU, long pFrequency, float pPower)
		{
			zeroSpanPointNo = (zeroSpanPointNo + 1) % zeroSpanPointCount;

			PointPair _pData = zeroSpanPoints[zeroSpanPointNo];

			zeroSpanPoints[(zeroSpanPointNo)].Y = pPower;

			zeroSpanPoints[(zeroSpanPointNo + 2) % zeroSpanPointCount].Y = PointPair.Missing;
			zeroSpanPoints[(zeroSpanPointNo + 3) % zeroSpanPointCount].Y = PointPair.Missing;
//			zeroSpanPoints[(zeroSpanPointNo + 4) % zeroSpanPointCount].Y = PointPair.Missing;
//			zeroSpanPoints[(zeroSpanPointNo + 5) % zeroSpanPointCount].Y = PointPair.Missing;
			//zeroSpanPoints[(zeroSpanPointNo + 6) % zeroSpanPointCount].Y = PointPair.Missing;
#if false
			PointF _pntGraph =  zgWaterfallPane.GeneralTransform(_pData.X, _pData.Y, CoordType.AxisXYScale);
			
			//zeroSpanY[zeroSpanPointNo] = pPower;
			
			zgWaterfallInvalidateRect = Rectangle.FromLTRB((int)_pntGraph.X-10, 0, 
				(int)_pntGraph.X + 10, (int)zgWaterfallPane.Rect.Bottom);
#endif
			lock (needRedrawWaterfallLock)
			{
				needRedrawWaterfall = true;
			}
			//new WaterFallRedrawDelegate(WaterFallRedraw).Invoke(zgWaterfallInvalidateRect);
		}

		delegate void WaterFallRedrawDelegate(Rectangle pRect);

		void WaterFallRedraw(Rectangle pRect)
		{
			zgWaterfall.Invalidate(pRect);
		}
		
		private void bgwWaterfall_DoWork(object sender, DoWorkEventArgs e)
		{
			while (!bgwWaterfall.CancellationPending)
			{
				if (needRedrawWaterfall)
				{
					
						zgWaterfall.Invalidate();
						//lock (needRedrawWaterfallLock)
						{
							needRedrawWaterfall = false;
						}
				}

				/// Ждем слеующей перерисовки
				Thread.Sleep(500);
			}
		}

		public void RedrawWaterfallFull(BaseTrace pTrace)
		{
			if (toolBtnWaterfallWaterfall.Checked)
			{
				zgWaterfall.GraphPane.CurveList.Clear();

				int _i = 0;

				foreach (SignalTraceCycle _stc in pTrace.SignalCycles.ToList())
				{
					if (_stc == null)
						continue;
					((GradientLineItem)_stc.BarItem).DrawY = _i;
					_i++;

					zgWaterfall.GraphPane.CurveList.Add(_stc.BarItem);
				}

				RedrawWaterfall();
			}
		}

		public void RedrawWaterfall()
		{
			if (toolBtnWaterfallWaterfall.Checked)
			{
				//if (Monitor.TryEnter(needRedrawWaterfallLock))
				{
					lock (needRedrawWaterfallLock)
						needRedrawWaterfall = true;
				}
			}
		}
		#endregion

		private void lstReports_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ListViewHitTestInfo _info = lstReports.HitTest(e.Location);

			if (_info.Item == null || _info.SubItem == null)
				return;

			OLVListItem _item = _info.Item as OLVListItem;


			ReportItem _repItem = _item.RowObject as ReportItem;

			if (_repItem == null)
				return;

			if (File.Exists(_repItem.FullFileName))
			{
				System.Diagnostics.Process.Start(_repItem.FullFileName);
			}
		}

		private void splitZgViews_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (splitZgViews.Panel1.Height > splitZgViews.Panel2.Height)
			{
				splitZgViews.SplitterDistance = splitZgViews.Panel1MinSize;
			}
			else
			{
				splitZgViews.SplitterDistance = splitZgViews.Height - splitZgViews.Panel2MinSize;
			}
		}

		/// <summary>
		/// Удаление выбранных отчетов из списка
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnDeleteReports_Click(object sender, EventArgs e)
		{
			if (lstReports.SelectedItems.Count == 0)
				return;

			if (MessageBox.Show(Locale.confirm_delete_reports,
					Locale.confirmation,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2) == DialogResult.No)
				return;

			foreach (ListViewItem _item in lstReports.SelectedItems)
			{
				OLVListItem _listItem = (OLVListItem)_item;

				if (_listItem == null)
					continue;

				ReportItem _report = (ReportItem)_listItem.RowObject;

				if (_report == null)
					continue;

				try
				{

					ReportsManager.DeleteReport(_report);
				}
				catch
				{
					MessageBox.Show(String.Format(Locale.error_deleting_report, _report.FullFileName), 
						Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btnAbout_Click(object sender, EventArgs e)
		{
			

			ControlUtils.Splash.Splash.ShowSplashModal(250, KaorVersion.FullVersion);

			
		}

		private void reportsFilter_Click(object sender, EventArgs e)
		{
			lstReports.BuildList(true);
		}

		private void BaseRCSView_VisibleChanged(object sender, EventArgs e)
		{
			this.splitContainer1.SplitterDistance = this.splitContainer1.Width - 320;
		}

		/// <summary>
		/// Экспорт отчета в указанный файл
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnExportReport_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstReports.SelectedItem as OLVListItem;
			
			if (_item == null)
				return;

			ReportItem _report = _item.RowObject as ReportItem;
			
			if (_report == null)
				return;


			SaveFileDialog _dlg = new SaveFileDialog();

			switch (_report.TypeReport)
			{
				case ReportType.Audio:
				case ReportType.SignalAudio:
					_dlg.Filter = Locale.wav_filter;
					break;

				case ReportType.PDF:
					_dlg.Filter = Locale.pdf_filter;
					break;

				default:
					_dlg.Filter = Locale.unk_filter;
					break;
			}
			
			_dlg.FilterIndex = 0;
			_dlg.Title = Locale.export_report;
			_dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			
			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				/// Копирование файла отчета в выбранный файл
				/// 
				try
				{
					if (File.Exists(_dlg.FileName))
					{
						//if (MessageBox.Show(Locale.confirm_file_overwrite, Locale.confirmation,
						//	MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
						//{
							File.Delete(_dlg.FileName);
						//}
						//else
						//{
						//	return;
						//}
					}

					File.Copy(_report.FullFileName, _dlg.FileName);

					MessageBox.Show(String.Format(Locale.inf_report_saved, _dlg.FileName),
						Locale.information, MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
				}

				catch
				{
					MessageBox.Show(String.Format(Locale.err_save_report, _dlg.FileName),
						Locale.error, 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// Создание трасс по сигналам
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBtnCreateSignalTraces_Click(object sender, EventArgs e)
		{

			if (lstSignals.SelectedItems.Count == 0)
				return;

			if (MessageBox.Show(Locale.confirm_create_traces, Locale.confirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				List<BaseTrace> _addTraces = new List<BaseTrace>();

				foreach (ListViewItem _item in lstSignals.SelectedItems)
				{
					OLVListItem _signalItem = _item as OLVListItem;

					if (_signalItem == null)
						continue;

					BaseSignal _signal = _signalItem.RowObject as BaseSignal;

					if (_signal == null)
						continue;

					/// По-умолчанию создаем трассу из 100 точек

					BaseTrace _trace = new BaseTrace(_signal.Frequency - _signal.Band / 2,
						_signal.Frequency + _signal.Band / 2,
						_signal.Band / 100, TracePoint.POWER_UNDEFINED,
						Utils.ColorUtils.RandomColor());
#if false
					/// Выбор РПУ , на котором будет создвать трасса
					IRPU _rpu = rcs.RPUManager.UserSelectAvailableRPU();

					if (_rpu == null)
						continue;

					if (_rpu.PowerMeter == null || _rpu.HasPowerMeter == false)
					{
						MessageBox.Show("Выбранное РПУ не поддерживает режим измерения мощности");
						continue;
					}

					
					BaseTrace _trace = _rpu.PowerMeter.UserCreateNewTrace(_signal.Frequency - _signal.Band / 2,
						_signal.Frequency + _signal.Band / 2);
#endif

					if (_trace == null)
						continue;

					_addTraces.Add(_trace);
				}

				/// Добавление новых трасс к списку
				foreach (BaseTrace _trace in _addTraces)
				{
					rcs.AddScanTrace(_trace);
					_trace.OnTraceScanChanged += new TraceChanged(_trace_OnTraceChanged);

					/// Отрисовка трассы на контролах
					_trace.DrawZedGraph(zgMain, true);
					_trace.DrawZedGraph(zgSmall, false);
				}

				UpdateControl();
			}
		}

		private void toolStripReports_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void toolBtnEditMarker_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstMarkers.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseRadioMarker _marker = _item.RowObject as BaseRadioMarker;

			if (_marker == null)
				return;

			if (_marker.UserEditMarker())
			{

				/// Перерисовка маркера
				/// 
				lstMarkers.BuildList(true);
				evtRedraw.Set();
			}
		}

		private void toolBtnDeleteMarker_Click(object sender, EventArgs e)
		{
			if (lstMarkers.SelectedItems.Count == 0)
				return;

			if (MessageBox.Show(Locale.confirm_delete_selected_markers, 
				Locale.confirmation, MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			foreach (ListViewItem _i in lstMarkers.SelectedItems)
			{
				OLVListItem _item = _i as OLVListItem;

				if (_item == null)
					return;

				BaseRadioMarker _marker = _item.RowObject as BaseRadioMarker;

				if (_marker == null)
					return;


				rcs.Markers.Remove(_marker);
				_marker.IsVisible = false;
				_marker.DrawZedGraph(zgMain);
				_marker.DrawZedGraph(zgSmall);
			}

			lstMarkers.BuildList(false);

		}

		void ClearMarkersMain()
		{
			rcs.ClearMarkers();
		}

		private void toolBtnClearMarkers_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show(Locale.confirm_delete_all_markers, 
				Locale.confirmation, 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question) != DialogResult.Yes)
				return;
#if false
			foreach (BaseRadioMarker _m in rcs.Markers)
			{

				_m.IsVisible = false;
//				_m.DrawZedGraph(zgMain);
//				_m.DrawZedGraph(zgSmall);
			}

			rcs.Markers.Clear();
//			lstMarkers.BuildList(false);
			UpdateControl();
#else
			Application.DoEvents();

			new MethodInvoker(ClearMarkersMain).BeginInvoke(null, null);
#endif
		}

		private void toolBtnMarkerGo_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstMarkers.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseRadioMarker _marker = _item.RowObject as BaseRadioMarker;

			if (_marker == null)
				return;

			//if (ctrlPressed)
			{
				double _span = zgMain.GraphPane.XAxis.Scale.Max - zgMain.GraphPane.XAxis.Scale.Min;

				SetFreqSpan(zgMain.GraphPane.XAxis.Scale, _marker.Frequency, _span);
//				zgMain.GraphPane.XAxis.Scale.Min = _marker.Frequency - _span / 2.0;
//				zgMain.GraphPane.XAxis.Scale.Max = _marker.Frequency + _span / 2.0;

				zgMain.AxisChange();
				zgMain.Invalidate();
			}
		}

		private void toolBtnTraceGo_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstScanTraces.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseTrace _trace = _item.RowObject as BaseTrace;

			if (_trace == null)
				return;

//			if (ctrlPressed)
			{
				zgMain.GraphPane.XAxis.Scale.Min = _trace.Fstart - _trace.MeasureStep * 2;
				zgMain.GraphPane.XAxis.Scale.Max = _trace.Fstop + _trace.MeasureStep * 2;

				zgMain.AxisChange();
				zgMain.Invalidate();

				if (toolBtnWaterfallWaterfall.Checked)
				{
					zgWaterfall.GraphPane.XAxis.Scale.Min = _trace.Fstart - _trace.MeasureStep * 2;
					zgWaterfall.GraphPane.XAxis.Scale.Max = _trace.Fstop + _trace.MeasureStep * 2;

					zgWaterfall.AxisChange();
					zgWaterfall.Invalidate();
				}
			}
		}

		private void btnLoadRCSState_Click(object sender, EventArgs e)
		{
			rcs.LoadFullState();
		}

		private void btnSaveRCSState_Click(object sender, EventArgs e)
		{
			rcs.SaveFullState();
		}

		void StartZeroSpan()
		{
			if (!InvokeRequired)
			{
				/// Запуск режима ZeroSpan
				/// 

				//chkSpectrogramm.Enabled = false;
				//toolBtnWaterfallWaterfall.Enabled = true;
				chkBtnMode.Enabled = false;

				zeroSpanX = new double[zeroSpanPointCount];
				zeroSpanY = new double[zeroSpanPointCount];

				for (int _i = 0; _i < zeroSpanPointCount; _i++)
				{
					zeroSpanX[_i] = _i * 2;
					zeroSpanY[_i] = -125;
				}
				zeroSpanPointNo = 0;
#if false
				//SetupZeroSpanView();

				zgWaterfall.GraphPane.CurveList.Clear();
				zgWaterfall.GraphPane.GraphObjList.Clear();

				zgWaterfall.GraphPane.YAxis.IsVisible = true;
				zgWaterfall.GraphPane.YAxis.Title.IsVisible = true;

				zgWaterfall.GraphPane.XAxis.Scale.Min = 0;
				zgWaterfall.GraphPane.XAxis.Scale.Max = zeroSpanPointCount * 2;
				zgWaterfall.IsEnableHPan = false;
				zgWaterfall.IsEnableHZoom = false;
				zgWaterfall.GraphPane.YAxis.MajorGrid.IsVisible = true;
				zgWaterfall.GraphPane.YAxis.Scale.IsVisible = true;

				zgWaterfall.GraphPane.XAxis.Scale.IsVisible = false;
				zgWaterfall.GraphPane.XAxis.Scale.Min = 0;
				zgWaterfall.GraphPane.XAxis.Scale.Max = zeroSpanPointCount * 2.0;
				zgWaterfall.GraphPane.YAxis.Scale.Min = -120.0;
				zgWaterfall.GraphPane.YAxis.Scale.MinScrollRange = -140.0;
				zgWaterfall.GraphPane.YAxis.Scale.Max = -20.0;
				zgWaterfall.GraphPane.YAxis.Scale.MaxScrollRange = +30.0;
				zgWaterfall.IsEnableVPan = true;
#endif
				zeroSpanPoints = new PointPairList(zeroSpanX, zeroSpanY);
				zeroSpanLine = zgWaterfall.GraphPane.AddCurve("",
					zeroSpanPoints,
					Color.LimeGreen, SymbolType.None);

				zeroSpanLine.Line.StepType = StepType.ForwardStep;
				zeroSpanLine.Line.IsOptimizedDraw = true;
				zeroSpanLine.Line.IsAntiAlias = false;
				zeroSpanLine.Symbol.IsVisible = false;

				zgWaterfall.AxisChange();
				zgWaterfall.Invalidate();

				rcs.RPUManager.ManualRPU.PowerMeter.OnNewPowerMeasure += new NewPowerMeasure(PowerMeter_OnNewPowerMeasure);
				rcs.RPUManager.ManualRPU.PowerMeter.Start();
			}
			else
				Invoke(new MethodInvoker(StartZeroSpan));
			//bgwWaterfall.RunWorkerAsync(null);
		}

		void StopZeroSpan()
		{
			if (!InvokeRequired)
			{
				rcs.RPUManager.ManualRPU.PowerMeter.Stop();
				rcs.RPUManager.ManualRPU.PowerMeter.OnNewPowerMeasure -= PowerMeter_OnNewPowerMeasure;

				zgWaterfall.GraphPane.CurveList.Remove(zeroSpanLine);
				//bgwWaterfall.CancelAsync();

				//chkSpectrogramm.Enabled = true;

				//toolBtnWaterfallWaterfall.Enabled = false;

				chkBtnMode.Enabled = true;
			}
			else
				Invoke(new MethodInvoker(StopZeroSpan));
		}
#if false
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

			if (chkZeroSpan.Checked == true)
			{
				chkBtnMode.Enabled = false;
				chkZeroSpan.BackColor = Color.LightPink;

				new MethodInvoker(StartZeroSpan).BeginInvoke(null, null);
				
			}
			else
			{
				new MethodInvoker(StopZeroSpan).BeginInvoke(null, null);

				chkZeroSpan.BackColor = Color.FromName("ControlLight");
				chkBtnMode.Enabled = true;
			}
		}
#endif
		private void toolBtnEditTrace_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstScanTraces.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			BaseTrace _trace = _item.RowObject as BaseTrace;

			if (_trace == null)
				return;

			if (_trace.UserEditParams())
			{
				if (_trace.TraceControl != null)
					_trace.TraceControl.ReInitialize();

				_trace.DrawZedGraph(zgMain, true);
				_trace.DrawZedGraph(zgSmall, false);

				lock (needRedrawLock)
					needRedraw = true;

				lstScanTraces.BuildList(true);
			}
			else
			{
			}
		}

		private void lstMarkers_SelectedIndexChanged(object sender, EventArgs e)
		{
			OLVListItem _item = lstMarkers.SelectedItem as OLVListItem;

			if (_item == null)
			{
				toolBtnMarkerGo.Enabled = false;
				toolBtnEditMarker.Enabled = false;
			}
			else
			{
				toolBtnMarkerGo.Enabled = true;
				toolBtnEditMarker.Enabled = true;
			}

			if (lstMarkers.SelectedItems.Count == 0)
			{
				toolBtnDeleteMarker.Enabled = false;
			}
			else
			{
				toolBtnDeleteMarker.Enabled = true;
				
				// Этот кусок кода нужен для отображения выделенного маркера на 
				// zedGraph. Но пока он глючит :)
				zgMain.Selection.ClearSelection(zgMain.MasterPane);
				
				foreach (ListViewItem _i in lstMarkers.SelectedItems)
				{
				    OLVListItem _olvItem = _i as OLVListItem;
				    if (_olvItem == null)
				        continue;

				    BaseRadioMarker _m = _olvItem.RowObject as BaseRadioMarker;
					
				    if (_m == null)
				        continue;

					zgMain.Selection.AddToSelection(zgMain.MasterPane, _m.LineItem);
				}

				zgMain.Invalidate();
			}
		}

		private void lstSignals_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (rcs.OperationMode != ERadioControlMode.Idle || 
				lstSignals.SelectedItems.Count == 0)
			{
				toolBtnEditSignal.Enabled = false;
				toolBtnDeleteSignal.Enabled = false;
				toolBtnCreateSignalTraces.Enabled = false;
			}
			else if(lstSignals.SelectedItems.Count == 1)
			{
				OLVListItem _item = lstSignals.SelectedItem as OLVListItem;

				BaseSignal _signal = _item.RowObject as BaseSignal;

				if (_signal != null)
				{
					toolBtnCreateSignalTraces.Enabled = _signal.CanCreateTrace;
				}
				
				toolBtnEditSignal.Enabled = true;
				toolBtnDeleteSignal.Enabled = true;
			}
		}

		private void toolBtnSpanBegin_Click(object sender, EventArgs e)
		{
			freqInputDialog.Text = Locale.start_frequency;

			freqInputDialog.FreqMin = 0;// (long)zgMain.GraphPane.XAxis.Scale.Min + 100;
			freqInputDialog.FreqMax = _rcsFMax;

			if (freqInputDialog.ShowDialog() == DialogResult.OK)
				SetStartFreq(freqInputDialog.InputFrequency);
		}

		private void toolBtnSpanCenter_Click(object sender, EventArgs e)
		{
			freqInputDialog.Text = Locale.center_frequency;

			freqInputDialog.FreqMin = 0;
			freqInputDialog.FreqMax = _rcsFMax;

			if (freqInputDialog.ShowDialog() == DialogResult.OK)
				SetCenterFreq(freqInputDialog.InputFrequency);
		}

		private void toolBtnSpanEnd_Click(object sender, EventArgs e)
		{
			freqInputDialog.Text = Locale.stop_frequency;

			freqInputDialog.FreqMin = 0;
			freqInputDialog.FreqMax = _rcsFMax;// (long)zgMain.GraphPane.XAxis.Scale.Max - 100;

			if (freqInputDialog.ShowDialog() == DialogResult.OK)
				SetStopFreq(freqInputDialog.InputFrequency);
		}

		private void toolBtnSpan_Click(object sender, EventArgs e)
		{
			freqInputDialog.Text = Locale.span;

			freqInputDialog.FreqMin = 1000;
			freqInputDialog.FreqMax = _rcsFMax;

			if (freqInputDialog.ShowDialog() == DialogResult.OK)
				SetSpan(freqInputDialog.InputFrequency);
		}

		private void toolBtnMove_Click(object sender, EventArgs e)
		{
			
		}

		private void toolBtnZoom_Click(object sender, EventArgs e)
		{
			if (toolBtnZoom.Checked)
			{
				toolBtnMove.Checked = false;

				zgMain.ZoomButtons = MouseButtons.Left;
				zgMain.ZoomModifierKeys = Keys.None;
				zgMain.Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.zoom.cur"));
			}
			else
			{
				zgMain.ZoomButtons = MouseButtons.None;
				zgMain.ZoomModifierKeys = Keys.None;
				zgMain.Cursor = Cursors.Cross;
			}
		}

		private void toolBtnUnZoom_Click(object sender, EventArgs e)
		{
			zgMain.ZoomOut(zgMain.GraphPane);
		}

		private void toolBtnUnZoomAll_Click(object sender, EventArgs e)
		{
			zgMain.ZoomOutAll(zgMain.GraphPane);
		}

		private void toolBtnMove_CheckedChanged(object sender, EventArgs e)
		{
			if (toolBtnMove.Checked)
			{
				toolBtnZoom.Checked = false;

				zgMain.PanButtons = MouseButtons.Left;
				zgMain.PanModifierKeys = Keys.None;
				zgMain.Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.pan.cur"));
			}
			else
			{
				zgMain.PanButtons = MouseButtons.None;
				zgMain.PanModifierKeys = Keys.None;
				zgMain.Cursor = Cursors.Cross;
			}
		}

		private void toolBtnPrint_Click(object sender, EventArgs e)
		{
			zgMain.DoPrintPreview();
		}

		private void toolBtnPageSetup_Click(object sender, EventArgs e)
		{
			zgMain.DoPageSetup();
		}

		private void toolBtnCopyClipboard_Click(object sender, EventArgs e)
		{
			zgMain.Copy(true);
		}

		private void toolBtnWaterfallPan_CheckedChanged(object sender, EventArgs e)
		{
			if (toolBtnWaterfallPan.Checked)
			{
				toolBtnWaterfallZoom.Checked = false;

				zgWaterfall.PanButtons = MouseButtons.Left;
				zgWaterfall.PanModifierKeys = Keys.None;
				zgWaterfall.Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.pan.cur"));
			}
			else
			{
				zgWaterfall.PanButtons = MouseButtons.None;
				zgWaterfall.PanModifierKeys = Keys.None;
				zgWaterfall.Cursor = Cursors.Cross;
			}
		}

		private void toolBtnWaterfallZoom_CheckedChanged(object sender, EventArgs e)
		{
			if (toolBtnWaterfallZoom.Checked)
			{
				toolBtnWaterfallPan.Checked = false;

				zgWaterfall.ZoomButtons = MouseButtons.Left;
				zgWaterfall.ZoomModifierKeys = Keys.None;
				zgWaterfall.Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("KaorCore.zoom.cur"));
			}
			else
			{

				zgWaterfall.ZoomButtons = MouseButtons.None;
				zgWaterfall.ZoomModifierKeys = Keys.None;
				zgWaterfall.Cursor = Cursors.Cross;
			}
		}

		private void toolBtnWaterfallUnZoom_Click(object sender, EventArgs e)
		{
			zgWaterfall.ZoomOut(zgWaterfall.GraphPane);
		}

		private void toolBtnWaterfallUnzoomAll_Click(object sender, EventArgs e)
		{
			zgWaterfall.ZoomOutAll(zgWaterfall.GraphPane);
		}

		private void toolBtnWaterfallPrint_Click(object sender, EventArgs e)
		{
			zgWaterfall.DoPrintPreview();
		}

		private void toolBtnWaterfallPageSetup_Click(object sender, EventArgs e)
		{
			zgWaterfall.DoPageSetup();
		}

		private void toolBtnWaterfallCopy_Click(object sender, EventArgs e)
		{
			zgWaterfall.Copy(true);
		}

		private void zgWaterfall_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{
			if (zgWaterfall.GraphPane.ZoomStack.Count == 0)
			{
				toolBtnWaterfallUnZoom.Enabled = false;
				toolBtnWaterfallUnzoomAll.Enabled = false;
			}
			else
			{
				toolBtnWaterfallUnZoom.Enabled = true;
				toolBtnWaterfallUnzoomAll.Enabled = true;
			}
		}

		private void SetFreqSpan(Scale pScale, double pFreq, double pSpan)
		{
			if (pFreq < pScale.MinScrollRange || pFreq > pScale.MaxScrollRange)
				return;

			double _tmpMin, _tmpMax, _tmpHalfSpan;

			_tmpHalfSpan = pSpan / 2.0;
			_tmpMin = pFreq - _tmpHalfSpan;
			_tmpMax = pFreq + _tmpHalfSpan;

			if (_tmpMin < pScale.MinScrollRange)
			{
				_tmpHalfSpan = (pFreq - pScale.MinScrollRange);
			}

			if (_tmpMax > pScale.MaxScrollRange)
			{
				_tmpHalfSpan = (pScale.MaxScrollRange - pFreq);
			}

			pScale.Min = pFreq - _tmpHalfSpan;
			pScale.Max = pFreq + _tmpHalfSpan;
		}

		private void toolBtnTraceMarkers_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstScanTraces.SelectedItem as OLVListItem;

			if (_item == null)
				return;

			CreateTraceMarkersDialog _dlg = new CreateTraceMarkersDialog();

			_dlg.Trace = _item.RowObject as BaseTrace;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				List<BaseRadioMarker> _newMarkers = _dlg.Markers;

				foreach (BaseRadioMarker _marker in _newMarkers)
					rcs.AddMarker(_marker);

				UpdateControl();
			}
		}
#if false
		private void chkSpectrogramm_CheckedChanged(object sender, EventArgs e)
		{
			if (chkSpectrogramm.Checked)
			{
				/// Подготовка zgWaterfall к отображения спектрограммы
				/// 
				chkZeroSpan.Enabled = false;

				SetupSpectrogramView();
			}
			else
			{
				if(!chkBtnMode.Checked)
					chkZeroSpan.Enabled = true;
			}
		}
#endif
		private void toolBtnScanParams_Click(object sender, EventArgs e)
		{
			OLVListItem _listItem = (OLVListItem)lstScanTraces.SelectedItem;

			if (_listItem == null)
				return;

			BaseTrace _trace = (BaseTrace)_listItem.RowObject;

			rcs.SetupTraceScan(_trace);

			lstScanTraces.BuildList(true);
		}

		private void toolBtnEditSignal_Click(object sender, EventArgs e)
		{
			OLVListItem _item = lstSignals.SelectedItem as OLVListItem;

			BaseSignal _signal = (BaseSignal)_item.RowObject;
			if (_signal != null)
			{
				ShowSignalChart(_signal, true);
			}
			lstSignals.RefreshItem(_item);
		}

		private void toolBtnReplayTrace_Click(object sender, EventArgs e)
		{
			if (!bgwDrawCycles.IsBusy)
			{
				/// Запуск проигрывания трасс
				OLVListItem _item = (OLVListItem)lstScanTraces.SelectedItem;

				if (_item == null)
					return;

				BaseTrace _trace = (BaseTrace)_item.RowObject;

				if (_trace == null)
					return;


				SetupReplayView(_trace.Fstart - _trace.MeasureStep * 2, _trace.Fstop + _trace.MeasureStep * 2);


				toolBtnReplayTrace.Image = imageList1.Images["stop.png"];

				bgwDrawCycles.RunWorkerAsync(_trace);
			}
			else
			{
				/// Останов проигрывателя сигнала
				bgwDrawCycles.CancelAsync();
				toolBtnReplayTrace.Image = imageList1.Images["play.png"];
			}
		}

		public delegate void QuitClickDelegate();
		public event QuitClickDelegate OnQuitClick;

		private void btnQuit_Click(object sender, EventArgs e)
		{
			if(OnQuitClick != null)
				OnQuitClick();
		}

		private void ctrlRadioConfig_OnSaveButtonClick()
		{
			string _fn = Application.StartupPath + "\\settings.xml";
			XmlTextWriter _writer = null;

			try
			{
				rcs.CheckConfiguration();
			}

			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (MessageBox.Show(Locale.confirm_save_system_settings,
				Locale.confirmation, 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				try
				{
					if (File.Exists(_fn))
					{
						if (File.Exists(_fn + ".bak"))
							File.Delete(_fn + ".bak");

						File.Copy(_fn, _fn + ".bak");
					}

					_writer = new XmlTextWriter(_fn, Encoding.UTF8);
					_writer.Formatting = Formatting.Indented;

					rcs.SaveToXmlWriter(_writer);

					_writer.Close();

					MessageBox.Show(String.Format(Locale.inf_settings_saved, _fn, _fn + ".bak"),
						Locale.information,
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show(String.Format(Locale.err_saving_settings, _fn),
						Locale.error,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (_writer != null)
						_writer.Close();
				}
			}

#if false
			SaveFileDialog _dlg = new SaveFileDialog();

			_dlg.Filter = "Настройки КАОР (*.xml) | *.xml";

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if (File.Exists(_dlg.FileName))
					{
						File.Delete(_dlg.FileName + ".bak");
						File.Copy(_dlg.FileName, _dlg.FileName + ".bak");
					}

					XmlTextWriter _writer = new XmlTextWriter(_dlg.FileName, Encoding.UTF8);
					_writer.Formatting = Formatting.Indented;

					rcs.SaveToXmlWriter(_writer);

					_writer.Close();

					MessageBox.Show("Настройки каор сохранены в файл \"" + _dlg.FileName + "\"",
						"Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Ошибка сохранения настроек КАОР в файл \"" + _dlg.FileName + "\"",
						"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}
#endif
		}

		public void OpenConfigPanel()
		{
			xPanderPanelList1.Expand(xPanelConfig);
		}

		private void toolBtnWaterfallWaterfall_Click(object sender, EventArgs e)
		{
			if (toolBtnWaterfallWaterfall.Checked)
			{
				SetupSpectrogramView();
			}
			else
			{
				SetupReplayView(zgMain.GraphPane.XAxis.Scale.Min,
					zgMain.GraphPane.XAxis.Scale.Max);
			}
		}

		private void toolBtnWaterfallZeroSpan_Click(object sender, EventArgs e)
		{
			if (toolBtnWaterfallZeroSpan.Checked)
			{
				SetupZeroSpanView();

				StartZeroSpan();
				//new MethodInvoker(StartZeroSpan).BeginInvoke(null, null);

			}
			else
			{
				StopZeroSpan();
				//new MethodInvoker(StopZeroSpan).BeginInvoke(null, null);
				SetupReplayView(zgMain.GraphPane.XAxis.Scale.Min,
					zgMain.GraphPane.XAxis.Scale.Max);
			}
		}

		void AddSingleFreqSignal(long pFreq, double pPower)
		{
			SingleFreqSignal _signal = new SingleFreqSignal();
			_signal.Frequency = pFreq;
			_signal.Power = pPower;

			rcs.AddSignal(_signal);

			_signal.IsVisible = true;

			ShowSignalChart(_signal, true);

			UpdateSignals();
		}

		private void singleFrequencySinalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			long _freq;
			double _power;

			_freq = (long)((zgMain.GraphPane.XAxis.Scale.Max +
				zgMain.GraphPane.XAxis.Scale.Min) / 2000.0) * 1000;
			_power = 0.0;

			AddSingleFreqSignal(_freq, _power);
			
		}
	}
}
