using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManagement.Mapper.Tests
{
    [TestClass]
    public class CategoryMapperTest
    {
        [TestMethod]
        public void GivenEntity_WhenMapping_ShouldReturnModel()
        {
            //Arrange
            CategoryMapper mapper = new CategoryMapper();

            //Act

            var model = mapper.ToModel(new Domain.Category
            {
                Id = 1,
                IsActive = true,
                IsDefault = false,
                Icon = "icon.png",
                Name = "Services",                
            });

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void GivenModel_WhenMapping_ShouldReturnEntity()
        {
            //Arrange
            CategoryMapper mapper = new CategoryMapper();

            //Act

            var entity = mapper.ToEntity(new Models.CategoryModel
            {
                Id = 1,
                IsActive = true,
                IsDefault = false,
                Icon = "icon.png",
                Name = "Services",
            });

            Assert.IsNotNull(entity);
        }
    }
}
