using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.AccountPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAccountPage : ContentPage
	{
        EditAccountViewModel viewModel;

        public EditAccountPage (Account account)
		{
			InitializeComponent ();
            viewModel = new EditAccountViewModel(account);
        }
	}
}