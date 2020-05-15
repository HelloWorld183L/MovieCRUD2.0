using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace MovieCRUD.Infrastructure.Logging
{
    public class LoggingConfig
    {
        private const string logPattern = "%date [%thread] %level %logger - %message%newline";
        private const string filePath = "F:\\MovieCRUD\\Logs\\log.txt";

        public static void Configure()
        {
            var layout = new PatternLayout(logPattern);
            var appender = new RollingFileAppender
            {
                File = filePath,
                Layout = layout,
                MaximumFileSize = "1000MB",
                StaticLogFileName = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                MaxSizeRollBackups = 5
            };
            layout.ActivateOptions();
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }
    }
}
