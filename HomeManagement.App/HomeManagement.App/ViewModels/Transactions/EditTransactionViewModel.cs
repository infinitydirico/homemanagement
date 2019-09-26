using HomeManagement.App.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class EditTransactionViewModel : BaseTransactionEditionViewModel
    {
        public EditTransactionViewModel(Account account, Charge charge) : base(account)
        {
            Transaction = charge;

            UpdateTransactionCommand = new Command(UpdateTransaction);
            SelectedTransactionType = Transaction.ChargeType;
        }

        public ICommand UpdateTransactionCommand { get; }

        public event EventHandler OnTransactionUpdated;

        private void UpdateTransaction()
        {
            if (HasInvalidValues()) return;

            chargeServiceClient.Put(Transaction);

            OnTransactionUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected override async Task InitializeAsync()
        {
            await LoadCategories();
            SelectedCategory = Categories.FirstOrDefault(x => x.Id.Equals(Transaction.CategoryId));
        }
    }
}
