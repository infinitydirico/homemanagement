using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetByAccount(int accountId);

        IEnumerable<Transaction> GetByMeasurableAccount(int accountId);

        IEnumerable<Transaction> GetByUser(string email);

        decimal SumBy(int userId, TransactionType transactionType, DateTime date);

        void DeleteAllByAccount(int accountId);
    }
}
