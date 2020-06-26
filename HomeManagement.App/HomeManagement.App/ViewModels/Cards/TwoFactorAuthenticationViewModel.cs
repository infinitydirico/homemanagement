using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Managers;
using System;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels.Cards
{
    public class TwoFactorAuthenticationViewModel : BaseViewModel
    {
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private bool isEnabled;
        private string code;
        private int expiration;

        public event EventHandler<CodeChangeEventArgs> OnCodeChanging;

        public bool IsEnabled 
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
                ChangeTwoFactor(isEnabled);
            }
        }

        public string Code
        {
            get => code;
            set
            {
                code = value;
                OnPropertyChanged();
            }
        }

        public int Expiration
        {
            get => expiration;
            set
            {
                expiration = value;
                OnPropertyChanged();
            }
        }

        protected override async Task InitializeAsync()
        {
            isEnabled = await authenticationManager.IsTwoFactorEnabled();
            OnPropertyChanged(nameof(IsEnabled));
        }

        private void ChangeTwoFactor(bool value)
        {
            Task.Run(async () =>
            {
                await authenticationManager.ChangeTwoFactorAuthentication(value);

                if (value)
                {
                    await UpdateCode();
                }
            });
        }

        private async Task UpdateCode(object state = null)
        {
            OnCodeChanging?.Invoke(this, CodeChangeEventArgs.Changed(false));

            var result = await authenticationManager.GetUserCode();
            var s1 = result.Code.ToString().Substring(0, 2);
            var s2 = result.Code.ToString().Substring(2, 2);
            var s3 = result.Code.ToString().Substring(4, 2);

            Code = $"{s1} {s2} {s3}";
            Expiration = (result.Expiration - DateTime.Now).Seconds;

            OnCodeChanging?.Invoke(this, CodeChangeEventArgs.Changed(true));

            await Task.Delay(Expiration * 1000);
            await UpdateCode();
        }
    }
}