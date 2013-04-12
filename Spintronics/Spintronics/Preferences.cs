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
		int[] assignments;
		string folder;

		public Preferences(int[] initialAssignments, string initialLogFolder)
		{
			InitializeComponent();
			populateAssignmentFields(initialAssignments);
			this.assignments = initialAssignments;
			refreshFolders();
			this.folder = initialLogFolder;
			this.currentDirectoryLabel.Text = initialLogFolder;
		}

		private void populateAssignmentFields(int[] initialAssignments)
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

		public int[] getAssignments()
		{
			return assignments;
		}

		public string[] getFolders()
		{
			return this.listBox1.Items.Cast<string>().ToArray();
		}

		public string getFolder()
		{
			return this.folder;
		}

		private void refreshFolders()
		{
			this.listBox1.Items.Clear();
			this.listBox1.Items.Add("data");
			this.folder = "data";
			string relativePath = Environment.CurrentDirectory;
			foreach (string d in Directory.GetDirectories(relativePath + "\\data", "*", SearchOption.TopDirectoryOnly))
			{
				int index = d.LastIndexOf("\\");
				string directory = "data/" + d.Substring(index + 1, d.Length - index - 1);
				this.listBox1.Items.Add(directory);
			}
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void saveAssignmentsButton_Click(object sender, EventArgs e)
		{
			foreach (TextBox t in this.tabPage1.Controls.OfType<TextBox>())
			{
				try {
					int value = System.Convert.ToInt32(t.Text);
					assignments[System.Convert.ToUInt32(t.Name.Substring(t.Name.Length - 2, 2)) - 1] = value;
				} catch (FormatException) {
					MessageBox.Show("Please enter a valid number for sensor " + t.Name);
				} catch (OverflowException) {
					MessageBox.Show("The value for sensor " + t.Name + " is too large.");
				}
			}
		}

		private void cancelPinAssignmentsButton_Click(object sender, EventArgs e)
		{
			foreach (TextBox t in this.tabPage1.Controls.OfType<TextBox>())
			{
				try {
					int value = assignments[System.Convert.ToUInt32(t.Name.Substring(t.Name.Length - 2, 2)) - 1];
					t.Text = System.Convert.ToString(value);
				} catch (FormatException) {
					MessageBox.Show("Please enter a valid number for sensor " + t.Name);
				} catch (OverflowException) {
					MessageBox.Show("The value for sensor " + t.Name + " is too large.");
				}
			}
		}

		private void refreshFoldersButton_Click(object sender, EventArgs e)
		{
			refreshFolders();
		}

		private void chooseFolderButton_Click(object sender, EventArgs e)
		{
			if (this.listBox1.SelectedItem == null)
			{
				MessageBox.Show("Please select a directory first");
				return;
			}
			this.folder = (string)this.listBox1.SelectedItem;
			this.currentDirectoryLabel.Text = this.folder;
		}

		private void addFolderButton_Click(object sender, EventArgs e)
		{
			InputTextForm inputText = new InputTextForm();
			inputText.ShowDialog();
			if (inputText.DialogResult == DialogResult.OK)
			{
				try {
					string directory = "./data/";// Environment.CurrentDirectory;
					directory += inputText.GetInput();
					Directory.CreateDirectory(directory);
					folder = inputText.GetInput();
					MessageBox.Show("Directory added");
				} catch (Exception) {
					MessageBox.Show("Failed to create directory");
				}
			}
			this.refreshFoldersButton.PerformClick();
		}

		private void deleteFolderButton_Click(object sender, EventArgs e)
		{
			if (this.listBox1.SelectedItem == null)
			{
				MessageBox.Show("Please select a directory first");
				return;
			}
			if ((string)this.listBox1.SelectedItem == "data")
			{
				MessageBox.Show("You cannot delete the root data directory");
				return;
			}
			if ((string)this.listBox1.SelectedItem == this.folder)
			{
				MessageBox.Show("You cannot delete the currently-chosen directory.\n" +
						    "Please select a different directory first.");
				return;
			}
			var result = MessageBox.Show("Are you sure you want to delete this folder?", "Delete", MessageBoxButtons.YesNoCancel);
			if(result == DialogResult.Yes)
			{
				try {
					string directory = "./";// Environment.CurrentDirectory;
					directory += /*"\\data\\" +*/ (string)this.listBox1.SelectedItem;
					Directory.Delete(directory, true);
					this.listBox1.Items.Remove(this.listBox1.SelectedItem);
				} catch (Exception) {
					MessageBox.Show("Failed to delete directory");
				}
			}
		}
	}
}
