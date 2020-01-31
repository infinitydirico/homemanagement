using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Stream> Get(string tag)
        {
            var result = await client.Get(tag);

            return result;
        }
    }

    public interface IStorageManager
    {
        Task<IEnumerable<StorageFileModel>> Get();

        Task<Stream> Get(string tag);
    }
}