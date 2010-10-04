using System;
using System.Data.SQLite;
using System.Threading;
using ScienceScope;

namespace SensorShare.Compact
{
   public partial class SensorShareServer
   {
      private enum RunningModes : byte
      {
         Manual,
         Starting,
         Started,
         Stopping,
         Stopped
      }

      private RunningModes runningMode = RunningModes.Started;

      private void Start()
      {
         runningMode = RunningModes.Starting;

         connectLogbook();
         Thread.Sleep(200);
         logbook_identifySensors();
         StartServer();
      }

      /// <summary>
      /// Called when sensor ids and definitions are received - when both are received updates the 
      /// server data and if server is starting, sends and invitation and begins taking readings
      /// </summary>
      private void UpdateSensorTypes()
      {
         if (sensorIDsUpdated && sensorRangesUpdated)
         {
            log.Append("UpdateSensorTypes", "Updating sensor defintions");
            lock (CurrentServerData.sensors)
            {
               CurrentServerData.sensors.Clear();
               for (int i = 0; i < 4; i++)
               {
                  SensorDefinition sensor = DatabaseHelper.GetSensorData(CurrentSensors[i], sensorsDatabase);
                  log.Append("Sensor connected: " + sensor.Description);
                  CurrentServerData.sensors.Add(sensor);
               }
            }
            //this.Invoke(sensorsUpdate);

            sensorIDsUpdated = false;
            sensorRangesUpdated = false;

            if (runningMode == RunningModes.Starting)
            {
               runningMode = RunningModes.Started;
               StartTakingReadings();
            }
         }
      }

      /// <summary>
      /// Terminate the varous components of the application
      /// </summary>
      private void ShutdownSensorServer()
      {
         log.Append("ShutdownSensorServer", "Shutting down server");
         runningMode = RunningModes.Stopped;

         StopTakingReadings();
         StopWorkings();

         if (logbook != null)
         {
            logbook.Deactivate();
         }
         //log.LogMessage -= displayLogHandler;
      }

   }
}