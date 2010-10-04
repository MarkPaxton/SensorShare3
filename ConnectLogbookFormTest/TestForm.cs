using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ScienceScope;
using SensorShare.Compact;
using mcp;

namespace ConnectLogbookFormTest
{
   public partial class TestForm : Form
   {
      public TestForm()
      {
         InitializeComponent();
      }

      // Logbook stuff
      ConnectLogbookForm connectForm = null;

      private Logbook logbook;
      private string logbookPort = "COM8";

      private void InitialiseLogbook()
      {
         Thread logbookThread = new Thread(new ThreadStart(LogbookThread));
         logbookThread.Start();
         Thread.Sleep(500);         
      }

      private void LogbookThread()
      {
         logbook = new Logbook(logbookPort);
      }

      private void button1_Click(object sender, EventArgs e)
      {
         connectForm = new ConnectLogbookForm(logbook);
         connectForm.ShowDialog();
      }

      VoidDelegate clodeDel = null;
      private void button2_Click(object sender, EventArgs e)
      {
         clodeDel = new VoidDelegate(this.Close);
         if (this.InvokeRequired)
         {
            this.Invoke(clodeDel);
         }
         else
         {
            this.Close();
         }
      }

      private void TestForm_Closing(object sender, CancelEventArgs e)
      {
         if (logbook.IsActive)
         {
            logbook.Deactivate();
         }
      }

      private void TestForm_Closed(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void TestForm_Load(object sender, EventArgs e)
      {
         InitialiseLogbook();
      }

   }
}