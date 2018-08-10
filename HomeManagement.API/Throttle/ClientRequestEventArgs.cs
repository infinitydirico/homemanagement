using System;

namespace HomeManagement.API.Throttle
{
    public class ClientRequestEventArgs : EventArgs
    {
        public ClientRequestEventArgs(ClientRequest clientRequest)
        {
            ClientRequest = clientRequest;
        }

        public ClientRequest ClientRequest { get; }
    }
}