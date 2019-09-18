using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Statistics;
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

            viewModel.OnBalancesChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var evolution in viewModel.AccountsEvolutions)
                    {
                        var accountEvolution = new AccountEvolutionFrame
                        {
                            AccountName = evolution.AccountName,
                            Series = evolution.Series
                        };

                        mainContainer.Children.Add(accountEvolution);
                    }
                });
            };

            Title = localizationManager.Translate("Statistics");
        }
    }
}