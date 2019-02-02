using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Reflection;

namespace HomeManagement.API.Tests
{
    public class TestServerFixture : IDisposable
    {
        private const string HomeManagamentProjPath = "HomeManagement.API";
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = WebHost.CreateDefaultBuilder()
                   .UseContentRoot(GetContentRootPath())
                   .UseEnvironment("Development")
                   .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            _testServer.BaseAddress = new Uri("http://localhost:60424/");
            Client = _testServer.CreateClient();

        }

        private string GetContentRootPath()
        {
            var value = Assembly.GetExecutingAssembly().Location.Replace(".Tests", string.Empty);

            return value.Substring(0, value.IndexOf(HomeManagamentProjPath) + HomeManagamentProjPath.Length);
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
