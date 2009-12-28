using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalAnalyzer.Core
{
	public interface IVectorAnalyzer
	{
		void GetIQData(long pFrequency, ref double[] pI, ref double[] pQ);
		int IQDataLength { get; set; }

		long FreqMin { get; }
		long FreqMax { get; }
	}
}
