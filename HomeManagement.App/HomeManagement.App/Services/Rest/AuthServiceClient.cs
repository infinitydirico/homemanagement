using HomeManagement.App.Common;
using HomeManagement.Models;
using System;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private UserModel user;

        public async Task<UserModel> Login(UserModel user)
        {
            try
            {
                var result = await RestClientFactory
                    .CreateClient()
                    .PostAsync(Constants.Endpoints.Auth.LOGIN, user.SerializeToJson())
                    .ReadContent<UserModel>();

                user = result;
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public async Task Logout(UserModel user)
        {
            await RestClientFactory
                    .CreateAuthenticatedClient()
                    .PostAsync(Constants.Endpoints.Auth.LOGOUT, user.SerializeToJson())
                    .ReadContent<UserModel>();
        }

        public async Task Logout()
        {
            await Logout(user);
        }

        public async Task RegisterAsync(UserModel user)
        {
            var result = await RestClientFactory
                    .CreateClient()
                    .PostAsync(Constants.Endpoints.Auth.REGISTER, user.SerializeToJson());

            result.EnsureSuccessStatusCode();
        }
    }

    public interface IAuthServiceClient
    {
        Task<UserModel> Login(UserModel user);

        Task RegisterAsync(UserModel user);

        Task Logout(UserModel user);
        Task Logout();
    }
}
