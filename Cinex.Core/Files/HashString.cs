
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cinex.Core.Files
{
    public class HashString
    {
        public static string hash(string key, string text)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] hashBytes = hmac.ComputeHash(bytes);

                StringBuilder hexHash = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hexHash.Append(b.ToString("x2")); // Convert byte to hex
                }

                // Truncate to a desired length, e.g., 16 characters
                return hexHash.ToString().Substring(0, 16);
            }
        }
        public static string HashData(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            // One-liner for HMACMD5 in modern .NET
            byte[] hash;
            using (HMACMD5 hmac = new HMACMD5(keyBytes))
            {
                hash = hmac.ComputeHash(dataBytes);
            }

            return Convert.ToBase64String(hash); // 24 characters long
        }
        public static string HashPassword(string password, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("cinexapikeyxglog"); // Must be 32 bytes for AES-256

        public static string Encrypt(string plainText)
        {
            // C# 7.3 requires explicit using blocks with curly braces
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.GenerateIV(); // Create a new IV for every encryption
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var ms = new MemoryStream())
                {
                    // Write IV to the start of the stream so we can use it for decryption
                    ms.Write(iv, 0, iv.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            byte[] fullCiphertext = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;

                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] ciphertext = new byte[fullCiphertext.Length - iv.Length];

                // Extract IV and ciphertext from the byte array
                Buffer.BlockCopy(fullCiphertext, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCiphertext, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                // Stacked using statements handle clean-up for all streams efficiently
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(ciphertext))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
