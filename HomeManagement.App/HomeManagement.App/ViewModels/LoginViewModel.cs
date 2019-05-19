using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string username = "ramiro.di.rico@gmail.com";
        string password = "4430598Q#$q";

        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly ICryptography crypto = App._container.Resolve<ICryptography>();

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
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public async Task LoginAsync()
        {
            IsBusy = true;

            try
            {
                //for testing purposes.
                //await authServiceClient.Login(new Domain.User
                //{
                //    Email = username,
                //    Password = password
                //});
                //
                //await authServiceClient.Login(new Domain.User
                //{
                //    Email = username,
                //    Password = crypto.Encrypt(password)
                //});
                await authenticationManager.AuthenticateAsync(Username, Password);

                OnLoginSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                OnLoginError?.Invoke(this, EventArgs.Empty);
            }

            IsBusy = false;
        }

        #region labels

        public string UsernameText => "Username";

        public string PasswordText => "Password";

        #endregion
    }
}
