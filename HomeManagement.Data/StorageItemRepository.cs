using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class StorageItemRepository : BaseRepository<StorageItem>, IStorageItemRepository
    {
        public StorageItemRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(StorageItem entity) => GetById(entity.Id) != null;

        public override StorageItem GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
