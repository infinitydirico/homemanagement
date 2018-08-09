using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string username = "ramiro.di.rico@gmail.com";
        string password = "4430598Q#$q";

        private IAuthServiceClient authServiceClient;
        private ICryptography crypto;

        public LoginViewModel()
        {
            crypto = DependencyService.Get<ICryptography>(DependencyFetchTarget.GlobalInstance);

            authServiceClient = DependencyService.Get<IAuthServiceClient>(DependencyFetchTarget.GlobalInstance);
        }

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
            await authServiceClient.Login(new Domain.User
            {
                Email = username,
                Password = crypto.Encrypt(password)
            });
            IsBusy = false;
        }

        #region labels

        public string UsernameText => language.CurrentLanguage.UsernameText;

        public string PasswordText => language.CurrentLanguage.PasswordText;

        public string LoginButtonText => language.CurrentLanguage.LoginText;

        #endregion
    }
}
