using HomeManagement.App.Common;
using System;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest.Identity
{
    public class TwoFactorServiceClient
    {
        BaseRestClient restClient;

        public TwoFactorServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<bool> IsEnabled()
        {
            try
            {
                using (var client = await restClient.CreateAuthenticatedClient())
                {
                    var header = Xamarin.Essentials.Preferences.Get("HomeManagementAppHeader", string.Empty);
                    client.DefaultRequestHeaders.Add("HomeManagementApp", header);
                    var result = await client.GetAsync(Endpoints.TwoFactor.IS_ENABLED);

                    if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new AppException();

                    var json = await restClient.ReadJsonResponse<bool>(result);
                    return json;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public async Task Enable()
        {
            try
            {
                using (var client = await restClient.CreateAuthenticatedClient())
                {
                    var header = Xamarin.Essentials.Preferences.Get("HomeManagementAppHeader", string.Empty);
                    client.DefaultRequestHeaders.Add("HomeManagementApp", header);
                    var result = await client.PostAsync(Endpoints.TwoFactor.ENABLE, null);

                    if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new AppException();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        public async Task Disable()
        {
            try
            {
                using (var client = await restClient.CreateAuthenticatedClient())
                {
                    var header = Xamarin.Essentials.Preferences.Get("HomeManagementAppHeader", string.Empty);
                    client.DefaultRequestHeaders.Add("HomeManagementApp", header);
                    var result = await client.PostAsync(Endpoints.TwoFactor.DISABLE, null);

                    if (result.StatusCode != System.Net.HttpStatusCode.OK) throw new AppException();
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
