using HomeManagement.App.Services.Rest;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class ChargesListViewModel : BaseViewModel
    {
        ObservableCollection<Charge> charges;
        private IChargeServiceClient chargeServiceClient = DependencyService.Get<IChargeServiceClient>(DependencyFetchTarget.GlobalInstance);

        Account account;
        Charge selectedCharge;

        ChargePageModel page = new ChargePageModel
        {
            PageCount = 6,
            CurrentPage = 1
        };

        public ChargesListViewModel(Account account)
        {
            this.account = account;
            page.AccountId = account.Id;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Charge>(Delete);

            Task.Run(async () =>
            {
                await Paginate();
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

        public ChargePageModel Page
        {
            get => page;
            set
            {
                page = value;
                Charges = new ObservableCollection<Charge>(page.Charges);
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

        async Task NextPage()
        {
            if (page.CurrentPage.Equals(page.TotalPages)) return;

            Page.CurrentPage++;
            await Paginate();
        }

        async Task PreviousPage()
        {
            if (page.CurrentPage.Equals(1)) return;

            Page.CurrentPage--;
            await Paginate();
        }

        async Task Paginate()
        {
            IsBusy = true;
            Page = await chargeServiceClient.Page(Page);
            IsBusy = false;
        }

        private void Delete(Charge charge)
        {
            if (charge != null)
            {
                chargeServiceClient.Delete(charge.Id);
                Paginate();
            }
        }
    }
}
