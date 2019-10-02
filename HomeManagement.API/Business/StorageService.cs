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

        public StorageService(IStorageItemMapper storageItemMapper,
            IStorageItemRepository storageItemRepository,
            IStorageClient storageClient,
            IUserRepository userRepository,
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            IPreferencesRepository preferencesRepository)
        {
            this.storageItemMapper = storageItemMapper;
            this.storageItemRepository = storageItemRepository;
            this.storageClient = storageClient;
            this.userRepository = userRepository;
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.preferencesRepository = preferencesRepository;
        }

        public async Task<OperationResult> Authorize(string state, string code)
        {
            var preference = preferencesRepository.FirstOrDefault(x => x.Value.Equals(state));

            var user = userRepository.GetById(preference.UserId);

            await storageClient.Authorize(user.Id, code, state);

            return OperationResult.Succeed();
        }

        public string CreateAccessToken(string email)
        {
            var user = userRepository.GetByEmail(email);

            var accessToken = storageClient.GetAccessToken(user.Id);

            return accessToken.ToString();
        }

        public async Task<FileModel> Download(int id, string email)
        {
            var user = userRepository.GetByEmail(email);

            var item = storageItemRepository.GetById(id);

            var fileStream = await storageClient.Download(user.Id, item.Path);

            return new FileModel
            {
                Name = item.Name,
                Stream = fileStream
            };
        }

        public IEnumerable<StorageItemModel> GetStorageItems(string email)
        {
            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email));
            var items = GetRepoItems(user.Id).Select(x => storageItemMapper.ToModel(x));
            return items;
        }

        public IEnumerable<StorageItemModel> GetTransactionFiles(string email, int transactionId)
        {
            var user = userRepository.FirstOrDefault(x => x.Email.Equals(email));

            var items = GetRepoItems(user.Id)
                .Where(x => x.TransactionId.Equals(transactionId))
                .Select(x => storageItemMapper.ToModel(x));

            return items;
        }

        public bool IsAuthorized(int userId) => storageClient.IsAuthorized(userId);

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

        IEnumerable<StorageItemModel> GetStorageItems(string email);

        IEnumerable<StorageItemModel> GetTransactionFiles(string email, int transactionId);

        string CreateAccessToken(string email);

        Task<OperationResult> Authorize(string state, string code);

        Task<FileModel> Download(int id, string email);

        Task<StorageItemModel> Upload(string filename, int transactionId, Stream stream);
    }
}
