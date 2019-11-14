using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public class StorageItemRepository : BaseRepository<StorageItem>, IStorageItemRepository
    {
        public StorageItemRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(StorageItem entity) => GetById(entity.Id) != null;

        public override StorageItem GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
