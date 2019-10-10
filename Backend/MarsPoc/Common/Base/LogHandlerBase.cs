using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Base
{
    public abstract class LogHandlerBase : ILogHandler
    {
        public abstract void LogError(params object[] values);
        public abstract void LogMessage(params object[] values);

        protected string GetParamsString(IEnumerable<object> values, string separator)
        {
            if (values == null)
                return "NOTHING";

            List<string> strList = new List<string>();

            foreach (var val in values)
            {
                if (val == null)
                    strList.Add("NULL");
                else if (val is IDictionary)
                    strList.Add("Dictionary: (" + GetDictionaryString(val as IDictionary) + ")");
                else if ((val is IEnumerable) && !(val is string))
                {
                    string str = "(" + GetParamsString((val as IEnumerable).Cast<object>(), ",") + ")";
                    strList.Add(str);
                }
                else
                    strList.Add(val.ToString());
            }

            return string.Join(separator, strList.ToArray());
        }

        private string GetDictionaryString(IDictionary dict, string keyDelimiter = " - ", string kvpDelimiter = ",")
        {
            StringBuilder sb = new StringBuilder();

            var keyCol = dict.Keys;

            foreach (var key in keyCol)
            {
                sb.Append(key.ToString());
                sb.Append(keyDelimiter);
                sb.Append(dict[key].ToString());
                sb.Append(kvpDelimiter);
            }

            return sb.ToString();
        }
    }
}
