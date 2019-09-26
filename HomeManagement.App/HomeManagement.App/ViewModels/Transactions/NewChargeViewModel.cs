using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class NewChargeViewModel : AddTransactionViewModel
    {
        private readonly IAccountServiceClient accountServiceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        protected IEnumerable<Account> accounts = Enumerable.Empty<Account>();

        public Account SelectedAccount
        {
            get => account;
            set
            {
                account = value;
                Charge.AccountId = account.Id;
                OnPropertyChanged();
            }
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

        protected override async Task InitializeAsync()
        {
            var page = await accountServiceClient.Page(new Models.AccountPageModel
            {
                UserId = authenticationManager.GetAuthenticatedUser().Id,
                PageCount = 10,
                CurrentPage = 1
            });

            accounts = from a in page.Accounts
                       select new Account
                       {
                           Id = a.Id,
                           Name = a.Name,
                           AccountType = (AccountType)Enum.Parse(typeof(AccountType), a.AccountType.ToString()),
                           Balance = a.Balance,
                           Measurable = a.Measurable,
                           CurrencyId = a.CurrencyId
                       };

            await base.InitializeAsync();
        }

        public override void AddCharge()
        {
            base.AddCharge();

            Charge = new Charge();
        }
    }
}
