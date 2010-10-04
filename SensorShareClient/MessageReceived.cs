using System;
//using OpenNETCF.Net;
using System.Net;
using System.Windows.Forms;
using OpenNETCF.Net.NetworkInformation;
using SensorShare.Network;

namespace SensorShare.Compact
{
   public partial class SensorShareClient
   {
      void networkNode_MessageReceived(object sender, MessageEventArgs args)
      {
         try
         {
            log.Append("networkNode_MessageReceived", String.Format("Message Received from {0}", args.SenderID.ToString()));
            TypedMessage message = new TypedMessage(args.Data, args.Data.GetLength(0));
            switch (message.type)
            {
               case MessageType.AliveMessage:
                  ProcessAliveMessage(args.SenderID, message.data);
                  break;
               case MessageType.DescriptionMessage:
                  ProcessDescriptionMessage(args.SenderID, message);
                  break;
               default:
                  break;
            }
         }
         catch (Exception ex)
         {
            log.LogException(ex, "Exception when processing message");
            MessageBox.Show(ex.Message, "Exception :(");
         }
      }      
   }
}