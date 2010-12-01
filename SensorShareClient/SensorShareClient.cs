using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SensorShare.Network;
using System.Net;
using SensorShare;
using System.Collections;
using System.Diagnostics;
using OpenNETCF.WindowsCE;
using OpenNETCF.Net;
using OpenNETCF.Net.NetworkInformation;
using System.Data.SQLite;
using OpenNETCF.Windows.Forms;
using System.IO;
using mcp;
using mcp.Logs;
using mcp.Compact;

namespace SensorShare.Compact
{
   public partial class SensorShareClient : Form
   {
      SQLiteConnection database = null;

      public SensorShareClient()
      {
         InitializeComponent();

         #region Copy databases if needed
         if (!Directory.Exists(SensorShareConfig.DatabaseFolder))
         {
             Debug.WriteLine("Creating: " + SensorShareConfig.DatabaseFolder);
             Directory.CreateDirectory(SensorShareConfig.DatabaseFolder);
         }
         if (!File.Exists(SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ClientDatabase))
         {
             Debug.WriteLine("Copying database to " + SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ClientDatabase);
             File.Copy(Application2.StartupPath + "\\" + SensorShareConfig.ClientDatabase,
               SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ClientDatabase);
         }


         database = new SQLiteConnection("Data Source=\"" +
            SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ClientDatabase + "\"");
         #endregion

         SetUpWorkings();
      }

      private void SensorShareClient_Load(object sender, EventArgs e)
      {
         log.Append("StartServer", "Starting on " + ClientID.ToString());
         StartClient();
      }

      
     private void menuItem1_Click(object sender, EventArgs e)
      {
         if (this.InvokeRequired)
         {
            this.Invoke(closeDelegate);
         }
         else
         {
            this.Close();
         }
      }




   }
}