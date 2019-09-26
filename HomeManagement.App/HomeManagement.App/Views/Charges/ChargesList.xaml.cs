using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.AccountPages;
using HomeManagement.App.Views.Controls;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Charges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChargesList : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        Account account;
        TransactionListViewModel viewModel;
        Modal modal;

        public ChargesList(Account account)
        {
            this.account = account;
            viewModel = new TransactionListViewModel(account);
            modal = new Modal(this);

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

        private void OnAddChargeCommand(object sender, EventArgs e)
        {
            var page = new AddCharge(account);

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

        private void OnViewChargesOnCalendar(object sender, EventArgs e)
        {
            var calendarPage = new CalendarPage(account);
            NavigationPage.SetHasBackButton(calendarPage, true);
            Navigation.PushAsync(calendarPage);
        }

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var charge = GetCurrentCharge(editButton);
            var editChargePage = new EditCharge(account, charge);
            NavigationPage.SetHasBackButton(editChargePage, true);

            Navigation.PushAsync(editChargePage);
        }

        private async void Delete(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var charge = GetCurrentCharge(editButton);

            var confirmed = await modal.ShowOkCancel(localizationManager.Translate("DeleteChargeModal"));

            if (confirmed)
            {
                viewModel.DeleteCommand.Execute(charge);
            }
        }

        private Charge GetCurrentCharge(View view)
        {
            var cell = GetViewCell(view);
            var charge = cell.BindingContext as Charge;
            return charge;
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
    }
}