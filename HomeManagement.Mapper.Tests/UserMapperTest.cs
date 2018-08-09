using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManagement.Mapper.Tests
{
    [TestClass]
    public class UserMapperTest
    {
        [TestMethod]
        public void GivenUserModel_WhenMappingToEntity_ShouldReturnEntity()
        {
            UserMapper userMapper = new UserMapper();

            var entity = userMapper.ToEntity(new Models.UserModel
            {
                Id = 1,
                Email = "email@email.com",
                Password = "1234"
            });

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void GivenUser_WhenMappingToModel_ShouldReturnModel()
        {
            UserMapper userMapper = new UserMapper();

            var entity = userMapper.ToModel(new Domain.User
            {
                Id = 1,
                Email = "email@email.com",
                Password = "1234"
            });

            Assert.IsNotNull(entity);
        }
    }
}
