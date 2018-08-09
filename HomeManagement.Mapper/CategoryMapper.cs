using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class CategoryMapper : BaseMapper<Category, CategoryModel>, ICategoryMapper
    {
    }
}
