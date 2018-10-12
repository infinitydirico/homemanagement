using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public class AccountMapper : BaseMapper<Account, AccountModel>, IAccountMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Account).GetProperty(nameof(Account.Id));
            yield return typeof(Account).GetProperty(nameof(Account.Name));
            yield return typeof(Account).GetProperty(nameof(Account.UserId));
            yield return typeof(Account).GetProperty(nameof(Account.ExcludeFromStatistics));
            yield return typeof(Account).GetProperty(nameof(Account.IsCash));
            yield return typeof(Account).GetProperty(nameof(Account.Balance));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Id));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Name));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.ExcludeFromStatistics));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.IsCash));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.UserId));
            yield return typeof(AccountModel).GetProperty(nameof(AccountModel.Balance));
        }
    }
}
