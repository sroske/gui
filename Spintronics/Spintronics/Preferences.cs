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
		public int tempFoldersToKeep;
		public int[] sensorMultiplexerValues;
		public string bufferName;
		public string mnpsName;
		public int preloadBufferVolume;
		public int defaultAddBufferVolume;
		public int defaultAddMnpsVolume;
		public string defaultVolumeUnit;
		public float wheatstoneAmplitude;
		public string wheatstoneAmplitudeUnit;
		public float wheatstoneFrequency;
		public float coilAmplitude;
		public string coilAmplitudeUnit;
		public float coilFrequency;
		public float coilDcOffset;
		public string coilDcOffsetUnit;
		public float measurementPeriod;
		public int postProcessingCount;

		public Preferences(Configuration config)
		{
			InitializeComponent();
			this.tempFoldersToKeep = config.tempFoldersToKeep;
			this.sensorMultiplexerValues = config.sensorMultiplexerValues;
			this.bufferName = config.bufferName;
			this.mnpsName = config.mnpsName;
			this.preloadBufferVolume = config.preloadBufferVolume;
			this.defaultAddBufferVolume = config.defaultAddBufferVolume;
			this.defaultAddMnpsVolume = config.defaultAddMnpsVolume;
			this.defaultVolumeUnit = config.defaultVolumeUnit;
			this.wheatstoneAmplitude = config.wheatstoneAmplitude;
			this.wheatstoneAmplitudeUnit = config.wheatstoneAmplitudeUnit;
			this.wheatstoneFrequency = config.wheatstoneFrequency;
			this.coilAmplitude = config.coilAmplitude;
			this.coilAmplitudeUnit = config.coilAmplitudeUnit;
			this.coilFrequency = config.coilFrequency;
			this.coilDcOffset = config.coilDcOffset;
			this.coilDcOffsetUnit = config.coilDcOffsetUnit;
			this.measurementPeriod = config.measurementPeriod;
			this.postProcessingCount = config.postProcessingCount;
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
			this.defaultAddBufferVolumeTextBox.Text = System.Convert.ToString(this.defaultAddBufferVolume);
			this.defaultAddMnpsVolumeTextBox.Text = System.Convert.ToString(this.defaultAddMnpsVolume);
			this.defaultVolumeUnitComboBox.SelectedItem = this.defaultVolumeUnit;
			this.wheatstoneAmplitudeTextBox.Text = System.Convert.ToString(this.wheatstoneAmplitude);
			this.wheatstoneAmplitudeUnitTextBox.Text = this.wheatstoneAmplitudeUnit;
			this.wheatstoneFrequencyTextBox.Text = System.Convert.ToString(this.wheatstoneFrequency);
			this.coilAmplitudeTextBox.Text = System.Convert.ToString(this.coilAmplitude);
			this.coilAmplitudeUnitTextBox.Text = this.coilAmplitudeUnit;
			this.coilFrequencyTextBox.Text = System.Convert.ToString(this.coilFrequency);
			this.coilDcOffsetTextBox.Text = System.Convert.ToString(this.coilDcOffset);
			this.coilDcOffsetUnitTextBox.Text = this.coilDcOffsetUnit;
			this.measurementPeriodTextBox.Text = System.Convert.ToString(this.measurementPeriod);
			this.postProcessingCountTextBox.Text = System.Convert.ToString(this.postProcessingCount);
		}

		private bool saveGeneralTabPreferences()
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

		private bool saveMeasurementParametersTabPreferences()
		{
			try {
				this.wheatstoneAmplitude = float.Parse(this.wheatstoneAmplitudeTextBox.Text);
				this.wheatstoneAmplitudeUnit = this.wheatstoneAmplitudeUnitTextBox.Text;
				this.wheatstoneFrequency = float.Parse(this.wheatstoneFrequencyTextBox.Text);
				this.coilAmplitude = float.Parse(this.coilAmplitudeTextBox.Text);
				this.coilAmplitudeUnit = this.coilAmplitudeUnitTextBox.Text;
				this.coilFrequency = float.Parse(this.coilFrequencyTextBox.Text);
				this.coilDcOffset = float.Parse(this.coilDcOffsetTextBox.Text);
				this.coilDcOffsetUnit = this.coilDcOffsetUnitTextBox.Text;
				this.measurementPeriod = float.Parse(this.measurementPeriodTextBox.Text);
				return true;
			} catch (ArgumentNullException) {
				MessageBox.Show("Please enter a for all fields");
				return false;
			} catch (FormatException) {
				MessageBox.Show("Please enter a valid value for all fields");
				return false;
			} catch (OverflowException) {
				MessageBox.Show("Please enter a valid value for all fields");
				return false;
			}
		}

		private void revertMeasurementParametersButton_Click(object sender, EventArgs e)
		{
			this.wheatstoneAmplitudeTextBox.Text = System.Convert.ToString(this.wheatstoneAmplitude);
			this.wheatstoneAmplitudeUnitTextBox.Text = this.wheatstoneAmplitudeUnit;
			this.wheatstoneFrequencyTextBox.Text = System.Convert.ToString(this.wheatstoneFrequency);
			this.coilAmplitudeTextBox.Text = System.Convert.ToString(this.coilAmplitude);
			this.coilAmplitudeUnitTextBox.Text = this.coilAmplitudeUnit;
			this.coilFrequencyTextBox.Text = System.Convert.ToString(this.coilFrequency);
			this.coilDcOffsetTextBox.Text = System.Convert.ToString(this.coilDcOffset);
			this.coilDcOffsetUnitTextBox.Text = this.coilDcOffsetUnit;
			this.measurementPeriodTextBox.Text = System.Convert.ToString(this.measurementPeriod);
		}

		private bool savePinAssignmentsTabPreferences()
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

		private bool saveLogInformationTabPreferences()
		{
			try {
				this.bufferName = this.bufferNameTextBox.Text;
				this.mnpsName = this.mnpsNameTextBox.Text;
				this.preloadBufferVolume = System.Convert.ToInt32(this.preloadBufferVolumeTextBox.Text);
				this.defaultAddBufferVolume = System.Convert.ToInt32(this.defaultAddBufferVolumeTextBox.Text);
				this.defaultAddMnpsVolume = System.Convert.ToInt32(this.defaultAddMnpsVolumeTextBox.Text);
				this.defaultVolumeUnit = (string)this.defaultVolumeUnitComboBox.SelectedItem;
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

		private void revertLogInformationButton_Click(object sender, EventArgs e)
		{
			this.bufferNameTextBox.Text = this.bufferName;
			this.mnpsNameTextBox.Text = this.mnpsName;
			this.preloadBufferVolumeTextBox.Text = System.Convert.ToString(this.preloadBufferVolume);
			this.defaultAddBufferVolumeTextBox.Text = System.Convert.ToString(this.defaultAddBufferVolume);
			this.defaultAddMnpsVolumeTextBox.Text = System.Convert.ToString(this.defaultAddMnpsVolume);
			this.defaultVolumeUnitComboBox.SelectedItem = this.defaultVolumeUnit;
		}

		private bool savePostProcessingTabPreferences()
		{
			try {
				this.postProcessingCount = System.Convert.ToInt32(this.postProcessingCountTextBox.Text);
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

		private void revertPostProcessingButton_Click(object sender, EventArgs e)
		{
			this.postProcessingCountTextBox.Text = System.Convert.ToString(this.postProcessingCount);
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			if (!saveGeneralTabPreferences())
				return;
			if (!saveMeasurementParametersTabPreferences())
				return;
			if (!savePinAssignmentsTabPreferences())
				return;
			if (!saveLogInformationTabPreferences())
				return;
			if (!savePostProcessingTabPreferences())
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
