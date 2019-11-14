using HomeManagement.Data;
using HomeManagement.Domain;
using HomeManagement.FilesStore;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.API.Business
{
    public class StorageService : IStorageService
    {
        private readonly IStorageItemMapper storageItemMapper;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IStorageClient storageClient;
        private readonly IUserSessionService userService;

        public StorageService(IStorageItemMapper storageItemMapper,
            IRepositoryFactory repositoryFactory,
            IStorageClient storageClient,
            IUserSessionService userService)
        {
            this.storageItemMapper = storageItemMapper;
            this.repositoryFactory = repositoryFactory;
            this.storageClient = storageClient;
            this.userService = userService;
        }

        public async Task<OperationResult> Authorize(string state, string code)
        {
            using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
            using (var userRepository = repositoryFactory.CreateUserRepository())
            {
                var preference = preferencesRepository.FirstOrDefault(x => x.Value.Equals(state));

                var user = userRepository.GetById(preference.UserId);

                await storageClient.Authorize(user.Id, code, state);

                return OperationResult.Succeed();
            }
        }

        public string CreateAccessToken()
        {
            var user = userService.GetAuthenticatedUser();

            var accessToken = storageClient.GetAccessToken(user.Id);

            return accessToken.ToString();
        }

        public async Task<FileModel> Download(int id)
        {
            using (var storageItemRepository = repositoryFactory.CreateStorageItemRepository())
            {
                var user = userService.GetAuthenticatedUser();

                var item = storageItemRepository.GetById(id);

                var fileStream = await storageClient.Download(user.Id, item.Path);

                return new FileModel
                {
                    Name = item.Name,
                    Stream = fileStream
                };
            }
        }

        public IEnumerable<StorageItemModel> GetStorageItems()
        {
            var user = userService.GetAuthenticatedUser();
            var items = GetRepoItems(user.Id).Select(x => storageItemMapper.ToModel(x));
            return items;
        }

        public IEnumerable<StorageItemModel> GetTransactionFiles(int transactionId)
        {
            var user = userService.GetAuthenticatedUser();

            var items = GetRepoItems(user.Id)
                .Where(x => x.TransactionId.Equals(transactionId))
                .Select(x => storageItemMapper.ToModel(x));

            return items;
        }

        public bool IsAuthorized(int userId) => storageClient.IsAuthorized(userId);

        public bool IsConfigured() => storageClient.IsConfigured();

        public async Task<StorageItemModel> Upload(string filename, int transactionId, Stream stream)
        {
            using (var transactionRepository = repositoryFactory.CreateTransactionRepository())
            using (var accountRepository = repositoryFactory.CreateAccountRepository())
            using (var storageItemRepository = repositoryFactory.CreateStorageItemRepository())
            {
                var transaction = transactionRepository.GetById(transactionId);
                var account = accountRepository.GetById(transaction.AccountId);

                var storageItem = await storageClient.Upload(account.UserId, filename, account.Name, transaction.Name, stream);

                storageItem.TransactionId = transactionId;

                storageItemRepository.Add(storageItem);
                storageItemRepository.Commit();

                return storageItemMapper.ToModel(storageItem);
            }
        }

        private List<StorageItem> GetRepoItems(int userId)
        {
            throw new System.NotImplementedException("pending change");
        }
    }

    public interface IStorageService
    {
        bool IsAuthorized(int userId);

        IEnumerable<StorageItemModel> GetStorageItems();

        IEnumerable<StorageItemModel> GetTransactionFiles(int transactionId);

        string CreateAccessToken();

        Task<OperationResult> Authorize(string state, string code);

        Task<FileModel> Download(int id);

        Task<StorageItemModel> Upload(string filename, int transactionId, Stream stream);

        bool IsConfigured();
    }
}
