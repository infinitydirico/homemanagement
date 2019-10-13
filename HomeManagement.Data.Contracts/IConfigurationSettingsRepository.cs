using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public interface IConfigurationSettingsRepository : IRepository<ConfigurationSetting>
    {
        string GetValue(string name);

        bool Exists(string name);
    }
}
