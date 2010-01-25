using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPURPV18.SignalConverter
{
	public struct ConvertBand
	{
		public long RF_Min;
		public long RF_Max;
		public long IF_Min;
		public long IF_Max;
		public long LO;

		public ConvertBand(long _RF_Min, long _RF_Max, long _IF_Min, long _IF_Max, long _LO)
		{
			this.RF_Min = _RF_Min;
			this.RF_Max = _RF_Max;
			this.IF_Min = _IF_Min;
			this.IF_Max = _IF_Max;
			this.LO = _LO;
		}

		public bool FreqIn(long f)
		{
			return (f >= RF_Min && f <= RF_Max);
		}
	}
}
