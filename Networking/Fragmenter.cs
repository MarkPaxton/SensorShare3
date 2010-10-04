using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SensorShare.Network
{
   partial class NetworkNode
   {
      /// <summary>
      /// The fragments for each message yet to be received
      /// </summary>
      private Hashtable messageFragmentsRequired = new Hashtable();

      /// <summary>
      /// Fragments the given message into a number of smaller messages for sending over the network
      /// </summary>
      /// <returns>An array of network mesages belonging which are the fragments of the given message</returns>
      public NetworkMessage[] Fragment(NetworkMessage message)
      {
         List<byte[]> fragments = new List<byte[]>();
         NetworkMessage[] messages;
         int numRead = 1;
         using (MemoryStream ms = new MemoryStream(message.GzData))
         {
            while (numRead > 0)
            {
               byte[] readBytes = new byte[FragmentSize];
               numRead = ms.Read(readBytes, 0, FragmentSize);
               if (numRead > 0)
               {
                  if (numRead < FragmentSize)
                  {
                     byte[] fragData = new byte[numRead];
                     Array.Copy(readBytes, fragData, numRead);
                     fragments.Add(fragData);
                  }
                  else
                  {
                     fragments.Add(readBytes);
                  }
               }
            }
            messages = new NetworkMessage[fragments.Count];
            for (int i = 0; i < fragments.Count; i++)
            {
               NetworkMessage fragmentMessage = new NetworkMessage(message.SenderID, message.DestinationID, message.MessageID, message.Type,
                  fragments[i], i, fragments.Count, message.Lifetime);
               messages[i] = fragmentMessage;
            }
         }
         return messages;
      }


      /// <summary>
      /// Stores a message fragment, if all the fragments of a message are stored MessageDefragmented event is fired
      /// </summary>
      public void Defragment(NetworkMessage message)
      {
         NetworkMessage[] buffer;
         List<int> fragmentsNeededList = null;
         Debug.WriteLine("Fragment: " + message.FragmentNumber + " size: " + message.Data.Length);
         if (incomingFragmentBuffer.ContainsKey(message.MessageID))
         {
            // first fragment has already been received so add to existing buffer
            buffer = (NetworkMessage[])incomingFragmentBuffer[message.MessageID];
         }
         else
         {
            // This is the first fragement of a message received, so create a new buffer
            buffer = new NetworkMessage[message.FragmentTotal];
            messageFragmentsRequired[message.MessageID] = new List<int>(message.FragmentTotal);
            for (int i = 0; i < message.FragmentTotal; i++)
            {
               ((List<int>) messageFragmentsRequired[message.MessageID]).Add(i);
            }
            messageExpiryTimes.Add(new TickGuid(message.MessageID, message.Lifetime));
         }
         fragmentsNeededList = (List<int>) messageFragmentsRequired[message.MessageID];
         buffer[message.FragmentNumber] = message;
         incomingFragmentBuffer[message.MessageID] = buffer;

         fragmentsNeededList.Remove(message.FragmentNumber);


         if (fragmentsNeededList.Count == 0)
         {
            byte[] messageData;
            using (MemoryStream ms = new MemoryStream())
            {
               for (int j = 0; j < buffer.Length; j++)
               {
                  ms.Write(buffer[j].Data, 0, buffer[j].Data.Length);
               }
               messageData = new byte[ms.Length];
               messageData = ms.ToArray();
            }
            MessageDefragmented(this, new MessageDefragmentedEventArgs(buffer[0].DestinationID, buffer[0].SenderID, buffer[0].MessageID, messageData));
            incomingFragmentBuffer.Remove(message.MessageID);
            messageFragmentsRequired.Remove(message.MessageID);
         }
         else
         {
            startMessageLifetimeTimer();
         }
      }

      void NetworkNode_MessageDefragmented(object sender, MessageDefragmentedEventArgs args)
      {
         Debug.WriteLine("Message " + args.MessageID + " defragmented");
         RemoveExpiry(args.MessageID);
         MessageReceived(this, new MessageEventArgs(args.SenderID, args.DestinationID, args.GzData));
      }

   }
}
