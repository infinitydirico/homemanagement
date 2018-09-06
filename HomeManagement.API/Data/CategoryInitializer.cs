using HomeManagement.Domain;
using System.Collections.Generic;
using System.Globalization;
using HomeManagement.API.Extensions;

namespace HomeManagement.API.Data
{
    public static class CategoryInitializer
    {
        public static IEnumerable<Category> GetDefaultCategories(CultureInfo culture = null) => new List<Category>
        {
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Various" : "Varios") : "Various",
                Icon = "layers",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Rent" : "Alquiler") : "Rent",
                Icon = "home",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Transportation" : "Transporte") : "Transportation",
                Icon = "directions_bus",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Supplies" : "Insumos") : "Supplies",
                Icon = "shopping_basket",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Entertainment" : "Entretenimiento") : "Entertainment",
                Icon = "theaters",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Gift" : "Regalo") : "Gift",
                Icon = "card_giftcard",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Clothes" : "Vestimenta") : "Clothes",
                Icon = "wc",
                IsActive = true,
                IsDefault = true
            },

            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Education" : "Educacion") : "Education",
                Icon = "book",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Health" : "Salud") : "Health",
                Icon = "healing",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Salary" : "Salario") : "Salary",
                Icon = "attach_money",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Bond" : "Bono") : "Bond",
                Icon = "local_atm",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Services" : "Servicios") : "Services",
                Icon = "credit_card",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Savings" : "Ahorros") : "Savings",
                Icon = "account_balance_wallet",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Transfers" : "Transferencia") : "Transfers",
                Icon = "compare_arrows",
                IsActive = true,
                IsDefault = true
            },

            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Donations" : "Donaciones") : "Donations",
                Icon = "child_care",
                IsActive = true,
                IsDefault = true
            },
            new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Extraction" : "Extraccion") : "Extraction",
                Icon = "autorenew",
                IsActive = true,
                IsDefault = true
            },
             new Category
            {
                Name = culture != null ? (culture.IsEnglish() ? "Deposit" : "Deposito") : "Deposit",
                Icon = "input",
                IsActive = true,
                IsDefault = true
            },
        };
    }
}
