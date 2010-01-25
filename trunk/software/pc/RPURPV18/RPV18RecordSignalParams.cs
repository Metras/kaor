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
using System.Xml.Serialization;

using KaorCore.Antenna;
using KaorCore.RPU;

namespace RPURPV18
{
	/// <summary>
	/// Параметры записи сигнала РПВ-18
	/// </summary>
	/// 
	[Serializable]
	public class RPV18RecordSignalParams
	{
		#region ================ Поля ================
		
		int filterBand;
		EAudioModulationType modulation;
		string recordPath;
		bool needSave;

		
		Guid antennaGuid;

		#endregion

		#region ================ Конструктор ================
		public RPV18RecordSignalParams()
		{
			filterBand = 230000;
			recordPath = "";
			modulation = EAudioModulationType.FM;
			needSave = false;
		}
		#endregion

		#region ================ Проперти ================
		public int FilterBand
		{
			get { return filterBand; }
			set { filterBand = value; }
		}

		public EAudioModulationType Modulation
		{
			get { return modulation; }
			set { modulation = value; }
		}

		public string RecordPath
		{
			get { return recordPath; }
			set { recordPath = value; }
		}

		public bool NeedSave
		{
			get { return needSave; }
			set { needSave = value; }
		}

		[XmlIgnore]
		public IAntenna Antenna
		{
			get 
			{
				return BaseAntenna.GetAntennaByGuid(antennaGuid); 
			}
			set 
			{
				if (value != null)
					antennaGuid = value.Id;
				else
					antennaGuid = Guid.Empty;
			}
		}

		public Guid AntennaGuid
		{
			get 
			{
				return antennaGuid;
			}

			set
			{
				antennaGuid = value;
			}
		}
		#endregion

		#region ================ Методы ================
		#endregion

	}
}
