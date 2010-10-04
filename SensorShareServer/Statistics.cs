using System;
using System.Collections.Generic;
using System.Data.SQLite;
using ScienceScope;

namespace SensorShare.Compact
{
   public partial class SensorShareServer 
   {
      private List<double[]> sensorStats = new List<double[]>();
      private double[] sensor_1_stats = new double[3];
      private double[] sensor_2_stats = new double[3];
      private double[] sensor_3_stats = new double[3];
      private double[] sensor_4_stats = new double[3];
      private double[] overall_stats = new double[13];
      private double[] overall_means = new double[4];

      private void InitialiseSensorData()
      {
         current_readings[0] = 0;
         current_readings[1] = 0;
         current_readings[2] = 0;
         current_readings[3] = 0;

         //sensorGraph.ToNow();

         sensorStats.Add(sensor_1_stats);
         sensorStats.Add(sensor_2_stats);
         sensorStats.Add(sensor_3_stats);
         sensorStats.Add(sensor_4_stats);


         //  UpdateOverallStats(database);

         //sensorGraph.MaxY = sensorStats[0][0] + Math.Abs(0.25 * sensorStats[0][0]);
         //sensorGraph.MinY = sensorStats[0][1] - Math.Abs(0.25 * sensorStats[0][0]);
      }

      private void UpdateOverallStats(SQLiteConnection database)
      {
         try
         {
            log.Append("updateStats", "Updating statistics for server");
            overall_stats = DatabaseHelper.getStats(CurrentServerData.id, "", database);

            int i = 0;
            for (int sensor_counter = 0; sensor_counter < 4; sensor_counter++)
            {
               for (int stat_counter = 0; stat_counter < 3; stat_counter++)
               {
                  sensorStats[sensor_counter][stat_counter] = overall_stats[i++];
               }
            }
            readings_count = Convert.ToInt32(overall_stats[i]);
            overall_means[0] = overall_stats[2];
            overall_means[1] = overall_stats[5];
            overall_means[2] = overall_stats[8];
            overall_means[3] = overall_stats[11];
            log.Append("updateStats", String.Format("Count: {0} - Means: [{1}, {2}, {3}, {4}]",
               overall_stats[12], overall_means[0], overall_means[1], overall_means[2], overall_means[3]));
         }
         catch (Exception ex)
         {
            log.LogException(ex);
         }
      }
   }
}
