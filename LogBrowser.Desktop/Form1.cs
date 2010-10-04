using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SensorShare.Desktop
{
   public partial class LogBrowserForm : Form
   {
      private string filename = "Data Source=\"D:/mcp/Temp/sqlite databases/SensorShareServer.db\"";
      public LogBrowserForm(string newFilename)
      {
         this.filename = String.Format("Data Source=\"{0}\"", newFilename);
         sqLiteConnection = new SQLiteConnection(this.filename);
         InitializeComponent();
         sqLiteConnection.Open();
         logDataAdapter.Fill(logDataSet);
         serverDataAdapter.Fill(serverDataSet);

      }
   }
}
