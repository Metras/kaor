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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading;

using KaorCore.Utils;

namespace KaorCore.Undo
{
	/// <summary>
	/// Класс состояния объекта
	/// </summary>
	public class UndoState<T>
	{
		string tempFileName;
		Guid id;
		ManualResetEvent evtSaveCompleted;

		public UndoState(T pObject)
		{
			if (!typeof(T).IsSerializable)
				throw new ArgumentException("Object must be serializable!");

			tempFileName = FileSystem.GetTempFileName();
			id = Guid.NewGuid();
			evtSaveCompleted = new ManualResetEvent(false);

			new SaveObjectDelegate(SaveObject).BeginInvoke(pObject, null, null);
		}

		~UndoState()
		{
			File.Delete(tempFileName);
		}

		public bool SaveCompleted
		{
			get
			{
				return evtSaveCompleted.WaitOne(0, true);
			}
		}

		delegate void SaveObjectDelegate(T pObject);

		void SaveObject(T pObject)
		{
			FileStream _fs = new FileStream(tempFileName, FileMode.Create);
			BinaryFormatter _bfmt = new BinaryFormatter();
			
			_bfmt.Serialize(_fs, pObject);

			_fs.Close();

			evtSaveCompleted.Set();
		}

		public T LoadObject()
		{
			if (!evtSaveCompleted.WaitOne(500, true))
				throw new Exception("Object save timeout");

			FileStream _fs = new FileStream(tempFileName, FileMode.Open);
			BinaryFormatter _bfmt = new BinaryFormatter();
			object _o = _bfmt.Deserialize(_fs);

			_fs.Close();

			return (T)_o;
		}
	}
}
