using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionMapper transactionMapper;
        private readonly ICategoryMapper categoryMapper;

        public TransactionService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            ITransactionMapper transactionMapper,
            ICategoryMapper categoryMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.transactionMapper = transactionMapper;
            this.categoryMapper = categoryMapper;
            this.userRepository = userRepository;
        }

        public void Add(TransactionModel transaction)
        {
            Category category;

            if (transaction.CategoryId.Equals(default(int)) || categoryRepository.All.FirstOrDefault(x => x.Id.Equals(transaction.CategoryId)) == null)
            {
                category = categoryRepository.FirstOrDefault();
                transaction.CategoryId = category.Id;
            }

            var entity = transactionMapper.ToEntity(transaction);

            transactionRepository.Add(entity);

            UpdateBalance(entity);

            transactionRepository.Commit();
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

            transactionRepository.Commit();
        }

        public void Delete(int id)
        {
            var entity = transactionRepository.GetById(id);

            transactionRepository.Remove(entity);

            UpdateBalance(entity, true);

            transactionRepository.Commit();
        }

        public IEnumerable<TransactionModel> GetAll(string userEmail)
        {
            var transactions = (from transaction in transactionRepository.All
                                join account in accountRepository.All
                                on transaction.AccountId equals account.Id
                                join user in userRepository.All
                                on account.UserId equals user.Id
                                where user.Email.Equals(userEmail)
                                select transactionMapper.ToModel(transaction)).ToList();

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
    }

    public interface ITransactionService
    {
        void Add(TransactionModel transaction);

        void Update(TransactionModel transaction);

        void Delete(int id);

        IEnumerable<TransactionModel> GetAll(string userEmail);

        TransactionModel Get(int id);
    }
}
