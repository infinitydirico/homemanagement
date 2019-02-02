using HomeManagement.Models;

namespace HomeManagement.API.Tests.Builders.Controllers
{
    public class RegistrationTestContext : TestContext<RegistrationTestContext>
    {
        public RegistrationTestContext(TestServerFixture fixture) : base(fixture)
        {
        }

        public RegistrationTestContext Register()
        {
            PostAsync("/api/Register");
            return this;
        }

        public RegistrationTestContext SignIn()
        {
            PostAsync("/api/Authentication/signin");
            return this;
        }
    }
}
