using System;

namespace SensorShare.Network
{
   public delegate void MessageMissedEventHandler(object sender, MessageMissedEventArgs args);

   public class MessageMissedEventArgs : EventArgs
   {
      protected Guid senderID;
      public Guid SenderID
      {
         get { return senderID; }
      }

      protected Guid destinationID;
      public Guid DestinationID
      {
         get { return destinationID; }
      }

      protected Guid messageID;
      public Guid MessageID
      {
         get { return messageID; }
      }

      public MessageMissedEventArgs(NetworkMessage message)
      {
         this.senderID = message.SenderID;
         this.destinationID = message.DestinationID;
         this.messageID = message.MessageID;
      }

      public MessageMissedEventArgs(Guid sender, Guid destination, Guid id)
      {
         this.senderID = sender;
         this.destinationID = destination;
         this.messageID = id;
      }
   }
}
