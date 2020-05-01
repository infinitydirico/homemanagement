using Autofac;
using HomeManagement.App.Managers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class MainViewModel : LocalizationBaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        bool isAuthenticated = false;

        public MainViewModel()
        {
            LogoutCommand = new Command(Logout);
            CheckAuthenticationStatus().GetAwaiter().GetResult();
        }

        public event EventHandler OnLogout;

        public ICommand LogoutCommand { get; }

        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set
            {
                isAuthenticated = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NeedsAuthentication));
            }
        }

        public bool NeedsAuthentication => !IsAuthenticated;

        public string TitleText => "Home";

        private async Task CheckAuthenticationStatus()
        {
            if (authenticationManager.HasValidCredentialsAvaible() && authenticationManager.AreCredentialsAvaible())
            {
                var user = authenticationManager.GetStoredUser();
                await authenticationManager.AuthenticateAsync(user.Email, user.Password);
                IsAuthenticated = authenticationManager.IsAuthenticated();
                authenticationManager.OnAuthenticationChanged += OnAuthenticationChanged;
            }
        }

        private async void OnAuthenticationChanged(object sender, EventArgs e)
        {
            await CheckAuthenticationStatus();
        }

        public async void Logout()
        {
            await authenticationManager.Logout();

            OnLogout?.Invoke(this, EventArgs.Empty);
        }
    }
}