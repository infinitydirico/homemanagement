using Autofac;
using HomeManagement.App.Services.Rest;
using Nightingale;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Statistics
{
    public class ExpensiveCategoriesViewModel : BaseViewModel
    {
        IAccountMetricsServiceClient accountMetricsServiceClient = App._container.Resolve<IAccountMetricsServiceClient>();

        public bool NoAvaibleStatistics => !DisplayExpensiveCategoriesChart;

        public List<SeriesValue> MostExpensiveCategories { get; private set; } = new List<SeriesValue>();

        public bool DisplayExpensiveCategoriesChart => MostExpensiveCategories.Count > 0;

        protected override async Task InitializeAsync()
        {
            await LoadMostExpensiveCategories();
        }

        private async Task LoadMostExpensiveCategories()
        {
            var expensiveCategories = await accountMetricsServiceClient.GetMostExpensiveCategories();

            MostExpensiveCategories.AddRange(from value in expensiveCategories.Categories
                                             select new SeriesValue
                                             {
                                                 Label = value.Category.Name,
                                                 Value = float.Parse(value.Price.ToString())
                                             });

            OnPropertyChanged(nameof(MostExpensiveCategories));
            OnPropertyChanged(nameof(DisplayExpensiveCategoriesChart));
            OnPropertyChanged(nameof(NoAvaibleStatistics));
        }
    }
}
