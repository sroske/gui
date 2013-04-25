using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpintronicsGUI
{
	enum PacketSender : byte
	{
		GUI = 0x00,
		Microcontroller = 0x80
	}

	enum PacketType : byte
	{
		Start,
		Stop,
		Report,
		Error,
		Config
	}

	enum ErrorTypes : byte
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

	public class Packet
	{
		public byte SOF = 0xFE;
		public byte command;
		public byte payloadLength;
		public byte[] payload;
		public byte Xor;

		public Packet(byte cmd, byte length = 0, byte[] pay = null)
		{
			if (pay == null)
			{
				if (length != 0)
					throw new System.ArgumentException("Payload length parameter does not match actual payload length");
			}
			else if (length != pay.Length)
			{
				throw new System.ArgumentException("Payload length parameter does not match actual payload length");
			}

			command = cmd;
			payloadLength = length;
			payload = pay;
			Xor = 0x00;
			Xor ^= payloadLength;
			for (int i = 0; i < payloadLength; i++)
			{
				Xor ^= payload[i];
			}
		}
	}
}
