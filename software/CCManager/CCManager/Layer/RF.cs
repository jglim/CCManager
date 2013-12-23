/*
 * User: JinGen
 * Date: 11/14/2013
 * Time: 6:23 PM
 */
using System;
using System.IO.Ports;
using System.Collections.Generic;

namespace CCManager
{
	/// <summary>
	/// Description of RF.
	/// </summary>
	public class RF
	{
		SerialPort sp;
		public RF(SerialPort mcuSerialPort)
		{
			Log.Info(string.Format("RF initialized - Serial Port {0} with Baud Rate {1}", mcuSerialPort.PortName, mcuSerialPort.BaudRate), 1);
			Log.Info("If nothing loads in ~3 seconds, consider reconnecting device", 1);
			sp = mcuSerialPort;
        	// The port must not be already open (only one app can use the serial port at any time)
            if (sp.IsOpen) 
            {
            	Log.Error("Serial Port is already in use!");
                throw new NotImplementedException();
            }
            sp.Open();
            
            string bridgeSignature = Serial.GetBridgeSignature(sp);
            if (bridgeSignature.Trim().ToLower() == "MSP430G2553 LAUNCHPAD 0.1".Trim().ToLower())
            {
            	Log.Success(string.Format("Connected to {0}", bridgeSignature));
            }
            else if (bridgeSignature.Trim().ToLower() == "ATMEGA328P PRO MINI 0.1".Trim().ToLower())
            {
            	Log.Success(string.Format("Connected to {0}", bridgeSignature));
            }
            else 
            {
            	Log.Warning(string.Format("Unknown device: {0}", bridgeSignature));
            }
		}
		
		private static byte FlipByte(byte inByte)
		{
			uint flipThis = inByte;
			return (byte)(((uint)~flipThis) & 0xff);
		}
		
		public void SetupRegisters()
		{
			Log.Info("Writing Registers", 1);
			
			foreach (KeyValuePair<string, byte> configurationRegisterValue in CCRegister.ConfigurationRegisterValues)
			{
				SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters[configurationRegisterValue.Key], configurationRegisterValue.Value);
			}
		}
		
		public double GetCarrierFrequency()
		{
			double firstRegisterByte = (double)CCRegister.ConfigurationRegisterValues["FREQ2"];
			double secondRegisterByte = (double)CCRegister.ConfigurationRegisterValues["FREQ1"];
			double thirdRegisterByte = (double)CCRegister.ConfigurationRegisterValues["FREQ0"];
			
			firstRegisterByte = firstRegisterByte * 26;
			secondRegisterByte = secondRegisterByte / 255 * 26;
			thirdRegisterByte = thirdRegisterByte / 255 / 255 * 26;
			
			return Math.Round((firstRegisterByte + +secondRegisterByte + +thirdRegisterByte), 4);
		}
		
		public void SetCarrierFrequency(double frequencyInMHz)
		{
			Log.Info(string.Format("Setting Carrier Frequency as {0} MHz", frequencyInMHz), 1);
			
			// bounds checking for cc1101 hardware limitations
			if (!(((frequencyInMHz >= 300) && (frequencyInMHz <= 348)) || ((frequencyInMHz >= 387) && (frequencyInMHz <= 464)) || ((frequencyInMHz >= 779) && (frequencyInMHz <= 928))))
			{
				Log.Error("Frequency out of bounds! Use 300-348, 387-464, 779-928MHz only!");
				return;
			}
			
			// trying to avoid any floating point issues
			double secondByteOverflow = frequencyInMHz % 26;
			double firstByteValue = (double)((frequencyInMHz - secondByteOverflow) / 26);
			
			double thirdByteOverflow = (secondByteOverflow * 255) % 26;
			double secondByteValue = (double)(((secondByteOverflow * 255) - thirdByteOverflow) / 26);
			
			double excessOverflow = (thirdByteOverflow * 255) % 26;
			double thirdByteValue = (double)(((thirdByteOverflow * 255) - excessOverflow) / 26);

			CCRegister.ConfigurationRegisterValues["FREQ2"] = (byte)firstByteValue;
			CCRegister.ConfigurationRegisterValues["FREQ1"] = (byte)secondByteValue;
			CCRegister.ConfigurationRegisterValues["FREQ0"] = (byte)thirdByteValue;

			SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters["FREQ2"], CCRegister.ConfigurationRegisterValues["FREQ2"]);
			SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters["FREQ1"], CCRegister.ConfigurationRegisterValues["FREQ1"]);
			SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters["FREQ0"] , CCRegister.ConfigurationRegisterValues["FREQ0"]);
			ShortWait();
			
		}
		
		// Set the baud rate of the device for tx/rx
		// in: baudrate in bauds
		// see http://www.ti.com/lit/ds/symlink/cc1101.pdf , page 35
		public void SetBaudRate(double baudRate)
		{
	        double clockFrequencyMHz = CCRegister.CC1101_CLOCK_FREQUENCY;
	        
	        double baudRateExponent = 0;
	        double baudRateMantissa = 0;
	        for (int tempExponent = 0; tempExponent < 16; tempExponent++)
	        {
	            int tempMantissa = (int)((baudRate * Math.Pow(2, 28) / (Math.Pow(2, tempExponent)* (clockFrequencyMHz * 1000000.0)) - 256) + .5);
	            if (tempMantissa < 256)
	            {
	                baudRateExponent = tempExponent;
	                baudRateMantissa = tempMantissa;
	                break;            
	            }
	        }

	        baudRate = (1000000.0 * clockFrequencyMHz * (256 + baudRateMantissa) * Math.Pow(2, baudRateExponent) / Math.Pow(2, 28));
	        baudRate = Math.Round(baudRate, 4);
	        
	        CCRegister.ConfigurationRegisterValues["MDMCFG4"] = (byte)((CCRegister.ConfigurationRegisterValues["MDMCFG4"] & 0xf0) | (int)baudRateExponent);
		    CCRegister.ConfigurationRegisterValues["MDMCFG3"] = (byte)baudRateMantissa;
		    
			SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters["MDMCFG4"], CCRegister.ConfigurationRegisterValues["MDMCFG4"]);
			SPI.WriteRegister(sp, CCRegister.ConfigurationRegisters["MDMCFG3"], CCRegister.ConfigurationRegisterValues["MDMCFG3"]);
			
		    Log.Info(string.Format("Transmission Baud rate set to {0}. E: {1}, M: {2}", baudRate, baudRateExponent, baudRateMantissa), 1);
		}
		
		public double GetBaudRate()
		{
			uint baudRateExponent = ((uint)CCRegister.ConfigurationRegisterValues["MDMCFG4"]) & 0x0F;
			uint baudRateMantissa = ((uint)CCRegister.ConfigurationRegisterValues["MDMCFG3"]);
		    
			double baudRate = 1000000.0 * CCRegister.CC1101_CLOCK_FREQUENCY * (256 + baudRateMantissa) * Math.Pow(2, baudRateExponent) / Math.Pow(2, 28);
			return Math.Round(baudRate, 4);
		}
		
		public void SetupPATABLE()
		{
			Log.Info("Writing PATABLE", 1);
			SPI.WriteBurstRegister(sp, CCRegister.CC1101_PATABLE, CCRegister.PATABLE);
		}
		
		public void Reset()
		{
			Log.Info("Resetting CC1101", 1);
			Serial.ResetRf(sp);
		}
		
		public void EnterIdleState()
		{
			Log.Info("Setting CC1101 Idle state", 1);
			SPI.Strobe(sp, CCRegister.CC1101_SIDLE);
		}
		
		public void EnterTransmitState()
		{
			Log.Info("Setting CC1101 Transmit state", 1);
			SPI.Strobe(sp, CCRegister.CC1101_STX);
		}
		
		public void ShortWait()
		{
			Log.Info("Waiting for CC1101 - 250ms", 1);
			Serial.Delay(sp, 250);
		}
		
		public void Transmit(byte[] dataToTransmit)
		{
			List<byte> dataToTransmitInverted = new List<byte>();
		  	for (int i = 0; i < dataToTransmit.Length; i++)
		  	{
		  		dataToTransmitInverted.Add(FlipByte(dataToTransmit[i]));
		  	}
		  
		  	Log.Info("Writing TXFIFO", 2);
		  	SPI.WriteRegister(sp, CCRegister.CC1101_TXFIFO, (byte)dataToTransmit.Length);
		  	SPI.WriteBurstRegister(sp, CCRegister.CC1101_TXFIFO, dataToTransmit);
		  	
		  	EnterTransmitState();
		  	ShortWait();
		  	EnterIdleState();
		  	Log.Success("Transmit Completed");
		}
		
		public void CloseSerial()
		{
			Log.Info("Closing Serial Port", 1);
			sp.Close();
		}
	}
}
