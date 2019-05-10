using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace HomeManagement.App.ViewModels
{
    public class SettingsViewModel : LocalizationBaseViewModel
    {
        CultureInfo[] cultures = new CultureInfo[] { new CultureInfo("es"), new CultureInfo("en") };

        public SettingsViewModel()
        {
            ChangeLanguageCommand = new Command(ChangeLanguage);
        }

        public ICommand ChangeLanguageCommand { get; set; }

        public string ChangeLanguageText { get; set; } = "ChangeLanguage";

        private void ChangeLanguage()
        {
            var nextCulture = cultures.FirstOrDefault(x => !x.Name.Equals(localization.GetCurrentCulture().Name));
            localization.ChangeCulture(nextCulture);
        }
    }
}
