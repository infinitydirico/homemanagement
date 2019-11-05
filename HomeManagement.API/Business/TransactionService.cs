using HomeManagement.API.Exportation;
using HomeManagement.Contracts.Repositories;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeManagement.API.Business
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly IPreferenceService preferenceService;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;
        private readonly IExportableTransaction exportableTransaction;
        private readonly IUserSessionService userService;
        private readonly IUnitOfWork unitOfWork;

        public TransactionService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            ITransactionMapper transactionMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository,
            IPreferenceService preferenceService,
            IExportableTransaction exportableTransaction,
            IUserSessionService userService,
            IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
            this.preferenceService = preferenceService;
            this.exportableTransaction = exportableTransaction;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        public void Add(TransactionModel transaction)
        {
            Category category;

            if (transaction.CategoryId.Equals(default(int)) || categoryRepository.FirstOrDefault(x => x.Id.Equals(transaction.CategoryId)) == null)
            {
                category = categoryRepository.FirstOrDefault();
                transaction.CategoryId = category.Id;
            }

            var account = accountRepository.GetById(transaction.AccountId);

            var entity = transactionMapper.ToEntity(transaction);

            transactionRepository.Add(entity);

            UpdateBalance(entity);

            unitOfWork.Commit();
        }

        public void Update(TransactionModel transaction)
        {
            var entity = transactionMapper.ToEntity(transaction);

            var current = transactionRepository.GetById(entity.Id);

            UpdateBalance(current, true);

            current.Name = entity.Name;
            current.Price = entity.Price;
            current.TransactionType = entity.TransactionType;
            current.CategoryId = entity.CategoryId;
            current.Date = entity.Date;

            transactionRepository.Update(current);            

            UpdateBalance(current);

            unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var entity = transactionRepository.GetById(id);

            transactionRepository.Remove(entity);

            UpdateBalance(entity, true);

            unitOfWork.Commit();
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .Where(x => x.Account.User.Email.Equals(authenticatedUser.Email))
                .Select(x => transactionMapper.ToModel(x))
                .ToList();

            return transactions;
        }

        private void UpdateBalance(Transaction c, bool reverse = false)
        {
            var account = accountRepository.FirstOrDefault(x => x.Id.Equals(c.AccountId));

            var price = c.Price;
            if (reverse)
            {
                price = -c.Price;
            }

            account.Balance = c.TransactionType.Equals(TransactionType.Income) ? account.Balance + price : account.Balance - price;
            accountRepository.Update(account);
        }

        public TransactionModel Get(int id)
        {
            var result = transactionRepository.GetById(id);

            return transactionMapper.ToModel(result);
        }

        public void Import(int accountId, IFormFile formFile)
        {
            var account = accountRepository.FirstOrDefault(x => x.Id.Equals(accountId));

            foreach (var entity in exportableTransaction.ToEntities(formFile.OpenReadStream().GetBytes()))
            {
                if (entity == null) continue;

                if (transactionRepository.Exists(entity)) continue;

                entity.Id = 0;
                entity.AccountId = accountId;
                transactionRepository.Add(entity);

                account.Balance = entity.TransactionType.Equals(TransactionType.Income) ? account.Balance + entity.Price : account.Balance - entity.Price;
                accountRepository.Update(account);
            }

            unitOfWork.Commit();
        }

        public ExportFile Export(int accountId)
        {
            var transactions = transactionRepository.Where(x => x.AccountId.Equals(accountId)).ToList();

            var account = accountRepository.GetById(accountId);

            var csv = exportableTransaction.ToCsv(transactions);

            return new ExportFile
            {
                Filename = $"{account}{DateTime.Now.ToString("yyyyMMddhhmmss")}.csv",
                Content = csv
            };
        }

        public TransactionPageModel Page(TransactionPageModel page)
        {
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
            var authenticatedUser = userService.GetAuthenticatedUser();

            var transactions = transactionRepository
                .GetByAccount(accountId)
                .OrderByDescending(x => x.Date);

            return transactionMapper.ToModels(transactions);
        }

        public IEnumerable<MonthlyCategory> CategoryEvolution(int categoryId)
        {
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
    }

    public interface ITransactionService
    {
        void Add(TransactionModel transaction);

        void Update(TransactionModel transaction);

        void Delete(int id);

        IEnumerable<TransactionModel> GetAll();

        IEnumerable<TransactionModel> GetByAccountId(int accountId);

        TransactionModel Get(int id);

        TransactionPageModel Page(TransactionPageModel page);

        IEnumerable<TransactionModel> FilterByDate(int year, int month);

        IEnumerable<TransactionModel> FilterByDateAndAccount(int year, int month, int accountId);

        IEnumerable<MonthlyCategory> CategoryEvolution(int categoryId);

        IEnumerable<MonthlyCategory> CategoryEvolutionByAccount(int categoryId, int accountId);

        void Import(int accountId, IFormFile formFile);

        ExportFile Export(int accountId);
    }

    public class ExportFile
    {
        public string Filename { get; set; }

        public byte[] Content { get; set; }
    }
}
