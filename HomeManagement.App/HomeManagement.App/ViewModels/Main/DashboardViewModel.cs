using Autofac;
using HomeManagement.App.Services.Rest;
using Nightingale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class DashboardViewModel : LocalizationBaseViewModel
    {
        IAccountMetricsServiceClient metricClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public event EventHandler OnBalancesChanged;

        public int IncomePercentage { get; private set; }
        public int TotalIncome { get; private set; }

        public int OutcomePercentage { get; private set; }
        public int TotalOutcome { get; private set; }

        public bool NoAvaibleStatistics => AccountsEvolutions.Count.Equals(0);

        public List<AccountEvolution> AccountsEvolutions { get; private set; } = new List<AccountEvolution>();

        protected override async Task InitializeAsync()
        {
            var income = await metricClient.GetTotalIncome();

            var outcome = await metricClient.GetTotalOutcome();

            await RetrieveAccountBalances();

            IncomePercentage = income.Percentage;
            TotalIncome = income.Total;

            OutcomePercentage = outcome.Percentage;
            TotalOutcome = outcome.Total;

            OnPropertyChanged(nameof(IncomePercentage));
            OnPropertyChanged(nameof(OutcomePercentage));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(TotalOutcome));
        }

        private async Task RetrieveAccountBalances()
        {
            var balances = await metricClient.GetAccountsBalances();

            AccountsEvolutions.AddRange(
                       from b in balances.Balances
                       select new AccountEvolution
                       {
                           AccountName = b.AccountName,
                           Series = b.BalanceEvolution.Select(x => new SeriesValue
                           {
                               Value = x,
                               Label = new DateTime(DateTime.Now.Year, b.BalanceEvolution.IndexOf(x) + 1, 1).ToString("MMM")
                           }).ToList()
                       });

            OnPropertyChanged(nameof(AccountsEvolutions));
            OnPropertyChanged(nameof(NoAvaibleStatistics));
            OnBalancesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
