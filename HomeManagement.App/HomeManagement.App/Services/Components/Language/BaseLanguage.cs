﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeManagement.App.Services.Components.Language
{
    public class BaseLanguage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
