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
                await LoadMostExpensiveCategories();

                await RetrieveAccountBalances();
            });
        }

        public event EventHandler OnBalancesChanged;

        public OverPricedCategories TopCategories { get; private set; }

        private async Task LoadMostExpensiveCategories()
        {
            var categories = await accountMetricsServiceClient.GetMostExpensiveCategories();

            TopCategories = categories;


            ChartValues.AddRange(from value in TopCategories.Categories
                                  select new SeriesValue
                                  {
                                      Label = value.Category.Name,
                                      Value = float.Parse(value.Price.ToString())
                                  });

            OnPropertyChanged(nameof(ChartValues));
        }

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
            OnBalancesChanged?.Invoke(this, EventArgs.Empty);
        }

        public List<SeriesValue> ChartValues { get; private set; } = new List<SeriesValue>();

        public List<AccountEvolution> AccountsEvolutions { get; private set; } = new List<AccountEvolution>();
    }

    public class AccountEvolution
    {
        public string AccountName { get; set; }

        public List<SeriesValue> Series { get; set; }
    }
}
