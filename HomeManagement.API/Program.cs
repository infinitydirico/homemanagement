using HomeManagement.API.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HomeManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .SeedData()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
