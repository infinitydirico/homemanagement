
using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{
		public AccountPage ()
		{
			InitializeComponent ();

            BindingContext = new AccountsViewModel();
		}

        private void EditCharge(object sender, ItemTappedEventArgs e)
        {
            var account = e.Item as Account;

            Navigation.PushAsync(new ChargesList(account));

            ((ListView)sender).SelectedItem = null;
        }

        private void NavigateToAddAccount(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AddAccountPage());
        }

        private void OnEdit(object sender, EventArgs e)
        {
            DisplayAlert("Edit", "Edit", "ok");
        }

        private void OnRemove(object sender, EventArgs e)
        {
            DisplayAlert("Remove", "Remove", "ok");
        }
    }
}