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
using System.Reflection;
using System.Text;

namespace KaorCore.RPUManager
{
	/// <summary>
	/// Класс описания класов РПУ
	/// Используется менеджером для хранения информации о доступных классах РПУ,
	/// находящихся в сборках
	/// </summary>
	public class RPUClass
	{
		string rpuName;
		Type rpuClass;

		public RPUClass(Type pRPUType)
		{
			rpuClass = pRPUType;

#if !DEBUG
			if ((bool)rpuClass.InvokeMember("InternalUseOnly",
				System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty, null, null, new object[] { }) == true)
				throw new InvalidOperationException("Specified RPU type is for internal use only!");
#endif
			if (rpuClass.GetProperty("ClassName", 
				System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public) == null)
			{
				throw new InvalidOperationException("Specified type does not implements ClassName property!");
			}

			rpuName = (string)rpuClass.InvokeMember("ClassName", 
				System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty, null, null, new object[] { });
		}

		public string RPUName
		{
			get { return rpuName; }
		}

		public Type RPUType
		{
			get { return rpuClass; }
		}

		public override string ToString()
		{
			return rpuName;
		}
	}
}
