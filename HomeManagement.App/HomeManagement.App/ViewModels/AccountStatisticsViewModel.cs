using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using Nightingale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class AccountStatisticsViewModel : LocalizationBaseViewModel
    {
        private readonly IMetricsManager metricsManager = App._container.Resolve<IMetricsManager>();

        public AccountStatisticsViewModel(Account account)
        {
            Account = account;
        }

        public Account Account { get; }

        public List<ChartValue> ChartValues { get; private set; } = new List<ChartValue>();

        protected override async Task InitializeAsync()
        {
            var result = await metricsManager.GetMostExpensiveCategories(Account.Id);

            var values = (from r in result
                          where r.Value > (result.Max(x => x.Value) * 10 / 100)
                          select new ChartValue
                          {
                              Label = r.Category.Name,
                              Value = float.Parse(r.Value.ToString())
                          });

            ChartValues.AddRange(values);

            OnPropertyChanged(nameof(ChartValues));
        }

    }
}
