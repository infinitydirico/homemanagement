using HomeManagement.API.Tests.Builders.Controllers;
using HomeManagement.API.Tests.Builders.Data;
using HomeManagement.Models;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System;

namespace HomeManagement.API.Tests.ControllerTests
{
    public class AccountControllerTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;
        private readonly AccountControllerContext accountControllerContext;
        private readonly RegistrationTestContext registrationTestContext;
        private readonly UserModelBuilder userModelBuilder = new UserModelBuilder();
        private readonly AccountModelBuilder accountModelBuilder = new AccountModelBuilder();

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
                .SignIn();

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .GetAccounts()
                .EnsureSuccessResponse();

        }

        [Fact]
        public void CanGetAccountById()
        {
            //Arrange
            //First register and login
            registrationTestContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .SignIn();

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .GetAccounts()
                .EnsureSuccessResponse()
                .GetAccount(accountControllerContext.GetResponseValues<List<AccountModel>>().First().Id)
                .EnsureSuccessResponse()
                .AssertCondition(() => Assert.NotNull(accountControllerContext.GetResponseValues<AccountModel>()));
        }

        [Fact]
        public void CanCreateNewAccount()
        {
            //Arrange
            //First register and login
            registrationTestContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .SignIn();

            var account = accountModelBuilder.CreateDefaultAccount(registrationTestContext.GetResponseValues<UserModel>().Id);

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .Serialize(account)
                .CreateAccount()
                .EnsureSuccessResponse()
                .GetAccounts()
                .EnsureSuccessResponse()
                .AssertCondition(() => 
                    Assert.Contains(
                        accountControllerContext.GetResponseValues<List<AccountModel>>(),
                        x => x.Name.Equals(account.Name)));
        }

        [Fact]
        public void CanUpdateAccount()
        {
            //Arrange
            //First register and login
            registrationTestContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .SignIn();

            var account = accountModelBuilder.CreateDefaultAccount(registrationTestContext.GetResponseValues<UserModel>().Id);

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .Serialize(account)
                .CreateAccount()
                .GetAccounts();

            var createdAccount = accountControllerContext.GetResponseValues<List<AccountModel>>().Last();

            account.Id = createdAccount.Id;
            account.Name = string.Concat(Guid.NewGuid().ToString("N").Take(6));

            accountControllerContext
                .Serialize(account)
                .UpdateAccount()
                .EnsureSuccessResponse();
        }

        [Fact]
        public void CanDeleteAccount()
        {
            //Arrange
            //First register and login
            registrationTestContext
                .Serialize(userModelBuilder.CreateRandomUserModel())
                .Register()
                .SignIn();

            var account = accountModelBuilder.CreateDefaultAccount(registrationTestContext.GetResponseValues<UserModel>().Id);

            //Act % Assert
            //Then test
            accountControllerContext
                .ProvideAuthorizationToken(registrationTestContext.GetAuthorizationToken())
                .Serialize(account)
                .CreateAccount()
                .GetAccounts();

            account = accountControllerContext.GetResponseValues<List<AccountModel>>().Last();

            accountControllerContext
                .DeleteAccount(account.Id)
                .EnsureSuccessResponse();
        }
    }
}
