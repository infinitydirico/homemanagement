using HomeManagement.App.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        DashboardViewModel dashboardViewModel = new DashboardViewModel();

        public DashboardPage()
        {
            InitializeComponent();

            BindingContext = dashboardViewModel;

            dashboardViewModel.OnInitializationError += DashboardViewModel_OnError;
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