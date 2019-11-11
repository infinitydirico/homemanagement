using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using Plugin.Media;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTransactionPage : ContentPage
	{
        private Account account;
        private AddTransactionViewModel viewModel;
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        private AddTransactionPage()
        {
            InitializeComponent();

            Title = localizationManager.Translate("NewMovement");
        }

        public AddTransactionPage(Account account) : this()
        {
            this.account = account;

            viewModel = new AddTransactionViewModel(account);

            BindingContext = viewModel;

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Icon = "add_photo_24dp.png",
                    Command = viewModel.TakePictureCommand
                });
            }

            viewModel.OnAdded += OnTransactionAdded;

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

        private void OnTransactionAdded(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            TransactionNameEntry.Focus();
            base.OnAppearing();
        }
    }
}