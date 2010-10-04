using System;
using System.IO;
using System.IO.Compression;

namespace SensorShare.Network
{
   public enum NetworkMessageType
   {
      /// <summary>
      /// Standard data message
      /// </summary>
      Message,
      /// <summary>
      /// Requests for IP address from target
      /// </summary>
      DirectRequest,
      /// <summary>
      /// Sends IP address to requester
      /// </summary>
      DirectReply,
      /// <summary>
      /// Message has been received
      /// </summary>
      ReceivedOK,
      /// <summary>
      /// Message hasn't been received properly
      /// </summary>
      ReceiveFailed,
      /// <summary>
      /// Request for a fragment of a large message
      /// </summary>
      FragmentRequest,
   }

   public class NetworkMessage
   {
      /// <summary>
      /// ID of the sender of the message
      /// </summary>
      private Guid senderID;
      /// <summary>
      /// ID of the destination Guid.Empty for broadcast
      /// </summary>
      private Guid destinationID;
      /// <summary>
      /// GZipped data payload for the message
      /// </summary>
      private byte[] gzData;
      /// <summary>
      /// Type of the message
      /// </summary>
      private NetworkMessageType type;
      /// <summary>
      /// The id of the message
      /// </summary>
      private Guid messageID;

      /// <summary>
      /// The ID of the message
      /// </summary>
      public Guid MessageID
      {
         get { return messageID; }
         set { messageID = value; }
      }

      public NetworkMessage(Guid senderID, Guid destinationID, Guid messageID, NetworkMessageType type, byte[] data, int fragment, int fragments, int life)
      {
         this.senderID = senderID;
         this.destinationID = destinationID;
         this.type = type;
         this.messageID = messageID;
         this.fragmentNumber = fragment;
         this.fragmentTotal = fragments;
         this.lifetime = life;
         if (this.FragmentTotal == 1)
         {
            // Compress the data in a memory stream
            using (MemoryStream ms = new MemoryStream())
            {
               using (GZipStream gzStream = new GZipStream(ms, CompressionMode.Compress, true))
               {
                  gzStream.Write(data, 0, data.Length);
                  gzStream.Flush();
                  gzStream.Close();
                  this.gzData = ms.ToArray();
               }
            }
         }
         else
         {
            this.gzData = new byte[data.Length];
            Array.Copy(data, this.gzData, data.Length);
         }
      }

      public NetworkMessage(byte[] bytes)
      {
         using (BinaryReader reader = new BinaryReader(new MemoryStream(bytes, 0, headerSize)))
         {
            this.senderID = new Guid(reader.ReadBytes(16));
            this.destinationID = new Guid(reader.ReadBytes(16));
            this.messageID = new Guid(reader.ReadBytes(16));
            this.fragmentNumber = reader.ReadInt32();
            this.fragmentTotal = reader.ReadInt32();
            this.lifetime = reader.ReadInt32();
            this.type = (NetworkMessageType)reader.ReadInt32();
         }
         int dataLength = bytes.Length - headerSize;
         gzData = new byte[dataLength];
         Array.Copy(bytes, headerSize, gzData, 0, dataLength);
      }

      /// <summary>
      /// The sender Guid of the message
      /// </summary>
      public Guid SenderID
      {
         get { return this.senderID; }
      }

      /// <summary>
      /// THe Guid destination of the message
      /// </summary>
      public Guid DestinationID
      {
         get { return this.destinationID; }
      }
      /// <summary>
      /// The data contained within the message
      /// </summary>
      public byte[] Data
      {
         get
         {
            byte[] toReturn = new byte[0];
            if (this.FragmentTotal == 1)
            {
               using (MemoryStream msIn = new MemoryStream(gzData))
               {
                  // Start the gunzip stream based on the memory stream of the compressed body
                  msIn.Position = 0;
                  using (GZipStream gzStream = new GZipStream(msIn, CompressionMode.Decompress, true))
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
            }
            else
            {
               toReturn = this.gzData;
            }
            return toReturn;
         }

      }

      /// <summary>
      /// The type of the message
      /// </summary>
      public NetworkMessageType Type
      {
         get { return this.type; }
      }

      /// <summary>
      /// Returns the message translated into bytes
      /// </summary>
      public byte[] GetBytes()
      {
         byte[] toReturn;
         using (MemoryStream ms = new MemoryStream())
         {
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
               writer.Write(senderID.ToByteArray());
               writer.Write(destinationID.ToByteArray());
               writer.Write(messageID.ToByteArray());
               writer.Write(fragmentNumber);
               writer.Write(fragmentTotal);
               writer.Write(lifetime);
               writer.Write((int)this.type);
               writer.Flush();
               ms.Write(gzData, 0, gzData.Length);
               toReturn = new byte[ms.Length];
               toReturn = ms.ToArray();
            }
         }
         return toReturn;
      }

      /// <summary>
      /// The fragment number of the message
      /// </summary>
      private int fragmentNumber;

      /// <summary>
      /// The fragment number of this message
      /// </summary>
      public int FragmentNumber
      {
         get { return fragmentNumber; }
         set { fragmentNumber = value; }
      }
      /// <summary>
      /// Thte total number of fragments of the message
      /// </summary>
      private int fragmentTotal;

      /// <summary>
      /// The total number of fradments for the message
      /// </summary>
      public int FragmentTotal
      {
         get { return fragmentTotal; }
         set { fragmentTotal = value; }
      }
      /// <summary>
      /// The number of ticks the receiver should wait for all fragments to be received before firing a failed event
      /// </summary>
      private int lifetime;

      /// <summary>
      /// The number of ticks the message fragments should be kept for before whilst waiting
      /// </summary>
      public int Lifetime
      {
         get { return lifetime; }
         set { lifetime = value; }
      }

      /// <summary>
      /// The size of the message header
      /// 3 Guid (16 bytes)
      /// 4 Int32 (4 bytes)
      /// </summary>
      private static int headerSize = (3 * 16) + (4 * 4);

      /// <summary>
      /// The Gzip compressed version of the message
      /// </summary>
      internal byte[] GzData
      {
         get
         {
            return gzData;
         }
         set
         {
            this.gzData = new byte[value.Length];
            Array.Copy(value, this.gzData, value.Length);
         }
      }

   }
}
