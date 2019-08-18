using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.Core.Extensions;
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

        public List<SeriesValue> ChartValues { get; private set; } = new List<SeriesValue>();

        public List<SeriesValue> IncomeSeries { get; set; } = new List<SeriesValue>();

        public List<SeriesValue> OutcomeSeries { get; set; } = new List<SeriesValue>();

        protected override async Task InitializeAsync()
        {
            await GetCategoriesMetrics();

            await GetAccountMetrics();
        }

        private async Task GetAccountMetrics()
        {
            var result = await metricsManager.GetAccountEvolution(Account.Id);

            IncomeSeries.AddRange(result.IncomingSeries.Select(x => new SeriesValue
            {
                Value = x,
                Label = DateTime.Now.GetCurrentMonth(result.IncomingSeries.IndexOf(x) + 1)
            }));

            OutcomeSeries.AddRange(result.OutgoingSeries.Select(x => new SeriesValue
            {
                Value = x,
                Label = DateTime.Now.GetCurrentMonth(result.OutgoingSeries.IndexOf(x) + 1)
            }));

            OnPropertyChanged(nameof(IncomeSeries));
            OnPropertyChanged(nameof(OutcomeSeries));
        }

        private async Task GetCategoriesMetrics()
        {
            var result = await metricsManager.GetMostExpensiveCategories(Account.Id);

            var values = (from r in result
                          where r.Value > (result.Max(x => x.Value) * 10 / 100)
                          select new SeriesValue
                          {
                              Label = r.Category.Name.Substring(0, r.Category.Name.Length > 10 ? 10 : r.Category.Name.Length),
                              Value = float.Parse(r.Value.ToString())
                          });

            ChartValues.AddRange(values);

            OnPropertyChanged(nameof(ChartValues));
        }

    }
}
