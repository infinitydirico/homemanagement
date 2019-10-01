using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Business
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountMapper accountMapper;
        private readonly IUserRepository userRepository;

        public AccountService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
        }

        public OperationResult Add(AccountModel accountModel)
        {
            var entity = accountMapper.ToEntity(accountModel);

            accountRepository.Add(entity);
            accountRepository.Commit();

            return OperationResult.Succeed();
        }

        public OperationResult Delete(int id)
        {
            var transaction = transactionRepository.FirstOrDefault(c => c.AccountId.Equals(id));

            if (transaction != null) return OperationResult.Error("The account has transactions");

            accountRepository.Remove(id);
            accountRepository.Commit();

            return OperationResult.Succeed();
        }

        public AccountModel Get(int id)
        {
            var account = accountRepository.GetById(id);

            if (account == null) return null;

            return accountMapper.ToModel(account);
        }

        public IEnumerable<AccountModel> GetAccounts(string userEmail)
        {
            var accounts = (from account in accountRepository.All
                            join user in userRepository.All
                            on account.UserId equals user.Id
                            where user.Email.Equals(userEmail)
                            select accountMapper.ToModel(account))
                            .ToList();

            return accounts;
        }

        public OperationResult Update(AccountModel accountModel)
        {
            var entity = accountMapper.ToEntity(accountModel);

            accountRepository.Update(entity);
            accountRepository.Commit();

            return OperationResult.Succeed();
        }
    }

    public interface IAccountService
    {
        OperationResult Add(AccountModel accountModel);

        OperationResult Update(AccountModel accountModel);

        OperationResult Delete(int id);

        IEnumerable<AccountModel> GetAccounts(string userEmail);

        AccountModel Get(int id);
    }
}
