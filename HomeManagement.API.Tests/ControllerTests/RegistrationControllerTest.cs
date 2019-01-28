using HomeManagement.API.Tests.Builders;
using HomeManagement.API.Tests.Builders.Data;
using HomeManagement.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class RegistrationControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;
        private readonly TestContext testContext;
        private readonly UserModelBuilder userModelBuilder = new UserModelBuilder();

        public RegistrationControllerTest(TestServerFixture fixture)
        {
            this.fixture = fixture;
            testContext = new TestContext(this.fixture);
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
        public void CanRegister()
        {
            testContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .EnsureSuccess();
        }

        [Fact]
        public void CanRegisterAndLogin()
        {
            testContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .EnsureSuccess()
                .SignIn()
                .EnsureSuccess();
        }
    }
}
