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
                Navigation.RemovePage(this);
            };

            ((AddAccountViewModel)BindingContext).OnError += (s, e) =>
            {
                DisplayAlert("Error", e.ErrorMessage, "Ok");
            };

        }
	}
}