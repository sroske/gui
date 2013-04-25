using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace SpintronicsGUI
{
	enum MicrocontrollerState
	{
		Idle,
		SendingData,
		SendingError
	}

	class Microcontroller
	{
		SerialPort serialPort;
		MicrocontrollerState state;
		byte errorSent = 0xFF;
		byte sensor = 0x00;
		int dataSpeed, sensorCount;
		double[] baseData = { 0.0, 0.4, 0.8, 1.2, 1.6, 2.0 };

		public Microcontroller(SerialPort port, int speed = 1000, int count = 30)
		{
			serialPort = port;
			serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
			state = MicrocontrollerState.Idle;
			dataSpeed = speed;
			sensorCount = count;
		}

		private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs args)
		{
			Packet packet = null;
			try
			{
				System.Threading.Thread.Sleep(100);
				byte startOfFrame = (byte)serialPort.ReadByte();
				if (startOfFrame != 0xFE)
				{
					return;
				}
				byte command = (byte)serialPort.ReadByte();
				byte payloadLength = (byte)serialPort.ReadByte();
				byte[] payload = new byte[payloadLength];
				if (serialPort.Read(payload, 0, payloadLength) < payloadLength)
				{
					return;
				}
				byte Xor = (byte)serialPort.ReadByte();
				packet = new Packet(command, payloadLength, payload);
				if (packet.Xor != Xor)
				{
					return;
				}
			}
			catch (TimeoutException)
			{
				return;
			}

			if (packet == null)
				return;

			switch (state)
			{
				case MicrocontrollerState.Idle:
					if (packet.command == ((byte)PacketType.Start | (byte)PacketSender.GUI))
					{// If we're idle and waiting to be told to do something and we receive a start packet from the GUI
						Packet startReplyPacket = new Packet(((byte)PacketSender.Microcontroller | (byte)PacketType.Start), (byte)packet.payload.Length, packet.payload);
						writePacket(startReplyPacket);
						Thread.Sleep(dataSpeed);
						writePacket(createDataPacket());
						state = MicrocontrollerState.SendingData;
					}
					break;

				case MicrocontrollerState.SendingData:
					if ((packet.command == ((byte)PacketType.Report | (byte)PacketSender.GUI)) &&
						(packet.payload[0] == sensor))
					{// If we're sending data packets and we receive an acknowledge from the GUI
						Thread.Sleep(dataSpeed);
						writePacket(createDataPacket());
					}
					if (packet.command == ((byte)PacketType.Stop | (byte)PacketSender.GUI))
					{// If we're sending data packets and we receive a stop packet from the GUI
						Packet stopReplyPacket = new Packet(((byte)PacketSender.Microcontroller | (byte)PacketType.Stop));
						writePacket(stopReplyPacket);
						state = MicrocontrollerState.Idle;
						sensor = 0x00;
					}
					break;

				case MicrocontrollerState.SendingError:
					if (packet.command == ((byte)PacketType.Error | (byte)PacketSender.GUI))
					{// If we're waiting for an error acknowledge packet and we receive one from the GUI
						if ((packet.payloadLength == 1) && (packet.payload[0] == errorSent))
								state = MicrocontrollerState.Idle;
					}
					break;

				default:
					break;
			}
		}

		private void writePacket(Packet packetToWrite)
		{
			byte[] buf = new byte[4 + packetToWrite.payloadLength];
			buf[0] = packetToWrite.SOF;
			buf[1] = packetToWrite.command;
			buf[2] = packetToWrite.payloadLength;
			//if(packetToWrite.payloadLength != 0)
			//	Array.Copy(packetToWrite.payload, 0, buf, 3, packetToWrite.payloadLength);
			for (int i = 0; i < packetToWrite.payloadLength; i++)
			{
				buf[i + 3] = packetToWrite.payload[i];
			}
			try
			{
				buf[3 + packetToWrite.payloadLength] = packetToWrite.Xor;
				if (serialPort != null)
					serialPort.Write(buf, 0, buf.Length);
			} catch (ArgumentNullException) {

			} catch (InvalidOperationException) {
				
			} catch (ArgumentOutOfRangeException) {

			} catch (ArgumentException) {

			} catch (TimeoutException) {

			} catch (Exception) {

			}
		}

		private Packet createDataPacket()
		{
			sensor++;
			if (sensor > sensorCount)
				sensor = 1;
			if (sensor == 16)
				sensor++;

			int baseSensor;
			if (sensor <= 5)
			{
				baseSensor = 0;
			}
			else if (sensor <= 10)
			{
				baseSensor = 1;
			}
			else if (sensor <= 15)
			{
				baseSensor = 2;
			}
			else if (sensor <= 20)
			{
				baseSensor = 3;
			}
			else if (sensor <= 25)
			{
				baseSensor = 4;
			}
			else if (sensor <= 30)
			{
				baseSensor = 5;
			}
			else
			{
				baseSensor = 0;
			}

			float[] data = new float[10];
			Random random = new Random();
			for(int i = 0; i < 10; i++)
			{
				data[i] = (float)baseData[baseSensor];
				data[i] += (float)(random.NextDouble() % 0.2);
			}
			/*data[0] = 1.0F;
			data[1] = 1.0F;
			data[2] = 1.0F;
			data[3] = 1.0F;
			data[4] = 1.0F;
			data[5] = 1.0F;
			data[6] = 1.0F;
			data[7] = 1.0F;
			data[8] = 1.0F;
			data[9] = 1.0F;*/
			byte[] payload = new byte[41];
			payload[0] = sensor;
			Buffer.BlockCopy(data, 0, payload, 1, payload.Length - 1);
			Packet dataPacket = new Packet((byte)PacketSender.Microcontroller | (byte)PacketType.Report, (byte)payload.Length, payload);
			return dataPacket;
		}
	}
}
