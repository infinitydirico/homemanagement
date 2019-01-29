using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Net.Http;

namespace HomeManagement.API.Tests
{
    public class TestServerFixture : IDisposable
    {
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
            return @"C:\Users\Adriana\Documents\ramiro\projects\HomeManagementMultiPlatform\homemanagement\HomeManagement.API";
            var testProjectPath = PlatformServices.Default.Application.ApplicationBasePath;
            var relativePathToHostProject = @"..\..\..\..\..\..\HomeManagement.API";
            return Path.Combine(testProjectPath, relativePathToHostProject);
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
