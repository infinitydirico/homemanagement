using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Transactions;
using HomeManagement.App.Views.Controls;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        AccountsViewModel viewModel = new AccountsViewModel();
        Modal modal;

        public AccountPage()
        {
            InitializeComponent();

            modal = new Modal(this);
            BindingContext = viewModel;            
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private async void PopupHelpModal(object sender, EventArgs e)
        {
            string message = $"Tap an account once to see the options. {Environment.NewLine}Tap twice to enter.";
            await modal.Show(message);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            Rotate(stackLayout);
        }

        private async void Rotate(StackLayout parent)
        {
            uint length = 250;
            var layouts = parent.Children.ToList();

            var infoLayout = layouts.First();
            var actionsLayout = layouts.Last();

            var actionsVisible = actionsLayout.IsVisible;
            if (actionsVisible)
            {
                await actionsLayout.RotateXTo(-90, length, Easing.SpringIn);

                actionsLayout.IsVisible = false;
                infoLayout.IsVisible = true;

                infoLayout.RotationX = -90;
                await infoLayout.RotateXTo(0, length, Easing.SpringOut);
            }
            else
            {
                await infoLayout.RotateXTo(-90, length, Easing.SpringIn);

                infoLayout.IsVisible = false;
                actionsLayout.IsVisible = true;

                actionsLayout.RotationX = -90;
                await actionsLayout.RotateXTo(0, length, Easing.SpringOut);
            }
        }

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent.Parent as StackLayout;
            var account = GetCurrentAccount(stacklayout);

            var editPage = new EditAccountPage(account);

            MessagingCenter.Subscribe<EditAccountPage>(editPage, Constants.Messages.UpdateOnAppearing, page =>
           {
               viewModel.Refresh();
               MessagingCenter.Unsubscribe<EditAccountPage>(editPage, Constants.Messages.UpdateOnAppearing);
           });

            Navigation.PushAsync(editPage);
        }

        private void ViewChargesList(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent.Parent as StackLayout;
            GoToChargesList(stacklayout);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var result = await modal.ShowOkCancel(localizationManager.Translate("DeleteChargeModal"));
            if (result)
            {
                var deleteButton = sender as Button;
                var stacklayout = deleteButton.Parent as StackLayout;
                var account = GetCurrentAccount(stacklayout);
                await viewModel.Delete(account);
                viewModel.Refresh();
            }
        }

        private void GoToChargesList(StackLayout stackLayout)
        {
            var account = GetCurrentAccount(stackLayout);
            Navigation.PushAsync(new TransactionListPage(account));
        }

        private Account GetCurrentAccount(StackLayout stackLayout)
        {
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var accounts = ((AccountsViewModel)accountsList.BindingContext).Accounts;
            return accounts.First(x => x.Name.Equals(label.Text));
        }
    }
}