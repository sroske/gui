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
		private string bufferName = "PBS";
		private string mnpsName = "MACS";
		private int preloadBufferVolume = 10;
		private string preloadBufferVolumeUnit = "uL";
		private int defaultAddBufferVolume = 5;
		private int defaultAddMnpsVolume = 20;
		private string defaultBufferMnpsVolumeUnit = "uL";

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
			this.bufferName = readBufferName();
			this.mnpsName = readMnpsName();
			this.preloadBufferVolume = readPreloadBufferVolume();
			this.preloadBufferVolumeUnit = readPreloadBufferVolumeUnit();
			this.defaultAddBufferVolume = readDefaultAddBufferVolume();
			this.defaultAddMnpsVolume = readDefaultAddMnpsVolume();
			this.defaultBufferMnpsVolumeUnit = readDefaultBufferMnpsVolumeUnit();
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
			file.WriteLine("BufferName:" + this.bufferName);
			file.WriteLine("MnpsName:" + this.mnpsName);
			file.WriteLine("PreloadBufferVolume:" + this.preloadBufferVolume);
			file.WriteLine("PreloadBufferVolumeUnit:" + this.preloadBufferVolumeUnit);
			file.WriteLine("DefaultAddBufferVolume:" + this.defaultAddBufferVolume);
			file.WriteLine("DefaultAddMnpsVolume:" + this.defaultAddMnpsVolume);
			file.WriteLine("DefaultBufferMnpsVolumeUnit:" + this.defaultBufferMnpsVolumeUnit);
			file.Flush();
			file.Close();
			file.Dispose();
		}


		public void saveConfigurations()
		{
			FileStream configFile;
			configFile = File.Create("./config.ini");
			configFile.Close();
			configFile.Dispose();
			writeConfigFileValues();
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

		public string readBufferName()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("BufferName:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 11, end - start - 12);
			return line;
		}

		public string readMnpsName()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("MnpsName:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 9, end - start - 10);
			return line;
		}

		public int readPreloadBufferVolume()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("PreloadBufferVolume:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 20, end - start - 21);
			return System.Convert.ToInt32(line);
		}

		public string readPreloadBufferVolumeUnit()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("PreloadBufferVolumeUnit:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 24, end - start - 25);
			return line;
		}

		public int readDefaultAddBufferVolume()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("DefaultAddBufferVolume:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 23, end - start - 24);
			return System.Convert.ToInt32(line);
		}

		public int readDefaultAddMnpsVolume()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("DefaultAddMnpsVolume:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 21, end - start - 22);
			return System.Convert.ToInt32(line);
		}

		public string readDefaultBufferMnpsVolumeUnit()
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			int start = line.IndexOf("DefaultBufferMnpsVolumeUnit:", 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + 28, end - start - 29);
			return line;
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

		public void setBufferName(string name)
		{
			this.bufferName = name;
			saveConfigurations();
		}

		public void setMnpsName(string name)
		{
			this.mnpsName = name;
			saveConfigurations();
		}

		public void setPreloadBufferVolume(int volume)
		{
			this.preloadBufferVolume = volume;
			saveConfigurations();
		}

		public void setPreloadBufferVolumeUnit(string unit)
		{
			this.preloadBufferVolumeUnit = unit;
			saveConfigurations();
		}

		public void setDefaultAddBufferVolume(int volume)
		{
			this.defaultAddBufferVolume = volume;
			saveConfigurations();
		}

		public void setDefaultAddMnpsVolume(int volume)
		{
			this.defaultAddMnpsVolume = volume;
			saveConfigurations();
		}

		public void setDefaultBufferMnpsVolumeUnit(string unit)
		{
			this.defaultBufferMnpsVolumeUnit = unit;
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
	}
}
