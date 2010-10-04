using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ScienceScope;

namespace SensorShare.Compact
{
   public partial class SensorShareServer 
   {
      // Logbook stuff
      private Logbook logbook;
      private string logbookPort = "COM8";

      private bool logbookConnectionError = false;

      private bool sensorIDsUpdated = false;
      private bool sensorRangesUpdated = false;
      private SensorDefinition[] CurrentSensors = null;

      private void InitialiseLogbook()
      {
         CurrentSensors = new SensorDefinition[4];
         for (int s = 0; s < 4; s++)
         {
            CurrentSensors[s] = new SensorDefinition(); 
         }
         Thread logbookThread = new Thread(new ThreadStart(LogbookThread));
         logbookThread.Start();
         Thread.Sleep(100);  //Allow thread to start now
      }

      private void ConnectLogbookAndIdentifySensors()
      {
         log.Append("Connecting to Logbook and identifying sensors");
         MessageBox.Show("About to connect to Logbook, please activate it now.", "Ready to connect");
         connectLogbook();
         Thread.Sleep(500);
         logbook_identifySensors();
         Thread.Sleep(1000);
      }

      private void LogbookThread()
      {
         logbook = new Logbook(logbookPort);

         logbook.CommandResult += new CommandResultEventHandler(logbook_CommandResult);
         logbook.CommandTimeout += new CommandTimeoutEventHandler(logbook_CommandTimeout);
         logbook.CommandFailed += new CommandFailedEventHandler(logbook_CommandFailed);
         logbook.Disconnected += new EventHandler(logbook_Disconnected);
      }

      private void connectLogbook()
      {
         try
         {
            log.Append("connectLogbook", "Connecting to logbook");
            logbook.Activate();
         }
         catch (LogbookConnectionException e)
         {
            log.Append("connectLogbook", "Error connecting: " + e.Message.ToString());
            log.LogException(e);
            logbookConnectionError = true;
         }
         catch (Exception ex)
         {
            log.LogException(ex);
            logbookConnectionError = true;
         }
         logbookConnectionError = false;
      }

      private void disconnectLogbook()
      {
         logbook.Deactivate();
      }



      private void logbook_Disconnected(object o, EventArgs ev)
      {
         log.Append("logbook_Disconnected", "Logbook disconnected");
         if (runningMode == RunningModes.Started)
         {
            log.Append("logbook_Disconnected", "Reconnecting logbook");
            logbook.Activate();
            logbook_sendCommand(Logbook.Command.ReadASCII);
         }
      }

      private void logbook_sendCommand(Logbook.Command command)
      {
         try
         {
            if (logbook.IsActive)
            {
               log.Append("logbook_sendCommand", String.Format("Sending {0}", command.name));
               if (logbookConnectionError)
               {
                  log.Append("logbook_sendCommand", "Logbook connection error - reconnecting");
                  logbook.reconnect();
                  logbookConnectionError = false;

               }
               logbook.sendCommand(command);
            }
         }
         catch (Exception ex)
         {
            logbookConnectionError = true;
            log.LogException(ex);
         }
      }

      private void logbook_identifySensors()
      {
         if (logbook.IsActive)
         {
            sensorRangesUpdated = false;
            sensorIDsUpdated = false;

            log.Append("logbook_identifySensors", "Identifying sensors");

            log.Append("logbook_identifySensors", "Input power on & confirm extended mode");
            logbook_sendCommand(Logbook.Command.InputPowerOn);
            Thread.Sleep(250);
            logbook_sendCommand(Logbook.Command.ConfirmExtendedProtocol);
            log.Append("logbook_identifySensors", "Getting sensor ranges");
            Thread.Sleep(250);
            logbook_sendCommand(Logbook.Command.GetSensorRanges);
         }
         else
         {
            MessageBox.Show("Not Connected", "Connect to Logbook first", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
         }
      }


      private void logbook_CommandResult(object o, CommandResultEventArgs e)
      {
         if (e.Command.Equals(Logbook.Command.ReadASCII))
         {
            logbook_ReadASCII_Result(o, e);
            if ((runningMode == RunningModes.Started) || takeReadings)
            {
               logbook.sendCommand(Logbook.Command.ReadASCII);
            }
            return;
         }
         if (e.Command.Equals(Logbook.Command.BatteryTest))
         {
            logbook_BatteryTest_Result(o, e);
            return;
         }
         if (e.Command.Equals(Logbook.Command.KeepAwake))
         {
            return;
         }
         if (e.Command.Equals(Logbook.Command.GetBatteryLevel))
         {
            logbook_GetBatteryLevel_Result(o, e);
            return;
         }
         if (e.Command.Equals(Logbook.Command.ConfirmExtendedProtocol))
         {
            return;
         }
         if (e.Command.Equals(Logbook.Command.InputPowerOn))
         {
            return;
         }
         if (e.Command.Equals(Logbook.Command.IdentifySensors))
         {
            logbook_IdentifySensors_Result(o, e);
            return;
         }
         if (e.Command.Equals(Logbook.Command.GetSensorRanges))
         {
            logbook_GetSensorRanges_Result(o, e);
            return;
         }

         string printString = "";
         foreach (byte letter in e.Result)
         {
            //serverMessagesBox.Text += Convert.ToChar(letter) + "\r\n";
            //charsBox.Text += letter.ToString() + " ";
            printString += Logbook.ConvertChar(letter);
         }
         log.Append("logbook_CommandResult", String.Format("Logbook command result: {0}", printString));
      }

      private void logbook_CommandTimeout(object o, CommandTimeoutEventArgs ev)
      {
         log.Append("logbook_CommandTimeout", String.Format("Error: Logbook didn't respond to {0}", ev.FailedCommand.name));
         log.Append("logbook_CommandTimeout", Encoding.ASCII.GetString(ev.FailedResponse, 0, ev.FailedResponse.Length));
         if (runningMode == RunningModes.Started)
         {
            if (ev.FailedCommand.Equals(Logbook.Command.ReadASCII))
            {
               ReadASCIICommandFailed(false);
            }
         }
      }

      void logbook_CommandFailed(object sender, CommandFailedEventArgs ev)
      {
         log.Append("logbook_CommandFailed", String.Format("Command {0} failed: {1}", ev.FailedCommand.name, ev.reason));
         if (ev.response.Length > 0)
         {
            log.Append("logbook_CommandFailed", "Resposnse: " + Encoding.ASCII.GetString(ev.response, 0, ev.response.Length));
         }
         logbookConnectionError = true;

         if (ev.FailedCommand.Equals(Logbook.Command.ReadASCII))
         {
            ReadASCIICommandFailed(true);
         }
      }

      private void logbook_ReadASCII_Result(object o, CommandResultEventArgs ev)
      {
         processASCIIReading(ev.Result);
      }

      private void logbook_BatteryTest_Result(object o, CommandResultEventArgs e)
      {
         int batValue = e.Result[1];
         string printString = "\rBattery level: " + batValue.ToString();
         if (batValue <= 180)
         {
            printString += " (Low!)";
         }
         else if (batValue == 254)
         {
            printString += " (Good)";
         }
         log.Append("logbook_BatteryTest_Result", printString.Replace("\r", "\r\n"));
      }

      private void logbook_GetBatteryLevel_Result(object o, CommandResultEventArgs e)
      {
         int batStat = e.Result[1];
         string printString = "\rBattery level: " + batStat.ToString();
         switch (batStat)
         {
            case 0:
               printString += "Battery critical";
               break;
            case 1:
               printString += "Battery empty (<10% remaining)";
               break;
            case 2:
               printString += "Less than 33% remaining";
               break;
            case 3:
               printString += "Between 33% and 66% remaining";
               break;
            case 4:
               printString += "Over 66% remaining";
               break;
            case 5:
               printString += "Full battery (ext. power)";
               break;
            case 6:
               printString += "Mains power, dead battery";
               break;
            case 7:
               printString += "No battery";
               break;
            default:
               printString += "Unknown response";
               break;
         }
         printString = printString.Replace("\r", "\r\n");
         log.Append("logbook_GetBatteryLevel_Result", printString);
      }

      private void logbook_GetSensorRanges_Result(object o, CommandResultEventArgs e)
      {
         string printString = "Sensors ranges: {";
         for (int i = 1; i <= 4; i++)
         {
            CurrentSensors[i - 1].Range = Convert.ToInt32(e.Result[i]);
            printString += String.Format("{0} ", e.Result[i]);
         }
         printString += "}";
         log.Append("logbook_GetSensorRanges_Result", printString);

         if (sensorIDsUpdated == false)
         {
            log.Append("logbook_GetSensorRanges_Result", "Requesting sensor IDs");
            logbook_sendCommand(Logbook.Command.IdentifySensors);
         }
         sensorRangesUpdated = true;
         UpdateSensorTypes();
      }

      private void logbook_IdentifySensors_Result(object o, CommandResultEventArgs e)
      {
         string printString = "Sensor IDs: {";
         for (int i = 1; i <= 4; i++)
         {
            CurrentSensors[i - 1].ID = Convert.ToInt32(e.Result[i]);
            printString += String.Format("{0} ", e.Result[i]);
         }
         printString += "}";
         log.Append("logbook_IdentifySensors_Result", printString);
         sensorIDsUpdated = true;
         UpdateSensorTypes();
      }


   }
}
