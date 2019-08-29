using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;
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

        public AccountPage ()
		{
			InitializeComponent ();

            modal = new Modal(this);
            BindingContext = viewModel;
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private void DoubleTaped(object sender, System.EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var account = GetCurrentAccount(stackLayout);
            Navigation.PushAsync(new ChargesList(account));
        }

        private async void Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;

            if (IsAlreadySwiped(label, e.Direction)) return;

            var buttonsActions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Button)));

            var trashButton = buttonsActions.First();
            var editButton = buttonsActions.Last();

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? 20 : -20;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? 40 : -40;

            var rectIn = label.Bounds.Offset(offsetIn, 0);
            var rectOut = label.Bounds.Offset(offsetOut, 0);

            var animationIn = await label.LayoutTo(rectIn, easing: Easing.SpringIn);

            if (e.Direction.Equals(SwipeDirection.Left))
            {
                trashButton.IsVisible = editButton.IsVisible = false;
                rectOut.X = 0;
            }

            var animationOut = await label.LayoutTo(rectOut, easing: Easing.SpringOut);

            trashButton.IsVisible = editButton.IsVisible = e.Direction.Equals(SwipeDirection.Right);
        }

        private bool IsAlreadySwiped(Label label, SwipeDirection direction)
            => label.Bounds.X > 20 && direction.Equals(SwipeDirection.Right) ||
                label.Bounds.X < 20 && direction.Equals(SwipeDirection.Left);

        private void Edit(object sender, EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent as StackLayout;
            var account = GetCurrentAccount(stacklayout);
            DisplayAlert("Edit", account.Name, "Ok");
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

        private Account GetCurrentAccount(StackLayout stackLayout)
        {
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var accounts = ((AccountsViewModel)accountsList.BindingContext).Accounts;
            return accounts.First(x => x.Name.Equals(label.Text));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;

            var swipeDirection = label.Bounds.X > 20 ? SwipeDirection.Left : SwipeDirection.Right;
            Swiped(sender, new SwipedEventArgs(null, swipeDirection));
        }
    }
}