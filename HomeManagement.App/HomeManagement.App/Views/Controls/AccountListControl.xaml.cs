using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.AccountPages;
using HomeManagement.App.Views.Transactions;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountListControl : StackLayout
	{
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        AccountsViewModel viewModel;
        Modal modal;

        public AccountListControl ()
		{
			InitializeComponent ();

            viewModel = new AccountsViewModel();
            BindingContext = viewModel;
        }

        private void EditTransaction(object sender, ItemTappedEventArgs e)
        {
            var account = e.Item as Account;

            Navigation.PushAsync(new TransactionListPage(account));

            ((ListView)sender).SelectedItem = null;
        }

        private async void StackLayout_LayoutChanged(object sender, EventArgs e)
        {
            var layout = sender as StackLayout;
            await layout.FadeTo(1);
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

        private void ViewTransactionsList(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent.Parent as StackLayout;
            GoToTransactionsList(stacklayout);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var result = await modal.ShowOkCancel(localizationManager.Translate("DeleteTransactionModal"));
            if (result)
            {
                var deleteButton = sender as Button;
                var stacklayout = deleteButton.Parent as StackLayout;
                var account = GetCurrentAccount(stacklayout);
                await viewModel.Delete(account);
                viewModel.Refresh();
            }
        }

        private void GoToTransactionsList(StackLayout stackLayout)
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