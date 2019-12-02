using Grpc.Core;
using Grpc.Net.Client;
using System;

namespace HomeManagement.Api.Identity.Services
{
    public class Broadcaster
    {
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
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var channel = GrpcChannel.ForAddress(new Uri("http://localhost:5001"), new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            });

            return new Protos.RegistrationRPC.RegistrationRPCClient(channel);
        }
    }
}
