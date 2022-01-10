using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Session
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Add_Overwrite()
        {
            Session.Add("my_bool", true);
            Session.Add("my_bool", false);

            bool? result = (bool?)Session.Get("my_bool");

            Assert.IsFalse(result);
        }

        [Test]
        public void Get_ReturnCorrectItem()
        {
            Session.Add("my_bool", true);
            bool? result = (bool?)Session.Get("my_bool");

            Assert.IsTrue(result);
        }

        [Test]
        public void Get_ItemThatDoesNotExist()
        {
            bool? result = (bool?)Session.Get("something_that_does_not_exist");

            Assert.IsNull(result);
        }

        [Test]
        public void Flush_RemovesAllItems()
        {
            Session.Add("my_bool", true);
            Session.Add("my_string", "13");
            Session.Add("my_array", new []{0,1,2});
            Session.Flush();

            bool result_a = (bool?)Session.Get("my_bool") is null;
            bool result_b = (string?)Session.Get("my_string") is null;
            bool result_c = (int[]?)Session.Get("my_array") is null;

            Assert.IsTrue( (result_a) && (result_b) && (result_c) );
        }
    }
}