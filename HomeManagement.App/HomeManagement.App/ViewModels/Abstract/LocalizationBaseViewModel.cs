using Autofac;
using HomeManagement.Localization;
using System.Linq;

namespace HomeManagement.App.ViewModels
{
    public class LocalizationBaseViewModel : BaseViewModel
    {
        protected readonly ILocalization localization;

        public LocalizationBaseViewModel()
        {
            localization = App._container.Resolve<ILocalization>();
            localization.OnCultureChanged += LocalizationBaseViewModel_OnCultureChanged;
        }

        public string CurrentLanguage { get; set; } = "Language";

        protected virtual void LocalizationBaseViewModel_OnCultureChanged(object sender, CultureChangeEventArgs e)
        {
            foreach (var property in GetType().GetProperties().Where(x => x.Name.Contains("Text")))
            {
                OnPropertyChanged(property.Name);
            }
            OnPropertyChanged(nameof(CurrentLanguage));
        }
    }
}
