using HomeManagement.App.Common;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class AccountMetricsServiceClient
    {
        BaseRestClient restClient;

        public AccountMetricsServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<AccountEvolutionModel> GetAccountEvolution(int accountId)
        {
            var result = await restClient.Get<AccountEvolutionModel>($"{Endpoints.Accounts.ACCOUNT}{accountId}/{Endpoints.Accounts.AccountEvolution}");
            return result;
        }

        public async Task<AccountsEvolutionModel> GetAccountsBalances()
        {
            var result = await restClient.Get<AccountsEvolutionModel>($"{Constants.Endpoints.Accounts.AccountsEvolution}");
            return result;
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsByDate(int accountId, int year, int month)
        {
            var result = await restClient.Get<IEnumerable<TransactionModel>>(string.Format(Endpoints.Transaction.BY_ACCOUNT_AND_DATE, year, month, accountId));
            return result;
        }

        public async Task<OverPricedCategories> GetMostExpensiveCategories()
        {
            var apiUrl = $"{Endpoints.Accounts.ACCOUNT}{Endpoints.Accounts.AccountTopTransactions}/{DateTime.Now.Month}";

            var result = await restClient.Get<OverPricedCategories>(apiUrl);
            return result;
        }

        public async Task<IEnumerable<OverPricedCategory2>> GetMostExpensiveCategories(int accountId)
        {
            var result = await restClient.Get<IEnumerable<OverPricedCategory2>>($"{Endpoints.Accounts.ACCOUNT}{accountId}/{Endpoints.Accounts.AccountTopTransactions}/{DateTime.Now.Month}");
            return result;
        }

        public async Task<MetricValueDto> GetTotalIncome()
        {
            var result = await restClient.Get<MetricValueDto>($"/{Endpoints.Accounts.TotalIncome}");
            return result;
        }

        public async Task<MetricValueDto> GetTotalOutcome()
        {
            var result = await restClient.Get<MetricValueDto>($"/{Endpoints.Accounts.TotalOutcome}");
            return result;
        }
    }
}
