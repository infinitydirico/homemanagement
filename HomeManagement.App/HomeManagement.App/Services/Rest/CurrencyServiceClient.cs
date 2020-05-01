using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class CurrencyServiceClient
    {
        BaseRestClient restClient;

        public CurrencyServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<IEnumerable<CurrencyModel>> GetCurrencies()
        {
            var api = Endpoints.Currency.CURRENCY;
            var result = await restClient.Get<IEnumerable<CurrencyModel>>(api);
            return result;
        }
    }
}
