using HomeManagement.Business.Contracts;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeManagement.Business.Units
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IExportableTransaction exportableTransaction;
        private readonly IUserSessionService userService;
        private readonly IRepositoryFactory repositoryFactory;

        public TransactionService(ITransactionMapper transactionMapper,
            ICategoryMapper categoryMapper,
            IExportableTransaction exportableTransaction,
            IUserSessionService userService,
            IRepositoryFactory repositoryFactory)
        {
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.exportableTransaction = exportableTransaction;
            this.userService = userService;
            this.repositoryFactory = repositoryFactory;
        }

        public void Add(TransactionModel transaction)
        {
            Category category;

            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
            {
                if (transaction.CategoryId.Equals(default) || categoryRepository.FirstOrDefault(x => x.Id.Equals(transaction.CategoryId)) == null)
                {
                    category = categoryRepository.FirstOrDefault();
                    transaction.CategoryId = category.Id;
                }

                var entity = transactionMapper.ToEntity(transaction);
                entity.ChangeStamp = DateTime.Now;
                transactionRepository.Add(entity);

                var account = accountRepository.FirstOrDefault(x => x.Id.Equals(entity.AccountId));
                account.Balance = entity.TransactionType.Equals(TransactionType.Income) ?
                    account.Balance + entity.Price :
                    account.Balance - entity.Price;

                accountRepository.Update(account);

                transactionRepository.Commit();
            }
        }

        public void Update(TransactionModel transaction)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var entity = transactionMapper.ToEntity(transaction);

                var current = transactionRepository.GetById(entity.Id);

                var account = accountRepository.FirstOrDefault(x => x.Id.Equals(entity.AccountId));
                var price = -current.Price;
                account.Balance = current.TransactionType.Equals(TransactionType.Income) ?
                    account.Balance + price :
                    account.Balance - price;

                current.Name = entity.Name;
                current.Price = entity.Price;
                current.TransactionType = entity.TransactionType;
                current.CategoryId = entity.CategoryId;
                current.Date = entity.Date;
                current.ChangeStamp = DateTime.Now;

                transactionRepository.Update(current);

                account.Balance = entity.TransactionType.Equals(TransactionType.Income) ?
                    account.Balance + entity.Price :
                    account.Balance - entity.Price;

                transactionRepository.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var entity = transactionRepository.GetById(id);

                transactionRepository.Remove(entity);

                var account = accountRepository.FirstOrDefault(x => x.Id.Equals(entity.AccountId));
                var price = -entity.Price;
                account.Balance = entity.TransactionType.Equals(TransactionType.Income) ?
                    account.Balance + price :
                    account.Balance - price;

                accountRepository.Update(account);

                transactionRepository.Commit();
            }
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email))
                .Select(x => transactionMapper.ToModel(x))
                .ToList();

            return transactions;
        }

        public TransactionModel Get(int id)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var result = transactionRepository.GetById(id);

            return transactionMapper.ToModel(result);
        }

        public void Import(int accountId, byte[] bytes)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var account = accountRepository.FirstOrDefault(x => x.Id.Equals(accountId));
                var transactions = exportableTransaction.ToEntities(bytes).Select(x =>
                {
                    x.Id = 0;
                    x.AccountId = account.Id;
                    x.ChangeStamp = DateTime.Now;
                    return x;
                }).ToList();

                var transactionsBalance = transactions.Sum(x => x.TransactionType.Equals(TransactionType.Income) ? x.Price : -x.Price);

                account.Balance += transactionsBalance;

                transactionRepository.Add(transactions);
                accountRepository.Update(account);

                transactionRepository.Commit();
            }
        }

        public FileModel Export(int accountId)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();
            var accountRepository = repositoryFactory.CreateAccountRepository();

            var transactions = transactionRepository.Where(x => x.AccountId.Equals(accountId)).ToList();

            var account = accountRepository.GetById(accountId);

            var csv = exportableTransaction.ToCsv(transactions);

            return new FileModel
            {
                Name = $"{account}{DateTime.Now.ToString("yyyyMMddhhmmss")}.csv",
                Contents = csv
            };
        }

        public TransactionPageModel Page(TransactionPageModel page)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            Expression<Func<Transaction, bool>> predicate = o => o.AccountId.Equals(page.AccountId);
            Func<Transaction, bool> filter = predicate.Compile();

            var total = (double)transactionRepository.Count(predicate);

            var totalPages = Math.Ceiling(total / (double)page.PageCount);

            page.TotalPages = int.Parse(totalPages.ToString());

            bool isFiltering = !string.IsNullOrEmpty(page.Property) && !string.IsNullOrEmpty(page.FilterValue);

            if (isFiltering)
            {
                filter = filter.Where(page.Property, page.FilterValue.ToLower(), page.Operator.IntToOperator());
            }

            var currentPage = page.CurrentPage - 1;

            var results = transactionRepository.Paginate(filter, x => x.Date, page.PageCount * currentPage, page.PageCount);

            page.Transactions = transactionMapper.ToModels(results).ToList();

            return page;
        }

        public IEnumerable<TransactionModel> FilterByDate(int year, int month)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email) &&
                            x.Date.Year.Equals(year) && x.Date.Month.Equals(month))
                .OrderByDescending(t => t.Date)
                .Select(x => transactionMapper.ToModel(x)).ToList();

            return transactions;
        }

        public IEnumerable<TransactionModel> FilterByDateAndAccount(int year, int month, int accountId)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email) &&
                            x.AccountId.Equals(accountId) &&
                            x.Date.Year.Equals(year) && x.Date.Month.Equals(month))
                .OrderByDescending(t => t.Date)
                .Select(x => transactionMapper.ToModel(x))
                .ToList();

            return transactions;
        }

        public IEnumerable<TransactionModel> GetByAccountId(int accountId)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .GetByAccount(accountId)
                .OrderByDescending(x => x.Date);

            return transactionMapper.ToModels(transactions);

        }

        public IEnumerable<MonthlyCategory> CategoryEvolution(int categoryId)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var result = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email) &&
                            x.Date.Year.Equals(DateTime.Now.Year) && x.Date < DateTime.Now &&
                            x.CategoryId.Equals(categoryId))
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date.Month)
                .Select(x => new MonthlyCategory
                {
                    Month = x.First().Date.ToString("MMMM"),
                    Price = x.Sum(z => z.Price)
                })
                .ToList();

            return result;

        }

        public IEnumerable<MonthlyCategory> CategoryEvolutionByAccount(int categoryId, int accountId)
        {
            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var authenticatedUser = userService.GetAuthenticatedUser();

            var result = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email) &&
                            x.Date.Year.Equals(DateTime.Now.Year) && x.Date < DateTime.Now &&
                            x.CategoryId.Equals(categoryId) && x.AccountId.Equals(accountId))
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date.Month)
                .Select(x => new MonthlyCategory
                {
                    Month = x.First().Date.ToString("MMMM"),
                    Price = x.Sum(z => z.Price)
                })
                .ToList();

            return result;
        }

        public void BatchDelete(int accountId)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var account = accountRepository.GetById(accountId);
                account.Balance = 0;
                transactionRepository.DeleteAllByAccount(accountId);
                accountRepository.Commit();
            }
        }

        public IEnumerable<TransactionModel> GetTransactionsForAutoComplete()
        {
            var user = userService.GetAuthenticatedUser();

            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var transactions = transactionRepository.GetByUser(user.Email);

            var groupedTransactions = transactions
                .GroupBy(x => x.Name.ToLower())
                .OrderBy(x => x.Count())
                .Where(x => x.Count() > 5)
                .ToList();

            var trs = (from t in groupedTransactions.Select(x => x) select t.First()).Select(x => transactionMapper.ToModel(x)).ToList();

            return trs;
        }

        public IEnumerable<TransactionModel> Delta(DateTime dateTime)
        {
            var user = userService.GetAuthenticatedUser();

            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(user.Email) && x.ChangeStamp > dateTime)
                .Select(x => transactionMapper.ToModel(x))
                .ToList();

            return transactions;
        }

        public IEnumerable<TransactionModel> Delta(DateTime dateTime, int accountId)
        {
            var user = userService.GetAuthenticatedUser();

            var transactionRepository = repositoryFactory.CreateTransactionRepository();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(user.Email) &&
                            x.ChangeStamp > dateTime &&
                            x.AccountId.Equals(accountId))
                .Select(x => transactionMapper.ToModel(x))
                .ToList();

            return transactions;
        }
    }
}
