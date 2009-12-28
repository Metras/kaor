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
using System.IO;
using System.Text;

namespace KaorCore.Audio
{
	/// <summary>
	/// Менеджер аудио-данных, аудио-устройств, и т.д.
	/// </summary>
	public class AudioManager
	{
		#region ================ Поля ================
		string wavPath;
		int serial;

		#endregion

		#region ================ Конструктор ================
		public AudioManager(string pWAVPath)
		{
			wavPath = pWAVPath;
			serial = 0;
		}

		public int Serial
		{
			get
			{
				serial++;
				return serial;
			}
		}

		#endregion

		#region ================ Секция синглтона ================
		private static AudioManager instance;

		public static AudioManager Instance
		{
			get
			{
				if (instance == null)
					instance = new AudioManager("wavs\\");

				return instance;
			}
		}


		/// <summary>
		/// Создание нового аудио-потока
		/// </summary>
		/// <returns></returns>
		public static WaveStream CreateStream()
		{
			Stream _wavData = new FileStream(Instance.wavPath + "s_ " +
				DateTime.Now.ToFileTime() + "_" + Instance.Serial.ToString(CultureInfo.InvariantCulture) + ".wav", FileMode.Create);

			WaveFormat _format = new WaveFormat(44100, 16, 2);
			
			WaveStream _stream = (WaveStream)WaveStream.CreateStream(_wavData, _format);

			return _stream;
		}

		/// <summary>
		/// Открытие аудиопотока из файла
		/// </summary>
		/// <returns></returns>
		public static WaveStream OpenStream()
		{
			return null;
		}
		#endregion
	}
}
