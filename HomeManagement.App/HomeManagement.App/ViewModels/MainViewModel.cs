using HomeManagement.App.Services.Rest;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAuthServiceClient authController = App._container.Resolve<IAuthServiceClient>();

        public MainViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        public event EventHandler OnLogout;

        public ICommand LogoutCommand { get; }

        public string TitleText => "Home";

        public async void Logout()
        {
            await authController.Logout();

            OnLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}
