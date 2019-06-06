using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AccountsViewModel : LocalizationBaseViewModel
    {
        private readonly IAccountServiceClient serviceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly IAccountManager accountManager = App._container.Resolve<IAccountManager>();

        IEnumerable<Account> accounts;
        bool isRefreshing;

        public AccountsViewModel()
        {
            RefreshCommand = new Command(Refresh);
        }

        protected override async Task InitializeAsync()
        {
            await HandleSafeExecutionAsync(async () =>
            {
                Accounts = await accountManager.LoadAsync();
            });
        }

        public Command RefreshCommand { get; }

        public IEnumerable<Account> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string AccountLabelText => "Accounts";
    }
}
