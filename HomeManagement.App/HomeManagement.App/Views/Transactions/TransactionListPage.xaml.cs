using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.AccountPages;
using HomeManagement.App.Views.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionListPage : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        Account account;
        TransactionListViewModel viewModel;
        Modal modal;

        public TransactionListPage(Account account)
        {
            this.account = account;
            viewModel = new TransactionListViewModel(account);
            modal = new Modal(this);

            viewModel.OnError += ViewModel_OnError;
            viewModel.OnInitializationError += ViewModel_OnInitializationError;

            BindingContext = viewModel;

            Title = account.Name;
            InitializeComponent();

            //AddFilterItem();
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

            NavigationPage.SetHasBackButton(page, true);

            NavigationPage.SetHasNavigationBar(page, true);

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

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var Transaction = GetCurrentTransaction(editButton);
            var editTransactionPage = new EditTransactionPage(account, Transaction);
            NavigationPage.SetHasBackButton(editTransactionPage, true);

            Navigation.PushAsync(editTransactionPage);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var Transaction = GetCurrentTransaction(editButton);

            var confirmed = await modal.ShowOkCancel(localizationManager.Translate("DeleteTransactionModal"));

            if (confirmed)
            {
                viewModel.DeleteCommand.Execute(Transaction);
            }
        }

        private Transaction GetCurrentTransaction(View view)
        {
            var cell = GetViewCell(view);
            var Transaction = cell.BindingContext as Transaction;
            return Transaction;
        }

        private ViewCell GetViewCell(Element view)
        {
            var parent = view.Parent;

            if (parent is ViewCell) return parent as ViewCell;

            return GetViewCell(parent);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var innerLayout = stackLayout.Children.First(x => x.GetType().Equals(typeof(StackLayout))) as StackLayout;

            Rotate(stackLayout);
        }

        private async void Rotate(StackLayout parent)
        {
            var layouts = parent.Children.ToList();

            var infoLayout = layouts.First();
            var actionsLayout = layouts.Last();

            var actionsVisible = actionsLayout.IsVisible;
            if (actionsVisible)
            {
                await ChangeVisibility(actionsLayout, infoLayout);
            }
            else
            {
                await ChangeVisibility(infoLayout, actionsLayout);
            }
        }

        private async void DisplayFilters(object sender, EventArgs e)
        {
            await ChangeVisibility(pageButtons, filters);
            RemoveToolbarItem("filter_list_24dp.png");
            AddClearFilterItem();
        }

        private async void ClearFilters(object sender, EventArgs e)
        {
            await ChangeVisibility(filters, pageButtons);
            RemoveToolbarItem("clear_all_24dp.png");
            AddFilterItem();
        }

        private async Task ChangeVisibility(View source, View target)
        {
            uint length = 250;
            await source.RotateXTo(-90, length, Easing.SpringIn);

            source.IsVisible = false;
            target.IsVisible = true;

            target.RotationX = -90;
            await target.RotateXTo(0, length, Easing.SpringOut);
        }

        private void AddClearFilterItem()
        {
            var clearFilterItem = new ToolbarItem
            {
                Icon = "clear_all_24dp.png"
            };

            clearFilterItem.Clicked += ClearFilters;
            ToolbarItems.Add(clearFilterItem);
        }

        private void AddFilterItem()
        {
            var filterItem = new ToolbarItem
            {
                Icon = "filter_list_24dp.png"
            };

            filterItem.Clicked += DisplayFilters;
            ToolbarItems.Add(filterItem);
        }

        private void RemoveToolbarItem(string icon)
        {
            var item = ToolbarItems.First(x => x.Icon.File.Equals(icon));
            ToolbarItems.Remove(item);
        }

        private void OnFilterFocusChanged(object sender, FocusEventArgs e)
        {
            transactionsFrame.IsVisible = !e.IsFocused;
        }
    }
}