using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts.Mapper;
using HomeManagement.Mapper;
using HomeManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        private readonly IAccountServiceClient serviceClient = App._container.Resolve<IAccountServiceClient>();
        private readonly IAuthServiceClient authServiceClient = App._container.Resolve<IAuthServiceClient>();
        private readonly IAccountMapper accountMapper = App._container.Resolve<IAccountMapper>();

        IEnumerable<Account> accounts;

        public AccountsViewModel()
        {
            Task.Run(async () =>
            {
                var user = authServiceClient.User;

                var page = await serviceClient.Page(new Models.AccountPageModel
                {
                    UserId = user.Id,
                    PageCount = 10,
                    CurrentPage = 1
                });

                Accounts = accountMapper.ToEntities(page.Accounts);
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

        public string AccountLabelText => "";// language.CurrentLanguage.AccountsText;
    }
}
