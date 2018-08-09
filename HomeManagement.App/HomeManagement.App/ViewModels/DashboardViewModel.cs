using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        IAccountMetricsServiceClient metricClient = DependencyService.Get<IAccountMetricsServiceClient>(DependencyFetchTarget.GlobalInstance);

        public DashboardViewModel()
        {
            Initialize();
        }

        public int IncomePercentage { get; private set; }
        public int TotalIncome { get; private set; }

        public int OutcomePercentage { get; private set; }
        public int TotalOutcome { get; private set; }

        private void Initialize()
        {
            Task.Run(async () =>
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
            });
        }

        public string OverviewText => language.CurrentLanguage.OverviewText;

        public string OverallIncomeText => language.CurrentLanguage.OverallIncomeText;
    }
}
