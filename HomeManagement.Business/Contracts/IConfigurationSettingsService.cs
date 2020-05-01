using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface IConfigurationSettingsService
    {
        IEnumerable<ConfigurationSettingModel> GetConfigs();

        ConfigurationSettingModel GetConfig(string name);

        OperationResult Save(ConfigurationSettingModel model);

        OperationResult Delete(int id);
    }
}
