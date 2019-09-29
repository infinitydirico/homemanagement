using HomeManagement.App.Data.Entities;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class TransactionTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool) return (bool)value ? Color.FromHex("#388E3C") : Color.FromHex("#ef5350");

            if(value is TransactionType) return (TransactionType)value == TransactionType.Income ? Color.FromHex("#388E3C") : Color.FromHex("#ef5350");

            throw new InvalidCastException($"Unable to cast to bool or {nameof(TransactionType)}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
