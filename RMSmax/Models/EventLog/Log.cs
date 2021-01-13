using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace RMSmax.Models.EventLog
{
    public class Log
    {
        public DateTime Time { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Admin {get;set;}
        public string Message { get; set; }
        public string Comment { get; set; }

        public Log(LogLevel logLevel, IdentityUser admin, string message, string comment = "")
        {
            Time = DateTime.Now;
            LogLevel = logLevel;
            Admin = admin.UserName;
            Message = message;
            Comment = comment;
        }
        public Log(LogLevel logLevel, string admin, string message, string comment = "")
        {
            Time = DateTime.Now;
            LogLevel = logLevel;
            Admin = admin;
            Message = message;
            Comment = comment;
        }
        public Log(){}
    }
}
