using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Data.Querys.Transaction
{
    public interface ITransactionPagingQuery
    {
        IEnumerable<Domain.Transaction> GetPage(int accountId, int page, int pageSize);

        IEnumerable<Domain.Transaction> GetPage(int accountId, string name, int page, int pageSize);

        IEnumerable<Domain.Transaction> GetPage(int accountId, int categoryId, int page, int pageSize);
    }
    public class TransactionPagingQuery : ITransactionPagingQuery
    {
        private readonly WebAppDbContext webAppDbContext;

        public TransactionPagingQuery(WebAppDbContext webAppDbContext)
        {
            this.webAppDbContext = webAppDbContext;
        }

        public IEnumerable<Domain.Transaction> GetPage(int accountId, int page, int pageSize)
        {
            var currentPage = page - 1;

            var result = webAppDbContext
                .Transactions
                .Where(x => x.AccountId.Equals(accountId))
                .OrderByDescending(x => x.Date)
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }

        public IEnumerable<Domain.Transaction> GetPage(int accountId, string name, int page, int pageSize)
        {
            var currentPage = page - 1;

            var result = webAppDbContext
                .Transactions
                .Where(x => x.AccountId.Equals(accountId) && x.Name.ToLower().Contains(name.ToLower()))
                .OrderByDescending(x => x.Date)
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }

        public IEnumerable<Domain.Transaction> GetPage(int accountId, int categoryId, int page, int pageSize)
        {
            var currentPage = page - 1;

            var result = webAppDbContext
                .Transactions
                .Where(x => x.AccountId.Equals(accountId) && x.CategoryId.Equals(categoryId))
                .OrderByDescending(x => x.Date)
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToList();

            return result;
        }
    }
}
