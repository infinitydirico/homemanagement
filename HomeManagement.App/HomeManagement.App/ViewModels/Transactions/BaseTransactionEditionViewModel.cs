using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class BaseTransactionEditionViewModel : BaseViewModel
    {
        protected Transaction transaction = new Transaction { Date = DateTime.Now };
        protected Account account;
        protected Category selectedCategory;
        protected ICategoryManager categoryManager;
        protected ITransactionServiceClient transactionServiceClient;
        protected readonly ITransactionManager transactionManager = App._container.Resolve<ITransactionManager>();
        protected IEnumerable<Category> categories;
        protected TransactionType selectedTransactionType;

        public BaseTransactionEditionViewModel()
        {
            categoryManager = App._container.Resolve<ICategoryManager>();
            transactionServiceClient = App._container.Resolve<ITransactionServiceClient>();

            CancelCommand = new Command(Cancel);
        }

        public BaseTransactionEditionViewModel(Account account) : this()
        {
            this.account = account;
            transaction.AccountId = account.Id;
        }

        public event EventHandler OnError;

        public event EventHandler OnCancel;

        public ICommand CancelCommand { get; }

        public Transaction Transaction
        {
            get => transaction;
            set
            {
                transaction = value;
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
                Transaction.CategoryId = selectedCategory.Id;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TransactionType> TransactionTypes => new List<TransactionType>
        {
                TransactionType.Expense,
                TransactionType.Income
        };

        public TransactionType SelectedTransactionType
        {
            get => selectedTransactionType;
            set
            {
                selectedTransactionType = value;
                Transaction.TransactionType = selectedTransactionType;
                OnPropertyChanged(nameof(SelectedTransactionType));
            }
        }

        protected override async Task InitializeAsync() => await LoadCategories();

        protected async Task LoadCategories()
        {
            Categories = await categoryManager.GetCategories();
        }

        protected virtual bool HasInvalidValues()
        {
            if (CheckForInvalidInput())
            {
                OnError?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        protected bool CheckForInvalidInput()
        {
            if (string.IsNullOrEmpty(transaction.Name)) return true;

            if (Transaction.Price < 0) return true;

            if (Transaction.CategoryId < 0) return true;

            if (Transaction.AccountId < 0) return true;

            return false;
        }

        protected void Cancel()
        {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }
    }
}
