using Autofac;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface IMetricsManager
    {
        Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId);

        Task<AccountEvolutionModel> GetAccountEvolution(int accountId);

        Task<AccountsEvolutionModel> GetAccountsEvolution();
    }

    public class MetricsManager : IMetricsManager
    {
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();
        private readonly IAccountMetricsServiceClient accountMetricsServiceClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public async Task<AccountEvolutionModel> GetAccountEvolution(int accountId)
        {
            if (cachingService.Exists($"{nameof(GetAccountEvolution)}{accountId}")) return cachingService.Get<AccountEvolutionModel>($"{nameof(GetAccountEvolution)}{accountId}");

            var result = await accountMetricsServiceClient.GetAccountEvolution(accountId);

            cachingService.StoreOrUpdate($"{nameof(GetAccountEvolution)}{accountId}", result);

            return result;
        }

        public async Task<AccountsEvolutionModel> GetAccountsEvolution()
        {
            if (cachingService.Exists($"{nameof(GetAccountsEvolution)}")) return cachingService.Get<AccountsEvolutionModel>($"{nameof(GetAccountsEvolution)}");

            var result = await accountMetricsServiceClient.GetAccountsBalances();

            cachingService.StoreOrUpdate($"{nameof(GetAccountsEvolution)}", result);

            return result;
        }

        public async Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId)
        {
            if (cachingService.Exists($"{nameof(GetMostExpensiveCategories)}{accountId}")) return cachingService.Get<IEnumerable<OverPricedCategory2>>($"{nameof(GetMostExpensiveCategories)}{accountId}");

            var result = await accountMetricsServiceClient.GetMostExpensiveCategories(accountId);

            cachingService.StoreOrUpdate($"{nameof(GetMostExpensiveCategories)}{accountId}", result);

            return result;
        }
    }
}
