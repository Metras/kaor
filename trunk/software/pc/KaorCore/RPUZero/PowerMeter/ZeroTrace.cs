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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.Task;
using KaorCore.Trace;


#if false
namespace RPUZero.PowerMeter
{
	public class ZeroTrace : BaseTrace
	{
		#region =============== Поля ===============
		TraceTaskProvider taskProvider;

		#endregion

		public ZeroTrace(Int64 pFstart, Int64 pFstop, int pFilterBand,
			int pMeasureStep, int pAverageTime, IRPU pRPU, IAntenna pAntenna)
			: base(pFstart, pFstop, pMeasureStep, TracePoint.POWER_UNDEFINED, System.Drawing.Color.LimeGreen)
		{
		}

		/// <summary>
		/// Конструктор из Xml
		/// </summary>
		/// <param name="pXmlReader"></param>
		public ZeroTrace(XmlNode pNode)
			: base(pNode)
		{

		}

		/// <summary>
		/// Получение провайдера задач для трассы
		/// </summary>
		public override ITaskProvider TaskProvider
		{
			get 
			{
				if (taskProvider == null)
				{
					taskProvider = new TraceTaskProvider(this);
					taskProvider.OnNewTaskCycle += new NewTaskCycleDelegate(taskProvider_OnNewTaskCycle);
				}

				return taskProvider;
			}
		}

		void taskProvider_OnNewTaskCycle(ITaskProvider pTaskProvider, Guid pTaskCycleGuid)
		{
			cycleGuid = pTaskCycleGuid;
			CallOnNewTraceCycle(pTaskCycleGuid);
		}

		/// <summary>
		/// Сохранение трассы в файл
		/// </summary>
		/// <param name="pFileName"></param>
		public override void SaveToXml(XmlWriter pXmlWriter)
		{
			try
			{
				pXmlWriter.WriteStartElement("KaorTrace");

				/// Версия и тип трассы
				pXmlWriter.WriteAttributeString("version", "1.0");
				pXmlWriter.WriteAttributeString("type", this.GetType().AssemblyQualifiedName);
				//pXmlWriter.WriteAttributeString("assembly", this.GetType().);

				/// Индивидуальные параметры ZeroTrace
				/// 

				base.SaveToXml(pXmlWriter);

				pXmlWriter.WriteEndElement(); /// KaorTrace
			}

			catch
			{
				throw;
			}
		}
#if false
		public override bool UserFillParamsFromDialog()
		{
			NewTraceDialog _dlg = new NewTraceDialog();
			bool _res = false;

			_dlg.Antennas = (rpu as CRPUZero).Antennas;

			if (_dlg.ShowDialog() == DialogResult.OK)
			{
				/// Шаг перестройки РАВЕН полосе фильтра измерителя мощности
				/// 

				fstart = _dlg.FStart;
				fstop = _dlg.FStop;
				filterBand = _dlg.FilterBand;
				measureStep = _dlg.FilterBand;
				averageTime = _dlg.AverageTime;
				antenna = _dlg.SelectedAntenna;

				PrepareTracePoints();
				name = _dlg.txtTraceName.Text;
				description = _dlg.txtTraceDescr.Text;
				lineItem.Color = _dlg.cmbTraceColor.Value;

				_res = true;
			}

			return _res;
		}
#endif
	}
}

#endif