using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class TransactionListViewModel : BaseViewModel
    {
        private readonly IChargeManager transactionManager = App._container.Resolve<IChargeManager>();
        ObservableCollection<Charge> transactions;
        Account account;
        Charge selectedTransaction;

        public TransactionListViewModel(Account account)
        {
            this.account = account;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Charge>(async (charge) => await DeleteAsync(charge));
        }

        public ObservableCollection<Charge> Transactions
        {
            get => transactions;
            set
            {
                transactions = value;
                OnPropertyChanged();
            }
        }

        public Command NextPageCommand { get; }

        public Command PreviousPageCommand { get; }

        public Command DeleteCommand { get; }

        public Charge SelectedTransaction
        {
            get => selectedTransaction;
            set
            {
                selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage => transactionManager.CurrentPage;

        async Task NextPage() =>
            await HandleSafeExecutionAsync(async () => Transactions = (await transactionManager.NextPageAsync()).ToObservableCollection());


        async Task PreviousPage() =>
            await HandleSafeExecutionAsync(async () => Transactions = (await transactionManager.PreviousPageAsync()).ToObservableCollection());


        private async Task DeleteAsync(Charge charge)
        {
            await HandleSafeExecutionAsync(async () =>
            {
                await transactionManager.DeleteChargeAsync(charge);
                Transactions = (await transactionManager.Load(account.Id)).ToObservableCollection();
            });
        }

        protected override async Task InitializeAsync()
        {
            Refresh();
        }

        public override void Refresh()
        {
            HandleSafeExecutionAsync(async () =>
            {
                IsBusy = true;
                Transactions = (await transactionManager.Load(account.Id)).ToObservableCollection();
                IsBusy = false;
            });            
        }
    }
}
