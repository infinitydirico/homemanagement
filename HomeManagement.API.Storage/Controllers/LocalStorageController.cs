using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomeManagement.Api.Core;
using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.API.Storage.Services;
using HomeManagement.Core.Extensions;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using FileManager = System.IO.File;

namespace HomeManagement.API.Storage.Controllers
{
    [EnableCors("StorageApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LocalStorageController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly IUserFilesService userFilesService;
        private readonly string bucket;

        public LocalStorageController(IConfiguration configuration, 
            IMemoryCache memoryCache,
            IUserFilesService userFilesService)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.userFilesService = userFilesService;
            var spacesConfig = this.configuration.GetSection("DO").GetSection("spaces");
            bucket = spacesConfig.GetValue<string>("bucket");
        }

        private HomeManagementPrincipal Principal => User as HomeManagementPrincipal;

        [Authorization]
        [HttpGet]
        public IActionResult Get() => Ok(userFilesService.GetDirectories($@"{Directory.GetCurrentDirectory()}/{bucket}/{Principal.Name}"));

        [HttpGet("getall")]
        [Authorization(Constants.Roles.Admininistrator)]
        public IActionResult GetAll() => Ok(GetObjects());

        [Authorization]
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            var objs = GetObjects();
            var file = objs.FirstOrDefault(x => x.Name.Contains(filename));

            if (file == null) return BadRequest();

            return PhysicalFile(file.Key, file.Key.GetMimeType(), file.Name);
        }

        [Authorization]
        [HttpPost("path/{path}/folder/{folder}")]
        public IActionResult Post(string path, string folder)
        {
            var slash = Core.Extensions.String.GetOsSlash();

            var root = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{slash}{bucket}{slash}{Principal.Name}");

            var directory = root
                .EnumerateFileSystemInfos("*.*", SearchOption.AllDirectories)
                .FirstOrDefault(x => x.Name.Equals(path));

            var newFolder = $"{directory.FullName}{slash}{folder}";

            if (!Directory.Exists(newFolder)) Directory.CreateDirectory(newFolder);

            return Ok();
        }

        [Authorization]
        [HttpPut("{path}")]
        public IActionResult Put(string path)
        {
            SendFile(Principal.Name,
                Principal.ApplicationName,
                path);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Send()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Username") &&
                !HttpContext.Request.Headers.ContainsKey("AppName")) return BadRequest("Missing headers.");

            var username = HttpContext.Request.Headers["Username"].First();

            var appName = HttpContext.Request.Headers["AppName"].First();

            var path = HttpContext.Request.Headers["Path"].FirstOrDefault();

            SendFile(username, appName, path);

            return Ok();
        }

        [Authorization(Constants.Roles.RegularUser, Constants.Roles.Admininistrator)]
        [HttpDelete("{filename}")]
        public IActionResult Delete(string filename)
        {
            var model = userFilesService.Find($@"{Directory.GetCurrentDirectory()}/{bucket}/{Principal.Name}", filename);

            if (model == null) return BadRequest();

            if (model.IsDirectory) Directory.Delete(model.Path);
            else FileManager.Delete(model.Name);

            ClearCachedItems();

            return Ok();
        }

        private void SendFile(string username, string app, string path = "")
        {
            foreach (var file in Request.Form.Files)
            {
                var slash = Core.Extensions.String.GetOsSlash();
                var p = path.IsEmpty() ?
                    $"{Directory.GetCurrentDirectory()}{slash}{bucket}{slash}{username}{slash}{app}" :
                    $"{Directory.GetCurrentDirectory()}{slash}{bucket}{slash}{username}{slash}{app}{slash}{path}";

                if (!Directory.Exists(p)) Directory.CreateDirectory(p);

                var fileKey = $"{p}/{file.FileName}";

                if (!FileManager.Exists(fileKey))
                {
                    using (var stream = FileManager.Create(fileKey))
                    {
                        file.CopyTo(stream);
                    }                    
                }
            }

            ClearCachedItems();
        }

        private IEnumerable<StorageFileModel> GetUserFiles()
        {
            return ListFiles($@"{Directory.GetCurrentDirectory()}/{bucket}/{Principal.Name}");
        }

        private IEnumerable<StorageFileModel> GetObjects()
        {
            var items = memoryCache.Get<IEnumerable<StorageFileModel>>(bucket);
            if (items == null)
            {
                items = ListFiles($"{Directory.GetCurrentDirectory()}/{bucket}");

                memoryCache.CreateEntry(bucket);
                memoryCache.Set(bucket, items, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return items;
        }

        private IEnumerable<StorageFileModel> ListFiles(string path)
        {
            var all = Directory.EnumerateFileSystemEntries(path, "*.*", SearchOption.AllDirectories);

            return all.Select(x =>
            {
                var directory = new DirectoryInfo(x);

                if (directory.Attributes.Equals(FileAttributes.Directory))
                {
                    return new StorageFileModel
                    {
                        Name = directory.Name,
                        Key = directory.FullName,
                        IsDirectory = true
                    };
                }
                else
                {
                    var fileInfo = new FileInfo(x);
                    return new StorageFileModel
                    {
                        Name = fileInfo.Name,
                        Key = fileInfo.FullName,
                        Tag = fileInfo.FullName,
                        LastModified = fileInfo.LastAccessTimeUtc,
                        Size = fileInfo.Length,
                        IsDirectory = false
                    };
                }
            });
        }

        private void ClearCachedItems()
        {
            memoryCache.Remove(bucket);
        }
    }
}