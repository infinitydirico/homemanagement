using HomeManagement.App.Data.Entities;
using HomeManagement.App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using HomeManagement.Models;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Files : ContentPage
    {
        public Files(Transaction transaction)
        {
            Title = "Files";
            BindingContext = new TransactionFilesViewModel(transaction);
            InitializeComponent();
        }

        private async void OnFileSelected(object sender, SelectionChangedEventArgs e)
        {
            var file = e.CurrentSelection.First() as StorageFileModel;
            await ((TransactionFilesViewModel)BindingContext).Download(file.Name);
        }
    }
}