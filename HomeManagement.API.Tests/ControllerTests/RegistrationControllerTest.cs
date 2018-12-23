using HomeManagement.API.Tests.Builders;
using HomeManagement.API.Tests.Builders.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class RegistrationControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly TestContext context = new TestContext();

        public RegistrationControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanRegister()
        {
            context.SetAuthenticationUser("ramiro.di.rico@gmail.com","4430598Q#$q");

            var result = await _client.GetAsync("/api/values");

            // The endpoint or route of the controller action.
            var httpResponse = await _client.PostAsJsonAsync("/api/register", context.User);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            //var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(stringResponse);
            //Assert.Contains(players, p => p.FirstName == "Wayne");
            //Assert.Contains(players, p => p.FirstName == "Mario");
        }
    }
}
