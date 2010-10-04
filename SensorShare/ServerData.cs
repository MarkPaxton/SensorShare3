using System;
using System.Drawing;
using System.IO;
using ScienceScope;

namespace SensorShare
{
   public class ServerData
   {
      public Guid id;
      public DateTime added_time;
      public string name = "";
      public string location = "";
      public string description = "";
      public byte[] pictureBytes;
      public SensorDescriptionsData sensors = new SensorDescriptionsData();

      public ServerData(Guid id, String name, String location, String description, 
         SensorDescriptionsData sensors, byte[] pictureBytes)
      {
         this.id = id;
         this.name = name;
         this.location = location;
         this.description = description;
         this.pictureBytes = pictureBytes;
         foreach (SensorDefinition sensorDef in sensors)
         {
            this.sensors.Add(sensorDef);
         }
      }

      public Bitmap Picture()
      {
         return new Bitmap(new MemoryStream(pictureBytes));
      }
   }
}
