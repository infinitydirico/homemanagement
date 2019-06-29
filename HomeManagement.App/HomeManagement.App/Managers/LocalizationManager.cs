using Autofac;
using HomeManagement.Localization;
using System;
using System.Reflection;
using System.Resources;

namespace HomeManagement.App.Managers
{
    public interface ILocalizationManager
    {
        string Translate(string text);
    }

    public class LocalizationManager : ILocalizationManager
    {
        ILocalization localization = App._container.Resolve<ILocalization>();
        const string ResourceId = "HomeManagement.Localization.Resource";
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(ILocalization)).Assembly));

        public string Translate(string text)
        {
            return ResMgr.Value.GetString(text, localization.GetCurrentCulture());
        }
    }
}
