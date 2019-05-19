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
    public class AccountsViewModel : LocalizationBaseViewModel
    {
        private readonly IAccountServiceClient serviceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        IEnumerable<Account> accounts;

        protected override async Task InitializeAsync()
        {
            await HandleSafeExecution(async () =>
            {
                var user = authenticationManager.GetAuthenticatedUser();

                var page = await serviceClient.Page(new Models.AccountPageModel
                {
                    UserId = user.Id,
                    PageCount = 10,
                    CurrentPage = 1
                });

                Accounts = from a in page.Accounts
                           select new Account
                           {
                               Id = a.Id,
                               Name = a.Name,
                               AccountType = (AccountType)Enum.Parse(typeof(AccountType), a.AccountType.ToString()),
                               Balance = a.Balance,
                               Measurable = a.Measurable,
                               CurrencyId = a.CurrencyId
                           };
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
