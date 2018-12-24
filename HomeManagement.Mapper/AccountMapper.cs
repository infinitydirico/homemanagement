using HomeManagement.Core.Extensions;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public class AccountMapper : BaseMapper<Account, AccountModel>, IAccountMapper
    {
        public override Account ToEntity(AccountModel model)
        {
            return new Account
            {
                Id = model.Id,
                Name = model.Name,
                AccountType = model.AccountType.ToString().ToEnum<HomeManagement.Domain.AccountType>(),
                Balance = model.Balance,
                Money = model.Money.ToString().ToEnum<HomeManagement.Domain.Money>(),
                ExcludeFromStatistics = model.ExcludeFromStatistics,
                UserId = model.UserId
            };
        }

        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Account).GetProperty(nameof(Account.Id));
            yield return typeof(Account).GetProperty(nameof(Account.Name));
            yield return typeof(Account).GetProperty(nameof(Account.UserId));
            yield return typeof(Account).GetProperty(nameof(Account.ExcludeFromStatistics));
            yield return typeof(Account).GetProperty(nameof(Account.AccountType));
            yield return typeof(Account).GetProperty(nameof(Account.Money));
            yield return typeof(Account).GetProperty(nameof(Account.Balance));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Id));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Name));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.ExcludeFromStatistics));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.AccountType));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Money));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.UserId));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Balance));
        }
    }
}
