/*
 * User: JinGen
 * Date: 11/12/2013
 * Time: 11:45 AM
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

namespace CCManager
{
	/// <summary>
	/// View to interact with CC1101 hardware via serial
	/// </summary>
	public partial class MainForm : Form
	{
		RF rf;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		
		
		void MainFormLoad(object sender, EventArgs e)
		{
			SetupRf();
			UpdateDataTypeLabel();
		}
		
		void SetupRf()
		{
			string serialPort = GetSerialPortViaPicker();
			SerialPort sp = new System.IO.Ports.SerialPort(serialPort, CCRegister.BRIDGE_BAUD_RATE);
			/*sp.Open();
			sp.Write("{g00}");
			Log.Error(sp.ReadLine());*/
			rf = new RF(sp);
            
            rf.Reset();
            
            rf.SetupPATABLE();
            
            rf.SetCarrierFrequency(315);
            txtCarrierFrequency.Text = rf.GetCarrierFrequency().ToString();
            
            rf.SetBaudRate(4153.25);
            txtBaudRate.Text = rf.GetBaudRate().ToString();
            
            rf.SetupRegisters();
            
            
            rf.EnterIdleState();
            rf.ShortWait();
            
            Log.Info("CC1101 Ready", 1);
		}
		
		
		string GetSerialPortViaPicker()
		{
			Form pickerDialog = new Form();
			pickerDialog.Text = "Select a serial port";
			pickerDialog.StartPosition = FormStartPosition.CenterScreen;
			pickerDialog.Height = 80;
			pickerDialog.Width = 220;
			pickerDialog.ShowIcon = false;
			pickerDialog.MaximizeBox = false;
			pickerDialog.MinimizeBox = false;
			pickerDialog.FormBorderStyle = FormBorderStyle.FixedSingle;
			
			ComboBox cbSerialPortPicker = new ComboBox();
			cbSerialPortPicker.Width = 90;
			cbSerialPortPicker.DropDownStyle = ComboBoxStyle.DropDownList;
			cbSerialPortPicker.Location = new Point(20, 15);
			PopulateSerialPortList(cbSerialPortPicker);
			pickerDialog.Controls.Add(cbSerialPortPicker);
			
			Button btnOk = new Button();
			btnOk.Text = "OK";
			btnOk.DialogResult = DialogResult.OK;
			btnOk.Location = new Point(120, 14);
			
			pickerDialog.Controls.Add(btnOk);
			
			if (pickerDialog.ShowDialog() == DialogResult.OK)
			{
				return cbSerialPortPicker.Text;
			}
			else 
			{
				string errorMessage = "No serial port picked! Exiting..";
				Log.Error(errorMessage);
				MessageBox.Show(errorMessage);
				Environment.Exit(0);
			}
			return null;
		}
		
		void PopulateSerialPortList(ComboBox cb)
		{
			cb.Items.Clear();
			string[] serialPorts = SerialPort.GetPortNames();
			foreach (string serialPort in serialPorts)
			{
				cb.Items.Add(serialPort);
			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			rf.CloseSerial();
			System.Threading.Thread.Sleep(500);
		}
		
		List<byte> ParseTransmitDataTextbox()
		{
			// assumes hex binaries, comma separated
			string[] stringBytes = txtTransmitData.Text
				.Replace("0x","")
				.Replace(" ", "")
				.Replace("\r", "")
				.Replace("\t", "")
				.Replace("\n", "")
				.Replace("\\x", ",")
				.Split(',');
			List<byte> result = new List<byte>();
			foreach (string stringByte in stringBytes)
			{
				result.Add((byte)Convert.ToInt32(stringByte, 16));
			}
			return result;
		}
		
		void BtnTransmitOnlyClick(object sender, EventArgs e)
		{
			((Button)sender).Enabled = false;
			
            rf.Transmit(ParseTransmitDataTextbox().ToArray());	
            ((Button)sender).Enabled = true;
		}
		
		
		void BtnWriteRegistersClick(object sender, EventArgs e)
		{
			((Button)sender).Enabled = false;
			rf.SetCarrierFrequency(double.Parse(txtCarrierFrequency.Text));
			rf.SetBaudRate(double.Parse(txtBaudRate.Text));
       		((Button)sender).Enabled = true;
		}
		
		void BtnViewEditRegistersClick(object sender, EventArgs e)
		{
			ShowRegisterViewEdit();
		}
		
		string ShowRegisterViewEdit()
		{
			string registerString = "";
			foreach (KeyValuePair<string, byte> registerValue in CCRegister.ConfigurationRegisterValues)
			{
				registerString += ",\r\n" + registerValue.Key + " = " + StringifyByteWithPadding(registerValue.Value);
			}
			
			if (registerString.Length != 0)
			{
				registerString = registerString.Remove(0,1);
			}
			
			registerString = registerString.Trim();
			
			Form registerDialog = new Form();
			registerDialog.Text = "View/Edit Registers";
			registerDialog.StartPosition = FormStartPosition.CenterScreen;
			registerDialog.Height = 580;
			registerDialog.Width = 328;
			registerDialog.ShowIcon = false;
			registerDialog.MinimizeBox = false;
			
			TextBox txtRegisterValues = new TextBox();
			txtRegisterValues.Multiline = true;
			txtRegisterValues.Location = new Point(10, 10);
			txtRegisterValues.Width = 300;
			txtRegisterValues.Height = 500;
			txtRegisterValues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			txtRegisterValues.Text = registerString;
			txtRegisterValues.Font = new Font("Courier New", 12);
			txtRegisterValues.ScrollBars = ScrollBars.Vertical; 
			registerDialog.Controls.Add(txtRegisterValues);
			
			Button btnOk = new Button();
			btnOk.Text = "OK";
			btnOk.DialogResult = DialogResult.OK;
			btnOk.Location = new Point(310 - btnOk.Width, 520);
			btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			
			registerDialog.Controls.Add(btnOk);
			
			if (registerDialog.ShowDialog() == DialogResult.OK)
			{
				string[] registerKeyValues = txtRegisterValues.Text.Split(',');
				foreach (string registerKeyValueString in registerKeyValues)
				{
					string[] registerKeyValueArray = registerKeyValueString.Split('=');
					string registerKey = registerKeyValueArray[0].Trim().ToUpper();
					string registerValue = registerKeyValueArray[1].Trim();
					
					CCRegister.ConfigurationRegisterValues[registerKey] = (byte)Convert.ToInt32(registerValue, 16);
				}
				rf.SetupRegisters();
				txtBaudRate.Text = rf.GetBaudRate().ToString();
				txtCarrierFrequency.Text = rf.GetCarrierFrequency().ToString();
				Log.Info("Registers OK", 2);
			}
			else 
			{
				// cancelled
			}
			return null;
		}
		
		void TxtTransmitDataTextChanged(object sender, EventArgs e)
		{
			UpdateDataTypeLabel();
		}
		
		void UpdateDataTypeLabel()
		{
			int resultByteCount = 0;
			TransmitDataType detectedType = DetectDataType(txtTransmitData.Text, ref resultByteCount);
			
			lblDataType.Text = string.Format("{0}, {1}/61 TX FIFO bytes", detectedType.ToString(), resultByteCount);
		}
		
		enum TransmitDataType 
		{
			UNKNOWN_UNSUPPORTED,
			BINARY_UNSUPPORTED,
			HEX_C,
			HEX_PYTHON,
			HEX_SPACE_UNSUPPORTED
		}
		
		// converts a byte into hex with padding
		string StringifyByteWithPadding(byte inputByte)
		{
			string output = inputByte.ToString("X");
			if (output.Length == 1)
			{
				return "0" + output;
			}
			return output;
		}
		
		// quick and lazy data type detection
		TransmitDataType DetectDataType(string testString, ref int byteCount)
		{
			// take the text and normalize, remove whitespace
			string cleanTransmitData = testString.ToLower()
				.Trim().Replace("\r","").Replace("\n", "").Replace("\t", "");
			
			// note that the search order matters! Look for complex items first, then the simpler forms e.g. binary
			if (cleanTransmitData.Contains("0x"))
		    {
				// likely hex e.g. "0x10, 0x20"
				string[] bytes = cleanTransmitData.Split(',');
				byteCount = bytes.Length;
				return TransmitDataType.HEX_C;

			}
			else if (cleanTransmitData.Contains("\\x"))
			{
				// likely python-style \x10\x20
				string[] bytes = cleanTransmitData.Split('\\');
				byteCount = bytes.Length;
				return TransmitDataType.HEX_PYTHON;
			}
			else if (cleanTransmitData.Contains(" "))
			{
				// space delimited hex e.g. DE AD BE EF
				string[] bytes = cleanTransmitData.Split(' ');
				byteCount = bytes.Length;
				return TransmitDataType.HEX_SPACE_UNSUPPORTED;
			}
			else if (cleanTransmitData.Contains("1") && cleanTransmitData.Contains("0"))
			{
				// likely plain binary eg 00100011111
				byteCount = (int)Math.Round((double)(cleanTransmitData.Length / 8), MidpointRounding.AwayFromZero);
				return TransmitDataType.BINARY_UNSUPPORTED;
			}
			
			byteCount = 0;
			return TransmitDataType.UNKNOWN_UNSUPPORTED;
		}
		
		// checks a string to see if it only contains valid chars defined in validChars
		bool CharsetTest(string testThis, string[] validChars)
		{
			string emptyBufferTest = testThis;
			foreach (string validChar in validChars)
			{
				emptyBufferTest = emptyBufferTest.Replace(validChar, "");
			}
			if (emptyBufferTest.Length == 0)
			{
				// looks good!
				return true;
			}
			return false;
		}
		
		void BtnImportSimpliciTIClick(object sender, EventArgs e)
		{
			Form simplicitiImport = new Form();
			simplicitiImport.Text = "Import SimpliciTI Settings";
			simplicitiImport.StartPosition = FormStartPosition.CenterScreen;
			simplicitiImport.Height = 580;
			simplicitiImport.Width = 328;
			simplicitiImport.ShowIcon = false;
			simplicitiImport.MinimizeBox = false;
			
			TextBox txtRegisterValues = new TextBox();
			txtRegisterValues.Multiline = true;
			txtRegisterValues.Location = new Point(10, 10);
			txtRegisterValues.Width = 300;
			txtRegisterValues.Height = 500;
			txtRegisterValues.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
			txtRegisterValues.Text = "";
			txtRegisterValues.Font = new Font("Courier New", 12);
			txtRegisterValues.ScrollBars = ScrollBars.Vertical; 
			simplicitiImport.Controls.Add(txtRegisterValues);
			
			Button btnOk = new Button();
			btnOk.Text = "OK";
			btnOk.DialogResult = DialogResult.OK;
			btnOk.Location = new Point(310 - btnOk.Width, 520);
			btnOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			
			simplicitiImport.Controls.Add(btnOk);
			
			if (simplicitiImport.ShowDialog() == DialogResult.OK)
			{
				// test if it looks ok
				string signature = "_RADIO_";
				if (txtRegisterValues.Text.Contains(signature))
				{
					string rawData = txtRegisterValues.Text.Replace("#endif", "");
					List<string> registerKeyValues = new List<string>(rawData.Remove(0, rawData.IndexOf(signature)).Replace("SMARTRF_SETTING_","").Replace("#define ", ",").Split(','));
					if (registerKeyValues.Count != 0)
					{
						registerKeyValues.RemoveAt(0);
					}
					foreach (string registerKeyValue in registerKeyValues) 
					{
						string[] registerKeyValueArray = registerKeyValue.Replace("0x", ",").Split(',');
						string registerKey = registerKeyValueArray[0].Trim().ToUpper();
						byte registerValue = (byte)Convert.ToInt32(registerKeyValueArray[1].Trim(), 16);
						
						CCRegister.ConfigurationRegisterValues[registerKey] = registerValue;
						
						Log.Info(string.Format("TI Key: {0} Value: {1:X}", registerKey, registerValue), 3);
					}
					rf.SetupRegisters();
					txtBaudRate.Text = rf.GetBaudRate().ToString();
					txtCarrierFrequency.Text = rf.GetCarrierFrequency().ToString();
					Log.Info("SimpliciTI Registers OK", 2);
				}
			}
		}
	}
}
