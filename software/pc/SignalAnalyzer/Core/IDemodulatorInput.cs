using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SignalAnalyzer.Core
{
	public delegate void DataChunkReceivedHandler(double[] pI, double[] pQ, bool pOverflow);
	public delegate void DemodulatorErrorHandler(int pErrorCode);

	public interface IDemodulatorInput
	{
		int DataChunkSize { get; set; }
		event DataChunkReceivedHandler OnDataChunkReceived;
		event DemodulatorErrorHandler OnDemodulatorError;

		long BaseFreq { get; set; }
		long FreqMin { get; }
		long FreqMax { get; }

		int SampleRate { get; set; }

		void StartReceiveData();
		bool IsRunning { get; }
		void StopReceiveData();
	}
}
