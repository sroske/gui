using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpintronicsGUI
{
	public partial class InputTextForm : Form
	{
		private string input;

		public InputTextForm()
		{
			InitializeComponent();
		}

		public string GetInput()
		{
			return input;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			if (this.textBox1.Text == "")
			{
				this.DialogResult = DialogResult.Cancel;
			}
			else
			{
				input = this.textBox1.Text;
				this.DialogResult = DialogResult.OK;
			}
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
