using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using HomeManagement.Api.Core;
using HomeManagement.Api.Identity.Filters;
using HomeManagement.Api.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HomeManagement.API.Storage.Controllers
{
    [Authorization]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var objs = await GetObjects();
            return Ok(objs);
        }

        [HttpGet("{tag}")]
        public async Task<IActionResult> Get(string tag)
        {
            var client = new AmazonS3Client(key, secret, new AmazonS3Config
            {
                ServiceURL = url
            });

            var objs = await GetObjects();
            var spaceObject = objs.FirstOrDefault(x => x.ETag.Equals($"\"{tag}\""));

            if (spaceObject == null) return BadRequest();

            var transferUtility = new TransferUtility(client);

            var fileName = spaceObject.Key.Split("/", System.StringSplitOptions.RemoveEmptyEntries).Last();
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

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            if (!ModelState.IsValid) return BadRequest("Invalid body.");

            var client = new AmazonS3Client(key, secret, new AmazonS3Config
            {
                ServiceURL = url
            });

            var file = Request.Form.Files.FirstOrDefault();

            var app = TokenFactory.GetAppName(HttpContext.User.Claims);

            await client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucket,
                Key = $"{HttpContext.User.Identity.Name}/{app}/{file.FileName}",
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType
            });

            ClearCachedItems();

            return Ok();
        }

        [HttpDelete("{tag}")]
        public async Task<IActionResult> Delete(string tag)
        {
            var objs = await GetObjects();
            var client = new AmazonS3Client(key, secret, new AmazonS3Config
            {
                ServiceURL = url
            });

            var spaceObject = objs.FirstOrDefault(x => x.ETag.Equals($"\"{tag}\""));

            if (spaceObject == null) return BadRequest();

            var result = await client.DeleteObjectAsync(bucket, spaceObject.Key);

            ClearCachedItems();

            return Ok();
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
                memoryCache.Set(bucket, items);
            }            

            return items.S3Objects.Where(x => x.Key.Contains(HttpContext.User.Identity.Name)).ToList();
        }

        private void ClearCachedItems()
        {
            memoryCache.Remove(bucket);
        }
    }
}