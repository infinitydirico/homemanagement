using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public class StorageManager : IStorageManager
    {
        StorageRestClient client = new StorageRestClient();

        public async Task<IEnumerable<StorageFileModel>> Get()
        {
            return await client.Get();
        }
    }

    public interface IStorageManager
    {
        Task<IEnumerable<StorageFileModel>> Get();
    }
}