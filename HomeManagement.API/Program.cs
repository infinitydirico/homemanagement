using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;

namespace HomeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options =>
                {
                    int grpcPort = 5001;
                    var apiPort = 60424;

                    options.ListenLocalhost(grpcPort, o => o.Protocols = HttpProtocols.Http2);
                    options.ListenLocalhost(apiPort, o => o.Protocols = HttpProtocols.Http1);
                })
                .UseStartup<Startup>()
                .ConfigureLogging((context, logging) => 
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
    }
}
