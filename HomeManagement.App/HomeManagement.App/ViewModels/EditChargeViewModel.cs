using HomeManagement.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class EditChargeViewModel : BaseChargeEditionViewModel
    {
        public EditChargeViewModel(Account account, Charge charge) : base(account)
        {
            this.Charge = charge;

            UpdateChargeCommand = new Command(UpdateCharge);

            ChargeType = charge.ChargeType.Equals(HomeManagement.Domain.ChargeType.Income) ? true : false;
        }

        public ICommand UpdateChargeCommand { get; }

        public event EventHandler OnChargeUpdated;

        private void UpdateCharge()
        {
            if (HasInvalidValues()) return;

            chargeServiceClient.Post(Charge);

            OnChargeUpdated?.Invoke(this, EventArgs.Empty);
        }

        protected override void InitializeData()
        {
            Task.Run(async () =>
            {
                await LoadCategories();
                SelectedCategory = Categories.FirstOrDefault(x => x.Id.Equals(Charge.CategoryId));
            });
        }
    }
}
