using Autofac;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface IMetricsManager
    {
        Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId);
    }

    public class MetricsManager : IMetricsManager
    {
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();
        private readonly IAccountMetricsServiceClient accountMetricsServiceClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public async Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId)
        {
            if (cachingService.Exists($"{nameof(GetMostExpensiveCategories)}{accountId}")) return cachingService.Get<IEnumerable<OverPricedCategory2>>($"{nameof(GetMostExpensiveCategories)}{accountId}");

            var result = await accountMetricsServiceClient.GetMostExpensiveCategories(accountId);

            cachingService.StoreOrUpdate($"{nameof(GetMostExpensiveCategories)}{accountId}", result);

            return result;
        }
    }
}
