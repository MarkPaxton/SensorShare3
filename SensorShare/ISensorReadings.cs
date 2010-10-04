using System;
namespace SensorShare
{
   public interface ISensorReadings
   {
      double Reading(int i);
      double Reading1 { get; set; }
      double Reading2 { get; set; }
      double Reading3 { get; set; }
      double Reading4 { get; set; }
      Guid ServerID { get; set; }
      DateTime Time { get; set; }
      long TimeBinary { get; set; }
   }
}
