using HomeManagement.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManagement.Mapper.Tests
{
    [TestClass]
    public class AccountMapperTest
    {
        [TestMethod]
        public void GivenAccountModel_WhenMapping_ShouldReturnEntity()
        {
            //Arrange
            AccountMapper mapper = new AccountMapper();

            //Act
            var entity = mapper.ToEntity(new Models.AccountModel
            {
                Id = 1,
                Balance = 6354,
                ExcludeFromStatistics = false,
                AccountType = Models.AccountType.Cash,
                Name = "HSBC",
                UserId = 1
            });

            //Assert
            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void GivenAccountEntity_WhenMapping_ShouldReturnModel()
        {
            //Arrange
            AccountMapper mapper = new AccountMapper();

            //Act
            var model = mapper.ToModel(new Domain.Account
            {
                Id = 1,
                Balance = 6354,
                ExcludeFromStatistics = false,
                AccountType = AccountType.Cash,
                Name = "HSBC",
                UserId = 1
            });

            //Assert
            Assert.IsNotNull(model);
        }
    }
}
