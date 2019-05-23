using HomeManagement.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            Initialize();
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotBusy));
            }
        }

        public bool NotBusy
        {
            get => !isBusy;
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public event EventHandler OnInitializationError;
        public event EventHandler<ErrorEventArgs> OnError;

        private void Initialize()
        {
            Task.Run(async () =>
            {
                await Task.Delay(250);
                try
                {
                    await InitializeAsync();
                }
                catch (Exception ex)
                {
                    OnInitializationError?.Invoke(this, EventArgs.Empty);
                }
            });
        }

        protected virtual async Task InitializeAsync()
        {
            await Task.Yield();
        }

        public virtual void Refresh() => Initialize();

        protected void HandleSafeExecution(Action action)
        {
            IsBusy = true;
            try
            {
                action();
            }
            catch (AppException aex)
            {
                OnError?.Invoke(this, new ErrorEventArgs(aex.Message));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs("An error ocurred."));
            }
            IsBusy = false;
        }

        protected async Task HandleSafeExecutionAsync(Func<Task> action)
        {
            IsBusy = true;
            try
            {
                await action();
            }
            catch(AppException aex)
            {
                OnError?.Invoke(this, new ErrorEventArgs(aex.Message));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs("An error ocurred."));
            }
            IsBusy = false;
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