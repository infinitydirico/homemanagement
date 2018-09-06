using System;
using System.Globalization;

namespace HomeManagement.Localization
{
    public interface ILocalization
    {
        void ChangeCulture(CultureInfo cultureInfo);

        CultureInfo GetCurrentCulture();

        event EventHandler<CultureChangeEventArgs> OnCultureChanged;
    }
}
