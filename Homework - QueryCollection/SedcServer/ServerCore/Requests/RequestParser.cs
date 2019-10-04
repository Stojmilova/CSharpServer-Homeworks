using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ServerInterfaces;

namespace ServerCore.Requests
{
    public class RequestParser
    {
        public static readonly Regex RequestLineRegex = new Regex(@"^([A-Z]+)\s\/([^\s?]*)\??([^\s]*)\sHTTP\/1\.1$");
        public static readonly Regex HeaderRegex = new Regex(@"^([^:]*):\s*(.*)$");

        public Request Parse(string requestData)
        {
            var lines = requestData.Split(Environment.NewLine);
            var requestLine = lines.First();
            var match = RequestLineRegex.Match(requestLine);
            if (!match.Success)
            {
                throw new ApplicationException("Unable to process request");
            }
            var method = ParseHelper.ParseMethod(match.Groups[1].Value);
            if (method == Method.None)
            {
                throw new ApplicationException($"Unable to match {match.Groups[1].Value} to an available method");
            }

            //PATH
            var path = match.Groups[2].Value;

            //QUERY with Tuple

            var queryLine = match.Groups[3].Value;
            var queryRegex = @"([^?=&]+)(=([^&]*))?";

            var queryTuple = new List<Tuple<string, string>>();
            MatchCollection matches = Regex.Matches(queryLine, queryRegex);

            foreach (Match queryMatch in matches)
            {
                var queryKey = queryMatch.Groups[1].Value;
                var queryValue = queryMatch.Groups[2].Value;

                queryTuple.Add(new Tuple<string, string>(queryKey, queryValue));
            }
            QueryCollection queries = new QueryCollection(queryTuple);

            //QUERY with Dictionary

            //var query = match.Groups[3].Value;          

            //var result = Regex.Matches(query, "([^?=&]+)(=([^&]*))?")
            //    .Cast<Match>()
            //    .ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);

            //QueryCollection queries = new QueryCollection(result);

            //HEADER
            var headerLines = lines.Skip(1).TakeWhile(line => !string.IsNullOrEmpty(line));
            var headerDict = new Dictionary<string, string>();

            foreach (var line in headerLines)
            {
                var hmatch = HeaderRegex.Match(line);
                if (!hmatch.Success)
                {
                    throw new ApplicationException($"Unable to process header line {line}");
                }
                var headerName = hmatch.Groups[1].Value;
                var headerValue = hmatch.Groups[2].Value;
                headerDict.Add(headerName, headerValue);
            }

            HeaderCollection headers = new HeaderCollection(headerDict);

            var bodyLines = lines.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);
            var body = string.Join(Environment.NewLine, bodyLines);

            return new Request
            {
                Method = method,
                Path = path,
                Queries = queries,
                Headers = headers,
                Body = body
            };
        }
    }
}
