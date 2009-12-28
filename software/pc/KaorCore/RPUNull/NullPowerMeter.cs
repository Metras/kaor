﻿// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
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
using System.Windows.Forms;

using KaorCore.I18N;
using KaorCore.RPU;

namespace KaorCore.RPUNull
{
	public class NullPowerMeter : IPowerMeter
	{
		#region IPowerMeter Members

		public int FilterBand
		{
			get
			{
				return 0;
			}
			set
			{
				
			}
		}

		public int AverageTime
		{
			get
			{
				return 0;
			}
			set
			{
				
			}
		}

		public double MeasurePower()
		{
			return 0.0;
		}

		public double MeasurePower(long pBaseFreq)
		{
			return 0.0;
		}

		public bool IsRunning
		{
			get { return false; }
		}

		public void Start()
		{
			
		}

		public void Stop()
		{
		}

		public KaorCore.Trace.BaseTrace UserCreateNewTrace(long pFstart, long pFstop)
		{
			MessageBox.Show(Locale.rpu_cant_create_trace, 
				Locale.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			return null; 
		}

		public KaorCore.Trace.BaseTrace UserQuickCreateNewTrace(long pFstart, long pFstop)
		{
			return null;
		}

		public event NewPowerMeasure OnNewPowerMeasure;

		#endregion
	}
}