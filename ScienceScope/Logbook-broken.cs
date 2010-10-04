using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Data;

namespace ScienceScope
{
    /// <summary>
    /// A CommandResultEvent is raised when a the expected number of bytes 
    /// have been received.  EventArgs are a byte array Result continaing the response;
    /// </summary>
    public class CommandResultEventArgs : EventArgs
    {
        private readonly byte[] result;
        private readonly Logbook.Command command;
        //Constructor.
        public CommandResultEventArgs(byte[] resultGot, Logbook.Command newCommand)
        {
            result = new byte[resultGot.Length];
            result = resultGot;
            command = newCommand;
        }

        public byte[] Result
        {
            get { return result; }
        }

        public Logbook.Command Command
        {
            get { return command; }
        }
    }

    // Delegate declaration.
    public delegate void CommandResultEventHandler(object sender, CommandResultEventArgs e);
    /// <summary>
    /// Summary description for CommandTimeOutEventArgs.
    ///    CommandTimeoutEvent raised when no response is reveived after 
    ///    the specified timeout period.
    /// </summary>
    public class CommandTimeoutEventArgs : EventArgs
    {
        private readonly Logbook.Command failedCommand;
        private readonly byte[] failedResponse;

        //Constructor.
        public CommandTimeoutEventArgs(Logbook.Command command, byte[] responseSoFar)
        {
            failedCommand = command;

            failedResponse = new byte[responseSoFar.Length];
            failedResponse = responseSoFar;
        }

        public Logbook.Command FailedCommand
        {
            get { return failedCommand; }
        }

        public byte[] FailedResponse
        {
            get { return failedResponse; }
        }
    }

    // Delegate declaration.
    public delegate void CommandTimeoutEventHandler(object sender, CommandTimeoutEventArgs e);

    public class CommandQueueFullEventArgs : EventArgs
    {
        private readonly Logbook.Command failedCommand;

        //Constructor.
        public CommandQueueFullEventArgs(Logbook.Command command)
        {
            failedCommand = command;
        }

        public Logbook.Command FailedCommand
        {
            get { return failedCommand; }
        }
    }

    public delegate void CommandQueueFullEventHandler(object sender, CommandQueueFullEventArgs e);

    public class CommandFailedEventArgs : EventArgs
    {
        public byte[] response;
        public string reason;
        public Logbook.Command command;

        public CommandFailedEventArgs(Logbook.Command command, byte[] data, string reason)
        {
            this.command = command;
            this.response = new byte[data.Length];
            Array.Copy(data, response, data.Length);
            this.reason = reason;
        }

        public CommandFailedEventArgs(Logbook.Command command, byte[] data)
        {
            this.command = command;
            this.response = new byte[data.Length];
            Array.Copy(data, response, data.Length);
            this.reason = "";
        }

        public CommandFailedEventArgs(Logbook.Command command)
        {
            this.command = command;
            this.response = new byte[0];
            this.reason = "";
        }   
    }

    public delegate void CommandFailedEventHandler(object sender, CommandFailedEventArgs e);

    public class LogbookConnectionException : IOException
    {
        public LogbookConnectionException()
        {
        }
        public LogbookConnectionException(string message)
            : base(message)
        {
        }
        public LogbookConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


    public class SensorID
    {
        public int ID;
        public int Range;
    }

    public class SensorDefinition : SensorID
    {
        public string Name;
        public string ASCIIUnit;
        public string Unit;
        public double MaxValue;
        public double MinValue;

        public string Description
        {
            get
            {
                if (ID == 255)
                {
                    return "Not Connected";
                }
                else
                {
                    string returnString = "";

                    if (Unit != null)
                    {
                        if (Name != null)
                        {
                            returnString = String.Format("{0} in {1}", Name, Unit);
                        }
                        else
                        {
                            returnString = String.Format("Reading in {0}", Unit);
                        }
                    }
                    else
                    {
                        returnString = "Unknown";
                    }
                    return returnString;
                }
            }
        }
    }
    
    /// <summary>
    /// Summary description for Logbook.
    /// Logbook class for ScienceScop WL/UL logbook
    /// handles serial IO and sending and recievign commands
    /// </summary>
    public class Logbook
    {
        public struct Command
        {
            public static readonly Command ReadASCII = new Command(new byte[] { Convert.ToByte(0x20) }, "Read data ASCII", 33, 7000);
            public static readonly Command GetVersion = new Command(new byte[] { Convert.ToByte(0x30) }, "Get Logbook version", 3, 600);
            public static readonly Command BatteryTest = new Command(new byte[] { Convert.ToByte(0x72) }, "Check battery status", 2, 600);
            public static readonly Command KeepAwake = new Command(new byte[] { Convert.ToByte(0x70) }, "Keep awake", 1, 500);
            public static readonly Command PowerDownAndStop = new Command(new byte[] { Convert.ToByte(0x74) }, "Power down and stop", 1, 500);
            public static readonly Command ConfirmExtendedProtocol = new Command(new byte[] { Convert.ToByte(0x52) }, "Confirm extended protocol", 1, 500);
            public static readonly Command IdentifySensors = new Command(new byte[] { Convert.ToByte(0x54) }, "Identify sensors", 5, 500);
            public static readonly Command GetTime = new Command(new byte[] { Convert.ToByte(0x84) }, "Read time and date", 5, 500);
            public static readonly Command GetBatteryLevel = new Command(new byte[] { Convert.ToByte(0xAA) }, "Get battery level", 2, 500);
            public static readonly Command GetSensorRanges = new Command(new byte[] { Convert.ToByte(0x8A) }, "Get sensor ranges", 5, 500);
//            public static readonly Command GetInputIDs = new Command(new byte[] { Convert.ToByte(0xE8) }, "Get input IDs", 17, 500);
            public static readonly Command InputPowerOn = new Command(new byte[] { Convert.ToByte(0x36) }, "Turn input power on", 1, 500);

            public byte[] command;
            public string name;
            public int responseLength;
            public int timeout;
            public byte commandConfirm;

            public Command(byte[] command, string name, int responseLength, int timeout, byte commandConfirm)
            {
                this.command = command;
                this.name = name;
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = commandConfirm;
            }

            public Command(byte[] command, int responseLength, int timeout)
            {
                this.command = command;
                this.name = "";
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = Convert.ToByte(Convert.ToInt16(command[0]) - 1);
            }

            public Command(byte[] command, string name, int responseLength, int timeout)
            {
                this.command = command;
                this.name = name;
                this.responseLength = responseLength;
                this.timeout = timeout;
                this.commandConfirm = Convert.ToByte(Convert.ToInt16(command[0]) - 1);
            }
        }

        public static SerialPort port;

        public bool IsConnected
        {
            get { return port.IsOpen; }
        }

        private int keepAwakeTime = 18000;  // Time between KeepAwake commands
        private System.Threading.Timer keepAwakeTimer;
        private TimerCallback keepAwakeTimerDelegate;

        private ArrayList inputBuffer; // current data recieved

        // the current command executing
        private Command currentCommand;

        // a list of commands to send
        private Queue<Command> commandQueue = new Queue<Command>();

        // the timer and event handler for current command
        private System.Threading.Timer commandTimer;
        private TimerCallback commandTimerDelegate;

        // true whilst waiting for a response or timeout 
        //   (prevents sending two commands at the same time)
        private bool waitingForResponse;
        private bool responseReceived; // when true no CommandTimeOutException will be raied on timeout

        // The event member that is of type CommandResultEventHandler.
        public event CommandResultEventHandler CommandResult;
        // The protected OnCommandResult method raises the event by invoking 
        // the delegates. The sender is always this, the current instance 
        // of the class.
        protected virtual void OnCommandResult(CommandResultEventArgs e)
        {
            if (CommandResult != null)
            {
                // Invokes the delegates. 
                CommandResult(this, e);
                // Send next command in queue
                sendNextCommand();
            }
        }


        // The event member that is of type CommandResultEventHandler.
        public event CommandTimeoutEventHandler CommandTimeout;
        // The protected OnCommandResult method raises the event by invoking 
        // the delegates. The sender is always this, the current instance 
        // of the class.
        protected virtual void OnCommandTimeout(CommandTimeoutEventArgs e)
        {
            if (CommandTimeout != null)
            {
                // Invokes the delegates. 
                CommandTimeout(this, e);
                sendNextCommand();
            }
        }


        public event CommandFailedEventHandler CommandFailed;

        protected virtual void OnCommandFailed(CommandFailedEventArgs e)
        {
            if (CommandFailed != null)
            {
                // Invokes the delegates. 
                CommandFailed(this, e);
                sendNextCommand();
            }
        }

        public event CommandQueueFullEventHandler CommandQueueFull;

        protected virtual void OnCommandQueueFull(CommandQueueFullEventArgs e)
        {
            if (CommandQueueFull != null) 
                CommandQueueFull(this, e);
        }

        public byte[] InputBuffer()
        {
            byte[] result = new byte[inputBuffer.Count];
            for (int i = 0; i < inputBuffer.Count; i++)
            {
                result[i] = (byte)inputBuffer[i];
            }
            return result;
        }

        public Logbook()
        {
            inputBuffer = new ArrayList();

            waitingForResponse = false;
            responseReceived = false;

            // create a default port on COM8
            port = new SerialPort("COM8:");

            // send/receive all data as it is queued
            // so 1 byte at a time is the best way to go
            port.ReceivedBytesThreshold = 1;	// get an event for every 1 byte received

            // define an event handler
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            commandTimerDelegate = new TimerCallback(CommandTimedOut);

            //keepAwakeTimerDelegate = new TimerCallback(SendKeepAwake);

        }

        // Distructor
        ~Logbook()
        {
            // kill timers
            if (waitingForResponse)
            {
                waitingForResponse = false;
                commandTimer.Dispose();
            }

            //keepAwakeTimer.Dispose();
            // release any port resources
            if (IsConnected)
            {
                port.Close();
            }
        }

        public void Connect()
        {
            if (port.IsOpen)
            {
                port.Close();    
            }

            try
            {
                port.Open();
            }
            catch (Exception e)
            {
                throw new LogbookConnectionException(String.Format("Connection couldn't be made: {0}", e.Message), e);
            }
            //keepAwakeTimer = new Timer(keepAwakeTimerDelegate, null, 0, keepAwakeTime);            
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                //sendCommand(Logbook.Command.PowerDownAndStop);
                try
                {
                    port.Close();
                }
                catch (InvalidOperationException e)
                {
                    throw new LogbookConnectionException(String.Format("Connection couldn't be broken: {0}", e.Message));
                }
                //keepAwakeTimer.Dispose();
            }
        }

        protected void updateSensors()
        {
            sendCommand(Command.IdentifySensors);
        }

        #region Event handlers

        private void SendKeepAwake(object o)
        {
            if (IsConnected)
            {
                sendCommand(Logbook.Command.KeepAwake);
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
                inputBuffer.Add(byteReceived);
                if (inputBuffer.Count == currentCommand.responseLength)
                {
                    GotResult();
                    // clear the input buffer
                    inputBuffer.Clear();
                }
            }
        }

        private void GotResult()
        {
            // got the right amount of bytes in the response
            // so disable the timeout timer and then 
            // check the confirmation char
            commandTimer.Dispose();

            lock (this)
            {
                waitingForResponse = false;
                if (InputBuffer()[0] == currentCommand.commandConfirm)
                {
                    responseReceived = true;
                    CommandResultEventArgs ev = new CommandResultEventArgs(this.InputBuffer(), currentCommand);
                    OnCommandResult(ev);
                }
                else
                {
                    //raise an event to say the command wasn't confirmed
                    OnCommandFailed(new CommandFailedEventArgs(currentCommand, InputBuffer(), "Logbook didn't confirm command!"));
                }
            }
        }

        private void CommandTimedOut(Object o)
        {
            commandTimer.Dispose();
            waitingForResponse = false;

            if (responseReceived == false)
            {
                CommandTimeoutEventArgs e = new CommandTimeoutEventArgs(currentCommand, InputBuffer());
                OnCommandTimeout(e);
                inputBuffer.Clear();
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

        private Mutex sendingCommandMutex = new Mutex();

        // Send the command and block for timeout peroid 
        // if no response has been reveived then raise an exception
        // runs in it's own thread 
        private void sendNextCommand()
        {
            sendingCommandMutex.WaitOne();
            Command commandToSend = commandQueue.Peek();
            try
            {

                if (!IsConnected)
                {
                    this.Connect();
                }
                if (IsConnected)
                {
                    if ((commandQueue.Count > 0) && (waitingForResponse == false))
                    {
                        //keepAwakeTimer.Change(0, keepAwakeTime);
                        foreach (int singleByte in commandToSend.command)
                        {
                            sendHex(singleByte);
                        }
                        currentCommand = commandToSend;
                        waitingForResponse = true;
                        responseReceived = false;
                        commandTimer = new System.Threading.Timer(commandTimerDelegate, null, currentCommand.timeout, Timeout.Infinite);
                    }
                }
                else
                {
                    throw new LogbookConnectionException("Connection to logbook failed");
                }
            }
            catch (IOException ex)
            {
                OnCommandFailed(new CommandFailedEventArgs(commandToSend));
                throw (new LogbookConnectionException("Can't connect to Logbook", ex));
            }
            finally
            {
                sendingCommandMutex.ReleaseMutex();
            }
        }

        protected int commandQueueMax = 10;

        public void sendCommand(Command commandToSend)
        {
            if (IsConnected )
            {
                if (commandQueue.Count >= commandQueueMax)
                {
                    OnCommandQueueFull(new CommandQueueFullEventArgs(commandToSend));
                }
                else
                {
                    commandQueue.Enqueue(commandToSend);
                    if ( waitingForResponse == false )
                    {
                        sendNextCommand();
                    }
                }
            }
        }

        #region static functions for dealing with data

        /// <summary>
        /// Translate special characters from the logbook into windows/ASCII printable versions.
        /// </summary>
        public static string ConvertChar(byte letterAsByte)
        {
            string printChar;
            switch (letterAsByte)
            {
                case 0:
                    printChar = "l";
                    break;
                case 3:
                    printChar = "²";
                    break;
                case 4:
                    printChar = "³";
                    break;
                case 5:
                    printChar = "-1";
                    break;
                case 6:
                    printChar = "-²";
                    break;
                case 7:
                    printChar = "-³";
                    break;
                case 0xDF:
                    printChar = "°";
                    break;
                case 0xE4:
                    printChar = "µ";
                    break;
                case 31:
                    printChar = " ";
                    break;
                default:
                    byte[] letterAsByteArray = new byte[1];
                    letterAsByteArray[0] = letterAsByte;
                    printChar = Encoding.ASCII.GetString(letterAsByteArray, 0, 1);
                    break;
            }
            return printChar;
        }

        public static string ConvertChars(string letters)
        {
            string result = "";
            foreach (char letter in letters)
            {
                result += ConvertChar((byte)letter);
            }
            return result;
        }

        /// <summary>
        /// Splits the ASCII string from ReadASCII command into an string array with an element or each of the 
        ///  four channels.  Will throw exceptions if strings aren't right.
        /// </summary>
        /// <param name="ASCIIData">ASCII string from ReadASCII command result</param>
        /// <returns>Four element array of strings representing channles 1, 2 3 and 4</returns>
        public static string[] SplitChannels(byte[] ASCIIData)
        {
            string[] result = new string[4] { "", "", "", "" };

            for (int i = 1; i < 9; i++)
            {
                result[0] += ConvertChar(ASCIIData[i]);
            }

            for (int i = 17; i < 25; i++)
            {
                result[1] += ConvertChar(ASCIIData[i]);
            }

            for (int i = 10; i < 17; i++)
            {
                result[2] += ConvertChar(ASCIIData[i]);
            }

            for (int i = 26; i <= 32; i++)
            {
                result[3] += ConvertChar(ASCIIData[i]);
            }
            return result;
        }

        #endregion

    }
}
