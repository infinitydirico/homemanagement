using HomeManagement.App.Data.Entities;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class ChargeTypeLabelConverter : IValueConverter
    {
        TranslationConverter translationConverter = new TranslationConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (bool)value ? TransactionType.Income.ToString() : TransactionType.Expense.ToString();
            var translatedText = translationConverter.Convert(text, targetType, null, CultureInfo.CurrentCulture);
            return translatedText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
