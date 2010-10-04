using System;
using System.Collections.Generic;

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
        public CommandTimeoutEventArgs(Logbook.Command Command, byte[] responseSoFar)
        {
            failedCommand = Command;

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
        public CommandQueueFullEventArgs(Logbook.Command Command)
        {
            failedCommand = Command;
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
        private readonly Logbook.Command command;

        public Logbook.Command FailedCommand
        {
            get { return command; }
        }

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

    public partial class Logbook
    {
        // The event member that is of type CommandResultEventHandler.
        public event CommandResultEventHandler CommandResult;
        // The protected OnCommandResult method raises the event by invoking 
        // the delegates. The sender is always this, the current instance 
        // of the class.
        protected virtual void OnCommandResult(CommandResultEventArgs e)
        {
            commandFailedCount = 0;
            if (CommandResult != null)
            {
                // Invokes the delegates. 
                CommandResult(this, e);
                // Send next Command in queue
                if (disconnectAfterResult)
                {
                    ClosePort();
                    FireOnDisconnected();
                }
                else
                {
                    commandQWait.Set();
                }
            }
        }

        public event EventHandler Disconnected;

        // The event member that is of type CommandResultEventHandler.
        public event CommandTimeoutEventHandler CommandTimeout;
        // The protected OnCommandResult method raises the event by invoking 
        // the delegates. The sender is always this, the current instance 
        // of the class.
        protected virtual void FireOnCommandTimeout(CommandTimeoutEventArgs e)
        {
            if (CommandTimeout != null)
            {
                // Invokes the delegates. 
                CommandTimeout(this, e);
                if (disconnectAfterResult)
                {
                    ClosePort();
                    FireOnDisconnected();
                }
                else
                {
                    commandQWait.Set();
                }
            }
        }

        public event CommandFailedEventHandler CommandFailed;

        private int commandFailedCount = 0;
        protected virtual void FireCommandFailed(CommandFailedEventArgs e)
        {
            //See how many Commands have failed...
            if (commandFailedCount < 2)
            {
                //Don't bother resending if another Command has already failed and been retried
                commandFailedCount++;

                if (commandFailedCount == 1)
                {
                    //If the first resend didn't work then try to reconnect  
                    if (active)
                    {
                        reconnect();
                    }
                }
                //Make sure no Commands are going to be processed
                lock (commandQueue)
                {
                    port.DiscardInBuffer();
                    //port.DiscardOutBuffer();
                    //then requeue the failed Command to try again.
                    Queue<Command> newCommandQueue = new Queue<Command>(commandQueueMax + 1);
                    newCommandQueue.Enqueue(e.FailedCommand);
                    while (commandQueue.Count > 0)
                    {
                        newCommandQueue.Enqueue(commandQueue.Dequeue());
                    }
                    commandQueue = newCommandQueue;
                }
                commandQWait.Set();
            }
            else
            {
                if (CommandFailed != null)
                {
                    // Invokes the delegates. 
                    CommandFailed(this, e);
                }
                if (disconnectAfterResult)
                {
                    ClosePort();
                    FireOnDisconnected();
                }
                else
                {
                    commandQWait.Set();
                }
            }
        }

        public event CommandQueueFullEventHandler CommandQueueFull;

        protected virtual void OnCommandQueueFull(CommandQueueFullEventArgs e)
        {
            if (CommandQueueFull != null)
                CommandQueueFull(this, e);
        }

    }

}