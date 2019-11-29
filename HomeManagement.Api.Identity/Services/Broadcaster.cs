using Grpc.Core;
using Grpc.Net.Client;
using System;

namespace HomeManagement.Api.Identity.Services
{
    public class Broadcaster
    {
        public bool BroadcastRegistration(string email)
        {
            var client = GetServiceClient();

            client.PushNewRegistration(new Protos.RegistrationSender
            {
                MessageType = Protos.EventType.Registration,
                Email = email
            });

            return true;
        }

        private Protos.ListenerService.ListenerServiceClient GetServiceClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(new Uri("http://localhost:5001"), new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            });

            var client = new Protos.ListenerService.ListenerServiceClient(channel);
            return client;
        }
    }
}
