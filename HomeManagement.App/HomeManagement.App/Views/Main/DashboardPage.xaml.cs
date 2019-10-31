using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Statistics;
using System.Linq;
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

            dashboardViewModel.OnInitializationFinished += OnInitialized;

            dashboardViewModel.OnBalancesChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var index = 2;
                    var categoriesChart = grid.Children.Last();
                    if (!categoriesChart.IsVisible)
                    {
                        grid.Children.Remove(categoriesChart);
                        index--;
                    }
                    foreach (var evolution in dashboardViewModel.AccountsEvolutions)
                    {
                        var accountEvolution = new AccountEvolutionFrame
                        {
                            AccountName = evolution.AccountName,
                            Series = evolution.Series,
                            Opacity = 0
                        };

                        grid.Children.Add(accountEvolution, 0, 2, index, index + 1);

                        accountEvolution.FadeTo(0.5, 500, Easing.SpringIn);
                        accountEvolution.FadeTo(1, 500, easing: Easing.SpringOut);
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

        private void OnInitialized(object sender, System.EventArgs e)
        {
            if (dashboardViewModel.Notifications.Any())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ToolbarItems.Clear();
                    var item = new ToolbarItem
                    {
                        Icon = "notifications_24dp.png"
                    };

                    item.Clicked += (s, ev) =>
                    {
                        Navigation.PushAsync(new NotificationsPage());
                    };

                    ToolbarItems.Add(item);
                });                
            }
        }
    }
}