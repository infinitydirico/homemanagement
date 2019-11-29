using Grpc.Core;
using Grpc.Net.Client;
using HomeManagement.API.Protos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Services
{
    public class ListenerService
    {
        public bool CheckRegistration()
        {
            var client = GetServiceClient();
            var message = client.Check(new ListenerMessage
            {
                MessageType = EventType.Registration
            });

            return message.Result;
        }

        public IEnumerable<string> GetRegisteredUsers()
        {
            var client = GetServiceClient();

            var response = client.GetRegistrations(new RegistrationRequest());
            
            return response.Emails.ToList();
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