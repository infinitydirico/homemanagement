using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Charges;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountListControl : StackLayout
	{
		public AccountListControl ()
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
    }
}