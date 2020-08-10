using HomeManagement.API.RabbitMQ;
using HomeManagement.API.RabbitMQ.Messages;
using HomeManagement.Business.Units;
using HomeManagement.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace HomeManagement.API.Queue
{
    public class UserDeletionServiceQueue : BaseServiceQueue
    {
        private readonly ILogger<UserDeletionServiceQueue> logger;

        public UserDeletionServiceQueue(IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration,
            ILogger<UserDeletionServiceQueue> logger) 
            : base(serviceScopeFactory, configuration)
        {
            this.logger = logger;
        }

        public override string QueueName => "DeleteUserQueue";

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
            var message = new DeleteUserMessage(ea.Body.ToArray());
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var repositoryFactory = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();

                    var userRepository = repositoryFactory.CreateUserRepository();
                    var user = userRepository.FirstOrDefault(x => x.Email.Equals(message.Email));

                    if(user == null)
                    {
                        logger.LogInformation($"User {message.Email} does not exists.");
                        return;
                    }

                    var userService = new UserService(null, null, repositoryFactory, null);
                    
                    userService.DeleteUser(user.Id);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while deletting user: {message.Email} on {nameof(UserDeletionServiceQueue)}");                
            }
        }

        protected override void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            
        }
    }
}
