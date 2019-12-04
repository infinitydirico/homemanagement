using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeManagement.Api.Identity.Services
{
    public class Broadcaster : IBroadcaster
    {
        private readonly IConfiguration configuration;

        public Broadcaster(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool BroadcastRegistration(string email, string language)
        {
            var client = GetServiceClient();

            client.NewRegistration(new Protos.User
            {
                Email = email,
                Language = language
            });

            return true;
        }

        private Protos.RegistrationRPC.RegistrationRPCClient GetServiceClient()
        {
            var grpcAddress = configuration.GetValue<string>("GprcEndpoint");
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(new Uri(grpcAddress), new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            });

            return new Protos.RegistrationRPC.RegistrationRPCClient(channel);
        }
    }

    public interface IBroadcaster
    {
        bool BroadcastRegistration(string email, string language);
    }
}
