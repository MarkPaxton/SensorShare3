using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Data.SQLite;
using OpenNETCF.Windows.Forms;
using OpenNETCF.WindowsCE;
using mcp;
using mcp.Logs;
using mcp.Compact;
using SensorShare;
using SensorShare.Compact;
using SensorShare.Network;
using ScienceScope;
using System.Threading;

namespace SensorShare.Compact
{
   public partial class SensorShareServer : Form
   {
      VoidDelegate closeDelegate;

      SQLiteConnection database = null;
      SQLiteConnection sensorsDatabase = null;
      ServerData CurrentServerData = null;

      bool takeReadings = false;
      TimerCallback readASCIITimerCallback;

      TextBoxDelegate textboxDelegate = new TextBoxDelegate(TextHelper.UpdateTextBox);

      public SensorShareServer()
      {
         InitializeComponent();
         closeDelegate = new VoidDelegate(this.Close);

         #region Copy Database Files

         FileHelper.CheckAndCreateFolder(SensorShareConfig.DatabaseFolder);
         FileHelper.CheckAndCopyFile(
            Application2.StartupPath + "\\" + SensorShareConfig.ServerDatabase,
            SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ServerDatabase);

         database = new SQLiteConnection("Data Source=\"" +
            SensorShareConfig.DatabaseFolder + "\\" + SensorShareConfig.ServerDatabase + "\"");
         sensorsDatabase = new SQLiteConnection("Data Source=\"" +
            Application2.StartupPath + "\\" + SensorShareConfig.SensorsDatabase + "\"");

         #endregion

         SetUpWorkings();
         readASCIITimerCallback = new TimerCallback(ReadASCIITimerTimeout);

      }

      private void SensorShareServer_Load(object sender, EventArgs e)
      {
      }

      private void exitMenuItem_Click(object sender, EventArgs e)
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

      private void SensorNetServer_Closing(object sender, CancelEventArgs e)
      {
         log.Append("SensorNetServer_Closed", "Application closing");
         StopWorkings();
         StopTakingReadings();
         if (logbook.IsActive)
         {
            disconnectLogbook();
         }
      }

      private void configMenuItem_Click(object sender, EventArgs e)
      {
         ServerConfig configForm = new ServerConfig();
         configForm.serverNameTextBox.Text = CurrentServerData.name;
         configForm.serverLocationTextBox.Text = CurrentServerData.location;
         configForm.serverDescriptionTextBox.Text = CurrentServerData.description;
         configForm.image = CurrentServerData.Picture();
         configForm.selectImageDialog.InitialDirectory = Application2.StartupPath;

         if (configForm.ShowDialog() == DialogResult.OK)
         {
            StopWorkings();
            CurrentServerData.id = Guid.NewGuid();
            CurrentServerData.name = configForm.serverNameTextBox.Text;
            CurrentServerData.location = configForm.serverLocationTextBox.Text;
            CurrentServerData.description = configForm.serverDescriptionTextBox.Text;
            CurrentServerData.pictureBytes = JpegImage.GetBytes(configForm.image);
            CurrentServerData.sensors = new SensorDescriptionsData();
            InitialiseLogbook();
            ConfirmSensors();
            
            DatabaseHelper.SaveServerConfigData(database, CurrentServerData);

            RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(SensorShareConfig.ConfigKeyName);
            registryKey.SetValue("ServerID", CurrentServerData.id.ToString());
            registryKey.Close();

            StartServer();
         }
      }

      private void ConfirmSensors()
      {
         log.Append("Confirming sensors - connected sensors are:");
         foreach (SensorDefinition sensor in CurrentServerData.sensors)
         {
            log.Append(sensor.Description);
         }
         log.Append("End of sensors");

         if (CurrentServerData.sensors.NoneConnected())
         {
            log.Append("No sensors connected!");
            DialogResult result = MessageBox.Show("No sensors are currently identified, would you like to connect to a Logbook now? (Otherwise dummy sensors will be used.)", "No Sensors Connected",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
               ConnectLogbookAndIdentifySensors();
               if (CurrentServerData.sensors.NoneConnected())
               {
                  log.Append("Still no sensors connected!");

                  result = MessageBox.Show("The connection to the Logbook could not be established or no sensors are connected, try manual connection?", "Connection Problem",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                  if (result == DialogResult.Yes)
                  {
                     log.Append("Using manual connection!");
                     ConnectLogbookForm connectForm = new ConnectLogbookForm(logbook);
                     connectForm.ShowDialog();
                  }
                  if (CurrentServerData.sensors.NoneConnected())
                  {
                     log.Append("Continuing with no sensors...");
                     CurrentSensors = new SensorDefinition[4];
                     CurrentServerData.sensors.Clear();
                     for (int i = 0; i < 4; i++)
                     {
                        CurrentServerData.sensors.Add(new SensorDefinition());
                        CurrentSensors[i] = CurrentServerData.sensors[i];
                     }
                  }
               }
            }
            else
            {
               log.Append("Continuing with dummy sensors");
               CurrentServerData.sensors.Clear();
               for (int i = 0; i < 4; i++)
               {
                  CurrentServerData.sensors.Add(new SensorDefinition());
               }
            }
         }
      }

      private void startReadingsMenuItem_Click(object sender, EventArgs e)
      {
          StartTakingReadings();
      }

      private void DoUpdateTextBox(TextBox box, String text, int lines)
      {
          if (!box.IsDisposed)
          {
              if (box.InvokeRequired)
              {
                  box.Invoke(textboxDelegate, box, text, lines);
              }
              else
              {
                  textboxDelegate(box, text, lines);
              }
          }
      }
   }
}