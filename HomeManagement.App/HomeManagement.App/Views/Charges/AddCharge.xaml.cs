using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Charges
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCharge : ContentPage
	{
        private Account account;
        private AddChargeViewModel viewModel;
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        private AddCharge()
        {
            InitializeComponent();

            Title = localizationManager.Translate("NewMovement");
        }

        public AddCharge(Account account) : this()
        {
            this.account = account;

            viewModel = new AddChargeViewModel(account);

            BindingContext = viewModel;

            viewModel.OnAdded += OnChargeAdded;

            viewModel.OnCancel += OnCancel;

            viewModel.OnError += ViewModel_OnError;
        }

        private void ViewModel_OnError(object sender, System.EventArgs e)
        {
            DisplayAlert("Error", "Algunos de los datos ingresados no son validos", "cancel");
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void OnChargeAdded(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            chargeNameEntry.Focus();
            base.OnAppearing();
        }
    }
}