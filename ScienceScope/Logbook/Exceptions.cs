using System;
using System.IO;

namespace ScienceScope
{
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
}