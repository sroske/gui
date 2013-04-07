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

namespace SpintronicsGUI
{
	public partial class ComSelector : Form
	{
		private string selectedPort = null;

		public ComSelector()
		{
			InitializeComponent();
		}

		public string GetPort()
		{
			return selectedPort;
		}

		public void button1_Click(object sender, EventArgs args)
		{
			string[] ports = SerialPort.GetPortNames();
			this.listBox1.DataSource = ports;
		}

		public void button2_Click(object sender, EventArgs args)
		{
			selectedPort = (string)this.listBox1.SelectedItem;
			this.Close();
		}

		public void button3_Click(object sender, EventArgs args)
		{
			selectedPort = null;
			this.Close();
		}
	}
}
