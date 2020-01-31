using System;
using System.Globalization;
using Xamarin.Forms;

namespace HomeManagement.App.Converters
{
    public class SizeConverter : IValueConverter
    {
        private const int Kb = 1024;
        private const int Mb = Kb * 1024;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = double.Parse(value.ToString());

            if ((size / Mb) % 2 > 1) return $"{(size / Mb).ToString("F2")} Mb";
            if ((size / Kb) % 2 > 1) return $"{(size / Kb).ToString("F2")} Kb";

            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => string.Empty;
    }
}
