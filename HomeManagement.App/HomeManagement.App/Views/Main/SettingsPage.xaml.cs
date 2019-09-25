using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Login;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

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

            Title = localizationManager.Translate("Settings");

#if DEBUG
            logFileFrame.IsVisible = true;
#endif
        }

        protected override void OnAppearing()
        {
            ((SettingsViewModel)BindingContext).RefreshCaching();
            base.OnAppearing();
        }

        private void OnLogoutClicked(object sender, System.EventArgs e)
        {
            MessagingCenter.Send(this, Constants.Messages.Logout);
        }

        private void ViewLogFile(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LogFilePage());
        }
    }
}