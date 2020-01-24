using Autofac;
using HomeManagement.App.Managers;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels.Cards
{
    public class LanguageCardViewModel : LocalizationBaseViewModel
    {
        CultureInfo[] cultures = new CultureInfo[] { new CultureInfo("es"), new CultureInfo("en") };
        private readonly ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public LanguageCardViewModel()
        {
            ChangeLanguageCommand = new Command(ChangeLanguage);
        }

        public ICommand ChangeLanguageCommand { get; set; }

        private void ChangeLanguage()
        {
            var nextCulture = cultures.FirstOrDefault(x => !x.Name.Equals(localization.GetCurrentCulture().Name));
            localization.ChangeCulture(nextCulture);
        }
    }
}
