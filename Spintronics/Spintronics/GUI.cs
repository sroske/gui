#define _DEBUG

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
using System.Threading;

namespace SpintronicsGUI
{
	enum PacketCommDirection
	{
		In,
		Out
	}

	public partial class GUI : Form
	{
		string comPortName;								// String that keeps the name of the COM port the user wants to use
		Configuration configFile;							// File containing user configurations
		SerialPort serialPort = null;							// Main microcontroller COM port
		SerialPort debugSerial = null;						// COM port used for debugging with microcontroller emulator
		ProtocolHandler protocolHandler;						// Protocol handler object
		Microcontroller microcontroller;						// Microcontroller emulator (debugging only)
		string runFilesDirectory;							// String containing the directory of the current run files
		TextWriter logFile = null;							// Log file writer
		TextWriter f1Mf2AFile = null;							// LT file writer
		TextWriter f1AFile = null;							// CT file writer
		TextWriter f1Pf2AFile = null;							// HT file writer
		TextWriter f1PFile = null;							// Another file writer
		TextWriter f2AFile = null;							// Same as above
		TextWriter f2PFile = null;							// Same as above
		TextWriter f1Mf2PFile = null;							// Same as above
		TextWriter f1Pf2PFile = null;							// Same as above
		TextWriter coilAFile = null;							// Same as above
		TextWriter coilPFile = null;							// Same as above
		string f1Mf2AFileName = "WS-F1MF2A.txt";					// File name
		string f1AFileName = "WS-F1A.txt";						// Same as above
		string f1Pf2AFileName = "WS-F1PF2A.txt";					// Same as above
		string f1PFileName = "WS-F1P.txt";						// Same as above
		string f2AFileName = "WS-F2A.txt";						// Same as above
		string f2PFileName = "WS-F2P.txt";						// Same as above
		string f1Mf2PFileName = "WS-F1MF2P.txt";					// Same as above
		string f1Pf2PFileName = "WS-F1PF2P.txt";					// Same as above
		string coilAFileName = "COIL-F2A.txt";					// Same as above
		string coilPFileName = "COIL-F2P.txt";					// Same as above
		bool running = false;								// Tells us whether or not a run is in progress
		bool resultsSaved = true;							// Tells us if the current results have been saved
		bool reactionWellValidated = false;						// Tells us if the Reaction Well name is valid
		SensorAssignment sensorAssignment = SensorAssignment.A;		// Tells us the sensor pin assignments for the physical sensor array being used
		public delegate void addNewDataPoint(Packet packet);			// Delegate for adding packets
		public addNewDataPoint addNewDataPointDelegate;				// See above
		public delegate void addDataErrorToTextBox(int sensor,		// Delagate for adding data error text to text box
									 int cycle,
									 string message);
		public addDataErrorToTextBox addNewDataErrorDelegate;			// See above
		int globalCycle = 0;								// The global cycle pointer; This always points to the cycle CURRENTLY BEING ACQUIRED AND RECORDED
		int tareIndex = 0;								// The global tare index (what is the first cycle of data the user can see)
		bool recalculate = true;							// Tells us if we should recalculate the adjusted chart points
		int latestSensorId = 0;								// Keeps track of the latest sensor we've received data for
		int demoModeSensorsCount;							// Keeps track of the number of sensors we expect to receive for a cycle in Demo Mode
		int demoModeCycleSensorsReceivedCount = 0;				// Keeps track of the number of sensors we've received data for this cycle for demo mode
		int mostRecentAddMpsCycle = 0;						// Keeps track of the latest cycle MNPs were added
		int enableAddBufferAndMnpAtCycle;						// Tells us when to re-enable the Add Buffer and Add MNPs buttons
		int[] referenceSensors = { 1, 2, 7, 29, 30 };				// Array containing the integer numbers of the reference sensors (they don't change)

		/*
		 * This is the constructor for the GUI that takes in a COM port name (e.g. "COM1")
		 */
		public GUI(string comPort)
		{
			// Initialize GUI controls
			InitializeComponent();

			// Create configuration object (automatically populated; see Configuration.cs)
			configFile = new Configuration();

			// Initialize delegate for adding new data to the graph
			// (this is because of threading rules)
			addNewDataPointDelegate = new addNewDataPoint(addNewDataPointMethod);
			addNewDataErrorDelegate = new addDataErrorToTextBox(addDataErrorMethod);

			// Initialize COM ports
			this.comPortName = comPort;
			reconnectToDevice();
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
			this.userNameTextBox.Focus();											// Put focus on the Reaction Well text box
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
			try {																				// (For all locations/specifications of data packets, see Coomunications document)
				int sensorId = 0;
				int sensorMultiplexerAddress = packet.payload[0];											// Get the sensor multiplexer address
				for (int i = 0; i < configFile.sensorMultiplexerValues.Length; i++)								// For each sensor multiplexer value in our array,
				{
					if (sensorMultiplexerAddress == configFile.sensorMultiplexerValues[i])							// if the member at i in the array matches our received address from the microcontroller,
					{
						sensorId = i + 1;															// then that's the sensor we received data for, so set sensor ID (incrementing i by 1 for the 0 indexing)
						break;																// We're done searching, so might as well break
					}
				}
				if (sensorId == 0)																// If we didn't find the address in our array,
				{
					// error																	// Let the user know we received bad data,
					return;																	// and don't bother adding any data
				}
				if (!this.demoModeToolStripMenuItem.Checked)
				{
					if (latestSensorId != 15)
					{
						if (sensorId != ((latestSensorId % 30) + 1))
						{
							fixNoncontinuityInLogFiles(sensorId, latestSensorId);
						}
					}
					else
					{
						if (sensorId != 17)
						{
							fixNoncontinuityInLogFiles(sensorId, latestSensorId);
						}
					}
				}

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
				rawChart1.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef1Mf2A);			// Add data to raw chart #1
				rawChart2.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef1A);				// Add data to raw chart #2
				rawChart3.Series[sensorId - 1].Points.AddXY(globalCycle + getAddTime(sensorId), wheatstonef1Pf2A);			// Add data to raw chart #3
				logData(f1Mf2AFile, sensorId, wheatstonef1Mf2A);											// Log the data in the appropriate file (these will be stored in 'temp' directory until saved)
				logData(f1AFile, sensorId, wheatstonef1A);												// See above
				logData(f1Pf2AFile, sensorId, wheatstonef1Pf2A);											// See above
				logData(f1PFile, sensorId, wheatstonef1P);												// See above
				logData(f2AFile, sensorId, wheatstonef2A);												// See above
				logData(f2PFile, sensorId, wheatstonef2P);												// See above
				logData(f1Mf2PFile, sensorId, wheatstonef1Mf2P);											// See above
				logData(f1Pf2PFile, sensorId, wheatstonef1Pf2P);											// See above
				logData(coilAFile, sensorId, wheatstoneCoilf2A);											// See above
				logData(coilPFile, sensorId, wheatstoneCoilf2P);											// See above

				// Add the data to the visible charts
				if (globalCycle > 1)																// If globalCycle is greater than 1 (i.e. we're done buffering),
				{
					wheatstonef1Mf2A = (float)rawChart1.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];				// grab the data points,
					wheatstonef1A = (float)rawChart2.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];
					wheatstonef1Pf2A = (float)rawChart3.Series[sensorId - 1].Points[globalCycle - 1].YValues[0];

					if (this.amplitudeTareCheckbox.Checked)												// if box is checked, subtract the sensor's amplitude value from cycle pointed at by tareIndex
					{
						wheatstonef1Mf2A -= (float)rawChart1.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];
						wheatstonef1A -= (float)rawChart2.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];
						wheatstonef1Pf2A -= (float)rawChart3.Series[sensorId - 1].Points.ElementAt(tareIndex).YValues[0];

						if (this.referenceTareCheckbox.Checked)											// if box is checked, subtract the average of the CHECKED reference sensors
						{
							wheatstonef1Mf2A -= (float)getReferenceAverage(this.rawChart1, (globalCycle - 1));
							wheatstonef1A -= (float)getReferenceAverage(this.rawChart2, (globalCycle - 1));
							wheatstonef1Pf2A -= (float)getReferenceAverage(this.rawChart3, (globalCycle - 1));
						}
					}

					adjustedChart1.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef1Mf2A);	// Add the data points,
					adjustedChart1.Series[sensorId - 1].Points.Last().MarkerStyle = getMarker(sensorId);				// and set their chart markers to circles
					adjustedChart2.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef1A);
					adjustedChart2.Series[sensorId - 1].Points.Last().MarkerStyle = getMarker(sensorId);
					adjustedChart3.Series[sensorId - 1].Points.AddXY(globalCycle - 1 + getAddTime(sensorId), wheatstonef1Pf2A);
					adjustedChart3.Series[sensorId - 1].Points.Last().MarkerStyle = getMarker(sensorId);
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
				latestSensorId = sensorId;
				if (this.demoModeToolStripMenuItem.Checked)
				{
					this.demoModeCycleSensorsReceivedCount++;
					if (this.demoModeCycleSensorsReceivedCount >= this.demoModeSensorsCount)
					{
						this.globalCycle++;
						this.demoModeCycleSensorsReceivedCount = 0;
					}
				}
				else
				{
					if (sensorId >= adjustedChart1.Series.Count)
						this.globalCycle++;
				}
			} catch (IndexOutOfRangeException) {
				
			} catch (ArgumentOutOfRangeException) {

			} catch (NullReferenceException) {

			}
		}

		/*
		 * This will get the time which the data should be added
		 * (globalTime plus an appropriate offset for the pin)
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
		 * This will get the marker style for the sensor (assigned using column of sensor)
		 */
		private MarkerStyle getMarker(int sensor)
		{
			if (sensorAssignment == SensorAssignment.A)
			{
				switch (sensor)
				{
					case 18: case 20: case 22: case 24: case 26: case 28: case 30:	// Column a offsets
						return MarkerStyle.Circle;
					case 17: case 19: case 21: case 23: case 25: case 27: case 29:	// Column b offsets
						return MarkerStyle.Diamond;
					case 15: case 13: case 11: case 9: case 6: case 4: case 2:		// Column c offsets
						return MarkerStyle.Triangle;
					case 14: case 12: case 10: case 8: case 5: case 3: case 1:		// Column d offsets
						return MarkerStyle.Cross;
					case 7:										// Column e offsets
						return MarkerStyle.Star5;
					default:										// Default (should not happen)
						return MarkerStyle.Circle;
				}
			}
			else
			{
				switch (sensor)
				{
					case 1: case 3: case 5: case 8: case 10: case 12: case 14:		// Column a offsets
						return MarkerStyle.Circle;
					case 2: case 4: case 6: case 9: case 11: case 13: case 15:		// Column b offsets
						return MarkerStyle.Diamond;
					case 29: case 27: case 25: case 23: case 21: case 19: case 17:	// Column c offsets
						return MarkerStyle.Triangle;
					case 30: case 28: case 26: case 24: case 22: case 20: case 18:	// Column d offsets
						return MarkerStyle.Cross;
					case 7:										// Column e offsets
						return MarkerStyle.Star5;
					default:										// Default (should not happen)
						return MarkerStyle.Circle;
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

			for (int i = 0; i < 5; i++)																				// For each reference sensor in our system,
			{
				foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
				{
					if (getSensorNumber(c.Name) == referenceSensors[i])														// if the check box is for a reference sensor,
					{
						try {
							if (c.Checked)																		// and the sensor is enabled,
							{
								double difference = double.NegativeInfinity;
								int k;
								for (k = 0; k < chart.Series[referenceSensors[i] - 1].Points.Count; k++)							// and the sensor has a data point with an X value
								{
									if (chart.Series[referenceSensors[i] - 1].Points[k].XValue == cycle + getAddTime(referenceSensors[i]))	// that matches what we're looking for,
									{
										difference = chart.Series[referenceSensors[i] - 1].Points[k].YValues[0];					// grab the data point
										break;
									}
								}
								if (difference == double.NegativeInfinity)												// Otherwise, if there was no available point,
									continue;																	// don't bother adding anything to the running total

								for (k = 0; k < chart.Series[referenceSensors[i] - 1].Points.Count; k++)							// Use the same process as above to subtract the value at the tare
								{																			// index (if one exists)
									if (chart.Series[referenceSensors[i] - 1].Points[k].XValue == tareIndex + getAddTime(referenceSensors[i]))
									{
										difference -= chart.Series[referenceSensors[i] - 1].Points.ElementAt(k).YValues[0];
										break;
									}
								}
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
		 * This is used to do a flush of the current data in the visible charts
		 * and a total recalculation of all to-be-displayed data points
		 */
		private void recalculateData()
		{
			if (!recalculate)														// If we don't need to recalculate, don't
				return;
			foreach (TabPage t in this.tabControl1.Controls.OfType<TabPage>())					// Otherwise, for each chart in each tab page,
			{
				foreach (Chart c in t.Controls.OfType<Chart>())
				{
					if (c.Name.Contains("adjusted"))									// that has "adjusted" in its name (i.e. it is a visible one),
					{
						c.ChartAreas[0].AxisX.Minimum = tareIndex;						// set the minimum viewing place to tare index (we only care about points after it),
						foreach (Series s in c.Series)								// and, for each series (i.e. set of data for one sensor),
						{
							try {
								s.Points.Clear();									// clear the previous points (since they need to be recalculated)
								for(int i = tareIndex; i < globalCycle; i++)				// Then, for each cycle until the most recent one, starting from tare index,
								{
									if (globalCycle != 1)							// If it isn't the first cycle,
									{										// and we're now adding points for the last visible cycle,
										if ((i == (globalCycle - 1)) && (System.Convert.ToInt32(s.Name) > latestSensorId))	// if the sensor ID is greater than the latest received for this cycle,
											continue;							// (i.e. we haven't received the point yet and shouldn't try to show it), continue without adding
									}
									int sensorId = System.Convert.ToInt32(((Series)s).Name);	// Get the sensor ID
									Chart chart;								// Declare a temporary chart
									if(c.Name.Contains("1")) {						// Set the temp chart to the appropriate raw-data chart
										chart = this.rawChart1;
									} else if (c.Name.Contains("2")) {
										chart = this.rawChart2;
									} else {
										chart = this.rawChart3;
									}

									double value = double.NaN;
									for (int k = 0; k < chart.Series[sensorId - 1].Points.Count; k++)					// We need to handle missing data points,
									{															// so for each data point available,
										if (chart.Series[sensorId - 1].Points[k].XValue == i + getAddTime(sensorId))		// if the X value matches the cycle we want,
										{
											value = chart.Series[sensorId - 1].Points[k].YValues[0];				// use that value
											break;
										}
									}
									if (value == double.NaN)											// If we didn't find a data point, that means there was none,
										continue;													// so don't bother adding data for this cycle for this point

									if (this.amplitudeTareCheckbox.Checked)									// If we need to factor in the amplitude tare,
									{															// subtract the amplitude of this sensor at index with X value = tare index
										for (int k = 0; k < chart.Series[sensorId - 1].Points.Count; k++)				// (this uses the same process as above)
										{
											if (chart.Series[sensorId - 1].Points[k].XValue == tareIndex + getAddTime(sensorId))
											{
												value -= (float)chart.Series[sensorId - 1].Points.ElementAt(k).YValues[0];
												break;
											}
										}

										if (this.referenceTareCheckbox.Checked)			// If we need to factor in the reference tare,
										{
											value -= (float)getReferenceAverage(chart, i);	// subtract the reference average from this sensor
										}
									}

									double time = getAddTime(sensorId);					// Get the offset for this sensor,
									time += (double)i;							// and include it
									s.Points.AddXY(time, value);						// Add the sensor's data
									s.Points.Last().MarkerStyle = getMarker(sensorId);		// Set the marker style
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
			byte startOfFrame;
			try {
				while(true) {                                                                       // Keep reading until we either
					try {
						if ((startOfFrame = (byte)serialPort.ReadByte()) == 0xFE)                   // hit a start of frame,
							break;
						else
							Console.WriteLine("<-<-<- Malformed packet received. Started with 0x{0:X}", startOfFrame);
					} catch (TimeoutException) {                                                    // or we run out of bytes to read (this will clear out the buffer in the event we don't get a valid SOF)
						return;
					}
				}
				Packet packet;
				byte command = (byte)serialPort.ReadByte();                                         // Read the command byte
				byte payloadLength = (byte)serialPort.ReadByte();                                   // Read the payload length byte
				Thread.Sleep(100);                                                                  // The GUI seems to hanve trouble going right into the serialPort.Read(...), so I put in a delay
				byte[] payload = new byte[payloadLength];                                           // Make the payload buffer
				if (serialPort.Read(payload, 0, payloadLength) < payloadLength)                     // Read in the whole payload
				{
					Console.WriteLine("<-<-<- Payload length did not match. Was 0x{0:X2}, got 0x{0:X}", payloadLength, payload);
					Console.WriteLine("<-<-<- (cont) Command was 0x{0:X2}", command);
					return;
				}
				byte Xor = (byte)serialPort.ReadByte();
				packet = new Packet(command, payloadLength, payload);
				printPacket(packet, PacketCommDirection.In);
				if (packet.Xor != Xor)
				{
					Console.WriteLine("<-<-<- XOR did not match. Received 0x{0:X}, should have been 0x{0:X}", Xor, packet.Xor);
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
					case (byte)((byte)PacketType.Config | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received a config acknowledge packet");
						break;
					case (byte)((byte)PacketType.Error | (byte)PacketSender.Microcontroller):
						Console.WriteLine("Received an error packet");
						break;
					default:
						Console.WriteLine("Received an unknown packet of type 0x{0:X}", packet.command);
                        return;
				}

				ProtocolDirective retval = protocolHandler.HandlePacket(packet);
				if (retval == ProtocolDirective.AddData && this.running)
				{
					if (InvokeRequired)
						this.Invoke(this.addNewDataPointDelegate, packet);
				}
				if (retval == ProtocolDirective.ErrorReceived && this.running)
				{
					if (InvokeRequired)
						this.Invoke(this.addNewDataErrorDelegate, this.latestSensorId + 1, this.globalCycle, protocolHandler.getErrorMessage());
					//addDataErrorToTextBox(this.latestSensorId + 1, this.globalCycle, protocolHandler.getErrorMessage());
				}

			} catch (ArgumentNullException) {
				MessageBox.Show("Argument Null Exception in GUI, most likely thrown by ProtocolHandler");
			} catch (InvalidOperationException e) {
				MessageBox.Show(e.Message);
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
		 * This returns the pin number of each sensor check box according to
		 * the current pin assignment
		 */
		private int getSensorNumber(string name)
		{
			if (sensorAssignment == SensorAssignment.A)
				return System.Convert.ToInt32(name.Substring(1, 2));
			else
				return System.Convert.ToInt32(name.Substring(4, 2));
		}

		/*
		 * This enables and disables the showing of the sensor data in each
		 * chart according to the sensor check boxes
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
			//for (int i = 0; i < 5; i++)
			//{
			//	if (number == referenceSensors[i])
			//	{
					recalculateData();
			//	}
			//}
		}

		/*
		 * This sets the charts' legend text for each sensor according to its
		 * position in the array (1-7, A-E)
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
			/* Check that we aren't already running */
			if (this.running == true)
			{
				MessageBox.Show("Please stop current session before starting a new session");
				return;
			}
			/* Check that valid names are entered */
			if ((this.reactionWellTextBox.Text == "") || (this.sampleTextBox.Text == "") || (this.userNameTextBox.Text == ""))
			{
				MessageBox.Show("Please enter values for Reaction Well, Sample, and User Name");
				return;
			}
			/* Validate the Reaction Well name (the sensor pin assignment depends on this) */
			this.validateReactionWellButton.PerformClick();
			if (!this.reactionWellValidated)
			{
				return;
			}
			/* Ensure we are connected to the microcontroller COM ports before starting */
			if (!reconnectToDevice())
			{
				return;
			}
			/* Send a start packet to the microcontroller */
			try {
				// Create configuration packet (and omit sensor 16 from the config array)
				byte[] payload;
				/* If the user wants to use Demo Mode, defer to the Demo Mode method for the configuration packet payload */
				if (this.demoModeToolStripMenuItem.Checked)
				{
					payload = demoModeStart();
					if (payload == null)
						return;
				}
				else
				{
					payload = new byte[this.configFile.sensorMultiplexerValues.Length - 1];
					Array.Copy(this.configFile.sensorMultiplexerValues, payload, 15);
					Array.Copy(this.configFile.sensorMultiplexerValues, 16, payload, 15, this.configFile.sensorMultiplexerValues.Length - 15 - 1);
				}
				Packet configPacket = new Packet((byte)PacketType.Config | (byte)PacketSender.GUI, (byte)payload.Length, payload);
				printPacket(configPacket, PacketCommDirection.Out);
				// Create start packet
				float[] data = new float[5];
				payload = new byte[21];
				data[0] = this.configFile.wheatstoneAmplitude;
				data[1] = this.configFile.wheatstoneFrequency;
				data[2] = this.configFile.coilAmplitude;
				data[3] = this.configFile.coilFrequency;
				data[4] = this.configFile.measurementPeriod;
				// Scale down if units other than V are specified (options are V, mV, uV, or nV)
				if (this.configFile.wheatstoneAmplitudeUnit.Contains("m"))
					data[0] = data[0] / 1000;
				else if (this.configFile.wheatstoneAmplitudeUnit.Contains("u"))
					data[0] = data[0] / 1000000;
				else if (this.configFile.wheatstoneAmplitudeUnit.Contains("n"))
					data[0] = data[0] / 1000000000;

				if (this.configFile.coilAmplitudeUnit.Contains("m"))
					data[2] = data[2] / 1000;
				else if (this.configFile.coilAmplitudeUnit.Contains("u"))
					data[2] = data[2] / 1000000;
				else if (this.configFile.coilAmplitudeUnit.Contains("n"))
					data[2] = data[2] / 1000000000;

				Buffer.BlockCopy(data, 0, payload, 0, payload.Length - 1);
				payload[20] = (byte)configFile.digitalGainFactor;
				Packet startPacket = new Packet((byte)PacketType.Start | (byte)PacketSender.GUI, (byte)payload.Length, payload);
				printPacket(startPacket, PacketCommDirection.Out);
				if (protocolHandler.StartRun(configPacket, startPacket) == true)
					this.running = true;
				else
				{
					MessageBox.Show("Failed to start: Error code " + protocolHandler.errorCode +
								"\n-> " + protocolHandler.getErrorMessage());
					return;
				}
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
				return;
			} catch (FormatException) {
				MessageBox.Show("Please enter a valid number for all fields");
				return;
			} catch (OverflowException) {
				MessageBox.Show("Please enter a valid number for all fields");
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
			this.demoModeToolStripMenuItem.Enabled = false;
			this.demoModeToolStripMenuItem.ToolTipText = "You cannot change this during a run";

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
			if (protocolHandler.StopRun(stopPacket) != true)
			{
				MessageBox.Show("Failed to stop: Error code " + protocolHandler.errorCode +
						    "\n-> " + protocolHandler.getErrorMessage() +
						    "\nExiting");
			}
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

			this.demoModeToolStripMenuItem.Enabled = true;
			this.demoModeToolStripMenuItem.ToolTipText = "";
			this.stopRunButton.Enabled = false;
			this.stopRunToolStripMenuItem.Enabled = false;
			this.addMnpButton.Enabled = false;
			this.addBufferButton.Enabled = false;

			try {
				this.logFile.Close();
				this.f1Mf2AFile.Close();
				this.f1AFile.Close();
				this.f1Pf2AFile.Close();
				this.f1PFile.Close();
				this.f2AFile.Close();
				this.f2PFile.Close();
				this.f1Mf2PFile.Close();
				this.f1Pf2PFile.Close();
				this.coilAFile.Close();
				this.coilPFile.Close();
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
		 * This enables and disables the option to change the reference-tare
		 * check box according to the amplitude tare check box state
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
		 * This enables and disables showing reference sensors in charts according
		 * to the reference tare check box
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
		 * This initializes a data file by printing out the sensor name headers
		 * at the top of the file
		 */
		private void createRunFiles()
		{
			try {
				try {
					logFile.Close();
					f1Mf2AFile.Close();
					f1AFile.Close();
					f1Pf2AFile.Close();
					f1Pf2AFile.Close();
					f1PFile.Close();
					f2AFile.Close();
					f2PFile.Close();
					f1Mf2PFile.Close();
					f1Pf2PFile.Close();
					coilAFile.Close();
					coilPFile.Close();
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
				f1Mf2AFile = new StreamWriter(logFileNameBase + f1Mf2AFileName);
				f1AFile = new StreamWriter(logFileNameBase + f1AFileName);
				f1Pf2AFile = new StreamWriter(logFileNameBase + f1Pf2AFileName);
				f1PFile = new StreamWriter(logFileNameBase + f1PFileName);
				f2AFile = new StreamWriter(logFileNameBase + f2AFileName);
				f2PFile = new StreamWriter(logFileNameBase + f2PFileName);
				f1Mf2PFile = new StreamWriter(logFileNameBase + f1Mf2PFileName);
				f1Pf2PFile = new StreamWriter(logFileNameBase + f1Pf2PFileName);
				coilAFile = new StreamWriter(logFileNameBase + coilAFileName);
				coilPFile = new StreamWriter(logFileNameBase + coilPFileName);

				logFile.WriteLine("User Name:\t" + this.userNameTextBox.Text);
				logFile.WriteLine("Reaction Well:\t" + this.reactionWellTextBox.Text);
				logFile.WriteLine("Sample:\t\t" + this.sampleTextBox.Text + "\n");
				logFile.WriteLine("Measurement Parameters:\n");
				logFile.WriteLine("\tWheatstone Amplitude:\t" + this.configFile.wheatstoneAmplitude + " " + this.configFile.wheatstoneAmplitudeUnit);
				logFile.WriteLine("\tWheatstone Frequency:\t" + this.configFile.wheatstoneFrequency + " Hz");
				logFile.WriteLine("\tCoil Amplitude:\t\t" + this.configFile.coilAmplitude + " " + this.configFile.coilAmplitudeUnit);
				logFile.WriteLine("\tCoil Frequency:\t\t" + this.configFile.coilFrequency + " Hz");
				logFile.WriteLine("\tCoil DC Offset:\t\t" + this.configFile.coilDcOffset + " " + this.configFile.coilDcOffsetUnit);
				logFile.WriteLine("\tMeasurement Period:\t" + this.configFile.measurementPeriod + " seconds\n");
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
					f1Mf2AFile.Write("Sensor " + (i + 1) + "\t");
					f1AFile.Write("Sensor " + (i + 1) + "\t");
					f1Pf2AFile.Write("Sensor " + (i + 1) + "\t");
					f1PFile.Write("Sensor " + (i + 1) + "\t");
					f2AFile.Write("Sensor " + (i + 1) + "\t");
					f2PFile.Write("Sensor " + (i + 1) + "\t");
					f1Mf2PFile.Write("Sensor " + (i + 1) + "\t");
					f1Pf2PFile.Write("Sensor " + (i + 1) + "\t");
					coilAFile.Write("Sensor " + (i + 1) + "\t");
					coilPFile.Write("Sensor " + (i + 1) + "\t");
				}
				f1Mf2AFile.Write("\n");
				f1AFile.Write("\n");
				f1Pf2AFile.Write("\n");
				f1PFile.Write("\n");
				f2AFile.Write("\n");
				f2PFile.Write("\n");
				f1Mf2PFile.Write("\n");
				f1Pf2PFile.Write("\n");
				coilAFile.Write("\n");
				coilPFile.Write("\n");
				f1Mf2AFile.Flush();
				f1AFile.Flush();
				f1Pf2AFile.Flush();
				f1PFile.Flush();
				f2AFile.Flush();
				f2PFile.Flush();
				f1Mf2PFile.Flush();
				f1Pf2PFile.Flush();
				coilAFile.Flush();
				coilPFile.Flush();

			} catch (Exception) {
				DialogResult messageBoxResult = MessageBox.Show("Unable to create run files. Continue?", "Error", MessageBoxButtons.YesNo);
				if (messageBoxResult != DialogResult.Yes)
					throw new Exception();
			}
		}

		/*
		 * This adds data to the data log file
		 */
		private void logData(TextWriter file, int sensor, double data, bool noData = false)
		{
			string dataString;
			if (noData)
				dataString = "N/A";
			else
				dataString = System.Convert.ToString(data);

			file.Write(dataString + "\t");
			if (sensor == 30 || this.demoModeToolStripMenuItem.Checked)
			{
				file.Write("\n");
			}
			file.Flush();
		}

		/*
		 * This will handle the case where we don't receive sequentially-occurring sensor IDs
		 * (i.e. a sensor was skipped or missed)
		 */
		private void fixNoncontinuityInLogFiles(int newId, int previousId)
		{
			previousId++;									// Since we're going to log this sensors
			if (previousId == 16)								// data, we only want to write
				previousId++;								// <difference> - 1 'N/A's
			else if (previousId == 31)
				previousId = 1;

			for (; previousId != newId; )
			{
				logData(this.f1Mf2AFile, previousId, 0.0, noData: true);
				logData(this.f1AFile, previousId, 0.0, noData: true);
				logData(this.f1Pf2AFile, previousId, 0.0, noData: true);
				logData(this.f1PFile, previousId, 0.0, noData: true);
				logData(this.f2AFile, previousId, 0.0, noData: true);
				logData(this.f2PFile, previousId, 0.0, noData: true);
				logData(this.f1Mf2PFile, previousId, 0.0, noData: true);
				logData(this.f1Pf2PFile, previousId, 0.0, noData: true);
				logData(this.coilAFile, previousId, 0.0, noData: true);
				logData(this.coilPFile, previousId, 0.0, noData: true);
				previousId++;
				if (previousId == 16)
					previousId++;
				else if (previousId == 31)
					previousId = 1;
			}
			addDataErrorMethod(newId, this.globalCycle, "Sensor was not expected next sensor");
		}

		/*
		 * This adds error text to the dataErrorTextBox control
		 */
		private void addDataErrorMethod(int sensor, int cycle, string message)
		{
			this.dataErrorTextBox.Visible = true;
			this.dataErrorLabel.Visible = true;
			this.dataErrorTextBox.Text += sensor + "\t" + cycle + "\t" + message + Environment.NewLine;
			this.dataErrorTextBox.SelectionStart = this.dataErrorTextBox.TextLength;
			this.dataErrorTextBox.ScrollToCaret();
			this.dataErrorTextBox.Refresh();
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
					configFile.setDigitalGainFactor(preferenceWindow.digitalGainFactor);
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
				this.f1Mf2AFile.Close();
				this.f1AFile.Close();
				this.f1Pf2AFile.Close();
				this.f1PFile.Close();
				this.f2AFile.Close();
				this.f2PFile.Close();
				this.f1Mf2PFile.Close();
				this.f1Pf2PFile.Close();
				this.coilAFile.Close();
				this.coilPFile.Close();
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
					File.Copy(this.runFilesDirectory + "/log.txt", saveFile.FileName + "/log.txt");
					File.Copy(this.runFilesDirectory + "/" + f1Mf2AFileName, saveFile.FileName + "/" + f1Mf2AFileName);
					File.Copy(this.runFilesDirectory + "/" + f1AFileName, saveFile.FileName + "/" + f1AFileName);
					File.Copy(this.runFilesDirectory + "/" + f1Pf2AFileName, saveFile.FileName + "/" + f1Pf2AFileName);
					File.Copy(this.runFilesDirectory + "/" + f1PFileName, saveFile.FileName + "/" + f1PFileName);
					File.Copy(this.runFilesDirectory + "/" + f2AFileName, saveFile.FileName + "/" + f2AFileName);
					File.Copy(this.runFilesDirectory + "/" + f2PFileName, saveFile.FileName + "/" + f2PFileName);
					File.Copy(this.runFilesDirectory + "/" + f1Mf2PFileName, saveFile.FileName + "/" + f1Mf2PFileName);
					File.Copy(this.runFilesDirectory + "/" + f1Pf2PFileName, saveFile.FileName + "/" + f1Pf2PFileName);
					File.Copy(this.runFilesDirectory + "/" + coilAFileName, saveFile.FileName + "/" + coilAFileName);
					File.Copy(this.runFilesDirectory + "/" + coilPFileName, saveFile.FileName + "/" + coilPFileName);
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
				this.f1Mf2AFile.Close();
				this.f1AFile.Close();
				this.f1Pf2AFile.Close();
			} catch (Exception) {
			}

			string logFileName = this.runFilesDirectory + "\\log.txt";
			string ltFileName = this.runFilesDirectory + "\\" + f1Mf2AFileName;
			string htFileName = this.runFilesDirectory + "\\" + f1Pf2AFileName;
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
					break;
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
					if(line.Contains("N/A"))
					{
						MessageBox.Show("Data files contain incomplete data ('N/A' entries)\n" + "Not continuing with post-processing");
						htFileReader.Close();
						ltFileReader.Close();
						return;
					}
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
					if (line.Contains("N/A"))
					{
						MessageBox.Show("Data files contain incomplete data ('N/A' entries)\n" + "Not continuing with post-processing");
						htFileReader.Close();
						ltFileReader.Close();
						return;
					}
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
					if (line.Contains("N/A"))
					{
						MessageBox.Show("Data files contain incomplete data ('N/A' entries)\n" + "Not continuing with post-processing");
						htFileReader.Close();
						ltFileReader.Close();
						return;
					}
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
					if (line.Contains("N/A"))
					{
						MessageBox.Show("Data files contain incomplete data ('N/A' entries)\n" + "Not continuing with post-processing");
						htFileReader.Close();
						ltFileReader.Close();
						return;
					}
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

			for (int i = 0; i < configFile.diffusionCount; i++)
			{
				ltFileReader.ReadLine();
				htFileReader.ReadLine();
			}

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
				if ((configFile.postProcessingFiles == 1) || (configFile.postProcessingFiles == 2))
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
					afterAverage[i] /= (configFile.sampleAverageCount * 2);
				else
					afterAverage[i] /= configFile.sampleAverageCount;
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
			if (this.running)
			{
				MessageBox.Show("Please stop before opening files");
				return;
			}
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
					string ltFileName = logFileName.Substring(0, startOfFileName + 1) + f1Mf2AFileName;
					string ctFileName = logFileName.Substring(0, startOfFileName + 1) + f1AFileName;
					string htFileName = logFileName.Substring(0, startOfFileName+1) + f1Pf2AFileName;
					StreamReader logFileReader = new StreamReader(logFileName);
					StreamReader htFileReader = new StreamReader(htFileName);
					StreamReader ltFileReader = new StreamReader(ltFileName);
					StreamReader ctFileReader = new StreamReader(ctFileName);
					this.globalCycle = 0;
					this.mostRecentAddMpsCycle = 0;
					this.tareIndex = 0;
					this.tareIndexTextbox.Text = "0";
					string line;
					while ((line = logFileReader.ReadLine()) != null)
					{
						if (line.Contains("User Name:"))
						{
							line = line.Replace("\t", "");
							int first = line.IndexOf(":");
							string me = line.Substring(first + 1, line.Length - first - 1);
							this.userNameTextBox.Text = line.Substring(first + 1, line.Length - first - 1);
						}
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
					}
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
					htFileReader.ReadLine();
					ctFileReader.ReadLine();
					ltFileReader.ReadLine();
					this.globalCycle++;
					for (int i = 0; ; i++)
					{
						int nullCount = 0;
						if ((line = ltFileReader.ReadLine()) != null)
						{
							for (int j = 0; ; j++)
							{
								if (line == "")
									break;
								if (j == 16)
									continue;

								double dataPoint;
								try {
									dataPoint = double.Parse(line.Substring(0, line.IndexOf("\t")));
								} catch (FormatException) {
									line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
									continue;
								}
								this.rawChart1.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
								this.adjustedChart1.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
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

								double dataPoint;
								try {
									dataPoint = double.Parse(line.Substring(0, line.IndexOf("\t")));
								} catch (FormatException) {
									line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
									continue;
								}
								this.rawChart2.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
								this.adjustedChart2.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
								line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
							}
						}
						else
							nullCount++;
						if ((line = htFileReader.ReadLine()) != null)
						{
							for (int j = 0; ; j++)
							{
								if (line == "")
									break;
								if (j == 16)
									continue;

								double dataPoint;
								try {
									dataPoint = double.Parse(line.Substring(0, line.IndexOf("\t")));
								} catch (FormatException) {
									line = line.Substring(line.IndexOf("\t") + 1, line.Length - line.IndexOf("\t") - 1);
									continue;
								}
								this.rawChart3.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
								this.adjustedChart3.Series[j].Points.AddXY(getAddTime(j + 1) + this.globalCycle, dataPoint);
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

					recalculateData();	// a cycle ahead so we can view all of our available data (note: we'll see more data points than we did when we saved the files)
					
					validatePostProcessing();
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

		/*
		 * This will attempt to reconnect to the microcontroller
		 */
		private bool reconnectToDevice()
		{
			try {
				serialPort.Close();										// Try to close the COM ports in case we've already opened them
			#if _DEBUG
				debugSerial.Close();
			#endif
			} catch (Exception) {
			}

			try {
				serialPort.Dispose();										// Dispose of the previous SerialPort objects (might as well clean up a little bit)
			#if _DEBUG
				debugSerial.Dispose();
			#endif
			} catch (Exception) {
			}

		#if _DEBUG														// If we're debugging (and thus don't have an actual microcontroller),
			serialPort = new SerialPort("COM5", 115200);							// manually set the COM ports
			debugSerial = new SerialPort("COM6", 115200);
			debugSerial.ReadTimeout = 200;
		#else															// Otherwise, start with the COM port name passed in (see Program.cs)
			serialPort = new SerialPort(this.comPortName, 10000);//115200);
		#endif
			serialPort.ReadTimeout = 800;										// Always set the main COM port ReadTimeout property to 800 milliseconds
			serialPort.WriteTimeout = 800;									// Always set the main COM port WriteTimeout property to 800 milliseconds
			// Open COM ports
			try {
				serialPort.Open();										// Open the main COM port
			#if _DEBUG													// If we're debugging,
				debugSerial.Open();										// open the debug COM port,
				microcontroller = new Microcontroller(debugSerial, speed: 200, count: 30);	// and start the microcontroller emulator (for behavior, see Microcontroller.cs)
			#endif
				serialPort.DataReceived += new SerialDataReceivedEventHandler(readPacket);	// Add the handler for COM port reading (automatically called when something is written to main COM port)
				protocolHandler = new ProtocolHandler(serialPort);					// Initialize the protocol handler
				this.startRunButton.Enabled = true;
			} catch (Exception) {
				MessageBox.Show("Failed to connect to device");
				return false;
			}

			return true;
		}

		/*
		 * This is for using Demo Mode (only up to two reference and two normal sensors used)
		 */
		private byte[] demoModeStart()
		{
			int[] selectedNormalSensors = new int[2];
			int selectedNormalSensorsCount = 0;
			int[] selectedReferenceSensors = new int[2];
			int selectedReferenceSensorsCount = 0;
			foreach (CheckBox c in this.groupBox1.Controls.OfType<CheckBox>())
			{
				if (c.Checked)
				{
					bool used = false;
					int number = getSensorNumber(c.Name);
					for (int i = 0; i < 5; i++)
					{
						if (number == referenceSensors[i])
						{
							if (selectedReferenceSensorsCount >= 2)
							{
								MessageBox.Show("Please select up to two reference and\n" +
											"two normal sensors to acquire data for");
								return null;
							}
							if (number > 16)
								selectedReferenceSensors[selectedReferenceSensorsCount] = this.configFile.sensorMultiplexerValues[number - 1];
							else
								selectedReferenceSensors[selectedReferenceSensorsCount] = this.configFile.sensorMultiplexerValues[number - 2];
							selectedReferenceSensorsCount++;
							used = true;
							break;
						}
					}
					if (!used && selectedNormalSensorsCount >= 2)
					{
						MessageBox.Show("Please select up to two reference and\n" +
									"two normal sensors to acquire data for");
						return null;
					}
					else if (!used)
					{
						if (number > 16)
							selectedNormalSensors[selectedNormalSensorsCount] = this.configFile.sensorMultiplexerValues[number - 1];
						else
							selectedNormalSensors[selectedNormalSensorsCount] = this.configFile.sensorMultiplexerValues[number - 1];
						selectedNormalSensorsCount++;
					}
				}
			}

			if (selectedReferenceSensorsCount == 0 || selectedNormalSensorsCount == 0)
			{
				MessageBox.Show("Please select up to two reference and\n" +
							"two normal sensors to acquire data for");
				return null;
			}

			/* Create a config packet for the microcontroller */
			byte[] payload = new byte[selectedReferenceSensorsCount + selectedNormalSensorsCount];
			payload[0] = (byte)selectedReferenceSensors[0];
			if (selectedReferenceSensorsCount > 1)
				payload[1] = (byte)selectedReferenceSensors[1];
			payload[selectedReferenceSensorsCount] = (byte)selectedNormalSensors[0];
			if (selectedNormalSensorsCount > 1)
				payload[selectedReferenceSensorsCount + 1] = (byte)selectedNormalSensors[1];

			this.demoModeSensorsCount = selectedNormalSensorsCount + selectedReferenceSensorsCount;

			return payload;
		}

		/* This is a little Easter Egg I decided to throw in to commemorate our team */
		int helpToolStripMenuItemClickCount = 0;
		private void helpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.helpToolStripMenuItemClickCount++;								// Increment the counter (which is set to 0 when the About item is clicked)
			if (this.helpToolStripMenuItemClickCount > 9)							// If the user has clicked ten times,
			{
				MessageBox.Show("U of M Spring 2013 Senior Design Group:\n" +			// show the box,
							"   Erik Johnson\n" +
							"   Michael Sandstedt\n" +
							"   Jonathon Pechuman\n" +
							"   Samiha Sultana\n" +
							"   Hajime Makino\n" +
							"'It's what we do'\n\t-Team");
				this.helpToolStripMenuItemClickCount = 0;							// and set the count back to 0
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{															// Reset the counter from above
			this.helpToolStripMenuItemClickCount = 0;								// This is something you need to do, Todd. I'd maybe suggest creating a new Form so you don't have to put a whole bunch of
			MessageBox.Show("Add some stuff here, Todd!");							// text in a message box (plus it'd be a nice way to start getting familiar with C# form programming)
		}
	}
}
