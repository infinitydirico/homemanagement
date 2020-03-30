using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using Nightingale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class DashboardViewModel : LocalizationBaseViewModel
    {
        private readonly AccountMetricsServiceClient accountMetricsServiceClient = new AccountMetricsServiceClient();
        INotificationManager notificationManager = App._container.Resolve<INotificationManager>();

        public event EventHandler OnBalancesChanged;

        public IEnumerable<NotificationModel> Notifications { get; private set; }

        public bool NoAvaibleStatistics => AccountsEvolutions.Count.Equals(0);

        public List<AccountEvolution> AccountsEvolutions { get; private set; } = new List<AccountEvolution>();

        protected override async Task InitializeAsync()
        {
            Notifications = await notificationManager.GetNotifications();

            await RetrieveAccountBalances();

            OnPropertyChanged(nameof(Notifications));
        }

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
