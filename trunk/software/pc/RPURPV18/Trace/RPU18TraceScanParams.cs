// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//			Maxim Anisenkov <anisenkov@niistt.ru>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KaorCore.Trace;
using KaorCore.RPU;
using RPURPV18.SignalConverter;
using RPURPV18.SignalConverterManager;
using RPURPV18;

namespace RPURPV18.Trace
{
	/// <summary>
	/// Расширение параметров сканирования трассы для РПВ-18
	/// </summary>
	public class RPU18TraceScanParams : TraceScanParams
	{
		private ISignalConverter signalConv = null;

		[XmlIgnore]
		public ISignalConverter SignalConverter
		{
			get
			{
				return signalConv;
			}
			set
			{
				if (value == null)
					signalConv = null;
				else
				{
					if (BaseSignalConverterManager.Converters.ContainsKey(value.ID))
						signalConv = BaseSignalConverterManager.Converters[value.ID];
					else
						signalConv = null;
				}
			}
		}

		public Guid SignalConverterID
		{
			get
			{
				if (SignalConverter == null)
					return Guid.Empty;
				else
					return SignalConverter.ID;
			}
			set
			{
				if (value == Guid.Empty || !BaseSignalConverterManager.Converters.ContainsKey(value))
					SignalConverter = null;
				else
					SignalConverter = BaseSignalConverterManager.Converters[value];
			}
		}

		[XmlIgnore]
		public CRPURPV18 RPU18
		{
			get { return ((CRPURPV18)RPU); }
		}

		public RPU18TraceScanParams()
		{
		}

		public RPU18TraceScanParams(IRPU pRPU) : base(pRPU)
		{
		}
	}
}
