using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.AccountPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionListPage : ContentPage
    {
        Account account;
        TransactionListViewModel viewModel;

        public TransactionListPage(Account account)
        {
            this.account = account;
            viewModel = new TransactionListViewModel(account);

            viewModel.OnError += ViewModel_OnError;
            viewModel.OnInitializationError += ViewModel_OnInitializationError;

            BindingContext = viewModel;

            Title = account.Name;
            InitializeComponent();
        }

        private void ViewModel_OnInitializationError(object sender, EventArgs e)
        {
            DisplayAlert("Info", "An error ocurred.", "Ok");
        }

        private void ViewModel_OnError(object sender, Common.ErrorEventArgs e)
        {
            DisplayAlert("Info", e.ErrorMessage, "Ok");
        }

        private void OnAddTransactionCommand(object sender, EventArgs e)
        {
            var page = new AddTransactionPage(account);

            MessagingCenter.Subscribe<AddTransactionPage>(page, Constants.Messages.UpdateOnAppearing, p =>
            {
                viewModel.Refresh();
                MessagingCenter.Unsubscribe<AddAccountPage>(p, Constants.Messages.UpdateOnAppearing);
            });

            Navigation.PushAsync(page);
        }

        private void OnViewAccountStatistics(object sender, EventArgs e)
        {
            var accountStatisticsPage = new AccountStatisticsPage(account);
            NavigationPage.SetHasBackButton(accountStatisticsPage, true);
            Navigation.PushAsync(accountStatisticsPage);
        }

        private void OnViewTransactionsOnCalendar(object sender, EventArgs e)
        {
            var calendarPage = new CalendarPage(account);
            NavigationPage.SetHasBackButton(calendarPage, true);
            Navigation.PushAsync(calendarPage);
        }
    }
}