using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface IAuthenticationManager
    {
        Task<User> AuthenticateAsync(string username, string password);

        User GetAuthenticatedUser();
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly ICryptography crypto = App._container.Resolve<ICryptography>();
        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();
        private readonly GenericRepository<User> userRepository = new GenericRepository<User>();

        private User user;

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var encryptedPassword = crypto.Encrypt(password);

            user = userRepository.All.FirstOrDefault(x => x.Email.Equals(username));// && x.Password.Equals(password));

            if (user != null)
            {
                cachingService.Store("header", user.Token);
                return user;
            }

            var userModel = await authServiceClient.Login(new UserModel { Email = username, Password = encryptedPassword });

            user = new User
            {
                Id = userModel.Id,
                Email = userModel.Email,
                Password = encryptedPassword,
                ChangeStamp = DateTime.Now,
                LastApiCall = DateTime.Now,
                Token = userModel.Token               
            };

            userRepository.Add(user);

            userRepository.Commit();

            return user;
        }

        public User GetAuthenticatedUser() => user;
    }
}
