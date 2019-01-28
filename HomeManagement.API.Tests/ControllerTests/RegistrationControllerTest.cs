using HomeManagement.API.Tests.Builders;
using HomeManagement.API.Tests.Builders.Data;
using HomeManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class RegistrationControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public RegistrationControllerTest(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task PingValues()
        {
            var response = await fixture.Client.GetAsync("api/values");

            response.EnsureSuccessStatusCode();

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
        }

        [Fact]
        public async Task CanRegister()
        {
            var json = JsonConvert.SerializeObject(new UserModel
            {
                Email = "brucewyane@gmail.com",
                Password = "4h5UHGckxny7Lux9A1g0mA==",
                Language = "en"
            });

            // The endpoint or route of the controller action.
            var httpResponse = await fixture.Client.PostAsync("/api/Register", new StringContent(json, Encoding.UTF8, "application/json"));

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {

            }
            // Deserialize and examine results.
            //var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            //var players = JsonConvert.DeserializeObject<IEnumerable<Player>>(stringResponse);
            //Assert.Contains(players, p => p.FirstName == "Wayne");
            //Assert.Contains(players, p => p.FirstName == "Mario");
        }
    }
}
