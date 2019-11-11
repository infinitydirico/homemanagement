using Autofac;
using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Core.Caching;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.App.Managers
{
    public interface ITransactionManager
    {
        int PageCount { get; }

        int TotalPages { get; }

        int CurrentPage { get; }

        Task<IEnumerable<Transaction>> Load(int accountId);

        Task<IEnumerable<Transaction>> NextPageAsync();

        Task<IEnumerable<Transaction>> PreviousPageAsync();

        Task<IEnumerable<Transaction>> FilterByName(string property, string value);

        Task AddTransactionAsync(Transaction transaction);

        Task DeleteTransactionAsync(Transaction transaction);

        Task UpdateAsync(Transaction transaction);

        Task<Transaction> CreateFromImage(Stream stream);
    }

    public class TransactionManager : BaseManager<Transaction, TransactionPageModel>, ITransactionManager
    {
        protected readonly ITransactionServiceClient transactionServiceClient = App._container.Resolve<ITransactionServiceClient>();
        private readonly GenericRepository<Transaction> transactionRepository = new GenericRepository<Transaction>();
        private readonly ICachingService cachingService = App._container.Resolve<ICachingService>();

        public TransactionManager()
        {
            page.PageCount = 10;
            page.CurrentPage = 1;
        }

        public int PageCount => page.PageCount;

        public int TotalPages => page.TotalPages;

        public int CurrentPage => page.CurrentPage;

        public virtual async Task AddTransactionAsync(Transaction transaction)
        {
            await transactionServiceClient.Post(MapToModel(transaction));

            if (coudSyncSetting.Enabled)
            {
                cachingService.StoreOrUpdate("ForceApiCall", true);
            }
        }

        public virtual async Task DeleteTransactionAsync(Transaction transaction)
        {
            await transactionServiceClient.Delete(transaction.Id);

            if (coudSyncSetting.Enabled)
            {
                cachingService.StoreOrUpdate("ForceApiCall", true);
            }
        }

        public virtual async Task<IEnumerable<Transaction>> Load(int accountId)
        {
            page.AccountId = accountId;
            page.FilterValue = string.Empty;
            page.Property = string.Empty;
            page.Operator = 0;

            return await Paginate();
        }

        public override async Task<IEnumerable<Transaction>> NextPageAsync()
        {
            if (page.CurrentPage.Equals(page.TotalPages))
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;
                return GetCachedFilteredTransactions(skip);
            }

            return await base.NextPageAsync();
        }

        public override async Task<IEnumerable<Transaction>> PreviousPageAsync()
        {
            if (page.CurrentPage == 1)
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;
                return GetCachedFilteredTransactions(skip);
            }

            return await base.PreviousPageAsync();
        }

        public async Task<IEnumerable<Transaction>> FilterByName(string property, string value)
        {
            page.CurrentPage = 1;
            page.FilterValue = value;
            page.Property = property;
            page.Operator = property.Equals("Name") ? 5 : 0;

            var results = await Paginate();
            return results;
        }

        protected override async Task<IEnumerable<Transaction>> Paginate()
        {
            if (!cachingService.Get<bool>("ForceApiCall") || coudSyncSetting.Enabled)
            {
                var skip = (page.CurrentPage - 1) * page.PageCount;

                if (transactionRepository.Count(x => x.AccountId.Equals(page.AccountId)) > skip)
                {
                    var c = GetCachedFilteredTransactions(skip);
                    return await Task.FromResult(c);
                }
            }

            page.Transactions.Clear();

            await Task.Delay(500);

            page = await transactionServiceClient.Page(page);

            var transasctionsResult = MapPageToEntity(page);

            UpdateCachedTransactions(transasctionsResult);

            return transasctionsResult;
        }

        private void UpdateCachedTransactions(IEnumerable<Transaction> transactions)
        {
            if (!coudSyncSetting.Enabled) return;

            Task.Run(() =>
            {
                foreach (var item in transactions)
                {
                    if (!transactionRepository.Any(x => x.Id.Equals(item.Id)))
                    {
                        transactionRepository.Add(item);
                    }
                }
                transactionRepository.Commit();

                cachingService.StoreOrUpdate("ForceApiCall", false);
            });
        }

        private IEnumerable<Transaction> GetCachedFilteredTransactions(int skip)
            => transactionRepository
            .Where(x => x.AccountId.Equals(page.AccountId))
            .OrderByDescending(x => x.Id)
            .Skip(skip)
            .Take(page.PageCount)
            .ToList();

        private IEnumerable<Transaction> MapPageToEntity(TransactionPageModel page) 
            => from transaction in page.Transactions
               select new Transaction
               {
                   Id = transaction.Id,
                   AccountId = transaction.AccountId,
                   CategoryId = transaction.CategoryId,
                   TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), transaction.TransactionType.ToString()),
                   Date = transaction.Date,
                   Name = transaction.Name,
                   Price = transaction.Price,
                   ChangeStamp = DateTime.Now,
                   LastApiCall = DateTime.Now,
                   NeedsUpdate = false
               };

        public async Task UpdateAsync(Transaction transaction)
        {
            var model = MapToModel(transaction);

            await transactionServiceClient.Put(model);
        }

        private TransactionModel MapToModel(Transaction transaction) => new TransactionModel
        {
            Id = transaction.Id,
            AccountId = transaction.AccountId,
            CategoryId = transaction.CategoryId,
            TransactionType = transaction.TransactionType.Equals(TransactionType.Expense) ? TransactionTypeModel.Expense : TransactionTypeModel.Income,
            Date = transaction.Date,
            Name = transaction.Name,
            Price = transaction.Price
        };

        public async Task<Transaction> CreateFromImage(Stream stream)
        {
            var result = await transactionServiceClient.PostPicture(stream);
            return new Transaction
            {
                Name = result.Name,
                CategoryId = result.CategoryId,
                Date = result.Date,
                Price = result.Price,
                TransactionType = TransactionType.Expense
            };
        }
    }
}
