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
using System.Diagnostics;
using SensorShare;
using Microsoft.Win32;
using System.IO;
using OpenNETCF.Windows.Forms;
using SensorShare.Compact;
using System.Data.SQLite;
using OpenNETCF.WindowsCE;
using mcp;
using mcp.Logs;
using mcp.Compact;
using ScienceScope;
using System.Threading;

namespace SensorShare.Compact
{
   public partial class SensorShareServer
   {
      DeviceNotification connectionHandler;
      NetworkNode networkNode;

      VoidDelegate descriptionUpdater;

      Log log = new Log();
      Boolean serverIsStarted = false;
      System.Threading.Timer aliveTimer = null;

      public void SetUpWorkings()
      {
         descriptionUpdater = new VoidDelegate(UpdateServerDescription);
         connectionHandler = new DeviceNotification(DeviceManagement_NetworkConnected);
         
         InitialiseWiFi();

         log.Name = "log";
         log.LogMessage += new LogMessageEventHandler(log_LogMessage);
         log.Start();

         #region Regestry data setup

         RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(SensorShareConfig.ConfigKeyName);

         bool createNewConfig = true;

         object regKey = registryKey.GetValue("ServerID");

         if (regKey != null)
         {
            Guid serverID = new Guid((string)regKey);
            CurrentServerData = DatabaseHelper.GetServerByID(database, serverID);
            if (CurrentServerData != null)
            {
               createNewConfig = false;
            }
            else
            {
               log.Append("Server ID saved in Registry not found");
            }
         }

#endregion

         #region Server Configuration Setup

         if (createNewConfig)
         {
            log.Append("Need to create a new server configuration");
            // Show server config before starting server (which is automatically done when config box is closed)
            Bitmap serverPic = new Bitmap(Application2.StartupPath + "/defaultImage.jpg");
            CurrentServerData = new ServerData(Guid.Empty, "New Server", "Location", "Description", new SensorDescriptionsData(), JpegImage.GetBytes(serverPic));
            configMenuItem_Click(this, new EventArgs());
         }
         else
         {
            //If config isn't needed, start now
            registryKey.Close();
            log.Append("Starting with server: " + CurrentServerData.id.ToString() + " " + CurrentServerData.name);

            // Save sensor data laoded from database (past session)
            SensorDescriptionsData loadedSensors = new SensorDescriptionsData();
            foreach (SensorDefinition sensor in CurrentServerData.sensors)
            {
               loadedSensors.Add(sensor);
            }
            //Clear current data as logbook isn't connected yet so sensors are unknown
            CurrentServerData.sensors.Clear();
            InitialiseLogbook(); // Set up logbook data

            // Connect Logbook and send identify commands (reply comes on other threads
            ConnectLogbookAndIdentifySensors();
            // Should have done Thread.Sleep in above method to allow reply to have come by now
            // Check sensors match the database version
            ConfirmSensors();

            bool sensorSelectionNotCompleted = true;
            while (sensorSelectionNotCompleted)
            {
               // check that the sensors connected are the same
               bool sameSensorsConnected = true;
               lock (CurrentServerData.sensors)
               {
                  if (loadedSensors.Count != CurrentServerData.sensors.Count)
                  {
                     sameSensorsConnected = false;
                  }
                  else
                  {

                     for (int s = 0; s < loadedSensors.Count; s++)
                     {
                        if ((loadedSensors[s].ID != CurrentServerData.sensors[s].ID) || (loadedSensors[s].Range != CurrentServerData.sensors[s].Range))
                        {
                           sameSensorsConnected = false;
                        }
                     }

                  }
               }
               if (!sameSensorsConnected)
               {
                  DialogResult result = MessageBox.Show("The sensors currently attached to the Logbook do not match the previously saved sensors.  Click Retry to change the sensors, or Cancel to create a new server configuration.", "Different Sensors Connected",
                  MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                  if (result == DialogResult.Retry)
                  {
                     disconnectLogbook();
                     StringBuilder sb = new StringBuilder();
                     sb.Append("You need to connect the following sensors:\r\n");
                     foreach (SensorDefinition sensor in loadedSensors)
                     {
                        sb.AppendFormat("{0}\r\n", sensor.Description);
                     }
                     MessageBox.Show(sb.ToString(), "Sensors Required", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                     CurrentServerData.sensors.Clear();
                     ConnectLogbookAndIdentifySensors();
                     ConfirmSensors();
                  }
                  else
                  {
                     if (result == DialogResult.Cancel)
                     {
                        CurrentServerData.id = Guid.NewGuid();
                        DatabaseHelper.SaveServerConfigData(database, CurrentServerData);
                        sensorSelectionNotCompleted = false;
                     }
                  }
               }
               else
               {
                  sensorSelectionNotCompleted = false;
               }
            }
            StartServer();
         }
         #endregion
         
         DeviceManagement.NetworkConnected += connectionHandler;
      }

      void log_LogMessage(object sender, LogMessageEventArgs e)
      {
         Debug.WriteLine(e.Message);
         log.WriteToSQL(database, e.Message);
         DoUpdateTextBox(logTextBox, e.Message, 3);
      }

      private void SendAliveMessage(object ob)
      {
         TypedMessage message = new TypedMessage(MessageType.AliveMessage, BitConverter.GetBytes((Int64) DateTime.UtcNow.Ticks));
         networkNode.Send(message.GetBytes());
      }

      private void SensorNetServer_Closed(object sender, EventArgs e)
      {
         log.Stop();
         Debug.WriteLine("Application closing");
         Application.Exit();
      }

      private void StartServer()
      {
         if (!serverIsStarted)
         {
            if (networkNode != null)
            {
               networkNode = null;
            }
            networkNode = new NetworkNode(CurrentServerData.id, IPAddress.Any, SensorShareConfig.CommunicationPort);
            aliveTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SendAliveMessage), null, 0, SensorShareConfig.AliveTimeout);
            
            networkNode.MessageReceived += new MessageEventHandler(networkNode_MessageReceived);
            networkNode.Start();

            serverIsStarted = true;
            MessageBox.Show(String.Format("Started for {0}", CurrentServerData.id));
            log.Append("StartServer", String.Format("Started for {0}", CurrentServerData.id));
         }
         if (this.InvokeRequired)
         {
            this.Invoke(descriptionUpdater);
         }
         else
         {
            UpdateServerDescription();
         }
      }

      private void StopWorkings()
      {
         if (serverIsStarted)
         {
            log.Append("StopServer", String.Format("Server {0} is being stopped", CurrentServerData.id.ToString()));
            if (aliveTimer != null)
            {
               aliveTimer.Dispose();
               aliveTimer = null;
            }
            if (networkNode != null)
            {
               networkNode.Stop();
            }
            serverIsStarted = false;
         }
      }

      private void UpdateServerDescription()
      {
         serverNameLabel.Text = CurrentServerData.name;
         StringBuilder sb = new StringBuilder("Sensors:\r\n");
         for (int s = 0; s < CurrentServerData.sensors.Count; s++)
         {
            sb.AppendFormat("[{0}]{1}\r\n", s + 1, CurrentServerData.sensors[s].Description);
         }
         sensorDescriptionsLabel.Text = sb.ToString();
         Bitmap pic = JpegImage.GetThumbnail(CurrentServerData.Picture(), serverPictureBox.Size);
         serverPictureBox.Image = pic;
      }
   }
}