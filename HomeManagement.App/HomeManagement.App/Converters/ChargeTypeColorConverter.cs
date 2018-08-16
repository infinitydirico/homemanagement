using HomeManagement.Domain;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class ChargeTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool) return (bool)value ? Color.FromHex("#388E3C") : Color.FromHex("#ef5350");

            if(value is ChargeType) return (ChargeType)value == ChargeType.Income ? Color.FromHex("#388E3C") : Color.FromHex("#ef5350");

            throw new InvalidCastException("Unable to cast to bool or ChargeType");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
