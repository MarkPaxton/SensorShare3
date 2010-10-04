using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SensorShare.Network
{
   partial class NetworkNode
   {
      /// <summary>
      /// Contains a message time and the tick time it expires
      /// </summary>
      class TickGuid
      {
         public long tick;
         public Guid id;

         public TickGuid()
         { }

         /// <summary>
         /// Create a TickGuid
         /// </summary>
         /// <param name="id">Guid</param>
         /// <param name="milliseconds">milliseconds from now for value of tick</param>
         public TickGuid(Guid id, long milliseconds)
         {
            this.id = id;
            this.tick = DateTime.Now.Ticks + (milliseconds * 10000);
         }

         public void SetExpityTime(int millisecs)
         {
            this.tick = DateTime.Now.Ticks + (millisecs * 10000);
         }
      }

      /// <summary>
      /// A list of the tick times that messages should be kept until
      /// </summary>
      List<TickGuid> messageExpiryTimes = new List<TickGuid>();

      /// <summary>
      /// Contains a array of all message fragments in the cache against their ids
      /// </summary>
      Hashtable incomingFragmentBuffer = new Hashtable();

      Hashtable outgoingFragments = new Hashtable();

      bool CompareTime(TickGuid x)
      {
         return x.tick <= DateTime.Now.Ticks;
      }

      private Timer messageLifetimeTimer = null;

      private void startMessageLifetimeTimer()
      {
         if (messageLifetimeTimer == null)
         {
            messageLifetimeTimer = new Timer(new TimerCallback(MessageLifetimeTimerTimeout), null, 1000, 1000);
         }
      }

      private void MessageLifetimeTimerTimeout(object ob)
      {
         Debug.WriteLine("MessageLifetimeTimerTimeout happened");
         lock (messageExpiryTimes)
         {
            List<TickGuid> expired = messageExpiryTimes.FindAll(earlierThanNow);

            Guid[] guids = new Guid[expired.Count];
            foreach (TickGuid item in expired)
            {
               // check if it's fragments being sent in messageCache
               if (outgoingFragments.ContainsKey(item.id))
               {
                  Debug.WriteLine(String.Format("Expiring from fragment store {0}", item.id));
                  outgoingFragments.Remove(item.id);
               }
               if (incomingFragmentBuffer.ContainsKey(item.id))
               {
                  NetworkMessage[] messageFragments = (NetworkMessage[])incomingFragmentBuffer[item.id];
                  for (int i = 0; i < messageFragments.Length; i++)
                  {
                     if (messageFragments[i] == null)
                     {
                        Debug.WriteLine(String.Format("Need to request resend of message {0} fragment {1}", item.id, i));
                        NetworkMessage message = new NetworkMessage(localID, Guid.Empty, Guid.NewGuid(), NetworkMessageType.FragmentRequest, item.id.ToByteArray(), i, 1, 5000);
                        sendMessage(message);
                     }
                  }
               }
               messageExpiryTimes.Remove(item);
               //Schedule next time to try
               messageExpiryTimes.Add(new TickGuid(item.id, 5000));
            }
         }
         if (messageExpiryTimes.Count == 0)
         {
            if (messageLifetimeTimer != null)
            {
               messageLifetimeTimer.Dispose();
               messageLifetimeTimer = null;
            }
         }
      }

      /// <summary>
      /// Remove a message expiry time from the list
      /// </summary>
      /// <param name="id">ID to be removed</param>
      void RemoveExpiry(Guid id)
      {
         lock (messageExpiryTimes)
         {
            List<TickGuid> toRemove = new List<TickGuid>();
            foreach (TickGuid expiry in messageExpiryTimes)
            {
               if (expiry.id == id)
               {
                  toRemove.Add(expiry);
               }
            }
            foreach (TickGuid expiry in toRemove)
            {
               messageExpiryTimes.Remove(expiry);
            }
         }
      }
   }
}