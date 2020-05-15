using log4net;
using System;

namespace MovieCRUD.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        private static ILog _logger;

        public Logger()
        {
            LoggingConfig.Configure();
            _logger = LogManager.GetLogger("defaultLogger");
        }

        public void LogError(Exception exception, string message)
        {
            _logger.Error(message, exception);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }
    }
}
