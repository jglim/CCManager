/*
 * Created by SharpDevelop.
 * User: JinGen
 * Date: 11/12/2013
 * Time: 11:45 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace CCManager
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnTransmitOnly = new System.Windows.Forms.Button();
			this.lblDescCarrierFrequency = new System.Windows.Forms.Label();
			this.txtCarrierFrequency = new System.Windows.Forms.TextBox();
			this.lblDescBaudRate = new System.Windows.Forms.Label();
			this.txtBaudRate = new System.Windows.Forms.TextBox();
			this.cbRegisterConfiguration = new System.Windows.Forms.GroupBox();
			this.btnWriteRegisters = new System.Windows.Forms.Button();
			this.gbTransmissionDetails = new System.Windows.Forms.GroupBox();
			this.lblDataType = new System.Windows.Forms.Label();
			this.lblDescData = new System.Windows.Forms.Label();
			this.txtTransmitData = new System.Windows.Forms.TextBox();
			this.gbAdvancedConfiguration = new System.Windows.Forms.GroupBox();
			this.btnViewEditRegisters = new System.Windows.Forms.Button();
			this.btnImportSimpliciTI = new System.Windows.Forms.Button();
			this.cbRegisterConfiguration.SuspendLayout();
			this.gbTransmissionDetails.SuspendLayout();
			this.gbAdvancedConfiguration.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnTransmitOnly
			// 
			this.btnTransmitOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTransmitOnly.Location = new System.Drawing.Point(408, 22);
			this.btnTransmitOnly.Name = "btnTransmitOnly";
			this.btnTransmitOnly.Size = new System.Drawing.Size(75, 23);
			this.btnTransmitOnly.TabIndex = 0;
			this.btnTransmitOnly.Text = "Transmit";
			this.btnTransmitOnly.UseVisualStyleBackColor = true;
			this.btnTransmitOnly.Click += new System.EventHandler(this.BtnTransmitOnlyClick);
			// 
			// lblDescCarrierFrequency
			// 
			this.lblDescCarrierFrequency.Location = new System.Drawing.Point(6, 27);
			this.lblDescCarrierFrequency.Name = "lblDescCarrierFrequency";
			this.lblDescCarrierFrequency.Size = new System.Drawing.Size(128, 23);
			this.lblDescCarrierFrequency.TabIndex = 1;
			this.lblDescCarrierFrequency.Text = "Carrier Frequency (MHz)";
			// 
			// txtCarrierFrequency
			// 
			this.txtCarrierFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCarrierFrequency.Location = new System.Drawing.Point(140, 24);
			this.txtCarrierFrequency.Name = "txtCarrierFrequency";
			this.txtCarrierFrequency.Size = new System.Drawing.Size(100, 20);
			this.txtCarrierFrequency.TabIndex = 2;
			// 
			// lblDescBaudRate
			// 
			this.lblDescBaudRate.Location = new System.Drawing.Point(72, 53);
			this.lblDescBaudRate.Name = "lblDescBaudRate";
			this.lblDescBaudRate.Size = new System.Drawing.Size(62, 23);
			this.lblDescBaudRate.TabIndex = 3;
			this.lblDescBaudRate.Text = "Baud Rate";
			// 
			// txtBaudRate
			// 
			this.txtBaudRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBaudRate.Location = new System.Drawing.Point(140, 50);
			this.txtBaudRate.Name = "txtBaudRate";
			this.txtBaudRate.Size = new System.Drawing.Size(100, 20);
			this.txtBaudRate.TabIndex = 4;
			// 
			// cbRegisterConfiguration
			// 
			this.cbRegisterConfiguration.Controls.Add(this.btnWriteRegisters);
			this.cbRegisterConfiguration.Controls.Add(this.lblDescCarrierFrequency);
			this.cbRegisterConfiguration.Controls.Add(this.txtBaudRate);
			this.cbRegisterConfiguration.Controls.Add(this.lblDescBaudRate);
			this.cbRegisterConfiguration.Controls.Add(this.txtCarrierFrequency);
			this.cbRegisterConfiguration.Location = new System.Drawing.Point(12, 12);
			this.cbRegisterConfiguration.Name = "cbRegisterConfiguration";
			this.cbRegisterConfiguration.Size = new System.Drawing.Size(252, 110);
			this.cbRegisterConfiguration.TabIndex = 5;
			this.cbRegisterConfiguration.TabStop = false;
			this.cbRegisterConfiguration.Text = "Transmission Configuration";
			// 
			// btnWriteRegisters
			// 
			this.btnWriteRegisters.Location = new System.Drawing.Point(140, 76);
			this.btnWriteRegisters.Name = "btnWriteRegisters";
			this.btnWriteRegisters.Size = new System.Drawing.Size(75, 23);
			this.btnWriteRegisters.TabIndex = 5;
			this.btnWriteRegisters.Text = "Write Data";
			this.btnWriteRegisters.UseVisualStyleBackColor = true;
			this.btnWriteRegisters.Click += new System.EventHandler(this.BtnWriteRegistersClick);
			// 
			// gbTransmissionDetails
			// 
			this.gbTransmissionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.gbTransmissionDetails.Controls.Add(this.lblDataType);
			this.gbTransmissionDetails.Controls.Add(this.lblDescData);
			this.gbTransmissionDetails.Controls.Add(this.txtTransmitData);
			this.gbTransmissionDetails.Controls.Add(this.btnTransmitOnly);
			this.gbTransmissionDetails.Location = new System.Drawing.Point(12, 138);
			this.gbTransmissionDetails.Name = "gbTransmissionDetails";
			this.gbTransmissionDetails.Size = new System.Drawing.Size(489, 163);
			this.gbTransmissionDetails.TabIndex = 6;
			this.gbTransmissionDetails.TabStop = false;
			this.gbTransmissionDetails.Text = "Transmission Details";
			// 
			// lblDataType
			// 
			this.lblDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDataType.Location = new System.Drawing.Point(42, 27);
			this.lblDataType.Name = "lblDataType";
			this.lblDataType.Size = new System.Drawing.Size(360, 22);
			this.lblDataType.TabIndex = 3;
			this.lblDataType.Text = "-";
			// 
			// lblDescData
			// 
			this.lblDescData.Location = new System.Drawing.Point(6, 27);
			this.lblDescData.Name = "lblDescData";
			this.lblDescData.Size = new System.Drawing.Size(41, 22);
			this.lblDescData.TabIndex = 2;
			this.lblDescData.Text = "Data";
			// 
			// txtTransmitData
			// 
			this.txtTransmitData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTransmitData.Location = new System.Drawing.Point(6, 52);
			this.txtTransmitData.Multiline = true;
			this.txtTransmitData.Name = "txtTransmitData";
			this.txtTransmitData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTransmitData.Size = new System.Drawing.Size(477, 105);
			this.txtTransmitData.TabIndex = 1;
			this.txtTransmitData.TextChanged += new System.EventHandler(this.TxtTransmitDataTextChanged);
			// 
			// gbAdvancedConfiguration
			// 
			this.gbAdvancedConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.gbAdvancedConfiguration.Controls.Add(this.btnViewEditRegisters);
			this.gbAdvancedConfiguration.Controls.Add(this.btnImportSimpliciTI);
			this.gbAdvancedConfiguration.Location = new System.Drawing.Point(270, 12);
			this.gbAdvancedConfiguration.Name = "gbAdvancedConfiguration";
			this.gbAdvancedConfiguration.Size = new System.Drawing.Size(231, 110);
			this.gbAdvancedConfiguration.TabIndex = 7;
			this.gbAdvancedConfiguration.TabStop = false;
			this.gbAdvancedConfiguration.Text = "Advanced Configuration";
			// 
			// btnViewEditRegisters
			// 
			this.btnViewEditRegisters.Location = new System.Drawing.Point(6, 48);
			this.btnViewEditRegisters.Name = "btnViewEditRegisters";
			this.btnViewEditRegisters.Size = new System.Drawing.Size(143, 23);
			this.btnViewEditRegisters.TabIndex = 1;
			this.btnViewEditRegisters.Text = "View/Edit Registers";
			this.btnViewEditRegisters.UseVisualStyleBackColor = true;
			this.btnViewEditRegisters.Click += new System.EventHandler(this.BtnViewEditRegistersClick);
			// 
			// btnImportSimpliciTI
			// 
			this.btnImportSimpliciTI.Location = new System.Drawing.Point(6, 22);
			this.btnImportSimpliciTI.Name = "btnImportSimpliciTI";
			this.btnImportSimpliciTI.Size = new System.Drawing.Size(143, 23);
			this.btnImportSimpliciTI.TabIndex = 0;
			this.btnImportSimpliciTI.Text = "Import SimpliciTI Settings";
			this.btnImportSimpliciTI.UseVisualStyleBackColor = true;
			this.btnImportSimpliciTI.Click += new System.EventHandler(this.BtnImportSimpliciTIClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(513, 313);
			this.Controls.Add(this.gbAdvancedConfiguration);
			this.Controls.Add(this.gbTransmissionDetails);
			this.Controls.Add(this.cbRegisterConfiguration);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "CCManager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.cbRegisterConfiguration.ResumeLayout(false);
			this.cbRegisterConfiguration.PerformLayout();
			this.gbTransmissionDetails.ResumeLayout(false);
			this.gbTransmissionDetails.PerformLayout();
			this.gbAdvancedConfiguration.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label lblDataType;
		private System.Windows.Forms.Button btnImportSimpliciTI;
		private System.Windows.Forms.Button btnViewEditRegisters;
		private System.Windows.Forms.GroupBox gbAdvancedConfiguration;
		private System.Windows.Forms.TextBox txtTransmitData;
		private System.Windows.Forms.Label lblDescData;
		private System.Windows.Forms.GroupBox gbTransmissionDetails;
		private System.Windows.Forms.Button btnWriteRegisters;
		private System.Windows.Forms.GroupBox cbRegisterConfiguration;
		private System.Windows.Forms.TextBox txtBaudRate;
		private System.Windows.Forms.Label lblDescBaudRate;
		private System.Windows.Forms.TextBox txtCarrierFrequency;
		private System.Windows.Forms.Label lblDescCarrierFrequency;
		private System.Windows.Forms.Button btnTransmitOnly;
	}
}
