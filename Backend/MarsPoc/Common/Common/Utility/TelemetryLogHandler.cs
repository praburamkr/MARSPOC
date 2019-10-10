using Common.Base;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class TelemetryLogHandler : LogHandlerBase
    {
        private TelemetryClient telemetryClient;        

        public TelemetryLogHandler(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
            this.telemetryClient.InstrumentationKey = "18450673-fb6a-4ff3-805e-0b571a75bab5";
        }

        public override void LogError(Exception ex, params object[] values)
        {
            Task.Run(() =>
            {
                this.telemetryClient.TrackException(ex, GetValueDictionary(values));
            });
        }

        public override void LogEvent(string eventName, params object[] values)
        {
            Task.Run(() =>
            {
                this.telemetryClient.TrackEvent(eventName, GetValueDictionary(values));
            });
        }

        public override void LogMessage(params object[] values)
        {
            Task.Run(() =>
            {
                this.telemetryClient.TrackTrace(GetParamsString(values, "|"));
            });
        }

        public override void LogMetric(string metricName, double metricValue, params object[] values)
        {
            Task.Run(() =>
            {
                this.telemetryClient.TrackMetric(metricName, metricValue, GetValueDictionary(values));
            });
        }

        private Dictionary<string, string> GetValueDictionary(object[] values)
        {
            if (values == null || values.Length == 0)
                return null;

            Dictionary<string, string> msgMap = new Dictionary<string, string>();

            for (int i = 0; i < values.Length; i++)
                msgMap.Add(i.ToString(), GetObjectString(values[i]));

            return msgMap;
        }
    }
}
