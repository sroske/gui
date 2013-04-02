using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpintronicsGUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//try
			//{
				ComSelector selector = new ComSelector();
				Application.Run(selector);
				string comPort = selector.GetPort();
				if (comPort != null)
				{
					GUI gui = new GUI(comPort);
					Application.Run(gui);
				}
			//} catch(Exception)
			//{
			//}
		}
	}
}
