using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class ChargesListViewModel : BaseViewModel
    {
        private readonly IChargeManager chargeManager = App._container.Resolve<IChargeManager>();
        ObservableCollection<Charge> charges;
        Account account;
        Charge selectedCharge;

        public ChargesListViewModel(Account account)
        {
            this.account = account;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Charge>(async (charge) => await DeleteAsync(charge));
        }

        public ObservableCollection<Charge> Charges
        {
            get => charges;
            set
            {
                charges = value;
                OnPropertyChanged();
            }
        }

        public Command NextPageCommand { get; }

        public Command PreviousPageCommand { get; }

        public Command DeleteCommand { get; }

        public Charge SelectedCharge
        {
            get => selectedCharge;
            set
            {
                selectedCharge = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage => chargeManager.CurrentPage;

        async Task NextPage() =>
            await HandleSafeExecutionAsync(async () => Charges = (await chargeManager.NextPageAsync()).ToObservableCollection());


        async Task PreviousPage() =>
            await HandleSafeExecutionAsync(async () => Charges = (await chargeManager.PreviousPageAsync()).ToObservableCollection());


        private async Task DeleteAsync(Charge charge)
        {
            await HandleSafeExecutionAsync(async () =>
            {
                await chargeManager.DeleteChargeAsync(charge);
                Charges = (await chargeManager.Load(account.Id)).ToObservableCollection();
            });
        }

        protected override async Task InitializeAsync()
        {
            Refresh();
        }

        public override void Refresh()
        {
            HandleSafeExecutionAsync(async () =>
            {
                IsBusy = true;
                Charges = (await chargeManager.Load(account.Id)).ToObservableCollection();
                IsBusy = false;
            });            
        }
    }
}
