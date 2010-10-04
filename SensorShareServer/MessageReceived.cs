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

namespace SensorShare.Compact
{
   public partial class SensorShareServer
   {
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

   }
}