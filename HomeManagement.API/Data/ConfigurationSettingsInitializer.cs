using System.Linq;

namespace HomeManagement.API.Data
{
    public static class ConfigurationSettingsInitializer
    {
        public static void SeedSettings(this WebAppDbContext context)
        {
            if (context.ConfigurationSettings.Count() > 0) return;

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "CurrencyService_API",
                Value = string.Empty
            });

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "CurrencyService_AppId",
                Value = string.Empty
            });

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "Dropbox_AppId",
                Value = string.Empty
            });

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "Dropbox_AppSecret",
                Value = string.Empty
            });

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "VisionApiKey",
                Value = string.Empty
            });

            context.ConfigurationSettings.Add(new Domain.ConfigurationSetting
            {
                Name = "VisionApiEndpoint",
                Value = string.Empty
            });

            context.SaveChanges();
        }
    }
}
