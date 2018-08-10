using System;
using System.Security.Cryptography;
using System.Text;

namespace HomeManagement.Core.Extensions
{
    public static class String
    {
        public static Guid ToGuid(this string value)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(data);
        }
    }
}
