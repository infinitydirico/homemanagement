using HomeManagement.App.ViewModels;
using HomeManagement.Domain;
using System;
using System.Threading.Tasks;

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

            BindingContext = viewModel;

            Title = account.Name;

            InitializeComponent();
        }

        protected async Task OnAddChargeCommand(object sender, EventArgs e)
        {
            var page = new AddCharge(account);

            NavigationPage.SetHasBackButton(page, true);

            NavigationPage.SetHasNavigationBar(page, true);

            await Navigation.PushAsync(page);
        }

        private void OnRemove(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            viewModel.DeleteCommand.Execute(menuItem.CommandParameter);
        }

        private void OnEdit(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var charge = menuItem.CommandParameter as Charge;

            var editChargePage = new EditCharge(account, charge)
            {
                Title = "Editar Movimiento"
            };
            NavigationPage.SetHasBackButton(editChargePage, true);

            Navigation.PushAsync(editChargePage);
        }

        private void chargesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}