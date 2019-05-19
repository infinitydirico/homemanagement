using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class ChargesListViewModel : BaseViewModel
    {
        private readonly IChargeManager chargeManager = App._container.Resolve<IChargeManager>();
        private readonly IChargeServiceClient chargeServiceClient = App._container.Resolve<IChargeServiceClient>();

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
            await HandleSafeExecution(async () => Charges = (await chargeManager.NextPage()).ToObservableCollection());
        

        async Task PreviousPage() =>        
            await HandleSafeExecution(async () => Charges = (await chargeManager.PreviousPage()).ToObservableCollection());
        

        private async Task DeleteAsync(Charge charge)
        {
            await HandleSafeExecution(async () =>
            {
                await chargeServiceClient.Delete(charge.Id);
                Charges = (await chargeManager.Load(account.Id)).ToObservableCollection();
            });
        }

        protected override async Task InitializeAsync()
        {
            Charges = (await chargeManager.Load(account.Id)).ToObservableCollection();
        }
    }
}
