using System;

namespace SensorShare
{
   /// <summary>
   /// Data from each of the four sensors
   /// </summary>
   public class SensorReadingsData:SensorReadings
   {
      public Double Mean1;
      public Double StdDev1;
      public Double Mean2;
      public Double StdDev2;
      public Double Mean3;
      public Double StdDev3;
      public Double Mean4;
      public Double StdDev4;

      /// <summary>
      /// </summary>
      /// <param name="reading1">Reading from sensor 1</param>
      /// <param name="reading2">Reading from sensor 2</param>
      /// <param name="reading3">Reading from sensor 3</param>
      /// <param name="reading4">Reading from sensor 4</param>
      /// <param name="time">Time readings were taken</param>
      public SensorReadingsData(Guid server_id, Double reading1, Double reading2, Double reading3, Double reading4, DateTime time, Double[] means, Double[] stdDevs):
         base(server_id, reading1, reading2, reading3, reading4, time)
      {

         this.Mean1 = means[0];
         this.Mean2 = means[1];
         this.Mean3 = means[2];
         this.Mean4 = means[3];
         this.StdDev1 = stdDevs[0];
         this.StdDev2 = stdDevs[1];
         this.StdDev3 = stdDevs[2];
         this.StdDev4 = stdDevs[3];
      }

      public SensorReadingsData()
      {
      }

      public Double Mean(int i)
      {
         Double[] mean = new Double[4] { Mean1, Mean2, Mean3, Mean4 };
         return mean[i];
      }

      public Double StdDev(int i)
      {
         Double[] stdDev = new Double[4] { StdDev1, StdDev2, StdDev3, StdDev4 };
         return stdDev[i];
      }
   }
}