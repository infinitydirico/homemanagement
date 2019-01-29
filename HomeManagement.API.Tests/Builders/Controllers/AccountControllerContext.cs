using HomeManagement.Models;
using System;
using System.Collections.Generic;

namespace HomeManagement.API.Tests.Builders.Controllers
{
    public class AccountControllerContext : TestContext<AccountControllerContext>
    {
        private string controllerApiUri => "api/account";

        public AccountControllerContext(TestServerFixture fixture) : base(fixture)
        {
        }

        public AccountControllerContext GetAccounts()
        {
            GetAsync(controllerApiUri);
            ReadResponse<List<AccountModel>>();
            return this;
        }

        public AccountControllerContext GetAccount(int id)
        {
            GetAsync(controllerApiUri + "/" + id);
            ReadResponse<AccountModel>();
            return this;
        }

        public AccountControllerContext CreateAccount()
        {
            PostAsync(controllerApiUri);
            ReadResponse<AccountModel>();
            return this;
        }

        public AccountControllerContext UpdateAccount()
        {
            PutAsync(controllerApiUri);
            return this;
        }

        public AccountControllerContext DeleteAccount(int id)
        {
            DeleteAsync(controllerApiUri + "/" + id);
            return this;
        }
    }
}
