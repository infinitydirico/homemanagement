using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsPage : ContentPage
    {
        StatisticsViewModel viewModel = new StatisticsViewModel();
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public StatisticsPage()
        {
            InitializeComponent();

            BindingContext = viewModel;

            viewModel.OnInitializationFinished += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    pieChart.InvalidateSurface();
                    barChart.InvalidateSurface();
                });
            };

            Title = localizationManager.Translate("Statistics");

            if (Device.Idiom.Equals(TargetIdiom.Desktop))
            {
                pieChart.TextSize = barChart.TextSize = 12;
            }
        }
    }
}