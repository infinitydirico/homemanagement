using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountServiceClient : IAccountServiceClient
    {
        public async Task Delete(int id)
        {
            await RestClientFactory
                .CreateAuthenticatedClient()
                .DeleteAsync($"{Constants.Endpoints.Accounts.ACCOUNT}?id={id}");
        }

        public async Task<IEnumerable<OverPricedCategory>> GetAccountTopTransactions(int accountId, int month)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{accountId}/{Constants.Endpoints.Accounts.AccountTopTransactions}/{month}")
                .ReadContent<IEnumerable<OverPricedCategory>>();
        }

        public async Task<AccountPageModel> Page(AccountPageModel dto)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Accounts.PAGE, dto.SerializeToJson())
                .ReadContent<AccountPageModel>();
        }

        public async Task Update(AccountModel account)
        {
            var response = 
                await RestClientFactory
                .CreateAuthenticatedClient()
                .PutAsync(Constants.Endpoints.Accounts.ACCOUNT, account.SerializeToJson());

            response.EnsureSuccessStatusCode();
        }
    }

    public interface IAccountServiceClient
    {
        Task<IEnumerable<OverPricedCategory>> GetAccountTopTransactions(int accountId, int month);

        Task<AccountPageModel> Page(AccountPageModel dto);

        Task Update(AccountModel account);

        Task Delete(int id);
    }
}
