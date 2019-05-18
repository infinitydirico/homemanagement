using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public interface ICurrencyServiceClient
    {
        Task<IEnumerable<CurrencyModel>> GetCurrencies();
    }

    public class CurrencyServiceClient : ICurrencyServiceClient
    {
        public async Task<IEnumerable<CurrencyModel>> GetCurrencies()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Currency.CURRENCY)
                .ReadContent<IEnumerable<CurrencyModel>>();
        }
    }
}
