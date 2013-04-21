using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
		string[] sensorProperties = { "7d_1a", "7c_1b", "6d_2a", "6c_2b", "5d_3a", "5c_3b", "1e_1e", "4d_4a", "4c_4b",
							"3d_5a", "3c_5b", "2d_6a", "2c_6b", "1d_7a", "1c_7b", "1b_7c", "1a_7a", "2b_6c", "2a_6d", "3b_5c",
							"3a_5d", "4b_4c", "4a_4d", "5b_3c", "5a_3d", "6b_2c", "6a_2d", "7b_1c", "7a_1d"};
		SensorAssignment sensorAssignment = SensorAssignment.A;
		AxisAssignment axisAssignment = AxisAssignment.SensorNumber;

		public PostProcessingResults(double[] pre, double[] post)
		{
			InitializeComponent();

			preResults = pre;
			postResults = post;

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
					this.chart1.Series[i].Points.AddXY((i + 1), (postResults[i] - preResults[i]));
					this.chart1.Series[i].Points.Last().MarkerStyle = MarkerStyle.Circle;
				}
				else
				{
					this.chart1.Series[i].Points.AddXY(((getSensorColumn(i + 1) - 1) * 7) + getSensorRow(i + 1), (postResults[i] - preResults[i]));
					this.chart1.Series[i].Points.Last().MarkerStyle = MarkerStyle.Circle;
					this.chart1.Series[i].Points.Last().AxisLabel = "" + getSensorRow(i + 1) + getSensorColumnLetter(i + 1);
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
	}
}
