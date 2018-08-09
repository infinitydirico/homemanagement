using HomeManagement.App.Services.Components;
using HomeManagement.App.Services.Rest;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        IAccountMetricsServiceClient accountMetricsServiceClient;
        private Chart accountBalances;
        private Chart expensivesCategories;

        public StatisticsViewModel()
        {
            accountMetricsServiceClient = DependencyService.Get<IAccountMetricsServiceClient>(DependencyFetchTarget.GlobalInstance);

            Task.Run(async () =>
            {
                await RetrieveAccountBalances();

                await RetrieveExpensiveCategories();
            });
        }

        private async Task RetrieveExpensiveCategories()
        {
            var categories = await accountMetricsServiceClient.GetMostExpensiveCategories();

            ExpensivesCategories = ChartsFactory.CreatePointChart(categories.ToDictionary());
        }

        private async Task RetrieveAccountBalances()
        {
            var balances = await accountMetricsServiceClient.GetAccountsBalances();
            var tempEntries = new List<Microcharts.Entry>();

            var values = balances.ToDictionary();

            AccountBalances = ChartsFactory.CreateLineChart(values.First());
        }

        public Chart AccountBalances
        {
            get => accountBalances;
            set
            {
                accountBalances = value;
                OnPropertyChanged();
            }
        }

        public Chart ExpensivesCategories
        {
            get => expensivesCategories;
            set
            {
                expensivesCategories = value;
                OnPropertyChanged();
            }
        }

        public string OverviewText => language.CurrentLanguage.OverviewText;

    }
}
