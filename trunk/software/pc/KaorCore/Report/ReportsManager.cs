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
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using KaorCore.Utils;
using KaorCore.I18N;

namespace KaorCore.Report
{
	public enum ReportType
	{
		PDF,
		Audio,
		SignalAudio
	}

	public class ReportItem : IComparable
	{
		string reportsRoot;

		DateTime timestamp;

		public DateTime Timestamp
		{
			get { return timestamp; }
			set 
			{ 
				timestamp = value;
				CallOnReportItemChanged();
			}
		}
		string name;

		public string Name
		{
			get { return name; }
			set 
			{ 
				name = value;
				CallOnReportItemChanged();
			}
		}
		string filename;

		public string Filename
		{
			get { return filename; }
			set 
			{ 
				filename = value;
				CallOnReportItemChanged();
			}
		}

		public string FullFileName
		{
			get { return ReportPath + filename + reportExt; }
		}

		string reportPath;

		public string ReportPath
		{
			get { return reportPath; }
			set 
			{ 
				reportPath = value;
				CallOnReportItemChanged();
			}
		}

		string reportExt;

		public string ReportExt
		{
			get { return reportExt; }
			set 
			{ 
				reportExt = value;
				CallOnReportItemChanged();
			}
		}

		ReportType typeReport;

		public ReportType TypeReport
		{
			get { return typeReport; }
			set 
			{ 
				typeReport = value;
				CallOnReportItemChanged();
			}
		}

		ReportItem()
		{
			reportsRoot = AppDomain.CurrentDomain.BaseDirectory;
			reportPath = reportsRoot + @"\reports\unknown\";
			reportExt = ".unk";
		}

		public ReportItem(ReportType pType, string pName)
			: this()
		{
			filename = Guid.NewGuid().ToString();

			timestamp = DateTime.Now;
			name = pName;

			typeReport = pType;

			switch(typeReport)
			{

				case ReportType.Audio:
				case ReportType.SignalAudio:
					reportPath = reportsRoot + @"\reports\audio\";
					reportExt = ".wav";
					break;

				case ReportType.PDF:
					reportPath = reportsRoot + @"\reports\pdf\";
					reportExt = ".pdf";
					break;

			}
		}
#if DEBUG
		public ReportItem(ReportType pType, string pName, DateTime pTimestamp)
			: this(pType, pName)
		{
			timestamp = pTimestamp;
		}
#endif
		public void CallOnReportItemChanged()
		{
			if (OnReportItemChanged != null)
				OnReportItemChanged(this);
		}


		public delegate void ReportItemChangedDelegate(ReportItem pItem);
		public event ReportItemChangedDelegate OnReportItemChanged;

		#region IComparable Members

		/// <summary>
		/// Сравнение двух отчетов производится по дате
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj)
		{
			ReportItem _item = (ReportItem)obj;

			if(_item == null)
				throw new ArgumentException("obj == null!");

			if (this.timestamp < _item.timestamp)
				return -1;
			else if (this.timestamp > _item.timestamp)
				return 1;
			else
				return 0;
		}

		#endregion
	}

	/// <summary>
	/// Менеджер отчетов
	/// </summary>
	public class ReportsManager
	{
		#region =============== Поля ===============
		static object saveLockObject;
		static string reportsRoot;
		#endregion

		#region =============== Поля ===============

		static List<ReportItem> reports;

		/// <summary>
		/// Статический конструктор менеджера отчетов
		/// </summary>
		static ReportsManager()
		{
			

			reportsRoot = AppDomain.CurrentDomain.BaseDirectory;

			try
			{
				saveLockObject = new object();
				reports = new List<ReportItem>();

				if (!Directory.Exists(reportsRoot + @"\reports"))
					Directory.CreateDirectory(reportsRoot + @"\reports");

				if (!Directory.Exists(reportsRoot + @"\reports\audio"))
					Directory.CreateDirectory(reportsRoot + @"\reports\audio");

				if (!Directory.Exists(reportsRoot + @"\reports\pdf"))
					Directory.CreateDirectory(reportsRoot + @"\reports\pdf");

				if (!Directory.Exists(reportsRoot + @"\reports\unknown"))
					Directory.CreateDirectory(reportsRoot + @"\reports\unknown");

				XmlDocument _doc = new XmlDocument();
				_doc.Load(reportsRoot + @"\reports\index.xml");

				/// Десериализация списка отчетов
				/// 
				XmlSerializer _reportSerializer = new XmlSerializer(typeof(ReportItem));
				XmlNode _reportsNode = _doc.SelectSingleNode("reports");

				if (_reportsNode != null)
				{
					foreach (XmlNode _node in _reportsNode)
					{
						ReportItem _repItem = (ReportItem)_reportSerializer.Deserialize(new XmlNodeReader(_node));

						if (File.Exists(_repItem.FullFileName))
						{
							reports.Add(_repItem);
							_repItem.OnReportItemChanged += new ReportItem.ReportItemChangedDelegate(_repItem_OnReportItemChanged);
						}
					}
				}
			}
			catch
			{
			}

#if DEBUG
			if(reports.Count == 0)
				CreateTestReports();
#endif
			reports.Sort();

		}

		/// <summary>
		/// Обработчик события изменения параметров отчета
		/// </summary>
		/// <param name="pItem"></param>
		static void _repItem_OnReportItemChanged(ReportItem pItem)
		{
			new ReportsSaveDelegate(SaveReports).Invoke();
		}

		~ReportsManager()
		{
		}

		#endregion

		#region =============== Проперти ===============
		static public List<ReportItem> Reports
		{
			get { return reports; }
		}
		#endregion

		#region =============== Методы ===============
#if DEBUG
		static void CreateTestReports()
		{
			reports.Add(new ReportItem(ReportType.Audio, Locale.report_test_record));
			reports.Add(new ReportItem(ReportType.PDF, Locale.report_test_report));

			reports.Add(new ReportItem(ReportType.Audio, Locale.report_test_record, DateTime.Now.AddDays(-1)));
			reports.Add(new ReportItem(ReportType.PDF, Locale.report_test_report, DateTime.Now.AddDays(-1)));

			reports.Add(new ReportItem(ReportType.Audio, Locale.report_test_record, DateTime.Now.AddDays(-5)));
			reports.Add(new ReportItem(ReportType.PDF, Locale.report_test_report, DateTime.Now.AddDays(-15)));

			foreach (ReportItem _item in reports)
			{
				_item.OnReportItemChanged += _repItem_OnReportItemChanged;
			}

		}
#endif
		/// <summary>
		/// Создание нового отчета
		/// </summary>
		/// <param name="pType"></param>
		/// <returns></returns>
		public static string NewReportName(ReportType pType, string pName)
		{
			return NewReport(pType, pName).FullFileName;
		}

		public static ReportItem NewReport(ReportType pType, string pName)
		{
			ReportItem _item = new ReportItem(pType, pName);

			reports.Add(_item);
			reports.Sort();

			/// Запуск сохранения индекса отчетов
			/// 
			new ReportsSaveDelegate(SaveReports).BeginInvoke(null, null);

			if (OnReportsChanged != null)
				OnReportsChanged();

			return _item;
		}

		static public void DeleteReport(ReportItem pItem)
		{
			if (pItem == null)
				return;

			if (File.Exists(pItem.FullFileName))
				File.Delete(pItem.FullFileName);

			reports.Remove(pItem);
			reports.Sort();

			pItem.OnReportItemChanged -= _repItem_OnReportItemChanged;

			new ReportsSaveDelegate(SaveReports).Invoke();

			if (OnReportsChanged != null)
				OnReportsChanged();
		}

		
		static void SaveReports()
		{
			lock (saveLockObject)
			{
				if (File.Exists(reportsRoot + @"\reports\index.xml.bak"))
					File.Delete(reportsRoot + @"\reports\index.xml.bak");

				if (File.Exists(reportsRoot + @"\reports\index.xml"))
					File.Move(reportsRoot + @"\reports\index.xml", reportsRoot + @"\reports\index.xml.bak");

				XmlTextWriter _xmlWriter = new XmlTextWriter(reportsRoot + @"\reports\index.xml", Encoding.Unicode);
				_xmlWriter.Formatting = Formatting.Indented;
				XmlSerializer _reportSerializer = new XmlSerializer(typeof(ReportItem));
				List<ReportItem> _tmpList = new List<ReportItem>(reports);

				_xmlWriter.WriteStartElement("reports");

				foreach(ReportItem _item in _tmpList)
				{
					_reportSerializer.Serialize(_xmlWriter, _item);
				}

				_xmlWriter.WriteEndElement();
				_xmlWriter.Flush();
				_xmlWriter.Close();
			}
		}

		public static event ReportsChangedDelegate OnReportsChanged;

		public delegate void ReportsChangedDelegate();

		public delegate void ReportsSaveDelegate();

		#endregion

	}
}
