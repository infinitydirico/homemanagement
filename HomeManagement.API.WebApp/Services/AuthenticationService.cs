using HomeManagement.Core.Cryptography;
using HomeManagement.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.API.WebApp.Services
{
    public class AuthenticationService
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<AuthenticationService> logger;
        public AuthenticationService(IConfiguration configuration,
            IMemoryCache memoryCache,
            ILogger<AuthenticationService> logger)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public async Task<bool> SignIn(string username, string password)
        {
            try
            {
                var endpoint = configuration.GetSection("Endpoints").GetSection("Identity").Value;
                var encrypter = new AesCryptographyService();
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(endpoint);
                    var content = Json.CreateJsonContent(new UserModel
                    {
                        Email = username,
                        Password = encrypter.Encrypt(password)
                    });

                    var response = await httpClient.PostAsync("/api/authentication/signin", content);
                    var result = await response.Content.ReadAsStringAsync();

                    response.EnsureSuccessStatusCode();

                    var model = Json.Deserialize<UserModel>(result);

                    memoryCache.CreateEntry(model.Email);
                    memoryCache.Set(model.Email, model, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(60)
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to authenticate {username}");
                return false;
            }
        }

        public bool IsAuthenticated()
        {
            return true;
        }
    }
}
