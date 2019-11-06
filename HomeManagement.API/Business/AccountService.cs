using HomeManagement.Contracts.Repositories;
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
        private readonly IUserSessionService userService;

        public AccountService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IAccountMapper accountMapper,
            IUserRepository userRepository,
            IUserSessionService userService)
        {
            this.accountRepository = accountRepository;
            this.transactionRepository = transactionRepository;
            this.accountMapper = accountMapper;
            this.userRepository = userRepository;
            this.userService = userService;
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

        public IEnumerable<AccountModel> GetAccounts()
        {
            var authenticatedUser = userService.GetAuthenticatedUser();

            var accounts = accountRepository
                .GetAllByUser(authenticatedUser.Email)
                .Select(account => accountMapper.ToModel(account))
                .ToList();            

            return accounts;
        }

        public AccountPageModel Page(AccountPageModel model)
        {
            if (model.TotalPages.Equals(default(int)))
            {
                var total = (double)accountRepository.Count(c => c.UserId.Equals(model.UserId));
                var totalPages = System.Math.Ceiling(total / (double)model.PageCount);
                model.TotalPages = int.Parse(totalPages.ToString());
            }

            var currentPage = model.CurrentPage - 1;

            var results = accountRepository.Paginate(x => x.UserId.Equals(model.UserId), x => x.Id, model.PageCount * currentPage, model.PageCount);

            model.Accounts = accountMapper.ToModels(results).ToList();

            return model;
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

        IEnumerable<AccountModel> GetAccounts();

        AccountModel Get(int id);

        AccountPageModel Page(AccountPageModel model);
    }
}
