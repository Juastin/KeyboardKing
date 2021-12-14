using NUnit.Framework;
using Controller;
using Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    public class Controller_Encryption_PasswordShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public static void PasswordShouldBeHashed()
        {
            string password = "Test123!";
            string salt = Argon2.CreateSalt();
            Assert.AreNotEqual(password, TripleDES.EncryptOrDecrypt(Argon2.HashPassword(password, salt), false));
        }

        [Test]
        public static void PasswordShouldWithHashIsTheSameInVerifyHash()
        {
            string password = "Test123!";
            string salt = Argon2.CreateSalt();
            string hashedPassword = Argon2.HashPassword(password, salt);
            Assert.IsTrue(Argon2.VerifyHash(password, salt, hashedPassword));
        }

        [Test]
        public static void PasswordShouldWithHashIsNotTheSameInVerifyHash()
        {
            string password = "Test123!";
            string password2 = "!Test123";
            string salt = Argon2.CreateSalt();
            string hashedPassword = Argon2.HashPassword(password, salt);
            Assert.IsFalse(Argon2.VerifyHash(password2, salt, hashedPassword));
        }

        [Test]
        public static void PasswordShouldWithHashIsInVerifyHashFalseWithDifferentSalt()
        {
            string password = "Test123!";
            string salt = Argon2.CreateSalt();
            string salt2 = Argon2.CreateSalt();
            string hashedPassword = Argon2.HashPassword(password, salt);
            Assert.IsFalse(Argon2.VerifyHash(password, salt2, hashedPassword));
        }

    }
}
