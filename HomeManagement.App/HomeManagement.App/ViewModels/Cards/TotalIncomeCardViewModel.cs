using Autofac;
using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Cards
{
    public class TotalIncomeCardViewModel : BaseViewModel
    {
        IAccountMetricsServiceClient metricClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public int IncomePercentage { get; private set; }
        public int TotalIncome { get; private set; }

        protected override async Task InitializeAsync()
        {
            var income = await metricClient.GetTotalIncome();

            IncomePercentage = income.Percentage;
            TotalIncome = income.Total;

            OnPropertyChanged(nameof(IncomePercentage));
            OnPropertyChanged(nameof(TotalIncome));
        }
    }
}
