using AgroControlUI.Services.Interfaces;
using Serilog;

namespace AgroControlUI.Services.Implementations
{
    public class LoggerService : ILoggerService
    {
        public void LogInformation(string message)
        {
            Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public void LogError(string message, Exception exception)
        {
            Log.Error(exception, message);
        }
    }

}
