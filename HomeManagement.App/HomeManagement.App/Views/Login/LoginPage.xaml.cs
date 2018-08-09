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

            BindingContext = viewModel;
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await viewModel.LoginAsync();

            var page = new MainPage();

            NavigationPage.SetHasBackButton(page, false);

            NavigationPage.SetHasNavigationBar(page, true);

            await Navigation.PushAsync(page);
            Navigation.RemovePage(this);
        }
    }
}