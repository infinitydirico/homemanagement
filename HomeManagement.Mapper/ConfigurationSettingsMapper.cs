using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public class ConfigurationSettingsMapper : BaseMapper<ConfigurationSetting, ConfigurationSettingModel>, IConfigurationSettingsMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(ConfigurationSetting).GetProperty(nameof(ConfigurationSetting.Id));
            yield return typeof(ConfigurationSetting).GetProperty(nameof(ConfigurationSetting.Name));
            yield return typeof(ConfigurationSetting).GetProperty(nameof(ConfigurationSetting.Value));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(ConfigurationSettingModel).GetProperty(nameof(ConfigurationSettingModel.Id));
            yield return typeof(ConfigurationSettingModel).GetProperty(nameof(ConfigurationSettingModel.Name));
            yield return typeof(ConfigurationSettingModel).GetProperty(nameof(ConfigurationSettingModel.Value));
        }
    }

    public interface IConfigurationSettingsMapper : IMapper<ConfigurationSetting, ConfigurationSettingModel>
    {

    }
}
