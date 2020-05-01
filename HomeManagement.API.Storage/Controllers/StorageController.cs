using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HomeManagement.Core.Extensions;
using HomeManagement.Api.Core;
using HomeManagement.Api.Core.Extensions;
using HomeManagement.Api.Identity.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeManagement.Models;
using Microsoft.AspNetCore.Cors;

namespace HomeManagement.API.Storage.Controllers
{
    [EnableCors("StorageApiCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly string bucket;
        private readonly string key;
        private readonly string secret;
        private readonly string url;

        public StorageController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            var spacesConfig = this.configuration.GetSection("DO").GetSection("spaces");
            bucket = spacesConfig.GetValue<string>("bucket");
            key = spacesConfig.GetValue<string>("key");
            secret = spacesConfig.GetValue<string>("secret");
            url = spacesConfig.GetValue<string>("url");
        }

        [Authorization]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var objs = await GetObjects();
            return Ok(objs.Select(x => new StorageFileModel
            {
                Name = x.Key.Split('/').Last(),
                Key = x.Key,
                Tag = x.ETag,
                LastModified = x.LastModified,
                Size = x.Size
            }));
        }

        [Authorization]
        [HttpGet("{tag}")]
        public async Task<IActionResult> Get(string tag)
        {
            var client = new AmazonS3Client(key, secret, new AmazonS3Config
            {
                ServiceURL = url
            });

            var objs = await GetObjects();
            var spaceObject = objs.FirstOrDefault(x => x.ETag.Equals($"{tag}"));

            if (spaceObject == null) return BadRequest();

            var transferUtility = new TransferUtility(client);

            var fileName = spaceObject.Key.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
            var directory = Directory.GetCurrentDirectory();
            var filePath = $@"{directory}\temporary\{fileName}";

            if (!System.IO.File.Exists(filePath))
            {
                await transferUtility.DownloadAsync(new TransferUtilityDownloadRequest
                {
                    BucketName = bucket,
                    Key = spaceObject.Key,
                    FilePath = filePath
                });
            }

            return PhysicalFile(filePath, filePath.GetMimeType(), fileName);
        }

        [Authorization]
        [HttpPut]
        public async Task<IActionResult> Put()
        {
            await SendFile(HttpContext.User.Identity.Name,
                TokenFactory.GetAppName(HttpContext.User.Claims),
                HttpContext.Request.Headers["Path"].FirstOrDefault());

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Send()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Username") &&
                !HttpContext.Request.Headers.ContainsKey("AppName")) return BadRequest("Missing headers.");

            var username = HttpContext.Request.Headers["Username"].First();

            var appName = HttpContext.Request.Headers["AppName"].First();

            var path = HttpContext.Request.Headers["Path"].FirstOrDefault();

            await SendFile(username, appName, path);

            return Ok();
        }

        [Authorization]
        [HttpDelete("{tag}")]
        public async Task<IActionResult> Delete(string tag)
        {
            var objs = await GetObjects();
            var client = new AmazonS3Client(key, secret, new AmazonS3Config
            {
                ServiceURL = url
            });

            var spaceObject = objs.FirstOrDefault(x => x.ETag.Equals($"{tag}"));

            if (spaceObject == null) return BadRequest();

            var result = await client.DeleteObjectAsync(bucket, spaceObject.Key);

            ClearCachedItems();

            return Ok();
        }

        private async Task SendFile(string username, string app, string path = "")
        {
            foreach (var file in Request.Form.Files)
            {
                var client = new AmazonS3Client(key, secret, new AmazonS3Config
                {
                    ServiceURL = url
                });

                var fileKey = path.IsEmpty() ?
                    $"{username}/{app}/{file.FileName}" :
                    $"{username}/{app}/{path}/{file.FileName}";
                
                await client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = fileKey,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType
                });
            }

            ClearCachedItems();
        }

        private async Task<IEnumerable<S3Object>> GetObjects()
        {
            var items = memoryCache.Get<ListObjectsResponse>(bucket);
            if (items == null)
            {
                var client = new AmazonS3Client(key, secret, new AmazonS3Config
                {
                    ServiceURL = url
                });

                items = await client.ListObjectsAsync(new ListObjectsRequest
                {
                    BucketName = bucket,
                    Prefix = HttpContext.User.Identity.Name
                });

                memoryCache.CreateEntry(bucket);
                memoryCache.Set(bucket, items, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }            

            return items
                .S3Objects
                .Where(x => x.Key.Contains(HttpContext.User.Identity.Name))
                .Select(x => new S3Object
                {
                    BucketName = x.BucketName,
                    ETag = x.ETag.Replace("\"", string.Empty),
                    Key = x.Key,
                    LastModified = x.LastModified,
                    Owner = x.Owner,
                    Size = x.Size
                })
                .ToList();
        }

        private void ClearCachedItems()
        {
            memoryCache.Remove(bucket);
        }
    }
}