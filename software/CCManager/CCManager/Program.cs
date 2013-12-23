/*
 * User: JinGen
 * Date: 11/12/2013
 * Time: 11:45 AM
 * 
 */
using System;
using System.Windows.Forms;

namespace CCManager
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			foreach (string arg in args)
			{
				Log.Info(string.Format("Argument: {0}", arg));
				if (arg == "-b:115200")
				{
					CCRegister.BRIDGE_BAUD_RATE = 115200;
					Log.Info("Set high speed UART");
				}
				else if (arg == "-b:9600")
				{
					CCRegister.BRIDGE_BAUD_RATE = 9600;
					Log.Info("Set normal speed UART");
				}
				else if (arg == "-b:57600")
				{
					CCRegister.BRIDGE_BAUD_RATE = 57600;
					Log.Info("Set half high speed UART");
				}
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}
