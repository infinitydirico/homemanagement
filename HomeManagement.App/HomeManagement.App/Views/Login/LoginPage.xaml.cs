using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Main;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();

            viewModel.OnLoginError += ViewModel_OnLoginError;
            viewModel.OnLoginSuccess += ViewModel_OnLoginSuccess;
            BindingContext = viewModel;
        }

        private void ViewModel_OnLoginSuccess(object sender, EventArgs e)
        {
            var page = new MainPage();

            NavigationPage.SetHasBackButton(page, false);

            NavigationPage.SetHasNavigationBar(page, true);

            Navigation.PushAsync(page);
            Navigation.RemovePage(this);
        }

        private void ViewModel_OnLoginError(object sender, EventArgs e)
        {
            DisplayAlert("Error", "An error occur during Login", "Ok");
        }
    }
}