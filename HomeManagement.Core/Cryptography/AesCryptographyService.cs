using HomeManagement.Contracts;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HomeManagement.Core.Cryptography
{
    public class AesCryptographyService : ICryptography
    {
        private CryptographyConfiguration configuration;

        public AesCryptographyService() : this(CryptographyConfiguration.GetDefaultConfiguration()) { }

        public AesCryptographyService(CryptographyConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException($"{nameof(CryptographyConfiguration)} cannot be null.");
        }

        public string Decrypt(string value)
        {
            return DecryptAes(value);
        }

        public string Encrypt(string value)
        {
            return EncryptToAes(value);
        }


        private AesCryptoServiceProvider CreateAesServiceWithConfig()
        {
            AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider();

            aesAlg.Key = configuration.Key;
            aesAlg.IV = configuration.IV;

            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.FeedbackSize = 128;

            return aesAlg;
        }

        #region working methods

        /// <summary>
        /// Encrpyt a string value using the AES encription algorithim
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private string EncryptToAes(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentException("plain text cannot be null or empty");

            string result = string.Empty;

            RijndaelManaged aes = null;

            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                aes = new RijndaelManaged();

                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.FeedbackSize = 128;

                aes.Key = configuration.Key;
                aes.IV = configuration.IV;

                using (var transform = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var cipherStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(cipherStream, transform, CryptoStreamMode.Write))
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        binaryWriter.Write(plainTextBytes);
                    }

                    var array = cipherStream.ToArray();
                    result = Convert.ToBase64String(array);
                }
            }
            finally
            {
                if (aes != null)
                {
                    aes.Clear();
                }
            }

            return result;
        }

        /// <summary>
        /// decrpyt a string value using the AES encription algorithim
        /// </summary>
        /// <param name="cypher"></param>
        /// <returns></returns>
        private string DecryptAes(string cypher)
        {
            if (string.IsNullOrEmpty(cypher)) throw new ArgumentException("cypher text cannot be null or empty");

            string result = string.Empty;

            RijndaelManaged aes = null;

            try
            {
                byte[] bytes = Convert.FromBase64String(cypher);

                using (var ms = new MemoryStream(bytes))
                {
                    aes = new RijndaelManaged();

                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.FeedbackSize = 128;

                    aes.Key = configuration.Key;
                    aes.IV = configuration.IV;

                    ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (CryptoStream cryptoStream = new CryptoStream(ms, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                            result = streamReader.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aes != null)
                {
                    aes.Clear();
                }
            }
            return result;

        }

        #endregion

        #region non working methods

        public string GetRawDecrypted(byte[] encrypted)
        {
            // Check arguments.
            if (encrypted == null || encrypted.Length <= 0) throw new ArgumentNullException($"{nameof(encrypted)} cannot be null.");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = CreateAesServiceWithConfig())
            {
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encrypted))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        public byte[] GetRawEncrypted(string value)
        {
            // Check arguments.
            if (value == null || value.Length <= 0) throw new ArgumentNullException($"{nameof(value)} cannot be null.");

            byte[] encrypted = Encoding.UTF8.GetBytes(value);

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = CreateAesServiceWithConfig())
            // Create a decrytor to perform the stream transform.
            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (BinaryWriter binaryWritter = new BinaryWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        binaryWritter.Write(value);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        #endregion
    }
}
