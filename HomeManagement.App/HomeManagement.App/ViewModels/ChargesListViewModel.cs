using Autofac;
using HomeManagement.App.Common;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.Services.Rest;
using System;
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
        bool isRefreshing;

        public ChargesListViewModel(Account account)
        {
            this.account = account;

            NextPageCommand = new Command(async () => await NextPage());
            PreviousPageCommand = new Command(async () => await PreviousPage());
            DeleteCommand = new Command<Charge>(async (charge) => await DeleteAsync(charge));
            RefreshCommand = new Command(Refresh);
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

        public Command RefreshCommand { get; }

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

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
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
            IsRefreshing = true;
            Charges = (await chargeManager.Load(account.Id)).ToObservableCollection();
            IsRefreshing = false;
        }
    }
}
