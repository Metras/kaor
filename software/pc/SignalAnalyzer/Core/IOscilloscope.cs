using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalAnalyzer.Core
{
	/// <summary>
	/// Интерфейс осциллографа для анализатора сигналов
	/// </summary>
	public interface IOscilloscope
	{
		bool IsQuadrature { get; }

		int SamplesLength { get; set; }
		double[] GetData(long pFrequency);
		int SampleRate { get; set; }
		int SampleRateMin { get; }
		int SampleRateMax { get; }

		long FreqMin { get; }
		long FreqMax { get; }
	}
}
