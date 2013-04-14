using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpintronicsGUI
{
	public class Configuration
	{
		private string defaultSaveDirectory = Directory.GetCurrentDirectory();
		private int tempFoldersToKeep = -1;
		private int[] sensorMultiplexerValues = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 17, 18, 19, 20, 21, 22, 23,
									24, 25, 26, 27, 28, 29, 30};
		private string reactionWell = "0000A";
		private string sample = "0ng/mL";

		public Configuration()
		{
			if(!File.Exists("./config.ini"))
			{
				FileStream configFile;
				configFile = File.Create("./config.ini");
				configFile.Close();
				configFile.Dispose();
				writeConfigFileValues();
			}

			this.defaultSaveDirectory = readDefaultSaveDirectory();
			this.sensorMultiplexerValues = readSensorMultiplexerValues();
			this.reactionWell = readReactionWell();
			this.sample = readSample();
		}

		public Configuration(string startSaveDirectory, int startFoldersToKeep, int[] startSensorMultiplexerValues, string startReactionWell, string startSample)
		{
			if (!File.Exists("./config.ini"))
			{
				FileStream configFile;
				configFile = File.Create("./config.ini");
				configFile.Close();
				configFile.Dispose();
				this.defaultSaveDirectory = startSaveDirectory;
				this.tempFoldersToKeep = startFoldersToKeep;
				this.sensorMultiplexerValues = startSensorMultiplexerValues;
				this.reactionWell = startReactionWell;
				this.sample = startSample;
				writeConfigFileValues();
			}

			this.defaultSaveDirectory = readDefaultSaveDirectory();
			this.sensorMultiplexerValues = readSensorMultiplexerValues();
			this.reactionWell = readReactionWell();
			this.sample = readSample();
		}

		private void writeConfigFileValues()
		{
			StreamWriter file = new StreamWriter("./config.ini");
			file.WriteLine("DefaultSaveDirectory:" + this.defaultSaveDirectory);
			file.WriteLine("TempFoldersToKeep:" + this.tempFoldersToKeep);
			file.Write("SensorMultiplexerValues:");
			for (int i = 0; i < sensorMultiplexerValues.Length; i++)
			{
				file.Write("-" + sensorMultiplexerValues[i]);
			}
			file.Write("-\n");
			file.WriteLine("ReactionWell:" + this.reactionWell);
			file.WriteLine("Sample:" + this.sample);
			file.Flush();
			file.Close();
			file.Dispose();
		}

		public string readDefaultSaveDirectory()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("DefaultSaveDirectory:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 21, end - start - 22);
			return line;
		}

		public string readTempFoldersToKeep()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("TempFoldersToKeep:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 18, end - start - 19);
			return line;
		}

		public int[] readSensorMultiplexerValues()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("SensorMultiplexerValues:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 24, end - start - 24);
			int[] array = new int[sensorMultiplexerValues.Length];
			for (int i = 0; i < sensorMultiplexerValues.Length; i++)
			{
				int first = line.IndexOf("-", 0);
				int last = line.IndexOf("-", first + 1);
				array[i] = System.Convert.ToInt32(line.Substring(first + 1, last - first - 1));
				line = line.Remove(first, last - first);
			}
			return array;
		}

		public string readReactionWell()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("ReactionWell:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 13, end - start - 14);
			return line;
		}

		public string readSample()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("Sample:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 7, end - start - 8);
			return line;
		}

		public void saveConfigurations()
		{
			FileStream configFile;
			configFile = File.Create("./config.ini");
			configFile.Close();
			configFile.Dispose();
			writeConfigFileValues();
		}

		public void setDefaultSaveDirectory(string directory)
		{
			this.defaultSaveDirectory = directory;
			saveConfigurations();
		}

		public void setTempFoldersToKeep(int number)
		{
			this.tempFoldersToKeep = number;
			saveConfigurations();
		}

		public void setSensorMultiplexerValues(int[] array)
		{
			this.sensorMultiplexerValues = array;
			saveConfigurations();
		}

		public void setReactionWell(string newReactionWell)
		{
			this.reactionWell = newReactionWell;
			saveConfigurations();
		}

		public void setSample(string newSample)
		{
			this.sample = newSample;
			saveConfigurations();
		}

		public string getDefaultSaveDirectory()
		{
			return this.defaultSaveDirectory;
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
	}
}
