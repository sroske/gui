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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
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
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(149, 271);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(230, 271);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(button2_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(427, 253);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.textBox30);
			this.tabPage1.Controls.Add(this.textBox29);
			this.tabPage1.Controls.Add(this.textBox28);
			this.tabPage1.Controls.Add(this.textBox27);
			this.tabPage1.Controls.Add(this.textBox26);
			this.tabPage1.Controls.Add(this.textBox25);
			this.tabPage1.Controls.Add(this.textBox24);
			this.tabPage1.Controls.Add(this.textBox23);
			this.tabPage1.Controls.Add(this.textBox12);
			this.tabPage1.Controls.Add(this.textBox13);
			this.tabPage1.Controls.Add(this.textBox14);
			this.tabPage1.Controls.Add(this.textBox15);
			this.tabPage1.Controls.Add(this.textBox16);
			this.tabPage1.Controls.Add(this.textBox17);
			this.tabPage1.Controls.Add(this.textBox18);
			this.tabPage1.Controls.Add(this.textBox19);
			this.tabPage1.Controls.Add(this.textBox20);
			this.tabPage1.Controls.Add(this.textBox21);
			this.tabPage1.Controls.Add(this.textBox22);
			this.tabPage1.Controls.Add(this.textBox11);
			this.tabPage1.Controls.Add(this.textBox10);
			this.tabPage1.Controls.Add(this.textBox09);
			this.tabPage1.Controls.Add(this.textBox08);
			this.tabPage1.Controls.Add(this.textBox07);
			this.tabPage1.Controls.Add(this.textBox06);
			this.tabPage1.Controls.Add(this.textBox05);
			this.tabPage1.Controls.Add(this.textBox04);
			this.tabPage1.Controls.Add(this.textBox03);
			this.tabPage1.Controls.Add(this.textBox02);
			this.tabPage1.Controls.Add(this.textBox01);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(419, 227);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Pin Multiplexer Values";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// textBox30
			// 
			this.textBox30.Location = new System.Drawing.Point(330, 145);
			this.textBox30.Name = "textBox30";
			this.textBox30.Size = new System.Drawing.Size(27, 20);
			this.textBox30.TabIndex = 30;
			// 
			// textBox29
			// 
			this.textBox29.Location = new System.Drawing.Point(297, 145);
			this.textBox29.Name = "textBox29";
			this.textBox29.Size = new System.Drawing.Size(27, 20);
			this.textBox29.TabIndex = 29;
			// 
			// textBox28
			// 
			this.textBox28.Location = new System.Drawing.Point(264, 145);
			this.textBox28.Name = "textBox28";
			this.textBox28.Size = new System.Drawing.Size(27, 20);
			this.textBox28.TabIndex = 28;
			// 
			// textBox27
			// 
			this.textBox27.Location = new System.Drawing.Point(231, 145);
			this.textBox27.Name = "textBox27";
			this.textBox27.Size = new System.Drawing.Size(27, 20);
			this.textBox27.TabIndex = 27;
			// 
			// textBox26
			// 
			this.textBox26.Location = new System.Drawing.Point(198, 145);
			this.textBox26.Name = "textBox26";
			this.textBox26.Size = new System.Drawing.Size(27, 20);
			this.textBox26.TabIndex = 26;
			// 
			// textBox25
			// 
			this.textBox25.Location = new System.Drawing.Point(165, 145);
			this.textBox25.Name = "textBox25";
			this.textBox25.Size = new System.Drawing.Size(27, 20);
			this.textBox25.TabIndex = 25;
			// 
			// textBox24
			// 
			this.textBox24.Location = new System.Drawing.Point(132, 145);
			this.textBox24.Name = "textBox24";
			this.textBox24.Size = new System.Drawing.Size(27, 20);
			this.textBox24.TabIndex = 24;
			// 
			// textBox23
			// 
			this.textBox23.Location = new System.Drawing.Point(99, 145);
			this.textBox23.Name = "textBox23";
			this.textBox23.Size = new System.Drawing.Size(27, 20);
			this.textBox23.TabIndex = 23;
			// 
			// textBox12
			// 
			this.textBox12.Location = new System.Drawing.Point(66, 89);
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new System.Drawing.Size(27, 20);
			this.textBox12.TabIndex = 22;
			// 
			// textBox13
			// 
			this.textBox13.Location = new System.Drawing.Point(99, 89);
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new System.Drawing.Size(27, 20);
			this.textBox13.TabIndex = 21;
			// 
			// textBox14
			// 
			this.textBox14.Location = new System.Drawing.Point(132, 89);
			this.textBox14.Name = "textBox14";
			this.textBox14.Size = new System.Drawing.Size(27, 20);
			this.textBox14.TabIndex = 20;
			// 
			// textBox15
			// 
			this.textBox15.Location = new System.Drawing.Point(165, 89);
			this.textBox15.Name = "textBox15";
			this.textBox15.Size = new System.Drawing.Size(27, 20);
			this.textBox15.TabIndex = 19;
			// 
			// textBox16
			// 
			this.textBox16.Location = new System.Drawing.Point(198, 89);
			this.textBox16.Name = "textBox16";
			this.textBox16.Size = new System.Drawing.Size(27, 20);
			this.textBox16.TabIndex = 18;
			// 
			// textBox17
			// 
			this.textBox17.Location = new System.Drawing.Point(231, 89);
			this.textBox17.Name = "textBox17";
			this.textBox17.Size = new System.Drawing.Size(27, 20);
			this.textBox17.TabIndex = 17;
			// 
			// textBox18
			// 
			this.textBox18.Location = new System.Drawing.Point(264, 89);
			this.textBox18.Name = "textBox18";
			this.textBox18.Size = new System.Drawing.Size(27, 20);
			this.textBox18.TabIndex = 16;
			// 
			// textBox19
			// 
			this.textBox19.Location = new System.Drawing.Point(297, 89);
			this.textBox19.Name = "textBox19";
			this.textBox19.Size = new System.Drawing.Size(27, 20);
			this.textBox19.TabIndex = 15;
			// 
			// textBox20
			// 
			this.textBox20.Location = new System.Drawing.Point(330, 89);
			this.textBox20.Name = "textBox20";
			this.textBox20.Size = new System.Drawing.Size(27, 20);
			this.textBox20.TabIndex = 14;
			// 
			// textBox21
			// 
			this.textBox21.Location = new System.Drawing.Point(33, 145);
			this.textBox21.Name = "textBox21";
			this.textBox21.Size = new System.Drawing.Size(27, 20);
			this.textBox21.TabIndex = 13;
			// 
			// textBox22
			// 
			this.textBox22.Location = new System.Drawing.Point(66, 145);
			this.textBox22.Name = "textBox22";
			this.textBox22.Size = new System.Drawing.Size(27, 20);
			this.textBox22.TabIndex = 12;
			// 
			// textBox11
			// 
			this.textBox11.Location = new System.Drawing.Point(33, 89);
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new System.Drawing.Size(27, 20);
			this.textBox11.TabIndex = 11;
			// 
			// textBox10
			// 
			this.textBox10.Location = new System.Drawing.Point(330, 31);
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new System.Drawing.Size(27, 20);
			this.textBox10.TabIndex = 10;
			// 
			// textBox09
			// 
			this.textBox09.Location = new System.Drawing.Point(297, 31);
			this.textBox09.Name = "textBox09";
			this.textBox09.Size = new System.Drawing.Size(27, 20);
			this.textBox09.TabIndex = 9;
			// 
			// textBox08
			// 
			this.textBox08.Location = new System.Drawing.Point(264, 31);
			this.textBox08.Name = "textBox08";
			this.textBox08.Size = new System.Drawing.Size(27, 20);
			this.textBox08.TabIndex = 8;
			// 
			// textBox07
			// 
			this.textBox07.Location = new System.Drawing.Point(231, 31);
			this.textBox07.Name = "textBox07";
			this.textBox07.Size = new System.Drawing.Size(27, 20);
			this.textBox07.TabIndex = 7;
			// 
			// textBox06
			// 
			this.textBox06.Location = new System.Drawing.Point(198, 31);
			this.textBox06.Name = "textBox06";
			this.textBox06.Size = new System.Drawing.Size(27, 20);
			this.textBox06.TabIndex = 6;
			// 
			// textBox05
			// 
			this.textBox05.Location = new System.Drawing.Point(165, 31);
			this.textBox05.Name = "textBox05";
			this.textBox05.Size = new System.Drawing.Size(27, 20);
			this.textBox05.TabIndex = 5;
			// 
			// textBox04
			// 
			this.textBox04.Location = new System.Drawing.Point(132, 31);
			this.textBox04.Name = "textBox04";
			this.textBox04.Size = new System.Drawing.Size(27, 20);
			this.textBox04.TabIndex = 4;
			// 
			// textBox03
			// 
			this.textBox03.Location = new System.Drawing.Point(99, 31);
			this.textBox03.Name = "textBox03";
			this.textBox03.Size = new System.Drawing.Size(27, 20);
			this.textBox03.TabIndex = 3;
			// 
			// textBox02
			// 
			this.textBox02.Location = new System.Drawing.Point(66, 31);
			this.textBox02.Name = "textBox02";
			this.textBox02.Size = new System.Drawing.Size(27, 20);
			this.textBox02.TabIndex = 2;
			// 
			// textBox01
			// 
			this.textBox01.Location = new System.Drawing.Point(33, 31);
			this.textBox01.Name = "textBox01";
			this.textBox01.Size = new System.Drawing.Size(27, 20);
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
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(419, 227);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(452, 306);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Preferences";
			this.Text = "Preferences";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
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
		private System.Windows.Forms.TabPage tabPage2;
	}
}