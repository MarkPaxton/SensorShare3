using System;
using System.Data.SQLite;
using System.Diagnostics;
using ScienceScope;

namespace SensorShare.Compact
{
   public partial class SensorShareServer
   {
      private int readings_count = 0;

      double[] current_readings = new double[4] { 0, 0, 0, 0 };

      DateTime sensorReadingTime = new DateTime();

      System.Threading.Timer takeReadingsTimer;
      int takeReadingsTimeLimit = 10000;


      protected void StartTakingReadings()
      {
         if (!takeReadings)
         {
            log.Append("StartTakingReadings", String.Format("Starting to take readings, mode: {0}", runningMode));
            takeReadings = true;
            logbook_sendCommand(Logbook.Command.ReadASCII);
            takeReadingsTimer = new System.Threading.Timer(readASCIITimerCallback, null, takeReadingsTimeLimit, takeReadingsTimeLimit);
            //DoUpdateTakeReadingsInvoke("Stop Taking Readings");
         }
      }

      protected void ResetReadASCIITimer()
      {
         if (takeReadingsTimer != null)
         {
            takeReadingsTimer.Dispose();
         }
         if (takeReadings)
         {
            takeReadingsTimer = new System.Threading.Timer(readASCIITimerCallback, null, takeReadingsTimeLimit, takeReadingsTimeLimit);
         }
      }

      protected void ReadASCIITimerTimeout(object o)
      {
         log.Append("ReadASCIITimerTimeout", "ReadASCII timer timed out - readings aren't being received");
         StopTakingReadings();
         readASCIIFailCount = 0;
         logbook.reconnect();
         if ((runningMode == RunningModes.Manual) || (runningMode == RunningModes.Started))
         {
            StartTakingReadings();
         }
      }

      protected void StopTakingReadings()
      {
         if (takeReadings)
         {
            log.Append("StopTakingReadings", "Stopping taking readings");
            takeReadings = false;
            takeReadingsTimer.Dispose();
            //DoUpdateTakeReadingsInvoke("Take Readings");
         }
      }

      protected SQLiteConnection asciiReadingDBConnection = null;
      /// <summary>
      /// When an ascii reading is reveived this function is called from logbook.cs and processes it
      /// </summary>
      /// <param name="dataBytes"></param>
      protected void processASCIIReading(byte[] dataBytes)
      {
         if (asciiReadingDBConnection == null)
         {
            asciiReadingDBConnection = database;
         }

         readASCIIFailCount = 0;
         ResetReadASCIITimer();

         DateTime receivedTime = DateTime.UtcNow;
         string[] channel = new string[4];
         try
         {
            channel = Logbook.SplitChannels(dataBytes);
         }
         catch (Exception ex)
         {
            log.LogException(ex, "processASCIIReading SplitChannels");
         }

         DateTime localTime = receivedTime.ToLocalTime();
         string dataText = String.Format("Reading: {4}-{5:d2}-{6:d2} {7:d2}:{8:d2}:{9:d2} [{0}] [{1}] [{2}] [{3}]",
             channel[0].Trim(), channel[1].Trim(), channel[2].Trim(), channel[3].Trim(),
             localTime.Year, localTime.Month, localTime.Day, localTime.Hour, localTime.Minute, localTime.Second);

         log.Append("processASCIIReading", dataText);
         //UpdateLatestDataLabel(dataText);

         double[] value = new double[4];
         try
         {
            for (int i = 0; i < 4; i++)
            {
               try
               {
                  value[i] = Logbook.ReadingToValue(channel[i], CurrentSensors[i]);
                  current_readings[i] = value[i];
               }
               catch (FormatException ex)
               {
                  log.LogException(ex, String.Format("processASCIIReading[{0}]", i));
                  current_readings[i] = 0;
               }

            }
            //if ((sensorGraph != null) && (sensorDefinitions[sensorToGraph].ID != 255))
            //{
            //   sensorGraph.Plot(receivedTime, value[sensorToGraph]);
            //   sensorGraph.ToNow();
            //}
            sensorReadingTime = receivedTime;
         }
         catch (Exception ex)
         {
            log.LogException(ex, "processASCIIReading sensorGraph");
         }

         SensorReadingsData dataToSend = null;
         double[] std_devs = null;
         try
         {
            SensorReadings readings = new SensorReadings(CurrentServerData.id, current_readings[0],
               current_readings[1], current_readings[2], current_readings[3], receivedTime);
            DatabaseHelper.saveSensorReadings(readings, asciiReadingDBConnection);
         }
         catch (Exception ex)
         {
            log.LogException(ex, "processASCIIReading saveSensorReadings");
         }

         //UpdateOverallStats(asciiReadingDBConnewction);

         //std_devs = DatabaseHelper.getStdDev(CurrentServerData.id, overall_means, (int)overall_stats[12], asciiReadingDBConnection);
         //dataToSend = new SensorReadingsData(CurrentServerData.id, value[0], value[1], value[2], value[3], receivedTime, overall_means, std_devs);
         //if ((runningMode == RunningModes.Manual) || (runningMode == RunningModes.Started))
         //{
            //sendSensorReadings(dataToSend);
         //}

         //double[] significances = new double[4];
         //for (int stat_count = 0; stat_count < 4; stat_count++)
         //{
         //   significances[stat_count] = Stats.Significance(overall_means[stat_count], std_devs[stat_count], value[stat_count]);
         //   Debug.WriteLine(String.Format("processASCIIReading Reading[{0}]: {1}, Significance[{0}]: {2}", stat_count, value[stat_count], significances[stat_count]));
         //}
         //QuestionMessage question = QuestionHelper.CreateCurrentReadingQuestion(overall_stats, dataToSend, significances, sensorDefinitions);
         //if (question != null)
         //{
         //   question.id = Guid.NewGuid();
         //   question.Time = receivedTime;
         //   question.Server = current_server_id;
         //   question.Author = ServerName;
         //   log.Append("processASCIIReading ", String.Format("Question {0} '{1}'", question.id, question.Text));
         //   try
         //   {
         //      DatabaseHelper.SaveQuestionMessage(question, asciiReadingDBConnection);
         //   }
         //   catch (Exception ex)
         //   {
         //      log.LogException(ex, "processASCIIReading SaveQuestion");
         //   }
         //   sendQuestion(question);
         //}

         //if (this.InvokeRequired)
         //{
         //   object[] invokeParams = new object[4];
         //   for (int i = 0; i < 4; i++)
         //   {
         //      invokeParams[i] = std_devs[i];
         //   }
         //   this.Invoke(statsDisplayUpdate, invokeParams);
         //}
         //else
         //{
         //   UpdateStatsDisplay(std_devs[0], std_devs[1], std_devs[2], std_devs[3]);
         //}
      }

      protected int readASCIIFailCount = 0;
      protected void ReadASCIICommandFailed(bool failed)
      {
         readASCIIFailCount++;
         if (readASCIIFailCount < 2)
         {
            ResetReadASCIITimer();
            logbook_sendCommand(Logbook.Command.ReadASCII);
         }
      }

   }

}