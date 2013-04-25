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
		AddData
	}

	class ProtocolHandler
	{
		ProtocolState state;

		public ProtocolHandler()
		{
			state = ProtocolState.Idle;
		}

		public ProtocolDirective HandlePacket(Packet packetIn, SerialPort serialPort, Chart chart = null, bool forceSend = false)
		{
			if (forceSend)
			{
				writePacket(packetIn, serialPort);
				return ProtocolDirective.DoNothing;
			}

			switch (state)
			{
				case ProtocolState.Idle:
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
					break;

				case ProtocolState.ConfigSent:
					if (packetIn.command == ((byte)PacketType.Config | (byte)PacketSender.Microcontroller))
					{
						state = ProtocolState.Idle;
					}
					break;

				case ProtocolState.StartSent:
					if (packetIn.command == ((byte)PacketType.Start | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.WaitForData;
					}
					else if (packetIn.command == ((byte)PacketType.Error | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive an error packet from the microcontroller
						state = ProtocolState.ErrorReceived;
					}
					break;

				case ProtocolState.StopSent:
					if (packetIn.command == ((byte)PacketType.Stop | (byte)PacketSender.Microcontroller))
					{// If we're waiting for an acknowledge and we receive one from the microcontroller
						state = ProtocolState.Idle;
					}
					break;

				case ProtocolState.WaitForData:
					if (packetIn.command == ((byte)PacketType.Report | (byte)PacketSender.Microcontroller))
					{// If we're waiting for data packets and we receive one from the microcontroller
						byte[] payload = new byte[1];
						payload[0] = packetIn.payload[0];
						Packet ackPacket = new Packet((byte)PacketType.Report | (byte)PacketSender.GUI, 1, payload);
						writePacket(ackPacket, serialPort);
						return ProtocolDirective.AddData;
					}
					else if (packetIn.command == ((byte)PacketType.Stop | (byte)PacketSender.GUI))
					{// If we're waiting for data packets and we receive a stop packet from the GUI
						writePacket(packetIn, serialPort);
						state = ProtocolState.StopSent;
					}
					break;

				case ProtocolState.ErrorReceived:
					MessageBox.Show("Error received!");
					break;

				default:
					break;
			}
			return ProtocolDirective.DoNothing;
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
			try
			{
				port.Write(buf, 0, buf.Length);
			} catch (ArgumentNullException) {
				//throw new ArgumentNullException();
			} catch (InvalidOperationException) {
				//throw new InvalidOperationException();
			} catch (ArgumentOutOfRangeException) {
				//throw new ArgumentOutOfRangeException();
			} catch (ArgumentException) {
				//throw new ArgumentException();
			} catch (TimeoutException) {
				//throw new TimeoutException();
			} catch (Exception) {
				//throw new Exception();
			}
		}

		private void addData(byte[] dataToAdd, int time, Chart chart = null)
		{
			int sensorId = dataToAdd[0];
			float wheatstonef1A = System.BitConverter.ToSingle(dataToAdd, 1);
			float wheatstonef1P = System.BitConverter.ToSingle(dataToAdd, 5);
			float wheatstonef2A = System.BitConverter.ToSingle(dataToAdd, 9);
			float wheatstonef2P = System.BitConverter.ToSingle(dataToAdd, 13);
			float wheatstonef1Mf2A = System.BitConverter.ToSingle(dataToAdd, 17);
			float wheatstonef1Mf2P = System.BitConverter.ToSingle(dataToAdd, 21);
			float wheatstonef1Pf2A = System.BitConverter.ToSingle(dataToAdd, 25);
			float wheatstonef1Pf2P = System.BitConverter.ToSingle(dataToAdd, 29);
			float wheatstoneCoilf2A = System.BitConverter.ToSingle(dataToAdd, 33);
			float wheatstoneCoilf2P = System.BitConverter.ToSingle(dataToAdd, 37);
			if (chart != null)
				chart.Series[sensorId - 1].Points.AddXY(time, wheatstonef1A);
			// Write data to text file, too
		}
	}
}
