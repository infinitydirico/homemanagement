using HomeManagement.API.Tests.Builders.Controllers;
using HomeManagement.API.Tests.Builders.Data;
using Xunit;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class RegistrationControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;
        private readonly RegistrationTestContext testContext;
        private readonly UserModelBuilder userModelBuilder = new UserModelBuilder();

        public RegistrationControllerTest(TestServerFixture fixture)
        {
            this.fixture = fixture;
            testContext = new RegistrationTestContext(this.fixture);
        }

        [Fact]
        public void PingValues()
        {
            testContext.GetAsync("api/values").EnsureSuccessResponse();
        }

        [Fact]
        public void CanRegister()
        {
            testContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .EnsureSuccessResponse();
        }

        [Fact]
        public void CanRegisterAndLogin()
        {
            testContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .EnsureSuccessResponse()
                .SignIn()
                .EnsureSuccessResponse()
                .AssertCondition(() => testContext.ContainsToken());
        }
    }
}
