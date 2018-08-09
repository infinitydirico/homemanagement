using HomeManagement.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace HomeManagement.Core.Cryptography
{
    public class RsaCryptographyService : ICryptography
    {
        public string Decrypt(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);

            return GetRawDecrypted(bytes);
        }

        public string Encrypt(string value)
        {
            var bytes = GetRawEncrypted(value);

            return Encoding.UTF8.GetString(bytes);
        }

        public string GetRawDecrypted(byte[] encrypted)
        {
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSA.ExportParameters(false));

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.            
                var encryptedValue = RSA.Encrypt(encrypted, false);

                return Encoding.UTF8.GetString(encryptedValue);
            }
        }

        public byte[] GetRawEncrypted(string value)
        {
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSA.ExportParameters(true));

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                var bytes = Encoding.UTF8.GetBytes(value);
                return RSA.Decrypt(bytes, false);
            }
        }
    }
}
