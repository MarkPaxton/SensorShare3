namespace SensorShare.Compact
{
   partial class SensorShareClient
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
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.serverListView1 = new SensorShare.Compact.ServerListView();
         this.SuspendLayout();
         // 
         // mainMenu
         // 
         this.mainMenu.MenuItems.Add(this.menuItem1);
         // 
         // menuItem1
         // 
         this.menuItem1.Text = "Exit";
         this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
         // 
         // serverListView1
         // 
         this.serverListView1.Dock = System.Windows.Forms.DockStyle.Top;
         this.serverListView1.Location = new System.Drawing.Point(0, 0);
         this.serverListView1.Name = "serverListView1";
         this.serverListView1.Size = new System.Drawing.Size(240, 196);
         this.serverListView1.TabIndex = 0;
         // 
         // SensorNetClient
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(240, 268);
         this.Controls.Add(this.serverListView1);
         this.Menu = this.mainMenu;
         this.Name = "SensorNetClient";
         this.Text = "SensorNetClient";
         this.Load += new System.EventHandler(this.SensorShareClient_Load);
         this.Closed += new System.EventHandler(this.SensorNetClient_Closed);
         this.Closing += new System.ComponentModel.CancelEventHandler(this.SensorNetClient_Closing);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.MenuItem menuItem1;
      private ServerListView serverListView1;
   }
}

