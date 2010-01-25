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
using RPURPV18.SignalConverterManager;

namespace RPURPV18.SignalConverter
{
	public class DirectSignalConverter : BaseSignalConverter
	{
		public DirectSignalConverter()
			: base(new ConvertBand[] 
			{
				new ConvertBand(20000000, 3000000000, 20000000, 3000000000, 0)	
			})
		{
		}

		public override string Name
		{
			get
			{
				return I18N.Locale.direct_signal_converter_name;
			}
		}

		public override long ConvertFreq(Injector inj, long f)
		{
			if (CheckMinFreq(f) && CheckMaxFreq(f))
				return f;
			else
				throw new Exception();
		}

		public override Guid ID
		{
			get
			{
				return TypeID;
			}
		}

		/// <summary>
		/// Возвращает GUID класса, соответствующего отсутствию конвертера частоты
		/// </summary>
		public static Guid TypeID
		{
			get
			{
				return new Guid("{E380C5EE-6E5F-4a08-B28F-D549E5D35EE5}");
			}
		}
	}
}
