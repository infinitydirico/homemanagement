using HomeManagement.App.Services.Rest;
using HomeManagement.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class NewChargeViewModel : AddChargeViewModel
    {
        protected IAccountServiceClient accountServiceClient = DependencyService.Get<IAccountServiceClient>(DependencyFetchTarget.GlobalInstance);
        protected IEnumerable<Account> accounts = Enumerable.Empty<Account>();
        private readonly IAuthServiceClient authServiceClient = DependencyService.Get<IAuthServiceClient>(DependencyFetchTarget.GlobalInstance);

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

        protected override void InitializeData()
        {
            Task.Run(async () =>
            {
                var page = await accountServiceClient.Page(new Models.AccountPageModel
                {
                    UserId = authServiceClient.User.Id,
                    PageCount = 10,
                    CurrentPage = 1
                });

                accounts = page.Accounts;
            });

            base.InitializeData();
        }

        public override void AddCharge()
        {
            base.AddCharge();

            Charge = new Charge();
        }
    }
}
