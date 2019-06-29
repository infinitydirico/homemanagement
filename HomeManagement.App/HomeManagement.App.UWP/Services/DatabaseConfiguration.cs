using HomeManagement.App.Services;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(HomeManagement.App.UWP.Services.DatabaseConfiguration))]
namespace HomeManagement.App.UWP.Services
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public string GetDatabasePath() => Path.Combine(ApplicationData.Current.LocalFolder.Path, "HomeManagement.db");
    }
}
