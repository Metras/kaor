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

//#define RPV_DEBUG

using System;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Timers;
using System.Runtime.InteropServices;

using KaorCore.Interfaces;
using KaorCore.Antenna;
using KaorCore.AntennaManager;
using KaorCore.RPU;
using KaorCore.Signal;
using KaorCore.Task;

using RPURPV3.Audio;
using RPURPV3.I18N;
using RPURPV3.PowerMeter;

namespace RPURPV3
{
	public partial class CRPURPV3 : BaseRPU
	{
		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct RPV_CmdOnOff
		{
			private Byte Cmd;
			private Byte OnOff;
			public RPV_CmdOnOff(bool PowerOn)
			{
				Cmd = 0x01;
				OnOff = PowerOn ? (Byte)0x01 : (Byte)0x00;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetPortSpeed
		{
			private byte Cmd;
			private byte Speed;
			private byte Reserved;
			public RPV_CmdSetPortSpeed(Int32 PortSpeed)
			{
				Cmd = 0x03;
				switch (PortSpeed)
				{
					case 9600:
						Speed = 0;
						break;
					case 19200:
						Speed = 1;
						break;
					case 38400:
						Speed = 2;
						break;
					case 56000:
						Speed = 3;
						break;
					case 57600:
						Speed = 4;
						break;
					case 76800:
						Speed = 5;
						break;
					case 128000:
						Speed = 6;
						break;
					default:
						throw new ArgumentException("Frequency not supported");
				}
				Reserved = 0x00;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdGetConfig
		{
			private Byte Cmd;
			public RPV_CmdGetConfig(Int32 Dummy)
			{
				Cmd = 0x05;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetFreq
		{
			private Byte Cmd;
			private UInt32 Freq;
			public RPV_CmdSetFreq(UInt32 Frequency)
			{
				this.Cmd = 0x10;
				this.Freq = Frequency;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetNoiseThreshold
		{
			private Byte Cmd;
			private SByte Level;
			public RPV_CmdSetNoiseThreshold(SByte Level)
			{
				Cmd = 0x11;
				this.Level = Level;
			}
		}

		public enum RPV_DMType { LSB, USB, AM, CW, FM, OFF };

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetDemodulation
		{
			private Byte Cmd;
			private Byte dmType;
			private Byte dmFilter;
			public RPV_CmdSetDemodulation(RPV_DMType Demodulation, Int32 Filter)
			{
				Cmd = 0x12;
				switch(Demodulation)
				{
					case RPV_DMType.LSB:
						dmType = 0;
						break;
					case RPV_DMType.USB:
						dmType = 1;
						break;
					case RPV_DMType.AM:
						dmType = 2;
						break;
					case RPV_DMType.CW:
						dmType = 3;
						break;
					case RPV_DMType.FM:
						dmType = 4;
						break;
					case RPV_DMType.OFF:
						dmType = 255;
						break;
					default:
						throw new Exception("Invalid demodulation");
				}
				switch(Filter)
				{
					case 3000:
						dmFilter = 0;
						break;
					case 6000:
						dmFilter = 1;
						break;
					case 15000:
						dmFilter = 2;
						break;
					case 50000:
						dmFilter = 3;
						break;
					case 230000:
						dmFilter = 4;
						break;
					default:
						throw new Exception("Invalid filter band");
				}
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetAttenuator
		{
			private Byte Cmd;
			private Byte AttHF;
			private Byte AttMF;
			public RPV_CmdSetAttenuator(Byte AttHF, Byte AttMF)
			{
				Cmd = 0x13;
				this.AttHF = AttHF;
				this.AttMF = AttMF;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSetVolume
		{
			private Byte Cmd;
			private Byte VolLeft;
			private Byte VolRight;
			public RPV_CmdSetVolume(double VolL, double VolR)
			{
				Cmd = 0x15;
				this.VolLeft = (Byte)(VolL * 63);
				this.VolRight = (Byte)(VolR * 63);
			}
		}

		public enum RPV_AntennaSource { A1, A2 };
		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdSelectAntenna
		{
			private Byte Cmd;
			private Byte AntSrc;
			public RPV_CmdSelectAntenna(int Src)
			{
				Cmd = 0x16;
				switch(Src)
				{
					case 1:
						AntSrc = 0x00;
						break;
					case 2:
						AntSrc = 0x01;
						break;
					default:
						throw new Exception("Invalid antenna source");
				}
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdModeRcv
		{
			private Byte Cmd;
			public RPV_CmdModeRcv(int Dummy)
			{
				Cmd = 0x20;
			}
		}

		public enum RPV_PwrMode { AvgDSP, Min, Max, MinMax, AvgTime, AvgCounts, AvgTimeCounts };

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdModePwr
		{
			private Byte Cmd;
			private Byte mode;
			private Byte filter;
			private UInt16 avgTime;
			private Int16 pigelPwrLevel;
			private Byte pigelSmooth;
			public RPV_CmdModePwr(RPV_PwrMode Mode, Int32 Filter, UInt16 AvgTime)
			{
				Cmd = 0x21;
				mode = (Byte)Mode;
				switch (Filter)
				{
					case 1000:
						filter = 1;
						break;
					case 3000:
						filter = 2;
						break;
					case 7500:
						filter = 3;
						break;
					case 10000:
						filter = 4;
						break;
					case 30000:
						filter = 5;
						break;
					case 100000:
						filter = 6;
						break;
					case 120000:
						filter = 8;
						break;
					case 280000:
						filter = 7;
						break;
					default:
						throw new Exception("Invalid power meter filter");
				}
				avgTime = AvgTime;
				pigelPwrLevel = 100;
				pigelSmooth = 0;
			}
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdModeSpk
		{

			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_CmdModeScan
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsHWErr
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsPower
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsSpector
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsScan
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsMode
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsVoltage
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsConfig
		{
			public Byte CmdCode;
		}

		/// <summary>
		/// -
		/// </summary>
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RPV_AnsCPULoad
		{
			public Byte CmdCode;
		}

		private Byte[] TxBuffer = new Byte[260];
		private Byte[] RxBuffer = new Byte[260];
		private Int32 RxBuffBytes = 0;
		private Int32 RxPktSize = -1;

		private IntPtr TempRx;
		private IntPtr TempTx;

		private Int32 BytesTx = 0;
		private Int32 CmdCount = 0;
		private Int32 AnsCount = 0;

		/// <summary>
		/// Событие получения ответа на команду
		/// </summary>
		ManualResetEvent evtCommandAck;

		//System.Timers.Timer RxTimer = new System.Timers.Timer(1000);


		private void DumpTxPacket(Byte[] Pkt)
		{
			Console.WriteLine(" -> New command sent form tread {0}:", Thread.CurrentThread.ManagedThreadId);
			Console.Write(" -> Code: 0x{0}  Data: ", Pkt[0].ToString("X2"));
			for (Int32 I = 1; I < Pkt.Length; I++)
			{
				Console.Write("0x{0} ", Pkt[I].ToString("X2"));
			}
			Console.WriteLine();
		}

#if DEBUG
		object logLock = new object();
		private void LogPort(byte[] pBuf, int pLength, bool pIsOut)
		{
			lock (logLock)
			{
				FileStream _f = new FileStream(Application.StartupPath + "\\rpv3.log", FileMode.Append);
				StreamWriter _sw = new StreamWriter(_f);

				try
				{
					if (pIsOut)
						_sw.Write("-->");
					else
						_sw.Write("<--");

					_sw.WriteLine(DateTime.Now.ToString());
					for (int _i = 0; _i < pLength; _i++)
					{
						if (_i != 0 && _i % 16 == 0)
						{
							_sw.WriteLine();
						}

						string _s = String.Format("{0:X2} ", pBuf[_i]);
						_sw.Write(_s);
					}
				}
				catch (Exception _ex)
				{
					_sw.WriteLine(_ex.Message);
				}
				_sw.WriteLine();
				_sw.Close();
				_f.Close();
			}
		}
#endif

		public void SendCommand(Byte[] Cmd)
		{
			Byte Checksum = 0;
			Int32 I;
			int _tryCount;


			if (port == null || !port.IsOpen)
				return;

			// DumpTxPacket(Cmd);

			lock (port)
			{
				unchecked
				{
					for (I = 0; I < Cmd.Length; I++)
						Checksum += Cmd[I];
					Checksum += (Byte)Cmd.Length;
				}

				TxBuffer[0] = 0xAA;
				TxBuffer[1] = (Byte)Cmd.Length;
				TxBuffer[Cmd.Length + 2] = Checksum;
				Cmd.CopyTo(TxBuffer, 2);
#if DEBUG
				LogPort(TxBuffer, Cmd.Length + 3, true);
#endif
				evtCommandAck.Reset();
				if ((port != null) && (port.IsOpen))
				{
					for (_tryCount = 0; _tryCount < 3; _tryCount++)
					{
						port.Write(TxBuffer, 0, Cmd.Length + 3);

						//while (port.BytesToWrite != 0) Thread.Sleep(1);
						BytesTx += (Cmd.Length + 3);
						CmdCount++;
						if (evtCommandAck.WaitOne(500, true))
							break;
					}
					if(_tryCount == 5)
						throw new Exception("Send command timeout");
				}
				else
					Console.WriteLine("Can't send command");
			}
		}

		public void SendCommand(System.ValueType CmdStruct)
		{
			Int32 Sz = Marshal.SizeOf(CmdStruct);
			Marshal.StructureToPtr(CmdStruct, TempTx, true);
			Byte[] Buf = new Byte[Sz];
			Marshal.Copy(TempTx, Buf, 0, Sz);
			SendCommand(Buf);
		}

		private void DumpRxPacket(Byte[] Pkt)
		{
			Console.WriteLine(" <- New Packet received:");
			Console.Write(" <- Code: 0x{0}  Data: ", Pkt[0].ToString("X2"));
			for (Int32 I = 1; I < (Pkt.Length - 1); I++)
			{
				Console.Write("0x{0} ", Pkt[I].ToString("X2"));
			}
			Console.WriteLine();
		}

		private void BindPktToStruct(Byte[] Pkt, System.ValueType Str)
		{
			if (Pkt.Length != Marshal.SizeOf(Str))
				throw new Exception("Invalid structure size");
			
		}

		void PacketReceived(Byte[] Pkt)
		{
			// DumpRxPacket(Pkt);
			switch (Pkt[0])
			{
				case 0x80:
					evtCommandAck.Set();
					//Console.WriteLine("Answer: OK");
					AnsCount++;
					break;
				case 0x81:
					evtCommandAck.Set();
					Console.WriteLine("Error: Unknown command");
					break;
				case 0x82:
					evtCommandAck.Set();
					Console.WriteLine("Error: Command not allowed");
					break;
				case 0x83:
					evtCommandAck.Set();
					Console.WriteLine("Error: Bad params");
					break;
				case 0x84:
					evtCommandAck.Set();
					Console.WriteLine("Error: Bad data block");
					break;
				
				case 0xa1:
					evtCommandAck.Set();
					Console.WriteLine("RPU already ON");
					break;

				case 0x90:
					powerMeter.ProcessPkt(Pkt);
					break;
				default:
					Console.WriteLine("Error: Unknown answer 0x{0}", Pkt[0].ToString("X2"));
					break;
			}
		}

		void RxTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			lock (port)
			{
				Console.WriteLine("TX: {0}  Cmd: {1}/{2}", BytesTx * 10, CmdCount, AnsCount);
				BytesTx = 0;
			}
		}

		byte[] tmpRxBuf = new byte[16384];

		void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			Byte[] Pkt;
			int _bytesRx, _i;
			byte _tmp;

			//lock (port)
			{
				if (!port.IsOpen)
					return;

				while (port.IsOpen && port.BytesToRead > 0)
				{
					try
					{
						_bytesRx = port.Read(tmpRxBuf, 0, port.BytesToRead);
					}
					catch
					{
						return;
					}

					for (_i = 0; _i < _bytesRx; _i++)
					{
						_tmp = tmpRxBuf[_i];

						// Ожидается начало пакета
						if (RxPktSize == -1)
						{
							RxBuffer[0] = _tmp;

							// Если принятый байт не является заголовком, он отбрасывается
							if (RxBuffer[0] != 0xAA)
							{
								Console.WriteLine("Invalid RX header");
								continue;
							}
							else
								RxPktSize = 0;
						}
						// Ожидается размер пакета
						else if (RxPktSize == 0)
						{
							RxPktSize = _tmp;
							RxBuffBytes = 0;
						}
						// Принят последний байт пакета
						else if (RxBuffBytes == RxPktSize)
						{
							// Считать байт контрольной суммы
							RxBuffer[RxBuffBytes] = _tmp;
							RxBuffBytes++;
							Pkt = new Byte[RxBuffBytes];
							Array.Copy(RxBuffer, Pkt, RxBuffBytes);
							RxBuffBytes = 0;
							RxPktSize = -1;

#if DEBUG
							LogPort(Pkt, Pkt.Length, false);
#endif
							PacketReceived(Pkt);
							Pkt = null;
						}
						else
						{
							RxBuffer[RxBuffBytes] = _tmp;
							RxBuffBytes++;
						}
					}
				}
			}
#if false
			while (port.BytesToRead > 0)
			{
				// Ожидается начало пакета
				if (RxPktSize == -1)
				{
					port.Read(RxBuffer, 0, 1);
					// Если принятый байт не является заголовком, он отбрасывается
					if (RxBuffer[0] != 0xAA)
					{
						Console.WriteLine("Invalid RX header");
						continue;
					}
					else
						RxPktSize = 0;
				}
				// Ожидается размер пакета
				else if (RxPktSize == 0)
				{
					port.Read(RxBuffer, 0, 1);
					RxPktSize = RxBuffer[0];
					RxBuffBytes = 0;
				}
				// Принят последний байт пакета
				else if (RxBuffBytes == RxPktSize)
				{
					// Считать байт контрольной суммы
					port.Read(RxBuffer, RxBuffBytes, 1);
					RxBuffBytes++;
					Pkt = new Byte[RxBuffBytes];
					Array.Copy(RxBuffer, Pkt, RxBuffBytes);
					RxBuffBytes = 0;
					RxPktSize = -1;
					PacketReceived(Pkt);
				}
				else
				{
					port.Read(RxBuffer, RxBuffBytes, 1);
					RxBuffBytes++;
				}
			}
#endif
		}

		public void SetDemodulation(String DmStr, Int32 Band)
		{
			if (Mode == RPUMode.Off)
				return;

			RPV_DMType _dm;
			switch(DmStr)
			{
				case "AM":
					_dm = RPV_DMType.AM;
					break;
				case "CW":
					_dm = RPV_DMType.CW;
					break;
				case "FM":
					_dm = RPV_DMType.FM;
					break;
				case "LSB":
					_dm = RPV_DMType.LSB;
					break;
				case "USB":
					_dm = RPV_DMType.USB;
					break;
				case "OFF":
					_dm = RPV_DMType.OFF;
					break;
				default:
					throw new Exception(String.Format("Invalid DmStr: {0}", DmStr));
			}
			SendCommand(new RPV_CmdSetDemodulation(_dm, Band));
		}



	

	}
}
