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
		string bufferName;
		string mnpsName;
		int preloadBufferVolume;
		string preloadBufferVolumeUnit;
		int defaultAddBufferVolume;
		int defaultAddMnpsVolume;
		string defaultBufferMnpsVolumeUnit;

		public Preferences(int initialFoldersToKeep, int[] initialAssignments, string initialReactionWell, string initialSample)
		{
			InitializeComponent();
			this.tempFoldersToKeep = initialFoldersToKeep;
			this.sensorMultiplexerValues = initialAssignments;
			this.bufferName = initialReactionWell;
			this.mnpsName = initialSample;
			populateFields();
		}

		public Preferences(Configuration config)
		{
			InitializeComponent();
			this.tempFoldersToKeep = config.getTempFoldersToKeep();
			this.sensorMultiplexerValues = config.getSensorMultiplexerValues();
			this.bufferName = config.getBufferName();
			this.mnpsName = config.getMnpsName();
			this.preloadBufferVolume = config.getPreloadBufferVolume();
			this.preloadBufferVolumeUnit = config.getPreloadBufferVolumeUnit();
			this.defaultAddBufferVolume = config.getDefaultAddBufferVolume();
			this.defaultAddMnpsVolume = config.getDefaultAddMnpsVolume();
			this.defaultBufferMnpsVolumeUnit = config.getDefaultBufferMnpsVolumeUnit();
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
			this.bufferNameTextBox.Text = this.bufferName;
			this.mnpsNameTextBox.Text = this.mnpsName;
			this.preloadBufferVolumeTextBox.Text = System.Convert.ToString(this.preloadBufferVolume);
			this.preloadBufferVolumeUnitTextBox.Text = this.preloadBufferVolumeUnit;
			this.defaultAddBufferVolumeTextBox.Text = System.Convert.ToString(this.defaultAddBufferVolume);
			this.defaultAddMnpsVolumeTextBox.Text = System.Convert.ToString(this.defaultAddMnpsVolume);
			this.defaultBufferMnpVolumeUnitComboBox.SelectedItem = this.defaultBufferMnpsVolumeUnit;
		}

		public int getTempFoldersToKeep()
		{
			return this.tempFoldersToKeep;
		}
		
		public int[] getSensorMultiplexerValues()
		{
			return this.sensorMultiplexerValues;
		}

		public string getBufferName()
		{
			return this.bufferName;
		}

		public string getMnpsName()
		{
			return this.mnpsName;
		}

		public int getPreloadBufferVolume()
		{
			return this.preloadBufferVolume;
		}

		public string getPreloadBufferVolumeUnit()
		{
			return this.preloadBufferVolumeUnit;
		}

		public int getDefaultAddBufferVolume()
		{
			return this.defaultAddBufferVolume;
		}

		public int getDefaultAddMnpsVolume()
		{
			return this.defaultAddMnpsVolume;
		}

		public string getDefaultBufferMnpsVolumeUnit()
		{
			return this.defaultBufferMnpsVolumeUnit;
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

		private bool saveLogInformation()
		{
			try {
				this.bufferName = this.bufferNameTextBox.Text;
				this.mnpsName = this.mnpsNameTextBox.Text;
				this.preloadBufferVolume = System.Convert.ToInt32(this.preloadBufferVolumeTextBox.Text);
				this.preloadBufferVolumeUnit = this.preloadBufferVolumeUnitTextBox.Text;
				this.defaultAddBufferVolume = System.Convert.ToInt32(this.defaultAddBufferVolumeTextBox.Text);
				this.defaultAddMnpsVolume = System.Convert.ToInt32(this.defaultAddMnpsVolumeTextBox.Text);
				this.defaultBufferMnpsVolumeUnit = (string)this.defaultBufferMnpVolumeUnitComboBox.SelectedItem;
				return true;
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a value for all fields");
				return false;
			} catch (FormatException) {
				MessageBox.Show("Please enter valid value for all fields");
				return false;
			} catch (OverflowException) {
				MessageBox.Show("Please enter valid value for all fields");
				return false;
			}
		}

		private void revertLogInformation_Click(object sender, EventArgs e)
		{
			this.bufferNameTextBox.Text = this.bufferName;
			this.mnpsNameTextBox.Text = this.mnpsName;
			this.preloadBufferVolumeTextBox.Text = System.Convert.ToString(this.preloadBufferVolume);
			this.preloadBufferVolumeUnitTextBox.Text = this.preloadBufferVolumeUnit;
			this.defaultAddBufferVolumeTextBox.Text = System.Convert.ToString(this.defaultAddBufferVolume);
			this.defaultAddMnpsVolumeTextBox.Text = System.Convert.ToString(this.defaultAddMnpsVolume);
			this.defaultBufferMnpVolumeUnitComboBox.SelectedItem = this.defaultBufferMnpsVolumeUnit;
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			if (!saveGeneral())
				return;
			if (!savePinAssignments())
				return;
			if (!saveLogInformation())
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
