using System;
using System.IO;
namespace SensorShare
{
   /// <summary>
   /// Create a main data message - These are sent by both the client and server anytime the
   /// public method Send is called.  The expectation is that in most systems that these will be the most
   /// frequently sent messages as these are used for application level conversation.  All other messages
   /// are basically system-management messages.
   /// Contains the standard header plus the array of bytes passed in by the application
   /// </summary>
   public class TypedMessage
   {
      public MessageType type;
      public byte[] data;

      public TypedMessage(MessageType type, byte[] data)
      {
         this.type = type;
         this.data = data;
      }

      public TypedMessage(byte[] messageBuffer, int messageLength)
      {
         // Get the type
         this.type = (MessageType)BitConverter.ToInt32(messageBuffer, 0);
         int copyPos = sizeof(Int32);

         // Data body is just a byte array so break it out into a new buffer
         int dataLength = messageLength - copyPos;
         this.data = new byte[dataLength];
         Array.Copy(messageBuffer, copyPos, this.data, 0, dataLength);
      }

      public byte[] GetBytes()
        {
           byte[] messageBody;
           using (MemoryStream ms = new MemoryStream())
           {
              // Get byte array for type
              ms.Write(BitConverter.GetBytes((Int32)this.type), 0, sizeof(Int32));
              ms.Write(data, 0,data.Length);

              messageBody = new byte[ms.Length];
              messageBody = ms.ToArray();
           }
            return messageBody;
        }
   }
}