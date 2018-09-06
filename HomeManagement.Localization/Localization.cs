using System;
using System.Globalization;

namespace HomeManagement.Localization
{
    public class LocalizationLanguage : ILocalization
    {
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
    }
}
