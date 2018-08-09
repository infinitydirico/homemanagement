using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountMetricsServiceClient : BaseService, IAccountMetricsServiceClient
    {
        public async Task<AccountEvolutionModel> GetAccountEvolution(int accountId)
        {
            return await Get<AccountEvolutionModel>($"{Constants.Endpoints.AccountMetric.AccountEvolution}/{accountId}");
        }

        public async Task<AccountsEvolutionModel> GetAccountsBalances()
        {
            return await Get<AccountsEvolutionModel>(Constants.Endpoints.AccountMetric.AccountsEvolution);
        }

        public async Task<OverPricedCategories> GetMostExpensiveCategories()
        {
            return await Get<OverPricedCategories>(Constants.Endpoints.AccountMetric.Overalloutgoing);
        }

        public async Task<MetricValueDto> GetTotalIncome()
        {
            return await Get<MetricValueDto>(Constants.Endpoints.AccountMetric.TotalIncome);
        }

        public async Task<MetricValueDto> GetTotalOutcome()
        {
            return await Get<MetricValueDto>(Constants.Endpoints.AccountMetric.TotalOutcome);
        }
    }

    public interface IAccountMetricsServiceClient
    {
        Task<OverPricedCategories> GetMostExpensiveCategories();

        Task<AccountsEvolutionModel> GetAccountsBalances();

        Task<AccountEvolutionModel> GetAccountEvolution(int accountId);

        Task<MetricValueDto> GetTotalIncome();

        Task<MetricValueDto> GetTotalOutcome();
    }
}
