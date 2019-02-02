using HomeManagement.Core.Cryptography;
using System.Net.Http;

namespace HomeManagement.API.Tests.Builders
{
    public class TestContext
    {
        protected readonly AesCryptographyService aesCryptographyService = new AesCryptographyService();
        protected readonly TestServerFixture fixture;

        public TestContext(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public string Token { get; set; }

        public string RequestBody { get; set; }

        public HttpResponseMessage Response { get; set; }
    } 
}
