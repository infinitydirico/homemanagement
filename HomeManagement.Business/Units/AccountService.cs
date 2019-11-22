using HomeManagement.Business.Contracts;
using HomeManagement.Data;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Business.Units
{
    public class AccountService : IAccountService
    {
        private readonly IAccountMapper accountMapper;
        private readonly IUserSessionService userService;
        private readonly IRepositoryFactory repositoryFactory;

        public AccountService(IAccountMapper accountMapper,
            IUserSessionService userService,
            IRepositoryFactory repositoryFactory)
        {
            this.accountMapper = accountMapper;
            this.userService = userService;
            this.repositoryFactory = repositoryFactory;
        }

        public OperationResult Add(AccountModel accountModel)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var entity = accountMapper.ToEntity(accountModel);

                accountRepository.Add(entity);
                accountRepository.Commit();
            }
            return OperationResult.Succeed();
        }

        public OperationResult Delete(int id)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            {
                var transaction = transactionRepository.FirstOrDefault(c => c.AccountId.Equals(id));

                if (transaction != null) return OperationResult.Error("The account has transactions");

                accountRepository.Remove(id);
                accountRepository.Commit();
            }

            return OperationResult.Succeed();
        }

        public AccountModel Get(int id)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var account = accountRepository.GetById(id);

                if (account == null) return null;

                return accountMapper.ToModel(account);
            }
        }

        public IEnumerable<AccountModel> GetAccounts()
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var authenticatedUser = userService.GetAuthenticatedUser();

                var accounts = accountRepository
                    .GetAllByUser(authenticatedUser.Email)
                    .Select(account => accountMapper.ToModel(account))
                    .ToList();

                return accounts;
            }
        }

        public AccountPageModel Page(AccountPageModel model)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
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
        }

        public OperationResult Update(AccountModel accountModel)
        {
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            {
                var entity = accountMapper.ToEntity(accountModel);

                accountRepository.Update(entity);
                accountRepository.Commit();

                return OperationResult.Succeed();
            }
        }
    }
}
