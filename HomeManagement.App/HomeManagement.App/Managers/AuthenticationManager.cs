using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Core.Extensions;
using HomeManagement.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.Managers
{
    public interface IAuthenticationManager
    {
        Task<User> AuthenticateAsync(string username, string password);

        Task<bool> RegisterAsync(string username, string password);

        User GetAuthenticatedUser();

        bool AreCredentialsAvaible();

        bool HasValidCredentialsAvaible();

        User GetStoredUser();

        Task Logout();

        bool IsAuthenticated();

        event EventHandler OnAuthenticationChanged;
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ICryptography crypto = App._container.Resolve<ICryptography>();
        private readonly AuthServiceClient authServiceClient = new AuthServiceClient();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();
        private readonly GenericRepository<AppSettings> appSettingsRepository = new GenericRepository<AppSettings>();
        string username;
        string password;
        string token;
        DateTime lastApiCall;
        bool isAuthenticated;

        public AuthenticationManager()
        {
            Load().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public event EventHandler OnAuthenticationChanged;

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            try
            {
                if (AreCredentialsAvaible() && HasValidCredentialsAvaible()) return GetStoredUser();

                var encryptedPassword = crypto.Encrypt(password);

                var userModel = await authServiceClient.Login(new UserModel { Email = username, Password = encryptedPassword });

                var user = new User
                {
                    Id = userModel.Id,
                    Email = userModel.Email,
                    Password = password,
                    ChangeStamp = DateTime.Now,
                    LastApiCall = DateTime.Now,
                    Token = userModel.Token
                };

                await SaveOrUpdateUser(user);

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                isAuthenticated = true;

                OnAuthenticationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public User GetAuthenticatedUser() => GetStoredUser();

        public async Task Logout()
        {
            isAuthenticated = false;
            OnAuthenticationChanged?.Invoke(this, EventArgs.Empty);
            //await authServiceClient.Logout();
            if (cachingService.Exists("user"))
            {
                cachingService.Remove("user");
            }
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var encryptedPassword = crypto.Encrypt(password);

            await authServiceClient.RegisterAsync(new UserModel { Email = username, Password = encryptedPassword });

            cachingService.Store("singupuser", new User { Email = username, Password = password });

            return true;
        }

        private async Task SaveOrUpdateUser(User user)
        {
            await SecureStorage.SetAsync("Username", user.Email);
            await SecureStorage.SetAsync("Password", user.Password);
            Preferences.Set("LastApiCall", user.LastApiCall);
            await SecureStorage.SetAsync("Token", user.Token);
        }

        public bool AreCredentialsAvaible() => username.IsNotEmpty() && password.IsNotEmpty();

        public bool HasValidCredentialsAvaible() => (DateTime.Now - lastApiCall).TotalHours < 1;

        public User GetStoredUser() => new User
        {
            Email = username,
            Password = password,
            LastApiCall = lastApiCall,
            NeedsUpdate = false,
            Token = token
        };

        private async Task Load()
        {
            username = await SecureStorage.GetAsync("Username");
            password = await SecureStorage.GetAsync("Password");
            lastApiCall = Preferences.Get("LastApiCall", DateTime.MinValue);
            token = await SecureStorage.GetAsync("Token");
        }

        public bool IsAuthenticated() => isAuthenticated;
    }
}
