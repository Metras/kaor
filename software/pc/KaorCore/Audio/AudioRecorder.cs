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

namespace KaorCore.Audio
{
	public class AudioRecorder : IDisposable
	{
		WaveFormat recordFormat;
		WaveInRecorder recorder;
		int audioSamplerate = 44100;

		public int AudioSamplerate
		{
			get { return audioSamplerate; }
			set { audioSamplerate = value; }
		}
		int audioBits = 16;

		public int AudioBits
		{
			get { return audioBits; }
			set { audioBits = value; }
		}
		int audioBufferSize = 16384;
		int audioChannels = 2;

		public int AudioChannels
		{
			get { return audioChannels; }
			set { audioChannels = value; }
		}
		short[] audioData;
		byte[] audioDataRaw;


		/// <summary>
		/// Делегат события получения новых аудио-данных
		/// </summary>
		/// <param name="data"></param>

		bool isAvailable;

		AudioRecorder()
		{
			isAvailable = false;

			recordFormat = new WaveFormat(audioSamplerate, audioBits, audioChannels);

			try
			{
				recorder = new WaveInRecorder(0, recordFormat, audioBufferSize * 2, 3, new BufferDoneEventHandler(AudioDataArrived));
				isAvailable = true;
			}

			catch
			{
				/// Ошибка создания аудио
				/// 
			}
		}

		public bool IsAvailable
		{
			get { return isAvailable; }
		}
		/// <summary>
		/// Обработчик события поступления новых аудио-данных
		/// </summary>
		/// <param name="data"></param>
		/// <param name="size"></param>
		void AudioDataArrived(IntPtr data, int size)
		{
			lock (recorder)
			{
				////if (OnNewAudioData == null)
				////    return;

				if (audioData == null || audioData.Length < size / 2)
					audioData = new short[size / 2];

				if (audioDataRaw == null || audioDataRaw.Length < size)
					audioDataRaw = new byte[size];
				
				if (audioData != null)
				{
					/// Обработка принятого блока аудио-данных
					System.Runtime.InteropServices.Marshal.Copy(data, audioData, 0, size / 2);
					System.Runtime.InteropServices.Marshal.Copy(data, audioDataRaw, 0, size);

					if (OnNewAudioData != null)
						OnNewAudioData(new AudioData(recordFormat, audioData, audioDataRaw));

					//// OnNewAudioData(new AudioData(new WaveFormat(audioSamplerate, audioBits, audioChannels), ));

					/// Сохраняем принятые аудиоданные во временный файл
					/// 
				}

				////if (recordStream != null)
				////{
				////    if (writeData == null || writeData.Length < size)
				////        writeData = new byte[size];

				////    if (writeData != null)
				////    {
				////        System.Runtime.InteropServices.Marshal.Copy(data, writeData, 0, size);
				////        recordStream.Write(writeData, 0, size);
				////    }
				////}

			}
		}

		#region ============= Секция синглтона =============
		static AudioRecorder instance;

		public static AudioRecorder Instance
		{
			get
			{
				if (instance == null)
					instance = new AudioRecorder();

				return instance;
			}
		}

		~AudioRecorder()
		{
			Dispose();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (recorder != null)
			{
				//lock (recorder)
				{
					recorder.Dispose();
					recorder = null;
				}
			}
		}

		#endregion

		public WaveFormat RecordFormat
		{
			get
			{
				return recordFormat;
			}
		}
		public event NewAudioData OnNewAudioData;
	}

	public delegate void NewAudioData(AudioData pData);

}
