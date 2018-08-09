using HomeManagement.Domain;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AddChargeViewModel : BaseChargeEditionViewModel
    {
        public AddChargeViewModel()
        {
        }

        public AddChargeViewModel(Account account) : base(account)
        {
            AddChargeCommand = new Command(AddCharge);
        }

        public ICommand AddChargeCommand { get; }

        public event EventHandler OnAdded;

        public virtual void AddCharge()
        {
            if (HasInvalidValues()) return;

            chargeServiceClient.Post(Charge);

            OnAdded.Invoke(this, EventArgs.Empty);
        }
    }
}
