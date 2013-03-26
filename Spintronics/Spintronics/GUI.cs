using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpintronicsGUI
{
	enum PacketCommDirection
	{
		In,
		Out
	}

	public partial class GUI : Form
	{
		SerialPort serialPort = null;
		SerialPort debugSerial = null;
		ProtocolHandler protocolHandler = new ProtocolHandler();
		Microcontroller microcontroller;
		TextWriter logFile = null;
		TextWriter dataLogFile = null;
		bool printLogText = true;
		public delegate void addNewDataPoint(Packet packet);
		public addNewDataPoint myDelegate;

		int globalTime = 1;

		public GUI()
		{
			InitializeComponent();

			/* 
			 * Try to create logs folder and log file
			*/
			try
			{
				System.IO.Directory.CreateDirectory("./logs");
				string logFileName = "./logs/";
				logFileName += DateTime.Now.Year + "-";
				logFileName += DateTime.Now.Month + "-";
				logFileName += DateTime.Now.Day + "__";
				logFileName += DateTime.Now.Hour + "-";
				logFileName += DateTime.Now.Minute + "-";
				logFileName += DateTime.Now.Second;
				logFileName += ".txt";
				logFile = new StreamWriter(logFileName);
				writeToFile(logFile, "Created log file");
			} catch (Exception) {
				MessageBox.Show("Unable to create log file");
				logFile = null;
			}

			/* 
			 * Try to create data folder and data file.
			 * If unsuccessful, prompt user and let them
			 * Decide if the want to keep going without
			 * logging data to a text file.
			*/
			try
			{
				System.IO.Directory.CreateDirectory("./data");
				string logFileName = "./data/";
				logFileName += DateTime.Now.Year + "-";
				logFileName += DateTime.Now.Month + "-";
				logFileName += DateTime.Now.Day + "__";
				logFileName += DateTime.Now.Hour + ".";
				logFileName += DateTime.Now.Minute + ".";
				logFileName += DateTime.Now.Second;
				logFileName += ".txt";
				dataLogFile = new StreamWriter(logFileName);
				writeToFile(dataLogFile, "Created data log file");
			} catch (Exception) {
				DialogResult messageBoxResult = MessageBox.Show("Unable to create file for recording data. Continue?", "Error", MessageBoxButtons.YesNo);
				if (messageBoxResult != DialogResult.Yes)
					throw new Exception();
			}

			/*
			 * Initialize delegate for adding new data to the graph (this is because of stupid threading stuff; just nod and move on)
			*/
			myDelegate = new addNewDataPoint(addNewDataPointMethod);

			for (int i = 0; i < chart1.Series.Count; i++)
			{
				foreach (TabPage t in this.tabControl1.Controls)
				{
					foreach (Chart c in t.Controls)
					{
						c.Series[i].Points.AddXY(0, 0);
					}
				}
			}

			/*
			 * Initialize COM ports
			*/
			serialPort = new SerialPort("COM5", 115200);
			debugSerial = new SerialPort("COM6", 115200);
			serialPort.ReadTimeout = 200;
			debugSerial.ReadTimeout = 200;

			/*
			 * Open COM ports 
			*/
			try
			{
				serialPort.Open();
				debugSerial.Open();
				serialPort.DataReceived += new SerialDataReceivedEventHandler(readPacket);
				microcontroller = new Microcontroller(debugSerial, speed: 300, count: 30);
			} catch(IOException) {
				MessageBox.Show("Port " + serialPort.PortName + " doesn't exist on this computer");
				//throw new ArgumentNullException();
			} catch(UnauthorizedAccessException) {
				MessageBox.Show("Please make sure COM port is not already in use");
				//throw new UnauthorizedAccessException();
			}
		}

		private void addNewDataPointMethod(Packet packet)
		{
			try
			{
				int sensorId = packet.payload[0];
				float wheatstonef1A = System.BitConverter.ToSingle(packet.payload, 1);
				float wheatstonef1P = System.BitConverter.ToSingle(packet.payload, 5);
				float wheatstonef2A = System.BitConverter.ToSingle(packet.payload, 9);
				float wheatstonef2P = System.BitConverter.ToSingle(packet.payload, 13);
				float wheatstonef1Mf2A = System.BitConverter.ToSingle(packet.payload, 17);
				float wheatstonef1Mf2P = System.BitConverter.ToSingle(packet.payload, 21);
				float wheatstonef1Pf2A = System.BitConverter.ToSingle(packet.payload, 25);
				float wheatstonef1Pf2P = System.BitConverter.ToSingle(packet.payload, 29);
				float wheatstoneCoilf2A = System.BitConverter.ToSingle(packet.payload, 33);
				float wheatstoneCoilf2P = System.BitConverter.ToSingle(packet.payload, 37);
				chart1.Series[sensorId - 1].Points.AddY(wheatstonef1A);
				chart2.Series[sensorId - 1].Points.AddY(wheatstonef1P);
				chart3.Series[sensorId - 1].Points.AddY(wheatstonef2A);
				//chart1.Series[sensorId - 1].Points.AddXY(globalTime, wheatstonef1A);
				if (sensorId >= chart1.Series.Count)
					globalTime++;
			} catch (IndexOutOfRangeException) {
				
			} catch (ArgumentOutOfRangeException) {

			}
		}


		/* 
		 * This is the handler for the serial port write event. It is called automagically when
		 * something is written to the serial port. It will read the raw bytes and write them
		 * into a packet object, which it will then pass on to
		*/
		private void readPacket(object sender, SerialDataReceivedEventArgs args)
		{
			try
			{
				byte startOfFrame = (byte)serialPort.ReadByte();
				if (startOfFrame != 0xFE)
				{
					writeToFile(logFile, "<-<-<- Malformed packet sent. Started with 0x{0:X}", startOfFrame);
					return;
				}
				Packet packet;
				byte command = (byte)serialPort.ReadByte();
				byte payloadLength = (byte)serialPort.ReadByte();
				byte[] payload = new byte[payloadLength];
				if (serialPort.Read(payload, 0, payloadLength) < payloadLength)
				{
					writeToFile(logFile, "<-<-<- Payload length did not match");
					return;
				}
				byte Xor = (byte)serialPort.ReadByte();
				packet = new Packet(command, payloadLength, payload);
				printPacket(packet, PacketCommDirection.In);
				if (packet.Xor != Xor)
				{
					writeToFile(logFile, "<-<-<- XOR did not match");
					return;
				}

				switch (packet.command)
				{
					case (byte)((byte)PacketType.Start | (byte)PacketSender.Microcontroller):
						writeToFile(logFile, "Received a start acknowledge packet");
						break;
					case (byte)((byte)PacketType.Stop | (byte)PacketSender.Microcontroller):
						writeToFile(logFile, "Received a stop acknowledge packet");
						break;
					case (byte)((byte)PacketType.Report | (byte)PacketSender.Microcontroller):
						writeToFile(logFile, "Received a report packet");
						break;
					case (byte)((byte)PacketType.Error | (byte)PacketSender.Microcontroller):
						writeToFile(logFile, "Received an error packet");
						break;
					default:
						writeToFile(logFile, "Received an unknown packet");
						break;
				}
				int retval = protocolHandler.HandlePacket(packet, serialPort, chart: this.chart1);
				if(retval == (int)ProtocolDirective.AddData)
					if (InvokeRequired)
						this.Invoke(this.myDelegate, packet);
			} catch (ArgumentNullException) {
				MessageBox.Show("Argument Null Exception in GUI, most likely thrown by ProtocolHandler");
			} catch (InvalidOperationException) {
				MessageBox.Show("Invalid Operation Exception in GUI, most likely thrown by ProtocolHandler");
			} catch (ArgumentOutOfRangeException) {
				MessageBox.Show("Argument Out of Range Exception in GUI, most likely thrown by ProtocolHandler");
			} catch (ArgumentException) {
				MessageBox.Show("Argument Exception in GUI, most likely thrown by ProtocolHandler");
			} catch (TimeoutException) {
				writeToFile(logFile, "<-<-<- Timeout Exception");
			} catch (Exception) {
				MessageBox.Show("Exception in GUI, most likely thrown by ProtocolHandler");
			}
		}

		private void sensor_Click(object sender, EventArgs e)
		{
			string resultString = Regex.Match(((CheckBox)sender).Name, @"\d+").Value;
			foreach (TabPage t in this.tabControl1.Controls)
			{
				foreach (Chart c in t.Controls)
				{
					c.Series[System.Convert.ToInt32(resultString) - 1].Enabled = ((CheckBox)sender).Checked;
				}
			}
		}

		private void selectAllButton_Click(object sender, EventArgs e)
		{
			foreach(Control c in this.groupBox1.Controls)
			{
				if(c is CheckBox)
					((CheckBox)c).Checked = true;
			}
		}

		private void invertSelectionButton_Click(object sender, EventArgs e)
		{
			foreach (Control c in this.groupBox1.Controls)
			{
				if (c is CheckBox)
				{
					if (((CheckBox)c).Checked)
						((CheckBox)c).Checked = false;
					else
						((CheckBox)c).Checked = true;
				}
			}
		}

		private void stopRun(object sender, EventArgs e)
		{
			try {
				byte[] payload = new byte[20];
				Packet stopPacket = new Packet((byte)PacketType.Stop | (byte)PacketSender.GUI);
				printPacket(stopPacket, PacketCommDirection.Out);
				protocolHandler.HandlePacket(stopPacket, serialPort);
			} catch (System.ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
			} catch (System.FormatException) {
				MessageBox.Show("Please enter a valid number for all fields");
			} catch (System.OverflowException) {
				MessageBox.Show("Please enter a valid number in the range X to X for all fields");
			}
		}

		private void startRun(object sender, EventArgs e)
		{
			if ((this.f1Amplitude.Text == "") |
				(this.f1Frequency.Text == "") |
				(this.f2Amplitude.Text == "") |
				(this.f2Frequency.Text == "") |
				(this.measurementPeriod.Text == "")
			   )
			{
				MessageBox.Show("Please enter a value for all fields");
				return;
			}
			try {
				float[] data = new float[5];
				byte[] payload = new byte[20];
				data[0] = float.Parse(this.f1Amplitude.Text);
				data[1] = float.Parse(this.f2Amplitude.Text);
				data[2] = float.Parse(this.f1Frequency.Text);
				data[3] = float.Parse(this.f2Frequency.Text);
				data[4] = float.Parse(this.measurementPeriod.Text);
				//Array.Copy(BitConverter.GetBytes(f1A), 0, payload, 0, 4);
				//Array.Copy(BitConverter.GetBytes(f2A), 0, payload, 4, 4);
				//Array.Copy(BitConverter.GetBytes(f1F), 0, payload, 8, 4);
				//Array.Copy(BitConverter.GetBytes(f2F), 0, payload, 12, 4);
				//Array.Copy(BitConverter.GetBytes(period), 0, payload, 16, 4);
				Buffer.BlockCopy(data, 0, payload, 0, payload.Length);
				Packet startPacket = new Packet((byte)PacketType.Start | (byte)PacketSender.GUI, (byte)payload.Length, payload);
				printPacket(startPacket, PacketCommDirection.Out);
				protocolHandler.HandlePacket(startPacket, serialPort);
			} catch (System.ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
			} catch (System.FormatException) {
				MessageBox.Show("Please enter a valid number for all fields");
			} catch (System.OverflowException) {
				MessageBox.Show("Please enter a valid number in the range X to X for all fields");
			}
		}

		private void timeTareButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < chart1.Series.Count; i++)
			{
				foreach (TabPage t in this.tabControl1.Controls)
				{
					foreach (Chart c in t.Controls)
					{
						c.ChartAreas[0].AxisX.Minimum = globalTime;
					}
				}
			}
		}

		private void printPacket(Packet packet, PacketCommDirection direction)
		{
			string directionString;
			if (direction == PacketCommDirection.In)
				directionString = "<-<-<- ";
			else
				directionString = "->->-> ";

			writeToFile(logFile, directionString + "CMD:0x{0:X2}", packet.command, addNewLine: false);
			writeToFile(logFile, " PL:" + packet.payloadLength, addNewLine: false, dateTimeStamp: false);
			writeToFile(logFile, " P:0x", addNewLine: false, dateTimeStamp: false);
			for (int i = 0; i < packet.payloadLength; i++)
			{
				writeToFile(logFile, "{0:X2}", packet.payload[i], addNewLine: false, dateTimeStamp: false);
			}
			writeToFile(logFile, " XOR:0x{0:X2}", packet.Xor, dateTimeStamp: false);
		}

		private void writeToFile(TextWriter file, string text, object arg0 = null, bool addNewLine = true, bool dateTimeStamp = true)
		{
			if (file != null)
			{
				if (dateTimeStamp)
					file.Write(DateTime.Now + ": ");

				if (arg0 != null)
					file.Write(text, arg0);
				else
					file.Write(text);

				if (addNewLine)
					file.Write("\n");

				file.Flush();
			}

			if (printLogText)
			{
				if (dateTimeStamp)
					Console.Write(DateTime.Now + ": ");

				Console.Write(text, arg0);

				if (addNewLine)
					Console.Write("\n");
			}
		}
	}
}
