using HomeManagement.App.Services.Rest;
using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Autofac;
using HomeManagement.App.Managers;
using HomeManagement.App.Common;

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
            DeleteCommand = new Command<Charge>(Delete);

            Task.Run(async () =>
            {
                Charges = (await chargeManager.Load(this.account.Id)).ToObservableCollection();
            });
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

        async Task NextPage()
        {
            IsBusy = true;

            Charges = (await chargeManager.NextPage()).ToObservableCollection();

            IsBusy = false;
        }

        async Task PreviousPage()
        {
            IsBusy = true;

            Charges = (await chargeManager.PreviousPage()).ToObservableCollection();

            IsBusy = false;
        }

        private void Delete(Charge charge)
        {
            if (charge != null)
            {
                //chargeServiceClient.Delete(charge.Id);
                //Paginate();
            }
        }
    }
}
