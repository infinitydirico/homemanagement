using Autofac;
using HomeManagement.App.Common;
using HomeManagement.Core.Caching;
using HomeManagement.Domain;
using System;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AuthServiceClient : IAuthServiceClient
    {
        public User User { get; private set; }

        public async Task<User> Login(User user)
        {
            try
            {
                var result = await RestClientFactory
                    .CreateClient()
                    .PostAsync(Constants.Endpoints.Auth.LOGIN, user.SerializeToJson())
                    .ReadContent<User>();

                App._container.Resolve<ICachingService>().Store("header", result.Token);

                User = result;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Logout(User user)
        {
            await RestClientFactory
                    .CreateAuthenticatedClient()
                    .PostAsync(Constants.Endpoints.Auth.LOGOUT, user.SerializeToJson())
                    .ReadContent<User>();

            App._container.Resolve<ICachingService>().Remove("header");
        }

        public async Task Logout()
        {
            await Logout(User);
        }
    }

    public interface IAuthServiceClient
    {
        Task<User> Login(User user);

        User User { get; }

        Task Logout(User user);
        Task Logout();
    }
}
