/*
 * User: JinGen
 * Date: 11/14/2013
 * Time: 6:13 PM
 */
using System;
using System.IO.Ports;

namespace CCManager
{
	/// <summary>
	/// Implementation of CC1101 SPI communication modes
	/// </summary>
	public class SPI
	{
		public static void WriteRegister(SerialPort sp, byte registerAddress, byte dataToWrite)
		{
			
			Serial.SelectRf(sp);
			Serial.WaitMISO(sp);
			Serial.SPITransfer(sp, registerAddress);
			Serial.SPITransfer(sp, dataToWrite);
			Serial.DeselectRf(sp);
		}

		public static void WriteBurstRegister(SerialPort sp, byte registerAddress, byte[] buffer)
		{
			Serial.SelectRf(sp);
			Serial.WaitMISO(sp);
			Serial.SPITransfer(sp, (byte)(registerAddress | CCRegister.WRITE_BURST));
			foreach (byte dataToTransfer in buffer)
			{
				Serial.SPITransfer(sp, dataToTransfer);
			}
		  	Serial.DeselectRf(sp);
		}

		public static void Strobe(SerialPort sp, byte dataToWrite)
		{
			Serial.SelectRf(sp);
			Serial.WaitMISO(sp);
			Serial.SPITransfer(sp, dataToWrite);
			Serial.DeselectRf(sp);
		}
	}
}
