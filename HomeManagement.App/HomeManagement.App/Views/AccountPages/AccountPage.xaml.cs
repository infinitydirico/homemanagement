
using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;
using System.Collections.Generic;
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

        private void ViewChargesList(object sender, ItemTappedEventArgs e)
        {
            var account = e.Item as Account;

            Navigation.PushAsync(new ChargesList(account));

            ((ListView)sender).SelectedItem = null;
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var actions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Image)));

            var trashBin = actions.First();
            trashBin.IsVisible = e.Direction.Equals(SwipeDirection.Right);

            var edit = actions.Last();
            edit.IsVisible = e.Direction.Equals(SwipeDirection.Right);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var accounts = ((AccountsViewModel)accountsList.BindingContext).Accounts;
            var account = accounts.First(x => x.Name.Equals(label.Text));
            Navigation.PushAsync(new ChargesList(account));
        }
    }
}