using System;
using System.Globalization;
using System.Linq;

namespace HomeManagement.Localization
{
    public class LocalizationLanguage : ILocalization
    {
        private static readonly string[] supportedLanguages = new string[2] { "en-US", "es" };

        private CultureInfo currentCulture;

        public LocalizationLanguage()
        {
            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        public event EventHandler<CultureChangeEventArgs> OnCultureChanged;

        public void ChangeCulture(CultureInfo cultureInfo)
        {
            currentCulture = cultureInfo;

            OnCultureChanged?.Invoke(this, new CultureChangeEventArgs
            {
                Culture = cultureInfo
            });
        }

        public CultureInfo GetCurrentCulture() => currentCulture;

        public static bool IsValid(string language) => supportedLanguages.Contains(language);
    }
}
