using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context)
            : base(context)
        {

        }
        public override Category GetById(int id) => context.Set<Category>().FirstOrDefault(x => x.Id.Equals(id));

        public override bool Exists(Category entity) => GetById(entity.Id) != null;

        public IEnumerable<Category> GetUserCategories(string username)
        {
            var categoryQuery = context.Set<Category>().AsQueryable();

            var userQuery = context.Set<User>().AsQueryable();

            var categories = (from category in categoryQuery
                              join user in userQuery
                              on category.UserId equals user.Id
                              where user.Email.Equals(username)
                              select category).ToList();

            return categories;
        }

        public IEnumerable<Category> GetActiveUserCategories(string username)
        {
            var categoryQuery = context.Set<Category>().AsQueryable();

            var userQuery = context.Set<User>().AsQueryable();

            var categories = (from category in categoryQuery
                              join user in userQuery
                              on category.UserId equals user.Id
                              where user.Email.Equals(username) && category.IsActive
                              select category).ToList();

            return categories;
        }
    }
}
