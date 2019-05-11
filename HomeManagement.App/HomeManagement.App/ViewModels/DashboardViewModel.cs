using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.ViewModels
{
    public class DashboardViewModel : LocalizationBaseViewModel
    {
        IAccountMetricsServiceClient metricClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public int IncomePercentage { get; private set; }
        public int TotalIncome { get; private set; }

        public int OutcomePercentage { get; private set; }
        public int TotalOutcome { get; private set; }
        public string OverallIncomeText { get; set; } = "OverallIncome";
        public string OverallOutcomeText { get; set; } = "OverallOutcome";
        public string GoToAccountsText => "GoToAccounts";

        protected override async Task InitializeAsync()
        {
            var income = await metricClient.GetTotalIncome();

            var outcome = await metricClient.GetTotalOutcome();

            IncomePercentage = income.Percentage;
            TotalIncome = income.Total;

            OutcomePercentage = outcome.Percentage;
            TotalOutcome = outcome.Total;

            OnPropertyChanged(nameof(IncomePercentage));
            OnPropertyChanged(nameof(OutcomePercentage));
            OnPropertyChanged(nameof(TotalIncome));
            OnPropertyChanged(nameof(TotalOutcome));
        }       
    }
}
