using HomeManagement.Contracts.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Mapper
{
    public class BaseMapper<TEntity, TModel> : IMapper<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        public virtual TEntity ToEntity(TModel model)
        {
            var properties = model.GetType().GetProperties().Select(x => x.GetValue(model)).ToArray();
            return (TEntity)Activator.CreateInstance(typeof(TEntity), properties);
        }

        public virtual TModel ToModel(TEntity entity)
        {
            var properties = entity.GetType().GetProperties().Where(x => x.PropertyType.IsPrimitive || x.PropertyType == typeof(string)).Select(x => x.GetValue(entity)).ToArray();
            return (TModel)Activator.CreateInstance(typeof(TModel), properties);
        }
    }

    public static class MappingExtensions
    {
        public static IEnumerable<TEntity> ToEntities<TEntity, TModel>(this IMapper<TEntity, TModel> mapper, IEnumerable<TModel> models)
            where TEntity : class
            where TModel : class
        {
            return models == null ?
                Enumerable.Empty<TEntity>() :
                (from model in models select mapper.ToEntity(model)).ToList();
        }

        public static IEnumerable<TModel> ToModels<TEntity, TModel>(this IMapper<TEntity, TModel> mapper, IEnumerable<TEntity> entities)
            where TEntity : class
            where TModel : class
        {
            return entities == null ?
                Enumerable.Empty<TModel>() :
                (from entity in entities select mapper.ToModel(entity)).ToList();
        }
    }
}
