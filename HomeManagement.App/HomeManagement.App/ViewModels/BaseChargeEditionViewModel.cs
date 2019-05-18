using HomeManagement.App.Services.Rest;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Autofac;

namespace HomeManagement.App.ViewModels
{
    public class BaseChargeEditionViewModel : BaseViewModel
    {
        protected Charge charge = new Charge { Date = DateTime.Now };
        protected Account account;
        protected Category selectedCategory;
        protected ICategoryServiceClient categoryServiceClient;
        protected IChargeServiceClient chargeServiceClient;
        protected IEnumerable<Category> categories;
        protected bool chargeType = false;

        public BaseChargeEditionViewModel()
        {
            categoryServiceClient = App._container.Resolve<ICategoryServiceClient>();
            chargeServiceClient = App._container.Resolve<IChargeServiceClient>();

            CancelCommand = new Command(Cancel);

            ChargeType = false;
        }

        public BaseChargeEditionViewModel(Account account) : this()
        {
            this.account = account;
            charge.AccountId = account.Id;
        }

        public event EventHandler OnError;

        public event EventHandler OnCancel;

        public ICommand CancelCommand { get; }

        public Charge Charge
        {
            get => charge;
            set
            {
                charge = value;
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
                Charge.CategoryId = selectedCategory.Id;
                OnPropertyChanged();
            }
        }

        public bool ChargeType
        {
            get
            {
                return chargeType;
            }
            set
            {
                chargeType = value;
                if (chargeType)
                {
                    Charge.ChargeType = HomeManagement.Domain.ChargeType.Income;
                }
                else
                {
                    Charge.ChargeType = HomeManagement.Domain.ChargeType.Expense;
                }
                OnPropertyChanged(nameof(ChargeType));
            }
        }

        protected override async Task InitializeAsync() => await LoadCategories();        

        protected async Task LoadCategories()
        {
            Categories = await categoryServiceClient.GetCategories();
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
            if (string.IsNullOrEmpty(charge.Name)) return true;

            if (Charge.Price < 0) return true;

            if (Charge.CategoryId < 0) return true;

            if (Charge.AccountId < 0) return true;

            return false;
        }

        protected void Cancel()
        {
            OnCancel?.Invoke(this, EventArgs.Empty);
        }
    }
}
