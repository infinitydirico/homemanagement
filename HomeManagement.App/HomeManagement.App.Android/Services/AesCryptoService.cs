using HomeManagement.Contracts;
using HomeManagement.Core.Cryptography;

[assembly: Xamarin.Forms.Dependency(typeof(HomeManagement.App.Droid.Services.AesCryptoService))]
namespace HomeManagement.App.Droid.Services
{
    public class AesCryptoService : AesCryptographyService, ICryptography
    {
    }
}