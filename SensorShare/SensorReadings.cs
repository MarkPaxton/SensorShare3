using System;

namespace SensorShare
{
   /// <summary>
   /// Data from each of the four sensors
   /// </summary>
   public class SensorReadings : ISensorReadings
   {
      private Guid server_id;

      public Guid ServerID
      {
         get { return server_id; }
         set { server_id = value; }
      }
      private Double reading1;

      public Double Reading1
      {
         get { return reading1; }
         set { reading1 = value; }
      }
      private Double reading2;

      public Double Reading2
      {
         get { return reading2; }
         set { reading2 = value; }
      }
      private Double reading3;

      public Double Reading3
      {
         get { return reading3; }
         set { reading3 = value; }
      }
      private Double reading4;

      public Double Reading4
      {
         get { return reading4; }
         set { reading4 = value; }
      }
      public DateTime Time
      {
         get { return DateTime.FromFileTimeUtc(timeBinary); }
         set { timeBinary = value.ToFileTimeUtc(); }
      }
      private long timeBinary;

      public long TimeBinary
      {
         get { return timeBinary; }
         set { timeBinary = value; }
      }

      /// <summary>
      /// </summary>
      /// <param name="reading1">Reading from sensor 1</param>
      /// <param name="reading2">Reading from sensor 2</param>
      /// <param name="reading3">Reading from sensor 3</param>
      /// <param name="reading4">Reading from sensor 4</param>
      /// <param name="time">Time readings were taken</param>
      public SensorReadings(Guid server_id, Double reading1, Double reading2, Double reading3, Double reading4, DateTime time)
      {
         this.server_id = server_id;
         this.reading1 = reading1;
         this.reading2 = reading2;
         this.reading3 = reading3;
         this.reading4 = reading4;
         this.Time = time;
      }

      public SensorReadings()
      {
      }

      public Double Reading(int i)
      {
         Double[] result = new Double[4] { reading1, reading2, reading3, reading4 };
         return result[i];
      }
   }
}