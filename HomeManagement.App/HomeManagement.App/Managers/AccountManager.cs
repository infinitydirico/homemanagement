using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Extensions;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface IAccountManager
    {
        Task<IEnumerable<Account>> LoadAsync(bool force = false);

        Task<IEnumerable<Account>> NextPageAsync();

        Task Delete(Account account);

        Task Update(Account account);
    }

    public class AccountManager : BaseManager<Account, AccountPageModel> , IAccountManager
    {        
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly AccountServiceClient accountServiceClient = new AccountServiceClient();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();

        public AccountManager()
        {
            page.UserId = authenticationManager.GetAuthenticatedUser().Id;

            page.PageCount = 10;
            page.CurrentPage = 1;
        }

        public async Task Delete(Account account)
        {
            await accountServiceClient.Delete(account.Id);
        }

        public async Task<IEnumerable<Account>> LoadAsync(bool force = false)
        {
            if (force)
            {
                cachingService.StoreOrUpdate("ForceApiCall", true);
            }
            return await Paginate();
        }

        public async Task Update(Account account)
        {
            var model = new AccountModel
            {
                Id = account.Id,
                Name = account.Name,
                UserId = account.UserId,
                AccountType = account.AccountType.To<Models.AccountType>(),
                Balance = account.Balance,
                Measurable = account.Measurable,
                CurrencyId = account.CurrencyId,
            };

            await accountServiceClient.Update(model);
        }

        protected override async Task<IEnumerable<Account>> Paginate()
        {
            if(coudSyncSetting != null && coudSyncSetting.Enabled)
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;

                if (repository.Count() > skip)
                {
                    var records = repository.Skip(skip).Take(page.PageCount).ToList();
                    return await Task.FromResult(records);
                }

                repository.RemoveAll();
                repository.Commit();
            }

            page = await accountServiceClient.Page(page);

            var accountsResult = from a in page.Accounts
                                 select new Account
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     UserId = a.UserId,
                                     AccountType = a.AccountType.To<Data.Entities.AccountType>(),
                                     Balance = a.Balance,
                                     Measurable = a.Measurable,
                                     LastApiCall = DateTime.Now,
                                     ChangeStamp = DateTime.Now,
                                     CurrencyId = a.CurrencyId,
                                     NeedsUpdate = false
                                 };

            Task.Run(() =>
            {
                foreach (var item in accountsResult)
                {
                    repository.Add(item);
                }

                repository.Commit();
            });

            return accountsResult;
        }
    }
}
