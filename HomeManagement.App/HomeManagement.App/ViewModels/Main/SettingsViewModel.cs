using Autofac;
using HomeManagement.App.Managers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class SettingsViewModel : LocalizationBaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        public SettingsViewModel()
        {
            LogoutCommand = new Command(Logout);
        }

        public event EventHandler OnLogout;

        public ICommand LogoutCommand { get; } 

        public ICommand ChangeLanguageCommand { get; set; }

        public async void Logout()
        {
            await authenticationManager.Logout();
            await Shell.Current.GoToAsync("//login");

            OnLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}