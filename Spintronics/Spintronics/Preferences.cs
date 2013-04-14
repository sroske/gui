using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SpintronicsGUI
{
	public partial class Preferences : Form
	{
		int tempFoldersToKeep;
		int[] sensorMultiplexerValues;
		string reactionWell;
		string sample;

		public Preferences(int initialFoldersToKeep, int[] initialAssignments, string initialReactionWell, string initialSample)
		{
			InitializeComponent();
			this.tempFoldersToKeep = initialFoldersToKeep;
			this.sensorMultiplexerValues = initialAssignments;
			this.reactionWell = initialReactionWell;
			this.sample = initialSample;
			populateFields();
		}

		public Preferences(Configuration config)
		{
			InitializeComponent();
			this.tempFoldersToKeep = config.getTempFoldersToKeep();
			this.sensorMultiplexerValues = config.getSensorMultiplexerValues();
			this.reactionWell = config.getReactionWell();
			this.sample = config.getSample();
			populateFields();
		}

		private void populateFields()
		{
			this.tempFoldersToKeepTextBox.Text = System.Convert.ToString(this.tempFoldersToKeep);

			foreach (TextBox t in this.pinMultiplexerValuesTabPage.Controls.OfType<TextBox>())
			{
				try {
					int element = System.Convert.ToInt32(t.Name.Substring(t.Name.Length - 2, 2));
					t.Text = System.Convert.ToString(sensorMultiplexerValues[element - 1]);
				} catch (FormatException) {
					t.Text = "0";
				} catch (OverflowException) {
					t.Text = "0";
				} catch (IndexOutOfRangeException) {
					t.Text = "0";
				}
			}
			this.reactionWellTextBox.Text = this.reactionWell;
			this.sampleTextBox.Text = this.sample;
		}

		public int getTempFoldersToKeep()
		{
			return this.tempFoldersToKeep;
		}
		
		public int[] getSensorMultiplexerValues()
		{
			return this.sensorMultiplexerValues;
		}

		public string getReactionWell()
		{
			return this.reactionWell;
		}

		public string getSample()
		{
			return this.sample;
		}

		private bool saveGeneral()
		{
			try {
				this.tempFoldersToKeep = System.Convert.ToInt32(this.tempFoldersToKeepTextBox.Text);
				return true;
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a value for the number of temporary run folders to keep");
				return false;
			} catch (FormatException) {
				MessageBox.Show("Please enter a valid value for the number of temporary run folders to keep");
				return false;
			} catch (OverflowException) {
				MessageBox.Show("Please enter a valid value for the number of temporary run folders to keep");
				return false;
			}
		}

		private void revertGeneralButton_Click(object sender, EventArgs e)
		{
			this.tempFoldersToKeepTextBox.Text = System.Convert.ToString(this.tempFoldersToKeep);
		}

		private bool savePinAssignments()
		{
			foreach (TextBox t in this.pinMultiplexerValuesTabPage.Controls.OfType<TextBox>())
			{
				try {
					int value = System.Convert.ToInt32(t.Text);
					this.sensorMultiplexerValues[System.Convert.ToUInt32(t.Name.Substring(t.Name.Length - 2, 2)) - 1] = value;
				} catch (ArgumentNullException) {
					MessageBox.Show("Please enter a value for sensor " + t.Name);
					return false;
				} catch (FormatException) {
					MessageBox.Show("Please enter a valid number for sensor " + t.Name);
					return false;
				} catch (OverflowException) {
					MessageBox.Show("The value for sensor " + t.Name + " is too large.");
					return false;
				}
			}
			return true;
		}

		private void revertPinAssignmentsButton_Click(object sender, EventArgs e)
		{
			foreach (TextBox t in this.pinMultiplexerValuesTabPage.Controls.OfType<TextBox>())
			{
				int value = sensorMultiplexerValues[System.Convert.ToUInt32(t.Name.Substring(t.Name.Length - 2, 2)) - 1];
				t.Text = System.Convert.ToString(value);
			}
		}

		private bool saveInitFileValues()
		{
			try {
				this.reactionWell = this.reactionWellTextBox.Text;
				this.sample = this.sampleTextBox.Text;
				return true;
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
				return false;
			} catch (FormatException) {
				MessageBox.Show("Please enter valid strings for all fields");
				return false;
			}
		}

		private void revertInitFileValuesButton_Click(object sender, EventArgs e)
		{
			this.reactionWellTextBox.Text = this.reactionWell;
			this.sampleTextBox.Text = this.sample;
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			if (!saveGeneral())
				return;
			if (!savePinAssignments())
				return;
			if (!saveInitFileValues())
				return;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
