using HomeManagement.Models;
using System;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class AuthServiceClient
    {
        BaseRestClient restClient;

        public AuthServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.IDENTITY_API);
        }

        public async Task<UserModel> Login(UserModel user)
        {
            try
            {
                using(var client = restClient.CreateClient())
                {
                    var header = Xamarin.Essentials.Preferences.Get("HomeManagementAppHeader", string.Empty);
                    client.DefaultRequestHeaders.Add("HomeManagementApp", header);
                    var result = await client.PostAsync(Endpoints.Auth.LOGIN, restClient.SerializeToJson(user));
                    var json = await restClient.ReadJsonResponse<UserModel>(result);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public async Task Logout(UserModel user)
        {
            using (var client = await restClient.CreateAuthenticatedClient())
            {
                var result = await client.PostAsync(Endpoints.Auth.LOGOUT, restClient.SerializeToJson(user));
                user = await restClient.ReadJsonResponse<UserModel>(result);
            }
        }

        public async Task RegisterAsync(UserModel user)
        {
            using (var client = restClient.CreateClient())
            {
                var result = await client.PostAsync(Endpoints.Auth.REGISTER, restClient.SerializeToJson(user));
                result.EnsureSuccessStatusCode();
            }
        }

        public async Task<UserCodeModel> GetCode()
        {
            try
            {
                using (var client = await restClient.CreateAuthenticatedClient())
                {
                    var header = Xamarin.Essentials.Preferences.Get("HomeManagementAppHeader", string.Empty);
                    client.DefaultRequestHeaders.Add("HomeManagementApp", header);
                    var result = await client.GetAsync(Endpoints.Auth.SECURITY_CODE);
                    var json = await restClient.ReadJsonResponse<UserCodeModel>(result);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }
    }
}
