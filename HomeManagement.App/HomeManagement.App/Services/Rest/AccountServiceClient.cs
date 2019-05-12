using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountServiceClient : IAccountServiceClient
    {
        public async Task<IEnumerable<OverPricedCategory>> GetAccountTopCharges(int accountId, int month)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{accountId}/{Constants.Endpoints.Accounts.AccountTopCharges}/{month}")
                .ReadContent<IEnumerable<OverPricedCategory>>();
        }

        public async Task<AccountPageModel> Page(AccountPageModel dto)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync(Constants.Endpoints.Accounts.PAGE, dto.SerializeToJson())
                .ReadContent<AccountPageModel>();
        }
    }

    public interface IAccountServiceClient
    {
        Task<IEnumerable<OverPricedCategory>> GetAccountTopCharges(int accountId, int month);

        Task<AccountPageModel> Page(AccountPageModel dto);
    }
}
