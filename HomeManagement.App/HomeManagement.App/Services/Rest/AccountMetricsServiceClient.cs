using HomeManagement.App.Common;
using HomeManagement.Models;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountMetricsServiceClient : BaseService, IAccountMetricsServiceClient
    {
        public async Task<AccountEvolutionModel> GetAccountEvolution(int accountId)
        {
            return await Get<AccountEvolutionModel>($"{Constants.Endpoints.Accounts.ACCOUNT}/{accountId}/{Constants.Endpoints.Accounts.AccountEvolution}");
        }

        public async Task<AccountsEvolutionModel> GetAccountsBalances()
        {
            return await Get<AccountsEvolutionModel>(Constants.Endpoints.Accounts.AccountsEvolution);
        }

        public async Task<OverPricedCategories> GetMostExpensiveCategories()
        {
            return new OverPricedCategories { Categories = new System.Collections.Generic.List<OverPricedCategory>() };
            //return await Get<OverPricedCategories>($"{Constants.Endpoints.Accounts.ACCOUNT}//{Constants.Endpoints.Accounts.AccountTopCharges}");
        }

        public async Task<MetricValueDto> GetTotalIncome()
        {
            return await Get<MetricValueDto>(Constants.Endpoints.Accounts.TotalIncome, true);
        }

        public async Task<MetricValueDto> GetTotalOutcome()
        {
            return await Get<MetricValueDto>(Constants.Endpoints.Accounts.TotalOutcome, true);
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
