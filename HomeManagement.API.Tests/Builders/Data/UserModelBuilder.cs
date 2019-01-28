using HomeManagement.Models;
using System;
using System.Linq;

namespace HomeManagement.API.Tests.Builders.Data
{
    public class UserModelBuilder
    {
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
            var password = "4430598Q#$q"; //"4h5UHGckxny7Lux9A1g0mA==";//string.Concat(values.Skip(6).Take(6), values.First().ToString().ToUpper());
            var userModel = CreateUserModel(username, password);
            return userModel;
        }
    }

    public static class UserModelBuilderExtensions
    {

    }
}
