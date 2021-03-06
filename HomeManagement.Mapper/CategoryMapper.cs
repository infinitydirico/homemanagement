﻿using System.Collections.Generic;
using System.Reflection;
using HomeManagement.Domain;
using HomeManagement.Models;

namespace HomeManagement.Mapper
{
    public class CategoryMapper : BaseMapper<Category, CategoryModel>, ICategoryMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Category).GetProperty(nameof(Category.Id));
            yield return typeof(Category).GetProperty(nameof(Category.Name));
            yield return typeof(Category).GetProperty(nameof(Category.IsActive));
            yield return typeof(Category).GetProperty(nameof(Category.Icon));
            yield return typeof(Category).GetProperty(nameof(Category.Measurable));
            yield return typeof(Category).GetProperty(nameof(Category.UserId));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.Id));
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.Name));
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.IsActive));
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.Icon));
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.Measurable));
            yield return typeof(CategoryModel).GetProperty(nameof(CategoryModel.UserId));
        }
    }
}
