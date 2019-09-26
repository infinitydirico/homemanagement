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
        public EditTransactionViewModel(Account account, Transaction charge) : base(account)
        {
            Transaction = charge;

            UpdateTransactionCommand = new Command(UpdateTransaction);
            SelectedTransactionType = Transaction.TransactionType;
        }

        public ICommand UpdateTransactionCommand { get; }

        public event EventHandler OnTransactionUpdated;

        private void UpdateTransaction()
        {
            if (HasInvalidValues()) return;

            transactionManager.UpdateAsync(Transaction);

            OnTransactionUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected override async Task InitializeAsync()
        {
            await LoadCategories();
            SelectedCategory = Categories.FirstOrDefault(x => x.Id.Equals(Transaction.CategoryId));
        }
    }
}
