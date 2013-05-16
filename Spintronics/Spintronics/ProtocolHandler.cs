using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace SpintronicsGUI
{
	enum ProtocolState
	{
		Idle,
		ConfigSent,
		StartSent,
		StopSent,
		WaitForData,
		ErrorReceived,
	}

	enum ProtocolDirective
	{
		DoNothing,
		AddData,
		ErrorReceived,
		WaitNotReady,
	}

	enum ErrorCode : byte
	{
		TimeOut				= 0x00,
		BadPacketXor			= 0x01,
		A1OoR					= 0x02,
		F1OoR					= 0x03,
		A2OoR					= 0x04,
		F2OoR					= 0x05,
		F1F2OoR				= 0x06,
		TOoR					= 0x07,
		InvalidDigitalGain		= 0x08,
		SignalClipBridgeAtoD		= 0x09,
		SignalClipCoilAtoD		= 0x0A,
		SignalClipBridgeDigitalGain	= 0x0B,
	}

	class ProtocolHandler
	{
		ProtocolState state;
		SerialPort serialPort;
		public byte errorCode;
		Timer timer;
		bool timerDone;

		public ProtocolHandler(SerialPort port)
		{
			state = ProtocolState.Idle;
			this.serialPort = port;
		}

		public ProtocolDirective HandlePacket(Packet packetIn)
		{
			switch (state)
			{
				case ProtocolState.ConfigSent:
					if (packetIn.Command == (byte)PacketType.ConfigReply)
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.Idle;
					}
					else if (packetIn.Command == (byte)PacketType.Error)
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						this.errorCode = packetIn.Payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;
				case ProtocolState.StartSent:
					if (packetIn.Command == (byte)PacketType.StartReply)
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.WaitForData;
					}
					else if (packetIn.Command == (byte)PacketType.Error)
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						this.errorCode = packetIn.Payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;
				case ProtocolState.StopSent:
					if (packetIn.Command == (byte)PacketType.StopReply)
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.Idle;
					}
					else if (packetIn.Command == (byte)PacketType.Error)
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						this.errorCode = packetIn.Payload[0];
						state = ProtocolState.ErrorReceived;
					}
					break;
				case ProtocolState.WaitForData:
					if (packetIn.Command == (byte)PacketType.Report)
					{// If we're waiting for data packets and we receive one from the microcontroller
						return ProtocolDirective.AddData;
					}
					else if (packetIn.Command == (byte)PacketType.Error)
					{
						this.errorCode = packetIn.Payload[0];
						return ProtocolDirective.ErrorReceived;	// This is the only case where we don't want to immediately set our state to something else; Instead, notify the GUI of the error
					}
					break;
				default:
					break;
			}

			return ProtocolDirective.DoNothing;
		}

		public void timerFinished(object arg)
		{
			this.timerDone = true;
		}

		public void oneshotTimer(int milliseconds)
		{
			this.timerDone = false;
			timer = new Timer(new TimerCallback(timerFinished));
			timer.Change(milliseconds, Timeout.Infinite);
		}

		public bool StartRun(ConfigPacket configPacket, StartPacket startPacket)
		{
			if (configPacket != null)									// If we need to send a configuration packet (by default, we always will),
			{
				try {
					writePacket<ConfigPacket>(configPacket, serialPort);					// try writing the packet
				} catch (Exception) {									// If something bad happens,
					this.errorCode = (byte)ErrorCode.TimeOut;					// set the error code to the ProtocolHandler-specific code,
					return false;									// and return false
				}
				this.state = ProtocolState.ConfigSent;						// Otherwise, set the state,
				oneshotTimer(2000);									// start the timer,
				while (!this.timerDone && this.state == ProtocolState.ConfigSent) ;	// and wait for either the timer to stop or the microcontroller to respond (state changes in HandlePacket)
				if (this.state == ProtocolState.ConfigSent)					// If we haven't changed states, that means we timed out,
				{
					this.state = ProtocolState.Idle;						// so set us back to idle,
					this.errorCode = (byte)ErrorCode.TimeOut;					// set the error code to the ProtocolHandler-specific code,
					return false;									// and return false
				}
				if (this.state == ProtocolState.ErrorReceived)					// If we received an error,
				{
					this.state = ProtocolState.Idle;						// set us back to idle,
					return false;									// and return false
				}
			}

			try {													// If we made it to here, that means we successfully sent the configuration packet
				writePacket<StartPacket>(startPacket, serialPort);						// Try writing the packet
			} catch (Exception) {										// If something bad happens,
				this.errorCode = (byte)ErrorCode.TimeOut;						// set the error code to the ProtocolHandler-specific code,
				return false;										// and return false
			}
			this.state = ProtocolState.StartSent;							// Otherwise, set the state,
			oneshotTimer(2000);										// start the timer,
			while (!this.timerDone && this.state == ProtocolState.StartSent) ;		// and wait for either the timer to stop or the microcontroller to respond
			if (this.state == ProtocolState.StartSent)						// If we haven't changed states, that means we timed out,
			{
				this.state = ProtocolState.Idle;							// so set us back to idle,
				this.errorCode = (byte)ErrorCode.TimeOut;						// set the error code to the ProtocolHander-specific code,
				return false;										// and return false
			}
			if (this.state == ProtocolState.ErrorReceived)						// If we received and error,
			{
				this.state = ProtocolState.Idle;							// set us back to idle,
				return false;										// and return false
			}

			return true;											// If we made it this far, that means we successfully sent both packets, so return true!
		}

		public bool StopRun(StopPacket stopPacket)
		{
			try {
				writePacket<StopPacket>(stopPacket, serialPort);						// Try to send the packet
			} catch (Exception) {										// If something bad happens,
				this.errorCode = (byte)ErrorCode.TimeOut;						// set the error code to the ProtocolHandler-specific code,
				return false;										// and return false
			}
			this.state = ProtocolState.StopSent;							// Otherwise, set the state,
			oneshotTimer(1000);										// start the timer,
			while (!this.timerDone && this.state == ProtocolState.StopSent) ;			// and wait for either the timer to stop or the microcontroller to respond
			if (this.state == ProtocolState.StopSent)							// If we haven't changed states, that means we timed out,
			{
				this.state = ProtocolState.Idle;							// so set us back to idle,
				this.errorCode = (byte)ErrorCode.TimeOut;						// set the error code to the ProtocolHander-specific code,
				return false;										// and return false
			}
			if (this.state == ProtocolState.ErrorReceived)						// If we received and error,
			{
				this.state = ProtocolState.Idle;							// set us back to idle,
				return false;										// and return false
			}

			return true;											// Otherwise, return true!
		}

		public string getErrorMessage()
		{
			switch (this.errorCode)
			{
				case (byte)ErrorCode.TimeOut:
					return "Microcontroller failed to respond";
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

		private void writePacket<T>(T packetToWrite, SerialPort port) where T : GenericPacket
		{
			byte[] buf = new byte[4 + packetToWrite.PayloadLength];
			buf[0] = Packet.SOF;
			buf[1] = packetToWrite.Command;
			buf[2] = packetToWrite.PayloadLength;
			if(packetToWrite.PayloadLength != 0)
				Array.Copy(packetToWrite.Payload, 0, buf, 3, packetToWrite.PayloadLength);
			buf[3 + packetToWrite.PayloadLength] = packetToWrite.Xor;
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
