namespace SensorShare.Compact
{
   partial class SensorShareServer
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;
      private System.Windows.Forms.MainMenu mainMenu;

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
         this.mainMenu = new System.Windows.Forms.MainMenu();
         this.menuItem4 = new System.Windows.Forms.MenuItem();
         this.configMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem2 = new System.Windows.Forms.MenuItem();
         this.serverNameLabel = new System.Windows.Forms.Label();
         this.sensorDescriptionsLabel = new System.Windows.Forms.Label();
         this.serverPictureBox = new System.Windows.Forms.PictureBox();
         this.SuspendLayout();
         // 
         // mainMenu
         // 
         this.mainMenu.MenuItems.Add(this.menuItem4);
         this.mainMenu.MenuItems.Add(this.menuItem2);
         // 
         // menuItem4
         // 
         this.menuItem4.MenuItems.Add(this.configMenuItem);
         this.menuItem4.Text = "Server";
         // 
         // configMenuItem
         // 
         this.configMenuItem.Text = "Config";
         this.configMenuItem.Click += new System.EventHandler(this.configMenuItem_Click);
         // 
         // menuItem2
         // 
         this.menuItem2.Text = "Exit";
         this.menuItem2.Click += new System.EventHandler(this.exitMenuItem_Click);
         // 
         // serverNameLabel
         // 
         this.serverNameLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
         this.serverNameLabel.Location = new System.Drawing.Point(0, 0);
         this.serverNameLabel.Name = "serverNameLabel";
         this.serverNameLabel.Size = new System.Drawing.Size(240, 28);
         // 
         // sensorDescriptionsLabel
         // 
         this.sensorDescriptionsLabel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
         this.sensorDescriptionsLabel.Location = new System.Drawing.Point(128, 35);
         this.sensorDescriptionsLabel.Name = "sensorDescriptionsLabel";
         this.sensorDescriptionsLabel.Size = new System.Drawing.Size(112, 112);
         // 
         // serverPictureBox
         // 
         this.serverPictureBox.Location = new System.Drawing.Point(1, 35);
         this.serverPictureBox.Name = "serverPictureBox";
         this.serverPictureBox.Size = new System.Drawing.Size(122, 112);
         // 
         // SensorShareServer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(240, 268);
         this.Controls.Add(this.serverPictureBox);
         this.Controls.Add(this.sensorDescriptionsLabel);
         this.Controls.Add(this.serverNameLabel);
         this.Menu = this.mainMenu;
         this.Name = "SensorShareServer";
         this.Text = "SensorNetServer";
         this.Load += new System.EventHandler(this.SensorShareServer_Load);
         this.Closed += new System.EventHandler(this.SensorNetServer_Closed);
         this.Closing += new System.ComponentModel.CancelEventHandler(this.SensorNetServer_Closing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.MenuItem menuItem2;
      private System.Windows.Forms.MenuItem menuItem4;
      private System.Windows.Forms.MenuItem configMenuItem;
      private System.Windows.Forms.Label serverNameLabel;
      private System.Windows.Forms.Label sensorDescriptionsLabel;
      private System.Windows.Forms.PictureBox serverPictureBox;
   }
}

