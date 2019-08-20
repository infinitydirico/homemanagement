using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
        AccountsViewModel viewModel = new AccountsViewModel();

        public AccountPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel;
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private async void SingleTaped(object sender, System.EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;

            await label.ScaleTo(2, easing: Easing.SinIn);
            await label.ScaleTo(1, easing: Easing.SinOut);
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

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? 35 : -35;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? 70 : -70;

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
            => label.Bounds.X > 50 && direction.Equals(SwipeDirection.Right) ||
                label.Bounds.X < 50 && direction.Equals(SwipeDirection.Left);

        private void Edit(object sender, System.EventArgs e)
        {
            var editButton = sender as Button;
            var stacklayout = editButton.Parent as StackLayout;
            var account = GetCurrentAccount(stacklayout);
            DisplayAlert("Edit", account.Name, "Ok");
        }

        private void Delete(object sender, System.EventArgs e)
        {
            var deleteButton = sender as Button;
            var stacklayout = deleteButton.Parent as StackLayout;
            var account = GetCurrentAccount(stacklayout);
            DisplayAlert("Delete", account.Name, "Ok");
        }

        private Account GetCurrentAccount(StackLayout stackLayout)
        {
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var accounts = ((AccountsViewModel)accountsList.BindingContext).Accounts;
            return accounts.First(x => x.Name.Equals(label.Text));
        }
    }
}