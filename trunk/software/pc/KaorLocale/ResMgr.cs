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
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace KaorCore.I18N
{
	/// <summary>
	/// Менеджер и18н ресурсов
	/// Выполнен в виде синглтона
	/// При инициализации подгружает нужную локализацию
	/// </summary>
	public class ResMgr
	{
		ResourceManager resourceManager;
		CultureInfo cultureInfo;

		ResMgr(string pLocale)
		{
			resourceManager = new ResourceManager("KaorCore.Locale", Assembly.GetExecutingAssembly());
			cultureInfo = new CultureInfo(pLocale);
		}

		#region Секция синглтона
		static ResMgr instance;
		static ResMgr Instance
		{
			get
			{
				if (instance == null)
				{
					/// Создание инстанса с культурой по-умолчанию
					instance = new ResMgr(CultureInfo.CurrentCulture.Name);
					//throw new InvalidOperationException("Need to initialize instance of ResMgr via CreateInstance(...) method first");
				}
				return instance;
			}
		}

		public static void CreateInstance(string pLocale)
		{
			instance = new ResMgr(pLocale);
		}

		public static string GetString(string pStringName)
		{
			return Instance.resourceManager.GetString(pStringName, Instance.cultureInfo);
		}

		public static object GetObject(string pObjectName)
		{
			return Instance.resourceManager.GetObject(pObjectName, Instance.cultureInfo);
		}
		#endregion
	}
}
