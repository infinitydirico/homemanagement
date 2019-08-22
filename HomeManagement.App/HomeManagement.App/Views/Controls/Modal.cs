using Autofac;
using HomeManagement.App.Managers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Views.Controls
{
    public class Modal
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public Modal(ContentPage parent)
        {
            Parent = parent;
        }

        public ContentPage Parent { get; }

        public async Task Show(string text, string title = "") => await Parent.DisplayAlert(title, text, "Ok");

        public async Task<bool> ShowOkCancel(string text, string title = "")
        {
            var acceptText = localizationManager.Translate("Accept");
            var cancelText = localizationManager.Translate("Cancel");
            return await Parent.DisplayAlert(title, text, acceptText, cancelText);
        }
    }
}
