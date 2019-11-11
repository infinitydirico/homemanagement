using HomeManagement.App.Data.Entities;
using Plugin.Media;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AddTransactionViewModel : BaseTransactionEditionViewModel
    {
        public AddTransactionViewModel()
        {
        }

        public AddTransactionViewModel(Account account) : base(account)
        {
            AddTransactionCommand = new Command(AddTransaction);
            TakePictureCommand = new Command(async () => await ProcessTransactionFromPicture());
            SelectedTransactionType = TransactionType.Expense;
        }

        public ICommand AddTransactionCommand { get; }

        public ICommand TakePictureCommand { get; }

        public event EventHandler OnAdded;

        public virtual void AddTransaction()
        {
            if (HasInvalidValues()) return;

            transactionManager.AddTransactionAsync(Transaction);

            OnAdded.Invoke(this, EventArgs.Empty);
        }

        public async Task ProcessTransactionFromPicture()
        {
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    Name = "transaction.jpg"
                });

                if (file == null)
                    return;

                var result = await transactionManager.CreateFromImage(file.GetStream());

                Transaction = result;
            }
            catch 
            {

            }
        }
    }
}
