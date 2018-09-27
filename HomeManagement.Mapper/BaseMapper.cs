using HomeManagement.Contracts.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public abstract class BaseMapper<TEntity, TModel> : IMapper<TEntity, TModel>
        where TEntity : class, new()
        where TModel : class, new()
    {
        public virtual TEntity ToEntity(TModel model) => ConvertTo<TEntity>(model);

        public virtual TModel ToModel(TEntity entity) => ConvertTo<TModel>(entity);

        private TTarget ConvertTo<TTarget>(object source)
            where TTarget : class, new()
        {
            var properties = (from entityProperty in GetEntityProperties().ToList()
                              join modelProperty in GetModelProperties().ToList()
                              on entityProperty.Name equals modelProperty.Name
                              where entityProperty.PropertyType.Equals(modelProperty.PropertyType)
                              select new { entityProperty, modelProperty }).ToList();

            var target = new TTarget();

            foreach (var property in properties)
            {
                property.modelProperty.SetValue(target, property.entityProperty.GetValue(source));
            }

            return target;
        }

        public abstract IEnumerable<PropertyInfo> GetEntityProperties();

        public abstract IEnumerable<PropertyInfo> GetModelProperties();
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
