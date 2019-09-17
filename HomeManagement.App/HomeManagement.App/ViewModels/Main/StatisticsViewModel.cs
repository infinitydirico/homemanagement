using Autofac;
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
        IAccountMetricsServiceClient accountMetricsServiceClient;

        public StatisticsViewModel()
        {
            accountMetricsServiceClient = App._container.Resolve<IAccountMetricsServiceClient>();

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
                       from b in balances.Balances
                       select new AccountEvolution
                       {
                           AccountName = b.AccountName,
                           Series = b.BalanceEvolution.Select(x => new SeriesValue
                           {
                               Value = x,
                               Label = new DateTime(DateTime.Now.Year, b.BalanceEvolution.IndexOf(x) + 1, 1).ToString("MMMM")
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
