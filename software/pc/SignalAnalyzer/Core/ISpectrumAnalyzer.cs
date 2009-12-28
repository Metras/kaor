using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalAnalyzer.Core
{
	public class CRBWValue
	{
		string Name { get; set; }
		float Value { get; set; }

		public CRBWValue(string pRBWName, float pRBWValue)
		{
			Name = pRBWName;
			Value = pRBWValue;
		}
	}

	public interface ISpectrumAnalyzer
	{
		/// <summary>
		/// Минимальная частота спектроанализатора
		/// </summary>
		long FMin { get; }

		/// <summary>
		/// Максимальная частота спектроанализатора
		/// </summary>
		long FMax { get; }

		/// <summary>
		/// Минимальный спан, в Гц
		/// </summary>
		long SpanMax { get; }

		/// <summary>
		/// Максимальный спан, в Гц
		/// </summary>
		long SpanMin { get; }

		/// <summary>
		/// Допустимые значения RBW
		/// </summary>
		List<CRBWValue> RBWValues { get; }

		double[] GetFFTData(long pFrequency);

		long BaseFreq { get; set; }
		long Span { get; set; }

		/// <summary>
		/// Разрешение по частоте (RBW)
		/// </summary>
		float FFTResolution { get; set; }
		float RBW { get; set; }

		/// <summary>
		/// Наверное это не надо
		/// </summary>
		long FFTSize { get; set; }

	}
}
