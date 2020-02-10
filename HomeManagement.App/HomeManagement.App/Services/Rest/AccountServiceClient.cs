using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class AccountServiceClient
    {
        BaseRestClient restClient;

        public AccountServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task Delete(int id)
        {
            var api = $"{Endpoints.Accounts.ACCOUNT}?id={id}";
            await restClient.Delete(api);
        }

        public async Task<IEnumerable<OverPricedCategory>> GetAccountTopTransactions(int accountId, int month)
        {
            var api = $"{accountId}/{Endpoints.Accounts.AccountTopTransactions}/{month}";
            var result = await restClient.Get<IEnumerable<OverPricedCategory>>(api);
            return result;
        }

        public async Task<AccountPageModel> Page(AccountPageModel dto)
        {
            var api = Endpoints.Accounts.PAGE;
            var result = await restClient.Post<AccountPageModel>(api, dto);
            return result;
        }

        public async Task Update(AccountModel account)
        {
            var api = Endpoints.Accounts.ACCOUNT;
            await restClient.Put<AccountPageModel>(api, account);
        }
    }
}
