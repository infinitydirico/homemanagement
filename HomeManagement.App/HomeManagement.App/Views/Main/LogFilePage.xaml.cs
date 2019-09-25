using HomeManagement.App.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogFilePage : ContentPage
	{
		public LogFilePage ()
		{
			InitializeComponent ();

            ShowLogs();
        }

        private void ClearFiles(object sender, System.EventArgs e)
        {
            Logger.Clear();
            ShowLogs();
        }

        private void ShowLogs()
        {
            logtext.Text = Logger.ReadLog();
        }
    }
}