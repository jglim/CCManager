/*
 * User: JinGen
 * Date: 11/12/2013
 * Time: 5:35 PM
 * 
 */
using System;
using System.IO.Ports;

namespace CCManager
{
	/// <summary>
	/// Methods to handle low-level communication between the computer and the MCU
	/// </summary>
	public static class Serial
	{
		/*
		 * Command bytes:
		 * a - Reset RF
		 * b - delay, delayTime (ms)
		 * c - select chip (SS)
		 * d - deselect chip (SS)
		 * e - wait MISO low
		 * f - SPI transfer, transferByte
		 */
		
		// Reset the CC1101 chip
		public static bool ResetRf(SerialPort sp)
		{
			sp.Write("{a00}");
			string result = sp.ReadLine();
			if (result == "a1")
			{
				return true;	
			}
			return false;
		}
		
		// Delays the MCU for a time between 0-255 ms
		public static bool Delay(SerialPort sp, byte delayTime)
		{
			sp.Write("{b" + StringifyByteWithPadding(delayTime) + "}");
			string result = sp.ReadLine();
			if (result == "b1")
			{
				return true;	
			}
			return false;
		}
		
		// Select the CC1101 chip's SPI
		public static bool SelectRf(SerialPort sp)
		{
			sp.Write("{c00}");
			string result = sp.ReadLine();
			if (result == "c1")
			{
				return true;	
			}
			return false;
		}

		// Deselect the CC1101 chip's SPI
		public static bool DeselectRf(SerialPort sp)
		{
			sp.Write("{d00}");
			string result = sp.ReadLine();
			if (result == "d1")
			{
				return true;	
			}
			return false;
		}
		
		// Wait for SPI MISO pin to go LOW
		public static bool WaitMISO(SerialPort sp)
		{
			sp.Write("{e00}");
			string result = sp.ReadLine();
			if (result == "e1")
			{
				return true;	
			}
			return false;
		}
		
		// Transfers a byte via SPI to CC1101
		public static bool SPITransfer(SerialPort sp, byte dataToTransfer)
		{
			Log.Info(string.Format("Transfer: 0x{0}", StringifyByteWithPadding(dataToTransfer)), 4);
			sp.Write("{f" + StringifyByteWithPadding(dataToTransfer) + "}");
			string result = sp.ReadLine();
			Log.Info(string.Format("Transfer Result: {0}", result ), 3);
			if (result == "f1")
			{
				return true;	
			}
			return false;
		}
		
		// Gets the uart->spi bridge's signature (MSP430 etc)
		public static string GetBridgeSignature(SerialPort sp)
		{
			//sp.Write("{g00}");
			sp.Write("{");
			sp.Write("g");
			sp.Write("0");
			sp.Write("0");
			sp.Write("}");
			return sp.ReadLine();
		}
		
		// Displays a byte in hex
		// Pads 0s if needed after a dec->hex conversion
		private static string StringifyByteWithPadding(byte inputByte)
		{
			string output = inputByte.ToString("X");
			if (output.Length == 1)
			{
				return "0" + output;
			}
			return output;
		}
	}
}
