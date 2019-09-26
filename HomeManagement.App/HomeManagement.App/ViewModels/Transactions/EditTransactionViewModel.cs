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
            Charge = charge;

            UpdateChargeCommand = new Command(UpdateCharge);
            SelectedChargeType = Charge.ChargeType;
        }

        public ICommand UpdateChargeCommand { get; }

        public event EventHandler OnChargeUpdated;

        private void UpdateCharge()
        {
            if (HasInvalidValues()) return;

            chargeServiceClient.Put(Charge);

            OnChargeUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected override async Task InitializeAsync()
        {
            await LoadCategories();
            SelectedCategory = Categories.FirstOrDefault(x => x.Id.Equals(Charge.CategoryId));
        }
    }
}
