using HomeManagement.App.Common;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        AccountsViewModel viewModel = new AccountsViewModel();
        Modal modal;

        public AccountPage()
        {
            InitializeComponent();

            modal = new Modal(this);
            BindingContext = viewModel;
        }

        private void NavigateToAddAccount(object sender, EventArgs e)
        {
            var page = new AddAccountPage();

            MessagingCenter.Subscribe<AddAccountPage>(page, Constants.Messages.UpdateOnAppearing, p =>
            {
                viewModel.Refresh();
                MessagingCenter.Unsubscribe<AddAccountPage>(p, Constants.Messages.UpdateOnAppearing);
            });

            Navigation.PushAsync(page);
        }

        private async void PopupHelpModal(object sender, EventArgs e)
        {
            string message = $"Tap an account once to see the options. {Environment.NewLine}Tap twice to enter.";
            await modal.Show(message);
        }
    }
}