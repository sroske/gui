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
			this.sampleTextBox = new System.Windows.Forms.TextBox();
			this.MnpsNameLabel = new System.Windows.Forms.Label();
			this.bufferNameLabel = new System.Windows.Forms.Label();
			this.reactionWellTextBox = new System.Windows.Forms.TextBox();
			this.revertInitFileValuesButton = new System.Windows.Forms.Button();
			this.preloadBufferVolumeLabel = new System.Windows.Forms.Label();
			this.preloadBufferVolumeTextBox = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.generalTabPage.SuspendLayout();
			this.pinMultiplexerValuesTabPage.SuspendLayout();
			this.logInformationTabPage.SuspendLayout();
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
			this.tabControl1.Controls.Add(this.pinMultiplexerValuesTabPage);
			this.tabControl1.Controls.Add(this.logInformationTabPage);
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(532, 253);
			this.tabControl1.TabIndex = 2;
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
			this.tempFoldersToKeepTextBox.TabIndex = 2;
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
			this.revertPinAssignmentsButton.TabIndex = 32;
			this.revertPinAssignmentsButton.Text = "Revert";
			this.revertPinAssignmentsButton.UseVisualStyleBackColor = true;
			this.revertPinAssignmentsButton.Click += new System.EventHandler(this.revertPinAssignmentsButton_Click);
			// 
			// textBox30
			// 
			this.textBox30.Location = new System.Drawing.Point(447, 145);
			this.textBox30.Name = "textBox30";
			this.textBox30.Size = new System.Drawing.Size(40, 20);
			this.textBox30.TabIndex = 30;
			// 
			// textBox29
			// 
			this.textBox29.Location = new System.Drawing.Point(401, 145);
			this.textBox29.Name = "textBox29";
			this.textBox29.Size = new System.Drawing.Size(40, 20);
			this.textBox29.TabIndex = 29;
			// 
			// textBox28
			// 
			this.textBox28.Location = new System.Drawing.Point(355, 145);
			this.textBox28.Name = "textBox28";
			this.textBox28.Size = new System.Drawing.Size(40, 20);
			this.textBox28.TabIndex = 28;
			// 
			// textBox27
			// 
			this.textBox27.Location = new System.Drawing.Point(309, 145);
			this.textBox27.Name = "textBox27";
			this.textBox27.Size = new System.Drawing.Size(40, 20);
			this.textBox27.TabIndex = 27;
			// 
			// textBox26
			// 
			this.textBox26.Location = new System.Drawing.Point(263, 145);
			this.textBox26.Name = "textBox26";
			this.textBox26.Size = new System.Drawing.Size(40, 20);
			this.textBox26.TabIndex = 26;
			// 
			// textBox25
			// 
			this.textBox25.Location = new System.Drawing.Point(217, 145);
			this.textBox25.Name = "textBox25";
			this.textBox25.Size = new System.Drawing.Size(40, 20);
			this.textBox25.TabIndex = 25;
			// 
			// textBox24
			// 
			this.textBox24.Location = new System.Drawing.Point(171, 145);
			this.textBox24.Name = "textBox24";
			this.textBox24.Size = new System.Drawing.Size(40, 20);
			this.textBox24.TabIndex = 24;
			// 
			// textBox23
			// 
			this.textBox23.Location = new System.Drawing.Point(125, 145);
			this.textBox23.Name = "textBox23";
			this.textBox23.Size = new System.Drawing.Size(40, 20);
			this.textBox23.TabIndex = 23;
			// 
			// textBox12
			// 
			this.textBox12.Location = new System.Drawing.Point(79, 89);
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new System.Drawing.Size(40, 20);
			this.textBox12.TabIndex = 22;
			// 
			// textBox13
			// 
			this.textBox13.Location = new System.Drawing.Point(125, 89);
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new System.Drawing.Size(40, 20);
			this.textBox13.TabIndex = 21;
			// 
			// textBox14
			// 
			this.textBox14.Location = new System.Drawing.Point(171, 89);
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new System.Drawing.Size(40, 20);
			this.textBox14.TabIndex = 20;
			// 
			// textBox15
			// 
			this.textBox15.Location = new System.Drawing.Point(217, 89);
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new System.Drawing.Size(40, 20);
			this.textBox15.TabIndex = 19;
			// 
			// textBox16
			// 
			this.textBox16.Enabled = false;
			this.textBox16.Location = new System.Drawing.Point(263, 89);
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new System.Drawing.Size(40, 20);
			this.textBox16.TabIndex = 18;
			// 
			// textBox17
			// 
			this.textBox17.Location = new System.Drawing.Point(309, 89);
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new System.Drawing.Size(40, 20);
			this.textBox17.TabIndex = 17;
			// 
			// textBox18
			// 
			this.textBox18.Location = new System.Drawing.Point(355, 89);
			this.textBox18.Name = "textBox18";
			this.textBox18.Size = new System.Drawing.Size(40, 20);
			this.textBox18.TabIndex = 16;
			// 
			// textBox19
			// 
			this.textBox19.Location = new System.Drawing.Point(401, 89);
			this.textBox19.Name = "textBox19";
			this.textBox19.Size = new System.Drawing.Size(40, 20);
			this.textBox19.TabIndex = 15;
			// 
			// textBox20
			// 
			this.textBox20.Location = new System.Drawing.Point(447, 89);
			this.textBox20.Name = "textBox20";
			this.textBox20.Size = new System.Drawing.Size(40, 20);
			this.textBox20.TabIndex = 14;
			// 
			// textBox21
			// 
			this.textBox21.Location = new System.Drawing.Point(33, 145);
			this.textBox21.Name = "textBox21";
			this.textBox21.Size = new System.Drawing.Size(40, 20);
			this.textBox21.TabIndex = 13;
			// 
			// textBox22
			// 
			this.textBox22.Location = new System.Drawing.Point(79, 145);
			this.textBox22.Name = "textBox22";
			this.textBox22.Size = new System.Drawing.Size(40, 20);
			this.textBox22.TabIndex = 12;
			// 
			// textBox11
			// 
			this.textBox11.Location = new System.Drawing.Point(33, 89);
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new System.Drawing.Size(40, 20);
			this.textBox11.TabIndex = 11;
			// 
			// textBox10
			// 
			this.textBox10.Location = new System.Drawing.Point(447, 31);
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new System.Drawing.Size(40, 20);
			this.textBox10.TabIndex = 10;
			// 
			// textBox09
			// 
			this.textBox09.Location = new System.Drawing.Point(401, 31);
			this.textBox09.Name = "textBox09";
			this.textBox09.Size = new System.Drawing.Size(40, 20);
			this.textBox09.TabIndex = 9;
			// 
			// textBox08
			// 
			this.textBox08.Location = new System.Drawing.Point(355, 31);
			this.textBox08.Name = "textBox08";
			this.textBox08.Size = new System.Drawing.Size(40, 20);
			this.textBox08.TabIndex = 8;
			// 
			// textBox07
			// 
			this.textBox07.Location = new System.Drawing.Point(309, 31);
			this.textBox07.Name = "textBox07";
			this.textBox07.Size = new System.Drawing.Size(40, 20);
			this.textBox07.TabIndex = 7;
			// 
			// textBox06
			// 
			this.textBox06.Location = new System.Drawing.Point(263, 31);
			this.textBox06.Name = "textBox06";
			this.textBox06.Size = new System.Drawing.Size(40, 20);
			this.textBox06.TabIndex = 6;
			// 
			// textBox05
			// 
			this.textBox05.Location = new System.Drawing.Point(217, 31);
			this.textBox05.Name = "textBox05";
			this.textBox05.Size = new System.Drawing.Size(40, 20);
			this.textBox05.TabIndex = 5;
			// 
			// textBox04
			// 
			this.textBox04.Location = new System.Drawing.Point(171, 31);
			this.textBox04.Name = "textBox04";
			this.textBox04.Size = new System.Drawing.Size(40, 20);
			this.textBox04.TabIndex = 4;
			// 
			// textBox03
			// 
			this.textBox03.Location = new System.Drawing.Point(125, 31);
			this.textBox03.Name = "textBox03";
			this.textBox03.Size = new System.Drawing.Size(40, 20);
			this.textBox03.TabIndex = 3;
			// 
			// textBox02
			// 
			this.textBox02.Location = new System.Drawing.Point(79, 31);
			this.textBox02.Name = "textBox02";
			this.textBox02.Size = new System.Drawing.Size(40, 20);
			this.textBox02.TabIndex = 2;
			// 
			// textBox01
			// 
			this.textBox01.Location = new System.Drawing.Point(33, 31);
			this.textBox01.Name = "textBox01";
			this.textBox01.Size = new System.Drawing.Size(40, 20);
			this.textBox01.TabIndex = 1;
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
			this.logInformationTabPage.Controls.Add(this.preloadBufferVolumeTextBox);
			this.logInformationTabPage.Controls.Add(this.preloadBufferVolumeLabel);
			this.logInformationTabPage.Controls.Add(this.sampleTextBox);
			this.logInformationTabPage.Controls.Add(this.MnpsNameLabel);
			this.logInformationTabPage.Controls.Add(this.bufferNameLabel);
			this.logInformationTabPage.Controls.Add(this.reactionWellTextBox);
			this.logInformationTabPage.Controls.Add(this.revertInitFileValuesButton);
			this.logInformationTabPage.Location = new System.Drawing.Point(4, 22);
			this.logInformationTabPage.Name = "logInformationTabPage";
			this.logInformationTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.logInformationTabPage.Size = new System.Drawing.Size(524, 227);
			this.logInformationTabPage.TabIndex = 1;
			this.logInformationTabPage.Text = "Log Information";
			this.logInformationTabPage.UseVisualStyleBackColor = true;
			// 
			// sampleTextBox
			// 
			this.sampleTextBox.Location = new System.Drawing.Point(3, 74);
			this.sampleTextBox.Name = "sampleTextBox";
			this.sampleTextBox.Size = new System.Drawing.Size(100, 20);
			this.sampleTextBox.TabIndex = 5;
			// 
			// MnpsNameLabel
			// 
			this.MnpsNameLabel.AutoSize = true;
			this.MnpsNameLabel.Location = new System.Drawing.Point(7, 58);
			this.MnpsNameLabel.Name = "MnpsNameLabel";
			this.MnpsNameLabel.Size = new System.Drawing.Size(67, 13);
			this.MnpsNameLabel.TabIndex = 4;
			this.MnpsNameLabel.Text = "MNPs Name";
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
			// reactionWellTextBox
			// 
			this.reactionWellTextBox.Location = new System.Drawing.Point(3, 35);
			this.reactionWellTextBox.Name = "reactionWellTextBox";
			this.reactionWellTextBox.Size = new System.Drawing.Size(100, 20);
			this.reactionWellTextBox.TabIndex = 2;
			// 
			// revertInitFileValuesButton
			// 
			this.revertInitFileValuesButton.Location = new System.Drawing.Point(6, 198);
			this.revertInitFileValuesButton.Name = "revertInitFileValuesButton";
			this.revertInitFileValuesButton.Size = new System.Drawing.Size(75, 23);
			this.revertInitFileValuesButton.TabIndex = 1;
			this.revertInitFileValuesButton.Text = "Revert";
			this.revertInitFileValuesButton.UseVisualStyleBackColor = true;
			this.revertInitFileValuesButton.Click += new System.EventHandler(this.revertInitFileValuesButton_Click);
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
			// preloadBufferVolumeTextBox
			// 
			this.preloadBufferVolumeTextBox.Location = new System.Drawing.Point(3, 113);
			this.preloadBufferVolumeTextBox.Name = "preloadBufferVolumeTextBox";
			this.preloadBufferVolumeTextBox.Size = new System.Drawing.Size(100, 20);
			this.preloadBufferVolumeTextBox.TabIndex = 7;
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
			this.pinMultiplexerValuesTabPage.ResumeLayout(false);
			this.pinMultiplexerValuesTabPage.PerformLayout();
			this.logInformationTabPage.ResumeLayout(false);
			this.logInformationTabPage.PerformLayout();
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
		private Button revertInitFileValuesButton;
		private TextBox sampleTextBox;
		private Label MnpsNameLabel;
		private Label bufferNameLabel;
		private TextBox reactionWellTextBox;
		private Button revertPinAssignmentsButton;
		private TabPage generalTabPage;
		private Button revertGeneralButton;
		private Label tempFoldersToKeepLabel;
		private TextBox tempFoldersToKeepTextBox;
		private TextBox preloadBufferVolumeTextBox;
		private Label preloadBufferVolumeLabel;
	}
}