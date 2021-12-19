using NUnit.Framework;
using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    public class Controller_Encryption_TripleDesShould
    {
        [SetUp]
        public void Setup()
        {
        }
/*
        [Test]
        public static void TripleDesShouldEncryptPassword()
        {
            string password = "test1";
            Assert.AreNotEqual(password, Encryption.TripleDes(password, true));
            
        }

        [Test]
        public static void TripleDesShouldDecryptPassword()
        {
            string password = "test2";
            Assert.AreEqual(password, Encryption.TripleDes(Encryption.TripleDes(password, true), false));
        }
*/
    }
}
