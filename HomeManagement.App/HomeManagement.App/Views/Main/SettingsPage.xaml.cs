using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Managers;
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

            Title = localizationManager.Translate("Settings");

#if DEBUG
            logFileFrame.IsVisible = true;
#endif
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