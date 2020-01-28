using HomeManagement.App.Common;
using System.Threading.Tasks;

namespace HomeManagement.App.Services.Rest
{
    public class PreferenceServiceClient : IPreferenceServiceClient
    {
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
    }
}
