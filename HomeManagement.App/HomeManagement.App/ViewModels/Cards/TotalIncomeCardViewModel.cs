using HomeManagement.App.Services.Rest;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Cards
{
    public class TotalIncomeCardViewModel : BaseViewModel
    {
        private readonly AccountMetricsServiceClient accountMetricsServiceClient = new AccountMetricsServiceClient();

        public int IncomePercentage { get; private set; }
        public int TotalIncome { get; private set; }

        protected override async Task InitializeAsync()
        {
            var income = await accountMetricsServiceClient.GetTotalIncome();

            IncomePercentage = income.Percentage;
            TotalIncome = income.Total;

            OnPropertyChanged(nameof(IncomePercentage));
            OnPropertyChanged(nameof(TotalIncome));
        }
    }
}
