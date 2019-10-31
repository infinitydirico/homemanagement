using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class TransactionListViewModel : BaseViewModel
    {
        private readonly ITransactionManager transactionManager = App._container.Resolve<ITransactionManager>();
        protected ICategoryManager categoryManager = App._container.Resolve<ICategoryManager>();
        ObservableCollection<Transaction> transactions;
        protected IEnumerable<Category> categories;
        protected Category selectedCategory;
        Account account;
        Transaction selectedTransaction;
        string selectedFilter;
        string filterByName;
        string filterByCategory;

        public TransactionListViewModel(Account account)
        {
            this.account = account;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Transaction>(async (transaction) => await DeleteAsync(transaction));
            FilterCommand = new Command(async () => await DoFilter());
            ClearFiltersCommand = new Command(Refresh);
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

        public IEnumerable<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public Command NextPageCommand { get; }

        public Command PreviousPageCommand { get; }

        public Command DeleteCommand { get; }

        public Command FilterCommand { get; }

        public Command ClearFiltersCommand { get; }

        public IEnumerable<string> Filters => new List<string>
        {
            "Name",
            "Categories"
        };

        public string SelectedFilter
        {
            get => selectedFilter;
            set
            {
                selectedFilter = value;
                OnPropertyChanged();

                FilterByNameVisibile = selectedFilter.Equals(Filters.First());
                OnPropertyChanged(nameof(FilterByNameVisibile));

                FilterByCategoryVisibile = selectedFilter.Equals(Filters.Last());
                OnPropertyChanged(nameof(FilterByCategoryVisibile));
            }
        }

        public string FilterByName
        {
            get => filterByName;
            set
            {
                filterByName = value;
                OnPropertyChanged();
            }
        }

        public bool FilterByNameVisibile { get; private set; }

        public string FilterByCategory
        {
            get => filterByCategory;
            set
            {
                filterByCategory = value;
                OnPropertyChanged();
            }
        }

        public bool FilterByCategoryVisibile { get; private set; }

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
            SelectedFilter = Filters.First();
            Categories = await categoryManager.GetCategories();
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

        public async Task DoFilter()
        {
            HandleSafeExecutionAsync(async () =>
            {
                IsBusy = true;
                var property = FilterByNameVisibile ? nameof(Transaction.Name) : "CategoryId";
                var value = FilterByNameVisibile ? FilterByName : SelectedCategory.Id.ToString();
                Transactions = (await transactionManager.FilterByName(property, value)).ToObservableCollection();
                IsBusy = false;
            });
        }
    }
}
