using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KarlsTools;

namespace SignalAnalyzer.Demodulators
{
	public partial class GSMDemodulatorControl : UserControl
	{
		GSMDemodulator device;

		public GSMDemodulatorControl(GSMDemodulator pDevice)
		{
			device = pDevice;
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (device.DemodulatorInput.IsRunning)
			{
				device.Stop();
			}
			else
			{
				OpenFileDialog _dlg = new OpenFileDialog();

				if (_dlg.ShowDialog() == DialogResult.OK)
				{
					((DemodFileInput)(device.DemodulatorInput)).FileName = _dlg.FileName;
					device.Start();
				}
			}
		}

		delegate void DrawIQDelegate(Complex[] pData, Complex[] pEqData);

		public void DrawIQ(Complex[] pData, Complex[] pEqData)
		{
			if (!InvokeRequired)
			{
				double[] _real = new double[pData.Length];
				double[] _imag = new double[pData.Length];

				double[] _realEq = new double[pEqData.Length];
				double[] _imagEq = new double[pEqData.Length];

				for (int _i = 0; _i < pData.Length; _i++)
				{
					_real[_i] = pData[_i].Real();
					_imag[_i] = pData[_i].Imag();
				}

				for (int _i = 0; _i < pEqData.Length; _i++)
				{
					_realEq[_i] = pEqData[_i].Real();
					_imagEq[_i] = pEqData[_i].Imag();
				}

				iqPlot.Draw(_real, _imag);
				iqPlotEqualized.Draw(_realEq, _imagEq);
			}
			else
				Invoke(new DrawIQDelegate(DrawIQ), pData, pEqData);
		}
	}
}
