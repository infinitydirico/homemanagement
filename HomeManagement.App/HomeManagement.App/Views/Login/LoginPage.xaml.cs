using HomeManagement.App.ViewModels;
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

            Appearing += OnAppearing;
        }

        private async void ViewModel_OnLoginSuccess(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//main");
        }

        private void ViewModel_OnLoginError(object sender, EventArgs e)
        {
            DisplayAlert("Error", "An error occur during Login", "Ok");
        }

        private void SignUpClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            usernameEntry.Focus();
        }
    }
}