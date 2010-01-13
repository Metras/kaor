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
using System.Xml;

namespace KaorCore.Signal
{
	/// <summary>
	/// Базовый класс информации о записи сигнала
	/// </summary>
	public abstract class RecordInfo
	{
		#region ============ Поля ============
		Guid id;
		bool isTemp;
		#endregion

		#region ============ Конструктор ============
		public RecordInfo()
		{
			id = Guid.NewGuid();
			isTemp = true;
		}
		#endregion

		#region ============ Проперти ============
		public Guid Id
		{
			get { return id; }
		}

		public bool IsTemp
		{
			get { return isTemp; }
			set
			{
				isTemp = value;

				if (isTemp == true)
				{
					/// Перемещение данных во временную область
					/// 
					ToTemp();
				}
				else
				{
					ToData();
				}
			}
		}
		#endregion

		#region ============ Методы ============

		public abstract void ShowInfo();
		public abstract void Playback();

		protected abstract void ToTemp();
		protected abstract void ToData();

		protected abstract void SaveBodyToXmlWriter(XmlWriter pWriter);
		protected abstract void LoadBodyFromXmlNode(XmlNode pNode);

		public void SaveToXmlWriter(XmlWriter pWriter)
		{
			SaveBodyToXmlWriter(pWriter);
		}

		public void LoadFromXmlNode(XmlNode pNode)
		{
			LoadBodyFromXmlNode(pNode);
		}

		#endregion

	}
}
