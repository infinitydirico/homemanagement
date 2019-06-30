using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Managers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class SignUpViewModel : LocalizationBaseViewModel
    {
        string username = string.Empty;
        string password = string.Empty;
        string confirmPassword = string.Empty;

        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged();
                RaiseEnableSignUp();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
                RaiseEnableSignUp();
            }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                confirmPassword = value;
                OnPropertyChanged();
                RaiseEnableSignUp();
            }
        }

        public bool EnableSignUp { get; protected set; }

        private void RaiseEnableSignUp()
        {
            EnableSignUp = !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(confirmPassword);
            OnPropertyChanged(nameof(EnableSignUp));
        }

        public ICommand SignUpCommand => new Command(async () => await SignUpAsync());

        public string UsernameText => "Username";

        public string PasswordText => "Password";

        public string ConfirmPasswordText => "ConfirmPassword";

        private async Task SignUpAsync()
        {
            await HandleSafeExecutionAsync(async () =>
            {
                if (!Password.Equals(ConfirmPassword))
                {
                    throw new AppException("The passwords does not match.");
                }

                await authenticationManager.RegisterAsync(Username, Password);
            });
        }
    }
}
