using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Charges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCharge : ContentPage
	{
        private Account account;
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public EditCharge()
        {
            InitializeComponent();

            Title = localizationManager.Translate("EditCharge");
        }

        public EditCharge(Account account, Charge charge) : this()
        {
            this.account = account;
            var viewModel = new EditTransactionViewModel(account, charge);
            BindingContext = viewModel;

            viewModel.OnChargeUpdated += ViewModel_OnChargeUpdated;
            viewModel.OnError += ViewModel_OnError;
            viewModel.OnCancel += ViewModel_OnCancel;
        }

        private void ViewModel_OnCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ViewModel_OnError(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Algunos de los datos ingresados no son validos", string.Empty);
        }

        private void ViewModel_OnChargeUpdated(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}