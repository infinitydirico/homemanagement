using Autofac;
using HomeManagement.App.Managers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class MainViewModel : LocalizationBaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        public MainViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        public event EventHandler OnLogout;

        public ICommand LogoutCommand { get; }

        public string TitleText => "Home";

        public async void Logout()
        {
            await authenticationManager.Logout();

            OnLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}
