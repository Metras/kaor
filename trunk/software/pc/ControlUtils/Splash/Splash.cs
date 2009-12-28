/*
 * Created by SharpDevelop.  
 * User: mjackson
 * Date: 25/10/2006
 * Time: 08:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ControlUtils.Splash
{
	
	/// <summary>
	/// Description of Splash.
	/// </summary>
	public sealed class Splash  : System.Windows.Forms.Form
	{
		int pFadeInTimeout;
	
#region initialisation


		public Splash() : base() {

			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//	Size to the image so as to display it fully and position the form in the center screen with no border.
			//this.Size = this.BackgroundImage.Size;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			//	Force the splash to stay on top while the mainform renders but don't show it in the taskbar.
			this.TopMost = true;
			this.ShowInTaskbar = false;
			
			//	Make the backcolour Fuchia and set that to be transparent
			//	so that the image can be shown with funny shapes, round corners etc.
			//this.BackColor = System.Drawing.Color.Transparent;
			//this.TransparencyKey = System.Drawing.Color.FromArgb(0xE5, 0xff, 0xff);
#if false		    
			this.TransparencyKey = Color.Black;
		    this.BackColor = Color.Black;
#endif
			//this.BackColor = System.Drawing.Color.FromArgb(0xE5, 0xff, 0xff);
			//this.TransparencyKey = System.Drawing.Color.FromArgb(0xe5, 0xff, 0xff);
			//	Initialise a timer to do the fade out
			if (this.components == null) {
				this.components = new System.ComponentModel.Container();
			}
			this.fadeTimer = new System.Windows.Forms.Timer(this.components);

			this.KeyPress += new KeyPressEventHandler(Splash_KeyPress);

			//lblMrq.BackColor = Color.FromArgb(0, 0, 0, 0);
		}

		int keyPointer = 0;
		char[] keyPresses = { 'n', 'i', 'i', 's', 't', 't' };
		bool isPlaying = false;
		private Label label1;
		private Label label2;
		private LinkLabel linkLabel1;
		string eggFileName = "";

		[DllImport("winmm.dll")]
		private static extern long mciSendString(string lpCommand, StringBuilder lpReturnString, int uReturnLength, int hwndCallback);


		void Splash_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == keyPresses[keyPointer])
			{
				keyPointer++;

				if (keyPointer == keyPresses.Length)
				{
					keyPointer = 0;
					EasterEgg();
				}
			}
			else
				keyPointer = 0;

		}

		private void EasterEgg()
		{
			string _cmd;
			long _res;

			isPlaying = true;

			Stream _s = Assembly.GetExecutingAssembly().GetManifestResourceStream("ControlUtils.Splash.1.mp3");

			if (_s != null)
			{
				eggFileName = Path.GetTempFileName();

				byte[] _arr = new byte[_s.Length];
				_s.Read(_arr, 0, _arr.Length);

				FileStream _stream = new FileStream(eggFileName, FileMode.Create);
				_stream.Write(_arr, 0, _arr.Length);
				_stream.Close();
				_s.Close();

				_cmd = "close Mp3File";
				_res = mciSendString(_cmd, null, 0, 0);

				_cmd = "open \"" + eggFileName+ "\" type MPEGVideo alias Mp3File";
				_res = mciSendString(_cmd, null, 0, 0);

				_cmd = "play Mp3File";
				_res = mciSendString(_cmd, null, 0, 0);
			}
		}

		private System.Windows.Forms.Timer fadeTimer;

#endregion

#region Static Methods

		internal static Splash Instance = null;
		private Label lblVersion;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		private Label lblStatus;
		internal static System.Threading.Thread splashThread = null;

		public static void ShowSplash(string pVersion) {
			//	Show Splash with no fading
			ShowSplash(0, pVersion);
		}

		public static void ShowSplash(int fadeinTime, string pVersion) {
			//	Only show if not showing already
			if (Instance == null) {
				Instance = new Splash();

				/// Установка строки версии
				Instance.lblVersion.Text = pVersion;

				//	Hide initially so as to avoid a nasty pre paint flicker
				Instance.Opacity = 0;
				Instance.Show();
				
				//	Process the initial paint events
				Application.DoEvents();
				
				// Perform the fade in
				if (fadeinTime > 0) {
					//	Set the timer interval so that we fade out at the same speed.
					int fadeStep = (int)System.Math.Round((double)fadeinTime/20);
					Instance.fadeTimer.Interval = fadeStep;

					for (int i = 0; i <= fadeinTime; i += fadeStep){
						System.Threading.Thread.Sleep(fadeStep);
						Instance.Opacity += 0.05;
					}
				} else {
					//	Set the timer interval so that we fade out instantly.
					Instance.fadeTimer.Interval = 1;
				}

				Instance.Opacity = 1;
			}
		}

		public static void ShowSplashModal(int fadeinTime, string pVersion)
		{
			//	Only show if not showing already
			if (Instance == null)
			{
				Instance = new Splash();
				int fadeStep = (int)System.Math.Round((double)fadeinTime / 20);
				Instance.fadeTimer.Interval = fadeStep;
				Instance.pFadeInTimeout = fadeinTime;

				/// Установка строки версии
				Instance.lblVersion.Text = pVersion;
				//	Hide initially so as to avoid a nasty pre paint flicker
				Instance.Opacity = 0;
				Instance.backgroundWorker1.RunWorkerAsync();
				Instance.ShowDialog();
			}
		}

		public static void Fadeout() {
			//	Only fadeout if we are currently visible.

			if (Instance != null) {
				Instance.BeginInvoke(new MethodInvoker(Instance.Close));
				
				//	Process the Close Message on the Splash Thread.
				Application.DoEvents();
			}
		}

		delegate void SetStatusDelegate(string pStatus);

		void SetStatus(string pStatus)
		{
			if (!InvokeRequired)
			{
				lock (lblStatus.Text)
				{
					lblStatus.Text = pStatus;
					Update();
					System.Threading.Thread.Sleep(100);
				}
			}
			else
				Invoke(new SetStatusDelegate(SetStatus), pStatus);
		
		}
		public static string Status
		{
			get
			{
				if (Instance != null)
					return Instance.lblStatus.Text;
				else
					return "";
			}

			set
			{
				if (Instance != null)
					new SetStatusDelegate(Instance.SetStatus).Invoke(value);
			}
		}
#endregion

#region Close Splash Methods

		protected override void OnClick(System.EventArgs e) {
			//	If we are displaying as a about dialog we need to provide a way out.
			if(!backgroundWorker1.IsBusy)
				this.Close();
		}
		
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);
			
			//	Close immediatly is the timer interval is set to 1 indicating no fade.
			if (this.fadeTimer.Interval == 1) 
			{
				if (isPlaying)
				{
					string _cmd = "close Mp3File";
					mciSendString(_cmd, null, 0, 0);

					if (eggFileName != "")
						File.Delete(eggFileName);
				}

				e.Cancel = false;
				Instance = null;
				return;
			}

			//	Only use the timer to fade out if we have a mainform running otherwise there will be no message pump
			if (Application.OpenForms.Count > 1) {
				if (this.Opacity > 0) {
					e.Cancel = true;
					this.Opacity -= 0.05;
					
					//	use the timer to iteratively call the close method thereby keeping the GUI thread available for other processes.
					this.fadeTimer.Tick -= new System.EventHandler(this.FadeoutTick);
					this.fadeTimer.Tick += new System.EventHandler(this.FadeoutTick);
					this.fadeTimer.Start();
				} else {
					e.Cancel = false;
					this.fadeTimer.Stop();
					
					//	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
					Instance = null;

					if (isPlaying)
					{
						string _cmd = "close Mp3File";
						mciSendString(_cmd, null, 0, 0);

						if (eggFileName != "")
							File.Delete(eggFileName);
					}
				}
			} else {
				if (this.Opacity > 0) {
					//	Sleep on this thread to slow down the fade as there is no message pump running
					System.Threading.Thread.Sleep(this.fadeTimer.Interval);
					Instance.Opacity -= 0.05;
					
					//	iteratively call the close method
					this.Close();
				} else {
					e.Cancel = false;

					//	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
					Instance = null;

					if (isPlaying)
					{
						string _cmd = "close Mp3File";
						mciSendString(_cmd, null, 0, 0);

						if (eggFileName != "")
							File.Delete(eggFileName);
					}
				}
			}
				
		}	
		
		void FadeoutTick(object sender, System.EventArgs e){
			this.Close();
		}

#endregion

#region Designer stuff

		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
			this.lblVersion = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.lblStatus = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblVersion.ForeColor = System.Drawing.Color.Gainsboro;
			this.lblVersion.Location = new System.Drawing.Point(17, 273);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(145, 17);
			this.lblVersion.TabIndex = 0;
			this.lblVersion.Text = "KAOR 0.9.1 build 172";
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			// 
			// lblStatus
			// 
			this.lblStatus.BackColor = System.Drawing.Color.Transparent;
			this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblStatus.ForeColor = System.Drawing.Color.Gainsboro;
			this.lblStatus.Location = new System.Drawing.Point(22, 387);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(651, 23);
			this.lblStatus.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.ForeColor = System.Drawing.Color.Gainsboro;
			this.label1.Location = new System.Drawing.Point(17, 290);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(290, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Copyright (c) 2009 CJSC NII STT and others.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.ForeColor = System.Drawing.Color.Gainsboro;
			this.label2.Location = new System.Drawing.Point(17, 370);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Homepage:";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.linkLabel1.LinkColor = System.Drawing.Color.White;
			this.linkLabel1.Location = new System.Drawing.Point(104, 370);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(179, 17);
			this.linkLabel1.TabIndex = 4;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = " http://dev.niistt.ru/projects/";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.White;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// Splash
			// 
			this.AutoSize = true;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(762, 416);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblVersion);
			this.DoubleBuffered = true;
			this.Name = "Splash";
			this.Text = "Splas";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
#endregion

		void SetInstanceOpacity()
		{
			if (!InvokeRequired)
			{
				Instance.Opacity += 0.05;
			}
			else
				Invoke(new MethodInvoker(SetInstanceOpacity));

		}
		private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			//	Set the timer interval so that we fade out at the same speed.
			int fadeStep = (int)System.Math.Round((double)pFadeInTimeout / 20);
			
			for (int i = 0; i <= pFadeInTimeout; i += fadeStep)
			{
				System.Threading.Thread.Sleep(fadeStep);
				SetInstanceOpacity();
			}

			Instance.Opacity = 1;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(linkLabel1.Text);
			}
			catch
			{
			}

			if (!backgroundWorker1.IsBusy)
				this.Close();
		}
	}
}
