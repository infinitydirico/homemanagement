using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountServiceClient : BaseService, IAccountServiceClient
    {
        public async Task<IEnumerable<OverPricedCategory>> GetAccountTopCharges(int accountId, int month)
        {
            var result = await this.Get<IEnumerable<OverPricedCategory>>($"{Constants.Endpoints.Accounts.AccountTopCharges}/{accountId}/{month}");

            return result;
        }

        public async Task<AccountPageModel> Page(AccountPageModel dto)
        {
            var result = await this.Post<AccountPageModel>(dto, Constants.Endpoints.Accounts.PAGE);

            return result;
        }
    }

    public interface IAccountServiceClient
    {
        Task<IEnumerable<OverPricedCategory>> GetAccountTopCharges(int accountId, int month);

        Task<AccountPageModel> Page(AccountPageModel dto);

    }
}
