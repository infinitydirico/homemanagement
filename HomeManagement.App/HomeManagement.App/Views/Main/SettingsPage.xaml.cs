using HomeManagement.App.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();

            BindingContext = new SettingsViewModel();
		}
	}
}