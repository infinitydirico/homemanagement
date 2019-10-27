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
        private readonly IStorageItemRepository storageItemRepository;
        private readonly IStorageClient storageClient;
        private readonly IUserRepository userRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IPreferencesRepository preferencesRepository;
        private readonly IUserSessionService userService;

        public StorageService(IStorageItemMapper storageItemMapper,
            IStorageItemRepository storageItemRepository,
            IStorageClient storageClient,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IPreferencesRepository preferencesRepository,
            IUserSessionService userService)
        {
            this.storageItemMapper = storageItemMapper;
            this.storageItemRepository = storageItemRepository;
            this.storageClient = storageClient;
            this.userRepository = userRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.preferencesRepository = preferencesRepository;
            this.userService = userService;
        }

        public async Task<OperationResult> Authorize(string state, string code)
        {
            var preference = preferencesRepository.FirstOrDefault(x => x.Value.Equals(state));

            var user = userRepository.GetById(preference.UserId);

            await storageClient.Authorize(user.Id, code, state);

            return OperationResult.Succeed();
        }

        public string CreateAccessToken()
        {
            var user = userService.GetAuthenticatedUser();

            var accessToken = storageClient.GetAccessToken(user.Id);

            return accessToken.ToString();
        }

        public async Task<FileModel> Download(int id)
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
            var transaction = transactionRepository.GetById(transactionId);
            var account = accountRepository.GetById(transaction.AccountId);

            var storageItem = await storageClient.Upload(account.UserId, filename, account.Name, transaction.Name, stream);

            storageItem.TransactionId = transactionId;

            storageItemRepository.Add(storageItem);
            storageItemRepository.Commit();

            return storageItemMapper.ToModel(storageItem);
        }

        private List<StorageItem> GetRepoItems(int userId)
        {
            return (from storageItem in storageItemRepository.All
                    join transaction in transactionRepository.All
                    on storageItem.TransactionId equals transaction.Id
                    join account in accountRepository.All
                    on transaction.AccountId equals account.Id
                    where account.UserId.Equals(userId)
                    select storageItem).ToList();
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
