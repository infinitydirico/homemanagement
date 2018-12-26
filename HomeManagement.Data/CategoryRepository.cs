using HomeManagement.Domain;
using System;
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

            context.SaveChanges();

            return category;
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

            context.SaveChanges();
        }

        public override bool Exists(Category entity) => GetById(entity.Id) != null;
    }
}
