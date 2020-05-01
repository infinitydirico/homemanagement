using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AddAccountViewModel : LocalizationBaseViewModel
    {
        private readonly CurrencyServiceClient currencyService = new CurrencyServiceClient();
        private readonly AccountServiceClient accountServiceClient = new AccountServiceClient();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();

        Currency selectedCurrency;
        AccountType selectedAccountType;
        IEnumerable<Currency> currencies;

        public AddAccountViewModel()
        {
            Account = new Account();
            AddAccountCommand = new Command(async () => await AddAccount());
            SelectedAccountType = AccountType.Cash;
            Account.UserId = authenticationManager.GetAuthenticatedUser().Id;
            Title = "AccountName";
        }

        public event EventHandler OnAccountCreated;

        public Account Account { get; set; }

        public ICommand AddAccountCommand { get; }

        public IEnumerable<Currency> Currencies
        {
            get
            {
                return currencies;
            }
            set
            {
                currencies = value;
                OnPropertyChanged();
            }
        }

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }
            set
            {
                selectedCurrency = value;
                Account.CurrencyId = selectedCurrency.Id;
                OnPropertyChanged();
            }
        }

        public IEnumerable<AccountType> AccountTypes => new List<AccountType>
        {
            AccountType.Bank,
            AccountType.Cash,
            AccountType.CreditCard
        };

        public AccountType SelectedAccountType
        {
            get
            {
                return selectedAccountType;
            }
            set
            {
                selectedAccountType = value;
                Account.AccountType = selectedAccountType;
                OnPropertyChanged();
            }
        }

        public bool MeasureAccount
        {
            get => Account.Measurable;
            set
            {
                Account.Measurable = value;
                OnPropertyChanged();
            }
        }

        protected override async Task InitializeAsync()
        {
            Currencies = (from c in await currencyService.GetCurrencies()
                          select new Currency
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Value = c.Value
                          }).ToList();

            SelectedCurrency = Currencies.FirstOrDefault();
        }

        public async Task AddAccount()
        {
            await HandleSafeExecutionAsync(async () =>
            {
                if (string.IsNullOrEmpty(Account.Name)) throw new AppException($"The Account does not have a name.");

                var accountModel = new Models.AccountModel
                {
                    Id = Account.Id,
                    UserId = Account.UserId,
                    AccountType = (Models.AccountType)Enum.Parse(typeof(Models.AccountType), Account.AccountType.ToString()),
                    Balance = Account.Balance,
                    CurrencyId = Account.CurrencyId,
                    Name = Account.Name,
                    Measurable = Account.Measurable
                };

                await accountServiceClient.Post(accountModel);

                OnAccountCreated?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
