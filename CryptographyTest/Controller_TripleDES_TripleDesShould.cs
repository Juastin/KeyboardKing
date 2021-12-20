using NUnit.Framework;
using Cryptography;

namespace CryptographyTest
{
    public class Controller_TripleDES_TripleDesShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public static void TripleDesShouldEncryptPassword()
        {
            string password = "test1";
            Assert.AreNotEqual(password, TripleDES.EncryptOrDecrypt(password, true));
            
        }

        [Test]
        public static void TripleDesShouldDecryptPassword()
        {
            string password = "test2";
            Assert.AreEqual(password, TripleDES.EncryptOrDecrypt(TripleDES.EncryptOrDecrypt(password, true), false));
        }

    }
}
