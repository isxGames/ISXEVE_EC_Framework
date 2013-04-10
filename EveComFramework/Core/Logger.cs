using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveComFramework.Core
{
    /// <summary>
    /// Handles logging and feedback, allows multiple events to collect feedback info
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Delegate for log events
        /// </summary>
        /// <param name="Message">Message that is being logged</param>
        public delegate void LogEvent(string Message);
        public event LogEvent Event;
        /// <summary>
        /// Send a log event
        /// </summary>
        /// <param name="Message">Message, may contain {0} type tokens like string.Format</param>
        /// <param name="Params">Paramters to insert into the message format string</param>
        public void Log(string Message, params object[] Params)
        {
            Event(string.Format(Message, Params));
        }
    }
}
