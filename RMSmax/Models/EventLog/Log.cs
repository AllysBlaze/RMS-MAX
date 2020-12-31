using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RMSmax.Models.EventLog
{
    public class Log
    {
        public DateTime Time { get; set; }
        public LogLevel LogLevel { get; set; }
        //public User Admin{get;set;}
        public string Message { get; set; }
        public string Comment { get; set; }

        public Log(LogLevel logLevel, string message, string comment = "")
        {
            Time = DateTime.Now;
            LogLevel = logLevel;
            Message = message;
            Comment = comment;
        }
        public Log(){}
    }
}
