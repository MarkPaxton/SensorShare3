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
using Logs;

namespace SensorShare.Compact
{
   public partial class SensorShareClient : Form
   {
      SQLiteConnection database = null;

      public SensorShareClient()
      {
         InitializeComponent();

         #region Copy databases if needed
         if (!Directory.Exists(SensorNetConfig.DatabaseFolder))
         {
            Debug.WriteLine("Creating: " + SensorNetConfig.DatabaseFolder);
            Directory.CreateDirectory(SensorNetConfig.DatabaseFolder);
         }
         if (!File.Exists(SensorNetConfig.DatabaseFolder + "\\" + SensorNetConfig.ClientDatabase))
         {
            Debug.WriteLine("Copying database to " + SensorNetConfig.DatabaseFolder + "\\" + SensorNetConfig.ClientDatabase);
            File.Copy(Application2.StartupPath + "\\" + SensorNetConfig.ClientDatabase,
               SensorNetConfig.DatabaseFolder + "\\" + SensorNetConfig.ClientDatabase);
         }
         database = DatabaseHelper.ConnectToSQL("Data Source=\"" +
            SensorNetConfig.DatabaseFolder + "\\" + SensorNetConfig.ClientDatabase + "\"");
         #endregion

         SetUpWorkings();
      }

      private void SensorNetClient_Load(object sender, EventArgs e)
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