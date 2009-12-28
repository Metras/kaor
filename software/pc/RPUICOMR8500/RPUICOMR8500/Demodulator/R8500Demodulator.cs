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
using System.IO;
using System.Text;
using System.Windows.Forms;

using KaorCore.Audio;
using KaorCore.Report;
using KaorCore.RPU;
using KaorCore.Utils;

using RPUICOMR8500.I18N;

namespace RPUICOMR8500.Demodulator
{
    public class R8500Demodulator : IAudioDemodulator
    {
        RPUR8500 rpu;
		AudioRecorderControl recorderControl;
		FileStream recordStream;
		object recordStreamLock = new object();

		string recordStreamName;
		string recordName;
		//byte[] writeData;

		/// <summary>
		/// Признак отключенного выхода
		/// </summary>
		bool mute;
		int audioVolume = 0;
		int lastVolume;

        /// <summary>
        /// создает новый экземпляр класса
        /// </summary>
        /// <param name="prpu"></param>
        public R8500Demodulator(RPUR8500 prpu)
        {
            this.rpu = prpu;
			recorderControl = new AudioRecorderControl();
			recorderControl.OnVolumeChanged += new AudioRecorderControl.VolumeChangedDelegate(recorderControl_OnVolumeChanged);
			recorderControl.OnMuteChanged += new AudioRecorderControl.MuteChangedDelegate(recorderControl_OnMuteChanged);
			recorderControl.OnNewAudioData += new NewAudioData(recorderControl_OnNewAudioData);
			recorderControl.OnRecordStateChanged += new AudioRecorderControl.RecordStateChangedDelegate(recorderControl_OnRecordStateChanged);
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
					string _reportName = ReportsManager.NewReportName(ReportType.Audio, _name);

					FileStream _origStream = File.Open(_tmpFileName, FileMode.Open);

					Stream _wavFile = WaveStream.CreateFileStream(_reportName, recorderControl.RecordFormat, (int)_origStream.Length);

					_origStream.Seek(0, SeekOrigin.Begin);

					int _len = (int)_origStream.Length;
					int _count = _len / 16384;
					byte[] _buf = new byte[16384];

					while(_len > 16384)
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

		public RPUR8500 RPU
		{
			get { return rpu; }
		}

		void recorderControl_OnNewAudioData(AudioData pData)
		{
			lock (recordStreamLock)
			{
				if (recordStream != null && pData != null)
				{
					//if (writeData == null || writeData.Length < pData.AudioDataLength)
					//    writeData = new byte[pData.AudioDataLength];

					//if (writeData != null)
					{
						//System.Runtime.InteropServices.Marshal.Copy(data, writeData, 0, size);

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

        #region IAudioDemodulator Members

        /// <summary>
        /// не задано
        /// </summary>
        public List<EAudioModulationType> SupportedModulations
        {
            get { throw new NotImplementedException(); }
        }

        EAudioModulationType currentModulation;
        /// <summary>
        /// Устанавливает Operating mode
        /// </summary>
        public EAudioModulationType CurrentModulation
        {
            get
            {
                return currentModulation;
            }
            set
            {
                currentModulation = value;

				if (rpu.Mode == RPUMode.Off)
					return;

                if (value == EAudioModulationType.AM)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x02, 0x02, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.AM_narrow)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x02, 0x03, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.AM_wide)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x02, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.CW)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x03, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.FM)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x05, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.FM_narrow)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x05, 0x02, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.WFM)
                {
                    if (rpu.BaseFreq < 30000000)
                        throw new Exception(Locale.wfm_error);
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x06, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.LSB)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x00, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
                else if (value == EAudioModulationType.USB)
                {
                    byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x06, 0x01, 0x01, 0xFD };
                    byte[] answer = rpu.SendCommand(buffer);
                }
            }
        }
        /// <summary>
        /// не задано
        /// </summary>
        public int FilterBand
        {
            get
            {
				return rpu.PowerMeter.FilterBand;
            }
            set
            {
				rpu.PowerMeter.FilterBand = value;
            }
        }
        /// <summary>
        /// не задано
        /// </summary>
        public int AudioSampleRate
        {
            get 
			{
				return recorderControl.AudioSamplerate;
			}
        }
        /// <summary>
        /// не задано
        /// </summary>
        public int AudioBits
        {
            get 
			{
				return recorderControl.AudioBits;
			}
        }
        /// <summary>
        /// не задано
        /// </summary>
        public int AudioChannels
        {
            get 
			
			{
				return recorderControl.AudioChannels;
			}
        }
        /// <summary>
        /// не задано
        /// </summary>
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
        /// не задано
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
        /// <summary>
        /// не задано
        /// </summary>
        /// <param name="pRecordName"></param>
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
        /// <summary>
        /// не задано
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
        /// не задано
        /// </summary>
        public bool IsHeadphonesSupported
        {
			get { return true; }
        }
        /// <summary>
        /// не задано
        /// </summary>
        public void HeadPhonesOn()
        {
			mute = false;
			AudioVolume = lastVolume;
        }
        /// <summary>
        /// не задано
        /// </summary>
        public void HeadPhonesOff()
        {
			lastVolume = AudioVolume;
			AudioVolume = 0;
			mute = true;
        }
        /// <summary>
        /// не задано
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

				double _volD = value * 2.55;
				int _volume = (int)_volD;
				if (_volume > 255)
					_volume = 255;
				else if (_volume < 0)
					_volume = 0;

				byte _hi = (byte)(_volume / 100);
				byte _lo = (byte)((((_volume % 99) / 10) * 16) + (_volume % 10));


/*				if (_volume < 100)
				{
					byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x14, _lo, 0xFD };
				}
				else*/
				//{
					byte[] buffer = { 0xFE, 0xFE, 0x4A, 0xE0, 0x14, 0x01, _hi, _lo, 0xFD };
				//}
				byte[] answer = rpu.SendCommand(buffer);

				audioVolume = value;
			}
		}

		#endregion

		#region IAudioDemodulator Members


		public List<int> SupportedFilterBands
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IAudioDemodulator Members


		public UserControl Control
		{
			get 
			{
				return recorderControl;
			}
		}

		#endregion
	}
}
