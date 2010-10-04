using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SensorShare
{
   public class DescriptionMessage
   {
      public string Name;
      public string Location;
      public string Description;
      public byte[] PictureBytes;
      public SensorDescriptionsData Sensors;

      public DescriptionMessage(ServerData serverData)
      {
         this.Name = serverData.name;
         this.Location = serverData.location;
         this.Description = serverData.description;
         this.PictureBytes = new byte[serverData.pictureBytes.Length];
         Array.Copy(serverData.pictureBytes, this.PictureBytes, serverData.pictureBytes.Length);
         this.Sensors = serverData.sensors;
      }

      public DescriptionMessage(byte[] bytes)
      {
         using (MemoryStream ms = new MemoryStream(bytes, false))
         {
            Name = TextHelper.DeStreamString(ms);
            Location = TextHelper.DeStreamString(ms);
            Description = TextHelper.DeStreamString(ms);

            // Sensors Length
            byte[] lengthBytes = new byte[sizeof(Int32)];
            ms.Read(lengthBytes, 0, sizeof(Int32));
            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] sensorsBytes = new byte[length];
            ms.Read(sensorsBytes, 0, length);
            Sensors = MessageHelper.DeserializeSensorDescriptionsData(sensorsBytes);

            //    Length
            ms.Read(lengthBytes, 0, sizeof(Int32));
            length = BitConverter.ToInt32(lengthBytes, 0);

            PictureBytes = new byte[length];
            ms.Read(PictureBytes, 0, length);
         }
      }

      public byte[] GetBytes()
      {
         byte[] bytes;
         using (MemoryStream ms = new MemoryStream())
         {
            TextHelper.StreamString(ms, Name);
            TextHelper.StreamString(ms, Location);
            TextHelper.StreamString(ms, Description);

            Int32 length;
            using (MemoryStream sensorsStream = new MemoryStream())
            {
               MessageHelper.SerializeSensorDescriptionsDataTimes(sensorsStream, Sensors);
               length = (int)sensorsStream.Length;
               ms.Write(BitConverter.GetBytes(length), 0, sizeof(Int32));
               sensorsStream.WriteTo(ms);
            }

            length = PictureBytes.Length;
            ms.Write(BitConverter.GetBytes(length), 0, sizeof(Int32));
            ms.Write(PictureBytes, 0, length);
            bytes = ms.ToArray();
         }
         return bytes;
      }

   }
}
