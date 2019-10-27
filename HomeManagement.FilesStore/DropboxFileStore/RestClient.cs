using Dropbox.Api;
using Dropbox.Api.Files;
using HomeManagement.Core.Extensions;
using HomeManagement.Data;
using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.FilesStore.DropboxFileStore
{
    public class RestClient : IStorageClient
    {
        //ideas on how to connect
        //1 make a call to the API to enable dropbox
        //2 show a new tab with the url to enable access, the redirect url must be a GET method to the API
        //3 when API gets called validate and then in someway the tab should be closed 

        private readonly IPreferencesRepository preferenceRepository;
        private readonly IConfigurationSettingsRepository configurationSettingsRepository;

        private Configuration configuration;

        private DropboxClient dropboxClient;

        public RestClient(IConfigurationSettingsRepository configurationSettingsRepository, 
            IPreferencesRepository preferenceRepository)
        {
            this.preferenceRepository = preferenceRepository;
            this.configurationSettingsRepository = configurationSettingsRepository;
        }

        public string RedirectUri { get; set; } = "http://localhost:60424/api/storage/authorize";

        public async Task Authorize(int userId, string code, string state)
        {
            var existingState = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxState));

            if (existingState == null)
            {
                throw new Exception("Missing access token for dropbox.");
            }

            configuration = GetDropboxConfig();

            var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                code,
                configuration.AppId,
                configuration.AppSecret,
                RedirectUri);

            preferenceRepository.Add(new Domain.Preferences
            {
                UserId = userId,
                Key = Constants.DropboxAccessToken,
                Value = response.AccessToken
            });

            var statePreference = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxState));

            preferenceRepository.Remove(statePreference);
        }

        public bool IsStateValid(int userId, string state)
        {
            var dropboxStatePreference = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxState));

            return dropboxStatePreference != null ?
                dropboxStatePreference.Value.Equals(state) :
                false;
        }

        public Uri GetAccessToken(int userId)
        {
            var state = this.CreateGuid();

            var existingState = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxState));

            if (existingState != null)
            {
                preferenceRepository.Remove(existingState);
            }

            preferenceRepository.Add(new Preferences
            {
                UserId = userId,
                Key = Constants.DropboxState,
                Value = state
            });

            configuration = GetDropboxConfig();

            return DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Code, configuration.AppId, RedirectUri, state: state);
        }

        private void InitialzeClient(string accessToken)
        {
            if (dropboxClient == null)
            {
                dropboxClient = new DropboxClient(accessToken);
            }
        }

        public async Task<IEnumerable<StorageItem>> Get(int userId)
        {
            DropboxCertHelper.InitializeCertPinning();

            var accessToken = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken));

            List<StorageItem> list = new List<StorageItem>();

            using (var dropboxClient = new DropboxClient(accessToken.Value))
            {
                var result = await dropboxClient.Files.ListFolderAsync("/Homemanagement");

                foreach (var item in result.Entries.OrderBy(x => x.IsFile).ThenBy(x => x.Name))
                {
                    list.Add(ParseStorageItem(item));
                }
            }

            return list;
        }

        public async Task<StorageItem> Upload(int userId, string filename, Stream stream)
        {
            DropboxCertHelper.InitializeCertPinning();

            var accessToken = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken));

            using (var dropboxClient = new DropboxClient(accessToken.Value))
            {
                var result = await dropboxClient.Files.UploadAsync(new Dropbox.Api.Files.CommitInfo($"/Homemanagement/{filename}"), stream);

                return ParseStorageItem(result);
                //var file = result.AsFile;

                //return new StorageItem
                //{
                //    ExternalId = file.Id,
                //    IsFolder = false,
                //    Name = file.Name,
                //    Path = file.PathDisplay,
                //    Size = file.Size
                //};
            }
        }

        public async Task<Stream> Download(int userId, string path)
        {
            DropboxCertHelper.InitializeCertPinning();

            var accessToken = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken));

            using (var dropboxClient = new DropboxClient(accessToken.Value))
            {
                var result = await dropboxClient.Files.DownloadAsync(path);

                return await result.GetContentAsStreamAsync();
            }
        }

        public bool IsAuthorized(int userId)
            => preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken)) != null;

        public async Task<StorageItem> Upload(int userId, string filename, string accountName, string chargeName, Stream stream)
        {
            DropboxCertHelper.InitializeCertPinning();

            var accessToken = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken));

            using (var dropboxClient = new DropboxClient(accessToken.Value))
            {
                var result = await dropboxClient.Files.UploadAsync(new CommitInfo($"/Homemanagement/{accountName}/{chargeName}/{filename}"), stream);

                return ParseStorageItem(result);
                //var file = result.AsFile;

                //return new StorageItem
                //{
                //    ExternalId = file.Id,
                //    IsFolder = false,
                //    Name = file.Name,
                //    Path = file.PathDisplay,
                //    Size = file.Size
                //};
            }
        }

        private StorageItem ParseStorageItem(Metadata metadata)
        {
            if (metadata.IsFile)
            {
                var file = metadata.AsFile;

                return new StorageItem
                {
                    ExternalId = file.Id,
                    IsFolder = false,
                    Name = file.Name,
                    Path = file.PathDisplay,
                    Size = file.Size
                };
            }
            else
            {
                var folder = metadata.AsFolder;

                return new StorageItem
                {
                    ExternalId = folder.Id,
                    IsFolder = true,
                    Name = folder.Name,
                    Path = folder.PathDisplay,
                };
            }
        }

        private Configuration GetDropboxConfig()
        {
            var appIdConfig = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(Constants.DropboxAppId));
            var appSecretConfig = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(Constants.DropboxAppSecret));

            if (appIdConfig == null || appSecretConfig == null)
            {
                throw new ArgumentNullException($"Dropbox configs cannot be null.");
            }

            return new Configuration
            {
                AppId = appIdConfig.Value,
                AppSecret = appSecretConfig.Value
            };
        }

        public bool IsConfigured()
        {
            var appIdConfig = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(Constants.DropboxAppId));
            var appSecretConfig = configurationSettingsRepository.FirstOrDefault(x => x.Name.Equals(Constants.DropboxAppSecret));

            return appIdConfig != null && !string.IsNullOrEmpty(appIdConfig.Value) &&
                appSecretConfig != null && !string.IsNullOrEmpty(appSecretConfig.Value);
        }
    }
}
