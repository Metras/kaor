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
	public abstract class BaseSignalConverter : ISignalConverter
	{
		private int selectedBandIndex = -1;

		public void Reset(Injector inj)
		{
			lock (this)
			{
				//inj.PowerOff();
				//inj.PowerOn();
				selectedBandIndex = -1;
			}
		}

		public ConvertBand[] ConvertBands { get; private set; }

		public BaseSignalConverter(ConvertBand[] bands)
		{
			ConvertBands = bands;
		}

		public abstract string Name { get; }

		public abstract Guid ID { get; }

		public virtual long ConvertFreq(Injector inj, long f)
		{
			lock (this)
			{
				if (CheckMinFreq(f) && CheckMaxFreq(f))
				{
					// Поиск нужного диапазона
					for (int i = 0; i < ConvertBands.Length; i++)
					{
						if (f >= ConvertBands[i].RF_Min && f <= ConvertBands[i].RF_Max && selectedBandIndex != i)
						{
							inj.SetFrequency(f);
							selectedBandIndex = i;
						}
					}
					if (selectedBandIndex != -1)
						return Math.Abs(ConvertBands[selectedBandIndex].LO - f);
					throw new Exception();
				}
				else
					throw new Exception();
			}
		}

		public virtual bool CheckMinFreq(long f)
		{
			return f >= MinFreq;
		}

		public virtual bool CheckMaxFreq(long f)
		{
			return f <= MaxFreq;
		}

		public override string ToString()
		{
			return Name;
		}

		public long MinFreq
		{
			get { return ConvertBands[0].RF_Min; }
		}

		public long MaxFreq
		{
			get { return ConvertBands[ConvertBands.Length - 1].RF_Max; }
		}
	}
}
