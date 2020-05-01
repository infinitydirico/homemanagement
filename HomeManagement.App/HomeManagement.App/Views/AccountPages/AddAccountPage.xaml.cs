using HomeManagement.App.Common;
using HomeManagement.App.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAccountPage : ContentPage
	{
		public AddAccountPage ()
		{
			InitializeComponent ();

            ((AddAccountViewModel)BindingContext).OnAccountCreated += (s, e) =>
            {
                MessagingCenter.Send(this, Constants.Messages.UpdateOnAppearing);
                Navigation.PopAsync();
            };

            ((AddAccountViewModel)BindingContext).OnError += (s, e) =>
            {
                DisplayAlert("Error", e.ErrorMessage, "Ok");
            };
        }

        protected override void OnAppearing()
        {
            accountName.Focus();

            base.OnAppearing();
        }
    }
}