using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    public class LoggerInitializer
    {
        public static void InitializeLogger(string workingDirectory)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget
            {
                FileName = Path.Combine(workingDirectory, @"Logs\SupportBank.log"),
                Layout = @"${longdate} ${level} - ${logger}: ${message}"
            };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
        }
    }
}