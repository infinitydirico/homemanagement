using HomeManagement.App.Data.Entities;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Files : ContentPage
    {
        private readonly Transaction transaction;

        public Files(Transaction transaction)
        {
            this.transaction = transaction;
            Title = "Files";
            InitializeComponent();
        }
    }
}