using Dropbox.Api;
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

        private readonly Configuration configuration;

        private DropboxClient dropboxClient;

        public RestClient(Configuration configuration, IPreferencesRepository preferenceRepository)
        {
            if (!configuration.IsInitialzed()) throw new ArgumentException($"The parameter {nameof(configuration)} has not been initialized.");

            this.configuration = configuration;
            this.preferenceRepository = preferenceRepository ?? throw new NullReferenceException($"The parameter{nameof(preferenceRepository)} is null.");
        }

        public string RedirectUri { get; set; } = "http://localhost:60424/api/storage/authorize";

        public async void GetFiles()
        {
            DropboxCertHelper.InitializeCertPinning();

            var root = await dropboxClient.Files.ListFolderAsync("/");

            var rootFolder = root.Entries.FirstOrDefault();

            var filesMetadata = await dropboxClient.Files.GetPreviewAsync(rootFolder.AsFolder.PathLower);
        }

        public async Task Authorize(int userId, string code, string state)
        {
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
            var state = Guid.NewGuid().ToString("N");

            preferenceRepository.Add(new Domain.Preferences
            {
                UserId = userId,
                Key = Constants.DropboxState,
                Value = state
            });

            return DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Code, configuration.AppId, RedirectUri, state: state);
        }

        private void InitialzeClient(string accessToken)
        {
            if (dropboxClient == null)
            {
                dropboxClient = new DropboxClient(accessToken);
            }
        }

        public async Task<StorageItem> GetRoot(int userId)
        {
            DropboxCertHelper.InitializeCertPinning();

            var accessToken = preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken));

            using (var dropboxClient = new DropboxClient(accessToken.Value))
            {
                var result = await dropboxClient.Files.ListFolderAsync("/");

                if (result.Entries.Count.Equals(0)) return null;

                return result.Entries.Select(x => new StorageItem
                {
                    IsFolder = true,
                    Name = x.Name,
                    Path = x.PathDisplay
                }).First(x => x.Name.Equals("Homemanagement"));
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
                    if (item.IsFile)
                    {
                        var file = item.AsFile;

                        list.Add(new StorageItem
                        {
                            ExternalId = file.Id,
                            IsFolder = false,
                            Name = file.Name,
                            Path = file.PathDisplay,
                            Size = file.Size
                        });
                    }
                    else
                    {
                        var folder = item.AsFolder;

                        list.Add(new StorageItem
                        {
                            ExternalId = folder.Id,
                            IsFolder = true,
                            Name = folder.Name,
                            Path = folder.PathDisplay,
                        });
                    }
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

                var file = result.AsFile;

                return new StorageItem
                {
                    ExternalId = file.Id,
                    IsFolder = false,
                    Name = file.Name,
                    Path = file.PathDisplay,
                    Size = file.Size
                };
            }
        }

        public bool IsAuthorized(int userId) 
            => preferenceRepository.FirstOrDefault(x => x.UserId.Equals(userId) && x.Key.Equals(Constants.DropboxAccessToken)) != null;
    }
}
