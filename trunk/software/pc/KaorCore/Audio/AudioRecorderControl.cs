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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KaorCore.Audio;
using KaorCore.Report;
using KaorCore.Utils;
using System.Diagnostics;

namespace KaorCore.Audio
{
	public partial class AudioRecorderControl : UserControl
	{
//		RPV3AudioDemodulator audioDemodulator;
		Color waveColor;
		bool isRecording;		
		Bitmap blackImage;
		AudioRecorder audioRecorder = AudioRecorder.Instance;
		
		public AudioRecorderControl()
		{
			InitializeComponent();

			if (!audioRecorder.IsAvailable)
			{
				Enabled = false;
				return;
			}

			waveColor = Color.LimeGreen;
			toolBtnRecord.Image = imgList.Images["record.png"];
			toolBtnRecord.ImageAlign = ContentAlignment.MiddleCenter;
			isRecording = false;
			audioRecorder.OnNewAudioData += new NewAudioData(audioDemodulator_OnNewAudioData);
		}


		public event NewAudioData OnNewAudioData;

		public int AudioSamplerate
		{
			get { return audioRecorder.AudioSamplerate; }
			set { audioRecorder.AudioSamplerate = value; }
		}

		public int AudioBits
		{
			get { return audioRecorder.AudioBits; }
			set { audioRecorder.AudioBits = value; }
		}

		public int AudioChannels
		{
			get { return audioRecorder.AudioChannels; }
			set { audioRecorder.AudioChannels = value; }
		}

		public WaveFormat RecordFormat
		{
			get { return audioRecorder.RecordFormat; }
		}
#if false
		void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (audioDemodulator == null)
				return;

			audioDemodulator.Stop();
			audioDemodulator.Dispose();
		}
#endif

		public void DrawTimeDomain(AudioData pWAVData)
		{
			Bitmap _canvasTimeDomain;

            // Set up for drawing
			_canvasTimeDomain = new Bitmap(pictTimeDomain.Width, pictTimeDomain.Height);
            Graphics offScreenDC = Graphics.FromImage(_canvasTimeDomain);
            SolidBrush brush = new System.Drawing.SolidBrush(Color.FromArgb(0, 0, 0));
            Pen pen = new System.Drawing.Pen(Color.WhiteSmoke);

            // Determine channnel boundries
            int width = _canvasTimeDomain.Width;
            int center = _canvasTimeDomain.Height / 2;
            int height = _canvasTimeDomain.Height;

            // offScreenDC.DrawLine(pen, 0, center, width, center);

            int leftLeft = 0;
            int leftTop = 0;
            int leftRight = width;
			int leftBottom = height; // center - 1;

            int rightLeft = 0;
            int rightTop = center + 1;
            int rightRight = width;
            int rightBottom = height;

            // Draw left channel
            double yCenterLeft = (leftBottom - leftTop) / 2;
            double yScaleLeft = 0.5 * (leftBottom - leftTop) / 32768;  // a 16 bit sample has values from -32768 to 32767
            int xPrevLeft = 0, yPrevLeft = 0;
            for (int xAxis = leftLeft; xAxis < leftRight; xAxis++)
            {
                //int yAxis = (int)(yCenterLeft + (pWAVData.Data[0, pWAVData.Data.Length / (leftRight - leftLeft) * xAxis] * yScaleLeft));
				
				int yAxis = (int)(yCenterLeft + (pWAVData.Data[pWAVData.AudioDataLength / (leftRight - leftLeft) * xAxis] * yScaleLeft));
                if (xAxis == 0)
                {
                    xPrevLeft = 0;
                    yPrevLeft = yAxis;
                }
                else
                {
					pen.Color = waveColor;
                    offScreenDC.DrawLine(pen, xPrevLeft, yPrevLeft, xAxis, yAxis);
                    xPrevLeft = xAxis;
                    yPrevLeft = yAxis;
                }
            }

            // Clean up
			pictTimeDomain.Image = _canvasTimeDomain;
            offScreenDC.Dispose();
		}

		#region ================ Проперти ================

#if false		
		public RPV3AudioDemodulator Demodulator
		{
			get
			{
				return audioDemodulator;
			}

			set
			{
				
				if (audioDemodulator != null)
				{
					audioDemodulator.StopRecorder();
					/// Если уже был прицеплен демодулятор, то освобождаем его
					/// 
					audioDemodulator.OnNewAudioData -= audioDemodulator_OnNewAudioData;
				}

				audioDemodulator = value;

				if (audioDemodulator != null)
				{
					audioDemodulator.StartRecorder();
					/// Регистрация обработчика событий поступления нового блока аудиоданныъ
					audioDemodulator.OnNewAudioData += new KaorCore.RPU.NewAudioData(audioDemodulator_OnNewAudioData);

					/// Разрешение кнопок записи
					/// 
					toolBtnRecord.Enabled = true;
					trackVolume.Value = audioDemodulator.AudioVolume;
				}
				else
				{
					/// Запрет кнопок записи
					/// 
					toolBtnRecord.Enabled = false;
					
				}
			}
		}
#endif

		#endregion

		#region ================ Обработчики событий ================

		delegate void NewAudioDataHandlerDelegate(AudioData pData);
		void NewAudioDataHandler(AudioData pData)
		{
			if (!InvokeRequired)
			{
				if (!Visible)
					return;

				if (!chkMute.Checked)
					DrawTimeDomain(pData);
				else
				{
					if (pictTimeDomain.Image != null)
					{
						Graphics _g = Graphics.FromImage(pictTimeDomain.Image);
						_g.Clear(Color.Black);
						pictTimeDomain.Invalidate();
						_g.Dispose();
						//pictTimeDomain.Image = _g
					}
				}
			}
			else
//				new NewAudioDataHandlerDelegate(NewAudioDataHandler).BeginInvoke(pData, null, null);
				Invoke(new NewAudioDataHandlerDelegate(NewAudioDataHandler), pData);
		}

		/// <summary>
		/// Обработчик события поступления нового буфера отсчетов от демодулятора
		/// </summary>
		/// <param name="pData"></param>
		void audioDemodulator_OnNewAudioData(AudioData pData)
		{

			if (OnNewAudioData != null)
				OnNewAudioData(pData);

			NewAudioDataHandler(pData);
		}

		#endregion

		public delegate void RecordStateChangedDelegate(AudioRecorderControl pControl, bool pIsRecording, string pRecordName);
		public event RecordStateChangedDelegate OnRecordStateChanged;

		private void CallOnRecordStateChanged(bool pIsRecording)
		{
			if (OnRecordStateChanged != null)
				OnRecordStateChanged(this, pIsRecording, txtName.Text);
		}

		private void toolBtnRecord_Click(object sender, EventArgs e)
		{

			isRecording = true;

			waveColor = Color.Tomato;

			/// Включение отображения аудио-данных
			chkMute.Checked = false;
			txtName.Enabled = false;
			chkMute.Enabled = false;
			toolBtnRecord.Enabled = false;
			btnStopRecord.Enabled = true;
			//toolBtnRecord.Image = imgList.Images["stop.png"];
			CallOnRecordStateChanged(isRecording);
		}

		public delegate void VolumeChangedDelegate(int pVolume);
		public event VolumeChangedDelegate OnVolumeChanged;

		private void trackVolume_Scroll(object sender, EventArgs e)
		{
			if (OnVolumeChanged != null)
				OnVolumeChanged(trackVolume.Value);
		}

		private void AudioRecorderControl_VisibleChanged(object sender, EventArgs e)
		{
#if false
			if(ParentForm != null)
				ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);			
#endif
		}

		public delegate void MuteChangedDelegate(bool pIsMute);
		public event MuteChangedDelegate OnMuteChanged;

		private void chkMute_CheckedChanged(object sender, EventArgs e)
		{
			if (chkMute.Checked)
			{
				trackVolume.Enabled = false;

				if(OnMuteChanged != null)
					OnMuteChanged(true);
			}
			else
			{
				if(OnMuteChanged != null)
					OnMuteChanged(false);

				trackVolume.Enabled = true;
			}
		}

		private void btnStopRecord_Click(object sender, EventArgs e)
		{
			isRecording = false;

			waveColor = Color.LimeGreen;
			btnStopRecord.Enabled = false;
			toolBtnRecord.Enabled = true;
			txtName.Enabled = true;
			chkMute.Enabled = true;
			CallOnRecordStateChanged(isRecording);
		}

		private void btnSettings_Click(object sender, EventArgs e)
		{
			Process.Start("rundll32.exe",
				"shell32.dll, Control_RunDLL mmsys.cpl,,");

		}
	}
}
