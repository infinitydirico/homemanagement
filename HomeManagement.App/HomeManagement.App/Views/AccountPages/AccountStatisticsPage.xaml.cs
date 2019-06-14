using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountStatisticsPage : ContentPage
    {
        public AccountStatisticsPage(Account account)
        {
            BindingContext = new AccountStatisticsViewModel(account);

            InitializeComponent();

            Title = account.Name;

            ((AccountStatisticsViewModel)BindingContext).OnInitializationFinished += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    barChart.InvalidateSurface();
                });
            };
        }
    }
}