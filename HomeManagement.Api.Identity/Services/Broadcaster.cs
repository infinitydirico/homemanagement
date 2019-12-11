using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace HomeManagement.Api.Identity.Services
{
    public class Broadcaster : IBroadcaster
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Broadcaster> logger;

        public Broadcaster(IConfiguration configuration, ILogger<Broadcaster> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public bool BroadcastRegistration(string email, string language)
        {
            try
            {
                var grpcAddress = configuration.GetValue<string>("GprcEndpoint");
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

                var channel = GrpcChannel.ForAddress(grpcAddress);

                var client = new Protos.Registration.RegistrationClient(channel);

                client.CreateDataForNewUser(new Protos.RegistrationRequest
                {
                    Email = email,
                    Language = language
                });

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed at: HomeManagement.Api.Identity.Services.Broadcaster::BroadcastRegistration");
                throw ex;
            }
        }
    }

    public interface IBroadcaster
    {
        bool BroadcastRegistration(string email, string language);
    }
}
