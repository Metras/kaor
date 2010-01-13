// Copyright (c) 2009 CJSC NII STT (http://www.niistt.ru) and the 
// individuals listed on the AUTHORS entries.
// All rights reserved.
//
// Authors: 
//          Valentin Yakovenkov <yakovenkov@niistt.ru>
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1.  Redistributions of source code must retain the above copyright
//     notice, this list of conditions and the following disclaimer.
// 2.  Redistributions in binary form must reproduce the above copyright
//     notice, this list of conditions and the following disclaimer in the
//     documentation and/or other materials provided with the distribution.
// 3.  Neither the name of CJSC NII STT ("NII STT") nor the names of
//     its contributors may be used to endorse or promote products derived
//     from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY NII STT AND ITS CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL NII STT OR ITS CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
// OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Xml;

using KaorCore.I18N;

namespace KaorCore.Base
{
	public enum EGPSEastWest
	{
		East,
		West
	}

	public enum EGPSNorthSouth
	{
		North,
		South
	}

	/// <summary>
	/// Географические координаты точки 
	/// </summary>
	/// 

	public class GPSCoordinates : ICloneable
	{

		private double lon;
		private double lat;
		//private EGPSEastWest ew;
		//private EGPSNorthSouth sn;
		private double alt;

		bool readOnly;

		#region ================ Конструктор ================
		public GPSCoordinates()
		{
			readOnly = false;
		}

		public GPSCoordinates(double pLat, double pLon, 
			double pAlt, bool pReadonly)
		{
			lon = pLon;
			lat = pLat;
			//sn = pSN;
			//ew = pEW;
			alt = pAlt;
			readOnly = false;
		}

		#endregion
		
		#region ================ Проперти ================
		public double Lon
		{
			get 
			{ 
				return lon; 
			}
			set 
			{ 
				if(!readOnly)
					lon = value; 
			}
		}

		public double Lat
		{
			get 
			{ 
				return lat; 
			}
			set 
			{ 
				if(!readOnly)
					lat = value; 
			}
		}
#if false
		public EGPSEastWest EW
		{
			get 
			{
				if (lon > 0)
					return EGPSEastWest.East;
				else
					return EGPSEastWest.West;
			}
			set 
			{ 
				if(!readOnly)
					ew = value; 
			}
		}

		public EGPSNorthSouth SN
		{
			get 
			{
				if (lat > 0)
					return EGPSNorthSouth.North;
				else
					return EGPSNorthSouth.South;
			}
			set 
			{ 
			}
		}
#endif
		public double Alt
		{
			get { return alt; }
			set 
			{
				if (!readOnly)
					alt = value;
			}
		}

		//public bool ReadOnly
		//{
		//    get { return readOnly; }
		//    set
		//    {
		//        if (!readOnly)
		//            readOnly = value;
		//    }
		//}

		#endregion

		#region ================ ICloneable Members ================

		public object Clone()
		{
			GPSCoordinates _coord = new GPSCoordinates(this.lat, this.lon,
				this.alt, false);

			return _coord;
		}

		#endregion

		public void SaveToXmlWriter(XmlWriter pWriter)
		{
			pWriter.WriteElementString("Lat", lat.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("Lon", lon.ToString(CultureInfo.InvariantCulture));
			pWriter.WriteElementString("Alt", alt.ToString(CultureInfo.InvariantCulture));
					
		}

		#region ================ Статические методы ================
		public static GPSCoordinates FromXmlNode(XmlNode pNode)
		{
			XmlNode _node;
			double _lon = 0.0, _lat = 0.0, _alt = 0.0;
			GPSCoordinates _coord;

			string _t;

			if (pNode != null)
			{
				_node = pNode.SelectSingleNode("Lat");
				if (_node != null)
					Double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _lat);

				_node = pNode.SelectSingleNode("Lon");
				if (_node != null)
					Double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _lon);

				_node = pNode.SelectSingleNode("Alt");
				if (_node != null)
					Double.TryParse(_node.InnerText, NumberStyles.Float, CultureInfo.InvariantCulture, out _alt);
			}
			_coord = new GPSCoordinates(_lat, _lon, _alt, false);

			return _coord;

		}

		public override string ToString()
		{
			return String.Format("{0}: {1}, {2}: {3}, {4}: {5}", Locale.lat, lat, 
				Locale.lon, lon, Locale.alt, alt);
		}
		#endregion
	}
}
