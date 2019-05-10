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
            var result = await Get<IEnumerable<OverPricedCategory>>($"{accountId}/{Constants.Endpoints.Accounts.AccountTopCharges}/{month}");

            return result;
        }

        public async Task<AccountPageModel> Page(AccountPageModel dto)
        {
            var result = await Post(dto, Constants.Endpoints.Accounts.PAGE, true);

            return result;
        }
    }

    public interface IAccountServiceClient
    {
        Task<IEnumerable<OverPricedCategory>> GetAccountTopCharges(int accountId, int month);

        Task<AccountPageModel> Page(AccountPageModel dto);
    }
}
