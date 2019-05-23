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

            ((SettingsViewModel)BindingContext).OnError += (s,e) =>
            {
                DisplayAlert("Error", e.ErrorMessage, "Ok");
            };

            ((SettingsViewModel)BindingContext).OnClearSuccess += (s, e) =>
            {
                DisplayAlert("Info", "The cache has been cleared out.", "Ok");
            };
		}
	}
}