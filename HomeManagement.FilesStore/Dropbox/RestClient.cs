using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManagement.FilesStore.Dropbox
{
    public class RestClient
    {
        private readonly Configuration configuration;
        // This loopback host is for demo purpose. If this port is not
        // available on your machine you need to update this URL with an unused port.
        private const string LoopbackHost = "http://127.0.0.1:52475/";

        // URL to receive OAuth 2 redirect from Dropbox server.
        // You also need to register this redirect URL on https://www.dropbox.com/developers/apps.
        private readonly Uri RedirectUri = new Uri(LoopbackHost + "authorize");

        public RestClient(Configuration configuration)
        {
            if (!configuration.IsInitialzed()) throw new ArgumentException($"The parameter {nameof(configuration)} has not been initialized.");

            this.configuration = configuration;
        }

        public void GetFiles()
        {
            DropboxCertHelper.InitializeCertPinning();

            //DropboxClient client = new DropboxClient()
        }

        public void GetAccessToken()
        {
            var state = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, configuration.AppId, RedirectUri, state: state);

        }
    }
}
