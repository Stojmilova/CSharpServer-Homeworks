using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerPlugins.SqlServer
{
    public class SqlHelpers
    {

        public static string GenerateJsonData(List<Dictionary<string, string>> data)
        {
            var objectStrings = data.Select(item => GenerateJsonData(item));

            var objectString = string.Join(",", objectStrings);
            var splitedObject = objectString.Split(",");

            StringBuilder sb = new StringBuilder();

            foreach (var item in splitedObject)
            {
                int separator = item.IndexOf(':');
                int curlyBrace = item.IndexOf("{");
                var key = item.Substring(curlyBrace + 2, separator - 2);
                var value = item.Substring(separator);
                sb.Append(" ");
                sb.Append(key);
                sb.AppendLine(value);
            }
            return sb.ToString();
        }

        public static string GenerateJsonData(Dictionary<string, string> item)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Join(",", item.Select(kvp => $"\"{kvp.Key}\": \"{kvp.Value}\"")));

            return sb.ToString();
        }
    }
}
