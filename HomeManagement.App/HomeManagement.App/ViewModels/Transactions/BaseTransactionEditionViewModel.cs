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
        protected Charge transaction = new Charge { Date = DateTime.Now };
        protected Account account;
        protected Category selectedCategory;
        protected ICategoryManager categoryManager;
        protected IChargeServiceClient chargeServiceClient;
        protected readonly IChargeManager transactionManager = App._container.Resolve<IChargeManager>();
        protected IEnumerable<Category> categories;
        protected ChargeType selectedChargeType;

        public BaseTransactionEditionViewModel()
        {
            categoryManager = App._container.Resolve<ICategoryManager>();
            chargeServiceClient = App._container.Resolve<IChargeServiceClient>();

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

        public Charge Transaction
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

        public IEnumerable<ChargeType> TransactionTypes => new List<ChargeType>
        {
                ChargeType.Expense,
                ChargeType.Income
        };

        public ChargeType SelectedTransactionType
        {
            get => selectedChargeType;
            set
            {
                selectedChargeType = value;
                Transaction.ChargeType = selectedChargeType;
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
