using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.ViewModels
{
    public class TransactionFilesViewModel : BaseViewModel
    {
        private readonly IStorageManager storageManager = App._container.Resolve<IStorageManager>();
        private readonly Transaction transaction;

        public TransactionFilesViewModel(Transaction transaction)
        {
            this.transaction = transaction;
        }

        public IEnumerable<StorageFileModel> Files { get; set; }

        public async Task Download(string filename)
        {
            var value = Files.First(x => x.Name.Equals(filename));

            var file = await storageManager.Get(value.Tag);

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), value.Name);

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(fileName)
            });
        }

        protected override async Task InitializeAsync()
        {
            var files = await storageManager.Get();
            Files = files.Where(x => x.Key.Contains(transaction.Id.ToString())).ToList();
            OnPropertyChanged(nameof(Files));
        }
    }
}
