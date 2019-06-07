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
        private ToolbarItem editToolItem;

        public ChargesList(Account account)
        {
            this.account = account;
            viewModel = new ChargesListViewModel(account);

            viewModel.OnError += ViewModel_OnError;
            viewModel.OnInitializationError += ViewModel_OnInitializationError;

            BindingContext = viewModel;

            Title = account.Name;
            ShowAddChargeToolbar();
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

        private void OnEdit(object sender, EventArgs e)
        {
            var editChargePage = new EditCharge(account, chargesList.SelectedItem as Charge)
            {
                Title = "Editar Movimiento"
            };
            NavigationPage.SetHasBackButton(editChargePage, true);

            Navigation.PushAsync(editChargePage);
        }

        private void ShowAddChargeToolbar()
        {
            ToolbarItems.Clear();

            var toolItem = new ToolbarItem
            {
                Icon = "add.png"
            };

            toolItem.Clicked += OnAddChargeCommand;
            ToolbarItems.Add(toolItem);
        }

        private void ShowEditionToolbar(object sender, ItemTappedEventArgs e)
        {
            ToolbarItems.Clear();

            var cancelToolbarItem = new ToolbarItem
            {
                Icon = "close.png"
            };
            cancelToolbarItem.Clicked += (s, ev) =>
            {
                chargesList.SelectedItem = null;
                ShowAddChargeToolbar();
            };
            ToolbarItems.Add(cancelToolbarItem);

            var editToolItem = new ToolbarItem
            {
                Icon = "edit.png"
            };

            editToolItem.Clicked += OnEdit;
            ToolbarItems.Add(editToolItem);

            var deleteItem = new ToolbarItem
            {
                Icon = "red_trash.png",
                IsDestructive = true
            };
            deleteItem.Clicked += (s, ev) =>
            {
                viewModel.DeleteCommand.Execute(chargesList.SelectedItem);
            };
            ToolbarItems.Add(deleteItem);
        }
    }
}