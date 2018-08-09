using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManagement.Mapper.Tests
{
    [TestClass]
    public class ChargeMapperTest
    {
        [TestMethod]
        public void GivenEntity_WhenMapping_ShouldReturnModel()
        {
            //Arrange
            ChargeMapper mapper = new ChargeMapper();

            //Act
            var model = mapper.ToModel(new Domain.Charge
            {
                Id = 1,
                Name = "Credit Card Payment",
                AccountId = 1,
                CategoryId = 1,
                ChargeType = Domain.ChargeType.Outgoing,
                Date = DateTime.Now,
                Price = 666
            });

            //Assert
            Assert.IsNotNull(model);
        }
    }
}
