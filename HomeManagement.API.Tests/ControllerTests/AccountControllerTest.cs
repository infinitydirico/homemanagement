using HomeManagement.API.Tests.Builders.Controllers;
using HomeManagement.API.Tests.Builders.Data;
using Xunit;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class AccountControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;
        private readonly AccountControllerContext accountControllerContext;
        private readonly RegistrationTestContext registrationTestContext;
        private readonly UserModelBuilder userModelBuilder = new UserModelBuilder();

        public AccountControllerTest(TestServerFixture fixture)
        {
            this.fixture = fixture;
            registrationTestContext = new RegistrationTestContext(fixture);
            accountControllerContext = new AccountControllerContext(fixture);
        }

        [Fact]
        public void CanGetAccounts()
        {
            //Arrange
            //First register and login 

            registrationTestContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .EnsureSuccessResponse()
                .SignIn()
                .EnsureSuccessResponse()
                .AssertCondition(() => registrationTestContext.ContainsToken());

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .GetAccounts()
                .EnsureSuccessResponse();

        }
    }
}
