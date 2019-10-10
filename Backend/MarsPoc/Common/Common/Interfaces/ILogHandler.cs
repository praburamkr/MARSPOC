using System;

namespace Common.Interfaces
{
    public interface ILogHandler
    {
        void LogMessage(params object[] values);

        void LogError(Exception ex, params object[] values);

        void LogEvent(string eventName, params object[] values);

        void LogMetric(string metricName, double metricValue, params object[] values);
    }
}
