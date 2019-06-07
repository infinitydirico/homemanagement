using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class AccountsViewModel : LocalizationBaseViewModel
    {
        private readonly IAccountServiceClient serviceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
        private readonly IAccountManager accountManager = App._container.Resolve<IAccountManager>();

        IEnumerable<Account> accounts;

        protected override async Task InitializeAsync()
        {
            await HandleSafeExecutionAsync(async () =>
            {
                Accounts = await accountManager.LoadAsync();
            });
        }

        public IEnumerable<Account> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                OnPropertyChanged();
            }
        }

        public string AccountLabelText => "Accounts";
    }
}
