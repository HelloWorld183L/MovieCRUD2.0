using System;

namespace MovieCRUD.Infrastructure.Logging
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogError(Exception exception, string message);
        void LogDebug(string message);
    }
}
