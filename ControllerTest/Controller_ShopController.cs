using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using System.Diagnostics.CodeAnalysis;

namespace ControllerTest
{
    [TestFixture]
    class Controller_ShopController
    {
        List<List<string>> Items = new List<List<string>>();

        [SetUp]
        public void SetUp()
        {
            // Create dummy data.
            Items = new List<List<string>> {
                new List<string> {"1", "KeyboardKing Light", "10", "Theme", "True" },
                new List<string> {"2", "KeyboardKing Dark", "20", "Theme", "True" },
                new List<string> {"3", "Space", "Theme", "20", "True" },
                new List<string> {"4", "Chinese", "50", "Theme", "True" },
                new List<string> {"5", "Paint", "100", "Theme", "False" },
                new List<string> {"6", "Obsidian", "250", "Theme", "True" },
                new List<string> {"7", "Hello Beertje", "500", "Theme", "False" },
                new List<string> {"8", "Christmas", "1000", "Theme", "False" },
                new List<string> {"9", "A Shelf on a shelf", "5000", "Theme", "False" },
                new List<string> {"10", "Crown", "5000", "Theme", "False" },
                new List<string> {"11", "A Coin", "10000", "Theme", "False" },
            };

            // Reset the controllers Allitems list.
            ShopController.CurrentPage = 1;
            ShopController.ItemsPerPage = 8;
            ShopController.AllItems = Item.ParseItems(Items);
            ShopController.MaxPage = ShopController.CalculateMaxPage();
        }

        [Test]
        public void GetPageItems_PageZero_ReturnCorrectItems()
        {
            ShopController.CurrentPage = 0;
            List<Item> result = ShopController.GetPageItems();

            List<Item> expectedResult = Item.ParseItems(new List<List<string>> {
                new List<string> {"1", "KeyboardKing Light", "10", "Theme", "True" },
                new List<string> {"2", "KeyboardKing Dark", "20", "Theme", "True" },
                new List<string> {"3", "Space", "50", "Theme", "True" },
                new List<string> {"4", "Chinese", "50", "Theme", "True" },
                new List<string> {"5", "Paint", "100", "Theme", "False" },
                new List<string> {"6", "Obsidian", "250", "Theme", "True" },
                new List<string> {"7", "Hello Beertje", "500", "Theme", "False" },
                new List<string> {"8", "Christmas", "1000", "Theme", "False" },
            });

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [Test]
        public void GetPageItems_CertainPage_ReturnCorrectItems()
        {
            List<Item> result = ShopController.GetPageItems(2);

            List<Item> expectedResult = Item.ParseItems(new List<List<string>> {
                new List<string> {"9", "A Shelf on a shelf", "/KeyBoardking;component/resources/images/shopping_shelf.png", "5000", "Theme", "False" },
                new List<string> {"10", "Crown", "/KeyBoardking;component/resources/images/icon.png", "5000", "Theme", "False" },
                new List<string> {"11", "A Coin", "/KeyBoardking;component/resources/images/icons/coin.png", "10000", "Theme", "False" },
            });

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [Test]
        public void GetPageItems_CertainPage2_ReturnCorrectItems()
        {
            ShopController.ItemsPerPage = 3;
            List<Item> result = ShopController.GetPageItems(3);

            List<Item> expectedResult = Item.ParseItems(new List<List<string>> {
                new List<string> {"7", "Hello Beertje", "/KeyBoardking;component/resources/images/hellobeertje_background_4k.png", "500", "Theme", "False" },
                new List<string> {"8", "Christmas", "/KeyBoardking;component/resources/images/kk_background_christmas.png", "1000", "Theme", "False" },
                new List<string> {"9", "A Shelf on a shelf", "/KeyBoardking;component/resources/images/shopping_shelf.png", "5000", "Theme", "False" },
            });

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [Test]
        public void ItemsPerPage_List_ReturnCorrectItems()
        {
            ShopController.CurrentPage = 0;
            ShopController.ItemsPerPage = 4;
            List<Item> result = ShopController.GetPageItems();

            List<Item> expectedResult = Item.ParseItems(new List<List<string>> {
                new List<string> {"1", "KeyboardKing Light", "10", "Theme", "True" },
                new List<string> {"2", "KeyboardKing Dark", "20", "Theme", "True" },
                new List<string> {"3", "Space", "50", "Theme", "True" },
                new List<string> {"4", "Chinese", "50", "Theme", "True" },
            });

            Assert.IsTrue(result.SequenceEqual(expectedResult));
        }

        [Test]
        public void SetCurrentItem_CurrentItem_ReturnCorrectItem()
        {
            Item item = Item.ParseItem(new List<string> { "1", "KeyboardKing Light", "/KeyBoardking;component/resources/images/kk_background_4K.png", "10", "Theme", "True" });
            ShopController.SetCurrentItem(item);
            Assert.AreEqual(item, ShopController.CurrentItem);
        }

        [TestCase(1, 11)]
        [TestCase(2, 6)]
        [TestCase(3, 4)]
        [TestCase(4, 3)]
        [TestCase(8, 2)]
        [TestCase(11, 1)]
        [TestCase(20, 1)]
        public void CalculateMaxPage_MaxPage_ReturnCorrectInt(int itemsPerPage, int expectedMaxPage)
        {
            ShopController.ItemsPerPage = itemsPerPage;
            Assert.AreEqual(expectedMaxPage, ShopController.CalculateMaxPage());
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(-1, 1)]
        [TestCase(-2, 1)]
        [TestCase(-3, 1)]
        public void UpdatePage_CurrentPage_ReturnCorrectInt(int page, int expectedCurrentPage)
        {
            ShopController.CurrentPage += page;
            Assert.AreEqual(expectedCurrentPage, ShopController.CurrentPage);
        }

    }
}
