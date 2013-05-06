using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace SpintronicsGUI
{
	enum ProtocolState
	{
		Idle,
		ConfigSent,
		StartSent,
		StopSent,
		WaitForData,
		ErrorReceived
	}

	enum ProtocolDirective
	{
		DoNothing,
		AddData,
		ErrorReceived,
		WaitNotReady
	}

	enum ErrorCode : byte
	{
		NoError,
		BadPacketXor,
		A1OoR,
		F1OoR,
		A2OoR,
		F2OoR,
		F1F2OoR,
		TOoR,
		InvalidDigitalGain,
		SignalClipBridgeAtoD,
		SignalClipCoilAtoD,
		SignalClipBridgeDigitalGain
	}

	class ProtocolHandler
	{
		ProtocolState state;
		SerialPort serialPort;
		public byte errorCode;
		//System.Threading.Timer timer = new System.Threading.Timer(oneshotTimer);

		public ProtocolHandler(SerialPort port)
		{
			state = ProtocolState.Idle;
			this.serialPort = port;
		}

		public ProtocolDirective HandlePacket(Packet packetIn)
		{
			switch (state)
			{
				/*case ProtocolState.Idle:
					if (packetIn.command == ((byte)PacketType.Config | (byte)PacketSender.GUI))
					{
						writePacket(packetIn, serialPort);
						state = ProtocolState.ConfigSent;
					}
					if (packetIn.command == ((byte)PacketType.Start | (byte)PacketSender.GUI))
					{// If we're idle and waiting to be told to do something and we receive a start packet from the GUI
						writePacket(packetIn, serialPort);
						state = ProtocolState.StartSent;
					}
					break;*/

				case ProtocolState.ConfigSent:
					if (packetIn.command == ((byte)PacketType.Config | (byte)PacketSender.Microcontroller))
					{
						state = ProtocolState.Idle;
					}
					else if (packetIn.command == ((byte)PacketType.Error | (byte)PacketSender.Microcontroller))
					{
						this.errorCode = packetIn.payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;

				case ProtocolState.StartSent:
					if (packetIn.command == ((byte)PacketType.Start | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.WaitForData;
					}
					else if (packetIn.command == ((byte)PacketType.Error | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						this.errorCode = packetIn.payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;

				case ProtocolState.StopSent:
					if (packetIn.command == ((byte)PacketType.Stop | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.Idle;
					}
					else if (packetIn.command == ((byte)PacketType.Error | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						this.errorCode = packetIn.payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;

				case ProtocolState.WaitForData:
					if (packetIn.command == ((byte)PacketType.Report | (byte)PacketSender.Microcontroller))
					{// If we're waiting for data packets and we receive one from the microcontroller
						return ProtocolDirective.AddData;
					}
					else if (packetIn.command == ((byte)PacketType.Error | (byte)PacketSender.Microcontroller))
					{
						this.errorCode = packetIn.payload[0];
						return ProtocolDirective.ErrorReceived;
					}
					else if (packetIn.command == ((byte)PacketType.Stop | (byte)PacketSender.GUI))
					{// If we're waiting for data packets and we receive a stop packet from the GUI
						writePacket(packetIn, serialPort);
						state = ProtocolState.Idle;
					}
					break;

				/*case ProtocolState.ErrorReceived:
					MessageBox.Show("Error received!");
					break;*/

				default:
					break;
			}

			return ProtocolDirective.DoNothing;
		}

		void oneshotTimer(int milliseconds)
		{
		}

		public bool StartRun(Packet configPacket, Packet startPacket)
		{
			if (configPacket != null)
			{
				try {
					writePacket(configPacket, serialPort);
				} catch (Exception) {
					return false;
				}
				this.state = ProtocolState.ConfigSent;
				while (this.state == ProtocolState.ConfigSent);
				if (this.state == ProtocolState.ErrorReceived)
				{
					this.state = ProtocolState.Idle;
					return false;
				}
			}

			try {
				writePacket(startPacket, serialPort);
			} catch (Exception) {
				return false;
			}
			this.state = ProtocolState.StartSent;
			while (this.state == ProtocolState.StartSent);
			if (this.state == ProtocolState.ErrorReceived)
			{
				this.state = ProtocolState.Idle;
				return false;
			}

			return true;
		}

		public bool StopRun(Packet stopPacket)
		{
			try {
				writePacket(stopPacket, serialPort);
			} catch (Exception) {
				return false;
			}
			this.state = ProtocolState.StopSent;
			while (this.state == ProtocolState.StopSent);
			if (this.state == ProtocolState.ErrorReceived)
			{
				this.state = ProtocolState.Idle;
				return false;
			}

			return true;
		}

		public string getErrorMessage()
		{
			switch (this.errorCode)
			{
				case (byte)ErrorCode.BadPacketXor:
					return "Bad communication formatting";
				case (byte)ErrorCode.A1OoR:
					return "Wheatstone bridge signal amplitude is out of range";
				case (byte)ErrorCode.F1OoR:
					return "Wheatstone bridge signal frequency is out of range";
				case (byte)ErrorCode.A2OoR:
					return "Coil signal amplitude is out of range";
				case (byte)ErrorCode.F2OoR:
					return "Coil signal frequency is out of range";
				case (byte)ErrorCode.F1F2OoR:
					return "Wheatsone + coil frequency sum is out of range";
				case (byte)ErrorCode.TOoR:
					return "Frequency acquisition period out of range";
				case (byte)ErrorCode.InvalidDigitalGain:
					return "Invalid digital gain factor";
				case (byte)ErrorCode.SignalClipBridgeAtoD:
					return "Wheatstone bridge A-to-D signal is clipping";
				case (byte)ErrorCode.SignalClipCoilAtoD:
					return "Coil A-to-D signal is clipping";
				case (byte)ErrorCode.SignalClipBridgeDigitalGain:
					return "Wheatstone bridge digital gain is clipping";
			}
			return "Unknown error of type " + errorCode;
		}

		private void writePacket(Packet packetToWrite, SerialPort port)
		{
			byte[] buf = new byte[4 + packetToWrite.payloadLength];
			buf[0] = packetToWrite.SOF;
			buf[1] = packetToWrite.command;
			buf[2] = packetToWrite.payloadLength;
			if(packetToWrite.payloadLength != 0)
				Array.Copy(packetToWrite.payload, 0, buf, 3, packetToWrite.payloadLength);
			buf[3 + packetToWrite.payloadLength] = packetToWrite.Xor;
			try {
				port.Write(buf, 0, buf.Length);
			} catch (ArgumentNullException) {
				throw new ArgumentNullException();
			} catch (InvalidOperationException) {
				throw new InvalidOperationException();
			} catch (ArgumentOutOfRangeException) {
				throw new ArgumentOutOfRangeException();
			} catch (ArgumentException) {
				throw new ArgumentException();
			} catch (TimeoutException) {
				throw new TimeoutException();
			} catch (Exception) {
				throw new Exception();
			}
		}
	}
}
