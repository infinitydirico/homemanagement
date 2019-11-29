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

        public static string CreateGuid(this object value)
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string RemoveEmptySpaces(this string value) => value.Replace(" ", string.Empty);

        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNotEmpty(this string value) => !value.IsEmpty();
    }
}
