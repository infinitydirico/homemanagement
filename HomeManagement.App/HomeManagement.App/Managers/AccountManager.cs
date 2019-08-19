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
    }

    public class AccountManager : BaseManager<Account, AccountPageModel> , IAccountManager
    {        
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly IAccountServiceClient accountServiceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();

        public AccountManager()
        {
            page.UserId = authenticationManager.GetAuthenticatedUser().Id;

            page.PageCount = 10;
            page.CurrentPage = 1;
        }

        public async Task<IEnumerable<Account>> LoadAsync(bool force = false)
        {
            if (force)
            {
                cachingService.StoreOrUpdate("ForceApiCall", true);
            }
            return await Paginate();
        }

        protected override async Task<IEnumerable<Account>> Paginate()
        {
            if(cachingService.Get<bool>("ForceApiCall"))
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;

                if (accountRepository.Count() > skip)
                {
                    var records = accountRepository.Skip(skip).Take(page.PageCount).ToList();
                    return await Task.FromResult(records);
                }
            }

            accountRepository.RemoveAll();
            accountRepository.Commit();

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
                    accountRepository.Add(item);
                }

                accountRepository.Commit();
            });

            return accountsResult;
        }
    }
}
