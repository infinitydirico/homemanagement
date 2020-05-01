using HomeManagement.App.Data;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Services.Rest;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeManagement.App.Services.BackgroundWorker
{
    public class SincronizationWorker : BaseWorker
    {
        protected readonly TransactionServiceClient transactionServiceClient = new TransactionServiceClient();
        private readonly GenericRepository<Transaction> transactionRepository = new GenericRepository<Transaction>();
        private const string key = "FullSync";

        public bool Sincronized { get; private set; }

        public override int GetTimerPeriod() => 60;

        public async Task NeedsSincronization(bool delta = true)
        {
            Sincronized = false;
            if (!delta)
            {
                Preferences.Set(key, true);
            }
            await Task.Yield();
        }

        protected override async Task Process()
        {
            if (!Sincronized)
            {                
                var fullSyncNeeded = Preferences.Get(key, true);

                var transactions = fullSyncNeeded ?
                    await transactionServiceClient.GetAll() :
                    await transactionServiceClient.GetDelta(DateTime.Now.Year, DateTime.Now.Month);

                var dbTransactions = transactionRepository.GetAll();

                if (fullSyncNeeded) DeleteTransactions(dbTransactions, transactions);

                SaveTransactions(dbTransactions, transactions);                

                Sincronized = true;
                Preferences.Set(key, false);
            }
        }

        private void SaveTransactions(IEnumerable<Transaction> dbTransactions, IEnumerable<TransactionModel> transactionModels)
        {
            var diff = transactionModels
                .Where(x => !dbTransactions.Any(z => x.Id.Equals(z.Id)))
                .Select(x => new Transaction
                {
                    Id = x.Id,
                    AccountId = x.AccountId,
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    Price = x.Price,
                    Date = x.Date,
                    LastApiCall = DateTime.Now,
                    ChangeStamp = DateTime.Now,
                    NeedsUpdate = false,
                    TransactionType = x.TransactionType.Equals(TransactionTypeModel.Expense) ?
                                        TransactionType.Expense :
                                        TransactionType.Income
                })
                .ToList();

            if (!diff.Any()) return;

            foreach (var transaction in diff)
            {
                transactionRepository.Add(transaction);
            }
            transactionRepository.Commit();
        }

        private void DeleteTransactions(IEnumerable<Transaction> dbTransactions, IEnumerable<TransactionModel> transactionModels)
        {
            var diff = dbTransactions
                .Where(x => !transactionModels.Any(z => x.Id.Equals(z.Id)))
                .ToList();

            if (!diff.Any()) return;

            foreach (var transaction in diff)
            {
                transactionRepository.Remove(transaction);
            }
            transactionRepository.Commit();
        }
    }
}
