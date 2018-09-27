using System.Collections.Generic;
using System.Reflection;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class UserMapper : BaseMapper<User, UserModel>, IUserMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(User).GetProperty(nameof(User.Id));
            yield return typeof(User).GetProperty(nameof(User.Email));
            yield return typeof(User).GetProperty(nameof(User.Password));

        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(UserModel).GetProperty(nameof(UserModel.Id));
            yield return typeof(UserModel).GetProperty(nameof(UserModel.Email));
            yield return typeof(UserModel).GetProperty(nameof(UserModel.Password));
        }
    }
}
