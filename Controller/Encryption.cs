using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class Encryption
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

        private static string TripleDes(string source, bool encrypt)
        {
            return encrypt ? Encrypt(source, "332cc6da-d757-4e80-a726-0bf6b615df09") : Decrypt(source, "332cc6da-d757-4e80-a726-0bf6b615df09");
        }

        // Argon2
        //https://github.com/kmaragon/Konscious.Security.Cryptography
        public static string HashPassword(string password, string salt)
        {
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(password));
            argon2.Salt = Convert.FromBase64String(TripleDes(salt, false));
            argon2.DegreeOfParallelism = 16;
            argon2.Iterations = 40;
            argon2.MemorySize = 8192;
            return TripleDes(Convert.ToBase64String(argon2.GetBytes(16)), true);
        }

        public static bool VerifyHash(string loginPw, string salt, string userPw)
        {
            byte[] loginHashPw = Convert.FromBase64String(TripleDes(HashPassword(loginPw, salt), false));
            byte[] userHashPw = Convert.FromBase64String(TripleDes(userPw, false));
            return loginHashPw.SequenceEqual(userHashPw);
        }

        public static string CreateSalt()
        {
            byte[] salt = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return TripleDes(Convert.ToBase64String(salt), true);
        }

    }
}
