namespace HomeManagement.API.Tests.Builders.Controllers
{
    public class AccountControllerContext : TestContext<AccountControllerContext>
    {
        public AccountControllerContext(TestServerFixture fixture) : base(fixture)
        {
        }

        public AccountControllerContext GetAccounts()
        {
            GetAsync("api/account");
            return this;
        }
    }
}
