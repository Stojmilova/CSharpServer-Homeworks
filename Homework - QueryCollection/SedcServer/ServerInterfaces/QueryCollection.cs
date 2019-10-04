using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ServerInterfaces
{
    public class QueryCollection
    {
        /// <summary>
        /// Dictionary don't allowed duplicates keys;
        /// The List class for KeyValuePair, for Tuple, allowed containing duplicates keys;:
        /// </summary>

        //private readonly Dictionary<string, string> queries = new Dictionary<string, string>();
        //private readonly List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

        private readonly List<Tuple<string, string>> queries = new List<Tuple<string, string>>();
        public QueryCollection(List<Tuple<string, string>> initialData)
        {
            queries = initialData;
        }
        public static QueryCollection Empty
        {
            get
            {
                return new QueryCollection(new List<Tuple<string, string>>());
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Tuple<string, string> query in queries)
            {
                sb.AppendLine($"Key = {query.Item1}, Value {query.Item2}");

            }
            return sb.ToString();
        }
    }
}
