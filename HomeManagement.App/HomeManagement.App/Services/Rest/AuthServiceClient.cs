using HomeManagement.App.Common;
using HomeManagement.App.Services.Components;
using HomeManagement.Domain;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Services.Rest
{
    public class AuthServiceClient : BaseService, IAuthServiceClient
    {
        public User User { get; private set; }

        public async Task<User> Login(User user)
        {
            var result = await Post(user, Constants.Endpoints.Auth.LOGIN);

            DependencyService.Get<IMetadataHandler>().StoreValue("header", result.Token.Value);

            User = result;

            return result;
        }

        public async Task Logout(User user)
        {
            await Post(user, Constants.Endpoints.Auth.LOGOUT);
            DependencyService.Get<IMetadataHandler>().Remove("header");
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
