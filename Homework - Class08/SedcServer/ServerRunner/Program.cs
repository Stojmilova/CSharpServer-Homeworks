using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PngResponseGeneratorLib;
using ServerCore;
using ServerInterfaces;
using ServerPlugins;
using ServerPlugins.SqlServer;

namespace ServerRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new WebServer())
            {
                server
                    .UseResponseGenerator<PngResponseGenerator>()
                    .UseResponseGenerator<PostMethodResponseGenerator>()
                    .UseResponsePostProcessor<NotFoundPostProcessor>()
                    .UseResponseGenerator(new StaticResponseGenerator(@"C:\Users\Stojmilova\Desktop\images"))                   
                    .UseResponseGenerator(new SqlServerResponseGenerator("SEDCHome", "Server=.\\SQLExpress;Database=SEDCHome;Trusted_Connection=True;"))
                    .UseResponseGenerator(new SqlServerResponseGenerator("invalid", string.Empty));

                var result = server.Run();
                result.Wait();
            }
        }
    }
}
