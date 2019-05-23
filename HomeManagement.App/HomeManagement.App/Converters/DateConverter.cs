using Autofac;
using HomeManagement.Localization;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class DateConverter : IValueConverter
    {
        private readonly ILocalization localization = App._container.Resolve<ILocalization>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;

            return date.ToString("dd MMM yyyy", localization.GetCurrentCulture());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
