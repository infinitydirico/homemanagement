using HomeManagement.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Data
{
    public static class CategoryInitializer
    {
        public static void SeedCategories(this WebAppDbContext context)
        {
            if (context.Categories.Count() > 0) return;

            context.Categories.AddRange(GetDefaultCategories());

            context.SaveChanges();
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
