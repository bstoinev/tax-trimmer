using NLog;
using System;

namespace DevOcean.TaxTrim.Cli
{
    public class NLogLoggingFacility<TSource> : ILoggingFacility<TSource>
        where TSource : class
    {
        private readonly Logger Log;
        private readonly Type LoggerType = typeof(TSource);

        public NLogLoggingFacility()
        {
            Log = LogManager.GetLogger(LoggerType.Name);
        }

        public void Debug(string message)
        {
            var info = new LogEventInfo(LogLevel.Debug, LoggerType.Name, message);
            Log.Log(LoggerType, info);
        }

        public void Error(Exception ex)
        {
            var info = new LogEventInfo(LogLevel.Error, LoggerType.Name, ex.ToString());
            Log.Log(LoggerType, info);
        }

        public void Error(string message)
        {
            var info = new LogEventInfo(LogLevel.Error, LoggerType.Name, message);
            Log.Log(LoggerType, info);
        }

        public void Info(string message)
        {
            var info = new LogEventInfo(LogLevel.Info, LoggerType.Name, message);
            Log.Log(LoggerType, info);
        }

        public void Trace(string message)
        {
            var info = new LogEventInfo(LogLevel.Trace, LoggerType.Name, message);
            Log.Log(LoggerType, info);
        }

        public void Warning(string message)
        {
            var info = new LogEventInfo(LogLevel.Warn, LoggerType.Name, message);
            Log.Log(LoggerType, info);
        }
    }
}
