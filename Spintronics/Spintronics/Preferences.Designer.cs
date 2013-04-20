using System.Windows.Forms;
using System;
namespace SpintronicsGUI
{
	partial class Preferences
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.doneButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.generalTabPage = new System.Windows.Forms.TabPage();
			this.tempFoldersToKeepLabel = new System.Windows.Forms.Label();
			this.tempFoldersToKeepTextBox = new System.Windows.Forms.TextBox();
			this.revertGeneralButton = new System.Windows.Forms.Button();
			this.measurementParametersTabPage = new System.Windows.Forms.TabPage();
			this.coilDcOffsetUnitTextBox = new System.Windows.Forms.TextBox();
			this.coilAmplitudeUnitTextBox = new System.Windows.Forms.TextBox();
			this.wheatstoneAmplitudeUnitTextBox = new System.Windows.Forms.TextBox();
			this.measurementPeriodUnitLabel = new System.Windows.Forms.Label();
			this.coilFrequencyUnitLabel = new System.Windows.Forms.Label();
			this.wheatstoneFrequencyUnitLabel = new System.Windows.Forms.Label();
			this.coilDcOffsetTextBox = new System.Windows.Forms.TextBox();
			this.coilDcOffsetLabel = new System.Windows.Forms.Label();
			this.wheatstoneAmplitudeTextBox = new System.Windows.Forms.TextBox();
			this.coilAmplitudeTextBox = new System.Windows.Forms.TextBox();
			this.wheatstoneFrequencyTextBox = new System.Windows.Forms.TextBox();
			this.coilFrequencyTextBox = new System.Windows.Forms.TextBox();
			this.measurementPeriodTextBox = new System.Windows.Forms.TextBox();
			this.measurementPeriodLabel = new System.Windows.Forms.Label();
			this.coilFrequencyLabel = new System.Windows.Forms.Label();
			this.wheatstoneFrequencyLabel = new System.Windows.Forms.Label();
			this.coilAmplitudeLabel = new System.Windows.Forms.Label();
			this.wheatstoneAmplitudeLabel = new System.Windows.Forms.Label();
			this.revertMeasurementParametersButton = new System.Windows.Forms.Button();
			this.pinMultiplexerValuesTabPage = new System.Windows.Forms.TabPage();
			this.sensor30 = new System.Windows.Forms.Label();
			this.sensor29 = new System.Windows.Forms.Label();
			this.sensor28 = new System.Windows.Forms.Label();
			this.sensor27 = new System.Windows.Forms.Label();
			this.sensor26 = new System.Windows.Forms.Label();
			this.sensor25 = new System.Windows.Forms.Label();
			this.sensor24 = new System.Windows.Forms.Label();
			this.sensor23 = new System.Windows.Forms.Label();
			this.sensor22 = new System.Windows.Forms.Label();
			this.sensor21 = new System.Windows.Forms.Label();
			this.sensor20 = new System.Windows.Forms.Label();
			this.sensor19 = new System.Windows.Forms.Label();
			this.sensor18 = new System.Windows.Forms.Label();
			this.sensor17 = new System.Windows.Forms.Label();
			this.sensor16 = new System.Windows.Forms.Label();
			this.sensor15 = new System.Windows.Forms.Label();
			this.sensor14 = new System.Windows.Forms.Label();
			this.sensor13 = new System.Windows.Forms.Label();
			this.sensor12 = new System.Windows.Forms.Label();
			this.sensor11 = new System.Windows.Forms.Label();
			this.sensor10 = new System.Windows.Forms.Label();
			this.sensor9 = new System.Windows.Forms.Label();
			this.sensor8 = new System.Windows.Forms.Label();
			this.sensor7 = new System.Windows.Forms.Label();
			this.sensor6 = new System.Windows.Forms.Label();
			this.sensor5 = new System.Windows.Forms.Label();
			this.sensor4 = new System.Windows.Forms.Label();
			this.sensor3 = new System.Windows.Forms.Label();
			this.sensor2 = new System.Windows.Forms.Label();
			this.sensor1 = new System.Windows.Forms.Label();
			this.revertPinAssignmentsButton = new System.Windows.Forms.Button();
			this.textBox30 = new System.Windows.Forms.TextBox();
			this.textBox29 = new System.Windows.Forms.TextBox();
			this.textBox28 = new System.Windows.Forms.TextBox();
			this.textBox27 = new System.Windows.Forms.TextBox();
			this.textBox26 = new System.Windows.Forms.TextBox();
			this.textBox25 = new System.Windows.Forms.TextBox();
			this.textBox24 = new System.Windows.Forms.TextBox();
			this.textBox23 = new System.Windows.Forms.TextBox();
			this.textBox12 = new System.Windows.Forms.TextBox();
			this.textBox13 = new System.Windows.Forms.TextBox();
			this.textBox14 = new System.Windows.Forms.TextBox();
			this.textBox15 = new System.Windows.Forms.TextBox();
			this.textBox16 = new System.Windows.Forms.TextBox();
			this.textBox17 = new System.Windows.Forms.TextBox();
			this.textBox18 = new System.Windows.Forms.TextBox();
			this.textBox19 = new System.Windows.Forms.TextBox();
			this.textBox20 = new System.Windows.Forms.TextBox();
			this.textBox21 = new System.Windows.Forms.TextBox();
			this.textBox22 = new System.Windows.Forms.TextBox();
			this.textBox11 = new System.Windows.Forms.TextBox();
			this.textBox10 = new System.Windows.Forms.TextBox();
			this.textBox09 = new System.Windows.Forms.TextBox();
			this.textBox08 = new System.Windows.Forms.TextBox();
			this.textBox07 = new System.Windows.Forms.TextBox();
			this.textBox06 = new System.Windows.Forms.TextBox();
			this.textBox05 = new System.Windows.Forms.TextBox();
			this.textBox04 = new System.Windows.Forms.TextBox();
			this.textBox03 = new System.Windows.Forms.TextBox();
			this.textBox02 = new System.Windows.Forms.TextBox();
			this.textBox01 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.logInformationTabPage = new System.Windows.Forms.TabPage();
			this.defaultVolumeUnitLabel = new System.Windows.Forms.Label();
			this.defaultVolumeUnitComboBox = new System.Windows.Forms.ComboBox();
			this.defaultAddMnpsVolumeTextBox = new System.Windows.Forms.TextBox();
			this.defaultAddMnpsVolumeLabel = new System.Windows.Forms.Label();
			this.defaultAddBufferVolumeTextBox = new System.Windows.Forms.TextBox();
			this.defaultAddBufferVolumeLabel = new System.Windows.Forms.Label();
			this.preloadBufferVolumeTextBox = new System.Windows.Forms.TextBox();
			this.preloadBufferVolumeLabel = new System.Windows.Forms.Label();
			this.mnpsNameTextBox = new System.Windows.Forms.TextBox();
			this.mnpsNameLabel = new System.Windows.Forms.Label();
			this.bufferNameLabel = new System.Windows.Forms.Label();
			this.bufferNameTextBox = new System.Windows.Forms.TextBox();
			this.revertLogInformationButton = new System.Windows.Forms.Button();
			this.postProcessingTabPage = new System.Windows.Forms.TabPage();
			this.diffusionCountTextBox = new System.Windows.Forms.TextBox();
			this.diffusionCountLabel = new System.Windows.Forms.Label();
			this.revertPostProcessingButton = new System.Windows.Forms.Button();
			this.sampleAverageCountTextBox = new System.Windows.Forms.TextBox();
			this.sampleAverageCountLabel = new System.Windows.Forms.Label();
			this.postProcessingFilesLabel = new System.Windows.Forms.Label();
			this.ltPostProcessingFileCheckBox = new System.Windows.Forms.CheckBox();
			this.htPostProcessingFileCheckBox = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.measurementParametersTabPage.SuspendLayout();
			this.pinMultiplexerValuesTabPage.SuspendLayout();
			this.logInformationTabPage.SuspendLayout();
			this.postProcessingTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// doneButton
			// 
			this.doneButton.Location = new System.Drawing.Point(149, 271);
			this.doneButton.Name = "doneButton";
			this.doneButton.Size = new System.Drawing.Size(75, 23);
			this.doneButton.TabIndex = 0;
			this.doneButton.Text = "Done";
			this.doneButton.UseVisualStyleBackColor = true;
			this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(230, 271);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.generalTabPage);
			this.tabControl1.Controls.Add(this.measurementParametersTabPage);
			this.tabControl1.Controls.Add(this.pinMultiplexerValuesTabPage);
			this.tabControl1.Controls.Add(this.logInformationTabPage);
			this.tabControl1.Controls.Add(this.postProcessingTabPage);
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(532, 253);
			this.tabControl1.TabIndex = 0;
			// 
			// generalTabPage
			// 
			this.generalTabPage.Controls.Add(this.tempFoldersToKeepLabel);
			this.generalTabPage.Controls.Add(this.tempFoldersToKeepTextBox);
			this.generalTabPage.Controls.Add(this.revertGeneralButton);
			this.generalTabPage.Location = new System.Drawing.Point(4, 22);
			this.generalTabPage.Name = "generalTabPage";
			this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.generalTabPage.Size = new System.Drawing.Size(524, 227);
			this.generalTabPage.TabIndex = 2;
			this.generalTabPage.Text = "General";
			this.generalTabPage.UseVisualStyleBackColor = true;
			// 
			// tempFoldersToKeepLabel
			// 
			this.tempFoldersToKeepLabel.AutoSize = true;
			this.tempFoldersToKeepLabel.Location = new System.Drawing.Point(22, 21);
			this.tempFoldersToKeepLabel.Name = "tempFoldersToKeepLabel";
			this.tempFoldersToKeepLabel.Size = new System.Drawing.Size(161, 26);
			this.tempFoldersToKeepLabel.TabIndex = 3;
			this.tempFoldersToKeepLabel.Text = "Temporary Run Folders To Keep\r\n(-1 to keep all)";
			// 
			// tempFoldersToKeepTextBox
			// 
			this.tempFoldersToKeepTextBox.Location = new System.Drawing.Point(25, 50);
			this.tempFoldersToKeepTextBox.Name = "tempFoldersToKeepTextBox";
			this.tempFoldersToKeepTextBox.Size = new System.Drawing.Size(30, 20);
			this.tempFoldersToKeepTextBox.TabIndex = 0;
			// 
			// revertGeneralButton
			// 
			this.revertGeneralButton.Location = new System.Drawing.Point(6, 198);
			this.revertGeneralButton.Name = "revertGeneralButton";
			this.revertGeneralButton.Size = new System.Drawing.Size(75, 23);
			this.revertGeneralButton.TabIndex = 1;
			this.revertGeneralButton.Text = "Revert";
			this.revertGeneralButton.UseVisualStyleBackColor = true;
			this.revertGeneralButton.Click += new System.EventHandler(this.revertGeneralButton_Click);
			// 
			// measurementParametersTabPage
			// 
			this.measurementParametersTabPage.Controls.Add(this.coilDcOffsetUnitTextBox);
			this.measurementParametersTabPage.Controls.Add(this.coilAmplitudeUnitTextBox);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneAmplitudeUnitTextBox);
			this.measurementParametersTabPage.Controls.Add(this.measurementPeriodUnitLabel);
			this.measurementParametersTabPage.Controls.Add(this.coilFrequencyUnitLabel);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneFrequencyUnitLabel);
			this.measurementParametersTabPage.Controls.Add(this.coilDcOffsetTextBox);
			this.measurementParametersTabPage.Controls.Add(this.coilDcOffsetLabel);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneAmplitudeTextBox);
			this.measurementParametersTabPage.Controls.Add(this.coilAmplitudeTextBox);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneFrequencyTextBox);
			this.measurementParametersTabPage.Controls.Add(this.coilFrequencyTextBox);
			this.measurementParametersTabPage.Controls.Add(this.measurementPeriodTextBox);
			this.measurementParametersTabPage.Controls.Add(this.measurementPeriodLabel);
			this.measurementParametersTabPage.Controls.Add(this.coilFrequencyLabel);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneFrequencyLabel);
			this.measurementParametersTabPage.Controls.Add(this.coilAmplitudeLabel);
			this.measurementParametersTabPage.Controls.Add(this.wheatstoneAmplitudeLabel);
			this.measurementParametersTabPage.Controls.Add(this.revertMeasurementParametersButton);
			this.measurementParametersTabPage.Location = new System.Drawing.Point(4, 22);
			this.measurementParametersTabPage.Name = "measurementParametersTabPage";
			this.measurementParametersTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.measurementParametersTabPage.Size = new System.Drawing.Size(524, 227);
			this.measurementParametersTabPage.TabIndex = 3;
			this.measurementParametersTabPage.Text = "Measurement Parameters";
			this.measurementParametersTabPage.UseVisualStyleBackColor = true;
			// 
			// coilDcOffsetUnitTextBox
			// 
			this.coilDcOffsetUnitTextBox.Location = new System.Drawing.Point(424, 95);
			this.coilDcOffsetUnitTextBox.Name = "coilDcOffsetUnitTextBox";
			this.coilDcOffsetUnitTextBox.Size = new System.Drawing.Size(26, 20);
			this.coilDcOffsetUnitTextBox.TabIndex = 8;
			// 
			// coilAmplitudeUnitTextBox
			// 
			this.coilAmplitudeUnitTextBox.Location = new System.Drawing.Point(92, 95);
			this.coilAmplitudeUnitTextBox.Name = "coilAmplitudeUnitTextBox";
			this.coilAmplitudeUnitTextBox.Size = new System.Drawing.Size(26, 20);
			this.coilAmplitudeUnitTextBox.TabIndex = 3;
			// 
			// wheatstoneAmplitudeUnitTextBox
			// 
			this.wheatstoneAmplitudeUnitTextBox.Location = new System.Drawing.Point(94, 36);
			this.wheatstoneAmplitudeUnitTextBox.Name = "wheatstoneAmplitudeUnitTextBox";
			this.wheatstoneAmplitudeUnitTextBox.Size = new System.Drawing.Size(26, 20);
			this.wheatstoneAmplitudeUnitTextBox.TabIndex = 1;
			// 
			// measurementPeriodUnitLabel
			// 
			this.measurementPeriodUnitLabel.AutoSize = true;
			this.measurementPeriodUnitLabel.Location = new System.Drawing.Point(94, 165);
			this.measurementPeriodUnitLabel.Name = "measurementPeriodUnitLabel";
			this.measurementPeriodUnitLabel.Size = new System.Drawing.Size(24, 13);
			this.measurementPeriodUnitLabel.TabIndex = 33;
			this.measurementPeriodUnitLabel.Text = "sec";
			// 
			// coilFrequencyUnitLabel
			// 
			this.coilFrequencyUnitLabel.AutoSize = true;
			this.coilFrequencyUnitLabel.Location = new System.Drawing.Point(261, 98);
			this.coilFrequencyUnitLabel.Name = "coilFrequencyUnitLabel";
			this.coilFrequencyUnitLabel.Size = new System.Drawing.Size(20, 13);
			this.coilFrequencyUnitLabel.TabIndex = 31;
			this.coilFrequencyUnitLabel.Text = "Hz";
			// 
			// wheatstoneFrequencyUnitLabel
			// 
			this.wheatstoneFrequencyUnitLabel.AutoSize = true;
			this.wheatstoneFrequencyUnitLabel.Location = new System.Drawing.Point(261, 39);
			this.wheatstoneFrequencyUnitLabel.Name = "wheatstoneFrequencyUnitLabel";
			this.wheatstoneFrequencyUnitLabel.Size = new System.Drawing.Size(20, 13);
			this.wheatstoneFrequencyUnitLabel.TabIndex = 29;
			this.wheatstoneFrequencyUnitLabel.Text = "Hz";
			// 
			// coilDcOffsetTextBox
			// 
			this.coilDcOffsetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilDcOffsetTextBox.Location = new System.Drawing.Point(336, 95);
			this.coilDcOffsetTextBox.Name = "coilDcOffsetTextBox";
			this.coilDcOffsetTextBox.Size = new System.Drawing.Size(82, 20);
			this.coilDcOffsetTextBox.TabIndex = 7;
			// 
			// coilDcOffsetLabel
			// 
			this.coilDcOffsetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilDcOffsetLabel.AutoSize = true;
			this.coilDcOffsetLabel.Location = new System.Drawing.Point(333, 79);
			this.coilDcOffsetLabel.Name = "coilDcOffsetLabel";
			this.coilDcOffsetLabel.Size = new System.Drawing.Size(73, 13);
			this.coilDcOffsetLabel.TabIndex = 26;
			this.coilDcOffsetLabel.Text = "Coil DC Offset";
			// 
			// wheatstoneAmplitudeTextBox
			// 
			this.wheatstoneAmplitudeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.wheatstoneAmplitudeTextBox.Location = new System.Drawing.Point(9, 36);
			this.wheatstoneAmplitudeTextBox.Name = "wheatstoneAmplitudeTextBox";
			this.wheatstoneAmplitudeTextBox.Size = new System.Drawing.Size(79, 20);
			this.wheatstoneAmplitudeTextBox.TabIndex = 0;
			// 
			// coilAmplitudeTextBox
			// 
			this.coilAmplitudeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilAmplitudeTextBox.Location = new System.Drawing.Point(6, 95);
			this.coilAmplitudeTextBox.Name = "coilAmplitudeTextBox";
			this.coilAmplitudeTextBox.Size = new System.Drawing.Size(82, 20);
			this.coilAmplitudeTextBox.TabIndex = 2;
			// 
			// wheatstoneFrequencyTextBox
			// 
			this.wheatstoneFrequencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.wheatstoneFrequencyTextBox.Location = new System.Drawing.Point(173, 36);
			this.wheatstoneFrequencyTextBox.Name = "wheatstoneFrequencyTextBox";
			this.wheatstoneFrequencyTextBox.Size = new System.Drawing.Size(82, 20);
			this.wheatstoneFrequencyTextBox.TabIndex = 5;
			// 
			// coilFrequencyTextBox
			// 
			this.coilFrequencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilFrequencyTextBox.Location = new System.Drawing.Point(173, 95);
			this.coilFrequencyTextBox.Name = "coilFrequencyTextBox";
			this.coilFrequencyTextBox.Size = new System.Drawing.Size(82, 20);
			this.coilFrequencyTextBox.TabIndex = 6;
			// 
			// measurementPeriodTextBox
			// 
			this.measurementPeriodTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.measurementPeriodTextBox.Location = new System.Drawing.Point(6, 158);
			this.measurementPeriodTextBox.Name = "measurementPeriodTextBox";
			this.measurementPeriodTextBox.Size = new System.Drawing.Size(82, 20);
			this.measurementPeriodTextBox.TabIndex = 4;
			// 
			// measurementPeriodLabel
			// 
			this.measurementPeriodLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.measurementPeriodLabel.AutoSize = true;
			this.measurementPeriodLabel.Location = new System.Drawing.Point(6, 142);
			this.measurementPeriodLabel.Name = "measurementPeriodLabel";
			this.measurementPeriodLabel.Size = new System.Drawing.Size(104, 13);
			this.measurementPeriodLabel.TabIndex = 25;
			this.measurementPeriodLabel.Text = "Measurement Period";
			// 
			// coilFrequencyLabel
			// 
			this.coilFrequencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilFrequencyLabel.AutoSize = true;
			this.coilFrequencyLabel.Location = new System.Drawing.Point(170, 79);
			this.coilFrequencyLabel.Name = "coilFrequencyLabel";
			this.coilFrequencyLabel.Size = new System.Drawing.Size(77, 13);
			this.coilFrequencyLabel.TabIndex = 24;
			this.coilFrequencyLabel.Text = "Coil Frequency";
			// 
			// wheatstoneFrequencyLabel
			// 
			this.wheatstoneFrequencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.wheatstoneFrequencyLabel.AutoSize = true;
			this.wheatstoneFrequencyLabel.Location = new System.Drawing.Point(170, 20);
			this.wheatstoneFrequencyLabel.Name = "wheatstoneFrequencyLabel";
			this.wheatstoneFrequencyLabel.Size = new System.Drawing.Size(118, 13);
			this.wheatstoneFrequencyLabel.TabIndex = 23;
			this.wheatstoneFrequencyLabel.Text = "Wheatstone Frequency";
			// 
			// coilAmplitudeLabel
			// 
			this.coilAmplitudeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.coilAmplitudeLabel.AutoSize = true;
			this.coilAmplitudeLabel.Location = new System.Drawing.Point(6, 79);
			this.coilAmplitudeLabel.Name = "coilAmplitudeLabel";
			this.coilAmplitudeLabel.Size = new System.Drawing.Size(73, 13);
			this.coilAmplitudeLabel.TabIndex = 22;
			this.coilAmplitudeLabel.Text = "Coil Amplitude";
			// 
			// wheatstoneAmplitudeLabel
			// 
			this.wheatstoneAmplitudeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.wheatstoneAmplitudeLabel.AutoSize = true;
			this.wheatstoneAmplitudeLabel.Location = new System.Drawing.Point(6, 20);
			this.wheatstoneAmplitudeLabel.Name = "wheatstoneAmplitudeLabel";
			this.wheatstoneAmplitudeLabel.Size = new System.Drawing.Size(114, 13);
			this.wheatstoneAmplitudeLabel.TabIndex = 21;
			this.wheatstoneAmplitudeLabel.Text = "Wheatstone Amplitude";
			// 
			// revertMeasurementParametersButton
			// 
			this.revertMeasurementParametersButton.Location = new System.Drawing.Point(6, 198);
			this.revertMeasurementParametersButton.Name = "revertMeasurementParametersButton";
			this.revertMeasurementParametersButton.Size = new System.Drawing.Size(75, 23);
			this.revertMeasurementParametersButton.TabIndex = 9;
			this.revertMeasurementParametersButton.Text = "Revert";
			this.revertMeasurementParametersButton.UseVisualStyleBackColor = true;
			this.revertMeasurementParametersButton.Click += new System.EventHandler(this.revertMeasurementParametersButton_Click);
			// 
			// pinMultiplexerValuesTabPage
			// 
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor30);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor29);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor28);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor27);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor26);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor25);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor24);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor23);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor22);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor21);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor20);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor19);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor18);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor17);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor16);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor15);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor14);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor13);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor12);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor11);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor10);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor9);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor8);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor7);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor6);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor5);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor4);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor3);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor2);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.sensor1);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.revertPinAssignmentsButton);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox30);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox29);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox28);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox27);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox26);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox25);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox24);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox23);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox12);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox13);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox14);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox15);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox16);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox17);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox18);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox19);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox20);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox21);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox22);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox11);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox10);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox09);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox08);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox07);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox06);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox05);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox04);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox03);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox02);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.textBox01);
			this.pinMultiplexerValuesTabPage.Controls.Add(this.label1);
			this.pinMultiplexerValuesTabPage.Location = new System.Drawing.Point(4, 22);
			this.pinMultiplexerValuesTabPage.Name = "pinMultiplexerValuesTabPage";
			this.pinMultiplexerValuesTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.pinMultiplexerValuesTabPage.Size = new System.Drawing.Size(524, 227);
			this.pinMultiplexerValuesTabPage.TabIndex = 0;
			this.pinMultiplexerValuesTabPage.Text = "Pin Multiplexer Values";
			this.pinMultiplexerValuesTabPage.UseVisualStyleBackColor = true;
			// 
			// sensor30
			// 
			this.sensor30.AutoSize = true;
			this.sensor30.Location = new System.Drawing.Point(444, 168);
			this.sensor30.Name = "sensor30";
			this.sensor30.Size = new System.Drawing.Size(19, 13);
			this.sensor30.TabIndex = 62;
			this.sensor30.Text = "30";
			// 
			// sensor29
			// 
			this.sensor29.AutoSize = true;
			this.sensor29.Location = new System.Drawing.Point(398, 168);
			this.sensor29.Name = "sensor29";
			this.sensor29.Size = new System.Drawing.Size(19, 13);
			this.sensor29.TabIndex = 61;
			this.sensor29.Text = "29";
			// 
			// sensor28
			// 
			this.sensor28.AutoSize = true;
			this.sensor28.Location = new System.Drawing.Point(352, 168);
			this.sensor28.Name = "sensor28";
			this.sensor28.Size = new System.Drawing.Size(19, 13);
			this.sensor28.TabIndex = 60;
			this.sensor28.Text = "28";
			// 
			// sensor27
			// 
			this.sensor27.AutoSize = true;
			this.sensor27.Location = new System.Drawing.Point(306, 168);
			this.sensor27.Name = "sensor27";
			this.sensor27.Size = new System.Drawing.Size(19, 13);
			this.sensor27.TabIndex = 59;
			this.sensor27.Text = "27";
			// 
			// sensor26
			// 
			this.sensor26.AutoSize = true;
			this.sensor26.Location = new System.Drawing.Point(260, 168);
			this.sensor26.Name = "sensor26";
			this.sensor26.Size = new System.Drawing.Size(19, 13);
			this.sensor26.TabIndex = 58;
			this.sensor26.Text = "26";
			// 
			// sensor25
			// 
			this.sensor25.AutoSize = true;
			this.sensor25.Location = new System.Drawing.Point(214, 168);
			this.sensor25.Name = "sensor25";
			this.sensor25.Size = new System.Drawing.Size(19, 13);
			this.sensor25.TabIndex = 57;
			this.sensor25.Text = "25";
			// 
			// sensor24
			// 
			this.sensor24.AutoSize = true;
			this.sensor24.Location = new System.Drawing.Point(168, 168);
			this.sensor24.Name = "sensor24";
			this.sensor24.Size = new System.Drawing.Size(19, 13);
			this.sensor24.TabIndex = 56;
			this.sensor24.Text = "24";
			// 
			// sensor23
			// 
			this.sensor23.AutoSize = true;
			this.sensor23.Location = new System.Drawing.Point(122, 168);
			this.sensor23.Name = "sensor23";
			this.sensor23.Size = new System.Drawing.Size(19, 13);
			this.sensor23.TabIndex = 55;
			this.sensor23.Text = "23";
			// 
			// sensor22
			// 
			this.sensor22.AutoSize = true;
			this.sensor22.Location = new System.Drawing.Point(76, 168);
			this.sensor22.Name = "sensor22";
			this.sensor22.Size = new System.Drawing.Size(19, 13);
			this.sensor22.TabIndex = 54;
			this.sensor22.Text = "22";
			// 
			// sensor21
			// 
			this.sensor21.AutoSize = true;
			this.sensor21.Location = new System.Drawing.Point(30, 168);
			this.sensor21.Name = "sensor21";
			this.sensor21.Size = new System.Drawing.Size(19, 13);
			this.sensor21.TabIndex = 53;
			this.sensor21.Text = "21";
			// 
			// sensor20
			// 
			this.sensor20.AutoSize = true;
			this.sensor20.Location = new System.Drawing.Point(444, 112);
			this.sensor20.Name = "sensor20";
			this.sensor20.Size = new System.Drawing.Size(19, 13);
			this.sensor20.TabIndex = 52;
			this.sensor20.Text = "20";
			// 
			// sensor19
			// 
			this.sensor19.AutoSize = true;
			this.sensor19.Location = new System.Drawing.Point(398, 112);
			this.sensor19.Name = "sensor19";
			this.sensor19.Size = new System.Drawing.Size(19, 13);
			this.sensor19.TabIndex = 51;
			this.sensor19.Text = "19";
			// 
			// sensor18
			// 
			this.sensor18.AutoSize = true;
			this.sensor18.Location = new System.Drawing.Point(352, 112);
			this.sensor18.Name = "sensor18";
			this.sensor18.Size = new System.Drawing.Size(19, 13);
			this.sensor18.TabIndex = 50;
			this.sensor18.Text = "18";
			// 
			// sensor17
			// 
			this.sensor17.AutoSize = true;
			this.sensor17.Location = new System.Drawing.Point(306, 112);
			this.sensor17.Name = "sensor17";
			this.sensor17.Size = new System.Drawing.Size(19, 13);
			this.sensor17.TabIndex = 49;
			this.sensor17.Text = "17";
			// 
			// sensor16
			// 
			this.sensor16.AutoSize = true;
			this.sensor16.Enabled = false;
			this.sensor16.Location = new System.Drawing.Point(260, 112);
			this.sensor16.Name = "sensor16";
			this.sensor16.Size = new System.Drawing.Size(19, 13);
			this.sensor16.TabIndex = 48;
			this.sensor16.Text = "16";
			// 
			// sensor15
			// 
			this.sensor15.AutoSize = true;
			this.sensor15.Location = new System.Drawing.Point(214, 112);
			this.sensor15.Name = "sensor15";
			this.sensor15.Size = new System.Drawing.Size(19, 13);
			this.sensor15.TabIndex = 47;
			this.sensor15.Text = "15";
			// 
			// sensor14
			// 
			this.sensor14.AutoSize = true;
			this.sensor14.Location = new System.Drawing.Point(168, 112);
			this.sensor14.Name = "sensor14";
			this.sensor14.Size = new System.Drawing.Size(19, 13);
			this.sensor14.TabIndex = 46;
			this.sensor14.Text = "14";
			// 
			// sensor13
			// 
			this.sensor13.AutoSize = true;
			this.sensor13.Location = new System.Drawing.Point(122, 112);
			this.sensor13.Name = "sensor13";
			this.sensor13.Size = new System.Drawing.Size(19, 13);
			this.sensor13.TabIndex = 45;
			this.sensor13.Text = "13";
			// 
			// sensor12
			// 
			this.sensor12.AutoSize = true;
			this.sensor12.Location = new System.Drawing.Point(76, 112);
			this.sensor12.Name = "sensor12";
			this.sensor12.Size = new System.Drawing.Size(19, 13);
			this.sensor12.TabIndex = 44;
			this.sensor12.Text = "12";
			// 
			// sensor11
			// 
			this.sensor11.AutoSize = true;
			this.sensor11.Location = new System.Drawing.Point(30, 112);
			this.sensor11.Name = "sensor11";
			this.sensor11.Size = new System.Drawing.Size(19, 13);
			this.sensor11.TabIndex = 43;
			this.sensor11.Text = "11";
			// 
			// sensor10
			// 
			this.sensor10.AutoSize = true;
			this.sensor10.Location = new System.Drawing.Point(444, 54);
			this.sensor10.Name = "sensor10";
			this.sensor10.Size = new System.Drawing.Size(19, 13);
			this.sensor10.TabIndex = 42;
			this.sensor10.Text = "10";
			// 
			// sensor9
			// 
			this.sensor9.AutoSize = true;
			this.sensor9.Location = new System.Drawing.Point(398, 54);
			this.sensor9.Name = "sensor9";
			this.sensor9.Size = new System.Drawing.Size(13, 13);
			this.sensor9.TabIndex = 41;
			this.sensor9.Text = "9";
			// 
			// sensor8
			// 
			this.sensor8.AutoSize = true;
			this.sensor8.Location = new System.Drawing.Point(352, 54);
			this.sensor8.Name = "sensor8";
			this.sensor8.Size = new System.Drawing.Size(13, 13);
			this.sensor8.TabIndex = 40;
			this.sensor8.Text = "8";
			// 
			// sensor7
			// 
			this.sensor7.AutoSize = true;
			this.sensor7.Location = new System.Drawing.Point(306, 54);
			this.sensor7.Name = "sensor7";
			this.sensor7.Size = new System.Drawing.Size(13, 13);
			this.sensor7.TabIndex = 39;
			this.sensor7.Text = "7";
			// 
			// sensor6
			// 
			this.sensor6.AutoSize = true;
			this.sensor6.Location = new System.Drawing.Point(260, 54);
			this.sensor6.Name = "sensor6";
			this.sensor6.Size = new System.Drawing.Size(13, 13);
			this.sensor6.TabIndex = 38;
			this.sensor6.Text = "6";
			// 
			// sensor5
			// 
			this.sensor5.AutoSize = true;
			this.sensor5.Location = new System.Drawing.Point(214, 54);
			this.sensor5.Name = "sensor5";
			this.sensor5.Size = new System.Drawing.Size(13, 13);
			this.sensor5.TabIndex = 37;
			this.sensor5.Text = "5";
			// 
			// sensor4
			// 
			this.sensor4.AutoSize = true;
			this.sensor4.Location = new System.Drawing.Point(168, 54);
			this.sensor4.Name = "sensor4";
			this.sensor4.Size = new System.Drawing.Size(13, 13);
			this.sensor4.TabIndex = 36;
			this.sensor4.Text = "4";
			// 
			// sensor3
			// 
			this.sensor3.AutoSize = true;
			this.sensor3.Location = new System.Drawing.Point(122, 54);
			this.sensor3.Name = "sensor3";
			this.sensor3.Size = new System.Drawing.Size(13, 13);
			this.sensor3.TabIndex = 35;
			this.sensor3.Text = "3";
			// 
			// sensor2
			// 
			this.sensor2.AutoSize = true;
			this.sensor2.Location = new System.Drawing.Point(76, 54);
			this.sensor2.Name = "sensor2";
			this.sensor2.Size = new System.Drawing.Size(13, 13);
			this.sensor2.TabIndex = 34;
			this.sensor2.Text = "2";
			// 
			// sensor1
			// 
			this.sensor1.AutoSize = true;
			this.sensor1.Location = new System.Drawing.Point(30, 54);
			this.sensor1.Name = "sensor1";
			this.sensor1.Size = new System.Drawing.Size(13, 13);
			this.sensor1.TabIndex = 33;
			this.sensor1.Text = "1";
			// 
			// revertPinAssignmentsButton
			// 
			this.revertPinAssignmentsButton.Location = new System.Drawing.Point(6, 198);
			this.revertPinAssignmentsButton.Name = "revertPinAssignmentsButton";
			this.revertPinAssignmentsButton.Size = new System.Drawing.Size(75, 23);
			this.revertPinAssignmentsButton.TabIndex = 35;
			this.revertPinAssignmentsButton.Text = "Revert";
			this.revertPinAssignmentsButton.UseVisualStyleBackColor = true;
			this.revertPinAssignmentsButton.Click += new System.EventHandler(this.revertPinAssignmentsButton_Click);
			// 
			// textBox30
			// 
			this.textBox30.Location = new System.Drawing.Point(447, 145);
			this.textBox30.Name = "textBox30";
			this.textBox30.Size = new System.Drawing.Size(40, 20);
			this.textBox30.TabIndex = 29;
			// 
			// textBox29
			// 
			this.textBox29.Location = new System.Drawing.Point(401, 145);
			this.textBox29.Name = "textBox29";
			this.textBox29.Size = new System.Drawing.Size(40, 20);
			this.textBox29.TabIndex = 28;
			// 
			// textBox28
			// 
			this.textBox28.Location = new System.Drawing.Point(355, 145);
			this.textBox28.Name = "textBox28";
			this.textBox28.Size = new System.Drawing.Size(40, 20);
			this.textBox28.TabIndex = 27;
			// 
			// textBox27
			// 
			this.textBox27.Location = new System.Drawing.Point(309, 145);
			this.textBox27.Name = "textBox27";
			this.textBox27.Size = new System.Drawing.Size(40, 20);
			this.textBox27.TabIndex = 26;
			// 
			// textBox26
			// 
			this.textBox26.Location = new System.Drawing.Point(263, 145);
			this.textBox26.Name = "textBox26";
			this.textBox26.Size = new System.Drawing.Size(40, 20);
			this.textBox26.TabIndex = 25;
			// 
			// textBox25
			// 
			this.textBox25.Location = new System.Drawing.Point(217, 145);
			this.textBox25.Name = "textBox25";
			this.textBox25.Size = new System.Drawing.Size(40, 20);
			this.textBox25.TabIndex = 24;
			// 
			// textBox24
			// 
			this.textBox24.Location = new System.Drawing.Point(171, 145);
			this.textBox24.Name = "textBox24";
			this.textBox24.Size = new System.Drawing.Size(40, 20);
			this.textBox24.TabIndex = 23;
			// 
			// textBox23
			// 
			this.textBox23.Location = new System.Drawing.Point(125, 145);
			this.textBox23.Name = "textBox23";
			this.textBox23.Size = new System.Drawing.Size(40, 20);
			this.textBox23.TabIndex = 22;
			// 
			// textBox12
			// 
			this.textBox12.Location = new System.Drawing.Point(79, 89);
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new System.Drawing.Size(40, 20);
			this.textBox12.TabIndex = 11;
			// 
			// textBox13
			// 
			this.textBox13.Location = new System.Drawing.Point(125, 89);
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new System.Drawing.Size(40, 20);
			this.textBox13.TabIndex = 12;
			// 
			// textBox14
			// 
			this.textBox14.Location = new System.Drawing.Point(171, 89);
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new System.Drawing.Size(40, 20);
			this.textBox14.TabIndex = 13;
			// 
			// textBox15
			// 
			this.textBox15.Location = new System.Drawing.Point(217, 89);
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new System.Drawing.Size(40, 20);
			this.textBox15.TabIndex = 14;
			// 
			// textBox16
			// 
			this.textBox16.Enabled = false;
			this.textBox16.Location = new System.Drawing.Point(263, 89);
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new System.Drawing.Size(40, 20);
			this.textBox16.TabIndex = 15;
			// 
			// textBox17
			// 
			this.textBox17.Location = new System.Drawing.Point(309, 89);
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new System.Drawing.Size(40, 20);
			this.textBox17.TabIndex = 16;
			// 
			// textBox18
			// 
			this.textBox18.Location = new System.Drawing.Point(355, 89);
			this.textBox18.Name = "textBox18";
			this.textBox18.Size = new System.Drawing.Size(40, 20);
			this.textBox18.TabIndex = 17;
			// 
			// textBox19
			// 
			this.textBox19.Location = new System.Drawing.Point(401, 89);
			this.textBox19.Name = "textBox19";
			this.textBox19.Size = new System.Drawing.Size(40, 20);
			this.textBox19.TabIndex = 18;
			// 
			// textBox20
			// 
			this.textBox20.Location = new System.Drawing.Point(447, 89);
			this.textBox20.Name = "textBox20";
			this.textBox20.Size = new System.Drawing.Size(40, 20);
			this.textBox20.TabIndex = 19;
			// 
			// textBox21
			// 
			this.textBox21.Location = new System.Drawing.Point(33, 145);
			this.textBox21.Name = "textBox21";
			this.textBox21.Size = new System.Drawing.Size(40, 20);
			this.textBox21.TabIndex = 20;
			// 
			// textBox22
			// 
			this.textBox22.Location = new System.Drawing.Point(79, 145);
			this.textBox22.Name = "textBox22";
			this.textBox22.Size = new System.Drawing.Size(40, 20);
			this.textBox22.TabIndex = 21;
			// 
			// textBox11
			// 
			this.textBox11.Location = new System.Drawing.Point(33, 89);
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new System.Drawing.Size(40, 20);
			this.textBox11.TabIndex = 10;
			// 
			// textBox10
			// 
			this.textBox10.Location = new System.Drawing.Point(447, 31);
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new System.Drawing.Size(40, 20);
			this.textBox10.TabIndex = 9;
			// 
			// textBox09
			// 
			this.textBox09.Location = new System.Drawing.Point(401, 31);
			this.textBox09.Name = "textBox09";
			this.textBox09.Size = new System.Drawing.Size(40, 20);
			this.textBox09.TabIndex = 8;
			// 
			// textBox08
			// 
			this.textBox08.Location = new System.Drawing.Point(355, 31);
			this.textBox08.Name = "textBox08";
			this.textBox08.Size = new System.Drawing.Size(40, 20);
			this.textBox08.TabIndex = 7;
			// 
			// textBox07
			// 
			this.textBox07.Location = new System.Drawing.Point(309, 31);
			this.textBox07.Name = "textBox07";
			this.textBox07.Size = new System.Drawing.Size(40, 20);
			this.textBox07.TabIndex = 6;
			// 
			// textBox06
			// 
			this.textBox06.Location = new System.Drawing.Point(263, 31);
			this.textBox06.Name = "textBox06";
			this.textBox06.Size = new System.Drawing.Size(40, 20);
			this.textBox06.TabIndex = 5;
			// 
			// textBox05
			// 
			this.textBox05.Location = new System.Drawing.Point(217, 31);
			this.textBox05.Name = "textBox05";
			this.textBox05.Size = new System.Drawing.Size(40, 20);
			this.textBox05.TabIndex = 4;
			// 
			// textBox04
			// 
			this.textBox04.Location = new System.Drawing.Point(171, 31);
			this.textBox04.Name = "textBox04";
			this.textBox04.Size = new System.Drawing.Size(40, 20);
			this.textBox04.TabIndex = 3;
			// 
			// textBox03
			// 
			this.textBox03.Location = new System.Drawing.Point(125, 31);
			this.textBox03.Name = "textBox03";
			this.textBox03.Size = new System.Drawing.Size(40, 20);
			this.textBox03.TabIndex = 2;
			// 
			// textBox02
			// 
			this.textBox02.Location = new System.Drawing.Point(79, 31);
			this.textBox02.Name = "textBox02";
			this.textBox02.Size = new System.Drawing.Size(40, 20);
			this.textBox02.TabIndex = 1;
			// 
			// textBox01
			// 
			this.textBox01.Location = new System.Drawing.Point(33, 31);
			this.textBox01.Name = "textBox01";
			this.textBox01.Size = new System.Drawing.Size(40, 20);
			this.textBox01.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(25, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Pin:";
			// 
			// logInformationTabPage
			// 
			this.logInformationTabPage.Controls.Add(this.defaultVolumeUnitLabel);
			this.logInformationTabPage.Controls.Add(this.defaultVolumeUnitComboBox);
			this.logInformationTabPage.Controls.Add(this.defaultAddMnpsVolumeTextBox);
			this.logInformationTabPage.Controls.Add(this.defaultAddMnpsVolumeLabel);
			this.logInformationTabPage.Controls.Add(this.defaultAddBufferVolumeTextBox);
			this.logInformationTabPage.Controls.Add(this.defaultAddBufferVolumeLabel);
			this.logInformationTabPage.Controls.Add(this.preloadBufferVolumeTextBox);
			this.logInformationTabPage.Controls.Add(this.preloadBufferVolumeLabel);
			this.logInformationTabPage.Controls.Add(this.mnpsNameTextBox);
			this.logInformationTabPage.Controls.Add(this.mnpsNameLabel);
			this.logInformationTabPage.Controls.Add(this.bufferNameLabel);
			this.logInformationTabPage.Controls.Add(this.bufferNameTextBox);
			this.logInformationTabPage.Controls.Add(this.revertLogInformationButton);
			this.logInformationTabPage.Location = new System.Drawing.Point(4, 22);
			this.logInformationTabPage.Name = "logInformationTabPage";
			this.logInformationTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.logInformationTabPage.Size = new System.Drawing.Size(524, 227);
			this.logInformationTabPage.TabIndex = 1;
			this.logInformationTabPage.Text = "Log Information";
			this.logInformationTabPage.UseVisualStyleBackColor = true;
			// 
			// defaultVolumeUnitLabel
			// 
			this.defaultVolumeUnitLabel.AutoSize = true;
			this.defaultVolumeUnitLabel.Location = new System.Drawing.Point(237, 97);
			this.defaultVolumeUnitLabel.Name = "defaultVolumeUnitLabel";
			this.defaultVolumeUnitLabel.Size = new System.Drawing.Size(101, 13);
			this.defaultVolumeUnitLabel.TabIndex = 56;
			this.defaultVolumeUnitLabel.Text = "Default Volume Unit";
			// 
			// defaultVolumeUnitComboBox
			// 
			this.defaultVolumeUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.defaultVolumeUnitComboBox.FormattingEnabled = true;
			this.defaultVolumeUnitComboBox.Items.AddRange(new object[] {
            "L",
            "mL",
            "uL",
            "nL"});
			this.defaultVolumeUnitComboBox.Location = new System.Drawing.Point(237, 113);
			this.defaultVolumeUnitComboBox.Name = "defaultVolumeUnitComboBox";
			this.defaultVolumeUnitComboBox.Size = new System.Drawing.Size(42, 21);
			this.defaultVolumeUnitComboBox.TabIndex = 5;
			// 
			// defaultAddMnpsVolumeTextBox
			// 
			this.defaultAddMnpsVolumeTextBox.Location = new System.Drawing.Point(237, 74);
			this.defaultAddMnpsVolumeTextBox.Name = "defaultAddMnpsVolumeTextBox";
			this.defaultAddMnpsVolumeTextBox.Size = new System.Drawing.Size(100, 20);
			this.defaultAddMnpsVolumeTextBox.TabIndex = 4;
			// 
			// defaultAddMnpsVolumeLabel
			// 
			this.defaultAddMnpsVolumeLabel.AutoSize = true;
			this.defaultAddMnpsVolumeLabel.Location = new System.Drawing.Point(237, 58);
			this.defaultAddMnpsVolumeLabel.Name = "defaultAddMnpsVolumeLabel";
			this.defaultAddMnpsVolumeLabel.Size = new System.Drawing.Size(133, 13);
			this.defaultAddMnpsVolumeLabel.TabIndex = 12;
			this.defaultAddMnpsVolumeLabel.Text = "Default Add MNPs Volume";
			// 
			// defaultAddBufferVolumeTextBox
			// 
			this.defaultAddBufferVolumeTextBox.Location = new System.Drawing.Point(237, 36);
			this.defaultAddBufferVolumeTextBox.Name = "defaultAddBufferVolumeTextBox";
			this.defaultAddBufferVolumeTextBox.Size = new System.Drawing.Size(100, 20);
			this.defaultAddBufferVolumeTextBox.TabIndex = 3;
			// 
			// defaultAddBufferVolumeLabel
			// 
			this.defaultAddBufferVolumeLabel.AutoSize = true;
			this.defaultAddBufferVolumeLabel.Location = new System.Drawing.Point(234, 19);
			this.defaultAddBufferVolumeLabel.Name = "defaultAddBufferVolumeLabel";
			this.defaultAddBufferVolumeLabel.Size = new System.Drawing.Size(132, 13);
			this.defaultAddBufferVolumeLabel.TabIndex = 9;
			this.defaultAddBufferVolumeLabel.Text = "Default Add Buffer Volume";
			// 
			// preloadBufferVolumeTextBox
			// 
			this.preloadBufferVolumeTextBox.Location = new System.Drawing.Point(3, 113);
			this.preloadBufferVolumeTextBox.Name = "preloadBufferVolumeTextBox";
			this.preloadBufferVolumeTextBox.Size = new System.Drawing.Size(100, 20);
			this.preloadBufferVolumeTextBox.TabIndex = 2;
			// 
			// preloadBufferVolumeLabel
			// 
			this.preloadBufferVolumeLabel.AutoSize = true;
			this.preloadBufferVolumeLabel.Location = new System.Drawing.Point(7, 97);
			this.preloadBufferVolumeLabel.Name = "preloadBufferVolumeLabel";
			this.preloadBufferVolumeLabel.Size = new System.Drawing.Size(112, 13);
			this.preloadBufferVolumeLabel.TabIndex = 6;
			this.preloadBufferVolumeLabel.Text = "Preload Buffer Volume";
			// 
			// mnpsNameTextBox
			// 
			this.mnpsNameTextBox.Location = new System.Drawing.Point(3, 74);
			this.mnpsNameTextBox.Name = "mnpsNameTextBox";
			this.mnpsNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.mnpsNameTextBox.TabIndex = 1;
			// 
			// mnpsNameLabel
			// 
			this.mnpsNameLabel.AutoSize = true;
			this.mnpsNameLabel.Location = new System.Drawing.Point(7, 58);
			this.mnpsNameLabel.Name = "mnpsNameLabel";
			this.mnpsNameLabel.Size = new System.Drawing.Size(67, 13);
			this.mnpsNameLabel.TabIndex = 4;
			this.mnpsNameLabel.Text = "MNPs Name";
			// 
			// bufferNameLabel
			// 
			this.bufferNameLabel.AutoSize = true;
			this.bufferNameLabel.Location = new System.Drawing.Point(7, 19);
			this.bufferNameLabel.Name = "bufferNameLabel";
			this.bufferNameLabel.Size = new System.Drawing.Size(66, 13);
			this.bufferNameLabel.TabIndex = 3;
			this.bufferNameLabel.Text = "Buffer Name";
			// 
			// bufferNameTextBox
			// 
			this.bufferNameTextBox.Location = new System.Drawing.Point(3, 35);
			this.bufferNameTextBox.Name = "bufferNameTextBox";
			this.bufferNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.bufferNameTextBox.TabIndex = 0;
			// 
			// revertLogInformationButton
			// 
			this.revertLogInformationButton.Location = new System.Drawing.Point(6, 198);
			this.revertLogInformationButton.Name = "revertLogInformationButton";
			this.revertLogInformationButton.Size = new System.Drawing.Size(75, 23);
			this.revertLogInformationButton.TabIndex = 6;
			this.revertLogInformationButton.Text = "Revert";
			this.revertLogInformationButton.UseVisualStyleBackColor = true;
			this.revertLogInformationButton.Click += new System.EventHandler(this.revertLogInformationButton_Click);
			// 
			// postProcessingTabPage
			// 
			this.postProcessingTabPage.Controls.Add(this.htPostProcessingFileCheckBox);
			this.postProcessingTabPage.Controls.Add(this.ltPostProcessingFileCheckBox);
			this.postProcessingTabPage.Controls.Add(this.postProcessingFilesLabel);
			this.postProcessingTabPage.Controls.Add(this.diffusionCountTextBox);
			this.postProcessingTabPage.Controls.Add(this.diffusionCountLabel);
			this.postProcessingTabPage.Controls.Add(this.revertPostProcessingButton);
			this.postProcessingTabPage.Controls.Add(this.sampleAverageCountTextBox);
			this.postProcessingTabPage.Controls.Add(this.sampleAverageCountLabel);
			this.postProcessingTabPage.Location = new System.Drawing.Point(4, 22);
			this.postProcessingTabPage.Name = "postProcessingTabPage";
			this.postProcessingTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.postProcessingTabPage.Size = new System.Drawing.Size(524, 227);
			this.postProcessingTabPage.TabIndex = 4;
			this.postProcessingTabPage.Text = "Post Processing";
			this.postProcessingTabPage.UseVisualStyleBackColor = true;
			// 
			// diffusionCountTextBox
			// 
			this.diffusionCountTextBox.Location = new System.Drawing.Point(29, 78);
			this.diffusionCountTextBox.Name = "diffusionCountTextBox";
			this.diffusionCountTextBox.Size = new System.Drawing.Size(100, 20);
			this.diffusionCountTextBox.TabIndex = 4;
			// 
			// diffusionCountLabel
			// 
			this.diffusionCountLabel.AutoSize = true;
			this.diffusionCountLabel.Location = new System.Drawing.Point(26, 61);
			this.diffusionCountLabel.Name = "diffusionCountLabel";
			this.diffusionCountLabel.Size = new System.Drawing.Size(119, 13);
			this.diffusionCountLabel.TabIndex = 3;
			this.diffusionCountLabel.Text = "Diffusion Cycles to Wait";
			// 
			// revertPostProcessingButton
			// 
			this.revertPostProcessingButton.Location = new System.Drawing.Point(6, 198);
			this.revertPostProcessingButton.Name = "revertPostProcessingButton";
			this.revertPostProcessingButton.Size = new System.Drawing.Size(75, 23);
			this.revertPostProcessingButton.TabIndex = 2;
			this.revertPostProcessingButton.Text = "Revert";
			this.revertPostProcessingButton.UseVisualStyleBackColor = true;
			this.revertPostProcessingButton.Click += new System.EventHandler(this.revertPostProcessingButton_Click);
			// 
			// sampleAverageCountTextBox
			// 
			this.sampleAverageCountTextBox.Location = new System.Drawing.Point(26, 34);
			this.sampleAverageCountTextBox.Name = "sampleAverageCountTextBox";
			this.sampleAverageCountTextBox.Size = new System.Drawing.Size(100, 20);
			this.sampleAverageCountTextBox.TabIndex = 0;
			// 
			// sampleAverageCountLabel
			// 
			this.sampleAverageCountLabel.AutoSize = true;
			this.sampleAverageCountLabel.Location = new System.Drawing.Point(23, 17);
			this.sampleAverageCountLabel.Name = "sampleAverageCountLabel";
			this.sampleAverageCountLabel.Size = new System.Drawing.Size(191, 13);
			this.sampleAverageCountLabel.TabIndex = 0;
			this.sampleAverageCountLabel.Text = "Number of Samples to Use for Average";
			// 
			// postProcessingFilesLabel
			// 
			this.postProcessingFilesLabel.AutoSize = true;
			this.postProcessingFilesLabel.Location = new System.Drawing.Point(26, 101);
			this.postProcessingFilesLabel.Name = "postProcessingFilesLabel";
			this.postProcessingFilesLabel.Size = new System.Drawing.Size(156, 13);
			this.postProcessingFilesLabel.TabIndex = 5;
			this.postProcessingFilesLabel.Text = "Files to Use for Post Processing";
			// 
			// ltPostProcessingFileCheckBox
			// 
			this.ltPostProcessingFileCheckBox.AutoSize = true;
			this.ltPostProcessingFileCheckBox.Location = new System.Drawing.Point(29, 118);
			this.ltPostProcessingFileCheckBox.Name = "ltPostProcessingFileCheckBox";
			this.ltPostProcessingFileCheckBox.Size = new System.Drawing.Size(39, 17);
			this.ltPostProcessingFileCheckBox.TabIndex = 6;
			this.ltPostProcessingFileCheckBox.Text = "LT";
			this.ltPostProcessingFileCheckBox.UseVisualStyleBackColor = true;
			// 
			// htPostProcessingFileCheckBox
			// 
			this.htPostProcessingFileCheckBox.AutoSize = true;
			this.htPostProcessingFileCheckBox.Location = new System.Drawing.Point(74, 118);
			this.htPostProcessingFileCheckBox.Name = "htPostProcessingFileCheckBox";
			this.htPostProcessingFileCheckBox.Size = new System.Drawing.Size(41, 17);
			this.htPostProcessingFileCheckBox.TabIndex = 7;
			this.htPostProcessingFileCheckBox.Text = "HT";
			this.htPostProcessingFileCheckBox.UseVisualStyleBackColor = true;
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 306);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.doneButton);
			this.Name = "Preferences";
			this.Text = "Preferences";
			this.tabControl1.ResumeLayout(false);
			this.generalTabPage.ResumeLayout(false);
			this.generalTabPage.PerformLayout();
			this.measurementParametersTabPage.ResumeLayout(false);
			this.measurementParametersTabPage.PerformLayout();
			this.pinMultiplexerValuesTabPage.ResumeLayout(false);
			this.pinMultiplexerValuesTabPage.PerformLayout();
			this.logInformationTabPage.ResumeLayout(false);
			this.logInformationTabPage.PerformLayout();
			this.postProcessingTabPage.ResumeLayout(false);
			this.postProcessingTabPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button doneButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage pinMultiplexerValuesTabPage;
		private System.Windows.Forms.TextBox textBox30;
		private System.Windows.Forms.TextBox textBox29;
		private System.Windows.Forms.TextBox textBox28;
		private System.Windows.Forms.TextBox textBox27;
		private System.Windows.Forms.TextBox textBox26;
		private System.Windows.Forms.TextBox textBox25;
		private System.Windows.Forms.TextBox textBox24;
		private System.Windows.Forms.TextBox textBox23;
		private System.Windows.Forms.TextBox textBox12;
		private System.Windows.Forms.TextBox textBox13;
		private System.Windows.Forms.TextBox textBox14;
		private System.Windows.Forms.TextBox textBox15;
		private System.Windows.Forms.TextBox textBox16;
		private System.Windows.Forms.TextBox textBox17;
		private System.Windows.Forms.TextBox textBox18;
		private System.Windows.Forms.TextBox textBox19;
		private System.Windows.Forms.TextBox textBox20;
		private System.Windows.Forms.TextBox textBox21;
		private System.Windows.Forms.TextBox textBox22;
		private System.Windows.Forms.TextBox textBox11;
		private System.Windows.Forms.TextBox textBox10;
		private System.Windows.Forms.TextBox textBox09;
		private System.Windows.Forms.TextBox textBox08;
		private System.Windows.Forms.TextBox textBox07;
		private System.Windows.Forms.TextBox textBox06;
		private System.Windows.Forms.TextBox textBox05;
		private System.Windows.Forms.TextBox textBox04;
		private System.Windows.Forms.TextBox textBox03;
		private System.Windows.Forms.TextBox textBox02;
		private System.Windows.Forms.TextBox textBox01;
		private System.Windows.Forms.Label label1;
		private Label sensor30;
		private Label sensor29;
		private Label sensor28;
		private Label sensor27;
		private Label sensor26;
		private Label sensor25;
		private Label sensor24;
		private Label sensor23;
		private Label sensor22;
		private Label sensor21;
		private Label sensor20;
		private Label sensor19;
		private Label sensor18;
		private Label sensor17;
		private Label sensor16;
		private Label sensor15;
		private Label sensor14;
		private Label sensor13;
		private Label sensor12;
		private Label sensor11;
		private Label sensor10;
		private Label sensor9;
		private Label sensor8;
		private Label sensor7;
		private Label sensor6;
		private Label sensor5;
		private Label sensor4;
		private Label sensor3;
		private Label sensor2;
		private Label sensor1;
		private TabPage logInformationTabPage;
		private Button revertLogInformationButton;
		private TextBox mnpsNameTextBox;
		private Label mnpsNameLabel;
		private Label bufferNameLabel;
		private TextBox bufferNameTextBox;
		private Button revertPinAssignmentsButton;
		private TabPage generalTabPage;
		private Button revertGeneralButton;
		private Label tempFoldersToKeepLabel;
		private TextBox tempFoldersToKeepTextBox;
		private TextBox preloadBufferVolumeTextBox;
		private Label preloadBufferVolumeLabel;
		private TextBox defaultAddMnpsVolumeTextBox;
		private Label defaultAddMnpsVolumeLabel;
		private TextBox defaultAddBufferVolumeTextBox;
		private Label defaultAddBufferVolumeLabel;
		private ComboBox defaultVolumeUnitComboBox;
		private TabPage measurementParametersTabPage;
		private Button revertMeasurementParametersButton;
		private TextBox coilDcOffsetTextBox;
		private Label coilDcOffsetLabel;
		private TextBox wheatstoneAmplitudeTextBox;
		private TextBox coilAmplitudeTextBox;
		private TextBox wheatstoneFrequencyTextBox;
		private TextBox coilFrequencyTextBox;
		private TextBox measurementPeriodTextBox;
		private Label measurementPeriodLabel;
		private Label coilFrequencyLabel;
		private Label wheatstoneFrequencyLabel;
		private Label coilAmplitudeLabel;
		private Label wheatstoneAmplitudeLabel;
		private Label defaultVolumeUnitLabel;
		private Label measurementPeriodUnitLabel;
		private Label coilFrequencyUnitLabel;
		private Label wheatstoneFrequencyUnitLabel;
		private TextBox coilDcOffsetUnitTextBox;
		private TextBox coilAmplitudeUnitTextBox;
		private TextBox wheatstoneAmplitudeUnitTextBox;
		private TabPage postProcessingTabPage;
		private TextBox sampleAverageCountTextBox;
		private Label sampleAverageCountLabel;
		private Button revertPostProcessingButton;
		private TextBox diffusionCountTextBox;
		private Label diffusionCountLabel;
		private CheckBox htPostProcessingFileCheckBox;
		private CheckBox ltPostProcessingFileCheckBox;
		private Label postProcessingFilesLabel;
	}
}