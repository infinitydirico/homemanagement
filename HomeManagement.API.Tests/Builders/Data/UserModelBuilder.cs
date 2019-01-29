using HomeManagement.Core.Cryptography;
using HomeManagement.Models;
using System;
using System.Linq;

namespace HomeManagement.API.Tests.Builders.Data
{
    public class UserModelBuilder
    {
        private readonly AesCryptographyService aesCryptographyService = new AesCryptographyService();

        public UserModel CreateUserModel(string email, string password, string language = "en") => new UserModel
        {
            Email = email,
            Password = password,
            Language = language
        };

        public UserModel CreateRandomUserModel()
        {
            var values = Guid.NewGuid().ToString("N");
            var username = string.Concat(values.Take(6));
            var password = DefaultPassword();
            var userModel = CreateUserModel(username, password);
            return userModel;
        }

        private string DefaultPassword() => "4h5UHGckxny7Lux9A1g0mA==";
    }

    public static class UserModelBuilderExtensions
    {

    }
}
