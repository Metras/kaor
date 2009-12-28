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
using System.Windows.Forms;

namespace RPUICOMR8500.Splash
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

		}

		private System.Windows.Forms.Timer fadeTimer;

#endregion

#region Static Methods

		internal static Splash Instance = null;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		internal static System.Threading.Thread splashThread = null;

		public static void ShowSplash() {
			//	Show Splash with no fading
			ShowSplash(0, 0);
		}

		public static void ShowSplash(int fadeinTime, int fadeoutTimer) {
			//	Only show if not showing already
			if (Instance == null) {
				Instance = new Splash();

				//	Hide initially so as to avoid a nasty pre paint flicker
				Instance.Opacity = 0;
				Instance.Show();
				
				//	Process the initial paint events
				Application.DoEvents();
				if (fadeoutTimer > 0)
				{
					int fadeStep = (int)System.Math.Round((double)fadeoutTimer / 20);
					Instance.fadeTimer.Interval = fadeStep;
				}
				else
				{
					//	Set the timer interval so that we fade out instantly.
					Instance.fadeTimer.Interval = 1;
				}
				// Perform the fade in
				if (fadeinTime > 0) {
					//	Set the timer interval so that we fade out at the same speed.
					int fadeStep = (int)System.Math.Round((double)fadeinTime/20);
					

					for (int i = 0; i <= fadeinTime; i += fadeStep){
						System.Threading.Thread.Sleep(fadeStep);
						Instance.Opacity += 0.05;
					}
				} else {
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
			if (this.fadeTimer.Interval == 1) {
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
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// backgroundWorker1
			// 
			this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
			// 
			// Splash
			// 
			this.AutoSize = true;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(348, 199);
			this.DoubleBuffered = true;
			this.Name = "Splash";
			this.Text = "Splas";
			this.ResumeLayout(false);

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
	}
}
