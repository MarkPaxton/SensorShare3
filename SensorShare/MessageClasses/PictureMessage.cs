using System;
using System.IO;

namespace SensorShare
{
   public class PictureMessage
   {
      public Guid ID = Guid.Empty;
      public string Name;
      public string Sender;
      public string Description;
      public byte[] PictureBytes;

      public PictureMessage()
      {

      }

      public PictureMessage(byte[] pictureBytes, string description, string name, string sender)
      {
         ID = Guid.NewGuid();
         PictureBytes = pictureBytes;
         Description = description;
         Name = name;
         Sender = sender;
      }

      public PictureMessage(byte[] data)
      {
         byte[] guidBytes = new byte[16];
         int copyPos = 0;
         Array.Copy(data, guidBytes, 16);
         ID = new Guid(guidBytes);
         copyPos += 16;

         int pictureBytesLength = BitConverter.ToInt32(data, copyPos);
         copyPos += sizeof(int);
         PictureBytes = new byte[pictureBytesLength];
         Array.Copy(data, copyPos, PictureBytes, 0, pictureBytesLength);
         copyPos += pictureBytesLength;
         using (MemoryStream ms = new MemoryStream(data, copyPos, data.Length - copyPos))
         {
            Name = TextHelper.DeStreamString(ms);
            Description = TextHelper.DeStreamString(ms);
            Sender = TextHelper.DeStreamString(ms);
         }
      }

      public byte[] GetBytes()
      {
         byte[] returnBytes;
         using (MemoryStream ms = new MemoryStream())
         {
            ms.Write(ID.ToByteArray(), 0, 16);

            int pictureBytesLength = PictureBytes.Length;
            byte[] pictureBytesLengthBytes = BitConverter.GetBytes(pictureBytesLength);

            ms.Write(pictureBytesLengthBytes, 0, pictureBytesLengthBytes.Length);
            ms.Write(PictureBytes, 0, pictureBytesLength);

            TextHelper.StreamString(ms, Name);
            TextHelper.StreamString(ms, Description);
            TextHelper.StreamString(ms, Sender);

            returnBytes = ms.ToArray();
         }
         return returnBytes;
      }
   }
}
