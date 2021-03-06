﻿using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Data.Repositories
{
    public class CategoryRepository : HomeManagement.Data.CategoryRepository
    {
        private readonly ICategoryMapper categoryMapper;

        public CategoryRepository(IPlatformContext platformContext) 
            : base(platformContext.CreateDbContext())
        {
        }

        public OverPricedCategories GetMostOverPricedCategories(TransactionType transactionType)
        {
            var query = (from category in context.Set<Category>()
                         join transaction in context.Set<Transaction>()
                         on category.Id equals transaction.CategoryId
                         where transaction.TransactionType == transactionType
                         group category by category.Id into g
                         select new OverPricedCategory
                         {
                             Category = categoryMapper.ToModel(context.Set<Category>().FirstOrDefault(c => c.Id.Equals(g.Key))),
                             Price = context.Set<Transaction>().Where(c => c.CategoryId.Equals(g.Select(s => s.Id).First()) && c.TransactionType == transactionType && c.Date.Month.Equals(DateTime.Now.Month))
                                                    .Sum(o => o.Price)
                         }).ToList();

            query = query.OrderByDescending(x => x.Price).Take(5).ToList();

            if (query.Count.Equals(default(int))) return null;

            var categories = new OverPricedCategories();

            foreach (var item in query.OrderBy(x => x.Category.Name))
            {
                item.Price = item.Price / 1000;
                categories.Categories.Add(item);
            }

            categories.HighestValue = query.Max(c => c.Price) + (query.Max(c => c.Price) / 4);
            categories.LowestValue = query.Min(c => c.Price);

            return categories;

        }

        public IEnumerable<Category> GetUserCategories(int userId)
        {
            var query = (from c in context.Set<Category>()
                         where c.UserId.Equals(userId)
                         select c);

            return query.ToList();

        }

        public IEnumerable<Category> GetUserActiveCategories(int userId)
        {
            var query = (from c in context.Set<Category>()
                         where c.UserId.Equals(userId) && c.IsActive
                         select c);

            return query.ToList();
        }
    }
}
