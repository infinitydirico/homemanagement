using HomeManagement.Data;
using HomeManagement.FilesStore;
using HomeManagement.Mapper;
using HomeManagement.Models;
using System;
using System.Collections.Generic;
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

        public bool IsAuthorized(int userId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IStorageService
    {
        bool IsAuthorized(int userId);
    }
}
