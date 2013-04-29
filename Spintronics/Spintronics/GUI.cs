#define _DEBUG_

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
		Configuration configFile;							// File containing user configurations
		SerialPort serialPort = null;							// Main microcontroller COM port
		SerialPort debugSerial = null;						// COM port used for debugging with microcontroller emulator
		ProtocolHandler protocolHandler = new ProtocolHandler();		// Protocol handler object
		Microcontroller microcontroller;						// Microcontroller emulator (debugging only)
		string runFilesDirectory;							// String containing the directory of the current run files
		TextWriter logFile = null;							// Log file writer
		TextWriter htFile = null;							// HT file writer
		TextWriter ltFile = null;							// LT file writer
		TextWriter ctFile = null;							// CT file writer
		bool running = false;								// Tells us whether or not a run is in progress
		bool resultsSaved = true;							// Tells us if the current results have been saved
		bool reactionWellValidated = false;						// Tells us if the Reaction Well name is valid
		SensorAssignment sensorAssignment = SensorAssignment.A;		// Tells us the sensor pin assignments for the physical sensor array being used
		public delegate void addNewDataPoint(Packet packet);			// Delegate for adding packets
		public addNewDataPoint myDelegate;						// See above
		int globalCycle = 0;								// The global cycle pointer; This always points to the cycle CURRENTLY BEING ACQUIRED AND RECORDED
		int tareIndex = 0;								// The global tare index (what is the first cycle of data the user can see)
		bool recalculate = true;							// Tells us if we should recalculate the adjusted chart points
		int cycleSensorCount = 0;							// Keeps track of the latest sensor we've received data for
		int mostRecentAddMpsCycle = 0;						// Keeps track of the latest cycle MNPs were added
		int enableAddBufferAndMnpAtCycle;						// Tells us when to re-enable the Add Buffer and Add MNPs buttons
		int[] referenceSensors = { 1, 2, 7, 29, 30 };				// Array containing the integer numbers of the reference sensors (they don't change)

		/*
		 * This is the constructor for the GUI that takes in a COM port name (e.g. "COM1")
		 */
		public GUI(string comPort)
		{
			InitializeComponent();											// Initialize GUI controls

			configFile = new Configuration();									// Create configuration object (automatically populated; see Configuration.cs)

			// Initialize delegate for adding new data to the graph
			// (this is because of threading rules)
			myDelegate = new addNewDataPoint(addNewDataPointMethod);

			// Initialize COM ports
#if _DEBUG_																// If we're debugging (and thus don't have an actual microcontroller),
			serialPort = new SerialPort("COM5", 115200);							// manually set the COM ports
			debugSerial = new SerialPort("COM6", 115200);
			debugSerial.ReadTimeout = 200;
#else																	// Otherwise, start with the COM port name passed in (see Program.cs)
			serialPort = new SerialPort(comPort, 115200);
#endif
			serialPort.ReadTimeout = 200;										// Always set the main COM port ReadTimeout property to 200 milliseconds

			// Open COM ports
			try {
				serialPort.Open();										// Open the main COM port
#if _DEBUG_																// If we're debugging,
				debugSerial.Open();										// open the debug COM port,
				microcontroller = new Microcontroller(debugSerial, speed: 200, count: 30);	// and start the microcontroller emulator (for behavior, see Microcontroller.cs)
#endif
				serialPort.DataReceived += new SerialDataReceivedEventHandler(readPacket);	// Add the handler for COM port reading (automatically called when something is written to main COM port)
			} catch(IOException) {
				MessageBox.Show("Port " + serialPort.PortName + " doesn't exist on this computer");
				throw new ArgumentNullException();
			} catch(UnauthorizedAccessException) {
				MessageBox.Show("Please make sure COM port is not already in use");
				throw new UnauthorizedAccessException();
			}
		}

		/*
		 * This is called when the GUI first starts up
		 */
		private void GUI_Shown(object sender, EventArgs e)
		{
			recalculate = false;													// This is the first time the GUI is being shown, and thus we have no data,
			this.reactionWellTextBox.Text = "-A";										// so validate the name in the Reaction Well text box (which we set to an
			this.validateReactionWellButton.PerformClick();									// assuredly-correct '-A') and force a click of the Validate button. This will
			this.reactionWellTextBox.Text = "";											// serve to initialize all of our check boxes and will not recalculate our non-
			recalculate = true;													// existent data.

			this.addBufferVolumeTextBox.Text = System.Convert.ToString(configFile.defaultAddBufferVolume);	// Set the default buffer volume,
			this.addMnpVolumeTextBox.Text = System.Convert.ToString(configFile.defaultAddMnpsVolume);		// MNPs volume,
			this.addBufferUnitLabel.Text = this.configFile.defaultVolumeUnit;						// buffer unit,
			this.addMnpUnitLabel.Text = this.configFile.defaultVolumeUnit;						// and MNPs unit texts according to our configuration file
			this.reactionWellTextBox.Focus();											// Put focus on the Reaction Well text box
		}

		/*
		 * This is called whenever a key is pressed (handles shortcuts)
		 */
		private void GUI_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && (e.KeyCode == Keys.S))
			{
				this.saveRunFilesAsToolStripMenuItem.PerformClick();		// Ctrl + S
			}

			if (e.Control && (e.KeyCode == Keys.N))					// Ctrl + N
			{
				this.startRunButton.PerformClick();
			}
		}

		/*
		 * This will add new data points to the GUI
		 */
		private void addNewDataPointMethod(Packet packet)
		{
			try
			{																				// (For all locations/specifications of data packets, see Coomunications document)
				int sensorId = packet.payload[0];														// Get the sensor ID
				float wheatstonef1A = System.BitConverter.ToSingle(packet.payload, 1);								// Get float #1
				float wheatstonef1P = System.BitConverter.ToSingle(packet.payload, 5);								// Get float #2
				float wheatstonef2A = System.BitConverter.ToSingle(packet.payload, 9);								// Get float #3
				float wheatstonef2P = System.BitConverter.ToSingle(packet.payload, 13);								// Get float #4
				float wheatstonef1Mf2A = System.BitConverter.ToSingle(packet.payload, 17);							// Get float #5
				float wheatstonef1Mf2P = System.BitConverter.ToSingle(packet.payload, 21);							// Get float #6
				float wheatstonef1Pf2A = System.BitConverter.ToSingle(packet.payload, 25);							// Get float #7
				float wheatstonef1Pf2P = System.BitConverter.ToSingle(packet.payload, 29);							// Get float #8
				float wheatstoneCoilf2A = System.BitConverter.ToSingle(packet.payload, 33);							// Get float #9
				float wheatstoneCoilf2P = System.BitConverter.ToSingle(packet.payload, 37);							// Get float #10

				// Add the data to the hidden charts and write them to the files
				rawChart1.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef1A);				// Add data to raw chart #1
				rawChart2.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef1P);				// Add data to raw chart #2
				rawChart3.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef2A);				// Add data to raw chart #3
				logData(htFile, sensorId, wheatstonef1A);													// Log the data in the appropriate file (these will be stored in 'temp' directory until saved)
				logData(ltFile, sensorId, wheatstonef1P);													// See above
				logData(ctFile, sensorId, wheatstonef2A);													// See above

				// Add the data to the visible charts
				if (globalCycle > 1)																// If globalCycle is greater than 1 (i.e. we're done buffering),
				{
					wheatstonef1A = (float)rawChart1.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];				// grab the data points,
					wheatstonef1P = (float)rawChart2.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];
					wheatstonef2A = (float)rawChart3.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];

					if (this.amplitudeTareCheckbox.Checked)												// if box is checked, subtract the sensor's amplitude value from cycle pointed at by tareIndex
					{
						wheatstonef1A -= (float)rawChart1.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];
						wheatstonef1P -= (float)rawChart2.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];
						wheatstonef2A -= (float)rawChart3.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];

						if (this.referenceTareCheckbox.Checked)											// if box is checked, subtract the average of the CHECKED reference sensors
						{
							wheatstonef1A -= (float)getReferenceAverage(this.rawChart1, (globalCycle - 1));
							wheatstonef1P -= (float)getReferenceAverage(this.rawChart2, (globalCycle - 1));
							wheatstonef2A -= (float)getReferenceAverage(this.rawChart3, (globalCycle - 1));
						}
					}

					adjustedChart1.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef1A);	// Add the data points,
					adjustedChart1.Series[sensorId - 1].Points.Last().MarkerStyle = MarkerStyle.Circle;					// and set their chart markers to circles
					adjustedChart2.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef1P);
					adjustedChart2.Series[sensorId - 1].Points.Last().MarkerStyle = MarkerStyle.Circle;
					adjustedChart3.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef2A);
					adjustedChart3.Series[sensorId - 1].Points.Last().MarkerStyle = MarkerStyle.Circle;
				}
				else
				{
					this.bufferingProgressBar.Maximum = 30;												// If we aren't done buffering, set the maximum to 30 cycles (buffering limit),
					this.bufferingProgressBar.Minimum = 0;												// set the minimum to 0,
					this.bufferingProgressBar.Visible = true;												// make its label visible,
					this.bufferingLabel.Visible = true;													// make the bar visible,
					this.bufferingProgressBar.Value = sensorId;											// and set its value to the latest sensor ID (once it hits 30, we're done buffering)
					if(sensorId == 30)
					{
						this.bufferingProgressBar.Visible = false;										// If we're done buffering, make the bar invisible,
						this.bufferingProgressBar.Value = 0;											// reset its value,
						this.bufferingLabel.Visible = false;											// and make its label invisible
					}
				}

				// Update the run status bars
				if (globalCycle <= this.configFile.sampleAverageCount)										// Waiting for first cycles to finish (averaging)
				{
					this.initialSignalProgressBar.Maximum = this.configFile.sampleAverageCount + 1;
					this.initialSignalProgressBar.Minimum = 0;
					this.initialSignalLabel.Text = "Averaging initial signal...";
					this.initialSignalProgressBar.Value = globalCycle;
				}
				else if (this.mostRecentAddMpsCycle == 0)													// Waiting for MNPs to be added
				{
					this.initialSignalProgressBar.Value = this.initialSignalProgressBar.Maximum;
					this.initialSignalLabel.Text = "Averaging initial signal...Done";
					this.signalChangeLabel.Text = "Waiting for MNPs to be added...";
				}
				else if ((globalCycle - this.mostRecentAddMpsCycle) <= this.configFile.diffusionCount)					// Waiting for diffusion cycles to finish
				{
					this.signalChangeProgressBar.Maximum = this.configFile.diffusionCount + 1;
					this.signalChangeProgressBar.Minimum = 0;
					this.signalChangeLabel.Text = "Waiting for signal change...";
					this.finalSignalLabel.Text = "";
					this.signalChangeProgressBar.Value = globalCycle - this.mostRecentAddMpsCycle;
					this.finalSignalProgressBar.Value = 0;
				}
				else if ((globalCycle - this.mostRecentAddMpsCycle) <= (this.configFile.diffusionCount + this.configFile.sampleAverageCount))	// Waiting for second set of averaging cycles to finish
				{
					this.signalChangeProgressBar.Value = this.signalChangeProgressBar.Maximum;
					this.signalChangeLabel.Text = "Waiting for signal change...Done";
					this.finalSignalProgressBar.Maximum = this.configFile.sampleAverageCount + 1;
					this.finalSignalProgressBar.Minimum = 0;
					this.finalSignalLabel.Text = "Averaging final signal...";
					this.finalSignalProgressBar.Value = globalCycle - this.mostRecentAddMpsCycle - this.configFile.diffusionCount;
				}
				else																			// Done
				{
					this.signalChangeProgressBar.Value = this.signalChangeProgressBar.Maximum;
					this.signalChangeLabel.Text = "Waiting for signal change...Done";
					this.finalSignalProgressBar.Value = this.finalSignalProgressBar.Maximum;
					this.finalSignalLabel.Text = "Averaging final signal...Done";
				}

				// Re-enable Add Buffer and Add MNPs buttons
				if (globalCycle >= this.enableAddBufferAndMnpAtCycle)
				{
					this.addBufferButton.Enabled = true;
					this.addMnpButton.Enabled = true;
				}

				// Update latest sensor ID and globalCycle
				cycleSensorCount = sensorId;
				if (sensorId >= adjustedChart1.Series.Count)
					globalCycle++;
			} catch (IndexOutOfRangeException) {
				
			} catch (ArgumentOutOfRangeException) {

			} catch (NullReferenceException) {

			}
		}

		/*
		 * This will get the time which the data should be added (globalTime plus an appropriate offset for the pin)
		 */
		private double getAddTime(int sensor)
		{
			if (sensorAssignment == SensorAssignment.A)
			{
				switch (sensor)
				{
					case 18: case 20: case 22: case 24: case 26: case 28: case 30:	// Column a offsets
						return 0.0;
					case 17: case 19: case 21: case 23: case 25: case 27: case 29:	// Column b offsets
						return 0.2;
					case 15: case 13: case 11: case 9: case 6: case 4: case 2:		// Column c offsets
						return 0.4;
					case 14: case 12: case 10: case 8: case 5: case 3: case 1:		// Column d offsets
						return 0.6;
					case 7:										// Column e offsets
						return 0.8;
					default:										// Default (should not happen)
						return 0.0;
				}
			}
			else
			{
				switch (sensor)
				{
					case 1: case 3: case 5: case 8: case 10: case 12: case 14:		// Column a offsets
						return 0.0;
					case 2: case 4: case 6: case 9: case 11: case 13: case 15:		// Column b offsets
						return 0.2;
					case 29: case 27: case 25: case 23: case 21: case 19: case 17:	// Column c offsets
						return 0.4;
					case 30: case 28: case 26: case 24: case 22: case 20: case 18:	// Column d offsets
						return 0.6;
					case 7:										// Column e offsets
						return 0.8;
					default:										// Default (should not happen)
						return 0.0;
				}
			}
		}

		/*
		 * This is used to get the average value of the reference sensors (most recent values)
		 */
		private double getReferenceAverage(Chart chart, int cycle)
		{
			double total = 0;
			int count = 0;

			for (int i = 0; i < 5; i++)										// For each sensor in our system,
			{
				foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])				// if the sensor is a reference sensor,
					{
						try {
							if (c.Checked)								// and the sensor is enabled,
							{
								double difference = chart.Series[referenceSensors[i] - 1].Points.ElementAt(cycle).YValues[0];
								difference -= chart.Series[referenceSensors[i] - 1].Points.ElementAt(tareIndex).YValues[0];
								total += difference;
								count++;								// add it to our running total and increment the count.
							}
						} catch (ArgumentNullException) {// Catch any potential null arguments in our array accessing (Series[referenceSensors[i] - 1])

						} catch (ArgumentOutOfRangeException) {

						}
					}
				}
			}

			if (count > 0)												// If we actually added anything up (i.e. sensors weren't all disabled),
				return (total / count);										// Divide and return that.
			else														// Otherwise,
				return 0;												// return 0. This will sidestep potential dividing-by-0 exceptions when all reference sensors are disabled.
		}

		/*
		 * This is used to do a flush of the current data in the visible charts and a total recalculation of all to-be-displayed data points
		 */
		private void recalculateData()
		{
			if (!recalculate)
				return;
			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					if (c.Name.Contains("adjusted"))
					{
						c.ChartAreas[0].AxisX.Minimum = tareIndex;
						foreach (Series s in c.Series)
						{
							try {
								s.Points.Clear();
								for(int i = tareIndex; i < globalCycle; i++)
								{
									if (globalCycle != 1)
									{
										if ((i == (globalCycle - 1)) && (System.Convert.ToInt32(s.Name) > cycleSensorCount))
											continue;
									}
									int sensorId = System.Convert.ToInt32(((Series)s).Name);
									Chart chart;
									if(c.Name.Contains("1")) {
										chart = this.rawChart1;
									} else if (c.Name.Contains("2")) {
										chart = this.rawChart2;
									} else {
										chart = this.rawChart3;
									}

									double value = chart.Series[sensorId - 1].Points.ElementAt(i).YValues[0];

									if (this.amplitudeTareCheckbox.Checked)
									{
										value -= (float)chart.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];

										if (this.referenceTareCheckbox.Checked)
										{
											value -= (float)getReferenceAverage(chart, i);
										}
									}

									double time = getAddTime(sensorId);
									time += (double)i;
									s.Points.AddXY(time, value);
									s.Points.Last().MarkerStyle = MarkerStyle.Circle;
								}
							} catch (ArgumentNullException) {

							} catch (ArgumentOutOfRangeException) {

							}
						}
					}
				}
			}
		}

		/* 
		 * This is the handler for the serial port write event. It is called automagically when
		 * something is written to the serial port. It will read the raw bytes and write them
		 * into a packet object, which it will then pass on to the Protocol Handler
		*/
		private void readPacket(object sender, SerialDataReceivedEventArgs args)
		{
			try
			{
				byte startOfFrame = (byte)serialPort.ReadByte();
				if (startOfFrame != 0xFE)
				{
					Console.WriteLine("<-<-<- Malformed packet sent. Started with 0x{0:X}", startOfFrame);
					return;
				}
				Packet packet;
				byte command = (byte)serialPort.ReadByte();
				byte payloadLength = (byte)serialPort.ReadByte();
				byte[] payload = new byte[payloadLength];
				if (serialPort.Read(payload, 0, payloadLength) < payloadLength)
				{
					Console.WriteLine("<-<-<- Payload length did not match");
					return;
				}
				byte Xor = (byte)serialPort.ReadByte();
				packet = new Packet(command, payloadLength, payload);
				printPacket(packet, PacketCommDirection.In);
				if (packet.Xor != Xor)
				{
					Console.WriteLine("<-<-<- XOR did not match");
					return;
				}

				switch (packet.command)
				{
					case (byte)((byte)PacketType.Start | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received a start acknowledge packet");
						break;
					case (byte)((byte)PacketType.Stop | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received a stop acknowledge packet");
						break;
					case (byte)((byte)PacketType.Report | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received a report packet");
						break;
					case (byte)((byte)PacketType.Error | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received an error packet");
						break;
					default:
						Console.WriteLine("Received an unknown packet");
						break;
				}

				ProtocolDirective retval = protocolHandler.HandlePacket(packet, serialPort, chart: this.adjustedChart1);
				if(retval == ProtocolDirective.AddData && this.running)
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
				Console.WriteLine("<-<-<- Timeout Exception");
			} catch (Exception) {
				MessageBox.Show("Exception in GUI, most likely thrown by ProtocolHandler");
			}
		}

		/*
		 * This returns the pin number of each sensor check box according to the current pin assignment
		 */
		private int getSensorNumber(string name)
		{
			if (sensorAssignment == SensorAssignment.A)
				return System.Convert.ToInt32(name.Substring(1, 2));
			else
				return System.Convert.ToInt32(name.Substring(4, 2));
		}

		/*
		 * This enables and disables the showing of the sensor data in each chart according to the sensor check boxes
		 */
		private void sensor_CheckedChanged(object sender, EventArgs e)
		{
			int number = getSensorNumber(((CheckBox)sender).Name);

			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					c.Series[number - 1].Enabled = ((CheckBox)sender).Checked;
					setSensorColor(number);
					setLegendText(number);
				}
			}
			for (int i = 0; i < 5; i++)
			{
				if (number == referenceSensors[i])
				{
					recalculateData();
				}
			}
		}

		/*
		 * This sets the charts' legend text for each sensor according to its position in the array (1-7, A-E)
		 */
		private void setLegendText(int sensor)
		{
			if (sensorAssignment == SensorAssignment.A)
			{
				foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
				{
					foreach (Chart c in t.Controls.OfType<Chart>())
					{
						string name;
						switch (sensor)
						{
							case 1: name = "1-0d"; break;
							case 2: name = "2-0c"; break;
							case 3: name = "3-6d"; break;
							case 4: name = "4-6c"; break;
							case 5: name = "5-5d"; break;
							case 6: name = "6-5c"; break;
							case 7: name = "7-0e"; break;
							case 8: name = "8-4d"; break;
							case 9: name = "9-4c"; break;
							case 10: name = "10-3d"; break;
							case 11: name = "11-3c"; break;
							case 12: name = "12-2d"; break;
							case 13: name = "13-2c"; break;
							case 14: name = "14-1d"; break;
							case 15: name = "15-1c"; break;
							case 16: name = "16"; break;
							case 17: name = "17-1b"; break;
							case 18: name = "18-1a"; break;
							case 19: name = "19-2b"; break;
							case 20: name = "20-2a"; break;
							case 21: name = "21-3b"; break;
							case 22: name = "22-3a"; break;
							case 23: name = "23-4b"; break;
							case 24: name = "24-4a"; break;
							case 25: name = "25-5b"; break;
							case 26: name = "26-5a"; break;
							case 27: name = "27-6b"; break;
							case 28: name = "28-6a"; break;
							case 29: name = "29-0b"; break;
							case 30: name = "30-0a"; break;
							default: name = ""; break;
						}
						c.Series.FindByName(System.Convert.ToString(sensor)).LegendText = name;
					}
				}
			}
			else
			{
				foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
				{
					foreach (Chart c in t.Controls.OfType<Chart>())
					{
						string name;
						switch (sensor)
						{
							case 1: name = "1-0a"; break;
							case 2: name = "2-0b"; break;
							case 3: name = "3-1a"; break;
							case 4: name = "4-1b"; break;
							case 5: name = "5-2a"; break;
							case 6: name = "6-3b"; break;
							case 7: name = "7-0e"; break;
							case 8: name = "8-3a"; break;
							case 9: name = "9-3b"; break;
							case 10: name = "10-4a"; break;
							case 11: name = "11-4b"; break;
							case 12: name = "12-5a"; break;
							case 13: name = "13-5b"; break;
							case 14: name = "14-6a"; break;
							case 15: name = "15-6b"; break;
							case 16: name = "16"; break;
							case 17: name = "17-6c"; break;
							case 18: name = "18-6d"; break;
							case 19: name = "19-5c"; break;
							case 20: name = "20-5d"; break;
							case 21: name = "21-4c"; break;
							case 22: name = "22-4d"; break;
							case 23: name = "23-3c"; break;
							case 24: name = "24-3d"; break;
							case 25: name = "25-2c"; break;
							case 26: name = "26-2d"; break;
							case 27: name = "27-1c"; break;
							case 28: name = "28-1d"; break;
							case 29: name = "29-0c"; break;
							case 30: name = "30-0d"; break;
							default: name = ""; break;
						}
						c.Series.FindByName(System.Convert.ToString(sensor)).LegendText = name;
					}
				}
			}
		}

		/*
		 * This sets the color of each sensor according to the current pin assignment
		 */
		private void setSensorColor(int sensor)
		{
			if (sensorAssignment == SensorAssignment.A)
			{
				foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
				{
					foreach (Chart c in t.Controls.OfType<Chart>())
					{
						switch (sensor)
						{
							case 18: case 17: case 15: case 14:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x1B, 0x9E, 0x77);
								break;
							case 20: case 19: case 13: case 12:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xD9, 0x5F, 0x02);
								break;
							case 22: case 21: case 11: case 10:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x75, 0x70, 0xB3);
								break;
							case 24: case 23: case 9: case 8:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xE7, 0x29, 0x8A);
								break;
							case 26: case 25: case 6: case 5:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x66, 0xA6, 0x1E);
								break;
							case 28: case 27: case 4: case 3:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xE6, 0xAB, 0x02);
								break;
							case 30: case 29: case 2: case 1:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xA6, 0x76, 0x1D);
								break;
							default:
								break;
						}
					}
				}
			}
			else
			{
				foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
				{
					foreach (Chart c in t.Controls.OfType<Chart>())
					{
						switch (sensor)
						{
							case 1: case 2: case 29: case 30:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x1B, 0x9E, 0x77);
								break;
							case 3: case 4: case 27: case 28:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xD9, 0x5F, 0x02);
								break;
							case 5: case 6: case 25: case 26:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x75, 0x70, 0xB3);
								break;
							case 8: case 9: case 23: case 24:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xE7, 0x29, 0x8A);
								break;
							case 10: case 11: case 21: case 22:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0x66, 0xA6, 0x1E);
								break;
							case 12: case 13: case 19: case 20:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xE6, 0xAB, 0x02);
								break;
							case 14: case 15: case 17: case 18:
								c.Series.FindByName(System.Convert.ToString(sensor)).Color = Color.FromArgb(0xA6, 0x76, 0x1D);
								break;
							default:
								break;
						}
					}
				}
			}
		}

		/*
		 * This checks all sensor check boxes
		 */
		private void selectAllButton_Click(object sender, EventArgs e)
		{
			int count = 0;
			recalculate = false;
			foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
			{
				int i;
				for (i = 0; i < 5; i++)
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])
					{
						count++;
						if (!c.Checked)
						{
							if (count == 5)
								recalculate = true;
							c.Checked = true;
						}
						else
						{
							if (count == 5)
							{
								recalculate = true;
								recalculateData();
							}
						}
						break;
					}
				}
				if (i >= 5)
					c.Checked = true;
			}
		}

		/*
		 * This inverts all sensor check boxes
		 */
		private void invertSelectionButton_Click(object sender, EventArgs e)
		{
			int count = 0;
			recalculate = false;
			foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
			{
				int i;
				for (i = 0; i < 5; i++)
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])
					{
						count++;
						if (count == 5)
							recalculate = true;
						c.Checked = !c.Checked;
						break;
					}
				}
				if (i >= 5)
				{
					c.Checked = !c.Checked;
				}
			}
		}

		/*
		 * This is called when the pin assignment is changed
		 */
		private void validateReactionWellButton_Click(object sender, EventArgs e)
		{
			this.validateReactionWellButton.BackColor = Color.IndianRed;
			if (reactionWellTextBox.Text == "")
			{
				MessageBox.Show("Terminate Reaction Well name with either '-A' or '-B' to indicate side of chip");
				this.reactionWellTextBox.Focus();
				return;
			}
			if (reactionWellTextBox.Text.Length == 1)
			{
				MessageBox.Show("Terminate Reaction Well name with either '-A' or '-B' to indicate side of chip");
				this.reactionWellTextBox.Focus();
				return;
			}
			if (reactionWellTextBox.Text.Substring(reactionWellTextBox.Text.Length - 2, 2).Equals("-A"))
			{
				this.sensorAssignment = SensorAssignment.A;
				this.reactionWellValidated = true;
				this.validateReactionWellButton.BackColor = Color.LightGreen;
			}
			else if (reactionWellTextBox.Text.Substring(reactionWellTextBox.Text.Length - 2, 2).Equals("-B"))
			{
				this.sensorAssignment = SensorAssignment.B;
				this.reactionWellValidated = true;
				this.validateReactionWellButton.BackColor = Color.LightGreen;
			}
			else
			{
				MessageBox.Show("Terminate Reaction Well name with either '-A' or '-B' to indicate side of chip");
				this.reactionWellTextBox.Focus();
				return;
			}
			int count = 0;
			recalculate = false;
			foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
			{
				int i;
				for (i = 0; i < 5; i++)
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])
					{
						count++;
						c.BackColor = Color.Blue;
						c.Checked = !c.Checked;
						if (count == 5)
						{
							recalculate = true;
						}
						c.Checked = !c.Checked;
						break;
					}
				}
				if (i >= 5)
				{
					c.BackColor = default(Color);
					c.Checked = !c.Checked;
					c.Checked = !c.Checked;
				}
			}
			if (this.sensorAssignment == SensorAssignment.A)
			{
				this.sensorsLabel1.Text = "1";
				this.sensorsLabel2.Text = "2";
				this.sensorsLabel3.Text = "3";
				this.sensorsLabel4.Text = "4";
				this.sensorsLabel5.Text = "5";
				this.sensorsLabel6.Text = "6";
				this.sensorsLabel7.Text = "0";
				this.s07_07.Location = new System.Drawing.Point(118, 173);
			}
			else
			{
				this.sensorsLabel1.Text = "0";
				this.sensorsLabel2.Text = "1";
				this.sensorsLabel3.Text = "2";
				this.sensorsLabel4.Text = "3";
				this.sensorsLabel5.Text = "4";
				this.sensorsLabel6.Text = "5";
				this.sensorsLabel7.Text = "6";
				this.s07_07.Location = new System.Drawing.Point(118, 53);
			}
			recalculate = true;
		}

		/*
		 * This is called when the user changes the text in the Reaction Well text box
		 */
		private void reactionWellTextBox_TextChanged(object sender, EventArgs e)
		{
			this.reactionWellValidated = false;			// Test changed, so unvalidate,
			this.validateReactionWellButton.BackColor = Color.IndianRed;
		}

		/*
		 * This starts a run
		 */
		private void startRun(object sender, EventArgs e)
		{
			if (this.running == true)
			{
				MessageBox.Show("Please stop the current run before starting a new one");
				return;
			}
			if ((this.reactionWellTextBox.Text == "") || (this.sampleTextBox.Text == ""))
			{
				MessageBox.Show("Please enter values for Reaction Well and Sample");
				return;
			}

			this.validateReactionWellButton.PerformClick();
			if (!this.reactionWellValidated)
			{
				return;
			}

			this.globalCycle = 0;
			this.tareIndex = 0;
			this.tareIndexTextbox.Text = "0";
			this.mostRecentAddMpsCycle = 0;
			this.initialSignalProgressBar.Value = 0;
			this.signalChangeProgressBar.Value = 0;
			this.finalSignalProgressBar.Value = 0;
			this.resultsSaved = false;

			this.stopRunButton.Enabled = true;
			this.stopRunToolStripMenuItem.Enabled = true;
			this.enableAddBufferAndMnpAtCycle = configFile.sampleAverageCount + 1;

			this.postProcessingToolStripMenuItem.Enabled = false;
			this.postProcessingToolStripMenuItem.ToolTipText = "Please stop the current run before doing any post-processing";

			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					c.ChartAreas[0].AxisX.Minimum = globalCycle;
					c.ChartAreas[0].AxisX.StripLines.Clear();
					foreach (Series s in c.Series)
					{
						s.Points.Clear();
						s.Points.AddXY(globalCycle + getAddTime(System.Convert.ToInt32(s.Name)), 0);
						s.Points.Last().MarkerStyle = MarkerStyle.Circle;
					}
				}
			}
			createRunFiles();
			this.globalCycle++;
			try {
				float[] data = new float[5];
				byte[] payload = new byte[21];
				data[0] = this.configFile.wheatstoneAmplitude;
				data[1] = this.configFile.coilAmplitude;
				data[2] = this.configFile.wheatstoneFrequency;
				data[3] = this.configFile.coilFrequency;
				data[4] = this.configFile.measurementPeriod;
				Buffer.BlockCopy(data, 0, payload, 0, payload.Length - 1);
				payload[20] = 0x01;
				Packet startPacket = new Packet((byte)PacketType.Start | (byte)PacketSender.GUI, (byte)payload.Length, payload);
				printPacket(startPacket, PacketCommDirection.Out);
				protocolHandler.HandlePacket(startPacket, serialPort);
				this.running = true;
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
			} catch (FormatException) {
				MessageBox.Show("Please enter a valid number for all fields");
			} catch (OverflowException) {
				MessageBox.Show("Please enter a valid number for all fields");
			}
		}

		/*
		 * This stops a run
		 */
		private void stopRun(object sender, EventArgs e)
		{
			if (this.running == false)
			{
				MessageBox.Show("There is not currently a run in progress");
				return;
			}

			byte[] payload = new byte[20];
			Packet stopPacket = new Packet((byte)PacketType.Stop | (byte)PacketSender.GUI);
			printPacket(stopPacket, PacketCommDirection.Out);
			protocolHandler.HandlePacket(stopPacket, serialPort);
			this.bufferingLabel.Visible = false;
			this.bufferingProgressBar.Visible = false;
			this.initialSignalProgressBar.Value = 0;
			this.initialSignalLabel.Text = "";
			this.signalChangeProgressBar.Value = 0;
			this.signalChangeLabel.Text = "";
			this.finalSignalProgressBar.Value = 0;
			this.finalSignalLabel.Text = "";
			this.running = false;

			validatePostProcessing();

			this.stopRunButton.Enabled = false;
			this.stopRunToolStripMenuItem.Enabled = false;
			this.addMnpButton.Enabled = false;
			this.addBufferButton.Enabled = false;

			try {
				this.logFile.Close();
				this.htFile.Close();
				this.ltFile.Close();
				this.ctFile.Close();
			} catch (Exception) {

			}
		}

		/*
		 * This validates the availability of post processing
		 */
		private void validatePostProcessing()
		{
			if (this.mostRecentAddMpsCycle == 0)
			{
				this.postProcessingToolStripMenuItem.Enabled = false;
				this.postProcessingToolStripMenuItem.ToolTipText = "MNPs were never added";
			}
			else if (this.mostRecentAddMpsCycle <= configFile.sampleAverageCount)
			{
				this.postProcessingToolStripMenuItem.Enabled = false;
				this.postProcessingToolStripMenuItem.ToolTipText = "Not enough cycles occurred before most-recent MNP add cycle\n" +
													"Minimum is " + this.configFile.sampleAverageCount + " cycles, " +
													"only " + (this.mostRecentAddMpsCycle - 1) + " occurred";
			}
			else if ((this.globalCycle - this.mostRecentAddMpsCycle) <= (this.configFile.sampleAverageCount + this.configFile.diffusionCount))
			{
				this.postProcessingToolStripMenuItem.Enabled = false;
				this.postProcessingToolStripMenuItem.ToolTipText = "Not enough cycles occurred after most-recent MNP add cycle\n" +
													"Minimum is " + (this.configFile.sampleAverageCount + this.configFile.diffusionCount) + " cycles, " +
													"only " + (this.globalCycle - this.mostRecentAddMpsCycle - 1) + " occurred";
			}
			else
			{
				this.postProcessingToolStripMenuItem.Enabled = true;
				this.postProcessingToolStripMenuItem.ToolTipText = "Perform post-processing on data";
			}
		}

		/*
		 * This enables and disables the option to change the reference-tare check box according to the amplitude tare check box state
		 */
		private void amplitudeTareCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.amplitudeTareCheckbox.Checked)			// If the amplitude-tare check box is checked,
			{
				this.referenceTareCheckbox.Enabled = true;	// enable the reference-tare check box.
				recalculateData();
			}
			else									// Otherwise,
			{
				this.referenceTareCheckbox.Enabled = false;	// and disable it.
				if (this.referenceTareCheckbox.Checked)
					this.referenceTareCheckbox.Checked = false;	// uncheck the reference-tare check box,
				else
					recalculateData();
			}
		}

		/*
		 * This enables and disables showing reference sensors in charts according to the reference tare check box
		 */
		private void referenceTareCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			recalculateData();
		}

		/*
		 * This handles a user entering a new Tare Index into the text box manually
		 */
		private void tareIndexTextbox_TextEntered(object sender, EventArgs e)
		{
			if (e is KeyEventArgs)
			{
				if (((KeyEventArgs)e).KeyCode != Keys.Return)
					return;
			}
			if (this.tareIndexTextbox.Text == "")
			{
				MessageBox.Show("Please enter a value for the Tare Index");
				return;
			}
			try {
				if ((System.Convert.ToInt32(this.tareIndexTextbox.Text) >= globalCycle) && (globalCycle != 0))
				{
					MessageBox.Show("You cannot set the tare index to a value greater than or equal to the current cycle");
					this.tareIndexTextbox.Text = System.Convert.ToString(tareIndex);
				}
				else if (System.Convert.ToInt32(this.tareIndexTextbox.Text) != tareIndex)
				{
					tareIndex = System.Convert.ToInt32(this.tareIndexTextbox.Text);
					recalculateData();
				}
			} catch (FormatException) {
				MessageBox.Show("Please make sure the tare index is an integer");
			} catch (OverflowException) {
				MessageBox.Show("Please enter a valid integer for the tare index");
			}
		}

		/*
		 * This handles the user adding a buffer to the sensor sample
		 */
		private void addBufferButton_Click(object sender, EventArgs e)
		{
			if (!this.running)
			{
				MessageBox.Show("Please start a run before trying to add Buffer");
				return;
			}
			if (this.addBufferVolumeTextBox.Text == "")
			{
				MessageBox.Show("Please add an amount to the Buffer text box");
				return;
			}
			this.logFile.WriteLine(globalCycle + "\t\tAdd " +
							this.addBufferVolumeTextBox.Text + " " +
							this.configFile.defaultVolumeUnit + " Buffer (" +
							this.configFile.bufferName + ")");
			this.logFile.Flush();

			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					StripLine line = new StripLine();
					line.Text = "Buffer";
					line.TextOrientation = TextOrientation.Horizontal;
					line.IntervalOffset = this.globalCycle;
					line.StripWidth = 1;
					line.BackColor = Color.FromArgb(0xDD, 0xDD, 0xDD);
					c.ChartAreas[0].AxisX.StripLines.Add(line);
				}
			}

			this.addBufferButton.Enabled = false;
			this.addMnpButton.Enabled = false;
			this.enableAddBufferAndMnpAtCycle = globalCycle + 1;
		}

		/*
		 * This handles the user adding MNPs to the sensor sample
		 */
		private void addMnpButton_Click(object sender, EventArgs e)
		{
			if (!this.running)
			{
				MessageBox.Show("Please start a run before trying to add MNP");
				return;
			}
			if (this.addMnpVolumeTextBox.Text == "")
			{
				MessageBox.Show("Please add an amount to the MNP text box");
				return;
			}
			this.logFile.WriteLine(this.globalCycle + "\t\tAdd " +
							this.addMnpVolumeTextBox.Text + " " +
							this.configFile.defaultVolumeUnit + " MNPs (" +
							this.configFile.mnpsName + ")");
			this.logFile.Flush();

			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					StripLine line = new StripLine();
					line.Text = "\nMNPs";
					line.TextOrientation = TextOrientation.Horizontal;
					line.IntervalOffset = this.globalCycle;
					line.StripWidth = 1;
					line.BackColor = Color.FromArgb(0x99, 0x99, 0x99);
					c.ChartAreas[0].AxisX.StripLines.Add(line);
				}
			}

			this.mostRecentAddMpsCycle = this.globalCycle;

			this.addBufferButton.Enabled = false;
			this.addMnpButton.Enabled = false;
			this.enableAddBufferAndMnpAtCycle = globalCycle + 1;
		}

		/*
		 * This prints out GUI-microcontroller packets
		 */
		private void printPacket(Packet packet, PacketCommDirection direction)
		{
			string directionString;
			if (direction == PacketCommDirection.In)
				directionString = "<-<-<- ";
			else
				directionString = "->->-> ";

			Console.Write(directionString + "CMD:0x{0:X2}", packet.command);
			Console.Write(" PL:" + packet.payloadLength);
			Console.Write(" P:0x");
			for (int i = 0; i < packet.payloadLength; i++)
			{
				Console.Write("{0:X2}", packet.payload[i]);
			}
			Console.Write(" XOR:0x{0:X2}\n", packet.Xor);
		}

		/*
		 * This initializes a data file by printing out the sensor name headers at the top of the file
		 */
		private void createRunFiles()
		{
			try {
				try {
					logFile.Close();
					htFile.Close();
					ltFile.Close();
					ctFile.Close();
				} catch (Exception) {

				}

				string[] tempDirectories;
				try {
					tempDirectories = System.IO.Directory.GetDirectories("./temp");
				} catch (Exception) {
					tempDirectories = null;
				}
				if(tempDirectories != null)
				{
					if ((configFile.tempFoldersToKeep != -1) && (tempDirectories.Length >= configFile.tempFoldersToKeep))
					{
						for (int i = 0; (i < tempDirectories.Length) && (i < (tempDirectories.Length - configFile.tempFoldersToKeep + 1)); i++)
						{
							try {
								Directory.Delete(tempDirectories[i], true);
							} catch (UnauthorizedAccessException) {

							} catch (IOException) {

							}
						}
					}
				}

				string timestamp = "";
				timestamp += DateTime.Now.Year + "-";
				timestamp += DateTime.Now.Month + "-";
				timestamp += DateTime.Now.Day + "__";
				timestamp += DateTime.Now.Hour + ".";
				timestamp += DateTime.Now.Minute + ".";
				timestamp += DateTime.Now.Second;
				runFilesDirectory = "./temp/" + timestamp;
				System.IO.Directory.CreateDirectory(runFilesDirectory);

				string logFileNameBase = runFilesDirectory + "/";
				logFile = new StreamWriter(logFileNameBase + "log.txt");
				htFile = new StreamWriter(logFileNameBase + "HT.txt");
				ltFile = new StreamWriter(logFileNameBase + "LT.txt");
				ctFile = new StreamWriter(logFileNameBase + "CT.txt");

				logFile.WriteLine("Reaction Well:\t" + this.reactionWellTextBox.Text);
				logFile.WriteLine("Sample:\t\t" + this.sampleTextBox.Text + "\n");
				logFile.WriteLine("Cycle\t\tDetails");
				logFile.WriteLine("*********************************************");
				logFile.WriteLine(globalCycle + "\t\tPreload " +
							 this.configFile.preloadBufferVolume + " " +
							 this.configFile.defaultVolumeUnit + " Buffer (" +
							 this.configFile.bufferName + ")");
				logFile.Flush();
				for (int i = 0; i < 30; i++)
				{
					if (i == 15)
						continue;
					if (i < 9)
					{
						htFile.Write("Sensor   " + (i + 1) + "\t");
						ltFile.Write("Sensor   " + (i + 1) + "\t");
						ctFile.Write("Sensor   " + (i + 1) + "\t");
					}
					else
					{
						htFile.Write("Sensor  " + (i + 1) + "\t");
						ltFile.Write("Sensor  " + (i + 1) + "\t");
						ctFile.Write("Sensor  " + (i + 1) + "\t");
					}
				}
				htFile.Write("\n");
				ltFile.Write("\n");
				ctFile.Write("\n");
				htFile.Flush();
				ltFile.Flush();
				ctFile.Flush();

			} catch (Exception) {
				DialogResult messageBoxResult = MessageBox.Show("Unable to create run files. Continue?", "Error", MessageBoxButtons.YesNo);
				if (messageBoxResult != DialogResult.Yes)
					throw new Exception();
			}
		}

		/*
		 * This adds data to the data log file
		 */
		private void logData(TextWriter file, int sensor, double data)
		{
			string dataString = System.Convert.ToString(data);
			/*try {
				dataString = dataString.Substring(0, 10);
			} catch (ArgumentOutOfRangeException) {
				dataString = dataString.PadRight(10, '0');
			}*/

			file.Write(dataString + "\t");
			if (sensor == 30)
			{
				file.Write("\n");
			}
			file.Flush();
		}

		/*
		 * This opens a Preferences window and saves the values entered
		 */
		private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.running == false)
			{
				Preferences preferenceWindow = new Preferences(configFile);
				var dialogResult = preferenceWindow.ShowDialog();
				if (dialogResult.Equals(DialogResult.OK))
				{
					configFile.setTempFoldersToKeep(preferenceWindow.tempFoldersToKeep);
					configFile.setSensorMultiplexerValues(preferenceWindow.sensorMultiplexerValues);
					configFile.setBufferName(preferenceWindow.bufferName);
					configFile.setMnpsName(preferenceWindow.mnpsName);
					configFile.setPreloadBufferVolume(preferenceWindow.preloadBufferVolume);
					configFile.setDefaultAddBufferVolume(preferenceWindow.defaultAddBufferVolume);
					configFile.setDefaultAddMnpsVolume(preferenceWindow.defaultAddMnpsVolume);
					configFile.setDefaultVolumeUnit(preferenceWindow.defaultVolumeUnit);
					configFile.setWheatstoneAmplitude(preferenceWindow.wheatstoneAmplitude);
					configFile.setWheatstoneAmplitudeUnit(preferenceWindow.wheatstoneAmplitudeUnit);
					configFile.setWheatstoneFrequency(preferenceWindow.wheatstoneFrequency);
					configFile.setCoilAmplitude(preferenceWindow.coilAmplitude);
					configFile.setCoilAmplitudeUnit(preferenceWindow.coilAmplitudeUnit);
					configFile.setCoilFrequncy(preferenceWindow.coilFrequency);
					configFile.setCoilDcOffset(preferenceWindow.coilDcOffset);
					configFile.setCoilDcOffsetUnit(preferenceWindow.coilDcOffsetUnit);
					configFile.setMeasurementPeriod(preferenceWindow.measurementPeriod);
					configFile.setSampleAverageCount(preferenceWindow.sampleAverageCount);
					configFile.setDiffusionCount(preferenceWindow.diffusionCount);
					configFile.setPostProcessingFiles(preferenceWindow.postProcessingFiles);
					this.addBufferUnitLabel.Text = configFile.defaultVolumeUnit;
					this.addMnpUnitLabel.Text = configFile.defaultVolumeUnit;
					validatePostProcessing();
				}
				preferenceWindow.Dispose();
			}
			else
			{
				Preferences preferenceWindow = new Preferences(configFile, readOnlySetting: true);
				preferenceWindow.ShowDialog();
			}
		}

		/*
		 * This opens a Save dialog and saves the data for the current run
		 */
		private void saveRunFilesAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.running)
			{
				MessageBox.Show("Please stop before saving files");
				return;
			}
			if (this.runFilesDirectory == null)
			{
				MessageBox.Show("Please complete a run before saving files");
				return;
			}

			this.validateReactionWellButton.PerformClick();
			if (!this.reactionWellValidated)
			{
				return;
			}

			try {
				this.logFile.Close();
				this.htFile.Close();
				this.ltFile.Close();
				this.ctFile.Close();
			} catch (Exception) {

			}

			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.FileName = this.reactionWellTextBox.Text + "(" + this.sampleTextBox.Text + ")";
			saveFile.FileName = saveFile.FileName.Replace('\\', '_');
			saveFile.FileName = saveFile.FileName.Replace('/', '_');
			saveFile.InitialDirectory = this.configFile.defaultSaveDirectory;
			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				try {
					string newDefaultDirectory = saveFile.FileName;
					int last = newDefaultDirectory.LastIndexOf("\\");
					newDefaultDirectory = newDefaultDirectory.Substring(0, last);
					this.configFile.setDefaultSaveDirectory(newDefaultDirectory);
					Directory.CreateDirectory(saveFile.FileName);
					File.Copy(this.runFilesDirectory + "/HT.txt", saveFile.FileName + "/HT.txt");
					File.Copy(this.runFilesDirectory + "/LT.txt", saveFile.FileName + "/LT.txt");
					File.Copy(this.runFilesDirectory + "/CT.txt", saveFile.FileName + "/CT.txt");
					File.Copy(this.runFilesDirectory + "/log.txt", saveFile.FileName + "/log.txt");
					this.resultsSaved = true;
				} catch (FileNotFoundException) {
					MessageBox.Show("Error while saving files: One or more files could not be found");
				} catch (UnauthorizedAccessException) {
					MessageBox.Show("Error while saving files: One or more files could not be accessed due to lack of authorization");
				} catch (Exception ex) {
					MessageBox.Show("Error while saving files:" + ex.Message);
				}
			}
		}

		/*
		 * This begins post-processing on a data set
		 */
		private void postProcessingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try {
				this.logFile.Close();
				this.htFile.Close();
				this.ltFile.Close();
				this.ctFile.Close();
			} catch (Exception) {

			}

			string logFileName = this.runFilesDirectory + "\\log.txt";
			string htFileName = this.runFilesDirectory + "\\HT.txt";
			string ltFileName = this.runFilesDirectory + "\\LT.txt";
			StreamReader logFileReader = new StreamReader(logFileName);
			StreamReader htFileReader = new StreamReader(htFileName);
			StreamReader ltFileReader = new StreamReader(ltFileName);
			double[] beforeAverage = new double[29];
			for (int i = 0; i < beforeAverage.Length; i++)
			{
				beforeAverage[i] = 0.0;
			}
			double[] afterAverage = new double[29];
			for (int i = 0; i < afterAverage.Length; i++)
			{
				afterAverage[i] = 0.0;
			}
			int cycle = 0;
			string line;
			while ((line = logFileReader.ReadLine()) != null)
			{
				if (line.Contains("MNPs"))
				{
					int end = line.IndexOf("\t");
					cycle = System.Convert.ToInt32(line.Substring(0, end));
				}
			}
			logFileReader.Close();
			int startPreCycle = cycle - configFile.sampleAverageCount;
			int endPreCycle = cycle - 1;
			int startPostCycle = cycle + configFile.diffusionCount + 1;
			int endPostCycle = startPostCycle + configFile.sampleAverageCount - 1;
			for (int i = 0; i < startPreCycle; i++)
			{
				htFileReader.ReadLine();
				ltFileReader.ReadLine();
			}
			for (int i = startPreCycle; i < endPreCycle + 1; i++)
			{
				if (configFile.postProcessingFiles == 0)
				{
					line = ltFileReader.ReadLine();
					for (int sensor = 0; sensor < beforeAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							beforeAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							beforeAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						beforeAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
				}
				else if (configFile.postProcessingFiles == 1)
				{
					line = htFileReader.ReadLine();
					for (int sensor = 0; sensor < beforeAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							beforeAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							beforeAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						beforeAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
				}
				else
				{
					line = ltFileReader.ReadLine();
					for (int sensor = 0; sensor < beforeAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							beforeAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							beforeAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						beforeAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
					line = htFileReader.ReadLine();
					for (int sensor = 0; sensor < beforeAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							beforeAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							beforeAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						beforeAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
				}
			}
			for (int i = 0; i < beforeAverage.Length; i++)
			{
				if (configFile.postProcessingFiles == 2)
					beforeAverage[i] /= (configFile.sampleAverageCount * 2);
				else
					beforeAverage[i] /= configFile.sampleAverageCount;
			}

			ltFileReader.ReadLine(); // Skip the MNPs-added cycle
			htFileReader.ReadLine(); // Skip the MNPs-added cycle

			for (int i = startPostCycle; i < endPostCycle + 1; i++)
			{
				if ((configFile.postProcessingFiles == 0) || (configFile.postProcessingFiles == 2))
				{
					line = ltFileReader.ReadLine();
					for (int sensor = 0; sensor < afterAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							afterAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							afterAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						afterAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
				}
				else if ((configFile.postProcessingFiles == 1) || (configFile.postProcessingFiles == 2))
				{
					line = htFileReader.ReadLine();
					for (int sensor = 0; sensor < afterAverage.Length; sensor++)
					{
						/*if (sensor < 9)
							afterAverage[sensor] += double.Parse(line.Substring(0, 10));
						else
							afterAverage[sensor] += double.Parse(line.Substring(0, 11));*/
						afterAverage[sensor] += double.Parse(line.Substring(0, line.IndexOf("\t")));
						line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
					}
				}
			}
			htFileReader.Close();
			ltFileReader.Close();
			for (int i = 0; i < afterAverage.Length; i++)
			{
				if (configFile.postProcessingFiles == 2)
					afterAverage[i] /= ((configFile.sampleAverageCount + configFile.diffusionCount) * 2);
				else
					afterAverage[i] /= (configFile.sampleAverageCount + configFile.diffusionCount);
			}
			double beforeTotal = 0.0;
			double afterTotal = 0.0;
			int count = 0;
			for (int i = 0; i < 5; i++)
			{
				foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])
					{
						try {
							if (c.Checked)
							{
								if (referenceSensors[i] < 16)
								{
									beforeTotal += beforeAverage[referenceSensors[i] - 1];
									afterTotal += afterAverage[referenceSensors[i] - 1];
								}
								else
								{
									beforeTotal += beforeAverage[referenceSensors[i] - 2];
									afterTotal += afterAverage[referenceSensors[i] - 2];
								}
								count++;
							}
						} catch (ArgumentNullException) {

						} catch (ArgumentOutOfRangeException) {

						}
					}
				}
			}

			if (count > 0)
			{
				beforeTotal /= count;
				afterTotal /= count;
			}
			else
			{
				beforeTotal = 0.0;
				afterTotal = 0.0;
			}

			for (int i = 0; i < beforeAverage.Length; i++)
			{
				beforeAverage[i] -= beforeTotal;
				afterAverage[i] -= afterTotal;
			}

			PostProcessingResults postProcessingResultsWindow = new PostProcessingResults(beforeAverage, afterAverage, this.runFilesDirectory,
																this.reactionWellTextBox.Text, this.sampleTextBox.Text,
																this.sensorAssignment);
			postProcessingResultsWindow.ShowDialog();
		}

		/*
		 * This starts the Open dialog and loads the opened run files into the charts for viewing
		 */
		private void openRunToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.resultsSaved)
			{
				if (MessageBox.Show("Save results first?", "Save", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
				{
					this.saveRunFilesAsToolStripMenuItem.PerformClick();
				}
			}

			OpenFileDialog openFile = new OpenFileDialog();
			openFile.InitialDirectory = this.configFile.defaultSaveDirectory;
			openFile.Multiselect = false;
			if (openFile.ShowDialog() == DialogResult.OK)
			{
				if(!openFile.FileName.Contains("log.txt"))
				{
					MessageBox.Show("That is not a valid log.txt file");
					return;
				}
				try
				{
					string logFileName = openFile.FileName;

					string newString = logFileName.Substring(0, logFileName.LastIndexOf("\\"));
					int last = newString.LastIndexOf("\\");
					this.configFile.setDefaultSaveDirectory(logFileName.Substring(0, last));

					int startOfFileName = logFileName.LastIndexOf("\\");
					this.runFilesDirectory = logFileName.Substring(0, startOfFileName);
					string htFileName = logFileName.Substring(0, startOfFileName+1) + "HT.txt";
					string ltFileName = logFileName.Substring(0, startOfFileName+1) + "LT.txt";
					string ctFileName = logFileName.Substring(0, startOfFileName+1) + "CT.txt";
					StreamReader logFileReader = new StreamReader(logFileName);
					StreamReader htFileReader = new StreamReader(htFileName);
					StreamReader ltFileReader = new StreamReader(ltFileName);
					StreamReader ctFileReader = new StreamReader(ctFileName);
					this.globalCycle = 0;
					this.mostRecentAddMpsCycle = 0;
					this.tareIndex = 0;
					this.tareIndexTextbox.Text = "0";
					foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
					{
						foreach (Chart c in t.Controls.OfType<Chart>())
						{
							c.ChartAreas[0].AxisX.Minimum = this.globalCycle;
							c.ChartAreas[0].AxisX.StripLines.Clear();
							foreach (Series s in c.Series)
							{
								s.Points.Clear();
								s.Points.AddXY(this.globalCycle + getAddTime(System.Convert.ToInt32(s.Name)), 0);
								s.Points.Last().MarkerStyle = MarkerStyle.Circle;
							}
						}
					}
					string line;
					htFileReader.ReadLine();
					ltFileReader.ReadLine();
					ctFileReader.ReadLine();
					globalCycle++;
					for (int i = 0; ; i++)
					{
						int nullCount = 0;
						if ((line = htFileReader.ReadLine()) != null)
						{
							for (int j = 0; ; j++)
							{
								if (line == "")
									break;
								if (j == 16)
									continue;
								this.rawChart1.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								this.adjustedChart1.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
							}
						}
						else
							nullCount++;
						if ((line = ltFileReader.ReadLine()) != null)
						{
							for (int j = 0; ; j++)
							{
								if (line == "")
									break;
								if (j == 16)
									continue;
								this.rawChart2.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								this.adjustedChart2.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
							}
						}
						else
							nullCount++;
						if ((line = ctFileReader.ReadLine()) != null)
						{
							for (int j = 0; ; j++)
							{
								if (line == "")
									break;
								if (j == 16)
									continue;
								this.rawChart3.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								this.adjustedChart3.Series[j].Points.AddXY(getAddTime(j) + globalCycle, double.Parse(line.Substring(0, line.IndexOf("\t"))));
								line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
							}
						}
						else
							nullCount++;
						if (nullCount >= 3)
						{
							globalCycle--;		// Because we incremented global cycle before we discovered that our last line was null/empty,
							break;			// it will be pointing to that cycle. GlobalCycle is ALWAYS pointing to the last uncompleted row
						}					// of data points, so we need to move it back to that row (because we can't verify it had exactly
						else					// 29 data points, one for each sensor (excluding 16)
							globalCycle++;
					}
					htFileReader.Close();
					ltFileReader.Close();
					ctFileReader.Close();
					while ((line = logFileReader.ReadLine()) != null)
					{
						if (line.Contains("Reaction Well:"))
						{
							line = line.Replace("\t", "");
							int first = line.IndexOf(":");
							string me = line.Substring(first + 1, line.Length - first - 1);
							this.reactionWellTextBox.Text = line.Substring(first + 1, line.Length - first - 1);
							this.validateReactionWellButton.PerformClick();
							if (!this.reactionWellValidated)
								return;
						}
						if (line.Contains("Sample:"))
						{
							line = line.Replace("\t", "");
							int first = line.IndexOf(":");
							string me = line.Substring(first + 1, line.Length - first - 1);
							this.sampleTextBox.Text = line.Substring(first + 1, line.Length - first - 1);
						}
						if(line.Contains("Buffer"))
						{
							if (line.Contains("Preload"))
								continue;
							int end = line.IndexOf("\t");
							foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
							{
								foreach (Chart c in t.Controls.OfType<Chart>())
								{
									StripLine strip = new StripLine();
									strip.Text = "Buffer";
									strip.TextOrientation = TextOrientation.Horizontal;
									strip.IntervalOffset = System.Convert.ToInt32(line.Substring(0, end));
									strip.StripWidth = 1;
									strip.BackColor = Color.FromArgb(0xDD, 0xDD, 0xDD);
									c.ChartAreas[0].AxisX.StripLines.Add(strip);
								}
							}
						}
						if (line.Contains("MNPs"))
						{
							int end = line.IndexOf("\t");
							this.mostRecentAddMpsCycle = System.Convert.ToInt32(line.Substring(0, end));
							foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())
							{
								foreach (Chart c in t.Controls.OfType<Chart>())
								{
									StripLine strip = new StripLine();
									strip.Text = "\nMNPs";
									strip.TextOrientation = TextOrientation.Horizontal;
									strip.IntervalOffset = System.Convert.ToInt32(line.Substring(0, end));
									strip.StripWidth = 1;
									strip.BackColor = Color.FromArgb(0x99, 0x99, 0x99);
									c.ChartAreas[0].AxisX.StripLines.Add(strip);
								}
							}
						}
					}
					logFileReader.Close();

					//globalCycle++;		// Because recalculateData() operates in the scope of the VISIBLE charts, we need to cheat and say we're
					recalculateData();	// a cycle ahead so we can view all of our available data (note: we'll see more data points than we did when we saved the files)
					//globalCycle--;		// Reverse our hack

					validatePostProcessing();

					//globalCycle += 2;		// Increment the global cycle again in case recalculateData() is called again
				} catch (FileNotFoundException) {

				}
			}
		}

		/*
		 * This exits the GUI if we aren't running
		 */
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.running)
			{
				MessageBox.Show("Please stop before exiting");
				return;
			}

			this.Close();
		}
	}
}
