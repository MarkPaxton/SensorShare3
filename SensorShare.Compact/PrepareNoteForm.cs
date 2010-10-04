using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using mcp.Logs;
using SensorShare;
using SensorShare.Network;
using mcp;

namespace SensorShare.Compact
{
   public partial class PrepareNoteForm : Form
   {
      NetworkNode client = null;
      ServerData currentServer = null;
      Log log = null;

      string dbConnectionString = null;

      MessageEventHandler receiveHandler;

      long timeOffset = 0;

      DialogResultDelegeate closeDelegate;
      VoidDelegate updateServerDelegate;
      VoidDelegate timeDelegate;
      ButtonStringBoolDelegate toggleSendButtonDelegate;
      System.Threading.Timer clockTimer;
      System.Threading.TimerCallback clockCallback;

      public String Note
      {
         get
         {
            if (textBox1 != null)
            {
               return textBox1.Text;
            }
            else
            {
               return "";
            }
         }
      }

      public PrepareNoteForm(NetworkNode client, string dbConnectionString, Log log)
      {
         this.client = client;
         this.dbConnectionString = dbConnectionString;
         this.log = log;

         InitializeComponent();

         this.updateServerDelegate = new VoidDelegate(UpdateCurrentServerDetails);
         this.receiveHandler = new MessageEventHandler(client_MessageReceived);
         this.toggleSendButtonDelegate = new ButtonStringBoolDelegate(ToggleButton);
         this.closeDelegate = new DialogResultDelegeate(DoClose);
         this.timeDelegate = new VoidDelegate(UpdateTime);
         this.clockCallback = new System.Threading.TimerCallback(ClockTimerTimedout);
      }


      public void SetCurrentServer(ServerData server)
      {
         if (server != null)
         {
            log.Append("SetCurrentServer", "Showing server " + server.name + " " + server.id);
            if (currentServer == null)
            {
               client.MessageReceived += receiveHandler;
            }
            clockTimer = new System.Threading.Timer(clockCallback, null, 2000, 1000);
            this.currentServer = server;
            ThreadPool.QueueUserWorkItem(new WaitCallback(SaveServerDetails)); 
         }
         else
         {
            log.Append("SetCurrentServer", "Clearing server details");
            if (currentServer != null)
            {
               client.MessageReceived -= receiveHandler;
               if(clockTimer!=null)
               {
                  clockTimer.Dispose();
                  clockTimer=null;
               }
            }
            currentServer = null;
         }
         InvokeUpdateCurrentServerDetails();
      }

      private void SaveServerDetails(object thing)
      {
         if (currentServer != null)
         {
            ServerData server = currentServer;
            try
            {
               using (SQLiteConnection dbConnection = new SQLiteConnection(dbConnectionString))
               {
                  bool serverExists = DatabaseHelper.GetServerSaved(dbConnection, currentServer.id);
                  if (!serverExists)
                  {
                     DatabaseHelper.SaveServerConfigData(dbConnection, currentServer);
                  }
               }
            }
            catch (Exception ex)
            {
               log.LogException(ex, "PrepareNoteForm database");
            }
         }
      }

      private void InvokeUpdateCurrentServerDetails()
      {
         if (this.InvokeRequired)
         {
            this.Invoke(updateServerDelegate);
         }
         else
         {
            UpdateCurrentServerDetails();
         }
      }

      private void UpdateCurrentServerDetails()
      {
         if (currentServer != null)
         {
            this.serverTitle.Text = currentServer.name;
            this.textBox1.Text = "";
            this.textBox1.Focus();

            ChangeButton("Send", true);
         }
         else
         {
            this.serverTitle.Text = "";
            ChangeButton("Send", false);
         }
      }


      void ClockTimerTimedout(object o)
      {
         if (timeLabel.InvokeRequired)
         {
            timeLabel.Invoke(timeDelegate);
         }
         else
         {
            UpdateTime();
         }
      }


      void client_MessageReceived(object sender, MessageEventArgs a)
      {
         TypedMessage message = new TypedMessage(a.Data, a.Data.Length);
         switch (message.type)
         {
            case MessageType.SensorReadingsData:
               SensorReadingsData data = MessageHelper.DeserializeSensorReadingsData(message.data);
               this.timeOffset = data.Time.Ticks - DateTime.Now.Ticks;
               break;
         }
      }

      void DoCloseInvoke(DialogResult result)
      {
         if (this.clockTimer != null)
         {
            clockTimer.Dispose();
            clockTimer = null;
         }
         if (this.InvokeRequired)
         {
            this.Invoke(closeDelegate, result);
         }
         else
         {
            DoClose(result);
         }
      }

      void UpdateTime()
      {
         timeLabel.Text = DateTime.Now.AddTicks(timeOffset).ToLongTimeString();
      }

      private void cancelButton_Click(object sender, EventArgs e)
      {
         DoCloseInvoke(DialogResult.Cancel);
      }

      private void DoClose(DialogResult result)
      {
         SetCurrentServer(null);
         this.DialogResult = result;
      }

      private void saveButton_Click(object sender, EventArgs e)
      {
         Debug.WriteLine(String.Format("Note {0}", textBox1.Text));
         DateTime noteTime = DateTime.Now.AddTicks(timeOffset);
         ChangeButton("Sending...", false);
         DoCloseInvoke(DialogResult.OK);
      }

      private void ChangeButton(string text, bool value)
      {
         if (this.sendButton.InvokeRequired)
         {
            sendButton.Invoke(toggleSendButtonDelegate, new object[] { sendButton, text, value });
         }
         else
         {
            ToggleButton(sendButton, text, value);
         }

      }

      private void ToggleButton(Button but, string text, bool value)
      {
         but.Text = text;
         but.Enabled = value;
      }

      private void PrepareNoteForm_Closing(object sender, CancelEventArgs e)
      {
         SetCurrentServer(null);
      }

   }
}