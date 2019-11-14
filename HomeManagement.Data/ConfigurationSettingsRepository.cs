using HomeManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeManagement.Data
{
    public class ConfigurationSettingsRepository : BaseRepository<ConfigurationSetting>, IConfigurationSettingsRepository
    {
        public ConfigurationSettingsRepository(DbContext context)
            : base(context)
        {

        }
        public override bool Exists(ConfigurationSetting entity)
            => context.Set<ConfigurationSetting>().FirstOrDefault(x => x.Id.Equals(entity.Id)) != null;

        public override ConfigurationSetting GetById(int id)
            => context.Set<ConfigurationSetting>().FirstOrDefault(x => x.Id.Equals(id));

        public string GetValue(string name)
        {
            var entity = FirstOrDefault(x => x.Name.Equals(name));

            return entity.Value;
        }

        public bool Exists(string name) 
            => context.Set<ConfigurationSetting>().FirstOrDefault(x => x.Name.Equals(name)) != null;
    }
}
