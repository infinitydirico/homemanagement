using HomeManagement.App.Services.Rest;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAuthServiceClient authController = DependencyService.Get<IAuthServiceClient>(DependencyFetchTarget.GlobalInstance);

        public MainViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        public event EventHandler OnLogout;

        public ICommand LogoutCommand { get; }

        public async void Logout()
        {
            await authController.Logout();

            OnLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}
