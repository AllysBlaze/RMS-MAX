using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

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
            string data = File.ReadAllText(path);
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
            File.WriteAllText(path, data);
        }
        public static void LogInformation(string message, string comment = "")
        {
            Log(new Log(LogLevel.Information, message, comment));
        }

        public static void LogWarning(string message, string comment = "")
        {
            Log(new Log(LogLevel.Warning, message, comment));
        }

        public static void LogError(string message, string comment = "")
        {
            Log(new Log(LogLevel.Error, message, comment));
        }
    }
}
