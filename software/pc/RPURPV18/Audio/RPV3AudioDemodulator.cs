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
using KaorCore.Utils;

namespace RPURPV18.Audio
{
	/// <summary>
	/// Блок демодулятора РПВ-18
	/// 
	/// </summary>
	public class RPV18AudioDemodulator : IAudioDemodulator, IDisposable
	{
		/// <summary>
		/// Объет записи звука с линейного входа
		/// </summary>
		
		////WaveInRecorder recorder;
		////short[] audioData;
		//byte[] writeData;

		/// <summary>
		/// Параметры аудио
		/// </summary>
		////int audioSamplerate = 44100;
		////int audioBits = 16;
		////int audioBufferSize = 16384;
		////int audioChannels = 2;

		RPURPV18Control control;
		CRPURPV18 rpu;
		string recordName;
		FileStream recordStream;
		string recordStreamName;
		int recordNumber;
		int audioVolume = 0;

		EAudioModulationType currentModulation = EAudioModulationType.AM;
		int currentFilterBand = 3000;

		/// <summary>
		/// Для получения темпового каталога для записи сохраняемых вавок
		/// </summary>
		/// <param name="nBufferLength"></param>
		/// <param name="lpBuffer"></param>
		/// <returns></returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern int GetTempPathA(int nBufferLength, string lpBuffer);


		/// <summary>
		/// Уровень последней громкости думодулятора
		/// </summary>
		int lastVolume;

		/// <summary>
		/// Признак отключенного выхода
		/// </summary>
		bool mute;

		AudioRecorderControl recorderControl;

		// Объект блокировки для работы с recordStream
		object recordStreamLock = new object();

		////WaveFormat recordFormat;
		#region ================ Конструктор ================
		
		public RPV18AudioDemodulator(CRPURPV18 pRPU)
		{
			rpu = pRPU;
			////recorder = null;
			recordStreamName = "";
			recordNumber = 0;

			recorderControl = new AudioRecorderControl();
			recorderControl.OnVolumeChanged += new AudioRecorderControl.VolumeChangedDelegate(recorderControl_OnVolumeChanged);
			recorderControl.OnMuteChanged += new AudioRecorderControl.MuteChangedDelegate(recorderControl_OnMuteChanged);
			recorderControl.OnNewAudioData += new NewAudioData(recorderControl_OnNewAudioData);
			recorderControl.OnRecordStateChanged += new AudioRecorderControl.RecordStateChangedDelegate(recorderControl_OnRecordStateChanged);
			////recordFormat = new WaveFormat(audioSamplerate, audioBits, audioChannels);			
		}

		void recorderControl_OnRecordStateChanged(AudioRecorderControl pControl, bool pIsRecording, string pRecordName)
		{
			if (pIsRecording)
			{
				/// Начало записи
				Start(FileSystem.GetTempFileName());
			}
			else
			{
				/// Останов записи
				/// 
				string _name = pRecordName;

				if (_name == "")
				{
					_name = KaorCore.Utils.FreqUtils.FreqToString(RPU.BaseFreq) + ", "
						+ CurrentModulation + "/"
						+ KaorCore.Utils.FreqUtils.FreqToString(FilterBand);
				}

				string _tmpFileName = Stop();

				if (_tmpFileName != null && _tmpFileName != "")
				{
					//string _reportName = ReportsManager.NewReportName(ReportType.Audio, _name);
					//File.Move(_tmpFileName, _reportName);

					string _reportName = ReportsManager.NewReportName(ReportType.Audio, _name);

					FileStream _origStream = File.Open(_tmpFileName, FileMode.Open);

					Stream _wavFile = WaveStream.CreateFileStream(_reportName, recorderControl.RecordFormat, (int)_origStream.Length);

					_origStream.Seek(0, SeekOrigin.Begin);

					int _len = (int)_origStream.Length;
					int _count = _len / 16384;
					byte[] _buf = new byte[16384];

					while (_len > 16384)
					{
						_origStream.Read(_buf, 0, 16384);
						_wavFile.Write(_buf, 0, 16384);
						_len -= 16384;
					}

					if (_len > 0)
					{
						_origStream.Read(_buf, 0, _len);
						_wavFile.Write(_buf, 0, _len);
					}

					_wavFile.Close();
					_origStream.Close();

					File.Delete(_origStream.Name);
				}
			}
		}

		void recorderControl_OnNewAudioData(AudioData pData)
		{
			lock (recordStreamLock)
			{
				if (recordStream != null && pData != null)
				{
//					if (writeData == null || writeData.Length < pData.AudioDataLength)
//						writeData = new byte[pData.AudioDataLength];

//					if (writeData != null)
					{
						recordStream.Write(pData.DataRaw, 0, pData.DataRaw.Length);
					}
				}
			}
		}

		void recorderControl_OnMuteChanged(bool pIsMute)
		{
			if (pIsMute)
			{
				new MethodInvoker(HeadPhonesOff).BeginInvoke(null, null);

			}
			else
			{
				new MethodInvoker(HeadPhonesOn).BeginInvoke(null, null);
			}
		}

		void recorderControl_OnVolumeChanged(int pVolume)
		{
			AudioVolume = pVolume;
		}


		////~RPV18AudioDemodulator()
		////{
		////    if (recorder != null)
		////        recorder.Dispose();
		////}
		#endregion

		#region ================ Проперти ================
		
		public UserControl Control
		{
			get
			{
				return recorderControl;
			}
		}

		public CRPURPV18 RPU
		{
			get { return rpu; }
		}
		#endregion

		#region ================ Приватные методы ================
		#endregion

		/// <summary>
		/// Установка порога шумоподавления демодулятора
		/// </summary>
		/// <param name="Level">Порог в дБ (от -127 до -20, -128 для выключения)</param>
		public void SetNoiseThreshold(Int32 Level)
		{
			rpu.SendCommand(new CRPURPV18.RPV_CmdSetNoiseThreshold((sbyte)Level));
		}

		#region IAudioDemodulator Members

		public List<EAudioModulationType> SupportedModulations
		{
			get { return new List<EAudioModulationType> { 
				EAudioModulationType.AM, 
				EAudioModulationType.FM, 
				EAudioModulationType.CW, 
				EAudioModulationType.LSB, 
				EAudioModulationType.USB }; }
		}

		public List<Int32> SupportedFilterBands
		{
			get
			{
				return new List<Int32> {3000, 6000, 15000, 50000, 230000}; 
			}
		}

		public EAudioModulationType CurrentModulation
		{
			get
			{
				return currentModulation;
			}
			set
			{
				rpu.SetDemodulation(value.ToString(CultureInfo.InvariantCulture), currentFilterBand);
				currentModulation = value;

				rpu.CallOnRPUParamsChanged();
			}
		}

		public int FilterBand
		{
			get
			{
				return currentFilterBand;
			}
			set
			{
				rpu.SetDemodulation(currentModulation.ToString(CultureInfo.InvariantCulture), value);
				currentFilterBand = value;

				rpu.CallOnRPUParamsChanged();
			}
		}

		/// <summary>
		/// Частота дискретизации
		/// </summary>
		public int AudioSampleRate
		{
			get 
			{
				return recorderControl.AudioSamplerate;
			}
		}

		/// <summary>
		/// Количество бит в сэмпле
		/// </summary>
		public int AudioBits
		{
			get 
			{
				return recorderControl.AudioBits;
			}
		}

		/// <summary>
		/// Количество каналов
		/// </summary>
		public int AudioChannels
		{
			get 
			{
				return recorderControl.AudioChannels;
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
				if (recordStream != null && recordStream.CanWrite)
					return true;
				else
					return false;
			}
		}

		public void StartRecorder()
		{
			//if (recorder != null)
			//    recorder.Dispose();

			
		}

		public void StopRecorder()
		{
			////if (recorder != null)
			////    recorder.Dispose();
		}

		/// <summary>
		/// Старт записи
		/// </summary>
		/// <param name="pRecordName">Полный путь к файлу записи</param>
		public void Start(string pRecordName)
		{
			Stop();

			////lock (recorder)
			{
				try
				{
					recordName = pRecordName;


					string _tempPath = FileSystem.GetTempFileName();
					recordStreamName = recordName;

					//recordStream = WaveStream.CreateFileStream(recordStreamName, recorderControl.RecordFormat);
					recordStream = File.Create(recordStreamName);
				}
				catch (Exception ex)
				{
					Stop();
					throw;
				}
			}
		}

		public void StartTime(string pRecordName, int pTime)
		{
			throw new NotImplementedException();
		}

		void _recTmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Stop();
		}

		/// <summary>
		/// Останов записи
		/// </summary>
		public string Stop()
		{
			string _res = "";
			lock (recordStreamLock)
			{
				if (recordStream != null)
				{
					try
					{
						recordStream.Close();
						_res = recordName;

					}
					finally
					{
						recordStream = null;
					}
				}

			}

			return _res;
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
		/// 

		public void HeadPhonesOn()
		{
			mute = false;
			AudioVolume = lastVolume;
		}

		/// <summary>
		/// Отключение выхода наушников
		/// </summary>
		public void HeadPhonesOff()
		{
			lastVolume = AudioVolume;
			AudioVolume = 0;
			mute = true;
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

		#endregion

		#region IAudioDemodulator Members


		public int AudioVolume
		{
			get
			{
				return audioVolume;
			}
			set
			{
				if (mute)
					return;

				if ((value < 0) || (value > 100))
					throw new Exception(String.Format("Invalid volume value {0}", value));
				rpu.SendCommand(new CRPURPV18.RPV_CmdSetVolume(value / 100.0, value / 100.0));
				audioVolume = value;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			////Stop();

			////if (recorder != null)
			////{
			////    recorder.Dispose();
			////}
		}

		#endregion
	}
}
