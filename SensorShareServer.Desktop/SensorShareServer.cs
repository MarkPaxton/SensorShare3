using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mcp.Logs;
using System.IO;
using Microsoft.Win32;
using System.Data.SQLite;
using SensorShare.Network;
using System.Diagnostics;
using System.Net;

namespace SensorShare.Desktop
{
   public partial class SensorNetServer : Form
   {
      VoidDelegate closeDelegate;

      SQLiteConnection database = null;
      NetworkNode networkNode;

      Log log = new Log();

      ServerConfigData CurrentServerData = null;

      Boolean serverIsStarted = false;
      System.Threading.Timer aliveTimer = null;

      VoidDelegate descriptionUpdater;

      public SensorNetServer()
      {
         InitializeComponent();

         descriptionUpdater = new VoidDelegate(UpdateServerDescription);
         closeDelegate = new VoidDelegate(this.Close);
         
         log.Name = "log";
         log.LogMessage += new LogMessageEventHandler(log_LogMessage);
         log.Start();
         #region copy data files

         if (!Directory.Exists(Application.UserAppDataPath))
         {
            Directory.CreateDirectory(Application.UserAppDataPath );
         }
         if (!File.Exists(Application.UserAppDataPath + "\\" + SensorNetConfig.ServerDatabase))
         {
            File.Copy(Application.StartupPath + "\\" + SensorNetConfig.ServerDatabase,
               Application.UserAppDataPath + "\\" + SensorNetConfig.ServerDatabase);
         }
         database = DatabaseHelper.ConnectToSQL("Data Source=\"" + Application.UserAppDataPath + 
            "\\" + SensorNetConfig.ServerDatabase + "\"");
         #endregion

         #region Regestry data setup

         RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(SensorNetConfig.ConfigKeyName);

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
         }

         if (createNewConfig)
         {
            // Show server config before starting server (which is automatically done when config box is closed)
            Bitmap serverPic = new Bitmap(Application.StartupPath + "/defaultImage.jpg");
            CurrentServerData = new ServerConfigData(Guid.Empty, "New Server", "Location", "Description", JpegImage.GetBytes(serverPic));
            configMenuItem_Click(this, new EventArgs());
         }
         else
         {
            //If config isn't needed, start now
            registryKey.Close();
            StartServer();
         }
         #endregion

      }

      void log_LogMessage(object sender, LogMessageEventArgs e)
      {
         Debug.WriteLine(e.Message);
         DatabaseHelper.SaveLogMessage(database, e.Message);
      }

      void networkNode_MessageReceived(object sender, MessageEventArgs args)
      {
         log.Append("networkNode_MessageReceived", String.Format("Message Received from {0}", args.SenderID.ToString()));
         TypedMessage message = new TypedMessage(args.Data, args.Data.Length);
         switch (message.type)
         {
            case MessageType.DescriptionRequest:
               log.Append("networkNode_MessageReceived", "Description Request received");
               DescriptionMessage description = new DescriptionMessage(CurrentServerData);
               TypedMessage reply = new TypedMessage(MessageType.DescriptionMessage, description.GetBytes());
               networkNode.SendDirect(reply.GetBytes(), args.SenderID);
               break;
            default:
               break;
         }
      }

      private void SendAliveMessage(object ob)
      {
         TypedMessage message = new TypedMessage(MessageType.AliveMessage, BitConverter.GetBytes((Int64)DateTime.UtcNow.Ticks));
         networkNode.Send(message.GetBytes());
      }

      private void menuItem2_Click(object sender, EventArgs e)
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

      private void SensorNetServer_FormClosing(object sender, FormClosingEventArgs e)
      {
         log.Append("SensorNetServer_Closed", "Application closing");
         StopServer();
      }

      private void SensorNetServer_Closed(object sender, EventArgs e)
      {
         log.Stop();
         Debug.WriteLine("Application closing");
         Application.Exit();
      }

      private void configMenuItem_Click(object sender, EventArgs e)
      {
         ServerConfig configForm = new ServerConfig();
         configForm.serverNameTextBox.Text = CurrentServerData.name;
         configForm.serverLocationTextBox.Text = CurrentServerData.location;
         configForm.serverDescriptionTextBox.Text = CurrentServerData.description;
         configForm.image = CurrentServerData.Picture();
         configForm.selectImageDialog.InitialDirectory = Application.StartupPath;

         if (configForm.ShowDialog() == DialogResult.OK)
         {
            StopServer();
            CurrentServerData.id = Guid.NewGuid();
            CurrentServerData.name = configForm.serverNameTextBox.Text;
            CurrentServerData.location = configForm.serverLocationTextBox.Text;
            CurrentServerData.description = configForm.serverDescriptionTextBox.Text;
            CurrentServerData.pictureBytes = JpegImage.GetBytes(configForm.image);
            //serverDescriptionMessage = new DescriptionMessage(CurrentServerData);

            DatabaseHelper.SaveServerConfigData(database, CurrentServerData);

            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(SensorNetConfig.ConfigKeyName);
            registryKey.SetValue("ServerID", CurrentServerData.id.ToString());
            registryKey.Close();

            StartServer();
         }
      }

      private void StartServer()
      {
         if (!serverIsStarted)
         {
            if (networkNode != null)
            {
               networkNode = null;
            }
            networkNode = new NetworkNode(CurrentServerData.id, IPAddress.Any, SensorNetConfig.CommunicationPort);
            aliveTimer = new System.Threading.Timer(new System.Threading.TimerCallback(SendAliveMessage), null, 0, SensorNetConfig.AliveTimeout);

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

      private void UpdateServerDescription()
      {
         serverNameLabel.Text = CurrentServerData.name;
      }

      private void StopServer()
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
            log.Stop();
         }
      }

   }
}