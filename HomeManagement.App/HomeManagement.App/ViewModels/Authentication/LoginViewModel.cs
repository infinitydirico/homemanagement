using Autofac;
using HomeManagement.App.Managers;
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
    }
}
