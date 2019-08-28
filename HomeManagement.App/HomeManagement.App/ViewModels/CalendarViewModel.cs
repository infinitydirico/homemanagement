using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        protected readonly IMetricsManager metricsManager = App._container.Resolve<IMetricsManager>();

        public CalendarViewModel(Account account)
        {
            Account = account;
        }

        public Account Account { get; }

        public DateTime CurrentDate { get; private set; } = DateTime.Now;

        public IEnumerable<EventDate> Events { get; set; }

        public async Task ChangeDate(DateTime date)
        {
            CurrentDate = date;

            var results = await metricsManager.GetChargesByDate(Account.Id, CurrentDate.Year, CurrentDate.Month);

            Events = results.Select(x => new EventDate
            {
                Title = x.Name,
                Date = x.Date
            }).ToList();

            OnPropertyChanged(nameof(Events));
        }

        protected override async Task InitializeAsync()
        {
            await ChangeDate(DateTime.Now);
        }
    }
}
