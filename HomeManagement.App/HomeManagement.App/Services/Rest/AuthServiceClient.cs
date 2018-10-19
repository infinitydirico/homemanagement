using HomeManagement.App.Common;
using HomeManagement.App.Services.Components;
using HomeManagement.Domain;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.Services.Rest
{
    public class AuthServiceClient : BaseService, IAuthServiceClient
    {
        public User User { get; private set; }

        public async Task<User> Login(User user)
        {
            try
            {
                var result = await Post(user, Constants.Endpoints.Auth.LOGIN);

                App._container.Resolve<IApplicationValues>().Store("header", result.Token.Value);

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
            await Post(user, Constants.Endpoints.Auth.LOGOUT);
            App._container.Resolve<IApplicationValues>().Remove("header");
        }

        public Task Logout()
        {
            throw new NotImplementedException();
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
