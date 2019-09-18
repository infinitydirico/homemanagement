using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class AccountsViewModel : LocalizationBaseViewModel
    {
        private readonly IAccountManager accountManager = App._container.Resolve<IAccountManager>();
        IEnumerable<Account> accounts;

        public IEnumerable<Account> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                OnPropertyChanged();
            }
        }

        protected override async Task InitializeAsync()
        {
            Refresh();
        }

        public override void Refresh()
        {
            HandleSafeExecutionAsync(async () =>
            {
                Accounts = await accountManager.LoadAsync();
            });
        }

        public async Task Delete(Account account)
        {
            await accountManager.Delete(account);
        }
    }
}
