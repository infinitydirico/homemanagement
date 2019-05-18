using Autofac;
using HomeManagement.App.Services.Rest;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.ViewModels
{
    public class NewChargeViewModel : AddChargeViewModel
    {
        private readonly IAccountServiceClient accountServiceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly IAccountMapper accountMapper = App._container.Resolve<IAccountMapper>();

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
                UserId = authServiceClient.User.Id,
                PageCount = 10,
                CurrentPage = 1
            });

            accounts = accountMapper.ToEntities(page.Accounts);

            await base.InitializeAsync();
        }

        public override void AddCharge()
        {
            base.AddCharge();

            Charge = new Charge();
        }
    }
}
