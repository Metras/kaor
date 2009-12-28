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
using System.Linq;
using System.Text;
using System.Xml;

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.TraceControl;

namespace KaorCore.Signal
{
	[Serializable]
	public class RangeSignalHitPoint
	{
		#region ============== Поля ==============
		Guid id;
		DateTime timestamp;
		Int64 frequency;
		int filterBand;
		int averageTime;
/*		IAntenna antenna;
		IRPU rpu;
		BaseTraceControl traceControl;*/

		string antennaName;
		string rpuName;
		string traceControlName;
		double power;
		double delta;
		string recordName;
		bool hasRecord;
		#endregion

		#region ============== Конструктор ==============

		public RangeSignalHitPoint()
		{
			id = Guid.NewGuid();
			timestamp = DateTime.Now;
		}

		public RangeSignalHitPoint(Int64 pFrequency, int pFilterBand, int pAverageTime,
			IAntenna pAntenna, IRPU pRPU, BaseTraceControl pControl,
			double pPower, double pDelta, string pRecordName, bool pHasRecord)
			: this()
		{
			frequency = pFrequency;
			filterBand = pFilterBand;
			averageTime = pAverageTime;
			power = pPower;
			delta = pDelta;
			recordName = pRecordName;

			antennaName = pAntenna.Name;
			rpuName = pRPU.Name;
			traceControlName = pControl.Name;
		}
		#endregion


		#region ============== Проперти ==============
		public Guid Id
		{
			get { return id; }
			set { id = value; }
		}

		public DateTime Timestamp 
		{
			get { return timestamp; }
			set { timestamp = value; }
		}

		public Int64 Frequency
		{
			get { return frequency; }
			set { frequency = value; }
		}

		public int FilterBand
		{
			get { return filterBand; }
			set { filterBand = value; }
		}

		public int AverageTime
		{
			get { return averageTime; }
			set { averageTime = value; }
		}

		public string AntennaName
		{
			get { return antennaName; }
			set { antennaName = value; }
		}

		public string RPUName
		{
			get { return rpuName; }
			set { rpuName = value; }
		}

		public string TraceControlName
		{
			get { return traceControlName; }
			set { traceControlName = value; }
		}

		public double Power
		{
			get { return power; }
			set { power = value; }
		}

		public double Delta
		{
			get { return delta; }
			set { delta = value; }
		}

		public string RecordName
		{
			get { return recordName; }
			set { recordName = value; }
		}

		public bool HasRecord
		{
			get { return hasRecord; }
			set { hasRecord = value; }
		}
		#endregion


		#region ============== Методы ==============

		#endregion

	}
}
