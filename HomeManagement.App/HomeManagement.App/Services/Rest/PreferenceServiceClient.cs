using Newtonsoft.Json;
using System.Threading.Tasks;
using static HomeManagement.App.Common.Constants;

namespace HomeManagement.App.Services.Rest
{
    public class PreferenceServiceClient
    {
        BaseRestClient restClient;

        public PreferenceServiceClient()
        {
            restClient = new BaseRestClient(Endpoints.BASEURL);
        }

        public async Task<bool> GetEnableBackups()
        {
            var definition = new { EnableDailyBackups = false };

            using(var client = await restClient.CreateAuthenticatedClient())
            {
                var result = await client.GetAsync(Endpoints.Preference.URL);
                var json = await result.Content.ReadAsStringAsync();

                var content = JsonConvert.DeserializeAnonymousType(json, definition);
                return content.EnableDailyBackups;
            }
        }

        public async Task UpdateDailyBackups(bool value)
        {
            var api = $"{Endpoints.Preference.URL}/{value.ToString()}";
            using (var client = await restClient.CreateAuthenticatedClient())
            {
                var result = await client.PostAsync($"{Endpoints.Preference.URL}/{value.ToString()}", null);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
