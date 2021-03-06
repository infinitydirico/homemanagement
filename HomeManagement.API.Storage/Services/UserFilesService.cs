﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeManagement.Models.Storage;
using Microsoft.Extensions.Configuration;

namespace HomeManagement.API.Storage.Services
{
    public interface IUserFilesService
    {
        IEnumerable<DirectoryModel> GetDirectories(string path);

		DirectoryModel Find(string rootPath, string name);
	}

    public class UserFilesService : IUserFilesService
    {
        private readonly string bucket;
        DirectoryInfo root;

        public UserFilesService(IConfiguration configuration)
        {
            var spacesConfig = configuration.GetSection("DO").GetSection("spaces");
            bucket = spacesConfig.GetValue<string>("bucket");
        }

        public DirectoryModel Find(string rootPath, string name)
        {
			root = new DirectoryInfo(rootPath);

			var found = DeepFind(root, name);

			return found;
		}

        public IEnumerable<DirectoryModel> GetDirectories(string path)
        {
			root = new DirectoryInfo(path);

			return GetAll();
		}

		IEnumerable<DirectoryModel> GetAll()
		{
			var dirs = root.EnumerateFileSystemInfos("*.*", SearchOption.TopDirectoryOnly);

			return dirs.Select(x =>
			{
				var model = new DirectoryModel
				{
					Name = x.Name,
					Path = x.FullName,
					IsDirectory = x.Attributes.Equals(FileAttributes.Directory),
					Parent = GetParent(x),
				};

				if (x.Attributes.Equals(FileAttributes.Directory))
				{
					model.Children.AddRange(GetChildren(x));
				}

				return model;
			});
		}


		IEnumerable<DirectoryModel> GetChildren(FileSystemInfo info)
		{
			var infoDir = new DirectoryInfo(info.FullName);

			if (!infoDir.Attributes.Equals(FileAttributes.Directory)) return Enumerable.Empty<DirectoryModel>();

			var dirs = infoDir.EnumerateFileSystemInfos("*.*", SearchOption.TopDirectoryOnly);

			return dirs.Select(x =>
			{
				var isDirectory = x.Attributes.Equals(FileAttributes.Directory);

				var model = new DirectoryModel
				{
					Name = x.Name,
					Path = x.FullName,
					IsDirectory = isDirectory,
					Parent = GetParent(x)
				};

				if (isDirectory)
				{
					model.Children.AddRange(GetChildren(x));
				}

				return model;
			});
		}

		DirectoryModel GetParent(FileSystemInfo info)
		{
			var infoDir = new DirectoryInfo(info.FullName);
			var model = new DirectoryModel
			{
				Name = infoDir.Parent.Name,
				Path = infoDir.Parent.FullName,
				IsDirectory = true,
			};

			model.Children.AddRange(GetChildren(infoDir));

			return model;
		}

		DirectoryModel DeepFind(DirectoryInfo dir, string name)
        {
			if(dir.Name.ToLower().Equals(name.ToLower())) return new DirectoryModel
			{
				Name = dir.Name,
				Path = dir.FullName,
				IsDirectory = dir.Attributes.Equals(FileAttributes.Directory)
			};

			var children = dir.EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly);

            foreach (var child in children)
            {
				var found = DeepFind(child, name);
				if (found != null) return found;
            }

			return null;
		}
	}
}
