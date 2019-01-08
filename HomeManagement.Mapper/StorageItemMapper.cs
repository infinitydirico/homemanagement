using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public interface IStorageItemMapper : IMapper<StorageItem, StorageItemModel>
    {

    }

    public class StorageItemMapper : BaseMapper<StorageItem, StorageItemModel>, IStorageItemMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.Id));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.Name));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.ChargeId));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.IsFolder));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.Name));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.Path));
            yield return typeof(StorageItem).GetProperty(nameof(StorageItem.Size));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.Id));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.Name));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.ChargeId));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.IsFolder));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.Name));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.Path));
            yield return typeof(StorageItemModel).GetProperty(nameof(StorageItemModel.Size));
        }
    }
}
