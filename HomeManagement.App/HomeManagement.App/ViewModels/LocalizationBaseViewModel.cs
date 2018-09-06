using Autofac;
using HomeManagement.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HomeManagement.App.ViewModels
{
    public class LocalizationBaseViewModel : BaseViewModel
    {
        protected readonly ILocalization localization;

        public LocalizationBaseViewModel()
        {
            localization = App._container.Resolve<ILocalization>();
            localization.OnCultureChanged += LocalizationBaseViewModel_OnCultureChanged; ;
        }

        public string CurrentLanguage { get; set; } = "Language";

        protected virtual void LocalizationBaseViewModel_OnCultureChanged(object sender, CultureChangeEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentLanguage));
        }

    }
}
