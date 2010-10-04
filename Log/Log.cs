using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace mcp.Logs
{
   public class LogMessageEventArgs : EventArgs
   {
      private readonly int level;
      private readonly string tags;
      private readonly string message;
      private readonly object source;

      public LogMessageEventArgs(int level, string message, object source)
      {
         this.level = level;
         this.message = message;
         this.source = source;
         this.tags = String.Format("{0}", level);
      }

      public LogMessageEventArgs(string tags, string message, object source)
      {
         this.tags = tags;
         this.message = message;
         this.source = source;
      }
      public int Level
      {
         get { return level; }
      }

      public string Tags
      {
         get { return tags; }
      }

      public string Message
      {
         get { return message; }
      }

      public object Source
      {
         get { return source; }
      }
   }

   public delegate void LogMessageEventHandler(object sender, LogMessageEventArgs e);

   public class Log
   {
      private int _writeLogLevel = 0;

      private string _name = "";
      public string Name
      {
         get { return this._name; }
         set { this._name = value; }
      }


      private bool active = false;
      private bool Running
      {
         get { return active; }
         set
         {
            if ((value == true) && !active)
            {
               ThreadPool.QueueUserWorkItem(LogSender);
            }
            else
            {
               if ((value == false) && active)
               {
                  logEventWait.Set();
                  Thread.Sleep(0);
               }
            }
            active = value;
         }
      }

      public bool IsRunning()
      {
         return active;
      }

      // When a message is logged the event is thrown and caught by event handler
      // this allows a single logging system to be used
      // accross multiple components.
      public event LogMessageEventHandler LogMessage;

      protected virtual void FireOnLogMessage(LogMessageEventArgs e)
      {
         logQueue.Enqueue(e);
         logEventWait.Set();
      }

      public Log()
      {
         this.Start();
      }

      private AutoResetEvent logEventWait = new AutoResetEvent(false);
      private Queue<LogMessageEventArgs> logQueue = new Queue<LogMessageEventArgs>();

      private void LogSender(object o)
      {
         while (this.IsRunning())
         {
            if (logQueue.Count == 0)
            {
               logEventWait.WaitOne(Timeout.Infinite, false);
            }
            lock (logQueue)
            {
               if (logQueue.Count > 0)
               {
                  LogMessageEventArgs args = logQueue.Dequeue();
                  if ((LogMessage != null) && (args != null))
                     LogMessage(args.Source, args);

               }
            }
         }
      }

      public void setLogFile(string logFile)
      {
         this._name = logFile;
      }

      public string getLogFile(string logFile)
      {
         return this._name;
      }

      public int WriteLogLevel
      {
         get { return _writeLogLevel; }
         set { _writeLogLevel = value; }
      }

      public void Start()
      {
         this.Running = true;
      }

      public void Stop()
      {
         this.Running = false;
         logEventWait.Set();
      }

      public void Clear()
      {
         writeMutex.WaitOne(1000, false);
         try
         {
            if (_name.Length > 0)
               if (File.Exists(_name))
                  File.Delete(_name);
         }
         finally
         {
            writeMutex.ReleaseMutex();
         }
      }

      private static string escapeString(string input)
      {
         return Regex.Replace(input, "\"", "\\\"");
      }

      private static string escapeSQL(string st)
      {
         string str = Regex.Replace(st, "'", "''");
         str = Regex.Replace(str, "\0", "\\0");
         return str;
      }

      public void WriteToFile(string message, int level)
      {
         this.WriteToFile(message, String.Format("{0}", level));
      }

      private Mutex writeMutex = new Mutex();
      public void WriteToFile(string message, string tags)
      {
         if ((_name.Length == 0) && (active == true))
         {
            throw (new ApplicationException("Can't write to log, no name set", null));
         }
         else
         {
            writeMutex.WaitOne(1000, false);
            StreamWriter logStream = null;
            try
            {
               logStream = File.AppendText(_name);
               logStream.WriteLine("{0} {1},\"{2}\",\"{3}\"",
                   DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(),
                   escapeString(tags), escapeString(message));
               logStream.Close();
            }
            catch (Exception ex)
            {
               throw (new ApplicationException(String.Format("Log error: {0}", ex.Message)));
            }
            finally
            {
               if (logStream != null)
               {
                  logStream.Close();
               }
               writeMutex.ReleaseMutex();
            }
         }
      }

      public void WriteToSQL(string message, int level, SQLiteConnection connection)
      {
         this.WriteToSQL(message, String.Format("{0}", level), connection);
      }

      public void WriteToSQL(string text, string tags, SQLiteConnection database)
      {
         this.WriteToSQL(database, tags + ": " + text);
      }

      public void WriteToSQL(SQLiteConnection database, string text)
      {
         if (_name == null)
         {
            throw (new Exception("Can't write to log, no name set", null));
         }
         if ((_name.Length == 0) && (active == true))
         {
            throw (new Exception("Can't write to log, no name set", null));
         }
         else
         {
            lock (database)
            {
               if (database.State != ConnectionState.Open)
               {
                  database.Open();
               }
               using (SQLiteCommand command = database.CreateCommand())
               {
                  // escape any special characters
                  string escapedMessage = escapeSQL(text);
                  if (escapedMessage.Length > 255)
                  {
                     escapedMessage = escapedMessage.Substring(0, 255);
                  }
                  command.CommandText = String.Format("INSERT INTO [{0}] ([time], [message]) VALUES ('{1}', '{2}')",
                      _name, DateTime.UtcNow.ToString("yyyy-MM-dd H:mm:ss"), escapedMessage);
                  //Debug.WriteLine(command.CommandText);
                  command.ExecuteNonQuery();
               }
               database.Close();
            }
         }
      }

      public void WriteToSQL(string text, string tags, string databaseConnectionString)
      {
         using (SQLiteConnection database = new SQLiteConnection(databaseConnectionString))
         {
            this.WriteToSQL(text, tags, database);
         }
      }

      /// <summary>
      /// Write a log message to the log file
      /// </summary>
      public void Append(string message)
      {
         Append(1, message);
      }

      public void Append(int logLevel, string message)
      {
         FireOnLogMessage(new LogMessageEventArgs(logLevel, message, this));
      }

      public void Append(string tags, string message)
      {
         FireOnLogMessage(new LogMessageEventArgs(tags, message, this));
      }

      public void LogException(Exception ex, string text)
      {
         string message = String.Format("Exception in application:\r\n{0}\r\nStatck Trace:\r\n{1}", ex.Message, ex.StackTrace);
         string tag = "Exception";
         if (text.Length > 0)
         {
            tag += " " + text;
         }
         Append(tag, message);
         if (ex.InnerException != null)
         {
            this.LogException(ex.InnerException);
         }
      }

      public void LogException(Exception ex)
      {
         LogException(ex, "");
      }
   }
}

