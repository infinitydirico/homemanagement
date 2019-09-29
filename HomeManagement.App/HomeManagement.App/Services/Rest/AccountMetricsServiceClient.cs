using HomeManagement.App.Common;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AccountMetricsServiceClient : IAccountMetricsServiceClient
    {
        public async Task<AccountEvolutionModel> GetAccountEvolution(int accountId)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{Constants.Endpoints.Accounts.ACCOUNT}/{accountId}/{Constants.Endpoints.Accounts.AccountEvolution}")
                .ReadContent<AccountEvolutionModel>();
        }

        public async Task<AccountsEvolutionModel> GetAccountsBalances()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Accounts.AccountsEvolution)
                .ReadContent<AccountsEvolutionModel>();
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsByDate(int accountId, int year, int month)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(string.Format(Constants.Endpoints.Transaction.BY_ACCOUNT_AND_DATE, year, month, accountId))
                .ReadContent<IEnumerable<TransactionModel>>();
        }

        public async Task<OverPricedCategories> GetMostExpensiveCategories()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{Constants.Endpoints.Accounts.ACCOUNT}/{Constants.Endpoints.Accounts.AccountTopTransactions}/{DateTime.Now.Month}")
                .ReadContent<OverPricedCategories>();
        }

        public async Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId)
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync($"{Constants.Endpoints.Accounts.ACCOUNT}/{accountId}/{Constants.Endpoints.Accounts.AccountTopTransactions}/{DateTime.Now.Month}")
                .ReadContent<IEnumerable<OverPricedCategory2>>();
        }

        public async Task<MetricValueDto> GetTotalIncome()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Accounts.TotalIncome)
                .ReadContent<MetricValueDto>();
        }

        public async Task<MetricValueDto> GetTotalOutcome()
        {
            return await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Accounts.TotalOutcome)
                .ReadContent<MetricValueDto>();
        }
    }

    public interface IAccountMetricsServiceClient
    {
        Task<OverPricedCategories> GetMostExpensiveCategories();

        Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId);

        Task<AccountsEvolutionModel> GetAccountsBalances();

        Task<AccountEvolutionModel> GetAccountEvolution(int accountId);

        Task<MetricValueDto> GetTotalIncome();

        Task<MetricValueDto> GetTotalOutcome();

        Task<IEnumerable<TransactionModel>> GetTransactionsByDate(int accountId, int year, int month);
    }
}
