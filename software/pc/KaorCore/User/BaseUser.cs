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

using KaorCore.Antenna;
using KaorCore.RPU;
using KaorCore.RadioControlSystem;

namespace KaorCore.User
{
	/// <summary>
	/// Пользователь системы
	/// </summary>
	public class BaseUser
	{
		#region ================ Поля ================
		
		Guid id;
		string name;
		string description;

		#endregion

		#region ================ Конструктор ================
		
		public BaseUser(string pName, string pDescr)
		{
			id = Guid.NewGuid();
			name = pName;
			description = pDescr;
		}

		#endregion

		#region ================ Проперти ================

		public Guid Id
		{
			get { return id; }
		}

		public string Name
		{
			get { return name; }
		}

		public string Description
		{
			get { return description; }
		}

		#endregion

		#region ================ События ================

		public event RequestRPU OnRequestRPU;
		public event FreeRPU OnFreeRPU;

		/// <summary>
		/// Врапперы событий
		/// </summary>
		/// 
		IRPU CallOnRequestRPU()
		{
			IRPU _rpu = null;

			if (OnRequestRPU != null)
				_rpu = OnRequestRPU(this);

			return _rpu;
		}

		void CallOnFreeRPU(IRPU pRPU)
		{
			if (OnFreeRPU != null)
				OnFreeRPU(this, pRPU);
		}
		#endregion
	}

	public delegate IRPU RequestRPU(BaseUser pUser);
	public delegate void FreeRPU(BaseUser pUser, IRPU pRPU);
}
