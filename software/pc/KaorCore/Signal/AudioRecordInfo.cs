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

using KaorCore.Report;

namespace KaorCore.Signal
{
	/// <summary>
	/// Информация об аудио-записи
	/// </summary>
	public class AudioRecordInfo : RecordInfo
	{
		#region ============= Поля =============
		
		string fileName;
		BaseAudioRecord audioRecord;
		#endregion

		#region ============= Конструктор =============
		public AudioRecordInfo()
			: base()
		{
		}
		#endregion

		#region ============= Проперти =============
		public BaseAudioRecord AudioRecord
		{
			get { return audioRecord; }
			set { audioRecord = value; }
		}
		#endregion

		#region ============= Методы =============
		protected override void ToTemp()
		{
			throw new NotImplementedException();
		}

		protected override void ToData()
		{
			throw new NotImplementedException();
		}

		protected override void SaveBodyToXmlWriter(System.Xml.XmlWriter pWriter)
		{
			throw new NotImplementedException();
		}

		protected override void LoadBodyFromXmlNode(System.Xml.XmlNode pNode)
		{
			throw new NotImplementedException();
		}
		#endregion

		public override void ShowInfo()
		{
			throw new NotImplementedException();
		}

		public override void Playback()
		{
			throw new NotImplementedException();
		}
	}
}
