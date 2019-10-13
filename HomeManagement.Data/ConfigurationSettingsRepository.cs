using HomeManagement.Domain;
using System.Linq;

namespace HomeManagement.Data
{
    public class ConfigurationSettingsRepository : BaseRepository<ConfigurationSetting>, IConfigurationSettingsRepository
    {
        public ConfigurationSettingsRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(ConfigurationSetting entity)
            => platformContext.GetDbContext().Set<ConfigurationSetting>().FirstOrDefault(x => x.Id.Equals(entity.Id)) != null;

        public override ConfigurationSetting GetById(int id)
            => platformContext.GetDbContext().Set<ConfigurationSetting>().FirstOrDefault(x => x.Id.Equals(id));

        public string GetValue(string name)
        {
            var entity = FirstOrDefault(x => x.Name.Equals(name));

            return entity.Value;
        }

        public bool Exists(string name) 
            => platformContext.GetDbContext().Set<ConfigurationSetting>().FirstOrDefault(x => x.Name.Equals(name)) != null;
    }
}
