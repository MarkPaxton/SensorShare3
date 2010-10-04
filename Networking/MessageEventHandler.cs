using System;
using System.IO;
using System.IO.Compression;

namespace SensorShare.Network
{
   public delegate void MessageEventHandler(object sender, MessageEventArgs args);

   public class MessageEventArgs : EventArgs
   {
      private Guid senderID;

      public Guid SenderID
      {
         get { return senderID; }
      }
      private Guid destinationID;

      public Guid DestinationID
      {
         get { return destinationID; }
      }
      private byte[] gzData;
      public byte[] Data
      {
         get
         {
            byte[] toReturn = new byte[0];
            using (MemoryStream msIn = new MemoryStream(gzData))
            {
               // Start the gunzip stream based on the memory stream of the compressed body
               msIn.Position = 0;
               using (GZipStream gzStream = new  GZipStream(msIn, CompressionMode.Decompress, true))
               {
                  // Create a new memory stream for the uncompressed body
                  using (MemoryStream msOut = new MemoryStream())
                  {
                     // Read the message into the new memory stream in chunks of bytes
                     int chunkSize = 1024;
                     byte[] inputChunks = new byte[chunkSize];
                     int numRead = 1;
                     while (numRead > 0)
                     {
                        numRead = gzStream.Read(inputChunks, 0, chunkSize);
                        msOut.Write(inputChunks, 0, numRead);
                     }
                     toReturn = new byte[msOut.Length];
                     toReturn = msOut.ToArray();
                  }
               }
           }
           return toReturn;
         }
      }

      public MessageEventArgs(NetworkMessage message)
      {
         this.gzData = message.GzData;
         this.senderID = message.SenderID;
         this.destinationID = message.DestinationID;
      }

      public MessageEventArgs(Guid sender, Guid destination, byte[] gzData)
      {
         this.senderID = sender;
         this.destinationID = destination;
         this.gzData = gzData;
      }
   }
}
