using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace SpintronicsGUI
{
	public partial class PostProcessingResults : Form
	{
		enum SensorAssignment
		{
			A,
			B
		}

		enum AxisAssignment
		{
			SensorNumber,
			SensorName
		}

		double[] preResults;
		double[] postResults;
		string[] sensorProperties = { "0d_0a", "0c_0b", "6d_1a", "6c_1b", "5d_2a", "5c_2b", "0e_0e", "4d_3a", "4c_3b", "3d_4a",
							"3c_4b", "2d_5a", "2c_5b", "1d_6a", "1c_6b", "1b_6c", "1a_6d", "2b_5c", "2a_5d", "3b_4c",
							"3a_4d", "4b_3c", "4a_3d", "5b_2c", "5a_2d", "6b_1c", "6a_1d", "0b_0c", "0a_0d"};
		string saveFileDirectory;
		string reactionWell;
		string sample;
		bool hasSaved = false;
		SensorAssignment sensorAssignment = SensorAssignment.A;
		AxisAssignment axisAssignment = AxisAssignment.SensorNumber;

		public PostProcessingResults(double[] pre, double[] post, string defaultSaveFileDirectory, string reaWell, string sam)
		{
			InitializeComponent();

			this.preResults = pre;
			this.postResults = post;
			this.saveFileDirectory = defaultSaveFileDirectory;
			this.reactionWell = reaWell;
			this.sample = sam;

			this.sensorAssignmentARadioButton.Checked = true;
			this.sensorNumberRadioButton.Checked = true;

			addDataToGraph();
		}

		public void addDataToGraph()
		{
			foreach (Series s in this.chart1.Series)
			{
				s.Points.Clear();
			}

			for(int i = 0; i < preResults.Length; i++)
			{
				if (this.axisAssignment == AxisAssignment.SensorNumber)
				{
					this.chart1.Series[i].Points.AddXY((i + 1), 0);
					this.chart1.Series[i].Points.AddXY((i + 1), (postResults[i] - preResults[i]));
					this.chart1.Series[i].Points.Last().MarkerStyle = MarkerStyle.Circle;
					this.chart1.Series[i].Points.Last().ToolTip = "" + this.chart1.Series[i].Points.Last().YValues[0];
				}
				else
				{
					if (getSensorRow(i + 1) > 0)
					{
						this.chart1.Series[i].Points.AddXY(((getSensorColumn(i + 1) - 1) * 6) + getSensorRow(i + 1), 0);
						this.chart1.Series[i].Points.AddXY(((getSensorColumn(i + 1) - 1) * 6) + getSensorRow(i + 1), postResults[i] - preResults[i]);
					}
					else
					{
						this.chart1.Series[i].Points.AddXY(getSensorColumn(i + 1) + 24, 0);
						this.chart1.Series[i].Points.AddXY(getSensorColumn(i + 1) + 24, postResults[i] - preResults[i]);
					}
					this.chart1.Series[i].Points.Last().MarkerStyle = MarkerStyle.Circle;
					this.chart1.Series[i].Points.First().AxisLabel = "" + getSensorRow(i + 1) + getSensorColumnLetter(i + 1);
					this.chart1.Series[i].Points.Last().AxisLabel = "" + getSensorRow(i + 1) + getSensorColumnLetter(i + 1);
					this.chart1.Series[i].Points.Last().ToolTip = "" + this.chart1.Series[i].Points.Last().YValues[0];
				}
			}
		}

		private void pinAssignmentARadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.sensorAssignmentARadioButton.Checked)
				this.sensorAssignment = SensorAssignment.A;
			else
				this.sensorAssignment = SensorAssignment.B;

			addDataToGraph();
		}

		private void sensorOnXAxisRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.sensorNumberRadioButton.Checked)
			{
				this.axisAssignment = AxisAssignment.SensorNumber;
				this.chart1.ChartAreas[0].AxisX.Title = "Sensor Number";
			}
			else
			{
				this.axisAssignment = AxisAssignment.SensorName;
				this.chart1.ChartAreas[0].AxisX.Title = "Sensor Name";
			}

			addDataToGraph();
		}

		private int getSensorColumn(int sensor)
		{
			string column;
			if (this.sensorAssignment == SensorAssignment.A)
				column = sensorProperties[sensor - 1].Substring(1, 1);
			else
				column = sensorProperties[sensor - 1].Substring(4, 1);

			if (column == "a")
				return 1;
			else if (column == "b")
				return 2;
			else if (column == "c")
				return 3;
			else if (column == "d")
				return 4;
			else
				return 5;
		}

		private string getSensorColumnLetter(int sensor)
		{
			if (this.sensorAssignment == SensorAssignment.A)
				return sensorProperties[sensor - 1].Substring(1, 1);
			else
				return sensorProperties[sensor - 1].Substring(4, 1);
		}

		private int getSensorRow(int sensor)
		{
			if (this.sensorAssignment == SensorAssignment.A)
				return System.Convert.ToInt32(sensorProperties[sensor - 1].Substring(0, 1));
			else
				return System.Convert.ToInt32(sensorProperties[sensor - 1].Substring(3, 1));
		}

		private void appendAndSaveButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.OverwritePrompt = false;
			saveFile.InitialDirectory = this.saveFileDirectory;
			saveFile.DefaultExt = ".txt";
			saveFile.FileName = "post-processing";
			if (saveFile.ShowDialog() == DialogResult.OK)
			{
				try {
					StreamWriter postProcessingFile = new StreamWriter(File.Open(saveFile.FileName, FileMode.Append));
					postProcessingFile.WriteLine("\nDate:\t\t" + DateTime.Now);
					postProcessingFile.WriteLine("Reaction Well:\t" + this.reactionWell);
					postProcessingFile.WriteLine("Sample:\t\t" + this.sample);
					for (int i = 0; i < 30; i++)
					{
						if (i == 15)
							continue;
						if(i < 9)
							postProcessingFile.Write("Sensor   " + (i + 1) + "\t");
						else
							postProcessingFile.Write("Sensor  " + (i + 1) + "\t");
					}
					postProcessingFile.Write("\n");
					for (int i = 0; i < 29; i++)
					{
						string dataString = System.Convert.ToString(postResults[i] - preResults[i]);
						try {
							dataString = dataString.Substring(0, 10);
						} catch (ArgumentOutOfRangeException) {
							dataString = dataString.PadRight(10, '0');
						}
						postProcessingFile.Write(dataString + "\t");
					}
					postProcessingFile.Write("\n");
					postProcessingFile.Flush();
					postProcessingFile.Close();
					this.hasSaved = true;
				} catch (ArgumentException) {
					MessageBox.Show("Error while saving files: One or more files could not be found");
				} catch (UnauthorizedAccessException) {
					MessageBox.Show("Error while saving files: One or more files could not be accessed due to lack of authorization");
				} catch (Exception ex) {
					MessageBox.Show("Error while saving files:" + ex.Message);
				}
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void PostProcessingResults_FormClosed(object sender, EventArgs e)
		{
			if (!hasSaved)
			{
				if (MessageBox.Show("Are you sure you want to close without saving?", "", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
					this.Close();
			}
			else
			{
				this.Close();
			}
		}
	}
}
