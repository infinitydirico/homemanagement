namespace HomeManagement.App.Data.Entities
{
    public class AppSettings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        public static AppSettings GetOfflineModeSetting() => new AppSettings { Name = "Offline Mode" };
    }
}
