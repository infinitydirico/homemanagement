using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using System;

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