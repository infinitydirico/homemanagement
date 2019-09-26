using HomeManagement.App.Data.Entities;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AddTransactionViewModel : BaseTransactionEditionViewModel
    {
        public AddTransactionViewModel()
        {
        }

        public AddTransactionViewModel(Account account) : base(account)
        {
            AddChargeCommand = new Command(AddCharge);
            SelectedChargeType = ChargeType.Expense;
        }

        public ICommand AddChargeCommand { get; }

        public event EventHandler OnAdded;

        public virtual void AddCharge()
        {
            if (HasInvalidValues()) return;

            chargeManager.AddChargeAsync(Charge);

            OnAdded.Invoke(this, EventArgs.Empty);
        }
    }
}
