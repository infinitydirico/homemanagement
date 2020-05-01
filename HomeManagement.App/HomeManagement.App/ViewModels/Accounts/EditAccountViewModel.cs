using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class EditAccountViewModel : BaseViewModel
    {
        private readonly CurrencyServiceClient currencyService = new CurrencyServiceClient();
        private readonly IAccountManager accountManager = App._container.Resolve<IAccountManager>();
        private readonly IAuthenticationManager authenticationManager = App._container.Resolve<IAuthenticationManager>();
    
        Account account;
        Currency selectedCurrency;
        AccountType selectedAccountType;
        IEnumerable<Currency> currencies;

        public EditAccountViewModel(Account account)
        {
            Account = account;
            SelectedAccountType = account.AccountType;
            Title = "AccountName";
        }

        public ICommand FinishEditAccountCommand => new Command(UpdateAccount);

        public Account Account
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged();
            }
        }

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

        private void UpdateAccount()
        {
            HandleSafeExecutionAsync(async () =>
            {
                await accountManager.Update(Account);
            });            
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
            SelectedCurrency = Currencies.First(x => x.Id.Equals(account.CurrencyId));
            OnPropertyChanged(nameof(Account));
        }
    }
}
