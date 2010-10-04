using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ScienceScope;
using mcp;
using System.Threading;

namespace SensorShare.Compact
{
   public partial class ConnectLogbookForm : Form
   {
      private Logbook logbook = null;
      LabelTextDelegate updateLabel = null;

      System.Threading.Timer activateTimer = null;
      TimerCallback activateTimerCallback = null;

      bool gotIDs = false;
      string sensorIDs = "";
      string sensorRanges = "";

      CommandResultEventHandler logbook_CommandResultHandler = null;
      CommandFailedEventHandler logbook_CommandFailedHandler = null;
      CommandTimeoutEventHandler logbook_CommandTimeoutHandler = null;
      EventHandler logbook_DisconnectedHandler = null;

      public ConnectLogbookForm(Logbook logbook)
      {
         InitializeComponent();
         updateLabel = new LabelTextDelegate(DoUpdateLabel);

         this.logbook = logbook;

         logbook_CommandResultHandler = new CommandResultEventHandler(logbook_CommandResult);
         logbook_CommandFailedHandler = new CommandFailedEventHandler(logbook_CommandFailed);
         logbook_CommandTimeoutHandler = new CommandTimeoutEventHandler(logbook_CommandTimeout);
         logbook_DisconnectedHandler = new EventHandler(logbook_Disconnected);
         activateTimerCallback += new TimerCallback(activateTimerPing);

         logbook.CommandResult += logbook_CommandResultHandler;
         logbook.CommandFailed += logbook_CommandFailedHandler;
         logbook.CommandTimeout += logbook_CommandTimeoutHandler;
         logbook.Disconnected += logbook_DisconnectedHandler;
         
         if (logbook.IsActive)
         {
            UpdateLabel(statusLabel, "Connected");
         }
      }

      private void ConnectLogbookButton_Click(object sender, EventArgs e)
      {
         logbook.Activate();
         activateTimer = new System.Threading.Timer(activateTimerCallback, null, 0, 1000);         
      }

      private void disconnectButton_Click(object sender, EventArgs e)
      {
         Thread closeThread = new Thread(new ThreadStart(DeactivateLogbookThread));
         closeThread.Start();
      }

      private void DeactivateLogbookThread()
      {
         logbook.Deactivate();
      }

      private void activateTimerPing(object o)
      {
         if (logbook.IsActive)
         {
            UpdateLabel(statusLabel, "Connected");
         }
         else
         {
            UpdateLabel(statusLabel, "Not Connected");
         }
      }

      private void identifyButton_Click(object sender, EventArgs e)
      {
         if (logbook.IsActive)
         {
            logbook.sendCommand(Logbook.Command.InputPowerOn);
         }
      }

      private void testReadingButton_Click(object sender, EventArgs e)
      {
         if (logbook.IsActive)
         {
            logbook.sendCommand(Logbook.Command.ReadASCII);
         }
      }

      void logbook_Disconnected(object sender, EventArgs e)
      {
         if (activateTimer != null)
         {
            activateTimer.Dispose();
            activateTimer = null;
         }
         UpdateLabel(statusLabel, "Not Connected");
      }

      void logbook_CommandResult(object sender, CommandResultEventArgs e)
      {
         if (e.Command == Logbook.Command.ReadASCII)
         {
            UpdateLabel(testReadingLabel, Logbook.ConvertChars(Encoding.ASCII.GetString(e.Result, 0, e.Result.Length)));
         }
         if (e.Command == Logbook.Command.InputPowerOn)
         {
            logbook.sendCommand(Logbook.Command.IdentifySensors);
         }
         if (e.Command == Logbook.Command.IdentifySensors)
         {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (byte b in e.Result)
            {
               if (!first)
               {
                  sb.AppendFormat("[{0}]", (uint)b);
               }
               else
               {
                  first = false;
               }
            }
            sensorIDs = sb.ToString();
            gotIDs = true;
            logbook.sendCommand(Logbook.Command.GetSensorRanges);
         }
         if (e.Command == Logbook.Command.GetSensorRanges)
         {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (byte b in e.Result)
            {
               if (!first)
               {
                  sb.AppendFormat("[{0}]", (uint)b);
               }
               else
               {
                  first = false;
               }
            }
            sensorRanges = sb.ToString();
            UpdateLabel(identifyLabel, sensorRanges + "\r\n" + sensorIDs);
         }
         }

      void logbook_CommandTimeout(object sender, CommandTimeoutEventArgs e)
      {
         if (e.FailedCommand == Logbook.Command.ReadASCII)
         {
            UpdateLabel(testReadingLabel, "Timed out");
         }
         if (e.FailedCommand == Logbook.Command.InputPowerOn)
         {
            UpdateLabel(identifyLabel, "Power on Timed out");
         }
         if (e.FailedCommand == Logbook.Command.IdentifySensors)
         {
            UpdateLabel(identifyLabel, "Identify Timed out");
         }
         if (e.FailedCommand == Logbook.Command.GetSensorRanges)
         {
            UpdateLabel(identifyLabel, "Get Range Timed out");
         }
      }

      void logbook_CommandFailed(object sender, CommandFailedEventArgs e)
      {
         if (e.FailedCommand == Logbook.Command.ReadASCII)
         {
            UpdateLabel(testReadingLabel, "Failed");
         }
         if (e.FailedCommand == Logbook.Command.InputPowerOn)
         {
            UpdateLabel(identifyLabel, "Power on failed");
         }
         if (e.FailedCommand == Logbook.Command.IdentifySensors)
         {
            UpdateLabel(identifyLabel, "Identify Failed");
         }
         if (e.FailedCommand == Logbook.Command.GetSensorRanges)
         {
            UpdateLabel(identifyLabel, "Get Range Failed");
         }
      }

      void UpdateLabel(Label label, string text)
      {
         if (label.InvokeRequired)
         {
            label.Invoke(updateLabel, label, text);
         }
         else
         {
            DoUpdateLabel(label, text);
         }
      }

      void DoUpdateLabel(Label label, string text)
      {
         label.Text = text;
      }

      private void ConnectLogbookForm_Closing(object sender, CancelEventArgs e)
      {
         logbook.CommandResult -= logbook_CommandResultHandler;
         logbook.CommandFailed -= logbook_CommandFailedHandler;
         logbook.CommandTimeout -= logbook_CommandTimeoutHandler;
         logbook.Disconnected -= logbook_DisconnectedHandler;

         if (activateTimer != null)
         {
            activateTimer.Dispose();
            activateTimer = null;
         }

      }

   }
}