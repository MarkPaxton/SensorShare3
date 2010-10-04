namespace SensorShare.Desktop
{
   partial class SensorNetServer
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
         this.components = new System.ComponentModel.Container();
         this.serverNameLabel = new System.Windows.Forms.Label();
         this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
         this.menuItem4 = new System.Windows.Forms.MenuItem();
         this.configMenuItem = new System.Windows.Forms.MenuItem();
         this.menuItem2 = new System.Windows.Forms.MenuItem();
         this.SuspendLayout();
         // 
         // serverNameLabel
         // 
         this.serverNameLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
         this.serverNameLabel.Location = new System.Drawing.Point(3, 9);
         this.serverNameLabel.Name = "serverNameLabel";
         this.serverNameLabel.Size = new System.Drawing.Size(212, 28);
         this.serverNameLabel.TabIndex = 1;
         // 
         // mainMenu
         // 
         this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4,
            this.menuItem2});
         // 
         // menuItem4
         // 
         this.menuItem4.Index = 0;
         this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.configMenuItem});
         this.menuItem4.Text = "Server";
         // 
         // configMenuItem
         // 
         this.configMenuItem.Index = 0;
         this.configMenuItem.Text = "Config";
         this.configMenuItem.Click += new System.EventHandler(this.configMenuItem_Click);
         // 
         // menuItem2
         // 
         this.menuItem2.Index = 1;
         this.menuItem2.Text = "Exit";
         this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
         // 
         // SensorNetServer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(599, 459);
         this.Controls.Add(this.serverNameLabel);
         this.Menu = this.mainMenu;
         this.Name = "SensorNetServer";
         this.Text = "SensorNetServer";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SensorNetServer_FormClosing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label serverNameLabel;
      private System.Windows.Forms.MainMenu mainMenu;
      private System.Windows.Forms.MenuItem menuItem4;
      private System.Windows.Forms.MenuItem configMenuItem;
      private System.Windows.Forms.MenuItem menuItem2;
   }
}

