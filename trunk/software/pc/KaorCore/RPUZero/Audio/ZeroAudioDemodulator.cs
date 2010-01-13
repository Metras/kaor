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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using KaorCore.Audio;
using KaorCore.RPU;
using KaorCore.Report;

namespace KaorCore.RPUZero.Audio
{
	/// <summary>
	/// Блок демодулятора РПВ-3
	/// 
	/// </summary>
	public class ZeroAudioDemodulator : IAudioDemodulator
	{
		AudioRecorderControl recordControl;

		RPUZeroControl control;
		CRPUZero rpu;
		string recordName;
		Stream recordStream;
		string recordStreamName;
		int recordNumber;

		/// <summary>
		/// Для получения темпового каталога для записи сохраняемых вавок
		/// </summary>
		/// <param name="nBufferLength"></param>
		/// <param name="lpBuffer"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int GetTempPathA(int nBufferLength, string lpBuffer);

		#region ================ Конструктор ================
		
		public ZeroAudioDemodulator(CRPUZero pRPU)
		{
			rpu = pRPU;
			//control = new RPURPV3Control();
			recordStreamName = "";
			recordNumber = 0;

			recordControl = new AudioRecorderControl();
		}

		
		#endregion

		#region ================ Приватные методы ================
		
		#endregion

		#region IAudioDemodulator Members

		public List<EAudioModulationType> SupportedModulations
		{
			get { throw new NotImplementedException(); }
		}

		public EAudioModulationType CurrentModulation
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public int FilterBand
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Частота дискретизации
		/// </summary>
		public int AudioSampleRate
		{
			get 
			{
				return recordControl.AudioSamplerate;
			}
		}

		/// <summary>
		/// Количество бит в сэмпле
		/// </summary>
		public int AudioBits
		{
			get 
			{
				return recordControl.AudioBits;
			}
		}

		/// <summary>
		/// Количество каналов
		/// </summary>
		public int AudioChannels
		{
			get 
			{
				return recordControl.AudioChannels;
			}
		}

		public int DemodulatorInput
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}


		/// <summary>
		/// Признак активности работы демодулятора
		/// </summary>
		public bool IsRunning
		{
			get 
			{
				return false;
			}
		}

		/// <summary>
		/// Старт записи
		/// </summary>
		public void Start(string pRecordName)
		{
			Stop();
			try
			{
				recordName = pRecordName;

				string _tempPath = Path.GetTempPath();
				recordStreamName = _tempPath + recordName + "_" + 
					DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture) + "_" + recordNumber + ".wav";


				recordStreamName.Replace(' ', '_');
				//FileStream _baseStream = new FileStream(recordStreamName, FileMode.Create);			
				//recordStream = WaveStream.CreateFileStream(recordStreamName, _format);
			}
			catch (Exception ex)
			{
				Stop();
				throw;
			}
		}

		/// <summary>
		/// Останов записи
		/// </summary>
		public void Stop()
		{
			if (recordStream != null)
			{
				lock (recordStream)
				{
					recordStream.Close();
					recordStream = null;
				}
			}

			/// Получена новая аудио-запись для отчета
			if (OnNewAudioRecord != null)
			{
				OnNewAudioRecord(recordName);
			}
		}
		/// <summary>
		/// Поддержка выхода наушников
		/// </summary>
		public bool IsHeadphonesSupported
		{
			get { return true; }
		}

		/// <summary>
		/// Включение выхода наушников
		/// </summary>
		public void HeadPhonesOn()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Отключение выхода наушников
		/// </summary>
		public void HeadPhonesOff()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Номер выхода наушников
		/// </summary>
		public int HeadPhonesOutput
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public event NewAudioData OnNewAudioData;

		public event NewAudioRecord OnNewAudioRecord;


		public int AudioVolume
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}


		public List<int> SupportedFilterBands
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IAudioDemodulator Members


		string IAudioDemodulator.Stop()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IAudioDemodulator Members


		public UserControl Control
		{
			get { return recordControl; }
		}

		#endregion
	}
}
