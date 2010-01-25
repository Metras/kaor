using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RPURPV18.SignalConverterManager
{
	public partial class InjectorConnectIndicator : UserControl
	{
		private Injector usedInjector = null;

		[Browsable(false)]
		public Injector UsedInjector
		{
			get
			{
				return usedInjector;
			}
			set
			{
				usedInjector = value;
				if (usedInjector != null)
					UsedInjector.StatusChanged += new EventHandler(Instance_StatusChanged);
			}
		}

		public InjectorConnectIndicator()
		{
			InitializeComponent();
			lblStatus.Text = I18N.Locale.injector_disconnected;
			picIndicator.Image = Properties.Resources.red_indicator;
			BackColor = Color.FromArgb(0, Color.White);
		}

		private void Instance_StatusChanged(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				Invoke(new EventHandler(Instance_StatusChanged), sender, e);
				return;
			}

			if (usedInjector.Status == null)
			{
				picIndicator.Image = Properties.Resources.gray_indicator;
				lblStatus.ForeColor = Color.Black;
				lblStatus.Text = I18N.Locale.injector_disconnected;
			}
			else
			{
				InjectorStatus status = usedInjector.Status.Value;
				if (usedInjector.Status.Value.IsError)
				{
					picIndicator.Image = Properties.Resources.red_indicator;
					lblStatus.ForeColor = Color.Red;
					lblStatus.Text = I18N.Locale.error + "! ";
					if (status.CurrentOverload)
						lblStatus.Text += I18N.Locale.injector_current_overload;
					else if (status.InputVoltageOver)
						lblStatus.Text += I18N.Locale.injector_input_voltage_over;
					else if (status.OutputCurrentOver)
						lblStatus.Text += I18N.Locale.injector_output_current_over;
					else if (status.OutputError)
						lblStatus.Text += I18N.Locale.injector_output_error;
				}
				else
				{
					if (status.PowerOn)
					{
						picIndicator.Image = Properties.Resources.green_bright_indicator;
						lblStatus.ForeColor = Color.Black;
						lblStatus.Text = I18N.Locale.injector_poweron;
					}
					else
					{
						picIndicator.Image = Properties.Resources.green_indicator;
						lblStatus.ForeColor = Color.Black;
						lblStatus.Text = I18N.Locale.injector_connected;
					}
				}
			}
		}
	}
}
