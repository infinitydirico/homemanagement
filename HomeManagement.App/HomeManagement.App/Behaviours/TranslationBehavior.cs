using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.Localization;
using System.Linq;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class TranslationBehavior : Behavior<VisualElement>
    {
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();
        ILocalization localization = App._container.Resolve<ILocalization>();
        VisualElement element;

        public static readonly BindableProperty LanguageKeyProperty =
            BindableProperty.Create("LanguageKey", typeof(string), typeof(TranslationBehavior), string.Empty);

        public string LanguageKey
        {
            get { return (string)GetValue(LanguageKeyProperty); }
            set { SetValue(LanguageKeyProperty, value); }
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            localization.OnCultureChanged += Localization_OnCultureChanged;
            element = bindable;
            SetLocalizedText();
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            localization.OnCultureChanged -= Localization_OnCultureChanged;
            element = null;
            base.OnDetachingFrom(bindable);
        }

        private void Localization_OnCultureChanged(object sender, CultureChangeEventArgs e)
        {
            SetLocalizedText();
        }

        private void SetLocalizedText()
        {
            var property = element?.GetType().GetProperties()
                .FirstOrDefault(x => x.Name.Equals("Title") || x.Name.Equals("Text"));

            if (property == null) return;

            var value = localizationManager.Translate(LanguageKey);
            property.SetValue(element, value);
        }
    }
}
