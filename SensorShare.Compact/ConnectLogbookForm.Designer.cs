namespace SensorShare.Compact
{
   partial class ConnectLogbookForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.ConnectLogbookButton = new System.Windows.Forms.Button();
         this.disconnectButton = new System.Windows.Forms.Button();
         this.statusLabel = new System.Windows.Forms.Label();
         this.identifyButton = new System.Windows.Forms.Button();
         this.identifyLabel = new System.Windows.Forms.Label();
         this.testReadingButton = new System.Windows.Forms.Button();
         this.testReadingLabel = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // ConnectLogbookButton
         // 
         this.ConnectLogbookButton.Location = new System.Drawing.Point(3, 3);
         this.ConnectLogbookButton.Name = "ConnectLogbookButton";
         this.ConnectLogbookButton.Size = new System.Drawing.Size(95, 31);
         this.ConnectLogbookButton.TabIndex = 0;
         this.ConnectLogbookButton.Text = "Connect";
         this.ConnectLogbookButton.Click += new System.EventHandler(this.ConnectLogbookButton_Click);
         // 
         // disconnectButton
         // 
         this.disconnectButton.Location = new System.Drawing.Point(3, 40);
         this.disconnectButton.Name = "disconnectButton";
         this.disconnectButton.Size = new System.Drawing.Size(95, 31);
         this.disconnectButton.TabIndex = 1;
         this.disconnectButton.Text = "Disconnect";
         this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
         // 
         // statusLabel
         // 
         this.statusLabel.Location = new System.Drawing.Point(113, 20);
         this.statusLabel.Name = "statusLabel";
         this.statusLabel.Size = new System.Drawing.Size(107, 51);
         this.statusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // identifyButton
         // 
         this.identifyButton.Location = new System.Drawing.Point(3, 108);
         this.identifyButton.Name = "identifyButton";
         this.identifyButton.Size = new System.Drawing.Size(95, 31);
         this.identifyButton.TabIndex = 3;
         this.identifyButton.Text = "Identify";
         this.identifyButton.Click += new System.EventHandler(this.identifyButton_Click);
         // 
         // identifyLabel
         // 
         this.identifyLabel.Location = new System.Drawing.Point(104, 108);
         this.identifyLabel.Name = "identifyLabel";
         this.identifyLabel.Size = new System.Drawing.Size(133, 40);
         this.identifyLabel.Text = "Sensor Information";
         this.identifyLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // testReadingButton
         // 
         this.testReadingButton.Location = new System.Drawing.Point(55, 154);
         this.testReadingButton.Name = "testReadingButton";
         this.testReadingButton.Size = new System.Drawing.Size(130, 31);
         this.testReadingButton.TabIndex = 6;
         this.testReadingButton.Text = "Get a Test Reading";
         this.testReadingButton.Click += new System.EventHandler(this.testReadingButton_Click);
         // 
         // testReadingLabel
         // 
         this.testReadingLabel.Location = new System.Drawing.Point(3, 188);
         this.testReadingLabel.Name = "testReadingLabel";
         this.testReadingLabel.Size = new System.Drawing.Size(237, 106);
         // 
         // ConnectLogbookForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(240, 294);
         this.Controls.Add(this.testReadingLabel);
         this.Controls.Add(this.testReadingButton);
         this.Controls.Add(this.identifyLabel);
         this.Controls.Add(this.identifyButton);
         this.Controls.Add(this.statusLabel);
         this.Controls.Add(this.disconnectButton);
         this.Controls.Add(this.ConnectLogbookButton);
         this.Name = "ConnectLogbookForm";
         this.Text = "Logbook Connection";
         this.Closing += new System.ComponentModel.CancelEventHandler(this.ConnectLogbookForm_Closing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button ConnectLogbookButton;
      private System.Windows.Forms.Button disconnectButton;
      private System.Windows.Forms.Label statusLabel;
      private System.Windows.Forms.Button identifyButton;
      private System.Windows.Forms.Label identifyLabel;
      private System.Windows.Forms.Button testReadingButton;
      private System.Windows.Forms.Label testReadingLabel;
   }
}