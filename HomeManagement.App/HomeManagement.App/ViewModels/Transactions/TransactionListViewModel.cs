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
        private readonly ITransactionManager transactionManager = App._container.Resolve<ITransactionManager>();
        ObservableCollection<Transaction> transactions;
        Account account;
        Transaction selectedTransaction;

        public TransactionListViewModel(Account account)
        {
            this.account = account;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Transaction>(async (transaction) => await DeleteAsync(transaction));
        }

        public ObservableCollection<Transaction> Transactions
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

        public Transaction SelectedTransaction
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


        private async Task DeleteAsync(Transaction transaction)
        {
            await HandleSafeExecutionAsync(async () =>
            {
                await transactionManager.DeleteTransactionAsync(transaction);
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
