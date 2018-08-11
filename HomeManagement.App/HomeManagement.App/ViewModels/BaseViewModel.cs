using HomeManagement.App.Services.Components.Language;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected ILanguageFactory language;
        const string englishFlag = "english_flag.png";
        const string spanishFlag = "spanish_flag.png";
        string languageFlag = englishFlag;

        public BaseViewModel()
        {
            language = App._container.Resolve<ILanguageFactory>();

            language.PropertyChanged += Language_PropertyChanged;

            ChangeLanguageCommand = new Command(ChangeLanguage);
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string LanguageText => language.CurrentLanguage.LanguateText;

        public string LanguageFlag => languageFlag;

        public ICommand ChangeLanguageCommand { get; }

        private void ChangeLanguage() => language.ChangeLanguage();

        public void Language_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var properties = GetType().GetRuntimeProperties().Where(x => x.Name.Contains("Text")).ToList();

            foreach (var property in properties)
            {
                OnPropertyChanged(property.Name);
            }

            languageFlag = language.CurrentLanguage.LanguagetType.Equals(Languages.English) ? englishFlag : spanishFlag;
            OnPropertyChanged("LanguageFlag");
        }

        public void OnLanguageChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}