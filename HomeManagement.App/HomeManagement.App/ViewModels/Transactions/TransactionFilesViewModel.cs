using Autofac;
using HomeManagement.App.Managers;

namespace HomeManagement.App.ViewModels
{
    public class TransactionFilesViewModel : BaseViewModel
    {
        private readonly IStorageManager storageManager = App._container.Resolve<IStorageManager>();
    }
}
