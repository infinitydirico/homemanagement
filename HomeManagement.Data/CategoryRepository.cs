using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override Category GetById(int id) => platformContext.GetDbContext().Set<Category>().FirstOrDefault(x => x.Id.Equals(id));

        public override void Add(Category entity)
        {
            throw new ArgumentException($"there is no argument for type {nameof(User)}");
        }

        public Category Add(Category entity, User user)
        {
            var context = platformContext.GetDbContext();

            Category category = null;

            var set = context.Set<Category>();

            category = set.FirstOrDefault(x => x.Name.Equals(entity.Name) && x.IsDefault);

            if (category == null)
            {
                category = set.Add(entity).Entity;

                context.SaveChanges();
            }

            context.Set<UserCategory>().Add(new UserCategory
            {
                UserId = user.Id,
                CategoryId = category.Id
            });

            return category;
        }

        public void Add(IEnumerable<Category> categories, User user)
        {
            var categorySet = dbContext.Set<Category>();
            var userCategorySet = dbContext.Set<UserCategory>();

            var missingCategories = (from c in categorySet
                                    join ca in categories
                                    on c.Name equals ca.Name
                                    where c.IsDefault
                                    select c).ToList();

            if (missingCategories.Any())
            {
                categorySet.AddRange(missingCategories);
            }

            var userCategories = missingCategories.Select(x => new UserCategory
            {
                CategoryId = x.Id,
                UserId = user.Id
            }).ToList();

            userCategorySet.AddRange(userCategories);
        }

        public override void Remove(int id)
        {
            throw new ArgumentException($"there is no argument for type {nameof(User)}");
        }

        public void Remove(int categoryId, User user)
        {
            var context = platformContext.GetDbContext();

            var userCategorySet = context.Set<UserCategory>();

            var userCategory = userCategorySet.FirstOrDefault(x => x.UserId.Equals(user.Id) && x.CategoryId.Equals(categoryId));

            if (userCategory != null)
            {
                userCategorySet.Remove(userCategory);
            }

            var set = context.Set<Category>();

            var entity = GetById(categoryId);

            if (userCategorySet.Any(x => x.CategoryId.Equals(entity.Id))) return;

            set.Remove(entity);
        }

        public void Remove(IEnumerable<Category> categories, User user)
        {
            var categorySet = dbContext.Set<Category>();
            var userCategorySet = dbContext.Set<UserCategory>();

            var userCategories = (from uc in userCategorySet
                                 join c in categories
                                 on uc.CategoryId equals c.Id
                                 where uc.UserId.Equals(user.Id)
                                 select uc).ToList();

            userCategorySet.RemoveRange(userCategories);

            var categoriesToRemove = (from uc in userCategories
                                      where !(from c in categorySet select c.Id).Contains(uc.CategoryId)
                                      select uc.Category).ToList();

            categorySet.RemoveRange(categories);
        }

        public override bool Exists(Category entity) => GetById(entity.Id) != null;

        public IEnumerable<Category> GetUserCategories(string username)
        {
            var context = platformContext.GetDbContext();

            var categoryQuery = context.Set<Category>().AsQueryable();

            var userCategoryQuery = context.Set<UserCategory>().AsQueryable();

            var userQuery = context.Set<User>().AsQueryable();

            var categories = (from category in categoryQuery
                              join userCategory in userCategoryQuery
                              on category.Id equals userCategory.CategoryId
                              join user in userQuery
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(username)
                              select category).ToList();

            return categories;
        }

        public IEnumerable<Category> GetActiveUserCategories(string username)
        {
            var context = platformContext.GetDbContext();

            var categoryQuery = context.Set<Category>().AsQueryable();

            var userCategoryQuery = context.Set<UserCategory>().AsQueryable();

            var userQuery = context.Set<User>().AsQueryable();

            var categories = (from category in categoryQuery
                              join userCategory in userCategoryQuery
                              on category.Id equals userCategory.CategoryId
                              join user in userQuery
                              on userCategory.UserId equals user.Id
                              where user.Email.Equals(username) && category.IsActive
                              select category).ToList();

            return categories;
        }
    }
}
