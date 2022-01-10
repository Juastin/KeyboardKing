using Model;
using NUnit.Framework;

namespace ModelTest
{
    public class Tests
    {
        private UList List { get; set; }

        [SetUp]
        public void Setup()
        {
            List = new UList(new object[]{13, "F en J", true, "7"});
        }

        [Test]
        public void Get_ShouldGet()
        {
            bool my_bool = List.Get<bool>(2);
            Assert.IsTrue(my_bool);
        }

        [Test]
        public void Get_ReturnsDefault()
        {
            bool my_bool = List.Get<bool>(1);
            Assert.IsFalse(my_bool);
        }

        [Test]
        public void Get_CanCastCompatibleTypes()
        {
            int my_string_int = List.Get<int>(3);
            Assert.IsTrue(my_string_int==7);
        }
    }
}