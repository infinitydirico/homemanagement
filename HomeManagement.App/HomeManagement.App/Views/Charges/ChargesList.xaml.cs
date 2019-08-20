using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.AccountPages;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Charges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChargesList : ContentPage
    {
        Account account;
        ChargesListViewModel viewModel;

        public ChargesList(Account account)
        {
            this.account = account;
            viewModel = new ChargesListViewModel(account);

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

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var charge = GetCurrentCharge(editButton);
            var editChargePage = new EditCharge(account, charge);
            NavigationPage.SetHasBackButton(editChargePage, true);

            Navigation.PushAsync(editChargePage);
        }

        private void Delete(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var charge = GetCurrentCharge(editButton);
            viewModel.DeleteCommand.Execute(charge);
        }

        private async void Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var innerLayouts = stackLayout.Children.First(x => x.GetType().Equals(typeof(StackLayout))) as StackLayout;

            if (IsAlreadySwiped(innerLayouts, e.Direction)) return;

            var buttonsActions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Button)));

            var trashButton = buttonsActions.First();
            var editButton = buttonsActions.Last();

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? 35 : -35;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? 70 : -70;

            var rectIn = innerLayouts.Bounds.Offset(offsetIn, 0);
            var rectOut = innerLayouts.Bounds.Offset(offsetOut, 0);

            var animationIn = await innerLayouts.LayoutTo(rectIn, easing: Easing.SpringIn);

            if (e.Direction.Equals(SwipeDirection.Left))
            {
                trashButton.IsVisible = editButton.IsVisible = false;
                rectOut.X = 0;
            }

            var animationOut = await innerLayouts.LayoutTo(rectOut, easing: Easing.SpringOut);

            trashButton.IsVisible = editButton.IsVisible = e.Direction.Equals(SwipeDirection.Right);
        }

        private bool IsAlreadySwiped(StackLayout layout, SwipeDirection direction)
            => layout.Bounds.X > 50 && direction.Equals(SwipeDirection.Right) ||
                layout.Bounds.X < 50 && direction.Equals(SwipeDirection.Left);

        private Charge GetCurrentCharge(Button button)
        {
            var parentLayout = button.Parent as StackLayout;
            var layout = parentLayout.Children.First(x => x.GetType().Equals(typeof(StackLayout))) as StackLayout;
            var label = layout.Children.First() as Label;
            var charge = viewModel.Charges.First(x => x.Name.Equals(label.Text));
            return charge;
        }
    }
}