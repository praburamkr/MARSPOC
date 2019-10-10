using Common.Base;
using System;

namespace Common.Utility
{
    public class ConsoleLogHandler : LogHandlerBase
    {
        public override void LogMessage(params object[] values)
        {
            string str = GetParamsString(values, "|");

            Console.WriteLine(str);
        }

        public override void LogError(Exception ex, params object[] values)
        {
            string msg = ex.ToString();
            if (values != null)
                msg += " -- " + GetParamsString(values, "|");

            Console.Error.WriteLine(msg);
        }

        public override void LogEvent(string eventName, params object[] values)
        {
            string msg = eventName;
            if (values != null)
                msg += " -- " + GetParamsString(values, "|");

            Console.WriteLine(msg);
        }

        public override void LogMetric(string metricName, double metricValue, params object[] values)
        {
            string msg = metricName + " = " + metricValue;
            if (values != null)
                msg += " -- " + GetParamsString(values, "|");

            Console.WriteLine(msg);
        }
    }
}
