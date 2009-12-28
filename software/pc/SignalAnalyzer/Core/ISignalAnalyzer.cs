using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalAnalyzer.Core
{
	public interface ISignalAnalyzer
	{
		ISpectrumAnalyzer SpectrumAnalyzer { get; }
		IOscilloscope Oscilloscope { get; }
		IVectorAnalyzer VectorAnalyzer { get; }
	}
}
