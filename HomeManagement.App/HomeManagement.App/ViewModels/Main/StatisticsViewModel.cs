using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using Nightingale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly AccountMetricsServiceClient accountMetricsServiceClient = new AccountMetricsServiceClient();

        public StatisticsViewModel()
        {
            Task.Run(async () =>
            {
                await RetrieveAccountBalances();
            });
        }

        public event EventHandler OnBalancesChanged;

        public OverPricedCategories TopCategories { get; private set; }

        private async Task RetrieveAccountBalances()
        {
            var balances = await accountMetricsServiceClient.GetAccountsBalances();

            AccountsEvolutions.AddRange(
                       from b in balances.Accounts
                       select new AccountEvolution
                       {
                           AccountName = b.AccountName,
                           Series = b.BalanceEvolution.Select(x => new SeriesValue
                           {
                               Value = x.Balance,
                               Label = new DateTime(DateTime.Now.Year, b.BalanceEvolution.IndexOf(x) + 1, 1).ToString("MMM")
                           }).ToList()
                       });

            OnPropertyChanged(nameof(AccountsEvolutions));
            OnPropertyChanged(nameof(NoAvaibleStatistics));
            OnBalancesChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool NoAvaibleStatistics => !DisplayExpensiveCategoriesChart && AccountsEvolutions.Count.Equals(0);

        public List<SeriesValue> MostExpensiveCategories { get; private set; } = new List<SeriesValue>();

        public bool DisplayExpensiveCategoriesChart => MostExpensiveCategories.Count > 0;

        public List<AccountEvolution> AccountsEvolutions { get; private set; } = new List<AccountEvolution>();
    }

    public class AccountEvolution
    {
        public string AccountName { get; set; }

        public List<SeriesValue> Series { get; set; }
    }
}
