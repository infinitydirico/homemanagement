using HomeManagement.App.Common;
using HomeManagement.App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// https://github.com/xamarin/Xamarin.Forms/pull/7840
        /// For the moment, prevent calling a method twice with a flag.
        /// </summary>
        protected bool initializing = false;
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

        public event EventHandler OnInitializationFinished;
        public event EventHandler OnInitializationError;
        public event EventHandler<ErrorEventArgs> OnError;
        public event EventHandler OnSuccess;

        private void Initialize()
        {
            Task.Run(async () =>
            {
                initializing = true;
                await Task.Delay(250);
                try
                {
                    await InitializeAsync();
                    OnInitializationFinished?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    OnInitializationError?.Invoke(this, EventArgs.Empty);
                }
                initializing = false;
            });
        }

        protected virtual async Task InitializeAsync()
        {
            await Task.Yield();
        }

        public virtual void Refresh() => Initialize();

        public ICommand RefreshCommand => new Command(() => Refresh());

        protected void HandleSafeExecution(Action action)
        {
            IsBusy = true;
            try
            {
                action();
                OnSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch (AppException aex)
            {
                Logger.LogException(aex);
                OnError?.Invoke(this, new ErrorEventArgs(aex.Message));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
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
                OnSuccess?.Invoke(this, EventArgs.Empty);
            }
            catch(AppException aex)
            {
                Logger.LogException(aex);
                OnError?.Invoke(this, new ErrorEventArgs(aex.Message));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
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