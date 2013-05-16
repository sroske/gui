using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpintronicsGUI
{
	class StartPacket : GenericPacket
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

		public StartPacket(byte[] Payload)
		{
			if (Payload == null || Payload.Length != (int)PacketLength.Start)
			{
				throw new InvalidPacketFormatException();
			}

			this.Command = (byte)PacketType.Start;
			this.PayloadLength = (byte)PacketLength.Start;
			this.Payload = Payload;
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
