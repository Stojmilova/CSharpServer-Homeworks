using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServerInterfaces;

namespace ServerPlugins.SqlServer.CommandResponders
{
    public class ErrorConnectionString : ICommandResponder
    {
        public async Task<Response> GetResponse()
        {
            return new Response
            {
                ContentType = ContentTypes.PlainText,
                ResponseCode = ResponseCode.WebServerIsDown,
                Type = ResponseType.Text,
                Body = "INVALID connection string or server is OFFLINE !"
            };
        }
    }
}
