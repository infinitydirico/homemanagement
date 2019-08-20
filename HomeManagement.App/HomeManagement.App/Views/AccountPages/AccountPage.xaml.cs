
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

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var label = stackLayout.Children.First(x => x.GetType().Equals(typeof(Label))) as Label;
            var actions = stackLayout.Children.Where(x => x.GetType().Equals(typeof(Button)));

            var trashBin = actions.First();
            var edit = actions.Last();

            var offsetIn = e.Direction.Equals(SwipeDirection.Right) ? 35 : -35;
            var offsetOut = e.Direction.Equals(SwipeDirection.Right) ? 70 : -70;

            var rectIn = label.Bounds.Offset(offsetIn, 0);
            var rectOut = label.Bounds.Offset(offsetOut, 0);

            if ((label.Bounds.X < 50 && e.Direction.Equals(SwipeDirection.Right))
                || (label.Bounds.X > 50 && e.Direction.Equals(SwipeDirection.Left)))
            {
                var animationIn = await label.LayoutTo(rectIn, easing: Easing.SinIn);

                if (e.Direction.Equals(SwipeDirection.Left))
                {
                    trashBin.IsVisible = edit.IsVisible = false;
                    rectOut.X = 0;
                }

                var animationOut = await label.LayoutTo(rectOut, easing: Easing.SinOut);
            }
            
            trashBin.IsVisible = edit.IsVisible = e.Direction.Equals(SwipeDirection.Right);
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