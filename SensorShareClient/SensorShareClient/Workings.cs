using System;
//using OpenNETCF.Net;
using System.Net;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
using SensorShare.Network;
using OpenNETCF.WindowsCE;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using mcp;
using mcp.Logs;
using mcp.Compact;

namespace SensorShare.Compact
{
   public partial class SensorShareClient
   {
      VoidDelegate closeDelegate;

      NetworkNode networkNode;
      MessageEventHandler receiveHandler;
      DeviceNotification connectionHandler;

      Guid ClientID;
      Log log = new Log();
      ObjectCache aliveServers = null;
      Hashtable serverTimeDifferences = new Hashtable();

      private void SetUpWorkings()
      {
         closeDelegate = new VoidDelegate(this.Close);
         connectionHandler = new DeviceNotification(DeviceManagement_NetworkConnected);

         InitialiseWiFi();
         
         log.Name = "log";
         log.LogMessage += new LogMessageEventHandler(log_LogMessage);
         log.Start();

         DeviceManagement.NetworkConnected += connectionHandler;

         aliveServers = new ObjectCache(7000);
         receiveHandler = new MessageEventHandler(networkNode_MessageReceived);
         aliveServers.ItemExpired += new ItemExpiredEventHandler(aliveServers_ItemExpired);

         ClientID = Guid.NewGuid();
         networkNode = new NetworkNode(ClientID, IPAddress.Any, SensorShareConfig.CommunicationPort);
         networkNode.MessageReceived += receiveHandler;
      }

      private void StartClient()
      {
         networkNode.Start();
      }

      private void StopClient()
      {
         networkNode.Stop();
      }

      private void log_LogMessage(object sender, LogMessageEventArgs e)
      {
         Debug.WriteLine(e.Message);
         log.WriteToSQL(database, e.Message);
      }

      private void ProcessAliveMessage(Guid guid, byte[] data)
      {
         lock (aliveServers)
         {
            log.Append(String.Format("Checking cache for {0}", guid.ToString()));
            object cacheItem = aliveServers.GetItem(guid);
            if (cacheItem == null)
            {
               log.Append("ProcessAliveMessage", String.Format("Not in cache {0}", guid.ToString()));
               //MessageBox.Show(String.Format("New server detected {0}", guid.ToString()));
               NewAliveServer(guid);
            }
            else
            {
               log.Append("ProcessAliveMessage", String.Format("Server in cache {0}", guid.ToString()));
               aliveServers.FreshenItem(guid);
            }
            long remoteTime = BitConverter.ToInt64(data, 0);
            long localTime = DateTime.UtcNow.Ticks;
            log.Append("ProcessAliveMessage", String.Format("Remote clock correction: R[{0}] L[{1}]", remoteTime, localTime));
            serverTimeDifferences[guid] = remoteTime - localTime;
            log.Append("ProcessAliveMessage", String.Format("...{0}", (long)serverTimeDifferences[guid]));
         }
      }

      private void aliveServers_ItemExpired(ItemExpiredEventArgs args)
      {
         foreach (ServerData data in args.Items)
         {
            log.Append("aliveServers_ItemExpired", "Removed " + data.id.ToString());
            serverTimeDifferences.Remove(data.id);
            serverListView1.Remove(data.id);
         }
      }

      private void NewAliveServer(Guid server_id)
      {
         ServerData server_data = DatabaseHelper.GetServerByID(database, server_id);
         if (server_data == null)
         {
            log.Append(String.Format("Sending description request for {0}", server_id));
            TypedMessage requestMessage = new TypedMessage(MessageType.DescriptionRequest, ClientID.ToByteArray());
            networkNode.SendDirect(requestMessage.GetBytes(), server_id);
         }
         else
         {
            log.Append(String.Format("Using database description for {0}", server_id));
            AddAliveServer(server_data);
         }
      }

      private void AddAliveServer(ServerData server)
      {
         log.Append(String.Format("Showing {0}:{1}", server.name, server.id));
         aliveServers.AddItem(server, server.id);
         serverListView1.Add(server);
      }

      private void ProcessDescriptionMessage(Guid sender_id, TypedMessage message)
      {
         log.Append("ProcessDescriptionMessage", String.Format("Description from {0}", sender_id.ToString()));
         DescriptionMessage description = new DescriptionMessage(message.data);
         if (aliveServers.GetItem(sender_id) == null)
         {
            bool alreadySaved = DatabaseHelper.GetServerSaved(database, sender_id);
            ServerData serverData = new ServerData(sender_id, description.Name, description.Location, description.Description, description.Sensors, description.PictureBytes);
            if (alreadySaved == false)
            {
               DatabaseHelper.SaveServerConfigData(database, serverData);
            }
            AddAliveServer(serverData);
         }
         log.Append("ProcessDescriptionMessage", String.Format("Description from {0} at {1}", description.Name, sender_id.ToString()));
         //MessageBox.Show(String.Format("Description from {0} at {1}", description.Name, sender.ToString()));
      }


      private void SensorNetClient_Closing(object sender, CancelEventArgs e)
      {
         StopClient();
      }

      private void SensorNetClient_Closed(object sender, EventArgs e)
      {
         Debug.WriteLine("Application closing");
         log.Stop();
         Application.Exit();
      }

   }
}