using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.Core.Caching;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string username = "test@test.com";
        string password = "123Abc!";

        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
        }

        public event EventHandler OnLoginError;
        public event EventHandler OnLoginSuccess;

        public ICommand LoginCommand { get; }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && NotBusy;

        protected override async Task InitializeAsync()
        {
            if (cachingService.Exists("singupuser"))
            {
                var user = cachingService.Get<User>("singupuser");
                Username = user.Email;
                Password = user.Password;
                return;
            }

            if (authenticationManager.AreCredentialsAvaible())
            {
                var user = authenticationManager.GetStoredUser();
                Username = user.Email;
                Password = user.Password;
            }
        }

        public async Task LoginAsync()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(CanLogin));
            try
            {
                await authenticationManager.AuthenticateAsync(Username, Password);

                OnLoginSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                OnLoginError?.Invoke(this, EventArgs.Empty);
            }

            IsBusy = false;
            OnPropertyChanged(nameof(CanLogin));
        }

        #region labels

        public string UsernameText => "Username";

        public string PasswordText => "Password";

        #endregion
    }
}
