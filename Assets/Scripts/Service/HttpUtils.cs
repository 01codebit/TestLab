using System.Collections.Generic;
using System.Text;

namespace TestLab.EventChannel
{
    public static class HttpUtils
    {
        public static string FixJson(string value)
        {
            if (value.StartsWith('['))
            {
                value = "{\"Items\":" + value + "}";
            }

            return value;
        }

        public static string GetRequestUri(string endpoint, Dictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(endpoint)) return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(endpoint);
            char separator = '?';
            //"todos?userId=1&completed=false"
            foreach (var key in parameters.Keys)
            {
                sb.Append(separator);
                sb.Append(key);
                sb.Append('=');
                sb.Append(parameters[key]);
                separator = '&';
            }

            return sb.ToString();
        }
    }
}