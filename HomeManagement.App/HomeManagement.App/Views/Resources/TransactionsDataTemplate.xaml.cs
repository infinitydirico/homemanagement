using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Extensions;
using HomeManagement.App.Managers;
using HomeManagement.App.Views.Transactions;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Resources
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsDataTemplate : ResourceDictionary
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        Transaction transaction;

        public TransactionsDataTemplate()
        {
            InitializeComponent();
        }

        private async void OnTransactionTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            transaction = stackLayout.BindingContext as Transaction;
            await stackLayout.RotateActions();
        }

        public void Edit(object sender, EventArgs e)
        {
            var viewModel = (sender as Button).Parent.GetViewModel<ViewModels.TransactionListViewModel>();
            var editTransactionPage = new EditTransactionPage(viewModel.Account, transaction);

            MessagingCenter.Subscribe<EditTransactionPage>(editTransactionPage, Constants.Messages.UpdateOnAppearing, p =>
            {
                viewModel.Refresh();
                MessagingCenter.Unsubscribe<EditTransactionPage>(p, Constants.Messages.UpdateOnAppearing);
            });

            Application.Current.MainPage.Navigation.PushAsync(editTransactionPage);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var viewModel = (sender as Button).Parent.GetViewModel<ViewModels.TransactionListViewModel>();

            var confirmed = await Application.Current.MainPage.DisplayAlert(localizationManager.Translate("Warning"),
                                                            $"{localizationManager.Translate("DeleteChargeModal")}",
                                                            "Ok",
                                                            localizationManager.Translate("Cancel"));
            if (confirmed)
            {
                viewModel.DeleteCommand.Execute(transaction);
            }
        }

        private void ViewFiles(object sender, EventArgs e) => Application.Current.MainPage.Navigation.PushAsync(new Files(transaction));
    }
}