using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SensorShare.Network
{
   public partial class NetworkNode
   {
      private Guid localID;
      private IPAddress listenIP;
      private int port;
      private AutoResetEvent messageReceivedEvent;
      private AutoResetEvent messageToSendEvent;
      private AutoResetEvent directUdpEvent;

      private bool keepRecieving = false;
      private bool keepSending = false;
      private Queue<NetworkMessage> ReceiveQueue = new Queue<NetworkMessage>();
      private Queue<NetworkMessage> SendQueue = new Queue<NetworkMessage>();

      private UdpClient udpSendClient;
      private UdpClient udpReceiveClient;

      private Queue<MessageRequest> directUdpTargets = new Queue<MessageRequest>();

      private Queue<TcpClient> incomingClients = new Queue<TcpClient>();

      public event MessageEventHandler MessageReceived;
      public event MessageEventHandler OtherMessageReceived;


      /// <summary>
      /// When all message fragments have been received this event is fired
      /// </summary>
      protected event MessageDefragmentedEventHandler MessageDefragmented;

      /// <summary>
      /// If all message fragments are not received before it's expiry time this event is fired
      /// </summary>
      protected event MessageMissedEventHandler MessageMissed;

      /// <summary>
      /// The size of message fragments to make
      /// </summary>
      static int FragmentSize = 500;

      Predicate<TickGuid> earlierThanNow;
      
      public NetworkNode(Guid localID, IPAddress listenIP, int port)
      {
         this.localID = localID;
         this.port = port;
         this.listenIP = listenIP;

         earlierThanNow = new Predicate<TickGuid>(CompareTime);
         
         messageReceivedEvent = new AutoResetEvent(false);
         messageToSendEvent = new AutoResetEvent(false);
         directUdpEvent = new AutoResetEvent(false);

         this.MessageDefragmented += new MessageDefragmentedEventHandler(NetworkNode_MessageDefragmented);
      }

      ~NetworkNode()
      {
         if (keepRecieving)
         {
            keepRecieving = false;
            messageReceivedEvent.Set();
         }
         if (keepSending)
         {
            keepSending = false;
            messageToSendEvent.Set();
         }
         if (udpReceiveClient != null)
         {
            udpReceiveClient.Close();
         }
         if (udpSendClient != null)
         {
            udpSendClient.Close();
         }
      }

      public void Start()
      {
         keepRecieving = true;
         Thread receiveThread = new Thread(new ThreadStart(ReceiveThread));
         receiveThread.Start();
         keepSending = true;
         Thread sendThread = new Thread(new ThreadStart(SendThread));
         sendThread.Start();
         Thread receiveProcessThread = new Thread(new ThreadStart(ReceiveProcessThread));
         receiveProcessThread.Start();
         Thread directUdpThread = new Thread(new ThreadStart(DirectUdpThread));
         directUdpThread.Start();
      }

      public void Stop()
      {
         keepSending = false;
         keepRecieving = false;
         messageReceivedEvent.Set();
         messageToSendEvent.Set();
         if (messageLifetimeTimer != null)
         {
            messageLifetimeTimer.Dispose();
            messageLifetimeTimer = null;
         }
         if (udpSendClient != null)
         {
            udpSendClient.Close();
            udpSendClient = null;
         }
         if (udpReceiveClient != null)
         {
            udpReceiveClient.Close();
            udpReceiveClient = null;
         }
         directUdpEvent.Set();
      }

      public void Send(byte[] bytes)
      {
         Send(bytes, Guid.Empty);
      }

      public void Send(byte[] bytes, Guid destinationID)
      {
         NetworkMessage message = new NetworkMessage(localID, destinationID, Guid.NewGuid(), NetworkMessageType.Message, bytes, 1, 1, 50000);
         if (message.GzData.Length > FragmentSize)
         {
            // This is a large message, break into fragments
            SendFragments(bytes, destinationID);
         }
         else
         {
            Debug.WriteLine("Created message " + message.MessageID.ToString());
            sendMessage(message);
         }
      }

      public void SendDirect(byte[] data, Guid destinationID)
      {
         Guid messageID = Guid.NewGuid();
         NetworkMessage[] fragments = null;
         TickGuid expiryTime = new TickGuid();
         expiryTime.id = messageID;

         NetworkMessage message = new NetworkMessage(localID, destinationID, messageID, NetworkMessageType.Message,
            data, 1, 1, 10000);

         if (message.GzData.Length > FragmentSize)
         {
            message.Lifetime = 30000;
            //Break the message up if it's large
            fragments = this.Fragment(message);
            expiryTime.SetExpityTime(message.Lifetime);
         }
         else
         {
            fragments = new NetworkMessage[1];
            fragments[0] = message;
            expiryTime.SetExpityTime(message.Lifetime);
         }

         Debug.WriteLine(String.Format("Adding {0} to store in {1} fragments", messageID, fragments.Length));
         outgoingFragments[messageID] = fragments;
         messageExpiryTimes.Add(expiryTime);

         // make the lifetime of the direct request message 5 seconds
         NetworkMessage directRequestMessage = new NetworkMessage(localID, destinationID, Guid.NewGuid(), NetworkMessageType.DirectRequest, messageID.ToByteArray(), 1, 1, 5000);
         sendMessage(directRequestMessage);
         startMessageLifetimeTimer();
      }

      public void SendFragments(byte[] data, Guid destinationID)
      {
         Guid messageID = Guid.NewGuid();
         Debug.WriteLine("Created message " + messageID.ToString());
         // Make the lifetime of the message 30 seconds
         NetworkMessage largeMessage = new NetworkMessage(localID, destinationID, messageID, NetworkMessageType.Message,
            data, 1,1, 300000);
         NetworkMessage[] fragments = this.Fragment(largeMessage);
         outgoingFragments[largeMessage.MessageID] = fragments;
         //double the lifetime in send cache so that receivers have a chance to request fragments within one timespan before it's expired
         messageExpiryTimes.Add(new TickGuid(largeMessage.MessageID, (largeMessage.Lifetime*2)));


         Debug.WriteLine("Message fragmented in " + fragments.Length + " fragments.");
         foreach (NetworkMessage message in fragments)
         {
            sendMessage(message);
            Thread.Sleep(0);
         }
         startMessageLifetimeTimer();

      }

      private void sendMessage(NetworkMessage message)
      {
         SendQueue.Enqueue(message);
         messageToSendEvent.Set();
      }

      private void ReceiveThread()
      {
         try
         {
            udpReceiveClient = new UdpClient(port, AddressFamily.InterNetwork);
            IPEndPoint receivedFrom = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = new byte[100000];
            while (keepRecieving)
            {
               receivedBytes = udpReceiveClient.Receive(ref receivedFrom);
               int receivedLength = receivedBytes.Length;
               if (receivedLength > 36)
               {
                  NetworkMessage message = new NetworkMessage(receivedBytes);
                  Debug.WriteLine(String.Format("Received {0} from {1}", message.Type, receivedFrom));
                  switch (message.Type)
                  {
                     case NetworkMessageType.Message:
                        ReceiveQueue.Enqueue(message);
                        messageReceivedEvent.Set();
                        break;
                     case NetworkMessageType.DirectRequest:
                        if (message.DestinationID == this.localID)
                        {
                           Debug.WriteLine("Received direct request for " + message.DestinationID.ToString());
                           NetworkMessage replyMessage = new NetworkMessage(this.localID, message.SenderID, Guid.NewGuid(), NetworkMessageType.DirectReply, message.Data, 1, 1, 50000);
                           sendMessage(replyMessage);
                        }
                        break;
                     case NetworkMessageType.DirectReply:
                        if (message.DestinationID == localID)
                        {
                           Debug.WriteLine("Recieved reply for " + message.DestinationID.ToString());
                           MessageRequest messageReq = new MessageRequest();
                           messageReq.Address = receivedFrom.Address;
                           messageReq.MessageID = new Guid(message.Data);
                           messageReq.Fragment = -1;
                           directUdpTargets.Enqueue(messageReq);
                           directUdpEvent.Set();
                        }
                        break;
                     case NetworkMessageType.FragmentRequest:
                        if (message.SenderID != localID)
                        {
                           Guid messageRequestID = new Guid(message.Data);
                           Debug.WriteLine(String.Format("Request for resend of message {0} fragment {1}", messageRequestID, message.FragmentNumber));
                           MessageRequest fragmentReq = new MessageRequest();
                           fragmentReq.Address = receivedFrom.Address;
                           fragmentReq.MessageID = new Guid(message.Data);
                           fragmentReq.Fragment = message.FragmentNumber;
                           directUdpTargets.Enqueue(fragmentReq);
                           directUdpEvent.Set();
                           Thread.Sleep(0);
                        }
                        break;
                  }
               }
            }
         }
         catch (SocketException ex)
         {
            Debug.WriteLine(String.Format("SocketException: {0} {1}\r\n{2}", ex.ErrorCode, ex.Message, ex.StackTrace));
         }
      }

      private void SendThread()
      {
         udpSendClient = new UdpClient();

         while (keepSending)
         {
            try
            {
               messageToSendEvent.WaitOne();
               while (SendQueue.Count > 0)
               {
                  NetworkMessage message = SendQueue.Dequeue();
                  byte[] bytes = message.GetBytes();
                  Debug.WriteLine(String.Format("Sending {0} bytes", bytes.Length));
                  udpSendClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, port));
               }
            }
            catch (SocketException ex)
            {
               Debug.WriteLine(String.Format("SocketException: {0} {1}\r\n{2}", ex.ErrorCode, ex.Message, ex.StackTrace));
            }
            catch (ObjectDisposedException ex)
            { }
         }
      }


      private void DirectUdpThread()
      {
         udpSendClient = new UdpClient();

         while (keepSending)
         {
            try
            {
               directUdpEvent.WaitOne();
               if (directUdpTargets.Count>0)
               {
                  MessageRequest target = directUdpTargets.Dequeue();
                  Debug.WriteLine(String.Format("Ready to send {0} to {1}", target.MessageID, target.Address));

                  if (outgoingFragments.ContainsKey(target.MessageID))
                  {
                     NetworkMessage[] messages = (NetworkMessage[])outgoingFragments[target.MessageID];
                     if (target.Fragment < 0)
                     {
                        foreach (NetworkMessage message in messages)
                        {
                           byte[] bytes = message.GetBytes();
                           Debug.WriteLine(String.Format("Sending {0} bytes", bytes.Length));
                           udpSendClient.Send(bytes, bytes.Length, new IPEndPoint(target.Address, port));
                        }
                        if (messages.Length == 1)
                        {
                           outgoingFragments.Remove(target.MessageID);
                        }
                        RemoveExpiry(target.MessageID);
                     }
                     else
                     {
                        byte[] bytes = messages[target.Fragment].GetBytes();
                        Debug.WriteLine(String.Format("Sending {0} bytes", bytes.Length));
                        udpSendClient.Send(bytes, bytes.Length, new IPEndPoint(target.Address, port));
                     }
                  }
                  else
                  {
                     Debug.WriteLine(String.Format("Can't find message {0} to send", target.MessageID));
                  }
               }
            }
            catch (SocketException ex)
            {
               Debug.WriteLine(String.Format("SocketException: {0} {1}\r\n{2}", ex.ErrorCode, ex.Message, ex.StackTrace));
               //Restart a sending thread if it's supposed to keep going...
               if (keepSending)
               {
                  Thread directUdpThread = new Thread(new ThreadStart(DirectUdpThread));
                  directUdpThread.Start();
               }
            }
            catch (ObjectDisposedException ex)
            { }
         }
      }

      public void ReceiveProcessThread()
      {
         while (keepRecieving)
         {
            messageReceivedEvent.WaitOne();
            while (ReceiveQueue.Count > 0)
            {
               NetworkMessage message = ReceiveQueue.Dequeue();
               Debug.WriteLine(String.Format("Message from {0} to {1}", message.SenderID, message.DestinationID));
               if (message.SenderID != this.localID)
               {
                  if (message.FragmentTotal == 1)
                  {
                     if ((message.DestinationID == this.localID) || (message.DestinationID == Guid.Empty))
                     {
                        FireMessageReceived(message);
                     }
                     else
                     {
                        FireOtherMessageReceived(message);
                     }
                  }
                  else
                  {
                     this.Defragment(message);
                  }
               }
            }
         }
      }

      public void FireMessageReceived(NetworkMessage message)
      {
         if (this.MessageReceived != null)
         {
            MessageReceived(this, new MessageEventArgs(message));
         }
      }

      public void FireOtherMessageReceived(NetworkMessage message)
      {
         if (this.OtherMessageReceived != null)
         {
            OtherMessageReceived(this, new MessageEventArgs(message));
         }
      }

      private struct MessageRequest
      {
         public IPAddress Address;
         public Guid MessageID;
         public int Fragment;
      }
   }
}
