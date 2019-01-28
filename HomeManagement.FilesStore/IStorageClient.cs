using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HomeManagement.FilesStore
{
    public interface IStorageClient
    {
        Task<IEnumerable<StorageItem>> Get(int userId);

        bool IsAuthorized(int userId);

        Task Authorize(int userId, string code, string state);

        Uri GetAccessToken(int userId);

        Task<StorageItem> Upload(int userId, string filename, Stream stream);

        Task<StorageItem> Upload(int userId, string filename,string accountName, string chargeName, Stream stream);

        Task<Stream> Download(int userId, string path);
    }
}
