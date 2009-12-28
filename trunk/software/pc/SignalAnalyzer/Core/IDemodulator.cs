using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SignalAnalyzer.Core
{
	public interface IDemodulator
	{
		UserControl DemodulatorControl { get; }
		IDemodulatorInput DemodulatorInput { get; set; }
		void Start();
		void Stop();

	}
}
