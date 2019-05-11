using Autofac;
using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using Nightingale;
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
            });
        }

        public OverPricedCategories TopCategories { get; private set; }

        private async Task LoadMostExpensiveCategories()
        {
            var categories = await accountMetricsServiceClient.GetMostExpensiveCategories();

            TopCategories = categories;


            ChartValues.AddRange(from value in TopCategories.Categories
                                  select new ChartValue
                                  {
                                      Label = value.Category.Name,
                                      Value = float.Parse(value.Price.ToString())
                                  });

            OnPropertyChanged(nameof(ChartValues));
        }

        private async Task RetrieveAccountBalances()
        {
            var balances = await accountMetricsServiceClient.GetAccountsBalances();

            var values = balances.ToDictionary();
        }

        public List<ChartValue> ChartValues { get; private set; } = new List<ChartValue>();

        public string OverviewText => "";//language.CurrentLanguage.OverviewText;
    }
}
