using HomeManagement.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        StatisticsViewModel viewModel = new StatisticsViewModel();

        public StatisticsPage()
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}