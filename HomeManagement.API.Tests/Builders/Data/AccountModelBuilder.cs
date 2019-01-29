using HomeManagement.Models;
using System;

namespace HomeManagement.API.Tests.Builders.Data
{
    public class AccountModelBuilder
    {
        public AccountModel CreateDefaultAccount(int userId) => new AccountModel
        {
            Name = Guid.NewGuid().ToString("N"),
            AccountType = AccountType.Bank,
            UserId = userId,
            CurrencyId = 1
        };
    }
}
