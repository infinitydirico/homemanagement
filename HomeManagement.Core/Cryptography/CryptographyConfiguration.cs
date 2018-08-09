using System.Text;

namespace HomeManagement.Core.Cryptography
{
    public class CryptographyConfiguration
    {
        private const string CryptoValue = "9126486423168794";

        public byte[] Key { get; set; } = Encoding.UTF8.GetBytes(CryptoValue);

        public byte[] IV { get; set; } = Encoding.UTF8.GetBytes(CryptoValue);

        public static CryptographyConfiguration GetDefaultConfiguration() => new CryptographyConfiguration();
    }
}
