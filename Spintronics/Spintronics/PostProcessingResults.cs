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
		SensorAssignment sensorAssignment;
		AxisAssignment axisAssignment = AxisAssignment.SensorNumber;

		public PostProcessingResults(double[] pre, double[] post, string defaultSaveFileDirectory, string reaWell, string sam, SensorAssignment assignment)
		{
			InitializeComponent();

			this.preResults = pre;
			this.postResults = post;
			this.saveFileDirectory = defaultSaveFileDirectory;
			this.reactionWell = reaWell;
			this.sample = sam;
			this.sensorAssignment = assignment;

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
			if (this.axisAssignment == AxisAssignment.SensorNumber)
			{
				for (int i = 15; i < preResults.Length; i++)
				{
					this.chart1.Series[i].Points.First().XValue++;
					this.chart1.Series[i].Points.Last().XValue++;
				}
			}
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
				StreamWriter postProcessingFile;
				if (!File.Exists(saveFile.FileName))
				{
					postProcessingFile = new StreamWriter(File.Open(saveFile.FileName, FileMode.Append));
					postProcessingFile.Write("Date\t");
					postProcessingFile.Write("Reaction Well\t");
					postProcessingFile.Write("Sample\t");
					string col = "a";
					int row = 1;
					for (int i = 0; i < 29; i++)
					{
						postProcessingFile.Write(row + col + "\t");

						if (i > 22)
						{
							row = 0;
							if (col == "a")
								col = "b";
							else if (col == "b")
								col = "c";
							else if (col == "c")
								col = "d";
							else if (i == 23)
								col = "a";
							else
								col = "e";
						}
						else if (++row > 6)
						{
							row = 1;
							if (col == "a")
								col = "b";
							else if (col == "b")
								col = "c";
							else if (col == "c")
								col = "d";
							else
								col = "a";
						}
					}
					postProcessingFile.Write("\n");
				}
				else
				{
					postProcessingFile = new StreamWriter(File.Open(saveFile.FileName, FileMode.Append));
				}
				try {
					postProcessingFile.Write(DateTime.Today.ToShortDateString() + "\t");
					postProcessingFile.Write(this.reactionWell + "\t");
					postProcessingFile.Write(this.sample + "\t");

					double[] data = new double[preResults.Length];

					for (int i = 0; i < data.Length; i++)
					{
						if (getSensorRow(i + 1) > 0)
						{
							data[((getSensorColumn(i + 1) - 1) * 6) + getSensorRow(i + 1) - 1] = postResults[i] - preResults[i];
						}
						else
						{
							data[getSensorColumn(i + 1) + 24 - 1] = postResults[i] - preResults[i];
						}
					}

					for (int i = 0; i < data.Length; i++)
					{
						string dataString = System.Convert.ToString(data[i]);
						try {
							dataString = dataString.Substring(0, 10);
						} catch (ArgumentOutOfRangeException) {
							dataString = dataString.PadRight(10, '0');
						}
						postProcessingFile.Write(dataString + "\t");
					}
					this.hasSaved = true;
				} catch (ArgumentException) {
					MessageBox.Show("Error while saving files: One or more files could not be found");
				} catch (UnauthorizedAccessException) {
					MessageBox.Show("Error while saving files: One or more files could not be accessed due to lack of authorization");
				} catch (Exception ex) {
					MessageBox.Show("Error while saving files: " + ex.Message);
				} finally {
					postProcessingFile.Write("\n");
					postProcessingFile.Flush();
					postProcessingFile.Close();
				}
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void PostProcessingResults_FormClosed(object sender, CancelEventArgs e)
		{
			if (!hasSaved)
			{
				if (MessageBox.Show("Are you sure you want to close without saving?", "", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
					e.Cancel = true;
			}
		}
	}
}
