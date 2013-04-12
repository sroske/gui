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
	public partial class Preferences : Form
	{
		int[] assignments;

		public Preferences(int[] initialAssignments)
		{
			InitializeComponent();
			populateFields(initialAssignments);
			assignments = initialAssignments;
		}

		private void populateFields(int[] initialAssignments)
		{
			foreach (TextBox t in this.tabPage1.Controls.OfType<TextBox>())
			{
				try {
					int element = System.Convert.ToInt32(t.Name.Substring(t.Name.Length - 2, 2));
					t.Text = System.Convert.ToString(initialAssignments[element - 1]);
				} catch (FormatException) {
					t.Text = "0";
				} catch (OverflowException) {
					t.Text = "0";
				} catch (IndexOutOfRangeException) {
					t.Text = "0";
				}
			}
		}

		public int[] getPreferences()
		{
			return assignments;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			foreach (TextBox t in this.tabPage1.Controls.OfType<TextBox>())
			{
				try {
					int value = System.Convert.ToInt32(t.Text);
					assignments[System.Convert.ToUInt32(t.Name.Substring(t.Name.Length - 2, 2)) - 1] = value;
				} catch (FormatException) {
					t.Text = "0";
				} catch (OverflowException) {
					t.Text = "0";
				}
			}

			if (this.assignments.Length == 30)
				this.DialogResult = DialogResult.OK;
			else
				this.DialogResult = DialogResult.Abort;

			this.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
