using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Services.Rest;
using HomeManagement.Domain;
using HomeManagement.Mapper;
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
        private readonly ICurrencyServiceClient currencyService = App._container.Resolve<ICurrencyServiceClient>();
        private readonly IAccountServiceClient accountServiceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly IAccountMapper accountMapper = App._container.Resolve<IAccountMapper>();
        private readonly ICurrencyMapper currencyMapper = App._container.Resolve<ICurrencyMapper>();
        private readonly IAuthServiceClient authService = App._container.Resolve<IAuthServiceClient>();

        Currency selectedCurrency;
        AccountType selectedAccountType;

        public AddAccountViewModel()
        {
            Account = new Account();
            AddAccountCommand = new Command(async () => await AddAccount());
            SelectedAccountType = AccountType.Cash;
            Account.UserId = authService.User.Id;
        }

        public event EventHandler OnAccountCreated;

        public Account Account { get; set; }

        public ICommand AddAccountCommand { get; }

        public IEnumerable<Currency> Currencies { get; private set; }

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

        protected override async Task InitializeAsync()
        {
            Currencies = currencyMapper.ToEntities(await currencyService.GetCurrencies());

            OnPropertyChanged(nameof(Currencies));

            SelectedCurrency = Currencies.FirstOrDefault();
        }

        public async Task AddAccount()
        {
            await HandleSafeExecution(async () =>
            {
                if (string.IsNullOrEmpty(Account.Name)) throw new AppException($"The Account does not have a name.");

                await accountServiceClient.Update(accountMapper.ToModel(Account));

                OnAccountCreated?.Invoke(this, EventArgs.Empty);
            });
        }

        public string AccountNameText { get; set; } = "AccountName";

        public string CurrencyText { get; set; } = "Currency";

        public string AccountTypeText { get; set; } = "AccountType";
    }
}
