using HomeManagement.API.Queue.Messages;
using HomeManagement.API.RabbitMQ;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;

namespace HomeManagement.API.Queue
{
    public class RegistrationServiceQueue : BaseServiceQueue
    {
        private readonly ILogger<RegistrationServiceQueue> logger;

        public RegistrationServiceQueue(ILogger<RegistrationServiceQueue> logger,
            IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) 
            : base(serviceScopeFactory, configuration)
        {
            this.logger = logger;
        }

        public override string QueueName => "RegistrationQueue";

        protected override void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            logger.LogInformation($"Consumer cancelled service {nameof(RegistrationServiceQueue)}.");
        }

        protected override void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            logger.LogInformation($"Service {nameof(RegistrationServiceQueue)} has been registered.");
        }

        protected override void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            logger.LogInformation($"shutting down service {nameof(RegistrationServiceQueue)}.");
        }

        protected override void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            logger.LogInformation($"Unregistering service {nameof(RegistrationServiceQueue)}.");
        }

        protected override void OnRecieved(object sender, BasicDeliverEventArgs ea)
        {            
            var message = new RegistrationMessage(ea.Body.ToArray());
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();

                    if (message.Email.IsEmpty()) throw new ApplicationException("Email cannot be empty.");

                    var userEntity = new User
                    {
                        Email = message.Email,
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
                            Name = message.Language.IsNotEmpty() ? message.Language.Contains("en") ? "Cash" : "Efectivo" : "Cash",
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
                            Value = message.Language,
                        });
                        preferencesRepository.Add(new Preferences
                        {
                            UserId = userEntity.Id,
                            Key = "EnableDailyBackups",
                            Value = true.ToString(),
                        });
                        preferencesRepository.Add(new Preferences
                        {
                            UserId = userEntity.Id,
                            Key = "EnableWeeklyEmails",
                            Value = true.ToString(),
                        });
                        preferencesRepository.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on creating new user data.");                
            }
        }

        protected override void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            
        }
    }
}
