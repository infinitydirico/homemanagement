using HomeManagement.API.Queue.Messages;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;

namespace HomeManagement.API.RabbitMQ.Consumers
{
    public class RegistrationConsumer : Consumer
    {
        public RegistrationConsumer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override string QueueName => "RegistrationQueue";

        public override EventingBasicConsumer CreateConsumer(IModel channel)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnRecieved;
            return consumer;
        }

        private void OnRecieved(object sender, BasicDeliverEventArgs ea)
        {
            var message = new RegistrationMessage(ea.Body.ToArray());
            try
            {
                if (message.Email.IsEmpty()) throw new ApplicationException("Email cannot be empty.");

                var userEntity = new User
                {
                    Email = message.Email,
                };

                var repositoryFactory = serviceProvider.GetService(typeof(IRepositoryFactory)) as IRepositoryFactory;

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
            catch (Exception ex)
            {
                //logger.LogError(ex, "Exception on creating new user data.");
            }
        }

    }
}
