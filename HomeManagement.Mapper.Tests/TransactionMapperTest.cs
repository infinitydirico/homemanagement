using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManagement.Mapper.Tests
{
    [TestClass]
    public class TransactionMapperTest
    {
        [TestMethod]
        public void GivenEntity_WhenMapping_ShouldReturnModel()
        {
            //Arrange
            TransactionMapper mapper = new TransactionMapper();

            //Act
            var model = mapper.ToModel(new Domain.Transaction
            {
                Id = 1,
                Name = "Credit Card Payment",
                AccountId = 1,
                CategoryId = 1,
                TransactionType = Domain.TransactionType.Expense,
                Date = DateTime.Now,
                Price = 666
            });

            //Assert
            Assert.IsNotNull(model);
        }
    }
}
