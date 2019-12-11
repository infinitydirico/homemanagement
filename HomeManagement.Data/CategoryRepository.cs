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

        public static IEnumerable<Category> GetDefaultCategories() => new List<Category>
        {
            new Category
            {
                Name = "Various",
                Icon = "layers",
                IsActive = true
            },
            new Category
            {
                Name = "Rent",
                Icon = "home",
                IsActive = true
            },
            new Category
            {
                Name = "Transportation",
                Icon = "directions_bus",
                IsActive = true
            },
            new Category
            {
                Name = "Supplies",
                Icon = "shopping_basket",
                IsActive = true
            },
            new Category
            {
                Name = "Entertainment",
                Icon = "theaters",
                IsActive = true
            },
            new Category
            {
                Name = "Gift",
                Icon = "card_giftcard",
                IsActive = true
            },
            new Category
            {
                Name = "Clothes",
                Icon = "wc",
                IsActive = true
            },

            new Category
            {
                Name = "Education",
                Icon = "book",
                IsActive = true
            },
            new Category
            {
                Name = "Health",
                Icon = "healing",
                IsActive = true
            },
            new Category
            {
                Name = "Salary",
                Icon = "attach_money",
                IsActive = true
            },
            new Category
            {
                Name = "Bond",
                Icon = "local_atm",
                IsActive = true
            },
            new Category
            {
                Name = "Services",
                Icon = "credit_card",
                IsActive = true
            },
            new Category
            {
                Name = "Savings",
                Icon = "account_balance_wallet",
                IsActive = true
            },
            new Category
            {
                Name = "Transfers",
                Icon = "compare_arrows",
                IsActive = true
            },

            new Category
            {
                Name = "Donations",
                Icon = "child_care",
                IsActive = true
            },
            new Category
            {
                Name = "Extraction",
                Icon = "autorenew",
                IsActive = true
            },
             new Category
            {
                Name = "Deposit",
                Icon = "input",
                IsActive = true
            },
        };
    }
}
