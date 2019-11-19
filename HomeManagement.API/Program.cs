using HomeManagement.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
                .UseStartup<Startup>()
                .ConfigureLogging((context, logging) => 
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();
    }
}
