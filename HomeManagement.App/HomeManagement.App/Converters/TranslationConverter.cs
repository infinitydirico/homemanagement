using Autofac;
using HomeManagement.Localization;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class TranslationConverter : IValueConverter
    {
        ILocalization localization = App._container.Resolve<ILocalization>();
        const string ResourceId = "HomeManagement.Localization.Resource";
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(ILocalization)).Assembly));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) throw new ArgumentException($"the parameter {nameof(value)} cannot be null or empty");

            var translation = ResMgr.Value.GetString(value.ToString(), localization.GetCurrentCulture());

            return translation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
