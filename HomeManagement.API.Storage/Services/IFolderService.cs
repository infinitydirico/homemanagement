using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeManagement.Api.Core;
using HomeManagement.Models.Storage;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.API.Storage.Services
{
    public interface IFolderService
    {
        FolderModel GetRoot(HomeManagementPrincipal principal);
        IEnumerable<FolderModel> GetDirectory(string path);
        void Add(string path, string name);
        void Remove(string path);
        bool IsOutsideUserScope(HomeManagementPrincipal principal, string path);
    }

    public class FolderService : IFolderService
    {
        private readonly string bucket;

        public FolderService(IConfiguration configuration)
        {
            var spacesConfig = configuration.GetSection("DO").GetSection("spaces");
            bucket = spacesConfig.GetValue<string>("bucket");
        }

        public FolderModel GetRoot(HomeManagementPrincipal principal)
        {
            var path = $@"{Directory.GetCurrentDirectory()}/{bucket}/{principal.Name}";

            var root = new DirectoryInfo(path);

            return new FolderModel
            {
                Name = root.Name,
                Path = root.FullName,
                Folders = root
                    .GetDirectories("*.*", SearchOption.TopDirectoryOnly)
                    .Select(x => new FolderModel
                    {
                        Name = x.Name,
                        Path = x.FullName
                    })
                    .ToList()
            };
        }

        public IEnumerable<FolderModel> GetDirectory(string path)
        {
            var directory = new DirectoryInfo(path);

            var children = directory
                    .GetDirectories("*.*", SearchOption.TopDirectoryOnly)
                    .Select(x => new FolderModel
                    {
                        Name = x.Name,
                        Path = x.FullName
                    })
                    .ToList();

            return children;
        }

        public void Add(string path, string name)
        {
            var directory = new DirectoryInfo(path);
            var newDirectory = directory.CreateSubdirectory(name);
        }

        public void Remove(string path)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists) directory.Delete(false);
        }

        public bool IsOutsideUserScope(HomeManagementPrincipal principal, string path)
        {
            var rootPath = $@"{Directory.GetCurrentDirectory()}/{bucket}/{principal.Name}";

            var root = new DirectoryInfo(rootPath);

            var pathDirectory = new DirectoryInfo(path);

            return pathDirectory.Equals(root);
        }
    }
}
