using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace RMSmax.Models.EventLog
{
    public static class EventLogs
    {
        private static string path;
        private static IWebHostEnvironment hostingEnvironment;
        private static ILogger consoleLogger;
        public static bool IsInitialized { get; private set; }
        public static IEnumerable<Log> Logs { get => Deserialize();}

        public static void Initialize(IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            if (!IsInitialized)
            {
                hostingEnvironment = environment;
                consoleLogger = loggerFactory.CreateLogger("EventLogs");

                path = Path.Combine(hostingEnvironment.WebRootPath, "logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, "events.json");
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "");
                }

                IsInitialized = true;
            }
        }

        private static void InitializeTest()
        {
            if(!IsInitialized)
                throw new InvalidOperationException("Object is not initialized");
        }
        private static IEnumerable<Log> Deserialize()
        {
            string data;
            try
            {
                data = File.ReadAllText(path);
            }
            catch (Exception)
            {
                data = "";
            }
            IEnumerable<Log> logs;
            if (string.IsNullOrEmpty(data))
                logs = new List<Log>();
            else
                logs = JsonSerializer.Deserialize<IList<Log>>(data);

            return logs.OrderByDescending(x => x.Time);
        }

        public static void Log(Log log)
        {
            InitializeTest();

            consoleLogger.Log(log.LogLevel, log.Message);

            IEnumerable<Log> logs = Deserialize();
            if(logs.Count() > 0)
                logs = logs.OrderBy(x => x.Time).Where(x => x.Time >= DateTime.Now.AddMonths(-3));

            IList<Log> list = logs.ToList();
            list.Add(log);

            string data = JsonSerializer.Serialize(list);
            try
            {
                File.WriteAllText(path, data);
            }
            catch (Exception)
            {
                consoleLogger.Log(LogLevel.Critical, "Can not save EventLogs");
            }
        }
        public static void LogInformation(IdentityUser userr, string message, string comment = "")
        {
            Log(new Log(LogLevel.Information, userr, message, comment));
        }

        public static void LogWarning(IdentityUser userr, string message, string comment = "")
        {
            Log(new Log(LogLevel.Warning, userr, message, comment));
        }

        public static void LogError(IdentityUser userr, string message, string comment = "")
        {
            Log(new Log(LogLevel.Error, userr, message, comment));
        }
    }
}
