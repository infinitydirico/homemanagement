using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Statistics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        DashboardViewModel dashboardViewModel = new DashboardViewModel();
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public DashboardPage()
        {
            InitializeComponent();

            BindingContext = dashboardViewModel;

            dashboardViewModel.OnInitializationError += DashboardViewModel_OnError;

            dashboardViewModel.OnBalancesChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var index = 2;
                    foreach (var evolution in dashboardViewModel.AccountsEvolutions)
                    {
                        var accountEvolution = new AccountEvolutionFrame
                        {
                            AccountName = evolution.AccountName,
                            Series = evolution.Series
                        };

                        grid.Children.Add(accountEvolution, 0, 2, index, index + 1);
                        index++;
                    }
                });
            };
        }

        private void DashboardViewModel_OnError(object sender, System.EventArgs e)
        {
            DisplayAlert("Error", "Something went worng....", string.Empty);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AccountPages.AccountPage());
        }
    }
}