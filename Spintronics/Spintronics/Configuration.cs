﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpintronicsGUI
{
	public class Configuration
	{
		public string defaultSaveDirectory = Directory.GetCurrentDirectory();
		public int tempFoldersToKeep = -1;
		public int[] sensorMultiplexerValues = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 17, 18, 19, 20, 21, 22, 23,
									24, 25, 26, 27, 28, 29, 30};
		public string bufferName = "PBS";
		public string mnpsName = "MACS";
		public int preloadBufferVolume = 10;
		public int defaultAddBufferVolume = 5;
		public int defaultAddMnpsVolume = 20;
		public string defaultVolumeUnit = "uL";
		public float wheatstoneAmplitude = 300;
		public string wheatstoneAmplitudeUnit = "mV";
		public float wheatstoneFrequency = 1000;
		public float coilAmplitude = 200;
		public string coilAmplitudeUnit = "mV";
		public float coilFrequency = 50;
		public float coilDcOffset = 0;
		public string coilDcOffsetUnit = "V";
		public float measurementPeriod = 1;

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

			try {
				this.defaultSaveDirectory = readStringConfiguration("DefaultSaveDirectory");
				this.sensorMultiplexerValues = readSensorMultiplexerValues();
				this.bufferName = readStringConfiguration("BufferName");
				this.mnpsName = readStringConfiguration("MnpsName");
				this.preloadBufferVolume = readIntConfiguration("PreloadBufferVolume");
				this.defaultAddBufferVolume = readIntConfiguration("DefaultAddBufferVolume");
				this.defaultAddMnpsVolume = readIntConfiguration("DefaultAddMnpsVolume");
				this.defaultVolumeUnit = readStringConfiguration("DefaultVolumeUnit");
				this.wheatstoneAmplitude = readFloatConfiguration("WheatstoneAmplitude");
				this.wheatstoneAmplitudeUnit = readStringConfiguration("WheatstoneAmplitudeUnit");
				this.wheatstoneFrequency = readFloatConfiguration("WheatstoneFrequency");
				this.coilAmplitude = readFloatConfiguration("CoilAmplitude");
				this.coilAmplitudeUnit = readStringConfiguration("CoilAmplitudeUnit");
				this.coilFrequency = readFloatConfiguration("CoilFrequency");
				this.coilDcOffset = readFloatConfiguration("CoilDcOffset");
				this.coilDcOffsetUnit = readStringConfiguration("CoilDcOffsetUnit");
				this.measurementPeriod = readFloatConfiguration("MeasurementPeriod");
			} catch (IOException) {

			} catch (UnauthorizedAccessException) {

			}
		}

		private void writeConfigFileValues()
		{
			try {
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
				file.WriteLine("DefaultAddBufferVolume:" + this.defaultAddBufferVolume);
				file.WriteLine("DefaultAddMnpsVolume:" + this.defaultAddMnpsVolume);
				file.WriteLine("DefaultVolumeUnit:" + this.defaultVolumeUnit);

				file.WriteLine("WheatstoneAmplitude:" + this.wheatstoneAmplitude);
				file.WriteLine("WheatstoneAmplitudeUnit:" + this.wheatstoneAmplitudeUnit);
				file.WriteLine("WheatstoneFrequency:" + this.wheatstoneFrequency);
				file.WriteLine("CoilAmplitude:" + this.coilAmplitude);
				file.WriteLine("CoilAmplitudeUnit:" + this.coilAmplitudeUnit);
				file.WriteLine("CoilFrequency:" + this.coilFrequency);
				file.WriteLine("CoilDcOffset:" + this.coilDcOffset);
				file.WriteLine("CoilDcOffsetUnit:" + this.coilDcOffsetUnit);
				file.WriteLine("MeasurementPeriod:" + this.measurementPeriod);
				file.Flush();
				file.Close();
				file.Dispose();
			} catch (UnauthorizedAccessException) {
				
			} catch (IOException) {
				
			}
		}


		public void saveConfigurations()
		{
			try {
				FileStream configFile;
				configFile = File.Create("./config.ini");
				configFile.Close();
				configFile.Dispose();
				writeConfigFileValues();
			} catch (UnauthorizedAccessException) {
				
			} catch (IOException) {
				
			}
		}


		public string readStringConfiguration(string label)
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			label += ":";
			int start = line.IndexOf(label, 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + label.Length, end - start - label.Length - 1);
			return line;
		}

		public int readIntConfiguration(string label)
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			label += ":";
			int start = line.IndexOf(label, 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + label.Length, end - start - label.Length - 1);
			return System.Convert.ToInt32(line);
		}

		public float readFloatConfiguration(string label)
		{
			StreamReader file = new StreamReader("./config.ini");
			string line = file.ReadToEnd();
			file.Close();
			file.Dispose();
			label += ":";
			int start = line.IndexOf(label, 0);
			int end = line.IndexOf("\n", start);
			line = line.Substring(start + label.Length, end - start - label.Length - 1);
			return float.Parse(line);
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

		public void setDefaultVolumeUnit(string unit)
		{
			this.defaultVolumeUnit = unit;
			saveConfigurations();
		}

		public void setWheatstoneAmplitude(float amplitude)
		{
			this.wheatstoneAmplitude = amplitude;
			saveConfigurations();
		}

		public void setWheatstoneAmplitudeUnit(string unit)
		{
			this.wheatstoneAmplitudeUnit = unit;
			saveConfigurations();
		}

		public void setWheatstoneFrequency(float frequency)
		{
			this.wheatstoneFrequency = frequency;
			saveConfigurations();
		}

		public void setCoilAmplitude(float amplitude)
		{
			this.coilAmplitude = amplitude;
			saveConfigurations();
		}

		public void setCoilAmplitudeUnit(string unit)
		{
			this.coilAmplitudeUnit = unit;
			saveConfigurations();
		}

		public void setCoilFrequncy(float frequency)
		{
			this.coilFrequency = frequency;
			saveConfigurations();
		}

		public void setCoilDcOffset(float offset)
		{
			this.coilDcOffset = offset;
			saveConfigurations();
		}

		public void setCoilDcOffsetUnit(string unit)
		{
			this.coilDcOffsetUnit = unit;
			saveConfigurations();
		}

		public void setMeasurementPeriod(float period)
		{
			this.measurementPeriod = period;
			saveConfigurations();
		}
	}
}
