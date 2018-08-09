using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeManagement.App.Services.Components.Language
{
    public class LanguageFactory : ILanguageFactory, INotifyPropertyChanged
    {
        public void ChangeLanguage()
        {
            if (CurrentLanguage is EnglishLanguage)
            {
                CurrentLanguage = new SpanishLanguage();
            }
            else
            {
                CurrentLanguage = new EnglishLanguage();
            }

            OnPropertyChanged(nameof(CurrentLanguage));
        }

        public ILanguage CurrentLanguage { get; private set; } = new EnglishLanguage();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface ILanguageFactory
    {
        ILanguage CurrentLanguage { get; }

        void ChangeLanguage();

        event PropertyChangedEventHandler PropertyChanged;
    }

    public enum Languages
    {
        English,
        Spanish
    }
}
