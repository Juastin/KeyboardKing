using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class TripleDES
    {
        public static string Encrypt(string source, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] data = Encoding.UTF8.GetBytes(source);
                    return Convert.ToBase64String(tripleDESCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        public static string Decrypt(string encrypt, string key)
        {
            using (TripleDESCryptoServiceProvider tripleDESCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDESCryptoService.Key = byteHash;
                    tripleDESCryptoService.Mode = CipherMode.ECB;
                    byte[] data = Convert.FromBase64String(encrypt);
                    return Encoding.UTF8.GetString(tripleDESCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
                }
            }
        }

        // Argon2
        //https://github.com/kmaragon/Konscious.Security.Cryptography
        public static byte[] HashPassword(string password, byte[] salt)
        {
            Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 16;
            argon2.Iterations = 40;
            argon2.MemorySize = 8192;
            return argon2.GetBytes(16);
        }

        public static bool VerifyHash(string loginPw, string salt, string userPw)
        {
            byte[] loginHashPw = HashPassword(loginPw, Convert.FromBase64String(salt));
            byte[] userHashPw = Convert.FromBase64String(userPw); 
            return loginHashPw.SequenceEqual(userHashPw);
        }

        public static byte[] CreateSalt()
        {
            byte[] buffer = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
    }
}
