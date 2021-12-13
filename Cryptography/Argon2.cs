﻿using Konscious.Security.Cryptography;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    public static class Argon2
    {
        // Argon2
        //https://github.com/kmaragon/Konscious.Security.Cryptography
        public static string HashPassword(string password, string salt)
        {
            Argon2id argon2 = new(Encoding.UTF8.GetBytes(password));
            argon2.Salt = Convert.FromBase64String(TripleDES.EncryptOrDecrypt(salt, false));
            argon2.DegreeOfParallelism = 16;
            argon2.Iterations = 40;
            argon2.MemorySize = 8192;
            return TripleDES.EncryptOrDecrypt(Convert.ToBase64String(argon2.GetBytes(16)), true);
        }

        public static bool VerifyHash(string loginPw, string salt, string userPw)
        {
            byte[] loginHashPw = Convert.FromBase64String(TripleDES.EncryptOrDecrypt(HashPassword(loginPw, salt), false));
            byte[] userHashPw = Convert.FromBase64String(TripleDES.EncryptOrDecrypt(userPw, false));
            return loginHashPw.SequenceEqual(userHashPw);
        }

        public static string CreateSalt()
        {
            byte[] salt = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return TripleDES.EncryptOrDecrypt(Convert.ToBase64String(salt), true);
        }

    }
}