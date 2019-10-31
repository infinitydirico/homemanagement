
using HomeManagement.App.ViewModels.Main;
using HomeManagement.Models;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationsPage : ContentPage
	{
		public NotificationsPage ()
		{
			InitializeComponent ();
		}

        private void NotificationsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Task.Run(async () =>
            {
                await ((NotificationsViewModel)BindingContext).Dismiss(e.Item as NotificationModel);
            });
        }
    }
}