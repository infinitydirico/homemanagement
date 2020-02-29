using HomeManagement.Api.Core;
using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Core.Extensions;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly string bucket;

        public LocalStorageController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            var spacesConfig = this.configuration.GetSection("DO").GetSection("spaces");
            bucket = spacesConfig.GetValue<string>("bucket");
        }

        [Authorization]
        [HttpGet]
        public IActionResult Get() => Ok(GetObjects());

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
        [HttpPut]
        public IActionResult Put()
        {
            SendFile(HttpContext.User.Identity.Name,
                TokenFactory.GetAppName(HttpContext.User.Claims),
                HttpContext.Request.Headers["Path"].FirstOrDefault());

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

        [Authorization]
        [HttpDelete("{filename}")]
        public IActionResult Delete(string filename)
        {
            var objs = GetObjects();

            var file = objs.FirstOrDefault(x => x.Name.Contains(filename));

            if (file == null) return BadRequest();

            FileManager.Delete(file.Key);

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

        private IEnumerable<StorageFileModel> GetObjects()
        {
            var items = memoryCache.Get<IEnumerable<StorageFileModel>>(bucket);
            if (items == null)
            {
                var files = Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}/{bucket}", "*.*", SearchOption.AllDirectories);

                items = files.Select(x =>
                {
                    var fileInfo = new FileInfo(x);
                    var model = new StorageFileModel
                    {
                        Name = fileInfo.Name,
                        Key = fileInfo.FullName,
                        Tag = fileInfo.FullName,
                        LastModified = fileInfo.LastAccessTimeUtc,
                        Size = fileInfo.Length
                    };

                    return model;
                });

                memoryCache.CreateEntry(bucket);
                memoryCache.Set(bucket, items, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return items;
        }

        private void ClearCachedItems()
        {
            memoryCache.Remove(bucket);
        }
    }
}