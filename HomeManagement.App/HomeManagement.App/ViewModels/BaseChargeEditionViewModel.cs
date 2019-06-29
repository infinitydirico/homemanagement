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
    public class BaseChargeEditionViewModel : BaseViewModel
    {
        protected Charge charge = new Charge { Date = DateTime.Now };
        protected Account account;
        protected Category selectedCategory;
        protected ICategoryManager categoryManager;
        protected IChargeServiceClient chargeServiceClient;
        protected readonly IChargeManager chargeManager = App._container.Resolve<IChargeManager>();
        protected IEnumerable<Category> categories;
        protected bool chargeType = false;

        public BaseChargeEditionViewModel()
        {
            categoryManager = App._container.Resolve<ICategoryManager>();
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

        public string ChargeNameText => "AddNewCharge";

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
                    Charge.ChargeType = Data.Entities.ChargeType.Income;
                }
                else
                {
                    Charge.ChargeType = Data.Entities.ChargeType.Expense;
                }
                OnPropertyChanged(nameof(ChargeType));
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
