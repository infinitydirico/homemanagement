using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Transactions;

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

        private void EditTransaction(object sender, ItemTappedEventArgs e)
        {
            var account = e.Item as Account;

            Navigation.PushAsync(new TransactionListPage(account));

            ((ListView)sender).SelectedItem = null;
        }

        private async void StackLayout_LayoutChanged(object sender, System.EventArgs e)
        {
            var layout = sender as StackLayout;
            await layout.FadeTo(1);
        }
    }
}