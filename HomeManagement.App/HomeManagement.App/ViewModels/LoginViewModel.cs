using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts;
using Autofac;
using System.Threading.Tasks;
using HomeManagement.Data;

namespace HomeManagement.App.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        string username = "ramiro.di.rico@gmail.com";
        string password = "4430598Q#$q";

        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly ICryptography crypto = App._container.Resolve<ICryptography>();
        private readonly IUserRepository userRepository = App._container.Resolve<IUserRepository>();

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
