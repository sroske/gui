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

	enum PinAssignment
	{
		A,
		B
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
		PinAssignment pins = PinAssignment.A;
		public delegate void addNewDataPoint(Packet packet);
		public addNewDataPoint myDelegate;
		int globalTime = 0;
		int tareIndex = 0;
		int[] referenceSensors = { 1, 2, 7, 29, 30 };


		public GUI(string comPort)
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
				writeToFile(logFile, "Created data log file");
				initDataFile(dataLogFile);
				writeToFile(logFile, "Initialized data log file");
			} catch (Exception) {
				DialogResult messageBoxResult = MessageBox.Show("Unable to create file for recording data. Continue?", "Error", MessageBoxButtons.YesNo);
				if (messageBoxResult != DialogResult.Yes)
					throw new Exception();
			}

			/*
			 * Initialize delegate for adding new data to the graph (this is because of stupid threading stuff; just nod and move on)
			*/
			myDelegate = new addNewDataPoint(addNewDataPointMethod);

			/*
			 * Initialize COM ports
			*/
			//serialPort = new SerialPort(comPort, 115200);
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

		private void GUI_Shown(object sender, EventArgs e)
		{
			this.radioButtonA.PerformClick();
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

				rawChart1.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef1A);
				rawChart1.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;
				logData(dataLogFile, sensorId, wheatstonef1A);
				rawChart2.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef1P);
				rawChart2.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;
				rawChart3.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef2A);
				rawChart3.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;

				// Write all to log files
				if (this.amplitudeTareCheckbox.Checked)
				{
					wheatstonef1A -= (float)rawChart1.Series.FindByName(System.Convert.ToString(sensorId)).Points.ElementAt(tareIndex).YValues[0];
					wheatstonef1P -= (float)rawChart2.Series.FindByName(System.Convert.ToString(sensorId)).Points.ElementAt(tareIndex).YValues[0];
					wheatstonef2A -= (float)rawChart3.Series.FindByName(System.Convert.ToString(sensorId)).Points.ElementAt(tareIndex).YValues[0];
				}

				if (this.referenceTareCheckbox.Checked)
				{
					wheatstonef1A -= (float)getReferenceAverage(this.rawChart1);
					wheatstonef1P -= (float)getReferenceAverage(this.rawChart2);
					wheatstonef2A -= (float)getReferenceAverage(this.rawChart3);
				}

				adjustedChart1.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef1A);
				adjustedChart1.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;
				adjustedChart2.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef1P);
				adjustedChart2.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;
				adjustedChart3.Series.FindByName(System.Convert.ToString(sensorId)).Points.AddXY(getAddTime(sensorId), wheatstonef2A);
				adjustedChart3.Series.FindByName(System.Convert.ToString(sensorId)).Points.Last().MarkerStyle = MarkerStyle.Circle;

				if (sensorId >= adjustedChart1.Series.Count)
					globalTime++;
			} catch (IndexOutOfRangeException) {
				
			} catch (ArgumentOutOfRangeException) {

			} catch (NullReferenceException) {

			}
		}

		private double getAddTime(int pin)
		{
			double time;
			if (pins == PinAssignment.A)
			{
				switch (pin)
				{
					case 18: case 20: case 22: case 24: case 26: case 28: case 30:
						time = ((double)globalTime) + 0.0;
						break;
					case 17: case 19: case 21: case 23: case 25: case 27: case 29:
						time = ((double)globalTime) + 0.2;
						break;
					case 15: case 13: case 11: case 9: case 6: case 4: case 2:
						time = ((double)globalTime) + 0.4;
						break;
					case 14: case 12: case 10: case 8: case 5: case 3: case 1:
						time = ((double)globalTime) + 0.6;
						break;
					case 7:
						time = ((double)globalTime) + 0.8;
						break;
					default:
						time = (double)globalTime;
						break;
				}
			}
			else
			{
				switch (pin)
				{
					case 1: case 3: case 5: case 8: case 10: case 12: case 14:
						time = ((double)globalTime) + 0.0;
						break;
					case 2: case 4: case 6: case 9: case 11: case 13: case 15:
						time = ((double)globalTime) + 0.2;
						break;
					case 29: case 27: case 25: case 23: case 21: case 19: case 17:
						time = ((double)globalTime) + 0.4;
						break;
					case 30: case 28: case 26: case 24: case 22: case 20: case 18:
						time = ((double)globalTime) + 0.6;
						break;
					case 7:
						time = ((double)globalTime) + 0.8;
						break;
					default:
						time = (double)globalTime;
						break;
				}
			}
			return time;
		}

		private double getReferenceAverage(Chart chart)
		{
			double total = 0;
			int count = 0;

			for (int i = 0; i < 5; i++)
			{
				foreach (Control c in this.groupBox1.Controls)
				{
					if (c is CheckBox)
					{
						if (getPinNumber(((CheckBox)c).Name) == referenceSensors[i])
						{
							try {
								if (((CheckBox)c).Enabled)
								{
									total += chart.Series.FindByName(System.Convert.ToString(referenceSensors[i])).Points.Last().YValues[0];
									count++;
								}
							} catch (ArgumentNullException) {
								MessageBox.Show("Argument was null in getReferenceAverage");
							}
						}
					}
				}
			}

			if (count > 0)
				return (total / count);
			else
				return 0;
		}

		void setPinColor(int pin)
		{
			if (pins == PinAssignment.A)
			{
				foreach (TabPage t in this.tabControl1.Controls)
				{
					foreach (Control c in t.Controls)
					{
						if (c is Chart)
						{
							switch (pin)
							{
								case 18: case 17: case 15: case 14:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x1B, 0x9E, 0x77);
									break;
								case 20: case 19: case 13: case 12:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xD9, 0x5F, 0x02);
									break;
								case 22: case 21: case 11: case 10:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x75, 0x70, 0xB3);
									break;
								case 24: case 23: case 9: case 8:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xE7, 0x29, 0x8A);
									break;
								case 26: case 25: case 6: case 5:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x66, 0xA6, 0x1E);
									break;
								case 28: case 27: case 4: case 3:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xE6, 0xAB, 0x02);
									break;
								case 30: case 29: case 2: case 1:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xA6, 0x76, 0x1D);
									break;
								default:
									break;
							}
						}
					}
				}
			}
			else
			{
				foreach (TabPage t in this.tabControl1.Controls)
				{
					foreach (Control c in t.Controls)
					{
						if (c is Chart)
						{
							switch (pin)
							{
								case 1: case 2:  case 29: case 30:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x1B, 0x9E, 0x77);
									break;
								case 3: case 4: case 27: case 28:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xD9, 0x5F, 0x02);
									break;
								case 5: case 6: case 25: case 26:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x75, 0x70, 0xB3);
									break;
								case 8: case 9: case 23: case 24:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xE7, 0x29, 0x8A);
									break;
								case 10: case 11: case 21: case 22:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0x66, 0xA6, 0x1E);
									break;
								case 12: case 13: case 19: case 20:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xE6, 0xAB, 0x02);
									break;
								case 14: case 15: case 17: case 18:
									((Chart)c).Series.FindByName(System.Convert.ToString(pin)).Color = Color.FromArgb(0xA6, 0x76, 0x1D);
									break;
								default:
									break;
							}
						}
					}
				}
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
				int retval = protocolHandler.HandlePacket(packet, serialPort, chart: this.adjustedChart1);
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

		private void sensor_CheckedChanged(object sender, EventArgs e)
		{
			int number = getPinNumber(((CheckBox)sender).Name);

			foreach (TabPage t in this.tabControl1.Controls)
			{
				foreach (Control c in t.Controls)
				{
					if (c is Chart)
					{
						((Chart)c).Series.FindByName(System.Convert.ToString(number)).Enabled = ((CheckBox)sender).Checked;
						setPinColor(number);
					}
				}
			}
		}

		private void referenceTareCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				foreach (Control c in this.groupBox1.Controls)
				{
					if (c is CheckBox)
					{
						if (getPinNumber(((CheckBox)c).Name) == referenceSensors[i])
						{
							((CheckBox)c).Enabled = !this.referenceTareCheckbox.Checked;
						}
					}
				}
			}
		}

		private void amplitudeTareCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.amplitudeTareCheckbox.Checked)
			{
				this.referenceTareCheckbox.Enabled = true;
			}
			else
			{
				this.referenceTareCheckbox.Checked = false;
				this.referenceTareCheckbox.Enabled = false;
			}
		}

		private int getPinNumber(string name)
		{
			if (pins == PinAssignment.A)
				return System.Convert.ToInt32(name.Substring(1, 2));
			else
				return System.Convert.ToInt32(name.Substring(4, 2));
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

		private void radioButton_Click(object sender, EventArgs e)
		{
			if(sender.Equals(this.radioButtonA))
			{
				this.radioButtonA.Checked = true;
				this.radioButtonB.Checked = false;
				this.pins = PinAssignment.A;
			}
			else
			{
				this.radioButtonA.Checked = false;
				this.radioButtonB.Checked = true;
				this.pins = PinAssignment.B;
			}
			foreach (Control c in this.groupBox1.Controls)
			{
				if (c is CheckBox)
				{
					int i;
					for (i = 0; i < 5; i++)
					{
						if (getPinNumber(((CheckBox)c).Name) == referenceSensors[i])
						{
							((CheckBox)c).Enabled = !this.referenceTareCheckbox.Checked;
							((CheckBox)c).Checked = !((CheckBox)c).Checked;
							((CheckBox)c).Checked = !((CheckBox)c).Checked;
							break;
						}
					}
					if (i >= 5)
					{
						((CheckBox)c).Enabled = true;
						((CheckBox)c).Checked = !((CheckBox)c).Checked;
						((CheckBox)c).Checked = !((CheckBox)c).Checked;
					}
				}
			}
		}

		private void amplitudeTareButton_Click(object sender, EventArgs e)
		{
			tareIndex = globalTime;
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
			globalTime = 0;
			tareIndex = 0;
			foreach (TabPage t in this.tabControl1.Controls)
			{
				foreach (Control c in t.Controls)
				{
					if (c is Chart)
					{
						((Chart)c).ChartAreas[0].AxisX.Minimum = globalTime;
						foreach (Series s in ((Chart)c).Series)
						{
							s.Points.Clear();
							s.Points.AddXY(globalTime, 0);
						}
					}
				}
			}
			globalTime++;
			try {
				float[] data = new float[5];
				byte[] payload = new byte[20];
				data[0] = float.Parse(this.f1Amplitude.Text);
				data[1] = float.Parse(this.f2Amplitude.Text);
				data[2] = float.Parse(this.f1Frequency.Text);
				data[3] = float.Parse(this.f2Frequency.Text);
				data[4] = float.Parse(this.measurementPeriod.Text);
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
			foreach (TabPage t in this.tabControl1.Controls)
			{
				foreach (Control c in t.Controls)
				{
					if(c is Chart)
						((Chart)c).ChartAreas[0].AxisX.Minimum = globalTime;
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

		private void initDataFile(TextWriter file)
		{
			for (int i = 1; i <= 30; i++)
			{
				file.Write("Sensor   " + i + "\t");
			}
			file.Write("\n");
		}

		static int currentSensor = 1;
		private void logData(TextWriter file, int sensor, double data)
		{
			if (sensor != currentSensor)
				return;

			string dataString = System.Convert.ToString(data);
			if(sensor >= 10)
			{
				try {
					dataString = dataString.Substring(0, 11);
				} catch (ArgumentOutOfRangeException) {
					dataString = dataString.PadRight(11, '0');
				}
			} else {
				try {
					dataString = dataString.Substring(0, 10);
				} catch (ArgumentOutOfRangeException) {
					dataString = dataString.PadRight(10, '0');
				}
			}

			file.Write(dataString + "\t");
			currentSensor++;
			if (currentSensor == 16)
			{
				file.Write("0.000000000" + "\t");
				currentSensor++;
			}
			else
			if (currentSensor > 30)
			{
				currentSensor = 1;
				file.Write("\n");
			}
			file.Flush();
		}
	}
}
