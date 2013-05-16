using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpintronicsGUI
{
	public enum PacketType : byte
	{
		Start = 0x00,
		StartReply = 0x80,
		Stop = 0x01,
		StopReply = 0x81,
		Report = 0x82,
		Error = 0x83,
		Config = 0x04,
		ConfigReply = 0x84,
	}

	public enum PacketLength
	{
		Start = 21,
		StartReply = 21,
		Stop = 0,
		StopReply = 0,
		Report = 41,
		Error = 1,
		// Config = 0xXX -> Config packet is the only one that is allowed to have a variable length
		ConfigReply = 0,
	}

	public enum ErrorTypes : byte
	{
		StartA1OutOfRange,
		StartF1OutOfRange,
		StartA2OutOfRange,
		StartF2OutOfRange,
		StartTOutOfRange,
		WheatstoneSignalClip,
		CoilSignalClip,
		WheatstoneDigitalClip,
		CoilDigitalClip
	}

	public class InvalidPacketFormatException : Exception
	{
		string GetMessage()
		{
			return "Packet was created with invalid parameters";
		}
	}

	public class Packet : GenericPacket
	{
		public const byte SOF = 0xFE;
		public byte Command
		{
			get;
			set;
		}
		public byte PayloadLength
		{
			get;
			set;
		}
		public byte[] Payload
		{
			get;
			set;
		}
		public byte Xor
		{
			get;
			set;
		}

		public Packet(byte Command, byte[] Payload)
		{
			this.Command = (byte)Command;
			this.PayloadLength = (byte)Payload.Length;
			this.Payload = Payload;
			ComputeXor();
		}

		public Packet(PacketType Command, byte[] Payload = null)
		{
			this.Command = (byte)Command;
			if (Payload != null)
			{
				this.PayloadLength = (byte)Payload.Length;
				this.Payload = Payload;
			}
			else
			{
				this.PayloadLength = 0;
			}
			ComputeXor();
		}

		public void ComputeXor()
		{
			this.Xor = 0x00;
			this.Xor ^= this.Command;
			this.Xor ^= this.PayloadLength;
			for (int i = 0; i < this.PayloadLength; i++)
			{
				Xor ^= this.Payload[i];
			}
		}
	}
}
