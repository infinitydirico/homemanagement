using Grpc.Core;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Services
{
    public class RegistrationService : Register.RegisterBase
    {
        private readonly ILogger<RegistrationService> logger;
        private readonly IRepositoryFactory repositoryFactory;

        public RegistrationService(ILogger<RegistrationService> logger, IRepositoryFactory repositoryFactory)
        {
            this.logger = logger;
            this.repositoryFactory = repositoryFactory;
        }

        public override Task<EchoReply> Echo(EchoRequest request, ServerCallContext context)
        {
            return Task.FromResult(new EchoReply());
        }

        public override Task<RegistrationResponse> NewRegistration(RegistrationRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Email.IsEmpty()) throw new ApplicationException("Email cannot be empty.");

                var userEntity = new User
                {
                    Email = request.Email,
                };

                using (var userRepository = repositoryFactory.CreateUserRepository())
                using (var accountRepository = repositoryFactory.CreateAccountRepository())
                using (var preferencesRepository = repositoryFactory.CreatePreferencesRepository())
                using (var categoryRepository = repositoryFactory.CreateCategoryRepository())
                {
                    userRepository.Add(userEntity);
                    userRepository.Commit();

                    accountRepository.Add(new Account
                    {
                        UserId = userEntity.Id,
                        Name = request.Language.Contains("en") ? "Cash" : "Efectivo",
                        AccountType = AccountType.Cash,
                        CurrencyId = 1
                    });

                    accountRepository.Commit();

                    var categories = CategoryRepository.GetDefaultCategories().Select(x => new Category
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Icon = x.Icon,
                        IsActive = true,
                        Measurable = true,
                        UserId = userEntity.Id
                    }).ToList();

                    categoryRepository.Add(categories);
                    categoryRepository.Commit();

                    preferencesRepository.Add(new Preferences
                    {
                        UserId = userEntity.Id,
                        Key = "PreferredCurrency",
                        Value = "USD",
                    });
                    preferencesRepository.Add(new Preferences
                    {
                        UserId = userEntity.Id,
                        Key = "Language",
                        Value = request.Language,
                    });
                    preferencesRepository.Commit();
                }

                return Task.FromResult(new RegistrationResponse
                {
                    Succeed = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on creating new user data");
                return Task.FromResult(new RegistrationResponse
                {
                    Succeed = false
                });
            }
        }
    }
}
