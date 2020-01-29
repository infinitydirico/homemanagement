using Autofac;
using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Cards
{
    public class TotalOutcomeCardViewModel : BaseViewModel
    {
        IAccountMetricsServiceClient metricClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public int OutcomePercentage { get; private set; }
        public int TotalOutcome { get; private set; }

        protected override async Task InitializeAsync()
        {
            var outcome = await metricClient.GetTotalOutcome();

            OutcomePercentage = outcome.Percentage;
            TotalOutcome = outcome.Total;

            OnPropertyChanged(nameof(OutcomePercentage));
            OnPropertyChanged(nameof(TotalOutcome));
        }
    }
}
