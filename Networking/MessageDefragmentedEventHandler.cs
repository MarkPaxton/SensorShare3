using System;

namespace SensorShare.Network
{
   public delegate void MessageDefragmentedEventHandler(object sender, MessageDefragmentedEventArgs args);

   public class MessageDefragmentedEventArgs : MessageMissedEventArgs
   {
      private byte[] gzData;
      public byte[] GzData
      {
         get { return gzData; }
      }

      public MessageDefragmentedEventArgs(NetworkMessage message):base(message)
      {
         this.gzData = message.Data;
      }

      public MessageDefragmentedEventArgs(Guid destination, Guid sender, Guid message, byte[] data):base(sender, destination,  message)
      {
         this.gzData = data;
      }
   }
}
