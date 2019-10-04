using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerInterfaces;

namespace ServerPlugins.SqlServer.CommandResponders
{
    class TableList: ICommandResponder
    {
        private string ConnectionString { get; set; }

        public TableList(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<Response> GetResponse()
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();

                string queryString = "select name from sys.objects where type = 'U' and name != 'sysdiagrams'";

                using (var command = new SqlCommand(queryString, cnn))
                {
                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        var tableNames = new List<string>();
                        while (dr.Read())
                        {
                            tableNames.Add(dr.GetString(0));
                        }
                        //JSON serializer-TableList;
                        var body = JsonConvert.SerializeObject(tableNames);

                        return new Response
                        {
                            ContentType = ContentTypes.JsonApplication,
                            ResponseCode = ResponseCode.Ok,
                            Type = ResponseType.Text,
                            Body = body
                        };
                    }
                }
            }
        }
    }
}
