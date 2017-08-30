/*
 * User: JinGen
 * Date: 11/12/2013
 * Time: 11:45 AM
 * 
 */
using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Generic;

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
            /*
             * 
             * Parameters required to start in console mode
             * -h:(data in hex, no affix FF00FF00)
             * -f:(frequency in mhz 433.9)
             * -b:(baud rate e.g. 3333)
             * -p:(serial port name e.g. COM3)
             * 
             * Example of working params: -h:00FF00AA00FF -f:433.92 -b:3333 -p:COM23
             * 
             * /

            /*
             * 
             *  Optional parameter (for console or windowed)
             * -bb:(bridge baud rate, default 9600)
             * 
             */

            bool canStartInConsoleMode = true;

            string hexData = GetParameterValue(args, "h");
            string frequencyMhz = GetParameterValue(args, "f");
            string baudRate = GetParameterValue(args, "b");
            string serialPortName = GetParameterValue(args, "p");

            string bridgeBaudRate = GetParameterValue(args, "bb");

            int bridgeBaudRateInt;
            double baudRateDouble;
            double frequencyMhzDouble;
            List<byte> hexDataBytes = new List<byte>();
            
            // validate if parameters are sane

            if (int.TryParse(bridgeBaudRate, out bridgeBaudRateInt))
            {
                CCRegister.BRIDGE_BAUD_RATE = bridgeBaudRateInt;
                Log.Info("Set bridge baud rate to " + CCRegister.BRIDGE_BAUD_RATE.ToString());
            }

            if (!double.TryParse(baudRate, out baudRateDouble))
            {
                canStartInConsoleMode = false;
            }
            if (!double.TryParse(frequencyMhz, out frequencyMhzDouble))
            {
                canStartInConsoleMode = false;
            }

            if (SerialPort.GetPortNames().Length == 0)
            {
                canStartInConsoleMode = false;
            }

            foreach (string availablePort in SerialPort.GetPortNames())
            {
                if (availablePort.ToUpper() == serialPortName.ToUpper())
                {
                    SerialPort sp = new SerialPort(serialPortName);
                    if (sp.IsOpen)
                    {
                        Log.Error("Specified serial port is in use!");
                        Environment.Exit(0);
                    }
                    else
                    {
                        break;
                    }
                    sp.Dispose();
                }
                canStartInConsoleMode = false;
            }

            if (hexData.Length % 2 != 0)
            {
                canStartInConsoleMode = false;
            }
            else
            {
                try
                {
                    for (int i = 0; i < hexData.Length; i += 2)
                    {
                        hexDataBytes.Add(byte.Parse(hexData[i].ToString() + hexData[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber));
                    }
                }

                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                    Environment.Exit(0);
                }
            }

            // start in console mode if possible

            if (canStartInConsoleMode)
            {
                Log.Info("Starting CCManager in Console mode");

                try
                {
                    RF rf = new RF(new SerialPort(serialPortName, CCRegister.BRIDGE_BAUD_RATE));

                    rf.Reset();
                    rf.SetupPATABLE();
                    rf.SetCarrierFrequency(frequencyMhzDouble);
                    rf.SetBaudRate(baudRateDouble);
                    rf.SetupRegisters();
                    rf.EnterIdleState();
                    rf.ShortWait();

                    rf.Transmit(hexDataBytes.ToArray());
                    rf.CloseSerial();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }

            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }

		}

        static string GetParameterValue(string[] args, string parameterKey)
        {
            string parameterKeyInArgs = "-" + parameterKey + ":";
            foreach (string arg in args)
            {   
                if (arg.Remove(parameterKeyInArgs.Length).ToLower() == parameterKeyInArgs)
                {
                    return arg.Remove(0, parameterKeyInArgs.Length);
                }
            }
            return "";
        }
        
	}
}
