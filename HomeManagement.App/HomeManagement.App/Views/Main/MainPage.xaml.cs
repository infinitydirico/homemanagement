using HomeManagement.App.Common;
using HomeManagement.App.ViewModels;
using HomeManagement.App.Views.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<Settings>(settingsPage, Constants.Messages.Logout, page =>
            {
                ((MainViewModel)BindingContext).LogoutCommand.Execute(null);
                MessagingCenter.Unsubscribe<Settings>(settingsPage, Constants.Messages.Logout);
            });

            ((MainViewModel)BindingContext).OnLogout += (s, e) =>
            {
                Navigation.PushAsync(new LoginPage());
                Navigation.RemovePage(this);
            };
        }
    }
}