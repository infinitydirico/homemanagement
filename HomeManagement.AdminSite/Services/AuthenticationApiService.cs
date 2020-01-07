using HomeManagement.Contracts;
using HomeManagement.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeManagement.AdminSite.Services
{
    public class AuthenticationApiService : IAuthenticationService, IApiService
    {
        private readonly ICryptography cryptography;

        public AuthenticationApiService(ICryptography cryptography,
            IConfiguration configuration)
        {
            this.cryptography = cryptography;
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public async Task<UserModel> Login(UserModel model)
        {
            using (var client = this.CreateIdentityClient())
            {
                model.Password = cryptography.Encrypt(model.Password);

                var response = await client.PostAsync("api/Authentication/SignIn", model, new System.Net.Http.Formatting.JsonMediaTypeFormatter());

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<UserModel>(result);

                return data;
            }
        }

        public string GetApiEndpoint() => this.GetEndpoint();
    }

    public interface IAuthenticationService
    {
        Task<UserModel> Login(UserModel model);
    }
}
