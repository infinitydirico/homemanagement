using HomeManagement.API.Tests.Builders.Data;
using HomeManagement.Core.Cryptography;
using HomeManagement.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HomeManagement.API.Tests.Builders
{
    public class TestContext
    {
        public readonly AesCryptographyService aesCryptographyService = new AesCryptographyService();
        public readonly TestServerFixture fixture;

        public TestContext(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        public string RequestBody { get; set; }

        public HttpResponseMessage Response { get; set; }

        public TestContext Serialize(object value)
        {
            RequestBody = JsonConvert.SerializeObject(value);
            return this;
        }

        public TestContext EnsureSuccess()
        {
            Response.EnsureSuccessStatusCode();

            return this;
        }

        public TestContext PostAsync(string uri)
        {
            Response = fixture
                .Client
                .PostAsync(uri, new StringContent(RequestBody, Encoding.UTF8, "application/json"))
                .Result;

            return this;
        }

        public TestContext Register()
        {
            PostAsync("/api/Register");

            return this;
        }

        public TestContext SignIn()
        {
            PostAsync("/api/Authentication/signin");

            return this;
        }


    }
}
