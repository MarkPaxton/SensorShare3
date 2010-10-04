using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using mcp;

namespace SensorShare.Compact
{
   public partial class LogBrowser : Form
   {
      
      public LogBrowser()
      {
         closeDelegate = new VoidDelegate(this.Close);
         InitializeComponent();
         sqLiteConnection.Open();
         logDataAdapter.Fill(logDataSet);
         serversDataAdapter.Fill(serverDataSet);         
      }

      VoidDelegate closeDelegate = null;
      private void closeMenuItem_Click(object sender, EventArgs e)
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

      private void LogBrowser_Closed(object sender, EventArgs e)
      {
         Application.Exit();
      }
   }
}