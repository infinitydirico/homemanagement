using System.Globalization;

namespace HomeManagement.API.Extensions
{
    public static class GlobalizationExtensions
    {
        public static bool IsEnglish(this CultureInfo culture) => culture.TwoLetterISOLanguageName.Equals("en");

        public static bool IsSpanish(this CultureInfo culture) => culture.TwoLetterISOLanguageName.Equals("es");
    }
}
