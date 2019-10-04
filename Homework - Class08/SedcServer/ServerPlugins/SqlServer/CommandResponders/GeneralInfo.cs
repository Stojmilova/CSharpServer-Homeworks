using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerInterfaces;

namespace ServerPlugins.SqlServer.CommandResponders
{
    public class GeneralInfo : ICommandResponder
    {
        private string ConnectionString { get; set; }

        public GeneralInfo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<Response> GetResponse()
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();

                string queryString = "select @@version";

                using (var command = new SqlCommand(queryString, cnn))
                {
                    var result = (await command.ExecuteScalarAsync()).ToString();

                    //JSON serializer-GeneralInfo:
                    var body = JsonConvert.SerializeObject(result);

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
