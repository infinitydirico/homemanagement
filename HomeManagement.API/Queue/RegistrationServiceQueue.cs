using HomeManagement.API.Queue.Messages;
using HomeManagement.API.RabbitMQ;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.API.Queue
{
    public interface IRegistrationServiceQueue : IConsummerService { }

    public class RegistrationServiceQueue : BackgroundService, IRegistrationServiceQueue
    {
        private readonly ILogger<RegistrationServiceQueue> logger;
        protected readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration configuration;
        private IModel _channel;
        private IConnection _connection;

        public RegistrationServiceQueue(ILogger<RegistrationServiceQueue> logger,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;

            Initialize();
        }

        public string QueueName => "RegistrationQueue";

        public EventingBasicConsumer Consumer { get; private set; }

        private void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMQ:HostName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:HostName cannot be null."),
                UserName = configuration.GetSection("RabbitMQ:UserName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:UserName cannot be null."),
                Password = configuration.GetSection("RabbitMQ:Password").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:Password cannot be null."),
                Port = int.Parse(configuration.GetSection("RabbitMQ:Port").Value),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(int.Parse(configuration.GetSection("RabbitMQ:RequestedConnectionTimeout").Value))
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void BindChannel(IModel channel)
        {
            Consumer = new EventingBasicConsumer(channel);
            Consumer.Received += OnRecieved;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnRecieved;

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(QueueName, false, consumer);

            return Task.CompletedTask;
        }

        private void OnRecieved(object sender, BasicDeliverEventArgs ea)
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

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
