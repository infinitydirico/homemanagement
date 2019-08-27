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

namespace HomeManagement.App.Views.Charges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChargesList : ContentPage
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        Account account;
        ChargesListViewModel viewModel;
        Modal modal;

        public ChargesList(Account account)
        {
            this.account = account;
            viewModel = new ChargesListViewModel(account);
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

        private async void Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;

            await PerformSwipe(stackLayout, e);
        }

        private async Task PerformSwipe(StackLayout stackLayout, SwipedEventArgs e, uint timeout = 250)
        {
            var innerLayout = stackLayout.Children.First(x => x.GetType().Equals(typeof(StackLayout))) as StackLayout;

            if (IsAlreadySwiped(innerLayout, e.Direction)) return;

            var buttonsActions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Button)));

            var trashButton = buttonsActions.First();
            var editButton = buttonsActions.Last();

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? 35 : -35;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? 70 : -70;

            var rectIn = innerLayout.Bounds.Offset(offsetIn, 0);
            var rectOut = innerLayout.Bounds.Offset(offsetOut, 0);

            var animationIn = await innerLayout.LayoutTo(rectIn, timeout, easing: Easing.SpringIn);

            if (e.Direction.Equals(SwipeDirection.Left))
            {
                trashButton.IsVisible = editButton.IsVisible = false;
                rectOut.X = 0;
            }

            var animationOut = await innerLayout.LayoutTo(rectOut, timeout, easing: Easing.SpringOut);

            trashButton.IsVisible = editButton.IsVisible = e.Direction.Equals(SwipeDirection.Right);
        }

        private bool IsAlreadySwiped(StackLayout layout, SwipeDirection direction)
            => layout.Bounds.X > 50 && direction.Equals(SwipeDirection.Right) ||
                layout.Bounds.X < 50 && direction.Equals(SwipeDirection.Left);

        private Charge GetCurrentCharge(View view)
        {
            var cell = view.Parent.Parent as ViewCell;
            var charge = cell.BindingContext as Charge;
            return charge;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var innerLayout = stackLayout.Children.First(x => x.GetType().Equals(typeof(StackLayout))) as StackLayout;

            var swipeDirection = innerLayout.Bounds.X > 50 ? SwipeDirection.Left : SwipeDirection.Right;
            Swiped(sender, new SwipedEventArgs(null, swipeDirection));
        }
    }
}