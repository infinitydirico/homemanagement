using HomeManagement.App.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class PreferenceServiceClient : IPreferenceServiceClient
    {
        public async Task<bool> GetEnableBackups()
        {
            var definition = new { EnableDailyBackups = false };

            var result = await RestClientFactory
                .CreateAuthenticatedClient()
                .GetAsync(Constants.Endpoints.Preference.URL);

            var json = await result.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeAnonymousType(json, definition);
            return content.EnableDailyBackups;
        }

        public async Task UpdateDailyBackups(bool value)
        {
            await RestClientFactory
                .CreateAuthenticatedClient()
                .PostAsync($"{Constants.Endpoints.Preference.URL}/{value.ToString()}", null);
        }
    }

    public interface IPreferenceServiceClient
    {
        Task UpdateDailyBackups(bool value);

        Task<bool> GetEnableBackups();
    }
}
