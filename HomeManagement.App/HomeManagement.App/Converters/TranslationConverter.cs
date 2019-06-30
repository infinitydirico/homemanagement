using Autofac;
using HomeManagement.App.Managers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class TranslationConverter : IValueConverter
    { 
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) throw new ArgumentException($"the parameter {nameof(value)} cannot be null or empty");

            var translation = localizationManager.Translate(value.ToString());

            return translation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
