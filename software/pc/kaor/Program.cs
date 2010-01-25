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
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using ControlUtils.Splash;
using kaor.I18N;
using KaorMainView;
using KaorCore.RadioControlSystem;

namespace kaor
{
	static partial class Program
	{
		//static string revisionFull = "$Revision: 302 $";
		static string version = "1.0";

		//static string revision;

		/// <summary>
		/// The main entry point for the application. 
		/// </summary> 
		[STAThread]
		static void Main()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
#if DEBUG1
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfoByIetfLanguageTag("en-US");
#endif
	
			//revision = revisionFull.Split(' ')[1];
			string _versionString = "KAOR® Community Edition v" + version + " build " + revisionSVN;
			
			AppDomain.CurrentDomain.SetData("revision", revisionSVN);
			AppDomain.CurrentDomain.SetData("version", version);
			AppDomain.CurrentDomain.SetData("versionstring", _versionString);
			AppDomain.CurrentDomain.SetData("low_band", 20000000L);	// Нижняя граница диапазона
			AppDomain.CurrentDomain.SetData("high_band", 18000000000L);	// Верхняя граница диапазона

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			/// Проверка запущенного ПО
			/// 
			Semaphore _s = new Semaphore(1, 1, "KAOR RCS");
			if (!_s.WaitOne(0, false))
			{
				MessageBox.Show(Locale.already_running, Locale.error,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				return;
			}

			Splash.ShowSplash(250, _versionString);
#if DEBUG
			Splash.Fadeout();
#endif

			/// Создание директорий маркеров, трасс, сигналов, остояний
			/// 
			if (!Directory.Exists(Application.StartupPath + "\\markers"))
				Directory.CreateDirectory(Application.StartupPath + "\\markers");

			if (!Directory.Exists(Application.StartupPath + "\\signals"))
				Directory.CreateDirectory(Application.StartupPath + "\\signals");

			if (!Directory.Exists(Application.StartupPath + "\\states"))
				Directory.CreateDirectory(Application.StartupPath + "\\states");

			if (!Directory.Exists(Application.StartupPath + "\\traces"))
				Directory.CreateDirectory(Application.StartupPath + "\\traces");

			if(File.Exists("settings.xml"))
				BaseRadioControlSystem.CreateInstance("settings.xml");
			else
				BaseRadioControlSystem.CreateInstance("radiocontrolsystem.xml");

			/// Включение РПУ
			BaseRadioControlSystem.Instance.SwitchOn();

			Splash.Status = Locale.status_loading_ui;
		 
			Application.Run(new KaorMain());

			BaseRadioControlSystem.Instance.SwitchOff();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ApplicationCrashForm _cf = new ApplicationCrashForm();
			_cf.FillData(e.ExceptionObject);

			Splash.Fadeout();

			if (_cf.ShowDialog() == DialogResult.OK)
			{
				if (_cf.checkBox1.Checked)
				{
					Semaphore s = Semaphore.OpenExisting("KAOR RCS");
					s.Release();

					AppDomain.CurrentDomain.SetData("ApplicationExit", true);
					Application.Restart();
				}
				else
				{
					Application.Exit();
					AppDomain.CurrentDomain.SetData("ApplicationExit", true);
				}
			}
			else
				AppDomain.CurrentDomain.SetData("ApplicationCrash", false);
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			ApplicationCrashForm _cf = new ApplicationCrashForm();
			_cf.FillData(e.Exception);

			Splash.Fadeout();

			if (_cf.ShowDialog() == DialogResult.OK)
			{
				BaseRadioControlSystem.Instance.SaveFullState();
			}
			if (_cf.checkBox1.Checked)
			{
				Semaphore s = Semaphore.OpenExisting("KAOR RCS");
				s.Release();

				AppDomain.CurrentDomain.SetData("ApplicationExit", true);
				Application.Restart();

				//Application.Exit();
			}
			else
			{
				Application.Exit();
				AppDomain.CurrentDomain.SetData("ApplicationExit", true);
			}
		}
	}
}
