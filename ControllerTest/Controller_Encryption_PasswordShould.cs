using NUnit.Framework;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptography;

namespace ControllerTest
{
    public class Controller_Encryption_PasswordShould
    {
        [SetUp]
        public void Setup()
        {
        }
/*
        [Test]
        public static void PasswordShouldBeHashed()
        {
            string password = "Test123!";
            string salt = Encryption.CreateSalt();
            Assert.AreNotEqual(password, Encryption.TripleDes(Encryption.HashPassword(password, salt), false));
        }

        [Test]
        public static void PasswordShouldWithHashIsTheSameInVerifyHash()
        {
            string password = "Test123!";
            string salt = Encryption.CreateSalt();
            string hashedPassword = Encryption.HashPassword(password, salt);
            Assert.IsTrue(Encryption.VerifyHash(password, salt, hashedPassword));
        }

        [Test]
        public static void PasswordShouldWithHashIsNotTheSameInVerifyHash()
        {
            string password = "Test123!";
            string password2 = "!Test123";
            string salt = Encryption.CreateSalt();
            string hashedPassword = Encryption.HashPassword(password, salt);
            Assert.IsFalse(Encryption.VerifyHash(password2, salt, hashedPassword));
        }

        [Test]
        public static void PasswordShouldWithHashIsInVerifyHashFalseWithDifferentSalt()
        {
            string password = "Test123!";
            string salt = Encryption.CreateSalt();
            string salt2 = Encryption.CreateSalt();
            string hashedPassword = Encryption.HashPassword(password, salt);
            Assert.IsFalse(Encryption.VerifyHash(password, salt2, hashedPassword));
        }
*/
    }
}
