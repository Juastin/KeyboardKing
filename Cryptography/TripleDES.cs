using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    /// <summary>
    /// Encryption Algorithm class used to encrypt and decrypt string data.
    /// </summary>
    public static class TripleDES
    {
        /// <summary>
        /// Specifies the block cipher mode to use for encryption.
        /// </summary>
        private const CipherMode Mode = CipherMode.ECB;

        /// <summary>
        /// Used to encrypt a data string 'source', using string 'key'.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string source, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = Mode;
                    byte[] data = Encoding.UTF8.GetBytes(source);
                    return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        /// <summary>
        /// Used to decrypt a data string 'encrypt', using string 'key'.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string encrypt, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = Mode;
                    byte[] data = Convert.FromBase64String(encrypt);
                    return Encoding.UTF8.GetString(tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        /// <summary>
        /// Static method used to quickly encrypt or decrypt data using a set key.
        /// Pass true to encrypt and false to decrypt.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string EncryptOrDecrypt(string source, bool encrypt)
        {
            return encrypt ? Encrypt(source, "332cc6da-d757-4e80-a726-0bf6b615df09") : Decrypt(source, "332cc6da-d757-4e80-a726-0bf6b615df09");
        }
    }
}
