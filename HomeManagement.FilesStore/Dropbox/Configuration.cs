namespace HomeManagement.FilesStore.Dropbox
{
    public class Configuration
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string AccessToken { get; set; }

        public bool IsInitialzed() => !string.IsNullOrEmpty(AppId) && !string.IsNullOrEmpty(AppSecret);
    }
}
