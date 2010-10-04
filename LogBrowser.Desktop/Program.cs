using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SensorShare.Desktop
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.FileName = "";
         openFileDialog.Filter = "Database Files|*.db";
         DialogResult result = openFileDialog.ShowDialog();
         if (result == DialogResult.OK)
         {
            Application.Run(new LogBrowserForm(openFileDialog.FileName));
         }
        
      }
   }
}
