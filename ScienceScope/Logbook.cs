using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace ScienceScope
{
   /// <summary>
   /// Summary description for  
   /// Logbook class for ScienceScop WL/UL logbook
   /// handles serial IO and sending and recievign Commands
   /// </summary>
   public partial class Logbook
   {
      public static SerialPort port;

      private int keepAwakeTime = 18000;  // Time between KeepAwake Commands
      private Timer keepAwakeTimer;
      private TimerCallback keepAwakeTimerDelegate;

      private MemoryStream inputBuffer; // current data recieved

      // the current Command executing
      private Command currentCommand;

      // Queue for Commands to to be sent
      private Queue<Command> commandQueue = new Queue<Command>();

      protected int commandQueueMax = 10;

      private AutoResetEvent commandQWait = new AutoResetEvent(true);
      private const int commandQWaitTimeOut = Timeout.Infinite;


      // the timer and event handler for current Command
      private System.Threading.Timer CommandTimer;
      private TimerCallback commandTimerDelegate;


      private bool active = false;
      public bool IsActive
      {
         get { return active; }
      }

      // true whilst waiting for a response or timeout 
      //   (prevents sending two Commands at the same time)
      private bool waitingForResponse;
      private bool responseReceived; // when true no CommandTimeOutException will be raied on timeout

      private bool disconnectAfterResult = false;

      public byte[] InputBuffer()
      {
         return inputBuffer.ToArray();
      }

      public Logbook(string portName)
      {
         inputBuffer = new MemoryStream();

         waitingForResponse = false;
         responseReceived = false;

         // create a default port on COM8
         port = new SerialPort(portName);

         // send/receive all data as it is queued
         // so 1 byte at a time is the best way to go
         port.ReceivedBytesThreshold = 1;	// get an event for every 1 byte received

         // define an event handler
         port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

         commandTimerDelegate = new TimerCallback(CommandTimedOut);

         //keepAwakeTimerDelegate = new TimerCallback(SendKeepAwake);
      }

      public Logbook()
         : this("COM8:")
      { }

      Thread sendCommandThread = null;
      public void Activate()
      {
         active = true;
         disconnectAfterResult = false;
         OpenPort();
         port.DiscardInBuffer();
         port.DiscardOutBuffer();
         sendCommandThread = new Thread(new ThreadStart(SendCommandThread));
         sendCommandThread.Start();
         Thread gotResultThread = new Thread(new ThreadStart(GotResultThread));
         gotResultThread.Start();
      }

      public void Deactivate()
      {
         active = false;
         commandQWait.Set();
         results.Clear();
         gotResult.Set();
         active = false;

         //Clear event handlers
         this.CommandResult = null;
         this.CommandQueueFull = null;
         this.CommandTimeout = null;
         this.CommandFailed = null;

         // kill timers
         if (waitingForResponse)
         {
            CommandTimer.Dispose();
            waitingForResponse = false;
         }
         //keepAwakeTimer.Dispose();
         if (sendCommandThread != null)
         {
            sendCommandThread.Join();
         }
         ClosePort();
      }

      private void BeginDisconnect()
      {
         disconnectAfterResult = true;
         if (!waitingForResponse)
         {
            ClosePort();
            FireOnDisconnected();
         }
      }


      public void OpenPort()
      {
         lock (port)
         {
            if (!port.IsOpen)
            {
               try
               {
                  port.Open();
                  //keepAwakeTimer = new Timer(keepAwakeTimerDelegate, null, 0, keepAwakeTime);  
               }
               catch (Exception e)
               {
                  throw new LogbookConnectionException(String.Format("Connection couldn't be made: {0}", e.Message), e);
               }
            }
         }
      }

      private void ClosePort()
      {
         lock (port)
         {
            if (port.IsOpen)
            {
               //sendCommand( Command.PowerDownAndStop);
               try
               {
                  port.Close();
               }
               catch (InvalidOperationException e)
               {
                  throw new LogbookConnectionException("Problem closing serial connection", e);
               }
               finally
               {
                  //keepAwakeTimer.Dispose();
               }
            }
         }
      }


      public void reconnect()
      {
         ClosePort();
         Thread.Sleep(500);
         OpenPort();
         port.DiscardInBuffer();
         port.DiscardOutBuffer();
         Thread.Sleep(500);
      }

      private void SendCommandThread()
      {
         Command commandToSend = null;
         while (IsActive)
         {
            // Wait until a command is queued
            if (commandQueue.Count == 0)
            {
               commandQWait.WaitOne(commandQWaitTimeOut, false);
            }
            lock (commandQueue)
            {

               if (commandQueue.Count > 0)
               {
                  commandToSend = commandQueue.Dequeue();
               }
               else
               {
                  commandToSend = null;
               }
            }
            if (commandToSend != null)
            {
               try
               {
                  foreach (int singleByte in commandToSend.command)
                  {
                     sendHex(singleByte);
                  }
                  currentCommand = commandToSend;
                  waitingForResponse = true;
                  responseReceived = false;
                  CommandTimer = new Timer(commandTimerDelegate, null, currentCommand.timeout, Timeout.Infinite);
               }
               catch
               {
                  FireCommandFailed(new CommandFailedEventArgs(commandToSend));
               }

            }
         }

      }


      protected void updateSensors()
      {
         sendCommand(Command.IdentifySensors);
      }

      #region Event handlers

      private void SendKeepAwake(object o)
      {
         if (IsActive)
         {
            sendCommand(Command.KeepAwake);
         }
      }

      private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
      {
         // Reset the keepawake timer
         //keepAwakeTimer.Change(0, keepAwakeTime);

         // Obtain the number of bytes waiting in the port's buffer
         int bytes = port.BytesToRead;

         // Create a byte array buffer to hold the incoming data
         byte[] dataReceived = new byte[bytes];

         // Read the data from the port and store it in our buffer
         port.Read(dataReceived, 0, bytes);

         // convert ASCII bytes to text
         //Encoding enc = Encoding.ASCII;
         //receivedString = enc.GetString(dataReceived, 0, dataReceived.Length);
         //MessageBox.Show(receivedString, "Received..", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1 );
         foreach (byte byteReceived in dataReceived)
         {
            inputBuffer.WriteByte(byteReceived);

            if (inputBuffer.Length == currentCommand.responseLength)
            {
               byte[] bufferCopy = InputBuffer();
               results.Enqueue(bufferCopy);
               // clear the input buffer
               inputBuffer.Close();
               inputBuffer = new MemoryStream();
               CommandTimer.Dispose();
               gotResult.Set();
            }
         }
      }

      AutoResetEvent gotResult = new AutoResetEvent(false);
      Queue<byte[]> results = new Queue<byte[]>();

      private void GotResultThread()
      {
         while (active)
         {
            gotResult.WaitOne();
            if (results.Count > 0)
            {
               byte[] input = results.Dequeue();

               // got the right amount of bytes in the response
               // so disable the timeout timer and then 
               // check the confirmation char
               waitingForResponse = false;
               if (input[0] == currentCommand.commandConfirm)
               {
                  responseReceived = true;
                  CommandResultEventArgs ev = new CommandResultEventArgs(input, currentCommand);
                  OnCommandResult(ev);
               }
               else
               {
                  //raise an event to say the Command wasn't confirmed
                  FireCommandFailed(new CommandFailedEventArgs(currentCommand, input, "Logbook didn't confirm Command!"));
               }
            }
         }
      }

      private void CommandTimedOut(Object o)
      {
         CommandTimer.Dispose();
         waitingForResponse = false;
         if (responseReceived == false)
         {
            CommandTimeoutEventArgs e = new CommandTimeoutEventArgs(currentCommand, InputBuffer());
            inputBuffer.Close();
            inputBuffer = new MemoryStream();
            FireOnCommandTimeout(e);
         }
      }

      private void FireOnDisconnected()
      {
         if (Disconnected != null)
         {
            Disconnected(this, new EventArgs());
         }
      }


      #endregion

      private void sendString(string message)
      {
         byte[] outputData = new byte[1];

         foreach (char letter in message)
         {
            outputData[0] = Convert.ToByte(letter);
            try
            {
               port.Write(outputData, 0, outputData.Length);
            }
            catch (FormatException e)
            {
               throw new LogbookConnectionException(String.Format("Can't send data: {0}", e.Message), e);
            }
         }
      }

      private void sendHex(int outputInt)
      {
         byte[] outputData = new byte[1];

         outputData[0] = Convert.ToByte(outputInt);
         try
         {
            port.Write(outputData, 0, 1);
         }
         catch (FormatException e)
         {
            throw new LogbookConnectionException(String.Format("Can't send data: {0}", e.Message), e);
         }
      }

      /// <summary>
      /// Convert a byte into a byte array with 1 element
      /// </summary>
      private byte[] toByteArray(byte byteToConvert)
      {
         byte[] result = new byte[1];
         result[0] = Convert.ToByte(byteToConvert);
         return result;
      }


      public void sendCommand(Command commandToSend)
      {
         if (active)
         {
            if (commandQueue.Count >= commandQueueMax)
            {
               OnCommandQueueFull(new CommandQueueFullEventArgs(commandToSend));
            }
            else
            {
               commandQueue.Enqueue(commandToSend);
               if (waitingForResponse == false)
               {
                  commandQWait.Set();
               }
            }
         }
         else
         {
            FireCommandFailed(new CommandFailedEventArgs(commandToSend));
         }
      }

   }
}
