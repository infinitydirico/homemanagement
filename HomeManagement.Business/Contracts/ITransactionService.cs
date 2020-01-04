using HomeManagement.Models;
using System.Collections.Generic;

namespace HomeManagement.Business.Contracts
{
    public interface ITransactionService
    {
        void Add(TransactionModel transaction);

        void Update(TransactionModel transaction);

        void Delete(int id);

        void BatchDelete(int accountId);

        IEnumerable<TransactionModel> GetAll();

        IEnumerable<TransactionModel> GetByAccountId(int accountId);

        TransactionModel Get(int id);

        TransactionPageModel Page(TransactionPageModel page);

        IEnumerable<TransactionModel> FilterByDate(int year, int month);

        IEnumerable<TransactionModel> FilterByDateAndAccount(int year, int month, int accountId);

        IEnumerable<MonthlyCategory> CategoryEvolution(int categoryId);

        IEnumerable<MonthlyCategory> CategoryEvolutionByAccount(int categoryId, int accountId);

        void Import(int accountId, byte[] bytes);

        FileModel Export(int accountId);
    }
}
